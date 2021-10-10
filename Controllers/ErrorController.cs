using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.Controllers
{
    [HandleError]
  
    public class ErrorController : Controller
    {
     
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PagenotFound()
        {

            return View();
        }
    }
}