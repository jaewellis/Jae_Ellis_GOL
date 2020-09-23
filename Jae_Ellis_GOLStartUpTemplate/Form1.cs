using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jae_Ellis_GOLStartUpTemplate
{
    public partial class Form1 : Form
    {
        // Set defualts
        int uWidth = (int)Properties.Settings.Default.UniverseWidth;
        int uHeight = (int)Properties.Settings.Default.UniverseHeight;
        decimal interval = Properties.Settings.Default.TimerInterval;
        // The universe array
        bool[,] universe = new bool[5, 5];
        bool showNeighborCount = false;
        // Drawing colors
        bool gridActive = Properties.Settings.Default.GridActive;
        Color gridColor = Properties.Settings.Default.GridColor;
        Color cellColor = Properties.Settings.Default.CellColor;

       
        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;
        int aliveCells = 0;

        // Create random
        Random rand = new Random(DateTime.Now.Second);

        //Hud
        bool hudIsActive = Properties.Settings.Default.HudIsActive;
        HUD hud = new HUD();


        public Form1()
        {
            InitializeComponent();

            graphicsPanel1.BackColor = Properties.Settings.Default.FormBackColor;

            RandomStart(universe, rand);
            // Setup the timer
            timer.Interval = (int)interval; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = true; // start timer running
            
            // Set Up Hud
            hud.Generation = generations.ToString();
            hud.AliveCells = aliveCells.ToString();
            hud.UniverseWidth = uWidth.ToString();
            hud.UniverseHeight = uHeight.ToString();
            if (hudIsActive)
            {
                hud.Visible = true;
            }
            else
            {
                hud.Visible = false;
            }
        }



        // Calculate the next generation of cells
        private void NextGeneration()
        {


            bool[,] scratchPad = new bool[uWidth, uHeight];


            // Loop current universe
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int neighbors = CountNeighbors(universe, x, y);
                    if (universe[x, y] == true) // Living cells
                    {
                        if (neighbors < 2 || neighbors > 3) //Less then 2, more then 3 dies
                        {
                            scratchPad[x, y] = false;
                        } else // range 2 or 3
                        {
                            scratchPad[x, y] = true;

                        }
                    } else // assume false
                    {
                        if (neighbors == 3)
                        {
                            scratchPad[x, y] = true;

                        }
                    }
                }
            }

            universe = scratchPad;
            graphicsPanel1.Invalidate();


            // Increment generation count
            generations++;
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            hud.Generation = generations.ToString();

        }

        private int CountNeighbors(bool[,] universe, int x, int y)
        {
            // set world bounds
            int width = universe.GetLength(0) - 1;
            int height = universe.GetLength(1) - 1;
            int numOfAlive = 0;

            // get all neighbors, setting out of world to false
            bool topLeft = (x <= 0 || y <= 0) ? false : universe[x - 1, y - 1];
            bool top = (y <= 0) ? false : universe[x, y - 1];
            bool topRight = (x >= width || y <= 0) ? false : universe[x + 1, y - 1];
            bool left = (x <= 0) ? false : universe[x - 1, y];
            bool right = (x >= width) ? false : universe[x + 1, y] ;
            bool bottomLeft = (x <= 0 || y >= height) ? false : universe[x - 1, y + 1];
            bool bottomRight = (x >= width || y >= height) ? false : universe[x + 1, y + 1];
            bool bottom = (y >= height) ? false : universe[x, y + 1];

            // collect all neighbors
            bool[] neighbors = new bool[]
            {
                top, topLeft, topRight, left,right,bottom, bottomLeft, bottomRight
            };

            // count alive neighbors
            foreach (var neighbor in neighbors)
            {
                if (neighbor == true)
                {
                    numOfAlive++;
                }
            }

            return numOfAlive;
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void Play_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (timer.Enabled == false)
            {
                NextGeneration();
            }
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);
            if (!gridActive)
            {
                gridPen.Color = Color.Transparent;
            }
            
             

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = (float)x * cellWidth;
                    cellRect.Y = (float)y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    if (showNeighborCount)
                    {
                         e.Graphics.DrawString(CountNeighbors(universe, x, y).ToString(), graphicsPanel1.Font, Brushes.Black, cellRect.Location);
                    }
                    
                    aliveCells = GetNumOfAlive(universe);
                    hud.AliveCells = aliveCells.ToString();
                    toolStripStatusAlive.Text = "Alive Cells = " + aliveCells.ToString();
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
                // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = (float)e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = (float)e.Y / cellHeight;

                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Properties.Resources.AppTitle;


        }

        //count num of alive cells in univers
        private static int GetNumOfAlive(bool[,] universe)
        {
            int count = 0;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x, y] == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }




        // Main Menu Strip Click events
        private void randomToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            clearUniverse(universe);
            RandomStart(universe, rand);
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = 0";
            graphicsPanel1.Invalidate();
        }
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clearUniverse(universe);
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = 0";
            graphicsPanel1.Invalidate();
        }
        private void blankToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = 0";
            toolStripStatusAlive.Text = "Alive Cells = 0";
            graphicsPanel1.Invalidate();
        }
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Clears univers
        static private void clearUniverse(bool[,] universe)
        {
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
        }

        // Creates a Random universe
        static private void RandomStart(bool[,] universe, Random rand)
        {
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int randNum = rand.Next(0, 2);
                    if (randNum == 0)
                    {
                        universe[x, y] = true;
                    }
                    else
                    {
                        universe[x, y] = false;
                    }

                }
            }
        }





        // Save
        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!This is my comment.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
     {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
          {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x,y] == true)
                        {
                            currentRow += "O";
                        } else
                        {
                            currentRow += ".";
                        }
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
        // Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (!row.Contains("!"))
                    {
                        maxHeight++;
                        maxWidth = row.Length;
                    }
                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universe = new bool[(uWidth = maxWidth), (uHeight = maxHeight)];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                int rowCounter = 0;
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    if (!row.Contains("!"))
                    {
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos] == 'O')
                            {
                                universe[xPos, rowCounter] = true;
                            }
                            else
                            {
                                universe[xPos, rowCounter] = false;
                            }

                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                        }
                        rowCounter++;
                    }
                    

                }

                // Close the file.
                reader.Close();
                graphicsPanel1.Invalidate();
            }
        }


        // Save settings on close
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.FormBackColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridActive = gridActive;
            Properties.Settings.Default.TimerInterval = interval;
            Properties.Settings.Default.UniverseWidth = uWidth;
            Properties.Settings.Default.UniverseHeight = uHeight;
            Properties.Settings.Default.HudIsActive = hudIsActive;
            Properties.Settings.Default.Save();
        }

        // Grid View
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gridActive = !gridActive;
        }
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
        }

        // BackGround Color
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backGroundColorToolStripMenuItem_Click(sender, e);
        }
        private void backGroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }

        // Cell Color
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = cellColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        private void cellColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cellColorToolStripMenuItem_Click(sender, e);
        }

        // Timer
        private void setTimerIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            IntervalForm sf = new IntervalForm();
            sf.Interval = Properties.Settings.Default.TimerInterval;

            if (DialogResult.OK == sf.ShowDialog())
            {
                Properties.Settings.Default.TimerInterval = sf.Interval;
            }
            timer.Enabled = true;
        }
        private void setTimerIntervalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setTimerIntervalToolStripMenuItem_Click(sender, e);
        }

        //Defualt
        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            interval = 100;
        }
        private void defaultToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            defaultToolStripMenuItem_Click(sender, e);
        }


        // Toggle NeighborCount
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNeighborCount = !showNeighborCount;
            graphicsPanel1.Invalidate();
        }
        private void neigborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            neighborCountToolStripMenuItem_Click(sender, e);
        }

        // World Size
        private void worldSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            UniverseSizeForm usf = new UniverseSizeForm();
            usf.UniverseWidth = uWidth;
            usf.UniverseHeight = uHeight;

            if (DialogResult.OK == usf.ShowDialog())
            {
                uWidth = (int)usf.UniverseWidth;
                uHeight = (int)usf.UniverseHeight;
            }
            newToolStripMenuItem1_Click(sender, e);
            timer.Enabled = true;

        }
        private void worldSizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            worldSizeToolStripMenuItem_Click(sender, e);
        }

        // Hud Toggle
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hudIsActive = !hudIsActive;
            if (hudIsActive)
            {
                hud.Visible = true;
            } else
            {
                hud.Visible = false;
            }
        }
        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem2_Click(sender, e);
        }
    }
}
