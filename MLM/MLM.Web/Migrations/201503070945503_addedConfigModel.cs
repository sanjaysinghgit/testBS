namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedConfigModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaveIncomeDaysDuration = c.Int(nullable: false),
                        PairIncomeAmout = c.Int(nullable: false),
                        PVAmout = c.Int(nullable: false),
                        RepurchasingVoucher = c.Int(nullable: false),
                        BinaryVoucher = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Setting");
        }
    }
}
