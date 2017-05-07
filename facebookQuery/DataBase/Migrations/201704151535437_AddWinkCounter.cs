namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWinkCounter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "CountWinksToFriends", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "CountWinksToFriends");
        }
    }
}
