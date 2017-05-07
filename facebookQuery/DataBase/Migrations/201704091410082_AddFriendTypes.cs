namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "FriendType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "FriendType");
        }
    }
}
