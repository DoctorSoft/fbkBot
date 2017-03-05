namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountOutgoingFriendshipsField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountStatistics", "CountOrdersConfirmedFriends", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountStatistics", "CountOrdersConfirmedFriends");
        }
    }
}
