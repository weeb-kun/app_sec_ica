using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_sec_ica
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void onSubmit(Object sender, EventArgs e)
        {
            bool result;
            // validate captcha
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create($"https://google.com/recaptcha/api/siteverify?secret={Environment.GetEnvironmentVariable("app_sec_ica_captcha_secret")}&response={Request.Form["g-recaptcha-response"]}")).GetResponse())
            {
                using(StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // get response from captcha
                    result = Convert.ToBoolean(new JavaScriptSerializer().Deserialize<Response>(reader.ReadToEnd()).success);
                }
            }
            // authenticate user
            // get pw from db and compare with input
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                con.Open();
                using(SqlDataAdapter da = new SqlDataAdapter("select * from account where email = @email", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@email", email.Text.Trim());
                    DataSet data = new DataSet();
                    da.Fill(data);
                    if(data.Tables[0].Rows.Count >= 1)
                    {
                        // get salt
                        String salt = data.Tables[0].Rows[0]["salt"].ToString();
                        // get hash
                        // compare pw
                        if(Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(password.Text + salt))) == data.Tables[0].Rows[0]["password"].ToString())
                        {
                            // authenticated
                            // check if not on lockout
                            using(SqlDataAdapter checkLockout = new SqlDataAdapter("select * from account where email = @email", con))
                            {
                                checkLockout.SelectCommand.Parameters.AddWithValue("@email", email.Text);
                                DataSet set = new DataSet();
                                checkLockout.Fill(set);
                                if(set.Tables[0].Rows.Count >= 0)
                                {
                                    DataRow row = set.Tables[0].Rows[0];
                                    if ((int)row["failures"] < 3)
                                    {
                                        // check if last password change is more than 15mins ago
                                        if((DateTime)row["last_change"] < DateTime.Now.AddMinutes(-15))
                                        {
                                            Session["msg"] = "you must change your password now.";
                                        }

                                        // set session and redirect to home
                                        Session["username"] = data.Tables[0].Rows[0]["email"].ToString();
                                        Session["auth"] = Guid.NewGuid().ToString();
                                        Response.Cookies.Add(new HttpCookie("auth", Session["auth"].ToString()));
                                        Response.Redirect("home");
                                    } else
                                    {
                                        if((DateTime)row["lockout"] > DateTime.Now)
                                        {
                                            // havent expire
                                            error.Text = $"Sorry. your account has been locked out due to many unsuccessful attempts at logging in.\nPlease wait another {((DateTime)row["lockout"] - DateTime.Now).Minutes} Minutes.";
                                            return;
                                        } else
                                        {
                                            // expired. reset failures
                                            using (SqlCommand command = new SqlCommand("update account set failures = 0 where email = @email", con))
                                            {
                                                command.Parameters.AddWithValue("@email", email.Text);
                                                command.ExecuteNonQuery();
                                            }
                                            // set session and redirect to home
                                            Session["username"] = data.Tables[0].Rows[0]["email"].ToString();
                                            Session["auth"] = Guid.NewGuid().ToString();
                                            Response.Cookies.Add(new HttpCookie("auth", Session["auth"].ToString()));
                                            Response.Redirect("home");
                                        }
                                    }
                                }
                            }
                            
                        } else
                        {
                            // wrong pw
                            // check if reached 3 failures
                            using(SqlDataAdapter adapter = new SqlDataAdapter("select failures from account where email = @email", con))
                            {
                                adapter.SelectCommand.Parameters.AddWithValue("@email", email.Text);
                                DataSet ds = new DataSet();
                                adapter.Fill(ds);
                                if(ds.Tables[0].Rows.Count >= 1)
                                {
                                    if((int)ds.Tables[0].Rows[0]["failures"] >= 3)
                                    {
                                        error.Text = "You have entered the wrong password 3 times. Please try again later.";
                                        // set lockout time
                                        using(SqlCommand lockout = new SqlCommand("update account set lockout = @dt where email = @email", con))
                                        {
                                            // set 5 min lockout
                                            lockout.Parameters.AddWithValue("@dt", DateTime.Now.AddMinutes(5).ToString("dd/mm/yyyy hh:mm"));
                                            lockout.Parameters.AddWithValue("@email", email.Text);
                                            lockout.ExecuteNonQuery();
                                        }
                                        return;
                                    } else
                                    {
                                        error.Text = "Username or password incorrect.";
                                    }
                                }
                            }
                            // add 1 failure to db
                            using(SqlCommand command = new SqlCommand("update account set failures = failures + 1 where email = @email", con))
                            {
                                command.Parameters.AddWithValue("@email", email.Text);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }

    class Response
    {
        public String success { get; set; }
        public List<String> errorMessage { get; set; }
    }
}