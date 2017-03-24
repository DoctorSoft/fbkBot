namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendIdFromJobStatusTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobStatus", "FriendId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobStatus", "FriendId");
        }
    }
}
