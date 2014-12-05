namespace ZeroWaste.SharePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Listing",
                c => new
                    {
                        ListingId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Group = c.String(),
                        GroupAddress = c.String(),
                        City = c.String(),
                        State = c.Int(nullable: false),
                        PostCode = c.String(),
                        Phone = c.String(),
                        AboutGroup = c.String(),
                        ListingMessage = c.String(),
                        MapAddress = c.String(),
                        WebLink = c.String(),
                        FacebookLink = c.String(),
                        ListingImageLink = c.String(),
                        Location = c.Geography(),
                        User_UserId = c.Int(),
                        ListingIcon_ListingIconId = c.Int(),
                    })
                .PrimaryKey(t => t.ListingId)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                //.ForeignKey("dbo.ListingIcon",f=>f.ListingIcon_ListingIconId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Listing", new[] { "User_UserId" });
            DropForeignKey("dbo.Listing", "User_UserId", "dbo.UserProfile");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Listing");
        }
    }
}
