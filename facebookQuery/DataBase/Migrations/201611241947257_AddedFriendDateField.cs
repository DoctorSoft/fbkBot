namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFriendDateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "AddedDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "AddedDateTime");
        }
    }
}
