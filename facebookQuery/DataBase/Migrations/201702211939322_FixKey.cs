namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixKey : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Settings", new[] { "Id" });
            DropPrimaryKey("dbo.Settings");
            AlterColumn("dbo.Settings", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Settings", "Id");
            CreateIndex("dbo.Settings", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Settings", new[] { "Id" });
            DropPrimaryKey("dbo.Settings");
            AlterColumn("dbo.Settings", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Settings", "Id");
            CreateIndex("dbo.Settings", "Id");
        }
    }
}
