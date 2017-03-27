namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldToDelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScheduleRemovalOfFriends", "ToDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScheduleRemovalOfFriends", "ToDelete");
        }
    }
}
