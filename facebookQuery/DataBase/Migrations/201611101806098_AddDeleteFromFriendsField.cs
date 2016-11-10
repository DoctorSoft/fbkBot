namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeleteFromFriendsField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "DeleteFromFriends", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "DeleteFromFriends");
        }
    }
}
