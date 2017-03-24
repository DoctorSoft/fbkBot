namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScheaduleForDeletingFriends : DbMigration
    {
        public override void Up()
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
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.JobStatus", "JobInformation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobStatus", "JobInformation", c => c.String());
            DropTable("dbo.ScheduleRemovalOfFriends");
        }
    }
}
