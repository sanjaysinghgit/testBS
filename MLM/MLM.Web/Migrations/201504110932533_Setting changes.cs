namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settingchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Setting", "TDSWithPAN", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Setting", "TDSWithOutPAN", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Setting", "VoucherProcessingCharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Setting", "VoucherProcessingChargeCapping", c => c.Decimal(nullable: false, precision: 18, scale: 2));
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
            DropColumn("dbo.Setting", "VoucherProcessingChargeCapping");
            DropColumn("dbo.Setting", "VoucherProcessingCharge");
            DropColumn("dbo.Setting", "TDSWithOutPAN");
            DropColumn("dbo.Setting", "TDSWithPAN");
        }
    }
}
