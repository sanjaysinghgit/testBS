namespace MLM.Web.Migrations
{
    using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MLM.Models;
using MLM.Models.Config;
using MLM.Web.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MLM.DB.MLMDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MLM.DB.MLMDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            


            //
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            ////// Create Roles
            if (!RoleManager.RoleExists("admin"))
            {
                var roleresult = RoleManager.Create(new IdentityRole("admin"));
            }
            if (!RoleManager.RoleExists("employee"))
            {
                var roleresult = RoleManager.Create(new IdentityRole("employee"));
            }
            if (!RoleManager.RoleExists("agent"))
            {
                var roleresult = RoleManager.Create(new IdentityRole("agent"));
            }
            ////END Roles creation

            if (UserManager.Users.FirstOrDefault(u => u.UserName == "admin") == null)
            {
                //// Create Default ADMIN user
                string password = "123456";
                var user = new ApplicationUser();
                user.UserName = "admin";
                user.DOB = DateTime.UtcNow;
                var adminresult = UserManager.Create(user, password);
                //Add User Admin to Role Admin
                if (adminresult.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "admin");
                }
            }
            //// END Admin user creation

            if (UserManager.Users.FirstOrDefault(u => u.UserName == "recentemp") == null)
            {
                string emppassword = "123456";
                var empuser = new ApplicationUser();
                empuser.UserName = "recentemp";
                empuser.DOB = DateTime.UtcNow;
                var empresult = UserManager.Create(empuser, emppassword);
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "employee");
                }
            }


            if (UserManager.Users.FirstOrDefault(u => u.UserName == "recentmart") == null)
            {
                //// Create Default Employee
                var agentinfo = new Agent
                {
                    Code = "1901010101",
                    SponsorCode = null,
                    IntroducerCode = null,
                    LeftAgent = null,
                    RightAgent = null,
                    Position = AgentPosition.Left,
                    TotalPaidPairs = 0,
                    SaveIncomeStatus = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    EPin = null,
                    Status = AgentStatus.Executive
                };
                string emppassword = "123456";
                var empuser = new ApplicationUser();
                empuser.UserName = "recentmart";
                empuser.Name = "Recent Company Agent Account";
                empuser.DOB = DateTime.UtcNow;
                empuser.AgentInfo = agentinfo;
                var empresult = UserManager.Create(empuser, emppassword);
                //Add User Admin to Role Admin
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "agent");
                }
            }




            if (UserManager.Users.FirstOrDefault(u => u.UserName == "recent") == null)
            {
                //// Create Default Employee
                var agentinfo = new Agent
                {
                    Code = "1900010101",
                    SponsorCode = null,
                    IntroducerCode = null,
                    LeftAgent = "2015030101",
                    RightAgent = "2015030102",
                    Position = AgentPosition.Left,
                    TotalPaidPairs = 0,
                    SaveIncomeStatus = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    EPin = null,
                    Status = AgentStatus.Executive
                };
                string emppassword = "123456";
                var empuser = new ApplicationUser();
                empuser.UserName = "recent";
                empuser.Name = "Recent Company Testing Account";
                empuser.DOB = DateTime.UtcNow;
                empuser.AgentInfo = agentinfo;
                var empresult = UserManager.Create(empuser, emppassword);
                //Add User Admin to Role Admin
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "agent");
                }
            }



            if (UserManager.Users.FirstOrDefault(u => u.UserName == "testagentone") == null)
            {
                //// Create Default Employee
                var agentinfo = new Agent
                {
                    Code = "2015030101",
                    SponsorCode = "1900010101",
                    IntroducerCode = "1900010101",
                    LeftAgent = "2015030105",
                    RightAgent = null,
                    Position = AgentPosition.Left,
                    TotalPaidPairs = 0,
                    SaveIncomeStatus = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    EPin = null,
                    Status = AgentStatus.Executive
                };
                string emppassword = "testagentone";
                var empuser = new ApplicationUser();
                empuser.UserName = "testagentone";
                empuser.DOB = DateTime.UtcNow;
                empuser.AgentInfo = agentinfo;
                var empresult = UserManager.Create(empuser, emppassword);
                //Add User Admin to Role Admin
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "agent");
                }
            }
            if (UserManager.Users.FirstOrDefault(u => u.UserName == "testagenttwo") == null)
            {
                //// Create Default Employee
                var agentinfo = new Agent
                {
                    Code = "2015030102",
                    SponsorCode = "1900010101",
                    IntroducerCode = "1900010101",
                    LeftAgent = "2015030103",
                    RightAgent = "2015030104",
                    Position = AgentPosition.Right,
                    TotalPaidPairs = 0,
                    SaveIncomeStatus = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    EPin = null,
                    Status = AgentStatus.Executive
                };
                string emppassword = "testagenttwo";
                var empuser = new ApplicationUser();
                empuser.UserName = "testagenttwo";
                empuser.DOB = DateTime.UtcNow;
                empuser.AgentInfo = agentinfo;
                var empresult = UserManager.Create(empuser, emppassword);
                //Add User Admin to Role Admin
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "agent");
                }
            }
            if (UserManager.Users.FirstOrDefault(u => u.UserName == "testagentthree") == null)
            {
                //// Create Default Employee
                var agentinfo = new Agent
                {
                    Code = "2015030103",
                    SponsorCode = "2015030102",
                    IntroducerCode = "1900010101",
                    LeftAgent = null,
                    RightAgent = null,
                    Position = AgentPosition.Left,
                    TotalPaidPairs = 0,
                    SaveIncomeStatus = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    EPin = null,
                    Status = AgentStatus.Executive
                };
                string emppassword = "testagentthree";
                var empuser = new ApplicationUser();
                empuser.UserName = "testagentthree";
                empuser.DOB = DateTime.UtcNow;
                empuser.AgentInfo = agentinfo;
                var empresult = UserManager.Create(empuser, emppassword);
                //Add User Admin to Role Admin
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "agent");
                }
            }
            if (UserManager.Users.FirstOrDefault(u => u.UserName == "testagentfour") == null)
            {
                //// Create Default Employee
                var agentinfo = new Agent
                {
                    Code = "2015030104",
                    SponsorCode = "2015030102",
                    IntroducerCode = "2015030102",
                    LeftAgent = null,
                    RightAgent = null,
                    Position = AgentPosition.Right,
                    TotalPaidPairs = 0,
                    SaveIncomeStatus = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    EPin = null,
                    Status = AgentStatus.Executive
                };
                string emppassword = "testagentfour";
                var empuser = new ApplicationUser();
                empuser.UserName = "testagentfour";
                empuser.DOB = DateTime.UtcNow;
                empuser.AgentInfo = agentinfo;
                var empresult = UserManager.Create(empuser, emppassword);
                //Add User Admin to Role Admin
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "agent");
                }
            }
            if (UserManager.Users.FirstOrDefault(u => u.UserName == "testagentfive") == null)
            {
                //// Create Default Employee
                var agentinfo = new Agent
                {
                    Code = "2015030105",
                    SponsorCode = "2015030101",
                    IntroducerCode = "2015030101",
                    LeftAgent = null,
                    RightAgent = null,
                    Position = AgentPosition.Left,
                    TotalPaidPairs = 0,
                    SaveIncomeStatus = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    EPin = null,
                    Status = AgentStatus.Executive
                };
                string emppassword = "testagentfive";
                var empuser = new ApplicationUser();
                empuser.UserName = "testagentfive";
                empuser.DOB = DateTime.UtcNow;
                empuser.AgentInfo = agentinfo;
                var empresult = UserManager.Create(empuser, emppassword);
                //Add User Admin to Role Admin
                if (empresult.Succeeded)
                {
                    var result = UserManager.AddToRole(empuser.Id, "agent");
                }
            }

            context.Settings.AddOrUpdate(
              p => p.Company,
              new Setting
              {
                  Company = "MLM",
                  SaveIncomeDaysDuration = 15,
                  PairIncomeAmout = 0,
                  PVAmount =   1,
                  RepurchasingVoucher = VoucherMode.ByMonthly,
                  BinaryVoucher = VoucherMode.Weekly,
                  CreatedDate = DateTime.UtcNow,
                  UpdateDate = DateTime.UtcNow,
              });

            //context.Agents.AddOrUpdate(
            //  p => p.Code,
            //  new Agent
            //  {
            //      Code = "1900010101",
            //      SponsorCode = null,
            //      IntroducerCode = null,
            //      LeftAgent = null,
            //      RightAgent = null,
            //      Position = AgentPosition.Left,
            //      CarryLeft = 0,
            //      CarryRight = 0,
            //      TotalPaidPairs = 0,
            //      SaveIncomeStatus = false,
            //      IsDeleted = false,
            //      CreatedDate = DateTime.UtcNow,
            //      UpdateDate = DateTime.UtcNow,
            //      EPin = null,
            //      Status = AgentStatus.Pedning
            //  },
            //new Agent
            //{
            //    Code = "2015030101",
            //    SponsorCode = "1900010101",
            //    IntroducerCode = null,
            //    LeftAgent = null,
            //    RightAgent = null,
            //    Position = AgentPosition.Left,
            //    CarryLeft = 0,
            //    CarryRight = 0,
            //    TotalPaidPairs = 0,
            //    SaveIncomeStatus = false,
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdateDate = DateTime.UtcNow,
            //    EPin = null,
            //    Status = AgentStatus.Pedning
            //},
            //new Agent
            //{
            //    Code = "2015030102",
            //    SponsorCode = "1900010101",
            //    IntroducerCode = null,
            //    LeftAgent = null,
            //    RightAgent = null,
            //    Position = AgentPosition.Right,
            //    CarryLeft = 0,
            //    CarryRight = 0,
            //    TotalPaidPairs = 0,
            //    SaveIncomeStatus = false,
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdateDate = DateTime.UtcNow,
            //    EPin = null,
            //    Status = AgentStatus.Pedning
            //},
            //new Agent
            //{
            //    Code = "2015030103",
            //    SponsorCode = "2015030101",
            //    IntroducerCode = null,
            //    LeftAgent = null,
            //    RightAgent = null,
            //    Position = AgentPosition.Right,
            //    CarryLeft = 0,
            //    CarryRight = 0,
            //    TotalPaidPairs = 0,
            //    SaveIncomeStatus = false,
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdateDate = DateTime.UtcNow,
            //    EPin = null,
            //    Status = AgentStatus.Pedning
            //},
            //new Agent
            //{
            //    Code = "2015030104",
            //    SponsorCode = "2015030101",
            //    IntroducerCode = null,
            //    LeftAgent = null,
            //    RightAgent = null,
            //    Position = AgentPosition.Right,
            //    CarryLeft = 0,
            //    CarryRight = 0,
            //    TotalPaidPairs = 0,
            //    SaveIncomeStatus = false,
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdateDate = DateTime.UtcNow,
            //    EPin = null,
            //    Status = AgentStatus.Pedning
            //}
            //);

        }
    }
}
