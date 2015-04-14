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
    public class AuthController : Controller
    {
        //
        // GET: /Auth/
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

                        var userIdentity = (ClaimsIdentity)User.Identity;
                        var claims = userIdentity.Claims;
                        var roleClaimType = userIdentity.RoleClaimType;
                        var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                        if (roles.Find(r => r.Value == "admin") != null)
                        {
                            ViewBag.Role = "admin";
                            ViewBag.Name = currentUser.Name;
                            return View("IndexAdmin");
                        }
                        else if (roles.Find(r => r.Value == "agent") != null)
                        {
                            ViewBag.Role = "agent";
                            ViewBag.Code = currentUser.AgentInfo.Code;
                            ViewBag.Name = currentUser.Name;
                            return View("IndexAgent");
                        }
                        else
                        {
                            return View();
                        }
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

    }
}
