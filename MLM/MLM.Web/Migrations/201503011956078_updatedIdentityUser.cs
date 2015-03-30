namespace MLM.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedIdentityUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Name", c => c.String());
            AddColumn("dbo.Users", "DOB", c => c.DateTime());
            AddColumn("dbo.Users", "FatherName", c => c.String());
            AddColumn("dbo.Users", "HusbandName", c => c.String());
            AddColumn("dbo.Users", "Gender", c => c.Int());
            AddColumn("dbo.Users", "City", c => c.String());
            AddColumn("dbo.Users", "State", c => c.String());
            AddColumn("dbo.Users", "Pin", c => c.String());
            AddColumn("dbo.Users", "Nominee", c => c.String());
            AddColumn("dbo.Users", "Relation", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Relation");
            DropColumn("dbo.Users", "Nominee");
            DropColumn("dbo.Users", "Pin");
            DropColumn("dbo.Users", "State");
            DropColumn("dbo.Users", "City");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "HusbandName");
            DropColumn("dbo.Users", "FatherName");
            DropColumn("dbo.Users", "DOB");
            DropColumn("dbo.Users", "Name");
        }
    }
}
