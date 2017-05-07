namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAgent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAgents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserAgentString = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Accounts", "UserAgentId", c => c.Long());
            CreateIndex("dbo.Accounts", "UserAgentId");
            AddForeignKey("dbo.Accounts", "UserAgentId", "dbo.UserAgents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "UserAgentId", "dbo.UserAgents");
            DropIndex("dbo.Accounts", new[] { "UserAgentId" });
            DropColumn("dbo.Accounts", "UserAgentId");
            DropTable("dbo.UserAgents");
        }
    }
}
