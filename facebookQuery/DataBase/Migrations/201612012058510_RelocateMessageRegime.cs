namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelocateMessageRegime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "MessageRegime", c => c.Int());
            DropColumn("dbo.FriendMessages", "MessageRegime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FriendMessages", "MessageRegime", c => c.Int(nullable: false));
            DropColumn("dbo.Friends", "MessageRegime");
        }
    }
}
