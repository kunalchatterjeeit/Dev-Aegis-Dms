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
    public partial class FileCategory : System.Web.UI.Page
    {
        private int _FileCategoryId
        {
            get { return Convert.ToInt32(ViewState["FileCategoryId"]); }
            set { ViewState["FileCategoryId"] = value; }
        }
        private void FileCategory_GetAll()
        {
            gvFileCategory.DataSource = BusinessLayer.FileCategory.FileCategoryGetAll(new Entity.FileCategory());
            gvFileCategory.DataBind();
        }
        private void FileCategory_GetById()
        {
            Entity.FileCategory fileCategory = new Entity.FileCategory()
            {
                FileCategoryId = _FileCategoryId
            };
            DataTable dtFileCategory = BusinessLayer.FileCategory.FileCategoryGetAll(fileCategory);
            if (dtFileCategory != null && dtFileCategory.Rows.Count > 0)
            {
                txtName.Text = dtFileCategory.Rows[0]["Name"].ToString();
                txtNote.Text = dtFileCategory.Rows[0]["Note"].ToString();
            }
        }
        private int FileCategory_Save()
        {
            int retValue = 0;
            Entity.FileCategory fileCategory = new Entity.FileCategory()
            {
                FileCategoryId = _FileCategoryId,
                Name = txtName.Text.Trim(),
                Note = txtNote.Text.Trim()
            };

            retValue = BusinessLayer.FileCategory.FileCategorySave(fileCategory);

            return retValue;
        }
        private int FileCategory_Delete(int fileCategoryId)
        {
            int retValue = 0;
            retValue = BusinessLayer.FileCategory.FileCategoryDelete(fileCategoryId);
            return retValue;
        }
        private void Clear()
        {
            _FileCategoryId = 0;
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;
            txtName.Text = string.Empty;
            txtNote.Text = string.Empty;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileCategory_GetAll();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int retValue = FileCategory_Save();
            Clear();
            FileCategory_GetAll();

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
        protected void gvFileCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFileCategory.PageIndex = e.NewPageIndex;
            FileCategory_GetAll();
        }
        protected void gvFileCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "E")
            {
                _FileCategoryId = Convert.ToInt32(e.CommandArgument.ToString());
                FileCategory_GetById();
            }
            else if (e.CommandName == "D")
            {
                try
                {
                    int retValue = FileCategory_Delete(Convert.ToInt32(e.CommandArgument.ToString()));
                    if (retValue > 0)
                    {
                        FileCategory_GetAll();
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