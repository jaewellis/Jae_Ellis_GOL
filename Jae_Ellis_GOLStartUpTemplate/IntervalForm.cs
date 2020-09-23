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
    public partial class IntervalForm : Form
    {
        public IntervalForm()
        {
            InitializeComponent();
        }

        public decimal Interval
        {
            get
            {
                return numericUpDown1.Value;
            }
            set
            {
                numericUpDown1.Value = value;
            }
        }

        private void IntervalForm_Load(object sender, EventArgs e)
        {

        }
    }
}
