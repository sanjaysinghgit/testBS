namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PairPrizeWinner",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        voucherid = c.Long(nullable: false),
                        voucherdate = c.DateTime(nullable: false),
                        Agentcode = c.String(),
                        TotPair = c.Int(nullable: false),
                        days = c.Int(nullable: false),
                        Prize = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
          //  AddColumn("dbo.PairPriceMaster", "pairFrom", c => c.Int(nullable: false));
            //AddColumn("dbo.PairPriceMaster", "pairTo", c => c.Int(nullable: false));
            //AddColumn("dbo.PairPriceMaster", "TimeinDaysFrom", c => c.Int(nullable: false));
            //AddColumn("dbo.PairPriceMaster", "TimeinDaysTo", c => c.Int(nullable: false));
            //DropColumn("dbo.PairPriceMaster", "pair");
            //DropColumn("dbo.PairPriceMaster", "TimeinDays");
        }
        
        public override void Down()
        {
           // AddColumn("dbo.PairPriceMaster", "TimeinDays", c => c.Int(nullable: false));
           // AddColumn("dbo.PairPriceMaster", "pair", c => c.Int(nullable: false));
           // DropColumn("dbo.PairPriceMaster", "TimeinDaysTo");
           // DropColumn("dbo.PairPriceMaster", "TimeinDaysFrom");
           // DropColumn("dbo.PairPriceMaster", "pairTo");
           //// DropColumn("dbo.PairPriceMaster", "pairFrom");
           // DropTable("dbo.PairPrizeWinner");
        }
    }
}
