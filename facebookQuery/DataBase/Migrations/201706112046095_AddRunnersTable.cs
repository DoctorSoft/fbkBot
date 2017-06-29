namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRunnersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Runners",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DeviceName = c.String(),
                        IsAllowed = c.Boolean(nullable: false),
                        LastAction = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Runners");
        }
    }
}
