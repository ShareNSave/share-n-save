namespace ZeroWaste.SharePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UserProfile_Add_Properties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Username", c => c.String());
            AddColumn("dbo.UserProfile", "OrganisationOrGroup", c => c.String());
            AddColumn("dbo.UserProfile", "GroupAddress", c => c.String());
            AddColumn("dbo.UserProfile", "City", c => c.String());
            AddColumn("dbo.UserProfile", "State", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "Postcode", c => c.String());
            AddColumn("dbo.UserProfile", "Phone", c => c.String());
            AddColumn("dbo.UserPorfile", "AboutGroup", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Phone");
            DropColumn("dbo.UserProfile", "Postcode");
            DropColumn("dbo.UserProfile", "State");
            DropColumn("dbo.UserProfile", "City");
            DropColumn("dbo.UserProfile", "GroupAddress");
            DropColumn("dbo.UserProfile", "OrganisationOrGroup");
            DropColumn("dbo.UserProfile", "Username");
            DropColumn("dbo.UserProfile", "AboutGroup");
        }
    }
}
