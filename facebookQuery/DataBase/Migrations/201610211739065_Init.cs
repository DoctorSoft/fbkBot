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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cookies",
                c => new
                    {
                        Id = c.Long(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cookies", "Id", "dbo.Accounts");
            DropIndex("dbo.Cookies", new[] { "Id" });
            DropTable("dbo.Cookies");
            DropTable("dbo.Accounts");
        }
    }
}
