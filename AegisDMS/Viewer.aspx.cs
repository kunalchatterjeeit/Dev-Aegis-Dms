using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;

namespace AegisDMS
{
    public partial class Viewer : System.Web.UI.Page
    {
        private Guid _fileGuid { get; set; }
        private void PrepareFileLoader()
        {
            DataTable dtFile = BusinessLayer.File.FileGetByFileGuid(_fileGuid.ToString());
            string fileExtension = (dtFile != null && dtFile.Rows.Count > 0) ? dtFile.Rows[0]["FileExtension"].ToString() : string.Empty;

            if (fileExtension.Equals(".doc") || fileExtension.Equals(".docx"))
            {
                iframe.Attributes.Add("src", string.Format("DisplayWord.aspx?id={0}", _fileGuid.ToString().ToEncrypt(true)));
            }
            else if (fileExtension.Equals(".pdf"))
            {
                iframe.Attributes.Add("src", string.Format("DisplayPdf.aspx?id={0}", _fileGuid.ToString().ToEncrypt(true)));
            }
            else if (fileExtension.Equals(".xls") || fileExtension.Equals(".xlsx"))
            {
                iframe.Attributes.Add("src", string.Format("DisplayExcel.aspx?id={0}", _fileGuid.ToString().ToEncrypt(true)));
            }
            else
            {
                iframe.Attributes.Add("src", "NoPreview.aspx");
            }

            if (dtFile != null && dtFile.Rows.Count > 0)
            {
                lblFileName.Text = dtFile.Rows[0]["FileOriginalName"].ToString() + dtFile.Rows[0]["FileExtension"].ToString();
                lblEntryDate.Text = dtFile.Rows[0]["EntryDate"].ToString();
                lblFileAttachment.Text = (dtFile.Rows[0]["IsAttachment"].ToString().Equals("0")) ? "No" : "Yes";
                lblFullTextCopy.Text = (dtFile.Rows[0]["IsFullTextCopied"].ToString().Equals("1")) ? "Done" : "Not done";
                lblUploadDate.Text = dtFile.Rows[0]["CreatedDate"].ToString();
                lblUploadedBy.Text = dtFile.Rows[0]["CreatedBy"].ToString();
                lblModifiedDate.Text = (string.IsNullOrEmpty(dtFile.Rows[0]["LastModifiedDate"].ToString())) ? "Not modified yet" : dtFile.Rows[0]["LastModifiedDate"].ToString();
                lblModifiedBy.Text = (string.IsNullOrEmpty(dtFile.Rows[0]["LastModifiedBy"].ToString())) ? "Not modified yet" : dtFile.Rows[0]["LastModifiedBy"].ToString();
            }
        }
        private void MetadataFileMappingGetByFileGuid()
        {
            DataTable dtMetadataFileMapping = BusinessLayer.File.MetadataFileMappingGetByFileGuid(_fileGuid.ToString());
            if (dtMetadataFileMapping != null && dtMetadataFileMapping.Rows.Count > 0)
            {
                gvMetaDataContent.DataSource = dtMetadataFileMapping;
                gvMetaDataContent.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawId = Request.QueryString["id"].ToString().ToDecrypt(true);
            _fileGuid = (rawId != null && !string.IsNullOrEmpty(rawId)) ? Guid.Parse(rawId) : new Guid();

            MetadataFileMappingGetByFileGuid();
            PrepareFileLoader();
        }

    }
}