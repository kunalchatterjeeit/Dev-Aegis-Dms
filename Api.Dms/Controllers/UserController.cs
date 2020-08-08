using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Dms.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [JwtAuthorization(Entity.Utility.USER)]
        public HttpResponseMessage DesignationGetAll()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.Designation> designations = new BusinessLayer.Designation().Designation_GetAll();
                response.ResponseData = designations;
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
        [JwtAuthorization(Entity.Utility.USER)]
        public HttpResponseMessage UserGetAll(Entity.User model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    List<Entity.User> users = new BusinessLayer.User().UserGetAll(model);
                    response.ResponseData = users;
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

        [HttpPost]
        [JwtAuthorization(Entity.Utility.USER)]
        public HttpResponseMessage UserCreate(Entity.User model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    model.Password = string.IsNullOrEmpty(model.Password) ? model.Password : model.Password.EncodePasswordToBase64();
                    int retValue = new BusinessLayer.User().UserSave(model);
                    Entity.User newUser = new BusinessLayer.User().UserGetAll(new Entity.User()
                    {
                        Username = model.Username
                    }).FirstOrDefault();

                    if (retValue > 0)
                    {
                        foreach (int userGroupId in model.SelectedUserGroups)
                            new BusinessLayer.UserGroup().UserGroupUserMapping_Save(new Entity.UserGroup()
                            {
                                UserGroupId = userGroupId,
                                UserId = newUser.UserId,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = model.CreatedBy,
                                Status = 1
                            });
                        foreach (int roleId in model.SelectedUserRoles)
                            new BusinessLayer.User().UserRole_Save(newUser.UserId, roleId, true);
                    }

                    if (model.UserId == 0)
                        response.Message = "User profile created.";
                    else
                        response.Message = "User profile updated.";
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

        [HttpPost]
        [JwtAuthorization(Entity.Utility.USER)]
        public HttpResponseMessage UserActiveChange(Entity.User model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.User().UserStatusChange(model.UserId, (int)model.Status, model.CreatedBy);
                    response.Message = "User updated.";
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

        [HttpPost]
        [JwtAuthorization(Entity.Utility.USER)]
        public HttpResponseMessage UserLoginChange(Entity.User model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.User().User_LoginStatusChange(model.UserId, (int)model.LoginStatus, model.CreatedBy);
                    response.Message = "User updated.";
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
        [JwtAuthorization(Entity.Utility.USERGROUP)]
        public HttpResponseMessage UserGroupGetAll()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.UserGroup> userGroups = new BusinessLayer.UserGroup().UserGroupGetAll(new Entity.UserGroup());
                response.ResponseData = userGroups;
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
        [JwtAuthorization(Entity.Utility.USERGROUP)]
        public HttpResponseMessage UserGroupCreate(Entity.UserGroup model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.UserGroup().UserGroupSave(model);
                    if (model.UserGroupId == 0)
                        response.Message = "User group created.";
                    else
                        response.Message = "User group updated.";
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
        [JwtAuthorization(Entity.Utility.USERGROUP)]
        public HttpResponseMessage UserGroupGetById(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.UserGroup> userGroups = new BusinessLayer.UserGroup().UserGroupGetAll(new Entity.UserGroup()
                {
                    UserGroupId = id
                });
                response.ResponseData = userGroups;
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
        [JwtAuthorization(Entity.Utility.USERGROUP)]
        public HttpResponseMessage UserGroupDelete(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                int retValue = new BusinessLayer.UserGroup().UserGroupDelete(id);
                if (retValue > 0)
                    response.Message = "User group deleted.";
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

    }
}