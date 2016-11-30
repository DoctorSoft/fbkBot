namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMessageRegimeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendMessages", "MessageRegime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FriendMessages", "MessageRegime");
        }
    }
}
