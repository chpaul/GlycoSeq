using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GlycanSeq_Form
{
    public partial class frmStart : Form
    {
        public frmStart()
        {
            InitializeComponent();
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            frmSingle sig = new frmSingle();
            this.Visible = false;
            sig.ShowDialog();
            this.Visible = true;
            //Application.Exit();
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            frmBatch batch = new frmBatch();
            this.Visible = false;
            batch.ShowDialog();
            this.Visible = true;

            //Application.Exit();
        }
    }
}
