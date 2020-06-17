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
    public partial class frmEmpMgmt : Form
    {
        SqlConnection con = new SqlConnection("server=localhost; Trusted_Connection=yes; database=hadGreenPayroll;");

        public frmEmpMgmt()
        {
            InitializeComponent();
        }

        private void frmEmpMgmt_Load(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("select empName from employee", con);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                txtREName.AutoCompleteCustomSource.Add(reader[0].ToString());
            }

            con.Close();

            fillEmpGrid();

        }

        private void btnAEAdd_Click(object sender, EventArgs e)
        {
            bool valid = false;

            string eName = txtAEName.Text;
            float bSal = 0;

            // validate data
            try
            {
                bSal = float.Parse(txtAEBSal.Text);
                valid = true;
            }

            catch(Exception ex)
            {
                MessageBox.Show("Please Entet Valid Value For Basic Salary. \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

            if(valid)
            {
                // check for epmloyee exist
                con.Open();

                SqlCommand cmd = new SqlCommand("select * from employee where empName=@name", con);
                cmd.Parameters.Add(new SqlParameter("Name", eName));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    MessageBox.Show("Employee Alredy Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }

                else
                {
                    // add employye to the data base
                    con.Close();
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("insert into employee(empName, hRate) values(@name, @bSal)", con);
                    cmd2.Parameters.Add(new SqlParameter("Name", eName));
                    cmd2.Parameters.Add(new SqlParameter("bSal", bSal));

                    cmd2.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Employee Added Successfully", "Done",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }  
        }

        private void fillEmpGrid()
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select empId as 'ID', empName as 'Name', hRate as 'Hourly Rate' from employee", con);

                SqlDataAdapter result = new SqlDataAdapter(cmd);

                DataTable data = new DataTable();

                result.Fill(data);

                dgvEmployee.DataSource = data;
                dgvEmployee.Columns[1].Width = 400;
                dgvEmployee.Columns[2].Width = 150;

                con.Close();
            }

            catch(Exception ex)
            {
                MessageBox.Show("Error occurred while loading data \n\n" + ex.ToString(), "Error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillEmpGrid();
        }

        private void btnRESrch_Click(object sender, EventArgs e)
        {
            string keyWord = txtREName.Text;
            con.Open();

            SqlCommand cmd = new SqlCommand("select empId as 'ID', empName as 'Name', hRate as 'Hourly Rate' from employee where empName like '%" + keyWord + "%'", con);

            SqlDataAdapter result = new SqlDataAdapter(cmd);

            DataTable data = new DataTable();

            result.Fill(data);

            dgvSearchRes.DataSource = data;
            dgvSearchRes.Columns[1].Width = 400;
            dgvSearchRes.Columns[2].Width = 150;

            con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = dgvSearchRes.CurrentRow.Cells[0].Value.ToString();
            string name = dgvSearchRes.CurrentRow.Cells[1].Value.ToString();

            DialogResult dr = MessageBox.Show("Are You Sure do you want to delet " + name, "Delete Employee", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(dr == DialogResult.Yes)
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("delete from employee where empId = " +id, con);

                    cmd.ExecuteNonQuery();

                    con.Close();

                    MessageBox.Show("Employee deleted Successfully", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Can't delete Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // update employee
        private void button2_Click(object sender, EventArgs e)
        {
            bool valid = false;

            string id = ""; // = dgvSearchRes.CurrentRow.Cells[0].Value.ToString();
            string name = ""; // = dgvSearchRes.CurrentRow.Cells[1].Value.ToString();

            string upName = txtUpName.Text;
            float upRate = 0;

            try
            {
                id = dgvSearchRes.CurrentRow.Cells[0].Value.ToString();
                name = dgvSearchRes.CurrentRow.Cells[1].Value.ToString();

                upRate = float.Parse(txtUpRate.Text);
                valid = true;
            }

            catch
            {
                MessageBox.Show("Please enter valide Hourly Rate..!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                valid = false;
            }

            DialogResult dr = MessageBox.Show("Are You Sure do you want to Update " + name, "Update Employee", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes && valid == true)
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("update employee set empName = @name, hRate = @rate where empId = @id", con);
                    cmd.Parameters.Add(new SqlParameter("name", upName));
                    cmd.Parameters.Add(new SqlParameter("rate", upRate));
                    cmd.Parameters.Add(new SqlParameter("id", id));

                    cmd.ExecuteNonQuery();

                    con.Close();

                    MessageBox.Show("Employee Updated Successfully", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Can't Update Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvSearchRes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUpRate.Text = dgvSearchRes.CurrentRow.Cells[2].Value.ToString();
            txtUpName.Text = dgvSearchRes.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
