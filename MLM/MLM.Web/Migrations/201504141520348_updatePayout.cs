namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePayout : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payout",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VoucherDate = c.DateTime(nullable: false),
                        Agentcode = c.String(),
                        PAN = c.String(),
                        TotalLeftPair = c.Int(nullable: false),
                        TotalRightPair = c.Int(nullable: false),
                        TotalLeftPairPV = c.Int(nullable: false),
                        TotalRightPairPV = c.Int(nullable: false),
                        PairsInThisPayout = c.Int(nullable: false),
                        PairsPVInThisPayout = c.Int(nullable: false),
                        SaveIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TDS = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProcessingCharges = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DispatchedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Payout");
        }
    }
}
