<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="app_sec_ica.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://google.com/recaptcha/api.js?render=6LdF2kcaAAAAAGHGqhzDOs6Ou4M_vwPb6_PEerRY"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Login</h1>
            <asp:Label runat="server" ID="error"></asp:Label>

            Email: 
            <asp:TextBox ID="email" runat="server"></asp:TextBox>
            <br />
            Password:
            <asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox>
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        </div>
        <asp:Button ID="submit" runat="server" Text="Log In" OnClick="onSubmit"/>
    </form>
    <script>
        grecaptcha.ready(() => {
            grecaptcha.execute("6LdF2kcaAAAAAGHGqhzDOs6Ou4M_vwPb6_PEerRY", { action: "login" }).then(token => {
                document.getElementById("g-recaptcha-response").value = token;
            })
        })
    </script>
</body>
</html>
