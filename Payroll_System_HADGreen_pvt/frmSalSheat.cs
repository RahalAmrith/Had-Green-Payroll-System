using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Payroll_System_HADGreen_pvt
{
    public partial class frmSalSheat : Form
    {
        SqlConnection con = new SqlConnection("server=localhost\\SQLEXPRESS; Trusted_Connection=yes; database=hadGreenPayroll;");

        string src;

        public frmSalSheat()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select p.empId as 'ID', e.empName as 'Name', e.hRate as 'Hourly Rate', count(p.workedHours) as 'Total Worked Days',sum(p.workedHours) as 'Total Hours', sum(p.netSal) as 'Total Salary' from payments p, employee e where p.workedDate >= @startDate and p.workedDate <= @endDate and e.empId = p.empId group by p.empId, e.empName, e.hRate", con);
                cmd.Parameters.Add(new SqlParameter("startDate", dtpStartDate.Value.ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("endDate", dtpEndDate.Value.ToString("yyyy-MM-dd")));

                SqlDataAdapter result = new SqlDataAdapter(cmd);

                DataTable data = new DataTable();

                result.Fill(data);

                dgvRes.DataSource = data;
                dgvRes.Columns[0].Width = 30;
                dgvRes.Columns[1].Width = 150;

                con.Close();
                cmd.Dispose();

                // calculate total

                con.Open();

                SqlCommand cmd2 = new SqlCommand("select sum(netSal) as 'Total' from payments where workedDate >= @startDate and workedDate <= @endDate", con);
                cmd2.Parameters.Add(new SqlParameter("startDate", dtpStartDate.Value.ToString("yyyy-MM-dd")));
                cmd2.Parameters.Add(new SqlParameter("endDate", dtpEndDate.Value.ToString("yyyy-MM-dd")));

                SqlDataReader reader = cmd2.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    txtTAmount.Text = reader[0].ToString();
                }

                con.Close();

            }

            catch(Exception ex)
            {
                MessageBox.Show("Error Occurred While connecting to the database.\n\n" + ex.ToString(),"Error.", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        // print button
        private void button1_Click(object sender, EventArgs e)
        {
            string mTitle = "Salary Sheate";
            string[] uTitles = new string[1];
            string[] lTitles = new string[1];

            uTitles[0] = dtpStartDate.Value.ToString("dd MMMM yyyy (dddd)") +
                         " - " +
                         dtpEndDate.Value.ToString("dd MMMM yyyy (dddd)");

            lTitles[0] = "Total Amount : Rs. " + this.txtTAmount.Text ;


            printer prnt = new printer(mTitle, uTitles, dgvRes, lTitles);
            prnt.print();
            
        }

        private void frmSalSheat_Load(object sender, EventArgs e)
        {

        }
    }
}
