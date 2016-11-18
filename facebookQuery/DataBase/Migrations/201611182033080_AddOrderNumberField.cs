namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderNumberField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendMessages", "OrderNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FriendMessages", "OrderNumber");
        }
    }
}
