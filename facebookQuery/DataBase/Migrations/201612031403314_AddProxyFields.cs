namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProxyFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Proxy", c => c.String());
            AddColumn("dbo.Accounts", "ProxyLogin", c => c.String());
            AddColumn("dbo.Accounts", "ProxyPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "ProxyPassword");
            DropColumn("dbo.Accounts", "ProxyLogin");
            DropColumn("dbo.Accounts", "Proxy");
        }
    }
}
