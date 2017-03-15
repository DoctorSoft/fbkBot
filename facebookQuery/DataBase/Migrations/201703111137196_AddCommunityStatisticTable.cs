namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommunityStatisticTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommunityStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        CountOfGroupInvitations = c.Long(nullable: false),
                        CountOfPageInvitations = c.Long(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommunityStatistics");
        }
    }
}
