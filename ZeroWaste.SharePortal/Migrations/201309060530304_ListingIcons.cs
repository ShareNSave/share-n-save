namespace ZeroWaste.SharePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ListingIcons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListingIcon",
                c => new
                    {
                        ListingIconId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IconPath = c.String(),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ListingIconId)
                .ForeignKey("dbo.ListingCategory", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Category_Id);

            CreateTable(
                "dbo.ListingCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            //AddColumn("dbo.Listing", "ListingIcon_ListingIconId", c => c.Int());
            AddForeignKey("dbo.Listing", "ListingIcon_ListingIconId", "dbo.ListingIcon", "ListingIconId");
            CreateIndex("dbo.Listing", "ListingIcon_ListingIconId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ListingIcon", new[] { "Category_Id" });
            DropIndex("dbo.Listing", new[] { "ListingIcon_ListingIconId" });
            DropForeignKey("dbo.ListingIcon", "Category_Id", "dbo.ListingCategory");
            DropForeignKey("dbo.Listing", "ListingIcon_ListingIconId", "dbo.ListingIcon");
            DropColumn("dbo.Listing", "ListingIcon_ListingIconId");
            DropTable("dbo.ListingCategory");
            DropTable("dbo.ListingIcon");
        }
    }
}
