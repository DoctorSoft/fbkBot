namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpyStatisticsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpyStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CountAnalizeFriends = c.Long(nullable: false),
                        DateTimeUpdateStatistics = c.DateTime(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SpyStatistics");
        }
    }
}
