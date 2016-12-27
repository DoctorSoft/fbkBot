namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountSettingsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountSettings",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        LivesPlace = c.String(),
                        SchoolPlace = c.String(),
                        WorkPlace = c.String(),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountSettings", "Id", "dbo.Accounts");
            DropIndex("dbo.AccountSettings", new[] { "Id" });
            DropTable("dbo.AccountSettings");
        }
    }
}
