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
    public partial class InpData : Form
    {
        SqlConnection con = new SqlConnection("server=localhost; Trusted_Connection=yes; database=hadGreenPayroll;");

        public InpData()
        {
            InitializeComponent();
        }

        private void InpData_Load(object sender, EventArgs e)
        {
            txtToday.Text = DateTime.Today.ToString("dd - MMMM - yyyy");

            // name suggestion
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select empName from employee", con);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    txtEName.AutoCompleteCustomSource.Add(reader[0].ToString());
                }

                con.Close();
            }

            catch
            {
                MessageBox.Show("Error occurred while loading employee list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtEName_TextChanged(object sender, EventArgs e)
        {
            string name = txtEName.Text;

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select * from employee where empName = @name", con);
                cmd.Parameters.Add(new SqlParameter("name", name));

                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.HasRows)
                {
                    reader.Read();

                    txtEId.Text = reader[0].ToString();
                    txtHRate.Text = reader[2].ToString();
                }

                con.Close();
            }

            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void btnRESrch_Click(object sender, EventArgs e)
        {
            bool valid = false;

            string id = txtEId.Text;

            DateTime wDate = this.dtpWDate.Value;
            DateTime inTime = this.dtpInTime.Value;
            DateTime outTime = this.dtpOutTime.Value;
            DateTime deadLine = this.dtpDeadLine.Value;

            float hourlyRate = 0;
            float advance = 0;
            float deduction = 0;
            float OTAdd = 0;

            try
            {
                hourlyRate = float.Parse(txtHRate.Text);
                advance = float.Parse(txtAdvance.Text);
                deduction = float.Parse(txtDeduction.Text);
                OTAdd = float.Parse(txtOTAdd.Text);

                valid = true;
            }

            catch
            {
                MessageBox.Show("Values are invalied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(valid)
            {
                Payment pmt = new Payment(id, wDate, inTime, outTime, hourlyRate, advance, deduction, deadLine, OTAdd);

                bool res = pmt.saveData();

                if(res)
                {
                    MessageBox.Show("Record Saved Successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    MessageBox.Show("Record not Saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // clear text boxes
        private void button1_Click(object sender, EventArgs e)
        {
            var sortedTextboxes = panel2.Controls.OfType<TextBox>();
            foreach ( TextBox tb in sortedTextboxes)
            {
                tb.Text = "";
            }
        }

        // search record
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select e.empId as 'ID', e.empName as 'Name', p.workedDate as 'Worked Date', p.bSal as 'Basic Salary', p.advance, p.deduction, p.netSal as 'Net Salary' from employee e, payments p where e.empId = p.empId and workedDate >= @startDate and workedDate <= @endDate", con);
                cmd.Parameters.Add(new SqlParameter("startDate", dtpStartDate.Value.ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("endDate", dtpEndDate.Value.ToString("yyyy-MM-dd")));

                SqlDataAdapter result = new SqlDataAdapter(cmd);

                DataTable data = new DataTable();

                result.Fill(data);

                dgvRes.DataSource = data;
                dgvRes.Columns[0].Width = 50;
                dgvRes.Columns[1].Width = 150;

                con.Close();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // delete record
        private void button2_Click(object sender, EventArgs e)
        {
            string id = dgvRes.CurrentRow.Cells[0].Value.ToString();
            string name = dgvRes.CurrentRow.Cells[1].Value.ToString();
            string date = dgvRes.CurrentRow.Cells[2].Value.ToString();

            DialogResult dr = MessageBox.Show("Are You Sure do you want to delet..\n\n " + name + "\n\n" + date, "Delete record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("delete from payments where empId = @id and workedDate = @date ", con);
                    cmd.Parameters.Add(new SqlParameter("id", id));
                    cmd.Parameters.Add(new SqlParameter("date", date));

                    cmd.ExecuteNonQuery();

                    con.Close();

                    MessageBox.Show("Employee deleted Successfully", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Can't delete Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
