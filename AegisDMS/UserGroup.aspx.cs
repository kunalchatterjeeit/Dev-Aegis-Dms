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
    public partial class UserGroup : System.Web.UI.Page
    {
        private int _UserGroupId
        {
            get { return Convert.ToInt32(ViewState["UserGroupId"]); }
            set { ViewState["UserGroupId"] = value; }
        }
        private void UserGroup_GetAll()
        {
            gvUserGroup.DataSource = BusinessLayer.UserGroup.UserGroupGetAll(new Entity.UserGroup());
            gvUserGroup.DataBind();
        }
        private void UserGroup_GetById()
        {
            Entity.UserGroup UserGroup = new Entity.UserGroup()
            {
                UserGroupId = _UserGroupId
            };
            DataTable dtUserGroup = BusinessLayer.UserGroup.UserGroupGetAll(UserGroup);
            if (dtUserGroup != null && dtUserGroup.Rows.Count > 0)
            {
                txtName.Text = dtUserGroup.Rows[0]["Name"].ToString();
                txtNote.Text = dtUserGroup.Rows[0]["Note"].ToString();
            }
        }
        private int UserGroup_Save()
        {
            int retValue = 0;
            Entity.UserGroup UserGroup = new Entity.UserGroup()
            {
                UserGroupId = _UserGroupId,
                Name = txtName.Text.Trim(),
                Note = txtNote.Text.Trim()
            };

            retValue = BusinessLayer.UserGroup.UserGroupSave(UserGroup);

            return retValue;
        }
        private int UserGroup_Delete(int UserGroupId)
        {
            int retValue = 0;
            retValue = BusinessLayer.UserGroup.UserGroupDelete(UserGroupId);
            return retValue;
        }
        private void Clear()
        {
            _UserGroupId = 0;
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;
            txtName.Text = string.Empty;
            txtNote.Text = string.Empty;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserGroup_GetAll();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int retValue = UserGroup_Save();
            Clear();
            UserGroup_GetAll();

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
        protected void gvUserGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserGroup.PageIndex = e.NewPageIndex;
            UserGroup_GetAll();
        }
        protected void gvUserGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "E")
            {
                _UserGroupId = Convert.ToInt32(e.CommandArgument.ToString());
                UserGroup_GetById();
            }
            else if (e.CommandName == "D")
            {
                try
                {
                    int retValue = UserGroup_Delete(Convert.ToInt32(e.CommandArgument.ToString()));
                    if (retValue > 0)
                    {
                        UserGroup_GetAll();
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