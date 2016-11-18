namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeFriendMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendMessages", "LastReadMessageDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.FriendMessages", "LastUnreadMessageDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.FriendMessages", "MessageDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FriendMessages", "MessageDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.FriendMessages", "LastUnreadMessageDateTime");
            DropColumn("dbo.FriendMessages", "LastReadMessageDateTime");
        }
    }
}
