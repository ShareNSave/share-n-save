namespace ZeroWaste.SharePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_ResetPasswordLog_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResetPasswordLog",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Email = c.String(nullable: false),
                    CreateBy = c.String(nullable: true),
                    CreateAt = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.ResetPasswordLog");
        }
    }
}
