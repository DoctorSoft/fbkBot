namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobsStatusTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobsStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        JobName = c.String(),
                        Status = c.Int(nullable: false),
                        StartDateTime = c.DateTime(),
                        EndDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobsStatus");
        }
    }
}
