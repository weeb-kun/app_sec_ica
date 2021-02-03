using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace app_sec_ica
{
    public partial class changePw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void change(Object sender, EventArgs ea)
        {
            if (new_pw.Text.Length >= 8 &&
                Regex.IsMatch(new_pw.Text, "[a-z]") &&
                Regex.IsMatch(new_pw.Text, "[A-Z]") &&
                Regex.IsMatch(new_pw.Text, "[0-9]"))
            {
                if(old_pw.Text != new_pw.Text)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
                    {
                        con.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter("select * from account where email = @email", con))
                        {
                            da.SelectCommand.Parameters.AddWithValue("@email", Session["username"].ToString());
                            DataSet data = new DataSet();
                            da.Fill(data);
                            if (data.Tables[0].Rows.Count >= 1)
                            {
                                // validate old pw
                                String salt = data.Tables[0].Rows[0]["salt"].ToString();
                                if (Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(old_pw.Text + salt))) == data.Tables[0].Rows[0]["password"].ToString())
                                {
                                    // hash pw
                                    byte[] s = new byte[8];
                                    new RNGCryptoServiceProvider().GetBytes(s);
                                    String newSalt = Convert.ToBase64String(s);
                                    String pw = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(new_pw.Text.Trim() + newSalt)));
                                    // update new pw and salt
                                    using (SqlCommand command = new SqlCommand("update account set password = @pw, salt = @salt where email = @email", con))
                                    {
                                        command.Parameters.addWithValue("@pw", pw)
                                            .addWithValue("@salt", newSalt)
                                            .addWithValue("@email", Session["username"].ToString());
                                        command.ExecuteNonQuery();
                                    }
                                    using (SqlCommand command = new SqlCommand("update account set last_change = current_timestamp where email = @email", con))
                                    {
                                        command.Parameters.addWithValue("@email", Session["username"].ToString());
                                        command.ExecuteNonQuery();
                                    }
                                    Session["msg"] = "password successfully changed.";
                                    Response.Redirect("home");
                                }
                            }
                        }
                    }
                } else
                {
                    msg.Text = "new password cannot be the same as the old password.";
                }
            } else
            {
                msg.Text = "new password must contain a lowercase, an uppercase character, and a number.";
            }
        }
    }
}
