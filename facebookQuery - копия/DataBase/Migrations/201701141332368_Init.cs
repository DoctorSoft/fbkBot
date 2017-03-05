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
                        Proxy = c.String(),
                        ProxyLogin = c.String(),
                        ProxyPassword = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        GroupSettingsId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupSettings", t => t.GroupSettingsId)
                .Index(t => t.GroupSettingsId);
            
            CreateTable(
                "dbo.AnalysisFriends",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FacebookId = c.Long(nullable: false),
                        FriendName = c.String(),
                        AccountId = c.Long(nullable: false),
                        AddedDateTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
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
                        Gender = c.Int(nullable: false),
                        Href = c.String(),
                        AddedDateTime = c.DateTime(nullable: false),
                        MessageRegime = c.Int(),
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
                "dbo.GroupSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupFunctions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionId = c.Long(nullable: false),
                        GroupId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Functions", t => t.FunctionId, cascadeDelete: true)
                .ForeignKey("dbo.GroupSettings", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.FunctionId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionName = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        ForSpy = c.Boolean(nullable: false),
                        FunctionTypeId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FunctionTypes", t => t.FunctionTypeId, cascadeDelete: true)
                .Index(t => t.FunctionTypeId);
            
            CreateTable(
                "dbo.FunctionTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionTypeName = c.Int(nullable: false),
                        TypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpyFunctions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionId = c.Long(nullable: false),
                        SpyId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpyAccounts", t => t.SpyId, cascadeDelete: true)
                .ForeignKey("dbo.Functions", t => t.FunctionId, cascadeDelete: true)
                .Index(t => t.FunctionId)
                .Index(t => t.SpyId);
            
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
                .ForeignKey("dbo.GroupSettings", t => t.MessageGroupId)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .Index(t => t.AccountId)
                .Index(t => t.MessageGroupId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        LivesPlace = c.String(),
                        SchoolPlace = c.String(),
                        WorkPlace = c.String(),
                        Gender = c.Int(),
                        DelayTimeSendUnread = c.Long(nullable: false),
                        DelayTimeSendUnanswered = c.Long(nullable: false),
                        DelayTimeSendNewFriend = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupSettings", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AccountStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        CountReceivedFriends = c.Long(nullable: false),
                        CountRequestsSentToFriends = c.Long(nullable: false),
                        DateTimeUpdateStatistics = c.DateTime(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExtraMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
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
                "dbo.SpyStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SpyId = c.Long(nullable: false),
                        CountAnalizeFriends = c.Long(nullable: false),
                        DateTimeUpdateStatistics = c.DateTime(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
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
            DropForeignKey("dbo.Accounts", "GroupSettingsId", "dbo.GroupSettings");
            DropForeignKey("dbo.Settings", "Id", "dbo.GroupSettings");
            DropForeignKey("dbo.Messages", "MessageGroupId", "dbo.GroupSettings");
            DropForeignKey("dbo.GroupFunctions", "GroupId", "dbo.GroupSettings");
            DropForeignKey("dbo.SpyFunctions", "FunctionId", "dbo.Functions");
            DropForeignKey("dbo.SpyFunctions", "SpyId", "dbo.SpyAccounts");
            DropForeignKey("dbo.CookiesForSpy", "Id", "dbo.SpyAccounts");
            DropForeignKey("dbo.GroupFunctions", "FunctionId", "dbo.Functions");
            DropForeignKey("dbo.Functions", "FunctionTypeId", "dbo.FunctionTypes");
            DropForeignKey("dbo.Friends", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.FriendMessages", "FriendId", "dbo.Friends");
            DropForeignKey("dbo.Cookies", "Id", "dbo.Accounts");
            DropForeignKey("dbo.AnalysisFriends", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Settings", new[] { "Id" });
            DropIndex("dbo.Messages", new[] { "MessageGroupId" });
            DropIndex("dbo.Messages", new[] { "AccountId" });
            DropIndex("dbo.CookiesForSpy", new[] { "Id" });
            DropIndex("dbo.SpyFunctions", new[] { "SpyId" });
            DropIndex("dbo.SpyFunctions", new[] { "FunctionId" });
            DropIndex("dbo.Functions", new[] { "FunctionTypeId" });
            DropIndex("dbo.GroupFunctions", new[] { "GroupId" });
            DropIndex("dbo.GroupFunctions", new[] { "FunctionId" });
            DropIndex("dbo.FriendMessages", new[] { "FriendId" });
            DropIndex("dbo.Friends", new[] { "AccountId" });
            DropIndex("dbo.Cookies", new[] { "Id" });
            DropIndex("dbo.AnalysisFriends", new[] { "AccountId" });
            DropIndex("dbo.Accounts", new[] { "GroupSettingsId" });
            DropTable("dbo.UrlParameters");
            DropTable("dbo.StopWords");
            DropTable("dbo.SpyStatistics");
            DropTable("dbo.Links");
            DropTable("dbo.ExtraMessages");
            DropTable("dbo.AccountStatistics");
            DropTable("dbo.Settings");
            DropTable("dbo.Messages");
            DropTable("dbo.CookiesForSpy");
            DropTable("dbo.SpyAccounts");
            DropTable("dbo.SpyFunctions");
            DropTable("dbo.FunctionTypes");
            DropTable("dbo.Functions");
            DropTable("dbo.GroupFunctions");
            DropTable("dbo.GroupSettings");
            DropTable("dbo.FriendMessages");
            DropTable("dbo.Friends");
            DropTable("dbo.Cookies");
            DropTable("dbo.AnalysisFriends");
            DropTable("dbo.Accounts");
        }
    }
}
