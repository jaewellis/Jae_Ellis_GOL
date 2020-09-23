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
    public partial class HUD : Form
    {
        public HUD()
        {
            InitializeComponent();
        }

        public string Generation
        {
            get
            {
                return tbGen.Text;
            }
            set
            {
                tbGen.Text = value;
            }
        }

        public string AliveCells
        {
            get
            {
                return tbAlive.Text;
            }
            set
            {
                tbAlive.Text = value;
            }
        }

        public string UniverseWidth
        {
            get
            {
                return tbUW.Text;
            }
            set
            {
                tbUW.Text = value;
            }
        }

        public string UniverseHeight
        {
            get
            {
                return tbUH.Text;
            }
            set
            {
                tbUH.Text = value;
            }
        }

        private void HUD_Load(object sender, EventArgs e)
        {

        }
    }
}
