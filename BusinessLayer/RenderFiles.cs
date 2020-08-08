using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using OpenXmlPowerTools;
using System;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace BusinessLayer
{
    public static class RenderFiles
    {
        public static object ReadMicrosoftWordByOfficeInterop(string decryptedFilePath)
        {
            object htmlFilePath = string.Empty;
            try
            {
                object _strFilePath = decryptedFilePath;
                object missingType = Type.Missing;
                object readOnly = true;
                object isVisible = false;
                object documentFormat = 8;
                string randomName = DateTime.Now.Ticks.ToString();
                htmlFilePath = HttpContext.Current.Server.MapPath("~/") + "RawFiles\\Temp\\" + randomName + ".htm";

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


                // Delete table
                foreach (Microsoft.Office.Interop.Word.Table tbl in document.Tables)
                    tbl.Delete();
                // Delete Shape
                foreach (Microsoft.Office.Interop.Word.Shape shp in document.Shapes)
                    shp.Delete();
                // Delete content control
                foreach (Microsoft.Office.Interop.Word.ContentControl contentControl in document.ContentControls)
                    contentControl.Delete();
                // Delete Inline Shape
                foreach (Microsoft.Office.Interop.Word.InlineShape ilshp in document.InlineShapes)
                {
                    if (ilshp.Type == Microsoft.Office.Interop.Word.WdInlineShapeType.wdInlineShapeEmbeddedOLEObject)
                        ilshp.Delete();
                }

                //Close the word document
                document.Close(false);
                applicationclass.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges,
                    Microsoft.Office.Interop.Word.WdOriginalFormat.wdWordDocument);
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(applicationclass);
                document = null;
                applicationclass = null;


                //Read the Html File as Byte Array and Display it on browser
                //byte[] bytes;
                using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
                {
                    //using (BinaryReader reader = new BinaryReader(fs))
                    //{
                    //    bytes = reader.ReadBytes((int)fs.Length);
                    //    fs.Close();
                    //    fs.Dispose();
                    //    reader.Dispose();
                    //}
                }

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                htmlFilePath = (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string)) + "RawFiles/Temp/" + randomName + ".htm";
                //System.IO.File.Delete(htmlFilePath.ToString());
                //System.IO.File.Delete(_strFilePath.ToString());
            }
            catch (Exception ex)
            {
                htmlFilePath = ex.Message;
            }
            return htmlFilePath;
        }

        public static object ReadMicrosoftExcelByOfficeInterop(string decryptedFilePath)
        {
            object htmlFilePath = string.Empty;
            try
            {
                string randomName = DateTime.Now.Ticks.ToString();
                htmlFilePath = HttpContext.Current.Server.MapPath("~/") + "RawFiles\\Temp\\" + randomName + ".htm";

                Application excel = null;
                Workbook xls = null;

                try
                {
                    excel = new Microsoft.Office.Interop.Excel.Application();
                    object missing = Type.Missing;
                    object trueObject = true;
                    excel.Visible = false;
                    excel.DisplayAlerts = false;//We set this to false since we don't want alert dialog boxes popping up on the server running this code
                    object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;

                    xls = excel.Workbooks.Open(decryptedFilePath, missing, trueObject, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                    foreach (Worksheet sheet in xls.Worksheets)
                    {
                        if (sheet.Index.Equals(1))
                        {
                            Range last = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                            Range range = sheet.get_Range("A1", last);

                            String excelrange = "$A1:$" + ColumnIndexToColumnLetter(range.Columns.Count) + range.Rows.Count;
                            xls.PublishObjects.Add(XlSourceType.xlSourceRange, htmlFilePath.ToString(), sheet.Name, excelrange, missing, missing, missing).Publish(true);
                        }
                    }

                    xls.Close(false);
                    Marshal.ReleaseComObject(xls);
                    Marshal.ReleaseComObject(excel);
                    xls = null;
                    excel = null;

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    htmlFilePath = (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string)) + "RawFiles/Temp/" + randomName + ".htm";
                }
                catch (Exception ex)
                {
                    htmlFilePath = ex.Message;
                }
                finally
                {
                    excel.Quit();//make sure to close the application when done
                }

                //Read the Html File as Byte Array and Display it on browser
                //byte[] bytes;
                using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
                {
                }
            }
            catch (Exception ex)
            {

            }
            return htmlFilePath;
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

        public static string ReadMicrosoftWordByOpenXml(string decryptedFilePath)
        {
            string randomName = DateTime.Now.Ticks.ToString();
            string htmlFilePath = HttpContext.Current.Server.MapPath("~/") + "RawFiles\\Temp\\" + randomName + ".html";

            try
            {
                DirectoryInfo convertedDocsDirectory =
                    new DirectoryInfo(htmlFilePath);
                ConvertWordToHtml(decryptedFilePath, convertedDocsDirectory, htmlFilePath);
                htmlFilePath = (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string)) + "RawFiles/Temp/" + randomName + ".html";
            }
            catch (Exception ex)
            {
                htmlFilePath = ex.Message;
            }
            return htmlFilePath;
        }

        public static void ConvertWordToHtml(string decryptedFilePath, DirectoryInfo destDirectory, string htmlFileName)
        {
            FileInfo fiHtml = new FileInfo(System.IO.Path.Combine(destDirectory.FullName, htmlFileName));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //memoryStream.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wDoc = WordprocessingDocument.Open(decryptedFilePath, true))
                {
                    var imageDirectoryFullName =
                        fiHtml.FullName.Substring(0, fiHtml.FullName.Length - fiHtml.Extension.Length) + "_files";
                    var imageDirectoryRelativeName =
                        fiHtml.Name.Substring(0, fiHtml.Name.Length - fiHtml.Extension.Length) + "_files";
                    int imageCounter = 0;
                    var pageTitle = "";// (string)wDoc.CoreFilePropertiesPart.GetXDocument().Descendants(DC.title).;

                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        PageTitle = pageTitle,
                        FabricateCssClasses = true,
                        CssClassPrefix = "pt-",
                        RestrictToSupportedLanguages = false,
                        RestrictToSupportedNumberingFormats = false,
                        ImageHandler = imageInfo =>
                        {
                            DirectoryInfo localDirInfo = new DirectoryInfo(imageDirectoryFullName);
                            if (!localDirInfo.Exists)
                                localDirInfo.Create();
                            ++imageCounter;
                            string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                            ImageFormat imageFormat = null;
                            if (extension == "png")
                            {
                                // Convert png to jpeg.
                                extension = "gif";
                                imageFormat = ImageFormat.Gif;
                            }
                            else if (extension == "gif")
                                imageFormat = ImageFormat.Gif;
                            else if (extension == "bmp")
                                imageFormat = ImageFormat.Bmp;
                            else if (extension == "jpeg")
                                imageFormat = ImageFormat.Jpeg;
                            else if (extension == "tiff")
                            {
                                // Convert tiff to gif.
                                extension = "gif";
                                imageFormat = ImageFormat.Gif;
                            }
                            else if (extension == "x-wmf")
                            {
                                extension = "wmf";
                                imageFormat = ImageFormat.Wmf;
                            }

                            // If the image format isn't one that we expect, ignore it,
                            // and don't return markup for the link.
                            if (imageFormat == null)
                                return null;

                            FileInfo imageFileName = new FileInfo(imageDirectoryFullName + "/image" +
                                imageCounter.ToString() + "." + extension);
                            try
                            {
                                imageInfo.Bitmap.Save(imageFileName.FullName, imageFormat);
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }
                            XElement img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, imageDirectoryRelativeName + "/" + imageFileName.Name),
                                imageInfo.ImgStyleAttribute,
                                imageInfo.AltText != null ?
                                    new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    XElement html = HtmlConverter.ConvertToHtml(wDoc, settings);

                    // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                    // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                    // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                    // for detailed explanation.
                    //
                    // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                    // must do it correctly, or entities will not be serialized properly.

                    var body = html.Descendants(Xhtml.body).First();
                    //body.AddFirst(
                    //    new XElement(Xhtml.p,
                    //        new XElement(Xhtml.A,
                    //            new XAttribute("href", "/WebForm1.aspx"), "Go back to Upload Page")));

                    var htmlString = html.ToString(SaveOptions.DisableFormatting);

                    System.IO.File.WriteAllText(fiHtml.FullName, htmlString, Encoding.UTF8);
                }
            }
        }
    }
}
