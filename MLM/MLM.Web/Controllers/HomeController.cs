using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MLM.DB;
using MLM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;

namespace MLM.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Index()
        {

            try
            {

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToRoute("AngularCatchAllRoute");
                }
                else
                {
                    return View();
                }
            }
            catch (System.Exception e)
            {

                return View("Error");
            }

        }

        public ActionResult about()
        {
            return View("about");
        }

        public ActionResult BusinessPlan()
        {
            return View("Bplan");
        }

        public ActionResult ChairmanMsges()
        {
            return View("ChairmanMsg");
        }

        public ActionResult Contectus()
        {
            return View("Contact");
        }

        public ActionResult Downloads()
        {
            return View("Download");
        }

    }
}
