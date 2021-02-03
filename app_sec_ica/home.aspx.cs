using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_sec_ica
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["username"] != null &&
                Session["auth"] != null &&
                Request.Cookies["auth"] != null &&
                Session["auth"].ToString() == Request.Cookies["auth"].Value)
            {
                username.Text = Session["username"].ToString();
            }
        }

        protected void logout(Object sender, EventArgs e)
        {
            // clear session
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            // reset and expire cookies
            if(Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = "";
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-1);
            }

            if(Request.Cookies["auth"] != null)
            {
                Response.Cookies["auth"].Value = "";
                Response.Cookies["auth"].Expires = DateTime.Now.AddMonths(-1);
            }

            Response.Redirect("login");
        }
    }
}