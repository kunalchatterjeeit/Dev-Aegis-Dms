using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using io = System.IO;
using BusinessLayer;

namespace AegisDMS
{
    public partial class DisplayWord : System.Web.UI.Page
    {
        private string _fileGuid { get; set; }
        private string _decryptFile { get; set; }

        private void LoadFile()
        {
            string FilePath = _decryptFile;

            object _strFilePath = _decryptFile;
            object missingType = Type.Missing;
            object readOnly = true;
            object isVisible = false;
            object documentFormat = 8;
            string randomName = DateTime.Now.Ticks.ToString();
            object htmlFilePath = Server.MapPath("") + "\\Files\\Temp\\" + randomName + ".htm";

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
            using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(fs);
                bytes = reader.ReadBytes((int)fs.Length);
                fs.Close();
            }
            Response.BinaryWrite(bytes);
            Response.Flush();

            io.File.Delete(htmlFilePath.ToString());
            io.File.Delete(_strFilePath.ToString());
            Response.End();        
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string rawId = Request.QueryString["id"].ToString().ToDecrypt(true);
            _fileGuid = (rawId != null && !string.IsNullOrEmpty(rawId)) ? rawId : string.Empty;

            DataTable dtFile = BusinessLayer.File.FileGetByFileGuid(_fileGuid);
            string encryptedPhysicalFileNameWithPath = (dtFile != null && dtFile.Rows.Count > 0) ? Server.MapPath("~/Files/") + dtFile.Rows[0]["PhysicalFileName"].ToString() + dtFile.Rows[0]["FileExtension"].ToString() : string.Empty;
            string decryptOriginalFileNameWithPath = (dtFile != null && dtFile.Rows.Count > 0) ? Server.MapPath("~/Files/Raw/") + dtFile.Rows[0]["FileOriginalName"].ToString() + dtFile.Rows[0]["FileExtension"].ToString() : string.Empty;
            _decryptFile = decryptOriginalFileNameWithPath;

            BusinessLayer.GeneralSecurity.DecryptFile(encryptedPhysicalFileNameWithPath, decryptOriginalFileNameWithPath);
            LoadFile();
        }
    }
}