using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Payroll_System_HADGreen_pvt
{
    class printer
    {
        string src; // webpage source
        string mainTitle;
        string[] upperTitles;
        string[] lowerTitles;

        DataGridView result;

        public printer(string mainTitle,  string[] upperTitles, DataGridView result, string[] lowerTitles)
        {
            this.mainTitle = mainTitle;
            this.upperTitles = upperTitles;
            this.lowerTitles = lowerTitles;
            this.result = result;
        }

        public void print()
        {
            WebBrowser wb = new WebBrowser();
            src = "";

            loadLines("Head");

            // src += "<h4> Date : " + DateTime.Today.ToString("dd MMMM yyyy - dddd") + "</h4>";

            // create main title
            src += "<center><h3>" + mainTitle + "</h3></center> <br>";

            // create upper titles
            foreach(string ttl in upperTitles)
            {
                src += "<h4>" + ttl + "</h4>";
            }

            src += "<br>";

            loadLines("tableHead");

            for (int i = 0; i < result.ColumnCount; i++)
            {
                src += "<th>" + result.Columns[i].HeaderText + "</th>";
            }

            src += "</tr>";

            for (int i = 0; i < result.RowCount - 1; i++)
            {
                src += "<tr>";

                for (int j = 0; j < result.ColumnCount; j++)
                {
                    if (result.Rows[i].Cells[j].ValueType == typeof(DateTime) )
                    {
                        DateTime temp = new DateTime();
                        temp = Convert.ToDateTime(result.Rows[i].Cells[j].Value.ToString());
                        src += "<td>" + temp.ToString("yyyy - MM - dd") + "</td>";
                    }
                    else
                    {
                        src += "<td>" + result.Rows[i].Cells[j].Value.ToString() + "</td>";
                    }
                    
                }

                src += "</tr>";
            }

            loadLines("tableEnd");

            // create lower titles
            foreach (string ttl in lowerTitles)
            {
                src += "<h4>" + ttl + "</h4>";
            }

            loadLines("End");

            wb.DocumentText += src;
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(PrintDocument);
        }

        private void loadLines(string name)
        {
            try
            {
                StreamReader sr = new StreamReader("Data\\" + name + ".html");

                string line = sr.ReadLine();

                while (line != null)
                {
                    src += line;

                    line = sr.ReadLine();
                }

                sr.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).ShowPrintPreviewDialog();
            //((WebBrowser)sender).Print();

            ((WebBrowser)sender).Dispose();
        }
    }
}
