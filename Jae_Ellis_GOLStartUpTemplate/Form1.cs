using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jae_Ellis_GOLStartUpTemplate
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[5, 5];

        

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        // Create random
        Random rand = new Random(DateTime.Now.Second);

        public Form1()
        {
            InitializeComponent();

            RandomStart(universe, rand);
            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = true; // start timer running
        }



        // Calculate the next generation of cells
        private void NextGeneration()
        {
            // Generate the scratch Pad
            bool[,] scratchPad = new bool[5, 5];

            // Loop current universe
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int neighbors = CountNeighbors(universe, x, y);
                    if (universe[x, y] == true)
                    {
                        if (neighbors > 2 || neighbors < 3)
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
                    e.Graphics.DrawString(CountNeighbors(universe, x, y).ToString(), graphicsPanel1.Font, Brushes.Black, cellRect.Location);
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

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            clearUniverse(universe);
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = 0";
            graphicsPanel1.Invalidate();
        }


        private void toolStripStatusLabelGenerations_Click(object sender, EventArgs e)
        {
            
        }

        private void blankToolStripMenuItem_Click(object sender, EventArgs e)
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
            graphicsPanel1.Invalidate();
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearUniverse(universe);
            RandomStart(universe, rand);
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = 0";
            graphicsPanel1.Invalidate();
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
    }
}
