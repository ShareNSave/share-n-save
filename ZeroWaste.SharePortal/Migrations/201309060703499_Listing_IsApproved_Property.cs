namespace ZeroWaste.SharePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Listing_IsApproved_Property : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listing", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listing", "IsApproved");
        }
    }
}
