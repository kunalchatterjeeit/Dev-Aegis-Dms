using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Api.Dms.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Index(Entity.Auth model)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    object token = null;
                    DataTable userDetails = GeneralSecurity.LogOn(model.Username.Trim());
                    if (userDetails != null && userDetails.Rows.Count > 0)
                    {
                        string passowrd = userDetails.Rows[0][1].ToString();
                        if (passowrd.Equals(model.Password.Trim().EncodePasswordToBase64()))
                        {
                            model.Token = new Authentication().GetToken(Request, userDetails.Rows[0]["UserId"].ToString());
                            model.Status = Entity.LoginStatus.Success;
                            responseMessage = Request.CreateResponse(HttpStatusCode.OK, model);
                        }
                        else
                        {
                            model.Status = Entity.LoginStatus.WrongPassword;
                            model.Message = "Invalid Username/Password";
                            responseMessage = Request.CreateResponse(HttpStatusCode.OK, model);
                        }
                    }
                    else
                    {
                        model.Status = Entity.LoginStatus.Failed;
                        model.Message = "Invalid Username/Password";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, model);
                    }
                }
            }
            catch (Exception ex)
            {
                model.Status = Entity.LoginStatus.Failed;
                model.Message = ex.Message;
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, model);
            }
            return responseMessage;
        }
    }
}
