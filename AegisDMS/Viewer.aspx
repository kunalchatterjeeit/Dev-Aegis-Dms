<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewer.aspx.cs" Inherits="AegisDMS.Viewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/custom.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <iframe id="iframe" runat="server" class="viewer" style="width: 100%; height: 90vh;"></iframe>
        </div>
    </form>
</body>
</html>
