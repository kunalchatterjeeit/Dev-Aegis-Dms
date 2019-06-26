using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using Entity;

namespace AegisDMS
{
    public partial class User : System.Web.UI.Page
    {
        private Int64 _UserGroupUserMappingId
        {
            get { return Convert.ToInt64(ViewState["UserGroupUserMappingId"]); }
            set { ViewState["UserGroupUserMappingId"] = value; }
        }
        private void User_GetAll()
        {
            gvUser.DataSource = BusinessLayer.User.UserGetAll(new Entity.User());
            gvUser.DataBind();

            //gvUserGroup.DataSource = BusinessLayer.User.UserGetAll(new Entity.User());
            //gvUserGroup.DataBind();
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
                CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
                Status = (int)UserStatus.Active
            };

            retValue = BusinessLayer.User.UserSave(user);

            return retValue;
        }
        private int UserRole_Save(int userId)
        {
            int retValue = 0;

            foreach (GridViewRow gvrRole in gvRole.Rows)
            {
                CheckBox chkRole = (CheckBox)gvrRole.FindControl("chkRole");
                retValue += BusinessLayer.User.UserRole_Save(userId, Convert.ToInt32(gvRole.DataKeys[gvrRole.RowIndex].Values[0].ToString()), chkRole.Checked);
            }
            //for (int index = 0; index < chkRole.Items.Count; index++)
            //{
            //    if (!chkRole.Items[index].Value.Equals("*"))
            //    {
            //        retValue += BusinessLayer.User.UserRole_Save(userId, Convert.ToInt32(chkRole.Items[index].Value), chkRole.Items[index].Selected);
            //    }
            //}

            return retValue;
        }
        private int UserGroupUserMapping_Save(int userId)
        {
            int retValue = 0;

            for (int index = 0; index < chklUserGroup.Items.Count; index++)
            {
                if (!chklUserGroup.Items[index].Value.Equals("*"))
                {
                    Entity.UserGroup userGroup = new Entity.UserGroup
                    {
                        UserGroupUserMappingId = _UserGroupUserMappingId,
                        UserGroupId = Convert.ToInt32(chklUserGroup.Items[index].Value),
                        UserId = userId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
                        Status = (chklUserGroup.Items[index].Selected) ? 1 : 0
                    };
                    retValue += BusinessLayer.UserGroup.UserGroupUserMapping_Save(userGroup);
                }
            }

            return retValue;
        }
        private void Role_GetAll()
        {
            DataTable dtRole = BusinessLayer.Role.RoleGetAll();
            //chkRole.DataSource = dtRole;
            //chkRole.DataTextField = "Name";
            //chkRole.DataValueField = "RoleId";
            //chkRole.DataBind();

            gvRole.DataSource = dtRole;
            gvRole.DataBind();
            //chkRole.Items.Insert(0, new ListItem() { Text = "All", Value = "*" });
        }
        private void UserGroup_GetAll()
        {
            chklUserGroup.DataSource = BusinessLayer.UserGroup.UserGroupGetAll(new Entity.UserGroup());
            chklUserGroup.DataTextField = "Name";
            chklUserGroup.DataValueField = "UserGroupId";
            chklUserGroup.DataBind();

            chklUserGroup.Items.Insert(0, new ListItem() { Text = "All", Value = "*" });
        }
        private void Clear()
        {
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;
            txtName.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;

            Role_GetAll();
            //foreach (ListItem item in chkRole.Items)
            //    item.Selected = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User_GetAll();
                Role_GetAll();
                UserGroup_GetAll();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int retValue = User_Save();

            if (retValue > 0)
            {
                DataTable userDetails = BusinessLayer.GeneralSecurity.LogOn(txtUserName.Text.Trim());

                if (userDetails != null && userDetails.Rows.Count > 0)
                {
                    if (UserRole_Save(Convert.ToInt32(userDetails.Rows[0]["UserId"].ToString())) > 0)
                    {
                        if (UserGroupUserMapping_Save(Convert.ToInt32(userDetails.Rows[0]["UserId"].ToString())) > 0)
                        {
                            Clear();
                            User_GetAll();

                            UserMessage.Text = "Saved.";
                            UserMessage.Css = BusinessLayer.MessageCssClass.Success;
                        }
                        else
                        {
                            UserMessage.Text = "Not mapped with user group.";
                            UserMessage.Css = BusinessLayer.MessageCssClass.Warning;
                        }
                    }
                    else
                    {
                        UserMessage.Text = "Not assigned any role.";
                        UserMessage.Css = BusinessLayer.MessageCssClass.Warning;
                    }
                }
                else
                {
                    UserMessage.Text = "User not created.";
                    UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                }
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
                catch (CustomException ex)
                {
                    UserMessage.Text = "Cannot change status. " + ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                    UserMessage.Css = BusinessLayer.MessageCssClass.Error;
                }
            }
            if (e.CommandName == "UG")
            {

            }
        }

        protected void chklUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexOne = Convert.ToInt32(Request.Form["__EVENTTARGET"].Substring(Request.Form["__EVENTTARGET"].LastIndexOf('$') + 1));
            if (indexOne == 0)
            {
                bool isChecked = chklUserGroup.Items[indexOne].Selected ? true : false;
                foreach (ListItem item in chklUserGroup.Items)
                {
                    item.Selected = isChecked;
                }
            }
        }

        //protected void chkRole_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int indexOne = Convert.ToInt32(Request.Form["__EVENTTARGET"].Substring(Request.Form["__EVENTTARGET"].LastIndexOf('$') + 1));
        //    if (indexOne == 0)
        //    {
        //        bool isChecked = chkRole.Items[indexOne].Selected ? true : false;
        //        foreach (ListItem item in chkRole.Items)
        //        {
        //            item.Selected = isChecked;
        //        }
        //    }
        //}
        void HtmlAnchor_Click(Object sender, EventArgs e)
        {
            // your code here
        }
    }
}