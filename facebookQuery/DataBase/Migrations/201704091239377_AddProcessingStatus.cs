namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcessingStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobsQueue", "IsProcessed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobsQueue", "IsProcessed");
        }
    }
}
