using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using io = System.IO;
using BusinessLayer;

namespace AegisDMS
{
    public partial class DisplayExcel : System.Web.UI.Page
    {
        private string _fileGuid { get; set; }
        private string _decryptFile { get; set; }

        private void LoadFile()
        {
            string randomName = DateTime.Now.Ticks.ToString();
            object htmlFilePath = Server.MapPath("") + "\\Files\\Temp\\" + randomName + ".htm";

            Excel.Application excel = null;
            Excel.Workbook xls = null;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                object missing = Type.Missing;
                object trueObject = true;
                excel.Visible = false;
                excel.DisplayAlerts = false;//We set this to false since we don't want alert dialog boxes popping up on the server running this code
                object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;

                xls = excel.Workbooks.Open(_decryptFile, missing, trueObject, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                foreach (Worksheet sheet in xls.Worksheets)
                {
                    if (sheet.Index.Equals(1))
                    {
                        Range last = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                        Range range = sheet.get_Range("A1", last);

                        String excelrange = "$A1:$" + ColumnIndexToColumnLetter(range.Columns.Count) + range.Rows.Count;
                        xls.PublishObjects.Add(XlSourceType.xlSourceRange, htmlFilePath.ToString(), sheet.Name, excelrange, missing, missing, missing).Publish(true);
                    }
                }
            }
            catch (CustomException ex)
            {
                ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            }
            finally
            {
                excel.Quit();//make sure to close the application when done
            }

            //Read the Html File as Byte Array and Display it on browser
            byte[] bytes;
            using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(fs);
                bytes = reader.ReadBytes((int)fs.Length);
                fs.Close();
            }
            Response.BinaryWrite(bytes);
            Response.Flush();

            io.File.Delete(htmlFilePath.ToString());
            io.File.Delete(_decryptFile.ToString());
            Response.End();
        }

        private static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string rawId = Request.QueryString["id"].ToString().ToDecrypt(true);
                _fileGuid = (rawId != null && !string.IsNullOrEmpty(rawId)) ? rawId : string.Empty;

                System.Data.DataTable dtFile = BusinessLayer.File.FileGetByFileGuid(_fileGuid);
                string encryptedPhysicalFileNameWithPath = (dtFile != null && dtFile.Rows.Count > 0) ? Server.MapPath("~/Files/") + dtFile.Rows[0]["PhysicalFileName"].ToString() + dtFile.Rows[0]["FileExtension"].ToString() : string.Empty;
                string decryptOriginalFileNameWithPath = (dtFile != null && dtFile.Rows.Count > 0) ? Server.MapPath("~/Files/Raw/") + dtFile.Rows[0]["FileOriginalName"].ToString() + dtFile.Rows[0]["FileExtension"].ToString() : string.Empty;
                _decryptFile = decryptOriginalFileNameWithPath;

                BusinessLayer.GeneralSecurity.DecryptFile(encryptedPhysicalFileNameWithPath, decryptOriginalFileNameWithPath);
                LoadFile();
            }
            catch (CustomException ex)
            {
                ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            }
        }
    }
}