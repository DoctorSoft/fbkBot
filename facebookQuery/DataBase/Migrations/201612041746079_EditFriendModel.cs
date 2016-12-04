namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditFriendModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Friends", "Href", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "Href");
            DropColumn("dbo.Friends", "Gender");
        }
    }
}
