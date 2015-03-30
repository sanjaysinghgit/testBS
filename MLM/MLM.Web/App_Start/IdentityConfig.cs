using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MLM.DB;
using MLM.Models;
using MLM.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MLM.Web
{
    // This is useful if you do not want to tear down the database each time you run the application.
    // You want to create a new database if the Model changes
    // public class MyDbInitializer : DropCreateDatabaseAlways<MyDbContext>
    public class MyDbInitializer : DropCreateDatabaseIfModelChanges<MLMDbContext>
    {
        protected override void Seed(MLMDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private void InitializeIdentityForEF(MLMDbContext context)
        {
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var agentinfo = new Agent() { Code= "0000000999", };
            //string name = "Admin";
            //string password = "123456";
            ////string test = "test";

            //////Create Role Test and User Test
            ////RoleManager.Create(new IdentityRole(test));
            ////UserManager.Create(new ApplicationUser() { UserName = test });

            ////Create Role Admin if it does not exist
            //if (!RoleManager.RoleExists(name))
            //{
            //    var roleresult = RoleManager.Create(new IdentityRole(name));
            //}

            ////Create User=Admin with password=123456
            //var user = new ApplicationUser();
            //user.UserName = name;
            ////user.HomeTown = "Seattle";
            //user.AgentInfo = agentinfo;
            //var adminresult = UserManager.Create(user, password);

            ////Add User Admin to Role Admin
            //if (adminresult.Succeeded)
            //{
            //    var result = UserManager.AddToRole(user.Id, name);
            //}
        }
    }
}