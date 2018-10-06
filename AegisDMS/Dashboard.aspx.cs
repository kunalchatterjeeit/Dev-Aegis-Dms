using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AegisDMS
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //SETTING
                liFile.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.FILE);
                liFileCategory.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.FILECATEGORY);
                liFileType.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.FILETYPE);
                liMetadata.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.METADATA);
                liRolePermission.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.ROLEPERMISSION);
                liUser.Visible = HttpContext.Current.User.IsInRole(Entity.Utility.USER);
            }
        }
    }
}