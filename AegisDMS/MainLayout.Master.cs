using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AegisDMS
{
    public partial class MainLayout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect("Login.aspx");
        }
    }
}