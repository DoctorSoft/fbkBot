namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropJobStatusTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.JobsStatus");
        }
        
        public override void Down()
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
    }
}
