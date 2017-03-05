namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobQueueTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobsQueue",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        FunctionName = c.Int(nullable: false),
                        AddedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobsQueue");
        }
    }
}
