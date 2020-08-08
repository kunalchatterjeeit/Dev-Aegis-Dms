using BusinessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.Dms.Controllers
{
    public class FileController : ApiController
    {
        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILECATEGORY)]
        public HttpResponseMessage FileCategoryGetAll()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.FileCategory> fileCategories = new BusinessLayer.FileCategory().FileCategoryGetAll(new Entity.FileCategory() { });
                response.ResponseData = fileCategories;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.FILECATEGORY)]
        public HttpResponseMessage FileCategoryCreate(Entity.FileCategory model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.FileCategory().FileCategorySave(model);
                    if (model.FileCategoryId == 0)
                        response.Message = "File category created.";
                    else
                        response.Message = "File category updated.";
                    response.ResponseCode = (int)ResponseCode.Success;
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILECATEGORY)]
        public HttpResponseMessage FileCategoryGetById(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.FileCategory> roles = new BusinessLayer.FileCategory().FileCategoryGetAll(new Entity.FileCategory()
                {
                    FileCategoryId = id
                });
                response.ResponseData = roles;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.FILECATEGORY)]
        public HttpResponseMessage FileCategoryDelete(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                int retValue = new BusinessLayer.FileCategory().FileCategoryDelete(id);
                if (retValue > 0)
                    response.Message = "File category deleted.";
                response.ResponseCode = (int)ResponseCode.Success;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILETYPE)]
        public HttpResponseMessage FileTypeGetAll()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.FileType> fileTypes = new BusinessLayer.FileType().GetAll(new Entity.FileType() { });
                response.ResponseData = fileTypes;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILETYPE)]
        public HttpResponseMessage FileTypeGetByFileCategoryId(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.FileType> fileTypes = new BusinessLayer.FileType().GetAll(new Entity.FileType()
                {
                    FileCategoryId = id
                });
                response.ResponseData = fileTypes;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.FILETYPE)]
        public HttpResponseMessage FileTypeCreate(Entity.FileType model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.FileType().Save(model);
                    if (model.FileTypeId == 0)
                        response.Message = "File type created.";
                    else
                        response.Message = "File type updated.";
                    response.ResponseCode = (int)ResponseCode.Success;
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.FILETYPE)]
        public HttpResponseMessage FileTypeGetById(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.FileType> roles = new BusinessLayer.FileType().GetAll(new Entity.FileType()
                {
                    FileTypeId = id
                });
                response.ResponseData = roles;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.FILETYPE)]
        public HttpResponseMessage FileTypeDelete(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                int retValue = new BusinessLayer.FileType().FileTypeDelete(id);
                if (retValue > 0)
                    response.Message = "File type deleted.";
                response.ResponseCode = (int)ResponseCode.Success;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.FILE)]
        public HttpResponseMessage Upload()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                string physicalPath = HttpContext.Current.Server.MapPath("~/RawFiles/");
                Guid? retValue = null;
                var httpRequest = HttpContext.Current.Request;


                if (httpRequest.Files.Count > 0)
                {
                    string fileName = string.Empty;
                    string entryDate = string.Empty;
                    string fileTypeId = string.Empty;
                    string metadataJsonString = string.Empty;
                    string userGroupJsonString = string.Empty;
                    int fileSize = httpRequest.Files["myFile"].ContentLength;

                    var postedFile = httpRequest.Files["myFile"];
                    fileName = postedFile.FileName;
                    fileTypeId = httpRequest["fileTypeId"];
                    entryDate = httpRequest["entryDate"];
                    metadataJsonString = httpRequest["metadata"];
                    userGroupJsonString = httpRequest["userGroup"];

                    List<Entity.Metadata> metadatas = JsonConvert.DeserializeObject<List<Entity.Metadata>>(metadataJsonString.Replace("%22", "'"));
                    List<string> userGroups = userGroupJsonString.Split(',').ToList();

                    Entity.File file = new Entity.File()
                    {
                        FileTypeId = Convert.ToInt32(fileTypeId),
                        PhysicalFileName = string.Empty,
                        FileOriginalName = Path.GetFileNameWithoutExtension(postedFile.FileName),
                        FileExtension = Path.GetExtension(postedFile.FileName),
                        EntryDate = Convert.ToDateTime(entryDate.Trim()),
                        IsFullTextCopied = false,
                        IsAttachment = false,
                        MainFileGuid = null,
                        CreatedDate = DateTime.Now,
                        CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
                        FileStatus = (int)Entity.FileStatus.Active,
                        SizeInKb = fileSize / 1000
                    };

                    //Saving file information
                    retValue = new BusinessLayer.File().FileSave(file);

                    string filePath = physicalPath + postedFile.FileName;
                    string newEncryptedFileNameAndPath = retValue + file.FileExtension;
                    postedFile.SaveAs(filePath);

                    file.PhysicalFileName = newEncryptedFileNameAndPath;

                    FileEncrypting(physicalPath, file, filePath);
                    MetadataSaving(retValue, file, metadatas, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                    //Saving user group
                    UserGroupFileMapping_Save(retValue, userGroups);

                    if (retValue != null)
                    {
                        response.Message = postedFile.FileName + " upload completed.";
                        response.ResponseCode = (int)ResponseCode.Success;
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                }
                else
                {
                    response.Message = "No file uploaded.";
                    response.ResponseCode = (int)ResponseCode.CriticalCode;
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.FILE)]
        public HttpResponseMessage Search(Entity.Search model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.SearchResult> files = new List<Entity.SearchResult>();
                if (model.SearchMode == Entity.SearchModeEnum.QuickSearch)
                    files = new BusinessLayer.File().FileSearchByPhrase(model.SearchString.Trim(), Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                else
                    files = new BusinessLayer.File().PrepareAdvanceSearch(model.Metadatas, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                response.ResponseData = files;
                response.ResponseCode = (int)ResponseCode.Success;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.FILE)]
        public HttpResponseMessage GetFilePath(Entity.File model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                Entity.File file = new BusinessLayer.File().FileGetByFileGuid(model.FileGuid.ToString());
                file.FileCompletePath = HttpContext.Current.Server.MapPath("~/RawFiles/Raw/") + file.FileOriginalName + file.FileExtension;
                string encryptedPhysicalFileNameWithPath = HttpContext.Current.Server.MapPath("~/RawFiles/") + file.PhysicalFileName + file.FileExtension;
                string decryptOriginalFileNameWithPath = HttpContext.Current.Server.MapPath("~/RawFiles/Raw/") + file.FileOriginalName + file.FileExtension;

                BusinessLayer.GeneralSecurity.DecryptFile(encryptedPhysicalFileNameWithPath, decryptOriginalFileNameWithPath);
                string runningOn = (string)new AppSettingsReader().GetValue("RunningOn", typeof(string));
                switch (file.FileExtension)
                {
                    case ".doc":
                        if (runningOn == "Server")
                            file.FileCompletePath = "https://docs.google.com/viewerng/viewer?url="
                                + (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string))
                                + "RawFiles/Raw/" + file.FileOriginalName + file.FileExtension + "&embedded=true";
                        else
                            file.FileCompletePath = (string)BusinessLayer.RenderFiles.ReadMicrosoftWordByOpenXml(decryptOriginalFileNameWithPath);
                        break;
                    case ".docx":
                        if (runningOn == "Server")
                            file.FileCompletePath = "https://docs.google.com/viewerng/viewer?url="
                                + (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string))
                                + "RawFiles/Raw/" + file.FileOriginalName + file.FileExtension + "&embedded=true";
                        else
                            file.FileCompletePath = (string)BusinessLayer.RenderFiles.ReadMicrosoftWordByOpenXml(decryptOriginalFileNameWithPath);
                        break;
                    case ".xls":
                        if (runningOn == "Server")
                            file.FileCompletePath = "https://docs.google.com/viewerng/viewer?url="
                                + (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string))
                                + "RawFiles/Raw/" + file.FileOriginalName + file.FileExtension + "&embedded=true";
                        else
                            file.FileCompletePath = (string)BusinessLayer.RenderFiles.ReadMicrosoftExcelByOfficeInterop(decryptOriginalFileNameWithPath);
                        break;
                    case ".xlsx":
                        if (runningOn == "Server")
                            file.FileCompletePath = "https://docs.google.com/viewerng/viewer?url="
                                + (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string))
                                + "RawFiles/Raw/" + file.FileOriginalName + file.FileExtension + "&embedded=true";
                        else
                            file.FileCompletePath = (string)BusinessLayer.RenderFiles.ReadMicrosoftExcelByOfficeInterop(decryptOriginalFileNameWithPath);
                        break;
                    case ".ppt":
                        if (runningOn == "Server")
                            file.FileCompletePath = "https://docs.google.com/viewerng/viewer?url="
                                + (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string))
                                + "RawFiles/Raw/" + file.FileOriginalName + file.FileExtension + "&embedded=true";
                        file.FileCompletePath = (string)BusinessLayer.RenderFiles.ReadMicrosoftWordByOfficeInterop(decryptOriginalFileNameWithPath);
                        break;
                    case ".pptx":
                        if (runningOn == "Server")
                            file.FileCompletePath = "https://docs.google.com/viewerng/viewer?url="
                                + (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string))
                                + "RawFiles/Raw/" + file.FileOriginalName + file.FileExtension + "&embedded=true";
                        else
                            file.FileCompletePath = (string)BusinessLayer.RenderFiles.ReadMicrosoftWordByOfficeInterop(decryptOriginalFileNameWithPath);
                        break;
                    default:
                        file.FileCompletePath = (string)new AppSettingsReader().GetValue("BaseUrl", typeof(string)) + "RawFiles/Raw/" + file.FileOriginalName + file.FileExtension;
                        break;
                }
                response.ResponseData = file;
                response.ResponseCode = (int)ResponseCode.Success;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
            return responseMessage;
        }

        private static void MetadataSaving(Guid? retValue, Entity.File file, List<Entity.Metadata> metadatas, int userId)
        {
            BusinessLayer.Metadata metadata = new Metadata();
            //Saving metadata
            if (metadata.IsMetadataManual(metadatas))
            {
                metadata.MetadataFileMapping_Save(retValue, metadatas, userId);
            }
            else
            {
                metadata.MetadataFileMapping_Save(retValue, metadatas, userId, file.FileOriginalName);
            }
        }

        private static void FileEncrypting(string physicalPath, Entity.File file, string filePath)
        {
            //Build the File Path for the original (input) and the encrypted (output) file.
            string decryptedOriginalFileNameWithPath = filePath;
            string encryptPhysicalFileNameWithPath = physicalPath + "\\" + file.PhysicalFileName;
            BusinessLayer.GeneralSecurity.EncryptFile(decryptedOriginalFileNameWithPath, encryptPhysicalFileNameWithPath);
            //Delete the original (input) file.
            System.IO.File.Delete(decryptedOriginalFileNameWithPath);
        }

        private int UserGroupFileMapping_Save(Guid? fileGuid, List<string> userGroups)
        {
            int retValue = 0;

            foreach (string ug in userGroups)
            {
                Entity.UserGroup userGroup = new Entity.UserGroup
                {
                    UserGroupFileMappingId = 0,
                    UserGroupId = Convert.ToInt32(ug),
                    FileGuid = fileGuid,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
                    Status = 1
                };
                retValue += BusinessLayer.UserGroup.UserGroupFileMapping_Save(userGroup);
            }

            return retValue;
        }
    }
}