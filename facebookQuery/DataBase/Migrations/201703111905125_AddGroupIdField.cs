namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupIdField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityStatistics", "GroupId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommunityStatistics", "GroupId");
        }
    }
}
