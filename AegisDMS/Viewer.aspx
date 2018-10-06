<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewer.aspx.cs" Inherits="AegisDMS.Viewer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="css/matrix-style.css" />
    <link rel="stylesheet" href="css/matrix-media.css" />
    <link href="font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/bootstrap-responsive.min.css" />
    <script src="js/custom.js"></script>
    <link rel="stylesheet" type="text/css" media="all" href="css/popup-slider.css" />
    <style type="text/css">
        body {
            line-height: 13px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div>
            <iframe id="iframe" runat="server" class="viewer" style="width: 100%; height: 90vh;"></iframe>
        </div>
        <section class="drawer">
            <!-- <div> -->
            <header class="clickme">File Details</header>
            <!-- </div> -->
            <div class="drawer-content">
                <div class="drawer-items">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="container-fluid">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="widget-box">
                                            <div class="form-horizontal">
                                                <asp:GridView ID="gvMetaDataContent" runat="server" class="table table-bordered table-striped"
                                                    AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                SN.
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Metadata">
                                                            <ItemTemplate>
                                                                <%# Eval("Name") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Content">
                                                            <ItemTemplate>
                                                                <%# Eval("MetaDataContent") %>
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
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="widget-box">
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <label class="control-label">File Name :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblFileName" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Entry Date :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblEntryDate" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">File Attachment :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblFileAttachment" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Full Text Copy :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblFullTextCopy" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="widget-box">
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <label class="control-label">Upload Date :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblUploadDate" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Uploaded By :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblUploadedBy" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Modified By :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblModifiedBy" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Modified Date :</label>
                                                    <div class="controls">
                                                        <asp:Label ID="lblModifiedDate" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </section>

        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
        <script type="text/javascript" src="js/jquery.slidedrawer.min.js"></script>
        <script>
            $(function () {
                $('.drawer').slideDrawer({
                    showDrawer: false,
                    // slideTimeout: true,
                    slideSpeed: 600,
                    slideTimeoutCount: 3000,
                });
            });
	</script>
    </form>
</body>
</html>
