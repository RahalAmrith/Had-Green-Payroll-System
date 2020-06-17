using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Payroll_System_HADGreen_pvt
{
    public partial class mainFrm : Form
    {
        public mainFrm()
        {
            InitializeComponent();
        }

        private void main_Load(object sender, EventArgs e)
        {

            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;

            Boolean cs = false;

            SqlConnection con = new SqlConnection("server=localhost; Trusted_Connection=yes; database=hadGreenPayroll;");

            while(!cs)
            {
                try
                {
                    con.Open();

                    lblCS.Text = "Connected";
                    lblCS.ForeColor = Color.Green;

                    btn1.Enabled = true;
                    btn2.Enabled = true;
                    btn3.Enabled = true;
                    btn4.Enabled = true;

                    cs = true;

                    con.Close();

                }

                catch(SqlException ex)
                {
                    // do nothing
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmSalSheat ss = new frmSalSheat();
            ss.Show();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            frmEmpMgmt fem = new frmEmpMgmt();
            fem.Show();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            InpData id = new InpData();
            id.Show();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            frmReports rep = new frmReports();
            rep.Show();

            // MessageBox.Show("This Feature is currently not available...", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
