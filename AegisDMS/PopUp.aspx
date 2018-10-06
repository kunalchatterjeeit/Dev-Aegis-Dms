<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopUp.aspx.cs" Inherits="AegisDMS.PopUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel='stylesheet prefetch' href='https://cdnjs.cloudflare.com/ajax/libs/magnific-popup.js/1.1.0/magnific-popup.css'>
    <link href="css/popup-control.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/prefixfree/1.0.7/prefixfree.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="inline-popups">
                <a href="#test-popup" data-effect="mfp-3d-unfold">3d unfold</a>
            </div>

            <!-- Popup itself -->
            <div id="test-popup" class="white-popup mfp-with-anim mfp-hide">
                You may put any HTML here. This is dummy copy. It is not meant to be read. 
                It has been placed here solely to demonstrate the look and feel of finished, typeset text. 
                Only for show. He who searches for meaning here will be sorely disappointed.
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Button" />
            </div>


            <script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
            <script src='https://cdnjs.cloudflare.com/ajax/libs/magnific-popup.js/1.1.0/jquery.magnific-popup.min.js'></script>
            <script src="js/custom/popup-control.js"></script>
        </div>
    </form>
</body>
</html>
