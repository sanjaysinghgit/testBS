namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePairPriceMaster : DbMigration
    {
        public override void Up()
        {
            
            
            AddColumn("dbo.PairPriceMaster", "pairFrom", c => c.Int(nullable: false));
            AddColumn("dbo.PairPriceMaster", "pairTo", c => c.Int(nullable: false));
            AddColumn("dbo.PairPriceMaster", "TimeinDaysFrom", c => c.Int(nullable: false));
            AddColumn("dbo.PairPriceMaster", "TimeinDaysTo", c => c.Int(nullable: false));
            DropColumn("dbo.PairPriceMaster", "pair");
            DropColumn("dbo.PairPriceMaster", "TimeinDays");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PairPriceMaster", "TimeinDays", c => c.Int(nullable: false));
            AddColumn("dbo.PairPriceMaster", "pair", c => c.Int(nullable: false));
            DropForeignKey("dbo.PairPrizeWinner", "PairPriceId_Id", "dbo.PairPriceMaster");
            DropIndex("dbo.PairPrizeWinner", new[] { "PairPriceId_Id" });
            DropColumn("dbo.PairPriceMaster", "TimeinDaysTo");
            DropColumn("dbo.PairPriceMaster", "TimeinDaysFrom");
            DropColumn("dbo.PairPriceMaster", "pairTo");
            DropColumn("dbo.PairPriceMaster", "pairFrom");
            DropTable("dbo.PairPrizeWinner");
        }
    }
}
