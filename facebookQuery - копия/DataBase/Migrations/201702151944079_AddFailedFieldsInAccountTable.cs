namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFailedFieldsInAccountTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "ProxyDataIsFailed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accounts", "AuthorizationDataIsFailed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "AuthorizationDataIsFailed");
            DropColumn("dbo.Accounts", "ProxyDataIsFailed");
        }
    }
}
