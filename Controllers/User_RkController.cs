using CCPNCR_Record_Management.App_Data;
using CCPNCR_Record_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CCPNCR_Record_Management.Controllers
{
    [HandleError]
    [SessionAuthorize]
    public class User_RkController : Controller
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
        // GET: User_Rk

        public string GetRKRemarks(int FileRecordId, int FileIn_OutRecordId)
        {
            return db.GetRKRemarks(FileRecordId, FileIn_OutRecordId);
        }

        public ActionResult FileInsideRecord()
        {
            return View(db.FileInsideRecord());
        }
        public ActionResult FileOutsideRecord()
        {
            return View(db.FileOutsideRecord());
        }

        public ActionResult FilePendingforApproval()
        {
            int userid=int.Parse(Session["UserId"].ToString());
            return View(db.filePendingforApproval(userid));
        }

        public ActionResult User_RkDashBoard()
        {
            CountFiles files = new CountFiles();
            files.List_CountFiles = db.Count();
            return View(files);

        }


    }
}