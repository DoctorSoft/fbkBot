namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeQueueAndStatusTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobsQueue", "JobId", c => c.String());
            AddColumn("dbo.JobsQueue", "LaunchDateTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobsQueue", "LaunchDateTime");
            DropColumn("dbo.JobsQueue", "JobId");
        }
    }
}
