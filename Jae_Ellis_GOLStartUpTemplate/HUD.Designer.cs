namespace Jae_Ellis_GOLStartUpTemplate
{
    partial class HUD
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelGenCount = new System.Windows.Forms.Label();
            this.labelCellCount = new System.Windows.Forms.Label();
            this.labelUW = new System.Windows.Forms.Label();
            this.tbUW = new System.Windows.Forms.TextBox();
            this.tbAlive = new System.Windows.Forms.TextBox();
            this.tbGen = new System.Windows.Forms.TextBox();
            this.tbUH = new System.Windows.Forms.TextBox();
            this.labelUH = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelGenCount
            // 
            this.labelGenCount.AutoSize = true;
            this.labelGenCount.Location = new System.Drawing.Point(28, 178);
            this.labelGenCount.Name = "labelGenCount";
            this.labelGenCount.Size = new System.Drawing.Size(59, 13);
            this.labelGenCount.TabIndex = 0;
            this.labelGenCount.Text = "Generation";
            // 
            // labelCellCount
            // 
            this.labelCellCount.AutoSize = true;
            this.labelCellCount.Location = new System.Drawing.Point(28, 213);
            this.labelCellCount.Name = "labelCellCount";
            this.labelCellCount.Size = new System.Drawing.Size(55, 13);
            this.labelCellCount.TabIndex = 1;
            this.labelCellCount.Text = "Alive Cells";
            // 
            // labelUW
            // 
            this.labelUW.AutoSize = true;
            this.labelUW.Location = new System.Drawing.Point(27, 248);
            this.labelUW.Name = "labelUW";
            this.labelUW.Size = new System.Drawing.Size(80, 13);
            this.labelUW.TabIndex = 2;
            this.labelUW.Text = "Universe Width";
            // 
            // tbUW
            // 
            this.tbUW.Location = new System.Drawing.Point(113, 244);
            this.tbUW.Name = "tbUW";
            this.tbUW.ReadOnly = true;
            this.tbUW.Size = new System.Drawing.Size(100, 20);
            this.tbUW.TabIndex = 3;
            // 
            // tbAlive
            // 
            this.tbAlive.Location = new System.Drawing.Point(113, 209);
            this.tbAlive.Name = "tbAlive";
            this.tbAlive.ReadOnly = true;
            this.tbAlive.Size = new System.Drawing.Size(100, 20);
            this.tbAlive.TabIndex = 4;
            // 
            // tbGen
            // 
            this.tbGen.Location = new System.Drawing.Point(113, 174);
            this.tbGen.Name = "tbGen";
            this.tbGen.ReadOnly = true;
            this.tbGen.Size = new System.Drawing.Size(100, 20);
            this.tbGen.TabIndex = 5;
            // 
            // tbUH
            // 
            this.tbUH.Location = new System.Drawing.Point(113, 279);
            this.tbUH.Name = "tbUH";
            this.tbUH.ReadOnly = true;
            this.tbUH.Size = new System.Drawing.Size(100, 20);
            this.tbUH.TabIndex = 7;
            // 
            // labelUH
            // 
            this.labelUH.AutoSize = true;
            this.labelUH.Location = new System.Drawing.Point(27, 283);
            this.labelUH.Name = "labelUH";
            this.labelUH.Size = new System.Drawing.Size(83, 13);
            this.labelUH.TabIndex = 6;
            this.labelUH.Text = "Universe Height";
            // 
            // HUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 473);
            this.Controls.Add(this.tbUH);
            this.Controls.Add(this.labelUH);
            this.Controls.Add(this.tbGen);
            this.Controls.Add(this.tbAlive);
            this.Controls.Add(this.tbUW);
            this.Controls.Add(this.labelUW);
            this.Controls.Add(this.labelCellCount);
            this.Controls.Add(this.labelGenCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = new System.Drawing.Point(1800, 500);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HUD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "HUD";
            this.Load += new System.EventHandler(this.HUD_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGenCount;
        private System.Windows.Forms.Label labelCellCount;
        private System.Windows.Forms.Label labelUW;
        private System.Windows.Forms.TextBox tbUW;
        private System.Windows.Forms.TextBox tbAlive;
        private System.Windows.Forms.TextBox tbGen;
        private System.Windows.Forms.TextBox tbUH;
        private System.Windows.Forms.Label labelUH;
    }
}