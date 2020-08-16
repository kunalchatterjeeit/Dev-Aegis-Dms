using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Dms.Controllers
{
    public class RoleController : ApiController
    {

        [HttpGet]
        [JwtAuthorization(Entity.Utility.ROLEPERMISSION)]
        public HttpResponseMessage RoleGetAll()
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                int roleId = 0;
                List<Entity.Role> roles = new BusinessLayer.Role().RoleGetAll(roleId);
                response.ResponseData = roles;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                new BusinessLayer.Logger().LogException(ex, "RoleGetAll");
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.USERROLE)]
        public HttpResponseMessage UserRoleCreate(Entity.Role model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.Role().RoleSave(model);
                    if (model.RoleId == 0)
                        response.Message = "User role created.";
                    else
                        response.Message = "User role updated.";
                    response.ResponseCode = (int)ResponseCode.Success;
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                new BusinessLayer.Logger().LogException(ex, "UserRoleCreate");
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.USERROLE)]
        public HttpResponseMessage RoleGetById(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.Role> roles = new BusinessLayer.Role().RoleGetAll(id);
                response.ResponseData = roles;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                new BusinessLayer.Logger().LogException(ex, "RoleGetById");
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.USERROLE)]
        public HttpResponseMessage RoleDelete(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                int retValue = new BusinessLayer.Role().RoleDelete(id);
                if (retValue > 0)
                    response.Message = "User role deleted.";
                response.ResponseCode = (int)ResponseCode.Success;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                new BusinessLayer.Logger().LogException(ex, "RoleDelete");
            }
            return responseMessage;
        }

        [HttpGet]
        [JwtAuthorization(Entity.Utility.ROLEPERMISSION)]
        public HttpResponseMessage RolePermissionGetByRoleId(int id)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                List<Entity.RolePermission> rolePermissions = new BusinessLayer.Role().RolePermissionGetByRoleId(id);
                response.ResponseData = rolePermissions;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                new BusinessLayer.Logger().LogException(ex, "RolePermissionGetByRoleId");
            }
            return responseMessage;
        }

        [HttpPost]
        [JwtAuthorization(Entity.Utility.ROLEPERMISSION)]
        public HttpResponseMessage RolePermissionSave(Entity.RolePermission model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    int retValue = new BusinessLayer.Role().RolePermissionSave(model.RoleId, model.PermissionId, model.IsEnabled);

                    response.Message = "Role permission updated.";
                    response.ResponseCode = (int)ResponseCode.Success;
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = (int)ResponseCode.CriticalCode;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                new BusinessLayer.Logger().LogException(ex, "RolePermissionSave");
            }
            return responseMessage;
        }
    }
}