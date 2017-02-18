namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropIsBlockField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Friends", "IsBlocked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friends", "IsBlocked", c => c.Boolean(nullable: false));
        }
    }
}
