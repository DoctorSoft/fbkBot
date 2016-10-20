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
                        CookieId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cookies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(),
                        Locale = c.String(),
                        Av = c.String(),
                        Datr = c.String(),
                        Sb = c.String(),
                        CUser = c.String(),
                        Xs = c.String(),
                        Fr = c.String(),
                        Csm = c.String(),
                        S = c.String(),
                        Pl = c.String(),
                        Lu = c.String(),
                        P = c.String(),
                        Act = c.String(),
                        Wd = c.String(),
                        Presence = c.String(),
                        Account_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cookies", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.Cookies", new[] { "Account_Id" });
            DropTable("dbo.Cookies");
            DropTable("dbo.Accounts");
        }
    }
}
