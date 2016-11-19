namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsBlockedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "IsBlocked", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friends", "IsBlocked");
        }
    }
}
