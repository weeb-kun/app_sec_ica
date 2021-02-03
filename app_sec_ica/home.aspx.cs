using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                if (Session["msg"] != null)
                {
                    msg.Text = Session["msg"].ToString();
                    msg.Visible = true;
                }
            } else
            {
                Response.StatusCode = 403;
                Response.End();
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

        protected void changePw(Object sender, EventArgs ea)
        {
            // check if pw was changed less than 5mins ago
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                con.Open();
                using(SqlDataAdapter adapter = new SqlDataAdapter("select * from account where email = @email", con))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@email", Session["username"].ToString());
                    DataSet set = new DataSet();
                    adapter.Fill(set);
                    if(set.Tables[0].Rows.Count >= 0)
                    {
                        if((DateTime)set.Tables[0].Rows[0]["last_change"] > DateTime.Now.AddMinutes(-5))
                        {
                            msg.Text = "you are not allowed to change your password within 5 minutes of changing your password.";
                            msg.Visible = true;
                        }
                    }
                }
            }
        }
    }
}