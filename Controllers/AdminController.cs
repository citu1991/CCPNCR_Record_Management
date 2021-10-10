using CCPNCR_Record_Management.App_Data;
using CCPNCR_Record_Management.Models;
using System.Collections.Generic;
using System.Web.Mvc;

using System.Web.Security;
using System.Web.SessionState;

namespace CCPNCR_Record_Management.Controllers
{
    [HandleError]
    [SessionAuthorize]
    public class AdminController : Controller
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
        // GET: Admin
       
        public ActionResult AdminDashBoard()
        {
            CountFiles files = new CountFiles();
            files.List_CountFiles= db.Count();
           
            return View(files); 
        }
        public ActionResult UsersActive_Inactive()
        {
            SignUp users = db.getActive_Inactive();
            return View(users);
        }

        public ActionResult UsersRoleAssign()
        {
            SignUpRole model = db.UsersRoleAssign();
            return View(model);
        }

        public PartialViewResult EditRole(int UserId)
        {          
           return PartialView("_EditRole",db.LoadRole(UserId));          
            
        }

        [HttpPost]
        public ActionResult UsersRoleAssign(SignUpRole model)
        {
            if (model.UserId > 0 && model.RoleId>0)
            {
                ViewBag.Message = db.AssignRolestoUsers(model);                
            }
            else { ViewBag.Message = "Alert"; }
            return View(db.UsersRoleAssign());
        }
        public ActionResult GetProfile()
        {
            return View(db.getProfile(int.Parse(Session["UserId"].ToString())));
        }

        [HttpPost]
        public ActionResult UpdateProfile(SignUp user)
        {           
            int result = db.UpdateProfile(user);
            if (result > 0)
            {
               
                ViewBag.Message = "Success";
                return View("GetProfile");
            }
            else
            {
                ViewBag.Message = "Failed";
                return View("GetProfile");
            }
           
        }

        //[HttpPost]
        //public ActionResult UpdatePassword(SignUp user)
        //{
        //    int result = db.UpdatePassword(user);
        //    if (result > 0)
        //    {

        //        ViewBag.Message = "Success";
        //        return View("UpdateProfile");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Failed";
        //        return View("UpdateProfile");
        //    }

        //}

        [HttpGet]
        public ActionResult UpdatePassword()
        {
           return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session["UserName"] = null;
            Session["UserFullName"] = null;
            Session["UserRoleType"] = null;
            TempData["LogoutFlag"] = true;
            return RedirectToAction("Login", "Login");
        }
        public ActionResult DbBackup()
        {
            return View();
        }
    }
}