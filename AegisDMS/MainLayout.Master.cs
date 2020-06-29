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

            if (!IsPostBack)
            {
                //SETTING
                //liFileNew.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.FILE);
                //liFile.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.FILE);
                //liFileCategory.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.FILECATEGORY);
                //liFileType.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.FILETYPE);
                //liMetadata.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.METADATA);
                //liRolePermission.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.ROLEPERMISSION);
                //liSearch.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.SEARCH);
                //liUser.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.USER);
                //liUserRole.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.USERROLE);
                //liUserGroup.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.USERGROUP);
            }
        }
    }
}