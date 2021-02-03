<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registration.aspx.cs" Inherits="app_sec_ica.registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        // password complexity
        function validate() {
            var password = document.getElementById("<%= password.ClientID %>");
            var button = document.getElementById("<%= submit.ClientID %>");
            var error = document.getElementById("error_msg");
            var hasError = false;
            // search for numbers, small, and capitol letters
            if (password.value.search(/[0-9]/) != -1 && password.value.search(/[a-z]/) != -1 && password.value.search(/[A-Z]/) != -1 && password.value.length >= 8) {
                // password passed
                button.disabled = false;
            } else {
                hasError = true;
                button.disabled = true;
                error.value = "password must contain 1 number, 1 lowercase and 1 uppercase character."
            }

            // check if other fields are empty
            button.disabled = isEmpty([document.getElementById("<%=first_name%>"),
                document.getElementById("<%=last_name%>"),
                document.getElementById("<%=credit_card%>"),
                document.getElementById("<%=email%>"),
                document.getElementById("<%=dob%>")])
        }

        function isEmpty(fields) {
            var button = document.getElementById("<%= submit.ClientID %>");
            var hasEmpty = false;
            for (var field of fields) {
                if (field.value == "") hasEmpty = true;
            }
            return hasEmpty;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Registration</h1>
            
            First Name:
            <asp:TextBox ID="first_name" runat="server"></asp:TextBox>
            <br />
            Last Name:
            <asp:TextBox ID="last_name" runat="server"></asp:TextBox>
            <br />
            Credit Card:
            <asp:TextBox ID="credit_card" runat="server"></asp:TextBox>
            <br />
            Email:
            <asp:TextBox ID="email" runat="server" TextMode="Email"></asp:TextBox>
            <br />
            Password:
            <asp:TextBox ID="password" runat="server" onkeyup="javascript:validate()" TextMode="Password"></asp:TextBox>
            <p id="error_msg"></p>
            <asp:Label runat="server" ID="error" Visible="false"></asp:Label>
            <br />
            Date of Birth:
            <asp:TextBox ID="dob" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="submit" runat="server" Text="Register" OnClick="onSubmit"/>
    </form>
</body>
</html>
