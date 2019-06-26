<%@ Page Title="User" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="AegisDMS.User" %>

<%@ Register Src="~/UserMessage.ascx" TagPrefix="uc1" TagName="UserMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                        <div class="span12">
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="icon-align-justify"></i></span>
                                    <h5>Personal-info</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label class="control-label">Name :</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtName" runat="server" class="span11" placeholder="Name"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">User name :</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtUserName" runat="server" class="span11" placeholder="User name" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Password</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="span11" placeholder="Enter password" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Confirm password</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" class="span11" placeholder="Enter confirm password"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" Style="color: red;" ErrorMessage="Didn't match, please re-enter password." ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="icon-align-justify"></i></span>
                                    <h5>Role</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <div class="control-group">
                                                <label class="control-label">Roles :</label>
                                                <div class="controls">
                                                    <asp:GridView ID="gvRole" runat="server" class="table table-bordered table-striped"
                                                        AutoGenerateColumns="false" DataKeyNames="RoleId">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkRoleAll" runat="server" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkRole" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    SN.
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Role Name">
                                                                <ItemTemplate>
                                                                    <%# Eval("Name") %>
                                                                </ItemTemplate>
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
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="icon-align-justify"></i></span>
                                    <h5>User Group</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <div class="control-group">
                                                <label class="control-label">User Group :</label>
                                                <div class="controls">
                                                    <asp:CheckBoxList ID="chklUserGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklUserGroup_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="control-group">
                                <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-success" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-danger" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="icon-align-justify"></i></span>
                                    <h5>List</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="form-horizontal">
                                        <asp:GridView ID="gvUser" runat="server" class="table table-bordered table-striped"
                                            AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                                            OnPageIndexChanging="gvUser_PageIndexChanging" OnRowCommand="gvUser_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        SN.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User Name">
                                                    <ItemTemplate>
                                                        <%# Eval("UserName") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Full Name">
                                                    <ItemTemplate>
                                                        <%# Eval("Name") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgStatus" runat="server" CausesValidation="false" CommandName="S"
                                                            CommandArgument='<%# Eval("UserId") %>' ImageUrl='<%# (Eval("Status").ToString() == "1")?"~/img/custom/block_user.png":"~/img/custom/unblock_user.png" %>'
                                                            ImageAlign="AbsMiddle" ToolTip='<%# (Eval("Status").ToString() == "1")?"BLOCK USER":"UNBLOCK USER" %>' Width="20px" Height="20px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div class="inline-popups">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/custom/user-group-icon.png" CausesValidation="false" CommandName="UG" Width="20px" Height="20px" />
                                                        </div>
                                                    </ItemTemplate>
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
                    </div>
                </div>
            </div>
            <!--end-main-container-part-->
            <%--<a id="anchorUserGroup" href="#test-popup" data-effect="mfp-3d-unfold"></a>--%>
            <!-- Popup itself -->
            <%--<div id="test-popup" class="white-popup mfp-with-anim mfp-hide">
                <div class="widget-box">
                    <div class="widget-title">
                        <span class="icon"><i class="icon-align-justify"></i></span>
                        <h5>User group list</h5>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal">
                            <asp:GridView ID="gvUserGroup" runat="server" class="table table-bordered table-striped"
                                AutoGenerateColumns="false" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            SN.
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name">
                                        <ItemTemplate>
                                            <%# Eval("UserName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Name">
                                        <ItemTemplate>
                                            <%# Eval("Name") %>
                                        </ItemTemplate>
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
            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/magnific-popup.js/1.1.0/jquery.magnific-popup.min.js'></script>
    <script src="js/custom/popup-control.js"></script>
</asp:Content>
