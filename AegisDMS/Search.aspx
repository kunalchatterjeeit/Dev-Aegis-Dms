<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="AegisDMS.Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="js/custom.js"></script>

    <script>
        function openNav() {
            document.getElementById("mySidenav").style.width = "100%";
        }

        function closeNav() {
            document.getElementById("mySidenav").style.width = "0";
        }
    </script>

    <link href="js/AutoComplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/AutoComplete/jquery.min.js"></script>
    <script type="text/javascript" src="js/AutoComplete/jquery-ui.min.js"></script>
    <script type="text/javascript">
        function GetAutocompleteMetadataValues() {
            var e = document.getElementById("<%=ddlMetadata.ClientID %>");
            var mdvalue = e.options[e.selectedIndex].value;

            $("#txtMetadataValue").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "InternalServices.asmx/LoadAutoCompleteMetadata",
                        data: "{'metadataId':'" + mdvalue + "','searchContent':'" + document.getElementById('txtMetadataValue').value + "'}",
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

    <style>
        body {
            font-family: "Lato", sans-serif;
        }

        .sidenav {
            height: 100%;
            width: 0;
            position: fixed;
            z-index: 1;
            top: 0;
            left: 0;
            background-color: #fcfcfc;
            overflow-x: hidden;
            transition: 0.7s;
            padding-top: 10px;
        }

            .sidenav a {
                padding: 8px 8px 8px 32px;
                text-decoration: none;
                font-size: 25px;
                color: #818181;
                display: block;
                transition: 0.3s;
            }

                .sidenav a:hover {
                    color: #f1f1f1;
                }

            .sidenav .closebtn {
                position: absolute;
                top: 0;
                right: 25px;
                font-size: 36px;
                margin-left: 50px;
            }

        @media screen and (max-height: 450px) {
            .sidenav {
                padding-top: 15px;
            }

                .sidenav a {
                    font-size: 18px;
                }
        }

        .dataList {
            padding: 12px 15px;
            background: #eaf4fc;
            border: 1px solid #0349AA;
            border-radius: 5px;
            display: inline-block;
            font-size: 18px;
            line-height: 1.3333333;
        }
    </style>

    <style>
        input[type=text].bigSearch {
            width: 50%;
            box-sizing: border-box;
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 16px;
            background-color: white;
            background-position: 10px 10px;
            background-repeat: no-repeat;
            padding: 12px 20px 12px 7px;
            -webkit-transition: width 0.4s ease-in-out;
            transition: width 0.4s ease-in-out;
            margin: 5px;
        }

        input[type=text]:focus.bigSearch {
            width: 90%;
        }

        .possition {
            position: relative;
            background: url('img/custom/searchicon.png') no-repeat center center;
            width: 22px;
            height: 22px;
            border: none;
            margin-left: -50px;
            top: 3px;
        }

        .text {
            padding-right: 22px;
        }

        .searchArea {
            margin: 0 auto;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
        }

        .dropdown-menu222 {
            top: 100%;
            left: 0;
            float: left;
            min-width: 200px;
            padding: 5px 0;
            margin: 2px 10px 0;
            font-size: 17px;
            text-align: left;
            list-style: none;
            background-color: #fff;
            -webkit-background-clip: padding-box;
            background-clip: padding-box;
            border: 1px solid #ccc;
            border: 1px solid rgba(0,0,0,.15);
            border-radius: 4px;
            -webkit-box-shadow: 0 6px 12px rgba(0,0,0,.175);
            box-shadow: 0 6px 12px rgba(0,0,0,.175);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="searchArea">
            <div id="search">
                <asp:TextBox ID="txtQuickSearch" runat="server" class="text bigSearch" autocomplete="off" placeholder="Search.."></asp:TextBox>
                <asp:Button ID="btnQuickSearch" runat="server" class="possition" OnClick="btnQuickSearch_Click" />
            </div>
        </div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div id="mySidenav" class="sidenav">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <a href="javascript:void(0)" class="closebtn" onclick="closeNav()">&times;</a>
                    <h3>Advance Search Criteria</h3>
                    <div class="form-group">
                        <asp:DropDownList ID="ddlFileCategory" class="dropdown-menu222" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFileCategory_SelectedIndexChanged"></asp:DropDownList>
                        <asp:DropDownList ID="ddlFileType" class="dropdown-menu222" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFileType_SelectedIndexChanged"></asp:DropDownList>
                        <asp:DropDownList ID="ddlMetadata" CssClass="dropdown-menu222" runat="server"></asp:DropDownList>
                        <asp:TextBox ID="txtMetadataValue" CssClass="form-control" Style="max-width: 200px; margin: 5px 10px; font-size: 15px" runat="server" onkeydown="javascript:GetAutocompleteMetadataValues()"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnAdd" Style="width: 200px; margin: 5px 10px" runat="server" Text="ADD" class="btn bg-primary btn-lg" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnAND" Style="width: 200px; margin: 5px 8px" runat="server" Text="AND" class="btn bg-success btn-lg" OnClick="btnAND_Click" />
                        <asp:Button ID="btnOR" Style="width: 200px; margin: 5px 5px" runat="server" Text="OR" class="btn bg-danger btn-lg" OnClick="btnOR_Click" />
                    </div>
                    <div class="form-group">
                        <asp:DataList ID="dlQuery" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                            Style="margin: 10px; width: 100%">
                            <ItemTemplate>
                                <asp:Label ID="lblMetaData" runat="server" Text='<%# Eval("MetadataName") + 
                        (!string.IsNullOrEmpty(Eval("MetadataValue").ToString())? " = " +Eval("MetadataValue").ToString():string.Empty) %>'
                                    CssClass="dataList"></asp:Label>

                                <asp:HiddenField ID="hdnMetadata" runat="server" Value='<%# Eval("MetadataId") + 
                        (!string.IsNullOrEmpty(Eval("MetadataValue").ToString())? " = " +Eval("MetadataValue").ToString():string.Empty) %>' />
                            </ItemTemplate>
                        </asp:DataList>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Button ID="btnSearch" Style="width: 200px; margin: 5px 10px" runat="server" Text="Search" class="btn bg-warning btn-lg" OnClick="btnSearch_Click" />
        </div>
        <a href="Dashboard.aspx">Back to Dashboard</a>
        <br />
        <br />
        <span style="font-size: 25px; cursor: pointer;" onclick="openNav()">&#9776; Advance Search</span>
        <br />
        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
        <br />
        <div class="container">
            <asp:GridView ID="gvFile" runat="server" class="table table-bordered table-striped"
                AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnRowDataBound="gvFile_RowDataBound"
                DataKeyNames="FileGuid, FileName" OnPageIndexChanging="gvFile_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemTemplate>
                            <a id="anchrPopUp" runat="server" style="cursor: pointer; color: #000000"
                                title='<%# Eval("FileName") %>'><span style="float: left; white-space: normal; text-decoration: underline;">
                                    <%# (Eval("FileName").ToString().Length > 80) ? Eval("FileName").ToString().Substring(0, 80) + " ..." : Eval("FileName").ToString()%></span></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Category">
                        <ItemTemplate>
                            <%# Eval("FileCategory") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Type">
                        <ItemTemplate>
                            <%# Eval("FileType") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Entry Date">
                        <ItemTemplate>
                            <%# Eval("EntryDate") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
