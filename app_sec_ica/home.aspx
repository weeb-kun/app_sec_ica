<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="app_sec_ica.home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label runat="server" ID="msg" Visible="false"></asp:Label>
        <br />
        <div>
            Username: <asp:Label ID="username" runat="server"></asp:Label>
            <asp:Button ID="log_out" runat="server" Text="Logout" OnClick="logout"/>
        </div>
        <div>
            <asp:Button runat="server" ID="change_pw" Text="Change Password" OnClick="changePw"/>
        </div>
    </form>
</body>
</html>
