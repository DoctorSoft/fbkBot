namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountIdField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobStatus", "AccountId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobStatus", "AccountId");
        }
    }
}
