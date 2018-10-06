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
    public partial class Metadata : System.Web.UI.Page
    {
        private Int64 _MetadataId
        {
            get { return Convert.ToInt64(ViewState["MetadataId"]); }
            set { ViewState["MetadataId"] = value; }
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
            DataTable dtFileType = BusinessLayer.FileType.GetAll(new Entity.FileType()
            {
                FileCategoryId = Convert.ToInt32(ddlFileCategory.SelectedValue.Trim())
            });
            ddlFileType.DataSource = dtFileType;
            ddlFileType.DataTextField = "Name";
            ddlFileType.DataValueField = "FileTypeId";
            ddlFileType.DataBind();
            ddlFileType.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "0", Selected = true });
        }
        private void Metadata_GetAll()
        {

            gvMetadata.DataSource = BusinessLayer.FileMetadata.MetadataGetAll(new Entity.Metadata());
            gvMetadata.DataBind();

        }
        private void Metadata_GetById()
        {
            Entity.Metadata metadata = new Entity.Metadata()
            {
                MetadataId = _MetadataId
            };
            DataTable dtMetadata = BusinessLayer.FileMetadata.MetadataGetAll(metadata);
            if (dtMetadata != null && dtMetadata.Rows.Count > 0)
            {
                ddlFileCategory.SelectedValue = dtMetadata.Rows[0]["FileCategoryId"].ToString();
                FileType_GetAll();
                ddlFileType.SelectedValue = dtMetadata.Rows[0]["FileTypeId"].ToString();
                txtName.Text = dtMetadata.Rows[0]["Name"].ToString();
                txtNote.Text = dtMetadata.Rows[0]["Note"].ToString();
            }
        }
        private int Metadata_Save()
        {
            int retValue = 0;
            Entity.Metadata fileType = new Entity.Metadata()
            {
                MetadataId = _MetadataId,
                FileTypeId = Convert.ToInt32(ddlFileType.SelectedValue),
                Name = txtName.Text.Trim(),
                Note = txtNote.Text.Trim()
            };

            retValue = BusinessLayer.FileMetadata.MetadataSave(fileType);

            return retValue;
        }
        private int Metadata_Delete(Int64 metadataId)
        {
            int retValue = 0;
            retValue = BusinessLayer.FileMetadata.MetadataDelete(metadataId);
            return retValue;
        }
        private void Clear()
        {
            _MetadataId = 0;
            UserMessage.Css = string.Empty;
            UserMessage.Text = string.Empty;
            ddlFileType.SelectedIndex = 0;
            txtName.Text = string.Empty;
            txtNote.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileCategory_GetAll();
                FileType_GetAll();
                Metadata_GetAll();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int retValue = Metadata_Save();
            Clear();
            Metadata_GetAll();

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
        protected void gvMetadata_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMetadata.PageIndex = e.NewPageIndex;
            Metadata_GetAll();
        }
        protected void ddlFileCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileType_GetAll();
        }
        protected void gvMetadata_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "E")
            {
                _MetadataId = Convert.ToInt64(e.CommandArgument.ToString());
                Metadata_GetById();
            }
            else if (e.CommandName == "D")
            {
                try
                {
                    int retValue = Metadata_Delete(Convert.ToInt64(e.CommandArgument.ToString()));
                    if (retValue > 0)
                    {
                        Metadata_GetAll();
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