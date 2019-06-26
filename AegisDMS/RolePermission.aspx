<%@ Page Title="Role Permission" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="RolePermission.aspx.cs" Inherits="AegisDMS.RolePermission" %>

<%@ Register Src="~/UserMessage.ascx" TagPrefix="uc1" TagName="UserMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <h5>Role-info</h5>
                        </div>
                        <div class="widget-content">
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label">Role :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlRole" runat="server" class="select2-choice" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"></asp:DropDownList>
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
                <div class="span6">
                    <div class="widget-box">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-align-justify"></i></span>
                            <h5>Permissions</h5>
                        </div>
                        <div class="widget-content">
                            <div class="form-horizontal">
                                 <div class="control-group">
                                    <label for="checkboxes" class="control-label">User Group:</label>
                                    <div class="controls">
                                        <asp:CheckBoxList ID="chkUserGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="900" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">User :</label>
                                    <div class="controls">
                                        <asp:CheckBoxList ID="chkUser" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="100" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">User Role:</label>
                                    <div class="controls">
                                        <asp:CheckBoxList ID="chkUserRole" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="200" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">Role Permission:</label>
                                    <div class="controls">
                                        <asp:CheckBoxList ID="chkRolePermission" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="300" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">File Category:</label>
                                    <div class="controls">
                                         <asp:CheckBoxList ID="chkFileCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="400" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">File Type:</label>
                                    <div class="controls">
                                         <asp:CheckBoxList ID="chkFileType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="500" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">Metadata:</label>
                                    <div class="controls">
                                         <asp:CheckBoxList ID="chkMetadata" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="600" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">File:</label>
                                    <div class="controls">
                                        <asp:CheckBoxList ID="chkFile" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="700" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="checkboxes" class="control-label">Search:</label>
                                    <div class="controls">
                                        <asp:CheckBoxList ID="chkSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckListBox_SelectedIndexChanged">
                                            <asp:ListItem Value="800" Text=""></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
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
