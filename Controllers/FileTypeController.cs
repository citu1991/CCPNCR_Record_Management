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
    public class FileTypeController : Controller
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
        // GET: FileType
        public ActionResult InsertFileType()
        {
            
            return View(db.LoadFileType());
        }

        [HttpPost]
        public ActionResult InsertFileType(FileType model)
        {
            if (model.FileTypeName!=null)
            {
                string res = db.insertFileType(model);
                if (res== "Success")
                {
                    ViewBag.Message = "Success";
                    ModelState.Clear();
                    return View(db.LoadFileType());
                }
                else { ViewBag.Message = "Failed"; ModelState.Clear();  return View(db.LoadFileType()); }
            }
            else { ViewBag.Message = "Alert"; ModelState.Clear();  return View(db.LoadFileType()); }
        }
        public PartialViewResult EditFileType(int fileTypeId)
        {
            if (fileTypeId>0) 
            {                
                return PartialView("_EditFileType", db.GetFileType(fileTypeId));

            } else 
            { 
                ViewBag.Message = "Alert";               
                return PartialView("_EditFileType", db.GetFileType(fileTypeId));
            }           


        }

    }
}