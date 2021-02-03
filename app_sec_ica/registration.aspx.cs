using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Ajax.Utilities;
using System.Data.SqlClient;
using System.Configuration;

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
                // encrypt pw and credit card and save to db
                byte[] s = new byte[8];
                new RNGCryptoServiceProvider().GetBytes(s);
                String salt = Convert.ToBase64String(s);
                String pw = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(password.Text.Trim() + salt)));

                // encrypt credit card
                byte[] buffer = Encoding.UTF8.GetBytes(credit_card.Text.Trim());
                RijndaelManaged aes = new RijndaelManaged().generateKey();
                byte[] cipher = aes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);

                // save into db
                using(SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
                {
                    con.Open();
                    using(SqlCommand command = new SqlCommand("insert into account values (@fn, @ln, @cc, @email, @pw, @salt, @iv, @key, 0, current_timestamp, current_timestamp)", con))
                    {
                        command.Parameters.addWithValue("@fn", first_name.Text)
                            .addWithValue("@ln", last_name.Text)
                            .addWithValue("@cc", Convert.ToBase64String(cipher))
                            .addWithValue("@email", email.Text)
                            .addWithValue("@pw", pw)
                            .addWithValue("@salt", salt)
                            .addWithValue("@iv", Convert.ToBase64String(aes.IV))
                            .addWithValue("@key", Convert.ToBase64String(aes.Key));
                        command.ExecuteNonQuery();
                    }
                }
                Response.Redirect("login");
            } else error.Text = "Please check your password again.";
            
        }
    }

    public static class Util
    {
        public static RijndaelManaged generateKey(this RijndaelManaged cipher)
        {
            cipher.GenerateKey();
            cipher.GenerateIV();
            return cipher;
        }

        public static SqlParameterCollection addWithValue(this SqlParameterCollection parameters, String key, String value)
        {
            parameters.AddWithValue(key, value);
            return parameters;
        }
    }
}