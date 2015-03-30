namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedActivationDateInAgent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agent", "ActivationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agent", "ActivationDate");
        }
    }
}
