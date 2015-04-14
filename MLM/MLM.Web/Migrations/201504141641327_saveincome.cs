namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saveincome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Setting", "SaveIncomeAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Setting", "SaveIncomeAmount");
        }
    }
}
