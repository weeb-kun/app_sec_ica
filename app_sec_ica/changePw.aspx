<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changePw.aspx.cs" Inherits="app_sec_ica.changePw" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Old Password: 
            <asp:TextBox ID="old_pw" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            New Password:
            <asp:TextBox ID="new_pw" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <asp:Label runat="server" ID="msg" Visible="false"></asp:Label>
        <asp:Button ID="submit" runat="server" Text="Change" OnClick="change"/>
    </form>
</body>
</html>
