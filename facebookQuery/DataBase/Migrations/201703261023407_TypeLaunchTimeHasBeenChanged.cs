namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypeLaunchTimeHasBeenChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobStatus", "LaunchDateTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobStatus", "LaunchDateTime", c => c.Time(nullable: false, precision: 7));
        }
    }
}
