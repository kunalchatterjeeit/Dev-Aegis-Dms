<%@ Page Title="FILE AUTO FORMAT" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="FileAutoFormat.aspx.cs" Inherits="AegisDMS.FileAutoFormat" %>
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
        </div>
        <!--End-breadcrumbs-->

        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span6">
                    <div class="widget-box">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-align-justify"></i></span>
                            <h5>List</h5>
                        </div>
                        <div class="widget-content">
                            <div class="form-horizontal">
                                <asp:GridView ID="gvUserGroup" runat="server" class="table table-bordered table-striped"
                                    AutoGenerateColumns="false" DataKeyNames="UserGroupId">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="5">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkGroupHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkGroupHeader_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkGroup" runat="server" AutoPostBack="true" OnCheckedChanged="chkGroup_CheckedChanged"/>
                                            </ItemTemplate>

                                            <HeaderStyle Width="5px"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="10">
                                            <HeaderTemplate>
                                                SN.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>

                                            <HeaderStyle Width="10px"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Group" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("Name") %>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#0349AA" Font-Bold="True" ForeColor="White" />
                                    <RowStyle CssClass="RowStyle" BackColor="#F7F6F3" ForeColor="#333333" />
                                    <EditRowStyle BackColor="#999999" />
                                    <EmptyDataRowStyle CssClass="EditRowStyle" />
                                    <AlternatingRowStyle CssClass="AltRowStyle" BackColor="White" ForeColor="#284775" />
                                    <EmptyDataTemplate>
                                        No Record Found...
                                    </EmptyDataTemplate>
                                </asp:GridView>
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
                                        <asp:DropDownList ID="ddlFileCategory" runat="server" class="select2-choice"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="controls">
                                        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-success" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-danger" OnClick="btnCancel_Click" />
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
