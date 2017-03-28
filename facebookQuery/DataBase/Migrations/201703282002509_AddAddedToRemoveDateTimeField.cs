namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddedToRemoveDateTimeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "IsWinkedFriendsFriend", c => c.Boolean(nullable: false));
            AddColumn("dbo.Friends", "AddedToRemoveDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "AddedToRemoveDateTime");
            DropColumn("dbo.Friends", "IsWinkedFriendsFriend");
        }
    }
}
