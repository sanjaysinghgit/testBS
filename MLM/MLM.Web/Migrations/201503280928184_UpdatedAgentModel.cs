namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedAgentModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Agent", "CarryLeft");
            DropColumn("dbo.Agent", "CarryRight");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Agent", "CarryRight", c => c.Int(nullable: false));
            AddColumn("dbo.Agent", "CarryLeft", c => c.Int(nullable: false));
        }
    }
}
