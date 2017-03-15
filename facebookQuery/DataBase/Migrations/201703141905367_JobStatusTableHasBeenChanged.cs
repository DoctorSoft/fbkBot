namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobStatusTableHasBeenChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobStatus", "JobId", c => c.String());
            AddColumn("dbo.JobStatus", "LaunchDateTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.JobStatus", "AddDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.JobStatus", "LastLaunchDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobStatus", "LastLaunchDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.JobStatus", "AddDateTime");
            DropColumn("dbo.JobStatus", "LaunchDateTime");
            DropColumn("dbo.JobStatus", "JobId");
        }
    }
}
