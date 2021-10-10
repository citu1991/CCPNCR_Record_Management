
using CCPNCR_Record_Management.App_Data;
using CCPNCR_Record_Management.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.Controllers
{
    public class ResetPasswordController : Controller  
    {
        
        string conStr = ConfigurationManager.ConnectionStrings["RMSConnection"].ConnectionString;
        DataBaseAccess db = new DataBaseAccess();
        public string SendPwdResetLink(String UserEmail)
        {

            string resultflag = "";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("spResetPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserEmail", UserEmail.Trim());
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    
                    if (Convert.ToBoolean(rdr["ReturnCode"]))
                    {
                        resultflag = SendPasswordResetEmail(rdr["Email"].ToString(), rdr["UniqueId"].ToString());
                        if (resultflag.Equals("Mail Sent"))
                        {
                            return resultflag;
                        }
                        else
                        {
                            return resultflag;
                        }                       
                    }
                    else
                    {
                       return "NOReturnCode";
                    }
                }
                return "NOReturnCode";
            }
        }

        private String SendPasswordResetEmail(string ToEmail, string UniqueId)
        {
            String flag = "";
            try
            {
                MailMessage mailMessage = new MailMessage("programmer.ccpncr@gmail.com", ToEmail);
             
               
                // localhost

                var verifyUrl = "https://localhost:44370/ResetPassword/ResetUpdatePassword?rt=" + UniqueId;           


                 string resetLink = "Please click on the following link to reset your password: <a href='"+verifyUrl+ "'>Reset Password Link</a>";


                //string resetLink = "Please click on the following link to reset your password: <a href='"
                // + Url.Action("ResetUpdatePassword", "ResetPassword", new { rt = UniqueId }, "http")+"?"
                // + "'>Reset Password Link</a>";

                mailMessage.IsBodyHtml = true;
                mailMessage.Body = resetLink;
                mailMessage.Subject = "Reset Your Password";
                // SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 25);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "programmer.ccpncr",
                    Password = "gmail1991((9438"
                };
                smtpClient.EnableSsl = true;
                // Attempt to send the email
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
               
                flag = e.Message;
                return flag;
            }
            flag = "Mail Sent";
            return flag;
        }


       [HttpGet]
        
        public ActionResult ResetUpdatePassword(string rt)
        {

            string url = HttpContext.Request.Url.AbsoluteUri;
            // http://localhost:1302/TESTERS/Default6.aspx

            string path = HttpContext.Request.Url.AbsolutePath;
            // /TESTERS/Default6.aspx

            string host = HttpContext.Request.Url.Host;
            ResetPasswordModel model = new ResetPasswordModel();
            model.ReturnToken = rt;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetUpdatePassword(ResetPasswordModel resetModel)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            try
            {
                if (resetModel.Password!=null && resetModel.ReturnToken!=null)
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        SqlCommand cmd = new SqlCommand("spChangePassword", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@GUID", resetModel.ReturnToken.ToString());
                        cmd.Parameters.AddWithValue("@UserPassword", Encrypt(resetModel.Password));
                        con.Open();                        
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0 && dt != null)
                        {
                            int i =int.Parse(dt.Rows[0]["IsPasswordChanged"].ToString());
                            if (i==1)
                            {
                                ViewBag.Reset_Success = "Password Updated Successfully";
                            }
                            else if (i==0)
                            {
                                ViewBag.Reset_Fail = "Your Password Reset Link Expired,please request for new Password Reset Link";
                            }
                        }
                        else
                        {
                            ViewBag.Reset_Fail = "Your Password Reset Link Expired,please request for new Password Reset Link";
                           
                        }                        
                    }
                }
            }catch(Exception ee)
            {
                ViewBag.Error = "Message :" + ee.Message;
               
            }
            model.ReturnToken = resetModel.ReturnToken;
            return View(model);
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }



    }
}
