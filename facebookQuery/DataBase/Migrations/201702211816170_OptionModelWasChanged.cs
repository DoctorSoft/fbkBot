namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionModelWasChanged : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Settings", new[] { "Id" });
            DropPrimaryKey("dbo.Settings");
            AddColumn("dbo.Settings", "GeoOptions", c => c.String());
            AddColumn("dbo.Settings", "MessageOptions", c => c.String());
            AddColumn("dbo.Settings", "FriendsOptions", c => c.String());
            AlterColumn("dbo.Settings", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Settings", "Id");
            CreateIndex("dbo.Settings", "Id");
            DropColumn("dbo.Settings", "Cities");
            DropColumn("dbo.Settings", "Countries");
            DropColumn("dbo.Settings", "Gender");
            DropColumn("dbo.Settings", "RetryTimeSendUnread");
            DropColumn("dbo.Settings", "RetryTimeSendUnanswered");
            DropColumn("dbo.Settings", "RetryTimeSendNewFriend");
            DropColumn("dbo.Settings", "RetryTimeConfirmFriendships");
            DropColumn("dbo.Settings", "RetryTimeGetNewAndRecommendedFriends");
            DropColumn("dbo.Settings", "RetryTimeRefreshFriends");
            DropColumn("dbo.Settings", "RetryTimeSendRequestFriendships");
            DropColumn("dbo.Settings", "UnansweredDelay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "UnansweredDelay", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeSendRequestFriendships", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeRefreshFriends", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeGetNewAndRecommendedFriends", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeConfirmFriendships", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeSendNewFriend", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeSendUnanswered", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "RetryTimeSendUnread", c => c.Long(nullable: false));
            AddColumn("dbo.Settings", "Gender", c => c.Int());
            AddColumn("dbo.Settings", "Countries", c => c.String());
            AddColumn("dbo.Settings", "Cities", c => c.String());
            DropIndex("dbo.Settings", new[] { "Id" });
            DropPrimaryKey("dbo.Settings");
            AlterColumn("dbo.Settings", "Id", c => c.Long(nullable: false));
            DropColumn("dbo.Settings", "FriendsOptions");
            DropColumn("dbo.Settings", "MessageOptions");
            DropColumn("dbo.Settings", "GeoOptions");
            AddPrimaryKey("dbo.Settings", "Id");
            CreateIndex("dbo.Settings", "Id");
        }
    }
}
