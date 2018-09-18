using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using io = System.IO;
using BusinessLayer;

namespace AegisDMS
{
    public partial class DisplayPdf : System.Web.UI.Page
    {
        private string _fileGuid { get; set; }
        private string _decryptFile { get; set; }

        private void LoadFile()
        {
            string FilePath = _decryptFile;
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);

                //Delete the original (input) and the decrypted (output) file.
                io.File.Delete(_decryptFile.ToString());

                Response.Flush();
                Response.End();
            }
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