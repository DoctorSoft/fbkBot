namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCounterCheckFriendsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CounterCheckFriends",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        RetryNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CounterCheckFriends", "Id", "dbo.Accounts");
            DropIndex("dbo.CounterCheckFriends", new[] { "Id" });
            DropTable("dbo.CounterCheckFriends");
        }
    }
}
