using CCPNCR_Record_Management.App_Data;
using CCPNCR_Record_Management.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace CCPNCR_Record_Management.Controllers
{
    [HandleError]
 
    public class LoginController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] != null)
            {
                ViewBag.NotificationCount = db.GetNotification(int.Parse(Session["UserId"].ToString()));
            }
        }

        DataBaseAccess db = new DataBaseAccess();
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Login user)
        {
            if (ModelState.IsValid)
            {
                string resultLogin = db.Login(user);
                if (resultLogin == "Admin")
                {
                    return RedirectToAction("AdminDashBoard", "Admin");
                }
                else if (resultLogin == "User" || resultLogin == "Record Keeper")
                {
                    return RedirectToAction("User_RkDashBoard", "User_Rk");
                }
                else
                {
                    ViewBag.MessageLogin = resultLogin;
                    ModelState.Clear();
                    return View();
                }
            }
            return View(user);
        }


    }
}