namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldAccountId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "UserId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "UserId");
        }
    }
}
