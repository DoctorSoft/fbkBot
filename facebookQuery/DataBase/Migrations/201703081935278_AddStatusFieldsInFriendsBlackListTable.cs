namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusFieldsInFriendsBlackListTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendsBlackList", "DialogIsCompleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.FriendsBlackList", "IsAddedToGroups", c => c.Boolean(nullable: false));
            AddColumn("dbo.FriendsBlackList", "IsAddedToPages", c => c.Boolean(nullable: false));
            AddColumn("dbo.FriendsBlackList", "IsWinked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FriendsBlackList", "IsWinked");
            DropColumn("dbo.FriendsBlackList", "IsAddedToPages");
            DropColumn("dbo.FriendsBlackList", "IsAddedToGroups");
            DropColumn("dbo.FriendsBlackList", "DialogIsCompleted");
        }
    }
}
