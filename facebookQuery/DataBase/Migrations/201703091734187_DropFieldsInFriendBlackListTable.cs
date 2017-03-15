namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropFieldsInFriendBlackListTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FriendsBlackList", "DialogIsCompleted");
            DropColumn("dbo.FriendsBlackList", "IsAddedToGroups");
            DropColumn("dbo.FriendsBlackList", "IsAddedToPages");
            DropColumn("dbo.FriendsBlackList", "IsWinked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FriendsBlackList", "IsWinked", c => c.Boolean(nullable: false));
            AddColumn("dbo.FriendsBlackList", "IsAddedToPages", c => c.Boolean(nullable: false));
            AddColumn("dbo.FriendsBlackList", "IsAddedToGroups", c => c.Boolean(nullable: false));
            AddColumn("dbo.FriendsBlackList", "DialogIsCompleted", c => c.Boolean(nullable: false));
        }
    }
}
