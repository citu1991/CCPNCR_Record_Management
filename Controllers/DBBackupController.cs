using CCPNCR_Record_Management.App_Data;
using CCPNCR_Record_Management.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace CCPNCR_Record_Management.Controllers
{

    [HandleError]
    [SessionAuthorize]
    public class DBBackupController : Controller
    {



        DataBaseAccess db = new DataBaseAccess();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] != null)
            {
                ViewBag.NotificationCount = db.GetNotification(int.Parse(Session["UserId"].ToString()));
            }
            else
            {
                Session.Abandon();
                Session.RemoveAll();
                Session["UserId"] = null;
                Session["UserName"] = null;
                Session["UserFullName"] = null;
                Session["UserRoleType"] = null;
                FormsAuthentication.SignOut();
                filterContext.Result = new RedirectResult("/Login/Login");
            }

        }
        string str = ConfigurationManager.ConnectionStrings["RMSConnection"].ConnectionString;
        SqlConnection con = null;

        public ActionResult CreateBackup()
        {
            BackupModels BKM = new BackupModels();
            using (con = new SqlConnection(str))
            {
                string _result = "0";
                con.Open();
                SqlCommand cmd = new SqlCommand("spDBBackup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "LastBackup");
                cmd.Parameters.AddWithValue("@_result", _result);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                List<BackupModels> listBackupModels = new List<BackupModels>();
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BackupModels bkm = new BackupModels();
                        bkm.DB_BK_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["DB_BK_ID"].ToString());
                        bkm.BackUpFileName = ds.Tables[0].Rows[i]["BackUpFileName"].ToString();
                        bkm.Createdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Createdate"].ToString());
                        listBackupModels.Add(bkm);
                    }

                }
                BKM.listBackUp = listBackupModels;

            }
            con.Close();
            return View(BKM);
        }

        [HttpPost]
        public ActionResult CreateBackup(string create)
        {
            using (con = new SqlConnection(str))
            {
                int _min = 1000;
                int _max = 999999;
                Random _rdm = new Random();
                string _result = _rdm.Next(_min, _max).ToString();
                con.Open();
                SqlCommand cmd = new SqlCommand("spDBBackup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "CreateBackup");
                string path = Server.MapPath("~/DataBaseBackup/");
                cmd.Parameters.AddWithValue("@path", path);
                cmd.Parameters.AddWithValue("@_result", _result);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i >= 0)
                {
                    TempData["SuccessBk"] = cmd.Parameters["@Result"].Value.ToString();
                    return RedirectToAction("CreateBackup");
                }
                else
                {
                    TempData["ErrorBk"] = cmd.Parameters["@Result"].Value.ToString();
                    return RedirectToAction("CreateBackup");
                }
            }
        }

    }
}