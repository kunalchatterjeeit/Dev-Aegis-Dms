using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Api.Dms.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Index(Entity.Auth model)
        {
            Entity.HttpResponse response = new Entity.HttpResponse();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    string runningOn = (string)new AppSettingsReader().GetValue("RunningOn", typeof(string));
                    if (runningOn == "Local")
                        BusinessLayer.GeneralSecurity.ValidateMac();
                    if (Convert.ToDateTime("2020-08-10") > DateTime.Now.AddDays(15))
                        throw new Exception("Problem in authenticating. Please contact support.");
                    DataTable userDetails = GeneralSecurity.LogOn(model.Username.Trim());
                    if (userDetails != null && userDetails.Rows.Count > 0)
                    {
                        string passowrd = userDetails.Rows[0][1].ToString();
                        if (passowrd.Equals(model.Password.EncodePasswordToBase64()))
                        {
                            int userId = Convert.ToInt32(userDetails.Rows[0]["UserId"].ToString());
                            string roles = GeneralSecurity.Permission_ByRoleId(userId);
                            model.Token = new Authentication().GetToken(Request, userId.ToString());
                            model.Status = Entity.LoginStatus.Success;
                            model.Roles = roles.Split(',').ToArray();
                            response.ResponseData = model;
                            responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                        else
                        {
                            response.ResponseCode = (int)ResponseCode.Failed;
                            response.Message = "Invalid Username/Password";
                            responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                    }
                    else
                    {
                        response.ResponseCode = (int)ResponseCode.Failed;
                        response.Message = "Invalid Username/Password";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                    }
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
    }
}
