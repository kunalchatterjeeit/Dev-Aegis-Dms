<%@ Page Title="" Language="C#" MasterPageFile="~/MainLayout.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="AegisDMS.Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Search File</title>
    <style>
        .dataList
        {
            padding: 3px 3px;
            background: #eaf4fc;
            border: 1px solid #0349AA;
            border-radius: 5px;
            display: inline-block;
        }
        body
        {
            font-size: 0.71em;
        }
        
    </style>
    <style type="text/css">
        .HeaderRow th
        {
            background: #0349AA;
            font-size: 8pt;
            color: #fff;
        }
        .HeaderRow th a
        {
            color: #fff;
        }
    </style>
    <script type="text/javascript">
        function openpopup(poplocation) {
            var popposition = 'left = 250, top=40, width=1000,align=center, height=580,menubar=no, scrollbars=yes, resizable=yes';

            var NewWindow = window.open(poplocation, '', popposition);
            if (NewWindow.focus != null) {
                NewWindow.focus();
            }
        }
    </script>
    <script type="text/javascript">
        function MetaDataValidation() {
            if (document.getElementById("<%=ddlMetaData.ClientID %>").selectedIndex == 0) {
                alert('Select Meta Data');
                return false;
            }
            if (document.getElementById("<%=txtSearch.ClientID %>").value == 'Search by Meta Data Content') {
                alert('Enter Meta Data Content');
                return false;
            }
        }
    </script>
    <style type="text/css">
        .WindowsStyle .ajax__combobox_itemlist
        {
            background-color: #EF7F00;
            padding-left: 5px;
            margin: 0;
        }
        .WindowsStyle .ajax__combobox_itemlist li
        {
            background-color: #EF7F00;
            color: #fff;
        }
        
        .WindowsStyle .ajax__combobox_inputcontainer .ajax__combobox_textboxcontainer input
        {
            margin: 0 0 0 0;
            float: left;
            border: solid 1px #DDDDDD;
            color: #000000;
            font-weight: normal;
            background-color: #fff;
            padding: 5px 0 5px 5px;
        }
        
        .WindowsStyle .ajax__combobox_inputcontainer .ajax__combobox_buttoncontainer button
        {
            background-image: url(../Images/bing.jpg);
            background-position: center;
            background-repeat: no-repeat;
            cursor: pointer;
            display: none;
        }
    </style>
    <style type="text/css">
        .checkboxes label
        {
            display: block;
            float: left;
            padding-right: 10px;
            white-space: nowrap;
            float: right;
            font-weight: bold;
        }
        .checkboxes input
        {
            vertical-align: middle;
            float: right;
            font-weight: bold;
        }
        .checkboxes label span
        {
            vertical-align: middle;
            float: right;
            font-weight: bold;
        }
    </style>
    <style type="text/css">
        .text
        {
            font-family: Helvetica;
            font-size: 13px;
            font-weight: normal;
            padding: 7px 0 0 15px;
        }
        .style1
        {
            width: 28%;
        }
        .style2
        {
            width: 223px;
        }
        .divWaiting
        {
            position: absolute;
            background-color: #FAFAFA;
            z-index: 2147483647 !important;
            opacity: 0.8;
            overflow: hidden;
            text-align: center;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            padding-top: 20%;
        }
    </style>
    <style type="text/css">
        .selected_row
        {
            background-color: #ffddba;
        }
    </style>
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                $("[id*=gvFiles] td").bind("click", function () {
                    var row = $(this).parent();
                    $("[id*=gvFiles] tr").each(function () {
                        if ($(this)[0] != row[0]) {
                            $("td", this).removeClass("selected_row");
                        }
                    });
                    $("td", row).each(function () {
                        if (!$(this).hasClass("selected_row")) {
                            $(this).addClass("selected_row");
                        } else {
                            $(this).removeClass("selected_row");
                        }
                    });
                });
            });
        }
    </script>
    <link href="js/AutoComplete/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/AutoComplete/jquery.min.js"></script>
    <script type="text/javascript" src="js/AutoComplete/jquery-ui.min.js"></script>
    <script type="text/javascript">
        function SearchText() {
            var e = document.getElementById("<%=ddlMetaData.ClientID %>");
            var mdvalue = e.options[e.selectedIndex].value;

            $("#ctl00_ContentPlaceHolder1_txtSearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "GetMetaData.asmx/LoadAutoCompleteMetadata",
                        data: "{'metadataId':'" + mdvalue + "','searchContent':'" + document.getElementById('ctl00_ContentPlaceHolder1_txtSearch').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert(result.responseText);
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" runat="server" ImageAlign="Middle" ImageUrl="Images/loading-bar.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; overflow: hidden; margin: -15px 0px" onload="SearchText()">
                <fieldset class="login">
                    <legend>Enter file search criteria</legend>
                    <table style="padding: 0 0 0 10px; width: 100%; text-align:left">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlFileCategory" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlFileCategory_SelectedIndexChanged" CssClass="dropdown_list">
                                    <asp:ListItem Value="0" Text="Select File Category"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFileType" runat="server" CssClass="dropdown_list" 
                                    Width="150px" AutoPostBack="True" 
                                    onselectedindexchanged="ddlFileType_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="Select File Type"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileName" runat="server" CssClass="textEntry" placeholder=" Item Name"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="textEntry" placeholder=" Entry From Date"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd MMM yyyy"
                                    TargetControlID="txtFromDate">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="textEntry" placeholder=" Entry To Date"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd MMM yyyy"
                                    TargetControlID="txtToDate">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileContent" runat="server" CssClass="textEntry" Width="150px"
                                    placeholder=" File Content"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlMetaData" runat="server" CssClass="dropdown_list" 
                                    Width="150px">
                                    <asp:ListItem Text="Select Meta Data" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <input type="text" id="txtSearch" class="textEntry" runat="server" style="width: 150px"
                                    placeholder="Meta Data Content" onkeydown="javascript:SearchText()" />
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td colspan="2">
                                <asp:Button ID="btnAdd" runat="server" CssClass="button" OnClick="btnAdd_Click" 
                                    OnClientClick="javascript:return MetaDataValidation()" Style="height: 27px" Text="Add" />
                                <asp:Button ID="btnReset" runat="server" CssClass="button" 
                                    OnClick="btnReset_Click" Style="height: 27px" Text="Reset" />
                                <asp:Button ID="btnSearch" runat="server" CssClass="button" 
                                    OnClick="btnSearch_Click" Style="height: 27px" Text="Search" />
                            </td>
                        </tr>
                    </table>
                    <asp:DataList ID="dlMetaData" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        Style="margin: 10px; width: 100%">
                        <ItemTemplate>
                            <asp:Label ID="lblMetaData" runat="server" Text='<%# Eval("MetaDataContent") %>'
                                CssClass="dataList"></asp:Label>
                        </ItemTemplate>
                    </asp:DataList>
                    <asp:HiddenField ID="hdnMetaDataIds" runat="server" />
                </fieldset>
            </div>
            <div style="clear: both">
            </div>
            <div style="width: 100%; float: right; margin: -10px 0px">
                <fieldset class="login">
                    <legend>File search result</legend>
                    <table style="padding: 0 0 0 10px; width: 100%; height: 102vh; margin: 0px 0px">
                        <tr>
                            <td style="width: 100%">
                                <b style="font-size: 11px">Total No. of Files: <span id="FileCount" runat="server"></span>
                                </b>
                                <asp:CheckBox ID="chkAllowPaging" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllowPaging_CheckedChanged"
                                    Checked="true" Text="Allow Paging" CssClass="checkboxes" />
                                <div style="height: 101vh; width: 100%; overflow: scroll">
                                    <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" CellPadding="4"
                                        ForeColor="#333333" GridLines="Horizontal" Width="100%" DataKeyNames="Id, Type, FileName"
                                        OnRowDataBound="gvFiles_RowDataBound" Style="white-space: nowrap;"
                                        OnPageIndexChanging="gvFiles_PageIndexChanging" PageSize="25" AllowSorting="false"
                                        ShowHeader="true" OnRowCommand="gvFiles_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkBtnItemName" runat="server" Text="Item Name" CommandName="Sort"
                                                        CommandArgument="FileName"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <a id="anchrPopUp" runat="server" style="text-decoration: underline; cursor: pointer;
                                                        color: #000000" title='<%# Eval("FileName") %>'><span style="float: left; white-space: normal">
                                                            <img id="ImgIcon" runat="server" alt="" width="13" />&nbsp;
                                                            <%# (Eval("FileName").ToString().Length > 80) ? Eval("FileName").ToString().Substring(0, 80) + " ..." : Eval("FileName").ToString()%></span></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkBtnFileRefNo" runat="server" Text="Doc. No" CommandName="Sort"
                                                        CommandArgument="FileRefNo"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("FileRefNo").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkBtnFileDate" runat="server" Text="File Date" CommandName="Sort"
                                                        CommandArgument="FileDate"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#(Eval("FileDate") == DBNull.Value || Eval("FileDate").ToString() == "0/0/0000 12:00:00 AM") ? "" : Convert.ToDateTime(Eval("FileDate")).ToString("dd MMM yyyy")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkBtnFileCategoryName" runat="server" Text="File Category" CommandName="Sort"
                                                        CommandArgument="FileCategoryName"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("FileCategoryName").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkBtnFileTypeName" runat="server" Text="File Type" CommandName="Sort"
                                                        CommandArgument="FileTypeName"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("FileTypeName").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Status</HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# (Eval("IsApproved").ToString() == "Approved") ? "Aprv." : "NT Aprv."%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# (Eval("FileAttachmentSign").ToString() == "1") ? "<img src='Images/Arrow-Right-icon.png' width='10px'/>" : "<img src='Images/blank-icon.png' width='10px'/>"%>
                                                    <a id="anchrFileAttachment" runat="server">File Attch.</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# (Eval("FileReferenceSign").ToString() == "1") ? "<img src='Images/Arrow-Right-icon.png' width='10px'/>" : "<img src='Images/blank-icon.png' width='10px'/>"%>
                                                    <a id="anchrReference" runat="server">Ref. Files</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle BackColor="#FFFEFC" />
                                        <AlternatingRowStyle BackColor="#E0E0E0" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle CssClass="HeaderRow" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <EmptyDataTemplate>
                                            No Result Found...
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle ForeColor="Black" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
