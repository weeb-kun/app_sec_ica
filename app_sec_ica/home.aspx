<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="app_sec_ica.home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="username" runat="server"></asp:Label>
            <asp:Button ID="log_out" runat="server" Text="Logout" OnClick="logout"/>
        </div>
    </form>
</body>
</html>
