<%@ Page Title="User Group" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="UserGroup.aspx.cs" Inherits="AegisDMS.UserGroup" %>

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
            </h4>
        </div>
        <!--End-breadcrumbs-->
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span6">
                    <div class="widget-box">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-align-justify"></i></span>
                            <h5>User group-info</h5>
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
                                    <label class="control-label">Note :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNote" runat="server" class="span11" placeholder="Note" TextMode="MultiLine"></asp:TextBox>
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
                            <h5>List</h5>
                        </div>
                        <div class="widget-content">
                            <div class="form-horizontal">
                                <asp:GridView ID="gvUserGroup" runat="server" class="table table-bordered table-striped"
                                    AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                                    OnPageIndexChanging="gvUserGroup_PageIndexChanging" OnRowCommand="gvUserGroup_RowCommand">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                SN.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Group Name">
                                            <ItemTemplate>
                                                <%# Eval("Name") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="false" CommandName="E"
                                                    CommandArgument='<%# Eval("UserGroupId") %>' ImageUrl="~/img/custom/edit-button.png"
                                                    ImageAlign="AbsMiddle" ToolTip="EDIT" Width="15px" Height="15px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="false" CommandName="D"
                                                    CommandArgument='<%# Eval("UserGroupId") %>' ImageUrl="~/img/custom/delete-button.png"
                                                    ImageAlign="AbsMiddle" ToolTip="DELETE" Width="20px" Height="20px" />
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
</asp:Content>
