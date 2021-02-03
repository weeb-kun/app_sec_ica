using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace app_sec_ica
{
    public partial class registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void onSubmit(Object sender, EventArgs e)
        {
            // todo finish registration logic

            // validate input
            if(password.Text.Length >= 8 &&
                Regex.IsMatch(password.Text, "[a-z]") &&
                Regex.IsMatch(password.Text, "[A-Z]") &&
                Regex.IsMatch(password.Text, "[0-9]"))
            {
                // contains lowercase, uppercase and numbers, and at least 8 characters
                // encrypt pw and save to db
            } else error.Text = "Please check your password again.";
            
        }
    }
}