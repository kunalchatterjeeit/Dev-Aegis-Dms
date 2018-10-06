using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;

namespace AegisDMS
{
    public partial class Search : System.Web.UI.Page
    {
        private static string _searchMode = "QuickSearch";

        public DataTable dtQuery
        {
            get
            {
                return (DataTable)ViewState["Query"];
            }
            set
            {
                ViewState["Query"] = value;
            }
        }
        private void FileCategory_GetAll()
        {
            DataTable dtFileCategory = BusinessLayer.FileCategory.FileCategoryGetAll(new Entity.FileCategory());
            ddlFileCategory.DataSource = dtFileCategory;
            ddlFileCategory.DataTextField = "Name";
            ddlFileCategory.DataValueField = "FileCategoryId";
            ddlFileCategory.DataBind();
            ddlFileCategory.Items.Insert(0, new ListItem() { Text = "--File Category--", Value = "0", Selected = true });
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
            ddlFileType.Items.Insert(0, new ListItem() { Text = "--File Type--", Value = "0", Selected = true });
        }

        private void Metadata_GetAll()
        {
            Entity.Metadata metadata = new Entity.Metadata()
            {
                FileTypeId = Convert.ToInt32(ddlFileType.SelectedValue.Trim())
            };

            ddlMetadata.DataSource = BusinessLayer.FileMetadata.MetadataGetAll(metadata);
            ddlMetadata.DataTextField = "Name";
            ddlMetadata.DataValueField = "MetadataId";
            ddlMetadata.DataBind();
            ddlMetadata.Items.Insert(0, new ListItem() { Text = "--Metadata--", Value = "0", Selected = true });
        }

        private void BindMetadataList()
        {
            if (dtQuery != null)
            {
                dlQuery.DataSource = dtQuery;
                dlQuery.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                FileCategory_GetAll();
                FileType_GetAll();
                Metadata_GetAll();
                btnQuickSearch_Click(sender, e);
                btnAND.Visible = false;
                btnOR.Visible = false;
            }
        }

        protected void ddlFileCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileType_GetAll();
        }

        protected void ddlFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Metadata_GetAll();

            btnAND.Visible = false;
            btnOR.Visible = false;
        }

        protected void btnOR_Click(object sender, EventArgs e)
        {
            if (dtQuery != null && dtQuery.Rows.Count > 0)
            {
                if (!(dtQuery.Rows[dtQuery.Rows.Count - 1]["MetadataName"].ToString()).Equals("OR")
                    && !(dtQuery.Rows[dtQuery.Rows.Count - 1]["MetadataName"].ToString()).Equals("AND"))
                {
                    DataRow drQuery = dtQuery.NewRow();
                    drQuery["MetadataName"] = "OR";

                    dtQuery.Rows.Add(drQuery);
                    dtQuery.AcceptChanges();

                    BindMetadataList();
                }
            }

            btnAND.Visible = false;
            btnOR.Visible = false;
        }

        protected void btnAND_Click(object sender, EventArgs e)
        {
            if (dtQuery != null && dtQuery.Rows.Count > 0)
            {
                if (!(dtQuery.Rows[dtQuery.Rows.Count - 1]["MetadataName"].ToString()).Equals("AND")
                    && !(dtQuery.Rows[dtQuery.Rows.Count - 1]["MetadataName"].ToString()).Equals("OR"))
                {
                    DataRow drQuery = dtQuery.NewRow();
                    drQuery["MetadataName"] = "AND";

                    dtQuery.Rows.Add(drQuery);
                    dtQuery.AcceptChanges();

                    BindMetadataList();
                }
            }

            btnAND.Visible = false;
            btnOR.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (dtQuery == null)
            {
                dtQuery = new DataTable();
                dtQuery.Columns.Add("MetadataName");
                dtQuery.Columns.Add("MetadataId");
                dtQuery.Columns.Add("MetadataValue");
            }

            DataRow drQuery = dtQuery.NewRow();
            drQuery["MetadataName"] = ddlMetadata.SelectedItem.Text.Trim();
            drQuery["MetadataId"] = ddlMetadata.SelectedValue;
            drQuery["MetadataValue"] = txtMetadataValue.Text.Trim();

            dtQuery.Rows.Add(drQuery);
            dtQuery.AcceptChanges();

            BindMetadataList();

            btnAND.Visible = true;
            btnOR.Visible = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _searchMode = "Search";
            btnAND.Visible = false;
            btnOR.Visible = false;
            lblMessage.Visible = false;

            string query = string.Empty;
            string queryType = string.Empty;
            int commonOccurance = 1;

            if (dtQuery != null && dtQuery.Rows.Count > 0)
            {
                foreach (DataRow drQuery in dtQuery.Rows)
                {
                    if (!(drQuery["MetadataName"].ToString().Equals("AND")) && !(drQuery["MetadataName"].ToString().Equals("OR")))
                    {
                        query += "(MetaDataId = " + drQuery["MetadataId"].ToString();
                    }

                    if ((drQuery["MetadataName"].ToString().Equals("AND")) || (drQuery["MetadataName"].ToString().Equals("OR")))
                    {
                        if (string.IsNullOrEmpty(queryType))
                        {
                            queryType = drQuery["MetadataName"].ToString(); //setting AND/OR to decide which query to call
                        }

                        query += " " + "OR" + " "; //OR for all query types //drQuery["MetadataName"].ToString()

                        ++commonOccurance; //Used for AND type query
                    }

                    if (!string.IsNullOrEmpty(drQuery["MetadataValue"].ToString()) || !string.IsNullOrEmpty(drQuery["MetadataValue"].ToString()))
                    {
                        query += " AND MetaDataContent = '" + drQuery["MetadataValue"].ToString().Trim() + "')";
                    }
                }

                if (queryType == "AND" && commonOccurance > 0)
                {
                    query += " GROUP BY FileGuid HAVING COUNT(FileGuid) >= " + commonOccurance;
                }

                gvFile.DataSource = BusinessLayer.File.FileSearchByMetadata(query, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
                gvFile.DataBind();
            }
            else
            {
                lblMessage.Text = "Error: Please add search criteria.";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
            }            
        }

        protected void btnQuickSearch_Click(object sender, EventArgs e)
        {
            _searchMode = "QuickSearch";
            gvFile.DataSource = BusinessLayer.File.FileSearchByPhrase(txtQuickSearch.Text.Trim(), Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            gvFile.DataBind();
        }

        protected void gvFile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HtmlAnchor anchrPopUp = (HtmlAnchor)e.Row.FindControl("anchrPopUp");
                    anchrPopUp.Attributes.Add("onclick", "javascript:openpopup('Viewer.aspx?id=" + gvFile.DataKeys[e.Row.RowIndex].Values[0].ToString().ToEncrypt(true) + "')");
                }
            }
            catch (CustomException ex)
            {
                ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            }
        }

        protected void gvFile_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFile.PageIndex = e.NewPageIndex;
            if (_searchMode.Equals("QuickSearch"))
            {
                btnQuickSearch_Click(sender, e);
            }
            else
            {
                btnSearch_Click(sender, e);
            }
        }
    }
}