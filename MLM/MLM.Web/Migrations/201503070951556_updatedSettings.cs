namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Setting", "Company", c => c.String());
            AddColumn("dbo.Setting", "PVAmount", c => c.Int(nullable: false));
            DropColumn("dbo.Setting", "PVAmout");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Setting", "PVAmout", c => c.Int(nullable: false));
            DropColumn("dbo.Setting", "PVAmount");
            DropColumn("dbo.Setting", "Company");
        }
    }
}
