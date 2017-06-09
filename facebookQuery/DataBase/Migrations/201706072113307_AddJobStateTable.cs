namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobStateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobsState",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        FriendId = c.Long(),
                        FunctionName = c.Int(nullable: false),
                        AddedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobsState");
        }
    }
}
