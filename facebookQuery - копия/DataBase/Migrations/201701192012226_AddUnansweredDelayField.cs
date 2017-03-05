namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUnansweredDelayField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "UnansweredDelay", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "UnansweredDelay");
        }
    }
}
