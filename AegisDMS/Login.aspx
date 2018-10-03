<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AegisDMS.Login" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/bootstrap-responsive.min.css" />
    <link rel="stylesheet" href="css/matrix-login.css" />
    <link href="font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,700,800' rel='stylesheet' type='text/css'>
    
</head>
<body>
    <form id="form1" runat="server">
        <div id="loginbox">
             <img src="img/custom/dms_logo.jpg" ID="dms_logo" alt="Logo" style="margin-left:60px"  />
            <img src="img/custom/Aegis_Docu_Search.png" ID="Aegis_Docu_Search" alt="Logo" style="margin-left:230px;margin-top:-120px;height:100px;width:150px;"  />
            
            <form id="loginform" class="form-vertical">
                <div class="control-group normal_text">
                    <h3>
                       
                        Aegis Docu-Search
                    </h3>
                </div>
                <div class="control-group">
                    <div class="controls">
                        <div class="main_input_box">
                            <span class="add-on bg_lg"><i class="icon-user"></i></span>
                            <asp:TextBox ID="txtUserName" runat="server" placeholder="Username"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="control-group">
                    <div class="controls">
                        <div class="main_input_box">
                            <span class="add-on bg_ly"><i class="icon-lock"></i></span>
                            <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-actions">
                    <span class="pull-left"><a href="#" class="flip-link btn btn-info" id="to-recover">Lost password?</a></span>
                    <span class="pull-right">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-success" OnClick="btnLogin_Click" />
                        <asp:Label ID="lblMessage" runat="server" Text="Invalid username/password." Style="color: red;"></asp:Label>
                    </span>
                </div>
            </form>
            
        </div>

        <script src="js/jquery.min.js"></script>
        <script src="js/matrix.login.js"></script>
    </form>
</body>
</html>
