using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Net;
using System.Net.Sockets;

namespace AegisDMS
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
            }

            FormsAuthentication.SignOut();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        private string GetIP()
        {
            string retValue = string.Empty;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    retValue = ip.ToString();
                }
            }

            return retValue;
        }

        private string GetClient()
        {
            return Request.Headers["User-Agent"].ToString();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Entity.Auth auth = new Entity.Auth();
                DataTable userDetails = BusinessLayer.GeneralSecurity.LogOn(txtUserName.Text.Trim());
                if (userDetails != null && userDetails.Rows.Count > 0)
                {
                    string passowrd = (userDetails).Rows[0][1].ToString();
                    if (passowrd.Equals(txtPassword.Text.Trim().ToEncrypt(true)))
                    {
                        string roles = BusinessLayer.GeneralSecurity.Permission_ByRoleId(Convert.ToInt32(userDetails.Rows[0]["UserId"].ToString()));
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                                                       1,
                                                                       (userDetails).Rows[0][0].ToString(), //UserId
                                                                       DateTime.Now,
                                                                       DateTime.Now.AddHours(2),
                                                                       false,
                                                                       roles, //define roles here
                                                                       "/");
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                        Response.Cookies.Add(cookie);

                        auth.UserId = Convert.ToInt32((userDetails).Rows[0][0].ToString());
                        auth.IP = GetIP();
                        auth.Status = Entity.LoginStatus.Success;
                        auth.Client = GetClient();
                        BusinessLayer.GeneralSecurity.Login_Save(auth);
                        Response.Redirect(@"Dashboard.aspx");
                    }
                    else
                    {
                        auth.UserId = Convert.ToInt32((userDetails).Rows[0][0].ToString());
                        auth.IP = GetIP();
                        auth.Status = Entity.LoginStatus.WrongPassword;
                        auth.Client = GetClient();
                        auth.FailedUserName = txtUserName.Text;
                        auth.FailedPassword = txtPassword.Text;
                        BusinessLayer.GeneralSecurity.Login_Save(auth);

                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    auth.IP = GetIP();
                    auth.Status = Entity.LoginStatus.Failed;
                    auth.Client = GetClient();
                    auth.FailedUserName = txtUserName.Text;
                    auth.FailedPassword = txtPassword.Text;
                    BusinessLayer.GeneralSecurity.Login_Save(auth);

                    lblMessage.Visible = true;
                }
            }
            catch (CustomException ex)
            {
                ex.Log(Request.Url.AbsoluteUri, 0);
                lblMessage.Visible = true;
            }
        }
    }
}