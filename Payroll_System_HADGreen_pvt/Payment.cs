using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Payroll_System_HADGreen_pvt
{
    class Payment
    {
        public string id;  //

        public DateTime wDate;  //
        public DateTime inTime;  //
        public DateTime outTime;  //

        public float hourlyRate;

        public float advance; //
        public float deduction;  //

        public int workedHours; //
        public int OTHours;

        public float bSal; //
        public float OT;
        public float netSal;  //

        public Payment(string id, DateTime wDate, DateTime inTime, DateTime outTime, float hourlyRate, float advance, float deduction, DateTime deadLine, float OTAdd)
        {
            this.id = id;
            this.wDate = wDate;
            this.inTime = inTime;
            this.outTime = outTime;
            this.hourlyRate = hourlyRate;
            this.advance = advance;
            this.deduction = deduction;

            if ((deadLine - inTime).TotalMinutes >= 0)
            {

                workedHours = (outTime - inTime).Hours;

                if((outTime - inTime).Minutes > 40)
                {
                    workedHours += 1;
                }

                if (workedHours > 4)
                {
                    workedHours -= 1;
                }

                if (workedHours > 8)
                {
                    bSal = hourlyRate * 8;
                    OTHours = workedHours - 8;
                    OT = OTHours * (hourlyRate + OTAdd);
                }

                else
                {
                    bSal = hourlyRate * workedHours;
                    OT = 0;
                }
            }

            else
            {
                workedHours = 0;
                bSal = 0;
                OT = 0;
            }


            netSal = bSal + OT + advance - deduction;
        }

        public bool saveData()
        {
            SqlConnection con = new SqlConnection("server=localhost; Trusted_Connection=yes; database=hadGreenPayroll;");

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO payments(empId, workedDate, inTime, outTime , workedHours, bSal, advance, deduction, netSal) values(@d1, @d2, @d3, @d4, @d5, @d6, @d7, @d8, @d9)", con);
                cmd.Parameters.Add(new SqlParameter("d1", this.id));
                cmd.Parameters.Add(new SqlParameter("d2", this.wDate.ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("d3", this.inTime.ToString("HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("d4", this.outTime.ToString("HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("d5", this.workedHours.ToString()));
                cmd.Parameters.Add(new SqlParameter("d6", this.bSal.ToString()));
                cmd.Parameters.Add(new SqlParameter("d7", this.advance.ToString()));
                cmd.Parameters.Add(new SqlParameter("d8", this.deduction.ToString()));
                cmd.Parameters.Add(new SqlParameter("d9", this.netSal.ToString()));

                cmd.ExecuteNonQuery();

                con.Close();

                return true;
            }

            catch
            {
                
                return false;
            }

        }
    }
}
