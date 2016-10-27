namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PageUrl = c.String(),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cookies",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        CookiesString = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        ImportancyFactor = c.Long(nullable: false),
                        IsStopped = c.Boolean(nullable: false),
                        AccountId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.UrlParameters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CodeParameters = c.Int(nullable: false),
                        ParametersSet = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Cookies", "Id", "dbo.Accounts");
            DropIndex("dbo.Messages", new[] { "AccountId" });
            DropIndex("dbo.Cookies", new[] { "Id" });
            DropTable("dbo.UrlParameters");
            DropTable("dbo.Messages");
            DropTable("dbo.Cookies");
            DropTable("dbo.Accounts");
        }
    }
}
