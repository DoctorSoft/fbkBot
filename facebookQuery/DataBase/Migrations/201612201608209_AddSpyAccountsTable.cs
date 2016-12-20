namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpyAccountsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CookiesForSpy",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        CookiesString = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpyAccounts", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SpyAccounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        PageUrl = c.String(),
                        FacebookId = c.Long(nullable: false),
                        Proxy = c.String(),
                        ProxyLogin = c.String(),
                        ProxyPassword = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CookiesForSpy", "Id", "dbo.SpyAccounts");
            DropIndex("dbo.CookiesForSpy", new[] { "Id" });
            DropTable("dbo.SpyAccounts");
            DropTable("dbo.CookiesForSpy");
        }
    }
}
