using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AegisDMS
{
    public partial class Search : System.Web.UI.Page
    {
        BusinessLayer.SearchFiles objBusiness = new BusinessLayer.SearchFiles();
        Entity.SearchFiles objEntity = new Entity.SearchFiles();

        public string SortDireaction
        {
            get
            {
                if (ViewState["SortDireaction"] == null)
                    return string.Empty;
                else
                    return ViewState["SortDireaction"].ToString();
            }
            set
            {
                ViewState["SortDireaction"] = value;
            }
        }

        public DataTable dt
        {
            get
            {
                return (DataTable)ViewState["dt"];
            }
            set
            {
                ViewState["dt"] = value;
            }
        }

        System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CompanyId"] = 2;
            Session["ProjectId"] = 8;
            Session["EmployeeId"] = 1;

            if (Session["CompanyId"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            if (!IsPostBack)
            {
                LoadFileCategory();
                LoadFiles();
            }
        }

        #region User Defined Functions
        protected void LoadFileCategory()
        {
            try
            {
                BusinessLayer.FileCategory objFileCategory = new BusinessLayer.FileCategory();
                Entity.FileCategory fileCategory = new Entity.FileCategory();
                DataTable DT = objFileCategory.GetAll(fileCategory);

                if (DT != null)
                {
                    ddlFileCategory.DataSource = DT;
                    ddlFileCategory.DataTextField = "Name";
                    ddlFileCategory.DataValueField = "FileCategoryId";
                    ddlFileCategory.DataBind();
                }
                ddlFileCategory.Items.Insert(0, "Select File Category");
            }
            catch
            { }
        }
        protected void LoadFileType()
        {
            try
            {
                Entity.FileType objFileType = new Entity.FileType();
                BusinessLayer.FileType _objFileType = new BusinessLayer.FileType();
                objFileType.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                objFileType.ProjectId = Convert.ToInt32(Session["ProjectId"]);
                objFileType.Mode = 3;
                objFileType.Name = "";
                objFileType.FileCategoryId = Convert.ToInt64(ddlFileCategory.SelectedValue);
                DataTable DT = _objFileType.GetAll(objFileType);
                if (DT != null)
                {
                    DataRow DR = DT.NewRow();
                    DR["FileTypeId"] = "0";
                    DR["Name"] = "Select File Type";
                    DT.Rows.InsertAt(DR, 0);

                    ddlFileType.DataSource = DT;
                    ddlFileType.DataValueField = "FileTypeId";
                    ddlFileType.DataTextField = "Name";
                    ddlFileType.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void LoadFiles()
        {
            //try
            //{
            string fileNameParam = string.Empty;
            if (!String.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                string[] ArrFileName = txtFileName.Text.Trim().Split(',').Where<string>(s => !string.IsNullOrEmpty(s)).ToArray();
                fileNameParam = " and (F.`Name` like '%" + String.Join(@"%' or F.`Name` like '%", ArrFileName) + "%') ";
            }
            objEntity.FileName = fileNameParam;

            string fileContentParam = string.Empty;
            if (!String.IsNullOrEmpty(txtFileContent.Text.Trim()))
            {
                string[] ArrFileContent = txtFileContent.Text.Trim().Split(' ').Where<string>(s => !string.IsNullOrEmpty(s)).ToArray();
                fileContentParam = " and (F.FileContent like '%" + String.Join(@"%' and F.FileContent like '%", ArrFileContent) + "%') ";
            }
            objEntity.FileContent = fileContentParam;
            objEntity.ProjectId = Convert.ToInt64(Session["ProjectId"]); //(ddlProject.SelectedIndex == 0) ? 0 : int.Parse(ddlProject.SelectedValue);
            objEntity.FileCategoryId = (ddlFileCategory.SelectedIndex == 0) ? 0 : Int64.Parse(ddlFileCategory.SelectedValue);
            objEntity.FileTypeId = (ddlFileType.SelectedIndex == 0) ? 0 : Convert.ToInt32(ddlFileType.SelectedValue);
            if (string.IsNullOrEmpty(txtFromDate.Text))
                objEntity.FromDate = null;
            else
                objEntity.FromDate = Convert.ToDateTime(txtFromDate.Text);

            if (string.IsNullOrEmpty(txtToDate.Text))
                objEntity.ToDate = null;
            else
                objEntity.ToDate = Convert.ToDateTime(txtToDate.Text + " 23:59:59");
            objEntity.EmployeeId = int.Parse(Session["EmployeeId"].ToString());

            DataTable dt = new DataTable();
            dt = objBusiness.File_Search_WithoutMetaData(objEntity);

            DataView dv = dt.DefaultView;
            if (this.ViewState["SortExpression"] != null)
            {
                dv.Sort = string.Format("{0} {1}", ViewState["SortExpression"].ToString(), this.ViewState["SortOrder"].ToString());
            }

            gvFiles.AllowPaging = chkAllowPaging.Checked;
            gvFiles.DataSource = dv.ToTable();
            gvFiles.DataBind();

            FileCount.InnerText = dt.Rows.Count.ToString();
            //}
            //catch
            //{
            //}
        }
        protected void LoadMetaDataSearch()
        {
            BusinessLayer.FileUploader objBusiness = new BusinessLayer.FileUploader();
            Entity.FileUploader objEntity = new Entity.FileUploader();

            objEntity.FileTypeId = Convert.ToInt32(ddlFileType.SelectedValue);
            DataTable DT = objBusiness.GetMetaDataByFile(objEntity);

            ddlMetaData.DataSource = DT;
            ddlMetaData.DataTextField = "MetaDataName";
            ddlMetaData.DataValueField = "MetaDataId";
            ddlMetaData.DataBind();

            ddlMetaData.Items.Insert(0, new ListItem("Select Meta Data", "0"));
        }
        protected void LoadFilesByMetaData()
        {
            //try
            //{
            string param = string.Empty;

            //Dynamic query for file
            foreach (DataListItem dli in dlMetaData.Items)
            {
                Label lblMetaData = (Label)dli.FindControl("lblMetaData");
                string metadataContent = (lblMetaData.Text.Split(':')[1].Trim());
                string[] metadataId = hdnMetaDataIds.Value.Split(',');
                if (dli.ItemIndex == 0)//For First Iteration
                    param += " (x.MetaDataId=" + metadataId[dli.ItemIndex] + " AND x.MetaDataContent LIKE '%" + metadataContent + "%')";
                else if (dli.ItemIndex == dlMetaData.Items.Count)//For Last Iteration
                    param += " (x.MetaDataId=" + metadataId[dli.ItemIndex] + " AND x.MetaDataContent LIKE '%" + metadataContent + "%')";
                else
                {
                    param += " OR (x.MetaDataId=" + metadataId[dli.ItemIndex] + " AND x.MetaDataContent LIKE ";
                    param += "'%" + metadataContent + "%')";
                }
            }
            objEntity.Param = param;

            objEntity.Count = dlMetaData.Items.Count;

            string fileNameParam = string.Empty;
            if (!String.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                string[] ArrFileName = txtFileName.Text.Trim().Split(',').Where<string>(s => !string.IsNullOrEmpty(s)).ToArray();
                fileNameParam = " and (F.`Name` like '%" + String.Join(@"%' or F.`Name` like '%", ArrFileName) + "%') ";
            }
            objEntity.FileName = fileNameParam;

            string fileContentParam = string.Empty;
            if (!String.IsNullOrEmpty(txtFileContent.Text.Trim()))
            {
                string[] ArrFileContent = txtFileContent.Text.Trim().Split(' ').Where<string>(s => !string.IsNullOrEmpty(s)).ToArray();
                fileContentParam = " and (F.FileContent like '%" + String.Join(@"%' and F.FileContent like '%", ArrFileContent) + "%') ";
            }
            objEntity.FileContent = fileContentParam;
            objEntity.ProjectId = Convert.ToInt64(Session["ProjectId"]); //(ddlProject.SelectedIndex == 0) ? 0 : int.Parse(ddlProject.SelectedValue);
            objEntity.FileCategoryId = (ddlFileCategory.SelectedIndex == 0) ? 0 : Int64.Parse(ddlFileCategory.SelectedValue);
            objEntity.FileTypeId = (ddlFileType.SelectedIndex == 0) ? 0 : Convert.ToInt32(ddlFileType.SelectedValue);
            if (string.IsNullOrEmpty(txtFromDate.Text))
                objEntity.FromDate = null;
            else
                objEntity.FromDate = Convert.ToDateTime(txtFromDate.Text);

            if (string.IsNullOrEmpty(txtToDate.Text))
                objEntity.ToDate = null;
            else
                objEntity.ToDate = Convert.ToDateTime(txtToDate.Text + " 23:59:59");
            objEntity.EmployeeId = int.Parse(Session["EmployeeId"].ToString());

            DataTable dt = new DataTable();
            dt = objBusiness.File_Search_ByMetaData(objEntity);

            DataView dv = dt.DefaultView;
            if (this.ViewState["SortExpression"] != null)
            {
                dv.Sort = string.Format("{0} {1}", ViewState["SortExpression"].ToString(), this.ViewState["SortOrder"].ToString());
            }

            gvFiles.AllowPaging = chkAllowPaging.Checked;
            gvFiles.DataSource = dv.ToTable();
            gvFiles.DataBind();

            FileCount.InnerText = dt.Rows.Count.ToString();
            //}
            //catch
            //{
            //}
        }
        private void Decrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }
        protected void Clear()
        {
            ddlFileCategory.SelectedIndex = 0;
            //ddlProject.SelectedIndex = 0;
            txtSearch.Value = "";
        }
        protected void LoadMetaData()
        {
            try
            {
                BusinessLayer.SearchFiles objBusiness = new BusinessLayer.SearchFiles();
                Entity.SearchFiles objEntity = new Entity.SearchFiles();
                DataTable dtMD = new DataTable();
                dtMD = objBusiness.MetaDataContent_GetAll(Int64.Parse(ddlMetaData.SelectedValue));
                DataView dv = new DataView(dtMD);
                dtMD = dv.ToTable(true, "FullMetaDataContent", "MetaDataContent");
                dv = new DataView(dtMD);

                if (txtSearch.Value != "Search by Meta Data Content" && !String.IsNullOrEmpty(txtSearch.Value))
                {
                    if (ViewState["MetaDataContent"] == null)
                    {
                        DataTable dtNew = new DataTable();
                        dtNew.Columns.Add("MetaDataContent");
                        dtNew.Rows.Add();
                        dv.RowFilter = "FullMetaDataContent LIKE '" + txtSearch.Value + "'";
                        dtNew.Rows[0]["MetaDataContent"] = (dv.ToTable().Rows.Count > 0) ? ddlMetaData.SelectedItem.Text + ": " + dv.ToTable().Rows[0]["MetaDataContent"].ToString() : txtSearch.Value;
                        hdnMetaDataIds.Value += ddlMetaData.SelectedValue + ",";
                        dtNew.AcceptChanges();
                        dlMetaData.DataSource = dtNew;
                        dlMetaData.DataBind();
                        ViewState["MetaDataContent"] = dtNew;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)ViewState["MetaDataContent"];
                        dt.Rows.Add();
                        dv.RowFilter = "FullMetaDataContent LIKE '" + txtSearch.Value + "'";
                        dt.Rows[dt.Rows.Count - 1]["MetaDataContent"] = (dv.ToTable().Rows.Count > 0) ? ddlMetaData.SelectedItem.Text + ": " + dv.ToTable().Rows[0]["MetaDataContent"].ToString() : txtSearch.Value;
                        hdnMetaDataIds.Value += ddlMetaData.SelectedValue + ",";
                        dt.AcceptChanges();
                        ViewState["MetaDataContent"] = dt;
                        dlMetaData.DataSource = dt;
                        dlMetaData.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>alert('Select Metadata content from list')</script>", false);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (dlMetaData.Items.Count > 0)
                LoadFilesByMetaData();
            else
                LoadFiles();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LoadMetaData();
            txtSearch.Value = "";
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchFiles.aspx");
        }

        protected void gvFiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Attributes.Add("onclick", "javascript:ChangeRowColor('" + e.Row.ClientID + "')");
                    HtmlAnchor anchrPopUp = (HtmlAnchor)e.Row.FindControl("anchrPopUp");
                    HtmlImage ImgIcon = (HtmlImage)e.Row.FindControl("ImgIcon");
                    HtmlAnchor anchrReference = (HtmlAnchor)e.Row.FindControl("anchrReference");
                    HtmlAnchor anchrFileAttachment = (HtmlAnchor)e.Row.FindControl("anchrFileAttachment");

                    if (((DataTable)gvFiles.DataSource).Rows[e.Row.RowIndex]["Type"].ToString().ToLower() == "group")
                    {
                        anchrPopUp.Attributes.Add("onclick", "javascript:openpopup('FileListViewer.aspx?GroupId=" + gvFiles.DataKeys[e.Row.RowIndex].Values[0].ToString() + "')");
                        e.Row.Cells[1].ToolTip = "Click to view files";
                        ImgIcon.Attributes.Add("src", "Images/folder_icon.png");

                        anchrReference.Attributes.Add("style", "display:none;");
                        anchrFileAttachment.Attributes.Add("style", "display:none;");
                    }
                    else if (((DataTable)gvFiles.DataSource).Rows[e.Row.RowIndex]["Type"].ToString().ToLower() == "file")
                    {
                        anchrPopUp.Attributes.Add("onclick", "javascript:openpopup('FileContentViewer.aspx?fileid=" + gvFiles.DataKeys[e.Row.RowIndex].Values[0].ToString() + "')");
                        e.Row.Cells[1].ToolTip = "Click to view file contents";
                        ImgIcon.Attributes.Add("src", "Images/file_icon.png");

                        anchrReference.Attributes.Add("onclick", "javascript:openpopup('FileReferenceViewer.aspx?FileId=" + gvFiles.DataKeys[e.Row.RowIndex].Values[0].ToString() + "')");
                        anchrReference.Attributes.Add("style", "text-decoration: underline; cursor: pointer; color: #0349AA");

                        anchrFileAttachment.Attributes.Add("onclick", "javascript:openpopup('FileAttachment.aspx?FileId=" + gvFiles.DataKeys[e.Row.RowIndex].Values[0].ToString() + "')");
                        anchrFileAttachment.Attributes.Add("style", "text-decoration: underline; cursor: pointer; color: #0349AA");
                    }
                    else if (((DataTable)gvFiles.DataSource).Rows[e.Row.RowIndex]["Type"].ToString().ToLower() == "draft")
                    {
                        anchrPopUp.Attributes.Add("onclick", "javascript:openpopup('PopUpDraftViewer.aspx?DraftId=" + gvFiles.DataKeys[e.Row.RowIndex].Values[0].ToString() + "')");
                        e.Row.Cells[1].ToolTip = "Click to view draft contents";
                        ImgIcon.Attributes.Add("src", "Images/draft_icon.png");

                        anchrReference.Attributes.Add("style", "display:none;");
                        anchrFileAttachment.Attributes.Add("style", "display:none;");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFiles.PageIndex = e.NewPageIndex;
            if (dlMetaData.Items.Count > 0)
                LoadFilesByMetaData();
            else
                LoadFiles();
        }

        protected void ddlFileCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFileType();
        }

        protected void gvFiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortDireaction == "ASC")
                SortDireaction = "DESC";
            else
                SortDireaction = "ASC";

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + SortDireaction;
                gvFiles.DataSource = dt;
                gvFiles.DataBind();

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gvFiles.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gvFiles.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }

                gvFiles.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

                FileCount.InnerText = dt.Rows.Count.ToString();
            }
        }

        protected void chkAllowPaging_CheckedChanged(object sender, EventArgs e)
        {
            if (dlMetaData.Items.Count > 0)
                LoadFilesByMetaData();
            else
                LoadFiles();
        }

        protected void gvFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Sort"))
            {
                if (ViewState["SortExpression"] != null)
                {
                    if (this.ViewState["SortExpression"].ToString() == e.CommandArgument.ToString())
                    {
                        if (ViewState["SortOrder"].ToString() == "ASC")
                            ViewState["SortOrder"] = "DESC";
                        else
                            ViewState["SortOrder"] = "ASC";
                    }
                    else
                    {
                        ViewState["SortOrder"] = "ASC";
                        ViewState["SortExpression"] = e.CommandArgument.ToString();
                    }

                }
                else
                {
                    ViewState["SortExpression"] = e.CommandArgument.ToString();
                    ViewState["SortOrder"] = "ASC";
                }
            }

            if (dlMetaData.Items.Count > 0)
                LoadFilesByMetaData();
            else
                LoadFiles();
        }

        protected void ddlFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMetaDataSearch();
        }
    }
}