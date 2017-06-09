namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteJobStatusTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.JobStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JobStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        FunctionName = c.Int(nullable: false),
                        JobId = c.String(),
                        FriendId = c.Long(),
                        LaunchDateTime = c.String(),
                        AddDateTime = c.DateTime(nullable: false),
                        IsForSpy = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
