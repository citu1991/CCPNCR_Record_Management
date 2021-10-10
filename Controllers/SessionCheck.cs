using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CCPNCR_Record_Management.Controllers
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["UserName"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
           
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session["UserId"] = null;
            HttpContext.Current.Session["UserName"] = null;
            HttpContext.Current.Session["UserFullName"] = null;
            HttpContext.Current.Session["UserRoleType"] = null;
            FormsAuthentication.SignOut();
            filterContext.Result = new RedirectResult("/Login/Login");
        }
    }

}