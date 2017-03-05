namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobStatusTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionName = c.Int(nullable: false),
                        LastLaunchDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobStatus");
        }
    }
}
