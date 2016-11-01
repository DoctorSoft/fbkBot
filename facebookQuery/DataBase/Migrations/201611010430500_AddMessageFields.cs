namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Messages", new[] { "AccountId" });
            AddColumn("dbo.Messages", "MessageRegime", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "StartTime", c => c.Time(precision: 7));
            AddColumn("dbo.Messages", "EndTime", c => c.Time(precision: 7));
            AddColumn("dbo.Messages", "OrderNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "IsEmergencyText", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Messages", "AccountId", c => c.Long());
            CreateIndex("dbo.Messages", "AccountId");
            AddForeignKey("dbo.Messages", "AccountId", "dbo.Accounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Messages", new[] { "AccountId" });
            AlterColumn("dbo.Messages", "AccountId", c => c.Long(nullable: false));
            DropColumn("dbo.Messages", "IsEmergencyText");
            DropColumn("dbo.Messages", "OrderNumber");
            DropColumn("dbo.Messages", "EndTime");
            DropColumn("dbo.Messages", "StartTime");
            DropColumn("dbo.Messages", "MessageRegime");
            CreateIndex("dbo.Messages", "AccountId");
            AddForeignKey("dbo.Messages", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
