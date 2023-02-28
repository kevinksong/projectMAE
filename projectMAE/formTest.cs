using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace projectMAE
{
    public partial class formTest : Form
    {
        public formTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Browse
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "AutoCAD files |*.dwg";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach(string file in openFileDialog.FileNames)
                    {
                        txtDirectory.Text = file;
                        //listBox1.Items.Add(Path.GetFileName(file));
                        listBox1.Items.Add(file);
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            //Create excel file in desired location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter= "Excel files|*.xls;*.xlsx;*.xlsm";
            saveFileDialog.Title = "Save Excel file";
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string excelFileName = saveFileDialog.FileName;
                string extension = Path.GetExtension(excelFileName);

                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.DisplayAlerts = false;
                Workbook wb = excel.Workbooks.Add();
                wb.SaveAs(excelFileName);
                //Read Drawings
                int columnNumber = 1;
                foreach (string fileName in listBox1.Items)
                {
                    DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager;
                    if (File.Exists(fileName))
                    {
                        drawingTools.writeExcel(columnNumber, fileName, wb);
                        columnNumber += 1;
                    }
                }
                wb.Close();
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Cancel
            this.DialogResult = DialogResult.Cancel;
        }

        private void formTest_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
