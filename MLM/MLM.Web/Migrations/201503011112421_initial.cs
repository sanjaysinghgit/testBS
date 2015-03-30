namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agent",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(),
                        SponsorCode = c.String(),
                        IntroducerCode = c.String(),
                        LeftAgent = c.String(),
                        RightAgent = c.String(),
                        Position = c.Int(nullable: false),
                        CarryLeft = c.Int(nullable: false),
                        CarryRight = c.Int(nullable: false),
                        TotalPaidPairs = c.Int(nullable: false),
                        SaveIncomeStatus = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        EPin_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EPin", t => t.EPin_Id)
                .Index(t => t.EPin_Id);
            
            CreateTable(
                "dbo.EPin",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SerialNumber = c.Int(nullable: false),
                        Pin = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Coupon",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SerialNumber = c.Int(nullable: false),
                        RandomNumber = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Expiry = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Type_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CouponType", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.CouponType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        ImagePath = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductGrade",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        BusinessShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductKit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        MRP = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DPPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BP = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Category_Id = c.Long(),
                        Grade_Id = c.Long(),
                        ProductKit_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategory", t => t.Category_Id)
                .ForeignKey("dbo.ProductGrade", t => t.Grade_Id)
                .ForeignKey("dbo.ProductKit", t => t.ProductKit_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Grade_Id)
                .Index(t => t.ProductKit_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.TempAgTree",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AgentCode = c.String(),
                        SponsorCode = c.String(),
                        IntroducerCode = c.String(),
                        Position = c.String(),
                        Level = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Address = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        AgentInfo_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agent", t => t.AgentInfo_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.AgentInfo_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "AgentInfo_Id", "dbo.Agent");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Product", "ProductKit_Id", "dbo.ProductKit");
            DropForeignKey("dbo.Product", "Grade_Id", "dbo.ProductGrade");
            DropForeignKey("dbo.Product", "Category_Id", "dbo.ProductCategory");
            DropForeignKey("dbo.Coupon", "Type_Id", "dbo.CouponType");
            DropForeignKey("dbo.Agent", "EPin_Id", "dbo.EPin");
            DropIndex("dbo.AspNetUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Users", new[] { "AgentInfo_Id" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Product", new[] { "ProductKit_Id" });
            DropIndex("dbo.Product", new[] { "Grade_Id" });
            DropIndex("dbo.Product", new[] { "Category_Id" });
            DropIndex("dbo.Coupon", new[] { "Type_Id" });
            DropIndex("dbo.Agent", new[] { "EPin_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.TempAgTree");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Product");
            DropTable("dbo.ProductKit");
            DropTable("dbo.ProductGrade");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.CouponType");
            DropTable("dbo.Coupon");
            DropTable("dbo.EPin");
            DropTable("dbo.Agent");
        }
    }
}
