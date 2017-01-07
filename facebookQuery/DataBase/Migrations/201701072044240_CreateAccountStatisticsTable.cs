namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAccountStatisticsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        CountReceivedFriends = c.Long(nullable: false),
                        CountRequestsSentToFriends = c.Long(nullable: false),
                        DateTimeUpdateStatistics = c.DateTime(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccountStatistics");
        }
    }
}
