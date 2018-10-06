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
    public partial class FileType : System.Web.UI.Page
    {
        private int _FileTypeId
        {
            get { return Convert.ToInt32(ViewState["FileTypeId"]); }
            set { ViewState["FileTypeId"] = value; }
        }
        private void FileCategory_GetAll()
        {
            DataTable dtFileCategory = BusinessLayer.FileCategory.FileCategoryGetAll(new Entity.FileCategory());
            ddlFileCategory.DataSource = dtFileCategory;
            ddlFileCategory.DataTextField = "Name";
            ddlFileCategory.DataValueField = "FileCategoryId";
            ddlFileCategory.DataBind();
            ddlFileCategory.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "0", Selected = true });
        }
        private void FileType_GetAll()
        {
            gvFileType.DataSource = BusinessLayer.FileType.GetAll(new Entity.FileType());
            gvFileType.DataBind();
        }
        private void FileType_GetById()
        {
            Entity.FileType fileType = new Entity.FileType()
            {
                FileTypeId = _FileTypeId
            };
            DataTable dtFileType = BusinessLayer.FileType.GetAll(fileType);
            if (dtFileType != null && dtFileType.Rows.Count > 0)
            {
                ddlFileCategory.SelectedValue = dtFileType.Rows[0]["FileCategoryId"].ToString();
                txtName.Text = dtFileType.Rows[0]["Name"].ToString();
                txtNote.Text = dtFileType.Rows[0]["Note"].ToString();
            }
        }
        private int FileType_Save()
        {
            int retValue = 0;
            Entity.FileType fileType = new Entity.FileType()
            {
                FileTypeId = 0,
                FileCategoryId = Convert.ToInt32(ddlFileCategory.SelectedValue),
                Name = txtName.Text.Trim(),
                Note = txtNote.Text.Trim()
            };

            retValue = BusinessLayer.FileType.Save(fileType);

            return retValue;
        }
        private int FileType_Delete(int fileTypeId)
        {
            int retValue = 0;
            retValue = BusinessLayer.FileType.FileTypeDelete(fileTypeId);
            return retValue;
        }
        private void Clear()
        {
            _FileTypeId = 0;
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;
            ddlFileCategory.SelectedIndex = 0;
            txtName.Text = string.Empty;
            txtNote.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileCategory_GetAll();
                FileType_GetAll();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int retValue = FileType_Save();
            Clear();
            FileType_GetAll();

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
        protected void gvFileType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFileType.PageIndex = e.NewPageIndex;
            FileType_GetAll();
        }
        protected void gvFileType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "E")
            {
                _FileTypeId = Convert.ToInt32(e.CommandArgument.ToString());
                FileType_GetById();
            }
            else if (e.CommandName == "D")
            {
                try
                {
                    int retValue = FileType_Delete(Convert.ToInt32(e.CommandArgument.ToString()));
                    if (retValue > 0)
                    {
                        FileType_GetAll();
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