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
using Entity;

namespace AegisDMS
{
    public partial class FileAutoFormat : System.Web.UI.Page
    {
        public string _LastMasterNumber
        {
            get { return (Session["LastMasterNumber"] != null) ? Convert.ToString(Session["LastMasterNumber"]) : string.Empty; }
            set { Session["LastMasterNumber"] = value; }
        }
        public string _LastDocumentType
        {
            get { return (Session["LastDocumentType"] != null) ? Convert.ToString(Session["LastDocumentType"]) : string.Empty; }
            set { Session["LastDocumentType"] = value; }
        }

        private void FileCategory_GetAll()
        {
            DataTable dtFileCategory = BusinessLayer.FileCategory.FileCategoryGetAll(new Entity.FileCategory());
            ddlFileCategory.DataSource = dtFileCategory;
            ddlFileCategory.DataTextField = "Name";
            ddlFileCategory.DataValueField = "FileCategoryId";
            ddlFileCategory.DataBind();

            ddlFileCategory.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "0", Selected = true });
        }
        private DataTable FileType_GetAll(int fileCategoryId)
        {
            DataTable dtFileType = BusinessLayer.FileType.GetAll(new Entity.FileType()
            {
                FileCategoryId = Convert.ToInt32(fileCategoryId)
            });
            return dtFileType;
        }
        private DataTable Metadata_GetAll(int fileTypeId)
        {
            Entity.Metadata metadata = new Entity.Metadata()
            {
                FileTypeId = Convert.ToInt32(fileTypeId)
            };
            return BusinessLayer.FileMetadata.MetadataGetAll(metadata);
        }
        private Collection<SeperatedFile> SegregatedFiles(string fileName)
        {
            Collection<SeperatedFile> seperatedFiles = new Collection<SeperatedFile>();
            DataTable pdfSeperator = BusinessLayer.File.PdfSeperatorGetAll();
            string filePath = Server.MapPath("\\Files\\Original\\") + fileName;

            if (pdfSeperator != null && pdfSeperator.Rows.Count > 0)
            {
                itext.Rectangle rectangle = new itext.Rectangle(0, 500, 110, 1000);
                RenderFilter[] filter = { new RegionTextRenderFilter(rectangle) };
                using (PdfReader pdfReader = new PdfReader(filePath))
                {
                    List<int> dumpPages = null;
                    for (int pageNo = 1; pageNo <= pdfReader.NumberOfPages; pageNo++)
                    {
                        ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
                        string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, pageNo, strategy);
                        var array = pageText.Split('\n');

                        bool isSeperatorMatched = false;
                        foreach (DataRow drSeperator in pdfSeperator.Rows)
                        {
                            if (array.Contains(drSeperator["Phrase"].ToString()))
                            {
                                seperatedFiles.Add(PdfCreator(array, pageNo - 1, pageNo - 1, filePath, fileName));
                                isSeperatorMatched = true;
                                break;
                            }
                        }

                        if (!isSeperatorMatched)
                        {
                            if (dumpPages == null)
                            {
                                dumpPages = new List<int>();
                            }
                            dumpPages.Add(pageNo);
                        }

                        if (dumpPages != null && dumpPages.Count() > 0 && (isSeperatorMatched || pageNo == pdfReader.NumberOfPages))
                        {
                            seperatedFiles.Add(PdfCreator(array, dumpPages[0] - 1, dumpPages[dumpPages.Count() - 1] - 1, filePath, fileName));
                            dumpPages = null;
                        }
                    }
                }

                if (!seperatedFiles.Any())
                {
                    seperatedFiles.Add(new SeperatedFile() { FileName = filePath, FileTypeId = 23 }); //Dump file type
                }
            }
            return seperatedFiles;
        }
        private SeperatedFile PdfCreator(string[] array, int startPage, int lastPage, string mainFilePath, string fileName)
        {
            SeperatedFile newPdf = new SeperatedFile();
            DataTable pdfSeperator = BusinessLayer.File.PdfSeperatorGetAll();

            foreach (DataRow drSeperator in pdfSeperator.Rows)
            {
                if (array.Contains(drSeperator["Phrase"].ToString()))
                {
                    string item = array.Where(p => p.Equals(drSeperator["Phrase"].ToString())).FirstOrDefault();
                    DataRow drPdfSeperator = pdfSeperator.Select("Phrase = '" + item + "'").FirstOrDefault();
                    DataRow drFileType = FileType_GetAll(Convert.ToInt32(ddlFileCategory.SelectedValue))
                                            .Select("Name = '" + drPdfSeperator["pdfType"].ToString() + "'").FirstOrDefault();
                    //DataTable metaData = Metadata_GetAll(Convert.ToInt32(drFileType["FileTypesId"].ToString()));
                    if (drFileType != null)
                    {
                        newPdf.FileTypeId = Convert.ToInt32(drFileType["FileTypeId"].ToString());
                        _LastDocumentType = Convert.ToString(newPdf.FileTypeId);
                    }
                    break;
                }
            }

            //For Dump in Master Type
            int fileType = 0;
            if (newPdf.FileTypeId == 0 && int.TryParse(_LastDocumentType, out fileType))
            {
                newPdf.FileTypeId = fileType;
            }

            //For full Dump but Master File Category Selected
            if (string.IsNullOrEmpty(_LastDocumentType))
            {
                newPdf.FileTypeId = 23;
            }

            newPdf.FirstPageIndex = startPage + 1;
            using (psPdf.PdfDocument mainDoc = new psPdf.PdfDocument())
            {
                using (psPdf.PdfDocument mainPdfCopy = ps.PdfReader.Open(mainFilePath, ps.PdfDocumentOpenMode.Import))
                {
                    for (int pageNo = startPage; pageNo <= lastPage; pageNo++)
                    {
                        mainDoc.AddPage(mainPdfCopy.Pages[pageNo]);
                    }
                    fileName = io.Path.GetFileNameWithoutExtension(fileName) + "_" + startPage.ToString() + "_" + lastPage.ToString() + ".pdf";
                    newPdf.FileName = Server.MapPath("\\Files\\Cropped\\") + fileName;
                    mainDoc.Save(newPdf.FileName);
                }
            }
            return newPdf;
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
            foreach (HttpPostedFile fileItem in FileUpload1.PostedFiles)
            {
                if (io.Path.GetExtension(fileItem.FileName) != ".pdf")
                {
                    UserMessage.Text = "Please select only pdf.";
                    UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtEntryDate.Text.Trim()))
            {
                UserMessage.Text = "Please select entry date.";
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
                    if (((CheckBox)gvUserGroup.Rows[index].FindControl("chkGroup")).Checked)
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
                    Status = (((CheckBox)gvUserGroup.Rows[index].FindControl("chkGroup")).Checked) ? 1 : 0
                };
                retValue += BusinessLayer.UserGroup.UserGroupFileMapping_Save(userGroup);
            }

            return retValue;
        }
        private int MetadataFileMapping_Save_pdf(Guid? fileGuid, SeperatedFile croppedFile)
        {
            int retValue = 0;
            int croppedPageNo = 1;
            try
            {
                DataTable pdfSeperator = BusinessLayer.File.PdfSeperatorGetAll();
                //string[] metadataValues = fileOriginalName.Split('_');
                List<string> metadataValues = new List<string>();

                //Extracting right panel
                itext.Rectangle rectangleRight = new itext.Rectangle(0, 500, 110, 1000);
                RenderFilter[] filterRight = { new RegionTextRenderFilter(rectangleRight) };
                string pageText = string.Empty;
                using (PdfReader pdfReader = new PdfReader(croppedFile.FileName))
                {
                    ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filterRight);
                    pageText = PdfTextExtractor.GetTextFromPage(pdfReader, croppedPageNo, strategy);
                }
                var arrayRight = pageText.Split('\n');

                if (arrayRight != null && arrayRight.Count() > 1 &&
                    arrayRight[1].Split(':') != null)
                {
                    if (arrayRight[1].Split(':').Count() > 1)
                    {
                        metadataValues.Add(arrayRight[1].Split(':')[1].Trim()); //PO No

                        //Saving for Dump documents
                        _LastDocumentType = Convert.ToString(croppedFile.FileTypeId);
                        _LastMasterNumber = Convert.ToString(arrayRight[1].Split(':')[1].Trim());
                    }
                    if (arrayRight[2].Split(':').Count() > 1)
                    {
                        metadataValues.Add(arrayRight[2].Split(':')[1].Trim()); //PO Date
                    }
                }

                if (metadataValues.Count() == 0)//If does not containing Master Number then will be treated as dump record
                {
                    if (_LastMasterNumber != null && !string.IsNullOrEmpty(_LastMasterNumber))
                    {
                        metadataValues.Add(_LastMasterNumber);
                    }
                }

                //Extracting left panel
                itext.Rectangle rectangleLeft = new itext.Rectangle(0, 0, 220, 200);
                RenderFilter[] filterLeft = { new RegionTextRenderFilter(rectangleLeft) };
                using (PdfReader pdfReader = new PdfReader(croppedFile.FileName))
                {
                    ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filterLeft);
                    pageText = PdfTextExtractor.GetTextFromPage(pdfReader, croppedPageNo, strategy);
                }
                var arrayLeft = pageText.Split('\n');

                foreach (string item in arrayLeft)
                {
                    if (item.ToUpper().Contains("VENDORNO") || item.ToUpper().Contains("VENDOR NO"))
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

                DataTable metadata = Metadata_GetAll(croppedFile.FileTypeId);
                for (int metadataIndex = 0; metadataIndex < metadataValues.Count(); metadataIndex++)
                {
                    if (!string.IsNullOrEmpty(metadataValues[metadataIndex]) &&
                        (metadata.Rows.Count > 0 && metadata.Rows.Count > metadataIndex))
                    {
                        Entity.MetadataFileMapping metadataFileMapping = new Entity.MetadataFileMapping()
                        {
                            MetadataId = Convert.ToInt64(metadata.Rows[metadataIndex]["MetaDataId"].ToString()),
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
        private void Clear()
        {
            txtEntryDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            UserMessage.Text = string.Empty;
        }
        private Guid? File_Save()
        {
            Guid? retValue = null;
            string filepath = Server.MapPath("\\Files");
            Collection<SeperatedFile> seperatedPdfFiles = new Collection<SeperatedFile>();

            if (FileUploadValidate())
            {
                foreach (HttpPostedFile fileItem in FileUpload1.PostedFiles)
                {
                    Entity.File file = new Entity.File();
                    _LastDocumentType = string.Empty;
                    _LastMasterNumber = string.Empty;

                    string originalFullFileName = filepath + "\\Original\\" + fileItem.FileName;
                    fileItem.SaveAs(originalFullFileName); //storing original file into ORIGINAL directory
                    seperatedPdfFiles = SegregatedFiles(io.Path.GetFileName(fileItem.FileName)); //splitting and storing into CROPPED directory
                    foreach (SeperatedFile pdfFile in seperatedPdfFiles)
                    {
                        file.FileTypeId = pdfFile.FileTypeId;
                        file.PhysicalFileName = string.Empty;
                        file.FileOriginalName = io.Path.GetFileNameWithoutExtension(pdfFile.FileName);
                        file.FileExtension = io.Path.GetExtension(pdfFile.FileName);
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
                            string fileWithFullPath = pdfFile.FileName;
                            //fileItem.SaveAs(fileWithFullPath);

                            //Build the File Path for the original (input) and the encrypted (output) file.
                            string decryptedOriginalFileNameWithPath = fileWithFullPath;
                            string encryptPhysicalFileNameWithPath = filepath + "\\" + file.PhysicalFileName;
                            BusinessLayer.GeneralSecurity.EncryptFile(decryptedOriginalFileNameWithPath, encryptPhysicalFileNameWithPath);

                            //Saving metadata                                
                            MetadataFileMapping_Save_pdf(retValue, pdfFile);

                            //Saving user group
                            UserGroupFileMapping_Save(retValue);

                            //Delete the original cropped(input) file.
                            System.IO.File.Delete(decryptedOriginalFileNameWithPath);
                        }
                    }
                    //Delete the original full(input) file.
                    System.IO.File.Delete(originalFullFileName);
                }
            }
            return retValue;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileCategory_GetAll();
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