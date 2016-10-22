namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrlParametersTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Messages", new[] { "AccountId" });
            CreateTable(
                "dbo.UrlParameters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CodeParameters = c.Int(nullable: false),
                        ParametersSet = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Messages");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.UrlParameters");
            CreateIndex("dbo.Messages", "AccountId");
            AddForeignKey("dbo.Messages", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
