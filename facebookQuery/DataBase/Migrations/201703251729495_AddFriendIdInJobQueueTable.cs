namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendIdInJobQueueTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobsQueue", "FriendId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobsQueue", "FriendId");
        }
    }
}
