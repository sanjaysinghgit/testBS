namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationissue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PairPriceMaster",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        pair = c.Int(nullable: false),
                        TimeinDays = c.Int(nullable: false),
                        Price = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        PairIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TDS = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProcessingCharges = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DispatchedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Setting", "TDSWithPAN", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Setting", "TDSWithOutPAN", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Setting", "VoucherProcessingCharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Setting", "VoucherProcessingChargeCapping", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Setting", "SaveIncomeAmount", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "BinaryIncomeSetting", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "WeeklyBinaryCapping", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "MonthlyBinaryCapping", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "WeeklyRepurchaseCapping", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "MonthlyRepurchaseCapping", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "VoucherStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Setting", "RepurchasingVoucher");
            DropColumn("dbo.Setting", "BinaryVoucher");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Setting", "BinaryVoucher", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "RepurchasingVoucher", c => c.Int(nullable: false));
            DropColumn("dbo.Setting", "VoucherStatus");
            DropColumn("dbo.Setting", "MonthlyRepurchaseCapping");
            DropColumn("dbo.Setting", "WeeklyRepurchaseCapping");
            DropColumn("dbo.Setting", "MonthlyBinaryCapping");
            DropColumn("dbo.Setting", "WeeklyBinaryCapping");
            DropColumn("dbo.Setting", "BinaryIncomeSetting");
            DropColumn("dbo.Setting", "SaveIncomeAmount");
            DropColumn("dbo.Setting", "VoucherProcessingChargeCapping");
            DropColumn("dbo.Setting", "VoucherProcessingCharge");
            DropColumn("dbo.Setting", "TDSWithOutPAN");
            DropColumn("dbo.Setting", "TDSWithPAN");
            DropTable("dbo.Payout");
            DropTable("dbo.PairPriceMaster");
        }
    }
}
