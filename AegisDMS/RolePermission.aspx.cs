using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AegisDMS
{
    public partial class RolePermission : System.Web.UI.Page
    {
        private int _RoleId
        {
            get { return Convert.ToInt32(ViewState["RoleId"]); }
            set { ViewState["RoleId"] = value; }
        }
        private void Role_GetAll()
        {
            DataTable dtFileCategory = BusinessLayer.Role.RoleGetAll();
            ddlRole.DataSource = dtFileCategory;
            ddlRole.DataTextField = "Name";
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "0", Selected = true });
        }
        private void RolePermission_GetByRoleId()
        {
            Clear();
            int roleId = Convert.ToInt32(ddlRole.SelectedValue);
            DataTable dtRolePermission = BusinessLayer.Role.RolePermissionGetByRoleId(roleId);

            foreach (DataRow drRolePermission in dtRolePermission.Rows)
            {
                if (chkUser.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkUser.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
                else if (chkUserRole.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkUserRole.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
                else if (chkRolePermission.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkRolePermission.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
                else if (chkFileCategory.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkFileCategory.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
                else if (chkFileType.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkFileType.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
                else if (chkMetadata.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkMetadata.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
                else if (chkFile.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkFile.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
                else if (chkSearch.Items.FindByValue(drRolePermission["PermissionId"].ToString()) != null)
                    chkSearch.Items.FindByValue(drRolePermission["PermissionId"].ToString()).Selected = true;
            }

            
        }
        private void SaveRoleAccessLevel(int PermissionId, bool IsChecked)
        {
            int RoleId = Convert.ToInt32(ddlRole.SelectedValue);
            BusinessLayer.Role.RolePermissionSave(RoleId, PermissionId, IsChecked);
        }
        private void Clear()
        {
            _RoleId = 0;
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;

            foreach (ListItem lstItem in chkUser.Items)
                lstItem.Selected = false;
            foreach (ListItem lstItem in chkUser.Items)
                lstItem.Selected = false;
            foreach (ListItem lstItem in chkRolePermission.Items)
                lstItem.Selected = false;
            foreach (ListItem lstItem in chkFileCategory.Items)
                lstItem.Selected = false;
            foreach (ListItem lstItem in chkFileType.Items)
                lstItem.Selected = false;
            foreach (ListItem lstItem in chkMetadata.Items)
                lstItem.Selected = false;
            foreach (ListItem lstItem in chkFile.Items)
                lstItem.Selected = false;
            foreach (ListItem lstItem in chkSearch.Items)
                lstItem.Selected = false;
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
           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlRole.SelectedIndex = 0;
             Clear();
        }

        protected void CheckListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(Request.Form["__EVENTTARGET"].Substring(Request.Form["__EVENTTARGET"].LastIndexOf('$') + 1));
            CheckBoxList cbl = (CheckBoxList)sender;
            int PermissionId = int.Parse(cbl.Items[index].Value);
            SaveRoleAccessLevel(PermissionId, cbl.Items[index].Selected);
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            RolePermission_GetByRoleId();
        }
    }
}