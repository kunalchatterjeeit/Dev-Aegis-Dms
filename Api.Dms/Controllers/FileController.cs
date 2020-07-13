using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Dms.Controllers
{
    public class FileController : ApiController
    {
        [HttpGet]
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
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        public HttpResponseMessage FileCategoryGetById(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.FileCategory> roles = new BusinessLayer.FileCategory().FileCategoryGetAll(new Entity.FileCategory() {
                    FileCategoryId = id
                });
                response.ResponseData = roles;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
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
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
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
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
        public HttpResponseMessage FileTypeGetByFileCategoryId(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.FileType> fileTypes = new BusinessLayer.FileType().GetAll(new Entity.FileType() {
                    FileCategoryId = id
                });
                response.ResponseData = fileTypes;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
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
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }

        [HttpGet]
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
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            return responseMessage;
        }
    }
}