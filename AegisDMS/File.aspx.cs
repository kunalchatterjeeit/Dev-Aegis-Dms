using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AegisDMS
{
    public partial class File : System.Web.UI.Page
    {
        private void FileCategory_GetAll()
        {
            DataTable dtFileCategory = BusinessLayer.FileCategory.FileCategoryGetAll(new Entity.FileCategory());
            ddlFileCategory.DataSource = dtFileCategory;
            ddlFileCategory.DataTextField = "Name";
            ddlFileCategory.DataValueField = "FileCategoryId";
            ddlFileCategory.DataBind();
            ddlFileCategory.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "0", Selected = true });
        }
        private void FileType_GetAll()
        {
            DataTable dtFileType = BusinessLayer.FileType.GetAll(new Entity.FileType()
            {
                FileCategoryId = Convert.ToInt32(ddlFileCategory.SelectedValue.Trim())
            });
            ddlFileType.DataSource = dtFileType;
            ddlFileType.DataTextField = "Name";
            ddlFileType.DataValueField = "FileTypeId";
            ddlFileType.DataBind();
            ddlFileType.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "0", Selected = true });
        }
        private void Metadata_GetAll()
        {
            Entity.Metadata metadata = new Entity.Metadata()
            {
                FileTypeId = Convert.ToInt32(ddlFileType.SelectedValue.Trim())
            };

            gvMetadata.DataSource = BusinessLayer.FileMetadata.MetadataGetAll(metadata);
            gvMetadata.DataBind();
        }
        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtEntryDate.Text.Trim()))
            {
                lblMessage.Text = "Error: Please select entry date.";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
                return false;
            }
            if (ddlFileType.SelectedIndex == 0)
            {
                lblMessage.Text = "Error: Please select file type.";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
                return false;
            }
            if (!FileUpload1.HasFile && !FileUpload1.HasFiles)
            {
                lblMessage.Text = "Error: Please select file(s).";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
                return false;
            }

            lblMessage.Visible = false;
            return true;
        }
        private Guid? File_Save()
        {
            Guid? retValue = null;
            string filepath = Server.MapPath("\\Files");

            if (Validate())
            {
                foreach (HttpPostedFile fileItem in FileUpload1.PostedFiles)
                {
                    Entity.File file = new Entity.File()
                    {
                        FileTypeId = Convert.ToInt32(ddlFileType.SelectedValue),
                        PhysicalFileName = string.Empty,
                        FileOriginalName = Path.GetFileNameWithoutExtension(fileItem.FileName),
                        FileExtension = Path.GetExtension(fileItem.FileName),
                        EntryDate = Convert.ToDateTime(txtEntryDate.Text.Trim()),
                        IsFullTextCopied = false,
                        IsAttachment = false,
                        MainFileGuid = null,
                        CreatedDate = DateTime.Now,
                        CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
                        FileStatus = (int)Entity.FileStatus.Active
                    };

                    retValue = BusinessLayer.File.FileSave(file);

                    if (retValue != null)
                    {
                        file.PhysicalFileName = retValue + file.FileExtension;

                        string fileWithFullPath = filepath + "\\" + Path.GetFileName(file.FileOriginalName + file.FileExtension);

                        fileItem.SaveAs(fileWithFullPath);

                        //Build the File Path for the original (input) and the encrypted (output) file.
                        string decryptedOriginalFileNameWithPath = fileWithFullPath;
                        string encryptPhysicalFileNameWithPath = filepath + "\\" + file.PhysicalFileName;
                        BusinessLayer.GeneralSecurity.EncryptFile(decryptedOriginalFileNameWithPath, encryptPhysicalFileNameWithPath);
                        //Delete the original (input) file.
                        System.IO.File.Delete(decryptedOriginalFileNameWithPath);
                    }

                    if (IsMetadataManual())
                    {
                        MetadataFileMapping_Save(retValue);
                    }
                    else
                    {
                        MetadataFileMapping_Save(retValue, file.FileOriginalName);
                    }
                }
            }
            return retValue;
        }
        private bool IsMetadataManual()
        {
            bool retValue = false;
            foreach (GridViewRow gvr in gvMetadata.Rows)
            {
                TextBox txtMetadataValue = (TextBox)gvr.FindControl("txtMetadataValue");
                if (!string.IsNullOrEmpty(txtMetadataValue.Text.Trim()))
                {
                    retValue = true;
                }
            }
            return retValue;
        }
        private int MetadataFileMapping_Save(Guid? fileGuid)
        {
            int retValue = 0;
            foreach (GridViewRow gvr in gvMetadata.Rows)
            {
                TextBox txtMetadataValue = (TextBox)gvr.FindControl("txtMetadataValue");
                if (!string.IsNullOrEmpty(txtMetadataValue.Text.Trim()))
                {
                    Entity.MetadataFileMapping metadataFileMapping = new Entity.MetadataFileMapping()
                    {
                        MetadataId = Convert.ToInt64(gvMetadata.DataKeys[gvr.RowIndex].Values[0].ToString()),
                        FileGuid = fileGuid,
                        MetadataContent = txtMetadataValue.Text.Trim(),
                        CreatedDate = DateTime.Now,
                        CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name)
                    };
                    int respose = BusinessLayer.MetadataFileMapping.MetadataFileMappingSave(metadataFileMapping);
                    if (respose > 0)
                    {
                        retValue++;
                    }
                }
            }
            return retValue;
        }
        private int MetadataFileMapping_Save(Guid? fileGuid, string fileOriginalName)
        {
            int retValue = 0;
            try
            {
                string[] metadataValues = fileOriginalName.Split('_');
                for (int metadataIndex = 0; metadataIndex < metadataValues.Count(); metadataIndex++)
                {
                    if (!string.IsNullOrEmpty(metadataValues[metadataIndex]))
                    {
                        Entity.MetadataFileMapping metadataFileMapping = new Entity.MetadataFileMapping()
                        {
                            MetadataId = Convert.ToInt64(gvMetadata.DataKeys[gvMetadata.Rows[metadataIndex].RowIndex].Values[0].ToString()),
                            FileGuid = fileGuid,
                            MetadataContent = metadataValues[metadataIndex].Trim(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name)
                        };

                        int respose = BusinessLayer.MetadataFileMapping.MetadataFileMappingSave(metadataFileMapping);
                        if (respose > 0)
                        {
                            retValue++;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
            
            }
            return retValue;
        }
        private void Clear()
        {
            ddlFileType.SelectedIndex = 0;
            txtEntryDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileCategory_GetAll();
                FileType_GetAll();
                Clear();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            File_Save();
            Clear();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        protected void ddlFileCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileType_GetAll();
        }
        protected void ddlFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Metadata_GetAll();
        }
    }
}