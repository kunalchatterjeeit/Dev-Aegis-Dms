using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Data;
using io = System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class FileContentTransfer
    {
        public static void StartProcess(Guid fileGuid, string decryptOriginalFileNameWithPath, string htmlFilePath)
        {
            try
            {
                string innertext = string.Empty;
                if (System.IO.Path.GetExtension(decryptOriginalFileNameWithPath) == ".pdf")
                {
                    //Get text from .pdf
                    innertext = ExtractTextFromPdf(decryptOriginalFileNameWithPath);
                    innertext = OptimizeFileContent(innertext);
                    io.File.Delete(decryptOriginalFileNameWithPath);
                }
                else if (System.IO.Path.GetExtension(decryptOriginalFileNameWithPath) == ".doc"
                    || System.IO.Path.GetExtension(decryptOriginalFileNameWithPath) == ".docx")
                {
                    //Get text from .doc, .docx
                    innertext = ExtractTextFromWord(decryptOriginalFileNameWithPath, htmlFilePath);
                    innertext = OptimizeFileContent(innertext);
                    io.File.Delete(decryptOriginalFileNameWithPath);
                }
                else if (System.IO.Path.GetExtension(decryptOriginalFileNameWithPath) == ".xls"
                    || System.IO.Path.GetExtension(decryptOriginalFileNameWithPath) == ".xlsx")
                {
                    //Get text from .xls, .xlsx
                    innertext = ExtractTextFromExcel(decryptOriginalFileNameWithPath, htmlFilePath);
                    innertext = OptimizeFileContent(innertext);
                    io.File.Delete(decryptOriginalFileNameWithPath);
                }

                BusinessLayer.File.File_Content_Save(fileGuid, innertext); //Saving
            }
            catch (CustomException ex)
            {
                string err = ex.Log("FileContentTransfer", 0).ToString();
            }
        }

        private static string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString();
            }
        }

        private static string ExtractTextFromWord(object _strFilePath, object htmlFilePath)
        {
            object missingType = Type.Missing;
            object readOnly = true;
            object isVisible = false;
            object documentFormat = 8;

            //Open the word document in background
            Microsoft.Office.Interop.Word.Application applicationclass = new Microsoft.Office.Interop.Word.Application();
            applicationclass.Documents.Open(ref _strFilePath,
                                            ref readOnly,
                                            ref missingType, ref missingType, ref missingType,
                                            ref missingType, ref missingType, ref missingType,
                                            ref missingType, ref missingType, ref isVisible,
                                            ref missingType, ref missingType, ref missingType,
                                            ref missingType, ref missingType);
            applicationclass.Visible = false;
            Microsoft.Office.Interop.Word.Document document = applicationclass.ActiveDocument;

            //Save the word document as HTML file
            document.SaveAs(ref htmlFilePath, ref documentFormat, ref missingType,
                            ref missingType, ref missingType, ref missingType,
                            ref missingType, ref missingType, ref missingType,
                            ref missingType, ref missingType, ref missingType,
                            ref missingType, ref missingType, ref missingType,
                            ref missingType);

            //Close the word document
            document.Close(ref missingType, ref missingType, ref missingType);

            //Read the Html File as Byte Array and Display it on browser
            byte[] bytes;
            using (io.FileStream fs = new io.FileStream(htmlFilePath.ToString(), io.FileMode.Open, io.FileAccess.Read))
            {
                io.BinaryReader reader = new io.BinaryReader(fs);
                bytes = reader.ReadBytes((int)fs.Length);
                fs.Close();
            }

            string content = Encoding.UTF8.GetString(bytes);

            io.File.Delete(htmlFilePath.ToString());
            io.File.Delete(_strFilePath.ToString());

            return content;
        }

        private static string ExtractTextFromExcel(object _strFilePath, string htmlFilePath)
        {
            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbook xls = null;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                object missing = Type.Missing;
                object trueObject = true;
                excel.Visible = false;
                excel.DisplayAlerts = false;//We set this to false since we don't want alert dialog boxes popping up on the server running this code
                object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;

                xls = excel.Workbooks.Open(_strFilePath.ToString(), missing, trueObject, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in xls.Worksheets)
                {
                    if (sheet.Index.Equals(1))
                    {
                        Microsoft.Office.Interop.Excel.Range last = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                        Microsoft.Office.Interop.Excel.Range range = sheet.get_Range("A1", last);

                        String excelrange = "$A1:$" + ColumnIndexToColumnLetter(range.Columns.Count) + range.Rows.Count;
                        xls.PublishObjects.Add(Microsoft.Office.Interop.Excel.XlSourceType.xlSourceRange, htmlFilePath.ToString(), sheet.Name, excelrange, missing, missing, missing).Publish(true);
                    }
                }
            }
            catch (CustomException ex)
            {
                ex.Log("FileContentTransfer", 0);
            }
            finally
            {
                excel.Quit();//make sure to close the application when done
            }

            //Read the Html File as Byte Array and Display it on browser
            byte[] bytes;
            using (io.FileStream fs = new io.FileStream(htmlFilePath.ToString(), io.FileMode.Open, io.FileAccess.Read))
            {
                io.BinaryReader reader = new io.BinaryReader(fs);
                bytes = reader.ReadBytes((int)fs.Length);
                fs.Close();
            }

            string content = Encoding.UTF8.GetString(bytes);

            io.File.Delete(htmlFilePath.ToString());
            io.File.Delete(_strFilePath.ToString());

            return content;
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

        private static string OptimizeFileContent(string content)
        {
            //Removing <Something> from content
            int count = content.Count(x => x == '>');
            for (int i = 0; i < count; i++)
            {
                try
                {
                    content = content.Remove(
                    content.IndexOf("<", StringComparison.InvariantCultureIgnoreCase)
                    ,
                    content.IndexOf(">") - content.IndexOf("<", StringComparison.InvariantCultureIgnoreCase) + 1
                    );
                }
                catch (CustomException ex)
                {
                    ex.Log("FileContentTransfer", 0);
                    break;
                }
            }

            //Removing special characters
            //content = Regex.Replace(content, "[^a-zA-Z_]+", " ");

            //Removing duplicate words
            string outputString = string.Empty;
            string[] input = content.Trim().Split(' ');

            IEnumerable<string> IList = input.Distinct();

            outputString = string.Join(" ", Array.ConvertAll(IList.ToArray(), i => i.ToString()));

            return outputString;
        }
    }
}
