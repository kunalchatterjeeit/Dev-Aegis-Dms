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
    public class HomeController : ApiController
    {
        [HttpGet]
        [JwtAuthorization(Entity.Utility.USER)]
        public HttpResponseMessage Index()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Called index");
        }
    }
}
