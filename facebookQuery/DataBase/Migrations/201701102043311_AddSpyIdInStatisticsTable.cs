namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpyIdInStatisticsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpyStatistics", "SpyId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpyStatistics", "SpyId");
        }
    }
}
