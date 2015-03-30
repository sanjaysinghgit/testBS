namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAgentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agent", "VoucherStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agent", "VoucherStatus");
        }
    }
}
