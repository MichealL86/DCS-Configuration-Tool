using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DCS_Configuration_Tool
{
    public partial class Progress_Form : Form
    {


        public Progress_Form()
        {
            InitializeComponent();


        }

        private void Progress_Form_Load(object sender, EventArgs e)
        {

        }

        public void progressBar1_Click(object sender, EventArgs e)
        {

        }

        public string LabelText
        {
            get
            {
                return this.resultLabel.Text;
            }
            set
            {
                this.resultLabel.Text = value;
            }
        }





    }
}
