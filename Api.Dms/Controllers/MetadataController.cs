using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Dms.Controllers
{
    public class MetadataController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage MetadataGetAll()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.Metadata> FileMetadatas = new BusinessLayer.FileMetadata().MetadataGetAll(new Entity.Metadata() { });
                response.ResponseData = FileMetadatas;
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
        [JwtAuthorization(Entity.Utility.METADATA)]
        public HttpResponseMessage MetadataCreate(Entity.Metadata model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.FileMetadata().MetadataSave(model);
                    if (model.MetadataId == 0)
                        response.Message = "Metadata created.";
                    else
                        response.Message = "Metadata updated.";
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
        public HttpResponseMessage MetadataGetById(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.Metadata> roles = new BusinessLayer.FileMetadata().MetadataGetAll(new Entity.Metadata()
                {
                    MetadataId = id
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
        [JwtAuthorization(Entity.Utility.METADATA)]
        public HttpResponseMessage MetadataDelete(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                int retValue = new BusinessLayer.FileMetadata().MetadataDelete(id);
                if (retValue > 0)
                    response.Message = "Metadata deleted.";
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