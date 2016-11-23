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
                        Name = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        PageUrl = c.String(),
                        FacebookId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cookies",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        CookiesString = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FacebookId = c.Long(nullable: false),
                        FriendName = c.String(),
                        DeleteFromFriends = c.Boolean(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                        AccountId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.FriendMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        MessageDateTime = c.DateTime(nullable: false),
                        OrderNumber = c.Int(nullable: false),
                        MessageDirection = c.Int(nullable: false),
                        FriendId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Friends", t => t.FriendId, cascadeDelete: true)
                .Index(t => t.FriendId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        ImportancyFactor = c.Long(nullable: false),
                        IsStopped = c.Boolean(nullable: false),
                        AccountId = c.Long(),
                        MessageGroupId = c.Long(),
                        MessageRegime = c.Int(nullable: false),
                        StartTime = c.Time(precision: 7),
                        EndTime = c.Time(precision: 7),
                        OrderNumber = c.Int(nullable: false),
                        IsEmergencyText = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MessageGroupDbModels", t => t.MessageGroupId)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .Index(t => t.AccountId)
                .Index(t => t.MessageGroupId);
            
            CreateTable(
                "dbo.MessageGroupDbModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StopWords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Word = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.Messages", "MessageGroupId", "dbo.MessageGroupDbModels");
            DropForeignKey("dbo.Friends", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.FriendMessages", "FriendId", "dbo.Friends");
            DropForeignKey("dbo.Cookies", "Id", "dbo.Accounts");
            DropIndex("dbo.Messages", new[] { "MessageGroupId" });
            DropIndex("dbo.Messages", new[] { "AccountId" });
            DropIndex("dbo.FriendMessages", new[] { "FriendId" });
            DropIndex("dbo.Friends", new[] { "AccountId" });
            DropIndex("dbo.Cookies", new[] { "Id" });
            DropTable("dbo.UrlParameters");
            DropTable("dbo.StopWords");
            DropTable("dbo.Links");
            DropTable("dbo.MessageGroupDbModels");
            DropTable("dbo.Messages");
            DropTable("dbo.FriendMessages");
            DropTable("dbo.Friends");
            DropTable("dbo.Cookies");
            DropTable("dbo.Accounts");
        }
    }
}
