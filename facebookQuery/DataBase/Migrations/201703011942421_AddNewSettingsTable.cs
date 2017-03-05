namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewSettingsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SettingsGroupId = c.Long(nullable: false),
                        AccountId = c.Long(nullable: false),
                        CommunityOptions = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupSettings", t => t.SettingsGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.SettingsGroupId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewSettings", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.NewSettings", "SettingsGroupId", "dbo.GroupSettings");
            DropIndex("dbo.NewSettings", new[] { "AccountId" });
            DropIndex("dbo.NewSettings", new[] { "SettingsGroupId" });
            DropTable("dbo.NewSettings");
        }
    }
}
