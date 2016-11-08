namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendMessagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FriendId = c.String(),
                        FriendName = c.String(),
                        AccountId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.FriendMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        MessageDateTime = c.DateTime(nullable: false),
                        MessageDirection = c.Int(nullable: false),
                        FriendId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Friends", t => t.FriendId, cascadeDelete: true)
                .Index(t => t.FriendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.FriendMessages", "FriendId", "dbo.Friends");
            DropIndex("dbo.FriendMessages", new[] { "FriendId" });
            DropIndex("dbo.Friends", new[] { "AccountId" });
            DropTable("dbo.FriendMessages");
            DropTable("dbo.Friends");
        }
    }
}
