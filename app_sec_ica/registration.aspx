<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registration.aspx.cs" Inherits="app_sec_ica.registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Registration</h1>
            
            First Name:
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <br />
            Last Name:
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <br />
            Credit Card:
            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            <br />
            Email:
            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
            <br />
            Password:
            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            <br />
            Date of Birth:
            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="Button1" runat="server" Text="Register" OnClick="onSubmit"/>
    </form>
</body>
</html>
