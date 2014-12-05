namespace ZeroWaste.SharePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIconDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListingIcon", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListingIcon", "Description");
        }
    }
}
