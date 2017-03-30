namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWinkOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "WinkFriendsOptions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "WinkFriendsOptions");
        }
    }
}
