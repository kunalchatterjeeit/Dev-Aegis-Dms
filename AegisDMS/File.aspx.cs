using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using io = System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using itext = iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using ps = PdfSharp.Pdf.IO;
//using psMerge = PdfMergeApp.Common;
using psPdf = PdfSharp.Pdf;

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
        private void UserGroup_GetAll()
        {
            //chklUserGroup.DataSource = BusinessLayer.UserGroup.UserGroupGetAll(new Entity.UserGroup());
            //chklUserGroup.DataTextField = "Name";
            //chklUserGroup.DataValueField = "UserGroupId";
            //chklUserGroup.DataBind();

            //chklUserGroup.Items.Insert(0, new ListItem() { Text = "All", Value = "*" });

            gvUserGroup.DataSource = BusinessLayer.UserGroup.UserGroupGetAll(new Entity.UserGroup());
            gvUserGroup.DataBind();
        }
        private bool FileUploadValidate()
        {
            if (string.IsNullOrWhiteSpace(txtEntryDate.Text.Trim()))
            {
                UserMessage.Text = "Please select entry date.";
                UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                return false;
            }
            if (ddlFileType.SelectedIndex == 0)
            {
                UserMessage.Text = "Please select file type.";
                UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                return false;
            }
            if (!FileUpload1.HasFile && !FileUpload1.HasFiles)
            {
                UserMessage.Text = "Please select file(s).";
                UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                return false;
            }
            if (gvUserGroup.Rows.Count > 1)
            {
                bool isCheckedAny = false;
                for (int index = 0; index < gvUserGroup.Rows.Count; index++)
                {
                    if (index != 0 && ((CheckBox)gvUserGroup.Rows[index].FindControl("chkGroup")).Checked)
                    {
                        isCheckedAny = true;
                        break;
                    }
                }

                if (!isCheckedAny)
                {
                    UserMessage.Text = "Please select user group.";
                    UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                    return false;
                }
            }
            else
            {
                UserMessage.Text = "Please select user group.";
                UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                return false;
            }

            UserMessage.Text = string.Empty;
            return true;
        }
        private Collection<string> SegregatedFiles(string fileName)
        {
            Collection<string> seperatedFiles = new Collection<string>();
            DataTable pdfSeperator = BusinessLayer.File.PdfSeperatorGetAll();
            string filePath = Server.MapPath("\\Files\\Original\\") + fileName;

            if (pdfSeperator != null && pdfSeperator.Rows.Count > 0)
            {
                itext.Rectangle rectangle = new itext.Rectangle(0, 500, 110, 1000);
                RenderFilter[] filter = { new RegionTextRenderFilter(rectangle) };
                using (PdfReader pdfReader = new PdfReader(filePath))
                {
                    for (int pageNo = 1; pageNo <= pdfReader.NumberOfPages; pageNo++)
                    {
                        ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
                        string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, pageNo, strategy);
                        var array = pageText.Split('\n');

                        foreach (DataRow drSeperator in pdfSeperator.Rows)
                        {
                            if (array.Contains(drSeperator["Phrase"].ToString()))
                            {
                                seperatedFiles.Add(PdfCreator(pageNo, pageNo, filePath, fileName));
                                break;
                            }
                        }
                    }
                }

                if (!seperatedFiles.Any())
                {
                    seperatedFiles.Add(filePath);
                }
            }
            return seperatedFiles;
        }
        private string PdfCreator(int startPage, int lastPage, string mainFilePath, string fileName)
        {
            string newPdfPath = string.Empty;
            using (psPdf.PdfDocument mainDoc = new psPdf.PdfDocument())
            {
                using (psPdf.PdfDocument mainPdfCopy = ps.PdfReader.Open(mainFilePath, ps.PdfDocumentOpenMode.Import))
                {
                    for (int pageNo = startPage; pageNo <= lastPage; pageNo++)
                    {
                        mainDoc.AddPage(mainPdfCopy.Pages[pageNo]);
                    }
                    fileName = io.Path.GetFileNameWithoutExtension(fileName) + "_" + startPage.ToString() + "_" + lastPage.ToString() + ".pdf";
                    newPdfPath = Server.MapPath("\\Files\\Cropped\\") + fileName;
                    mainDoc.Save(newPdfPath);
                }
            }
            return newPdfPath;
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
                    if (!string.IsNullOrEmpty(metadataValues[metadataIndex]) && (gvMetadata.Rows.Count > 0 && gvMetadata.Rows.Count > metadataIndex))
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
            catch (CustomException ex)
            {
                ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            }
            return retValue;
        }
        private int MetadataFileMapping_Save_pdf(Guid? fileGuid, string croppedFile)
        {
            int retValue = 0;
            int pageNo = 1;
            try
            {
                DataTable pdfSeperator = BusinessLayer.File.PdfSeperatorGetAll();
                //string[] metadataValues = fileOriginalName.Split('_');
                List<string> metadataValues = new List<string>();

                //Extracting right panel
                itext.Rectangle rectangleRight = new itext.Rectangle(0, 500, 110, 1000);
                RenderFilter[] filterRight = { new RegionTextRenderFilter(rectangleRight) };
                string pageText = string.Empty;
                using (PdfReader pdfReader = new PdfReader(croppedFile))
                {
                    ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filterRight);
                    pageText = PdfTextExtractor.GetTextFromPage(pdfReader, pageNo, strategy);
                }
                var arrayRight = pageText.Split('\n');

                if (arrayRight != null && arrayRight.Count() > 1 &&
                    arrayRight[1].Split(':') != null)
                {
                    if (arrayRight[1].Split(':').Count() > 1)
                    {
                        metadataValues.Add(arrayRight[1].Split(':')[1].Trim()); //PO No
                    }
                    if (arrayRight[2].Split(':').Count() > 1)
                    {
                        metadataValues.Add(arrayRight[2].Split(':')[1].Trim()); //PO Date
                    }
                }

                //Extracting left panel
                itext.Rectangle rectangleLeft = new itext.Rectangle(0, 0, 220, 200);
                RenderFilter[] filterLeft = { new RegionTextRenderFilter(rectangleLeft) };
                using (PdfReader pdfReader = new PdfReader(croppedFile))
                {
                    ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filterLeft);
                    pageText = PdfTextExtractor.GetTextFromPage(pdfReader, pageNo, strategy);
                }
                var arrayLeft = pageText.Split('\n');

                foreach (string item in arrayLeft)
                {
                    if (item.ToUpper().Contains("VENDORNO"))
                    {
                        if (item.Split(':').Count() > 1)
                        {
                            metadataValues.Add(item.Split(':')[1].Trim());
                        }
                    }
                    if (item.ToUpper().Contains("GSTIN"))
                    {
                        if (item.Split(':').Count() > 1)
                        {
                            metadataValues.Add(item.Split(':')[1].Trim());
                        }
                    }
                }

                for (int metadataIndex = 0; metadataIndex < metadataValues.Count(); metadataIndex++)
                {
                    if (!string.IsNullOrEmpty(metadataValues[metadataIndex]) &&
                        (gvMetadata.Rows.Count > 0 && gvMetadata.Rows.Count > metadataIndex))
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
            catch (Exception ex)
            {
                ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            }
            return retValue;
        }
        private int UserGroupFileMapping_Save(Guid? fileGuid)
        {
            int retValue = 0;

            for (int index = 0; index < gvUserGroup.Rows.Count; index++)
            {
                Entity.UserGroup userGroup = new Entity.UserGroup
                {
                    UserGroupFileMappingId = 0,
                    UserGroupId = Convert.ToInt32(gvUserGroup.DataKeys[index].Values[0]),
                    
                    FileGuid = fileGuid,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
                    Status = (((CheckBox)gvUserGroup.Rows[index].FindControl("chkGroup")).Checked)? 1 : 0
                };
                retValue += BusinessLayer.UserGroup.UserGroupFileMapping_Save(userGroup);
            }

            return retValue;
        }
        private void Clear()
        {
            ddlFileType.SelectedIndex = 0;
            txtEntryDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            UserMessage.Text = string.Empty;
        }
        private Guid? File_Save()
        {
            Guid? retValue = null;
            string filepath = Server.MapPath("\\Files");
            Collection<string> seperatedPdfFiles = new Collection<string>();

            if (FileUploadValidate())
            {
                foreach (HttpPostedFile fileItem in FileUpload1.PostedFiles)
                {
                    Entity.File file = new Entity.File();
                    if (io.Path.GetExtension(fileItem.FileName) == ".pdf")
                    {
                        string originalFullFileName = filepath + "\\Original\\" + fileItem.FileName;
                        fileItem.SaveAs(originalFullFileName); //storing original file into ORIGINAL directory
                        seperatedPdfFiles = SegregatedFiles(io.Path.GetFileName(fileItem.FileName)); //splitting and storing into CROPPED directory
                        foreach (string pdfFile in seperatedPdfFiles)
                        {
                            file.FileTypeId = Convert.ToInt32(ddlFileType.SelectedValue);
                            file.PhysicalFileName = string.Empty;
                            file.FileOriginalName = io.Path.GetFileNameWithoutExtension(pdfFile);
                            file.FileExtension = io.Path.GetExtension(pdfFile);
                            file.EntryDate = Convert.ToDateTime(txtEntryDate.Text.Trim());
                            file.IsFullTextCopied = false;
                            file.IsAttachment = false;
                            file.MainFileGuid = null;
                            file.CreatedDate = DateTime.Now;
                            file.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                            file.FileStatus = (int)Entity.FileStatus.Active;

                            //Saving file information
                            retValue = BusinessLayer.File.FileSave(file);

                            //Uploading file
                            if (retValue != null)
                            {
                                file.PhysicalFileName = retValue + file.FileExtension;
                                //string fileWithFullPath = filepath + "\\Cropped\\" + io.Path.GetFileName(file.FileOriginalName + file.FileExtension);
                                string fileWithFullPath = pdfFile;
                                //fileItem.SaveAs(fileWithFullPath);

                                //Build the File Path for the original (input) and the encrypted (output) file.
                                string decryptedOriginalFileNameWithPath = fileWithFullPath;
                                string encryptPhysicalFileNameWithPath = filepath + "\\" + file.PhysicalFileName;
                                BusinessLayer.GeneralSecurity.EncryptFile(decryptedOriginalFileNameWithPath, encryptPhysicalFileNameWithPath);

                                //Saving metadata
                                if (IsMetadataManual())
                                {
                                    MetadataFileMapping_Save(retValue);
                                }
                                else
                                {
                                    MetadataFileMapping_Save_pdf(retValue, pdfFile);
                                }

                                //Saving user group
                                UserGroupFileMapping_Save(retValue);

                                //Delete the original cropped(input) file.
                                System.IO.File.Delete(decryptedOriginalFileNameWithPath);
                            }
                        }
                        //Delete the original full(input) file.
                        System.IO.File.Delete(originalFullFileName);
                    }
                    else
                    {
                        file.FileTypeId = Convert.ToInt32(ddlFileType.SelectedValue);
                        file.PhysicalFileName = string.Empty;
                        file.FileOriginalName = io.Path.GetFileNameWithoutExtension(fileItem.FileName);
                        file.FileExtension = io.Path.GetExtension(fileItem.FileName);
                        file.EntryDate = Convert.ToDateTime(txtEntryDate.Text.Trim());
                        file.IsFullTextCopied = false;
                        file.IsAttachment = false;
                        file.MainFileGuid = null;
                        file.CreatedDate = DateTime.Now;
                        file.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                        file.FileStatus = (int)Entity.FileStatus.Active;

                        //Saving file information
                        retValue = BusinessLayer.File.FileSave(file);

                        //Uploading file
                        if (retValue != null)
                        {
                            file.PhysicalFileName = retValue + file.FileExtension;

                            string fileWithFullPath = filepath + "\\" + io.Path.GetFileName(file.FileOriginalName + file.FileExtension);

                            fileItem.SaveAs(fileWithFullPath);

                            //Build the File Path for the original (input) and the encrypted (output) file.
                            string decryptedOriginalFileNameWithPath = fileWithFullPath;
                            string encryptPhysicalFileNameWithPath = filepath + "\\" + file.PhysicalFileName;
                            BusinessLayer.GeneralSecurity.EncryptFile(decryptedOriginalFileNameWithPath, encryptPhysicalFileNameWithPath);
                            //Delete the original (input) file.
                            System.IO.File.Delete(decryptedOriginalFileNameWithPath);
                        }

                        //Saving metadata
                        if (IsMetadataManual())
                        {
                            MetadataFileMapping_Save(retValue);
                        }
                        else
                        {
                            MetadataFileMapping_Save(retValue, file.FileOriginalName);
                        }

                        //Saving user group
                        UserGroupFileMapping_Save(retValue);
                    }
                }
            }
            return retValue;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileCategory_GetAll();
                FileType_GetAll();
                UserGroup_GetAll();
                Clear();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (File_Save().HasValue)
            {
                Clear();
                UserMessage.Text = "Saved.";
                UserMessage.Css = BusinessLayer.MessageCssClass.Success;
            }
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
        //protected void chklUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int indexOne = Convert.ToInt32(Request.Form["__EVENTTARGET"].Substring(Request.Form["__EVENTTARGET"].LastIndexOf('$') + 1));
        //    if (indexOne == 0)
        //    {
        //        bool isChecked = chklUserGroup.Items[indexOne].Selected ? true : false;
        //        foreach (ListItem item in chklUserGroup.Items)
        //        {
        //            item.Selected = isChecked;
        //        }
        //    }
        //}
        protected void chkGroupHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkGroupHeader = (CheckBox)sender;
            foreach (GridViewRow gvr in gvUserGroup.Rows)
            {
                ((CheckBox)gvr.FindControl("chkGroup")).Checked = chkGroupHeader.Checked;
            }
        }
        protected void chkGroup_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkGroup = (CheckBox)sender;
            if (!chkGroup.Checked)
            {
                ((CheckBox)gvUserGroup.HeaderRow.FindControl("chkGroupHeader")).Checked = false;
            }
        }
    }
}