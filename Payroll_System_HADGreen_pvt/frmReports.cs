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
    public partial class frmReports : Form
    {
        SqlConnection con = new SqlConnection("server=localhost; Trusted_Connection=yes; database=hadGreenPayroll;");


        public frmReports()
        {
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            // pnlContent.Hide();

            con.Open();

            SqlCommand cmd = new SqlCommand("select empName from employee", con);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                txtR1EName.AutoCompleteCustomSource.Add(reader[0].ToString());
            }

            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select workedDate as 'Worked Date', inTime as 'In Time', outTime as 'Out Time', workedHours as 'Worked Hours', bSal as 'Basic Salary', advance as 'Advance', deduction as 'Deduction', netSal as 'Net Salary' from payments where empId = (select empId from employee where empName = @name) and workedDate >= @startDate and workedDate <= @endDate", con);

                cmd.Parameters.Add(new SqlParameter("name", txtR1EName.Text));
                cmd.Parameters.Add(new SqlParameter("startDate", dtpR1StartDate.Value.ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("endDate", dtpR1EndDate.Value.ToString("yyyy-MM-dd")));

                SqlDataAdapter result = new SqlDataAdapter(cmd);

                DataTable data = new DataTable();

                result.Fill(data);

                // titles

                string mTitle = "Report by Employee";

                string[] uTitles = new string[3];
                uTitles[0] = "Employee Name : " + txtR1EName.Text;
                uTitles[1] = "Start Date    : " + dtpR1StartDate.Value.ToString("yyyy-MM-dd");
                uTitles[2] = "End Date      : " + dtpR1EndDate.Value.ToString("yyyy-MM-dd");

                string[] lTitles = new string[0];

                frmReporViewer rep = new frmReporViewer(data, mTitle, uTitles, lTitles);
                rep.Show();

                

                con.Close();
            }

            catch
            {

            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                string query = txtCQuery.Text;

                SqlCommand cmd = new SqlCommand(query , con);

                SqlDataAdapter result = new SqlDataAdapter(cmd);

                DataTable data = new DataTable();

                result.Fill(data);

                // titles

                string mTitle = "Custom Report";

                string[] uTitles = new string[0];

                string[] lTitles = new string[0];

                frmReporViewer rep = new frmReporViewer(data, mTitle, uTitles, lTitles);
                rep.Show();



                con.Close();
            }

            catch
            {

            }
        }
    }
}
