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

                //Redirect(string.Format("https://{0}{1}", HttpContext.Request.Url.Host,
                //                               HttpContext.Request.Url.PathAndQuery));
                if (User.Identity.IsAuthenticated)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MLMDbContext("MLMCon")));

                    // Get the current logged in User and look up the user in ASP.NET Identity
                    var currentUser = manager.FindById(User.Identity.GetUserId());
                    if (currentUser != null)
                    {
                        ViewBag.Code = currentUser.AgentInfo.Code;
                        ViewBag.Name = currentUser.Name;
                        return View("IndexAuth");
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                        return View();
                        //ModelState.AddModelError("", "Invalid username or password.");
                    }
                    // Recover the profile information about the logged in user



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
