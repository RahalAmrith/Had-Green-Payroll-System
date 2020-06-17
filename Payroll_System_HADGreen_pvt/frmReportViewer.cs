using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Payroll_System_HADGreen_pvt
{
    public partial class frmReporViewer : Form
    {
        DataTable data;
        string mainTitle;

        string[] upperTitles;
        string[] lowerTitles;

        public frmReporViewer()
        {
            InitializeComponent();
        }

        public frmReporViewer(DataTable data, string mainTitle, string[] upperTitles, string[] lowerTitles)
        {
            InitializeComponent();

            this.data = data;
            this.mainTitle = mainTitle;
            this.upperTitles = upperTitles;
            this.lowerTitles = lowerTitles;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmReporViewer_Load(object sender, EventArgs e)
        {
            dgvRes.DataSource = data;
            this.lblMainTitle.Text = this.mainTitle;

            this.lblUpperTitle.Text = "";
            foreach (string ttl in upperTitles)
            {
                this.lblUpperTitle.Text += ttl + "\n";

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            printer prnt = new printer(mainTitle, upperTitles, dgvRes, lowerTitles);
            prnt.print();
        }
    }
}
