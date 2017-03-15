namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeleteFriendsOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "DeleteFriendsOptions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "DeleteFriendsOptions");
        }
    }
}
