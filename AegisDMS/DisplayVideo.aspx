<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayVideo.aspx.cs" Inherits="AegisDMS.DisplayVideo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <video width="400" controls="controls" >
                <source src="Files/Raw/test2.mp4"/>
            </video>
        </div>
    </form>
</body>
</html>
