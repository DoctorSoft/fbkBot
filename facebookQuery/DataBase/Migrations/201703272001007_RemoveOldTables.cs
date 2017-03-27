namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOldTables : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ScheduleRemovalOfFriends");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ScheduleRemovalOfFriends",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        FriendId = c.Long(nullable: false),
                        FunctionName = c.Int(nullable: false),
                        AddDateTime = c.DateTime(nullable: false),
                        ToDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
