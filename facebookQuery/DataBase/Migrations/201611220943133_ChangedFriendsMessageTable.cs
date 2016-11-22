namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFriendsMessageTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendMessages", "MessageDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.FriendMessages", "SendingDateTime");
            DropColumn("dbo.FriendMessages", "LastUnreadMessageDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FriendMessages", "LastUnreadMessageDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.FriendMessages", "LastReadMessageDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.FriendMessages", "MessageDateTime");
        }
    }
}
