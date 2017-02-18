namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedGroupOptionsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "RetryTimeSendUnread", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeSendUnanswered", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeSendNewFriend", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeConfirmFriendships", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeGetNewAndRecommendedFriends", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeRefreshFriends", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeSendRequestFriendships", c => c.Long(nullable: false));
            DropColumn("dbo.Settings", "DelayTimeSendUnread");
            DropColumn("dbo.Settings", "DelayTimeSendUnanswered");
            DropColumn("dbo.Settings", "DelayTimeSendNewFriend");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "DelayTimeSendNewFriend", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "DelayTimeSendUnanswered", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "DelayTimeSendUnread", c => c.Long(nullable: false));
            DropColumn("dbo.Settings", "RetryTimeSendRequestFriendships");
            DropColumn("dbo.Settings", "RetryTimeRefreshFriends");
            DropColumn("dbo.Settings", "RetryTimeGetNewAndRecommendedFriends");
            DropColumn("dbo.Settings", "RetryTimeConfirmFriendships");
            DropColumn("dbo.Settings", "RetryTimeSendNewFriend");
            DropColumn("dbo.Settings", "RetryTimeSendUnanswered");
            DropColumn("dbo.Settings", "RetryTimeSendUnread");
        }
    }
}
