using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MLM.Models;
using MLM.DB;
using System.Data.SqlClient;
using MLM.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;

namespace MLM.Controllers
{
    [Authorize]
    public class ApplicationUserController : BaseApiController
    {
        //public ApplicationUserController()
        //    : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MLMDbContext("MLMCon"))))
        //{
        //}

        //public ApplicationUserController(UserManager<ApplicationUser> userManager)
        //{
        //    UserManager = userManager;
        //}

        public UserManager<ApplicationUser> UserManager { get; private set; }


        [HttpGet]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public ApplicationUser GetCurrentUser()
        {
            try
            {
                // Get the current logged in User and look up the user in ASP.NET Identity
                //return UserManager.FindById(User.Identity.GetUserId());
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MLMDbContext("MLMCon")));
                var user = manager.FindById(User.Identity.GetUserId());
                


                var userIdentity = (ClaimsIdentity)User.Identity;
                var claims = userIdentity.Claims;
                var roleClaimType = userIdentity.RoleClaimType;
                var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                //user.Roles.Add(roles);

                return user;
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside ApplicationUserController | GetCurrentUser", ex);
                return null;
            }
        }

        [HttpGet]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IList<string> GetCurrentUserRoles()
        {
            try
            {
                // Get the current logged in User and look up the user in ASP.NET Identity
                //return UserManager.FindById(User.Identity.GetUserId());
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MLMDbContext("MLMCon")));
                var roles = manager.GetRoles(User.Identity.GetUserId());

                return roles;
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside ApplicationUserController | GetCurrentUserRole", ex);
                return null;
            }
        }

        //[HttpGet]
        //[System.Web.Mvc.ValidateAntiForgeryToken]
        //public async Task<ApplicationUser> GetCurrentUser()
        //{
        //    try
        //    {
        //        // Get the current logged in User and look up the user in ASP.NET Identity
        //        //return UserManager.FindById(User.Identity.GetUserId());
        //        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MLMDbContext("MLMCon")));
        //        return await manager.FindByIdAsync(User.Identity.GetUserId());
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Logger.LogDebug("Exception Inside ApplicationUserController | GetCurrentUser", ex);
        //        return null;
        //    }
        //}

    }
}