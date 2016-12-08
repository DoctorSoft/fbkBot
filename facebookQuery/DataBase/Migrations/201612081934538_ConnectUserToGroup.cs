namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectUserToGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "MessageGroupId", c => c.Long());
            CreateIndex("dbo.Accounts", "MessageGroupId");
            AddForeignKey("dbo.Accounts", "MessageGroupId", "dbo.MessageGroupDbModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "MessageGroupId", "dbo.MessageGroupDbModels");
            DropIndex("dbo.Accounts", new[] { "MessageGroupId" });
            DropColumn("dbo.Accounts", "MessageGroupId");
        }
    }
}
