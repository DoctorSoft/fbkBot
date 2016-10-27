namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoginAndPasswordFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Login", c => c.String());
            AddColumn("dbo.Accounts", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Password");
            DropColumn("dbo.Accounts", "Login");
        }
    }
}
