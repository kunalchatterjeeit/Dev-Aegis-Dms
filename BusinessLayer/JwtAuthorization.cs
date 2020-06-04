using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace BusinessLayer
{
    public class JwtAuthorization : AuthorizeAttribute
    {
        private new List<string> Roles;
        public JwtAuthorization(params string[] roles)
        {
            Roles = roles.ToList();
        }
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            try
            {
                if (Roles == null)
                {
                    HandleUnauthorizedRequest(filterContext);
                }
                else
                {
                    string userId = string.Empty;
                    var cookies = filterContext.Request.Headers.Authorization.Scheme;//.GetCookies("Authorization").FirstOrDefault();
                    if (cookies != null)
                    {
                        string paCookies = cookies;
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(paCookies);
                        var pingToken = handler.ReadToken(paCookies) as JwtSecurityToken;
                        userId = pingToken.Claims.First(claim => claim.Type == "userId").Value;
                        DateTime issueTime = Convert.ToDateTime(pingToken.Claims.First(claim => claim.Type == "issueTime").Value);

                        if (!string.IsNullOrEmpty(userId) && issueTime.AddHours(8) > DateTime.UtcNow)
                        {
                            string roles = BusinessLayer.GeneralSecurity.Permission_ByRoleId(Convert.ToInt32(userId));
                            Roles = roles.Split(',').ToList();
                            //if(Roles.Any(r=>r==))
                            filterContext.ControllerContext.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(userId), Roles.ToArray());
                        }
                        else
                            HandleUnauthorizedRequest(filterContext);
                    }
                    else
                        HandleUnauthorizedRequest(filterContext);
                }
            }
            catch (Exception ex)
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
