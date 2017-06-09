namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobIdFiels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobsState", "JobId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobsState", "JobId");
        }
    }
}
