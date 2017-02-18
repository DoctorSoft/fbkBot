namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoticesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        NoticeText = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notices");
        }
    }
}