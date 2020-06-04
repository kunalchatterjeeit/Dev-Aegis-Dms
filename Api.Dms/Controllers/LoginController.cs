using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Dms.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Index(Entity.Auth model)
        {
            if (ModelState.IsValid && model != null)
            {
                object token = null;
                DataTable userDetails = GeneralSecurity.LogOn(model.Username.Trim());
                if (userDetails != null && userDetails.Rows.Count > 0)
                {
                    string passowrd = userDetails.Rows[0][1].ToString();
                    if (passowrd.Equals(model.Password.Trim().ToEncrypt(true)))
                    {
                        token = new Authentication().GetToken(Request, userDetails.Rows[0]["UserId"].ToString());
                        return Request.CreateResponse(HttpStatusCode.OK, token);
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Invalid Username/Password");
        }
    }
}
