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
    public class FileVolumeController : Controller
    {

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
        DataBaseAccess db = new DataBaseAccess();
        // GET: FileVolume
        public ActionResult InsertFileVolume()
        {
            return View(db.LoadFileVolume());
        }

        [HttpPost]
        public ActionResult InsertFileVolume(FileVolume model)
        {
            if (model.FileVolumeName != null)
            {
                string res = db.insertFileVolume(model);
                if (res == "Success")
                {
                    ViewBag.Message = "Success";
                    ModelState.Clear();
                    return View(db.LoadFileVolume());
                }
                else { ViewBag.Message = "Failed"; ModelState.Clear(); return View(db.LoadFileVolume()); }
            }
            else { ViewBag.Message = "Alert"; ModelState.Clear(); return View(db.LoadFileVolume()); }
        }
        public PartialViewResult EditFileVolume(int fileVolumeId)
        {
            if (fileVolumeId > 0)
            {
                return PartialView("_EditFileVolume", db.GetFileVolume(fileVolumeId));

            }
            else
            {
                ViewBag.Message = "Alert";
                return PartialView("_EditFileVolume", db.GetFileVolume(fileVolumeId));
            }


        }
    }
}