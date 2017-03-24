namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobStatus", "JobInformation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobStatus", "JobInformation");
        }
    }
}
