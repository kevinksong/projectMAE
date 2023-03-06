using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

[assembly: CommandClass(typeof(projectMAE.drawingTools))]
namespace projectMAE
{
    public class drawingTools
    {

        [CommandMethod("DEMO")]
        public void Demo()
        {
            //opening the form

            using (formTest ft = new formTest())
            {
                ft.ShowDialog();
            }
              
        }
        [CommandMethod("EXTRACTTEXT")]
        public static void extractText()
        {
            //get the current database

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //start a transaction
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))
                {
                    //opening the block table record
                    BlockTableRecord model = trans.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead) as BlockTableRecord;

                    foreach (ObjectId id in model)
                    {
                        switch (id.ObjectClass.DxfName)
                        {
                            case "TEXT":
                                DBText text = trans.GetObject(id, OpenMode.ForRead) as DBText;
                                if (text.TextString != "")
                                {
                                    outputFile.WriteLine(text.TextString);
                                }
                                //ed.WriteMessage($"\nText = {text.TextString} ({text.Position})");
                                break;
                            case "MTEXT":
                                MText mtext = trans.GetObject(id, OpenMode.ForWrite) as MText;
                                if (mtext.Text != "")
                                {
                                    outputFile.WriteLine(mtext.Text);
                                }
                                //ed.WriteMessage($"\nText = {mtext.Text} ({mtext.Location})");
                                break;
                            default:
                                break;
                        }
                    }
                }
                //commit the transaction
                trans.Commit();
            }
        }
        public static void writeExcel(int columnNumber, string fileName, Workbook wb)
        {
            //get the current database
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.Open(fileName);
            Database db = doc.Database;

            //opening excel workbook and worksheet
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //suppress displaying alerts (such as prompting to overwrite existing file
            excel.DisplayAlerts = false;

            //open worksheet
            Worksheet ws;
            ws = wb.Worksheets[1];

            //first cell in each column is reserved for the filename of associated AutoCAD drawing
            ws.Cells[1, columnNumber] = fileName;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //lock document before opening block table record
                doc.LockDocument();

                    //opening the block table record
                    BlockTableRecord model = trans.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead) as BlockTableRecord;

                    int rownumber = 2;

                    foreach (ObjectId id in model)
                    {
                        switch (id.ObjectClass.DxfName)
                        {
                            case "TEXT":
                                DBText text = trans.GetObject(id, OpenMode.ForRead) as DBText;
                                ws.Cells[rownumber, columnNumber] = text.TextString;
                                rownumber += 1;
                                //ed.WriteMessage($"\nText = {text.TextString} ({text.Position})");
                                break;
                            case "MTEXT":
                                MText mtext = trans.GetObject(id, OpenMode.ForWrite) as MText;
                                ws.Cells[rownumber, columnNumber] = mtext.Text;
                                rownumber += 1;
                                //ed.WriteMessage($"\nText = {mtext.Text} ({mtext.Location})");
                                break;
                            default:
                                break;
                        }
                    }
                //commit the transaction
                trans.Commit();
            }
            wb.Save();
        }
        public static void ReadExcel()
        {

        }
    }

}
