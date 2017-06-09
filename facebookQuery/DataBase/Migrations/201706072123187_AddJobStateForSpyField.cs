namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobStateForSpyField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobsState", "IsForSpy", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobsState", "IsForSpy");
        }
    }
}
