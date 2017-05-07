namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForSpyFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpyAccounts", "ProxyDataIsFailed", c => c.Boolean(nullable: false));
            AddColumn("dbo.SpyAccounts", "AuthorizationDataIsFailed", c => c.Boolean(nullable: false));
            AddColumn("dbo.SpyAccounts", "ConformationIsFailed", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobsQueue", "IsForSpy", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobStatus", "IsForSpy", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobStatus", "IsForSpy");
            DropColumn("dbo.JobsQueue", "IsForSpy");
            DropColumn("dbo.SpyAccounts", "ConformationIsFailed");
            DropColumn("dbo.SpyAccounts", "AuthorizationDataIsFailed");
            DropColumn("dbo.SpyAccounts", "ProxyDataIsFailed");
        }
    }
}
