using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;

namespace AegisDMS
{
    public partial class User : System.Web.UI.Page
    {
        private void User_GetAll()
        {
            gvUser.DataSource = BusinessLayer.User.UserGetAll(new Entity.User());
            gvUser.DataBind();
        }
        private int User_Save()
        {
            int retValue = 0;
            Entity.User user = new Entity.User()
            {
                UserId = 0,
                Name = txtName.Text.Trim(),
                Username = txtUserName.Text.Trim(),
                Password = txtPassword.Text.Trim().ToEncrypt(true),
                CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name)
            };

            retValue = BusinessLayer.User.UserSave(user);

            return retValue;
        }
        private void Clear()
        {
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;
            txtName.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User_GetAll();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int retValue = User_Save();
            Clear();
            User_GetAll();

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
        protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUser.PageIndex = e.NewPageIndex;
            User_GetAll();
        }

        protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "S")
            {
                try
                {
                    int userId = Convert.ToInt32(e.CommandArgument.ToString());
                    DataTable dtUser = BusinessLayer.User.UserGetAll(new Entity.User { UserId = userId });
                    int existingStatus = 0;
                    if (dtUser != null && dtUser.Rows.Count > 0)
                    {
                        existingStatus = Convert.ToInt32(dtUser.Rows[0]["Status"].ToString());
                    }

                    if (existingStatus == 1)
                        existingStatus = 0;
                    else if (existingStatus == 0)
                        existingStatus = 1;

                    int retValue = BusinessLayer.User.UserStatusChange(userId, existingStatus, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                    if (retValue > 0)
                    {
                        User_GetAll();
                        UserMessage.Text = "Status changed.";
                        UserMessage.Css = BusinessLayer.MessageCssClass.Success;
                    }
                    else
                    {
                        UserMessage.Text = "Cannot change status.";
                        UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                    }
                }
                catch (Exception ex)
                {
                    UserMessage.Text = "Cannot change status. " + ex.Message;
                    UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                }
            }
        }
    }
}