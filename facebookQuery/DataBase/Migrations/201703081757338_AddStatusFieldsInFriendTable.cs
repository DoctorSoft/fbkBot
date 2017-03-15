namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusFieldsInFriendTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "DialogIsCompleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Friends", "IsAddedToGroups", c => c.Boolean(nullable: false));
            AddColumn("dbo.Friends", "IsAddedToPages", c => c.Boolean(nullable: false));
            AddColumn("dbo.Friends", "IsWinked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "IsWinked");
            DropColumn("dbo.Friends", "IsAddedToPages");
            DropColumn("dbo.Friends", "IsAddedToGroups");
            DropColumn("dbo.Friends", "DialogIsCompleted");
        }
    }
}
