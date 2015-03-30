namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allownullforactivationDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Agent", "ActivationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Agent", "ActivationDate", c => c.DateTime(nullable: false));
        }
    }
}
