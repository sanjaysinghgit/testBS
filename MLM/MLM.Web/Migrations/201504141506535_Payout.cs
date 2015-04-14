namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Setting", "BinaryIncomeSetting", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Setting", "BinaryIncomeSetting");
        }
    }
}
