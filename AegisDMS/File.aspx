<%@ Page Title="File" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="File.aspx.cs" Inherits="AegisDMS.File" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserMessage.ascx" TagPrefix="uc1" TagName="UserMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <!--main-container-part-->
    <div id="content">
        <!--breadcrumbs-->
        <div id="content-header">
            <div id="breadcrumb"><a href="Dashboard.aspx" title="Go to Home" class="tip-bottom"><i class="icon-home"></i>Home</a></div>
            <h4>
                <uc1:UserMessage runat="server" ID="UserMessage" />
            </h4>
        </div>
        <!--End-breadcrumbs-->

        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span6">
                    <div class="widget-box">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-align-justify"></i></span>
                            <h5>File attributes-info</h5>
                        </div>
                        <div class="widget-content">
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label">Entry Date :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtEntryDate" runat="server" class="span11"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEntryDate_CalendarExtender" runat="server" TargetControlID="txtEntryDate" Format="dd MMM yyyy">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">File category :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlFileCategory" runat="server" class="select2-choice" AutoPostBack="True" OnSelectedIndexChanged="ddlFileCategory_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">File Type :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlFileType" runat="server" class="select2-choice" AutoPostBack="True" OnSelectedIndexChanged="ddlFileType_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <asp:GridView ID="gvMetadata" runat="server" class="table table-bordered table-striped"
                                        AutoGenerateColumns="false" DataKeyNames="MetadataId">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Metadata">
                                                <ItemTemplate>
                                                    <%# Eval("Name") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Metadata Value">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMetadataValue" runat="server" class="span11"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="span6">
                    <div class="widget-box">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-align-justify"></i></span>
                            <h5>Select File(s)</h5>
                        </div>
                        <div class="widget-content">
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <div class="controls">
                                        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-success" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-danger" OnClick="btnCancel_Click" />
                                    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end-main-container-part-->
</asp:Content>
