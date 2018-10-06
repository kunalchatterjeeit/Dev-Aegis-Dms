using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AegisDMS
{
    public partial class Role : System.Web.UI.Page
    {
        private int _RoleId
        {
            get { return Convert.ToInt32(ViewState["RoleId"]); }
            set { ViewState["RoleId"] = value; }
        }
        private void Role_GetAll()
        {
            gvRole.DataSource = BusinessLayer.Role.RoleGetAll();
            gvRole.DataBind();
        }
        private void Role_GetById()
        {
            Entity.Role Role = new Entity.Role()
            {
                RoleId = _RoleId
            };
            DataTable dtRole = BusinessLayer.Role.RoleGetAll();
            if (dtRole != null && dtRole.Rows.Count > 0)
            {
                txtName.Text = dtRole.Rows[0]["Name"].ToString();
                txtNote.Text = dtRole.Rows[0]["Note"].ToString();
            }
        }
        private int Role_Save()
        {
            int retValue = 0;
            Entity.Role Role = new Entity.Role()
            {
                RoleId = _RoleId,
                Name = txtName.Text.Trim(),
                Note = txtNote.Text.Trim()
            };

            retValue = BusinessLayer.Role.RoleSave(Role);

            return retValue;
        }
        private int Role_Delete(int RoleId)
        {
            int retValue = 0;
            retValue = BusinessLayer.Role.RoleDelete(RoleId);
            return retValue;
        }
        private void Clear()
        {
            _RoleId = 0;
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;
            txtName.Text = string.Empty;
            txtNote.Text = string.Empty;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Role_GetAll();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int retValue = Role_Save();
            Clear();
            Role_GetAll();

            if (retValue > 0)
            {
                UserMessage.Text = "Saved.";
                UserMessage.Css = BusinessLayer.MessageCssClass.Success;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRole.PageIndex = e.NewPageIndex;
            Role_GetAll();
        }
        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "E")
            {
                _RoleId = Convert.ToInt32(e.CommandArgument.ToString());
                Role_GetById();
            }
            else if (e.CommandName == "D")
            {
                try
                {
                    int retValue = Role_Delete(Convert.ToInt32(e.CommandArgument.ToString()));
                    if (retValue > 0)
                    {
                        Role_GetAll();
                        UserMessage.Text = "Deleted.";
                        UserMessage.Css = BusinessLayer.MessageCssClass.Success;
                    }
                    else
                    {
                        UserMessage.Text = "Cannot delete. Posible Reason: reference exists.";
                        UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                    }
                }
                catch (CustomException ex)
                {
                    UserMessage.Text = "Cannot delete. " + ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                    UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                }
            }
        }
    }
}