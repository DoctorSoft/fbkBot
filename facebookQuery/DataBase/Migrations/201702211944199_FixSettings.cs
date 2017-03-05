namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSettings : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Settings", newName: "SettingsDbModels");
            DropIndex("dbo.SettingsDbModels", new[] { "Id" });
            DropPrimaryKey("dbo.SettingsDbModels");
            AlterColumn("dbo.SettingsDbModels", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.SettingsDbModels", "Id");
            CreateIndex("dbo.SettingsDbModels", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SettingsDbModels", new[] { "Id" });
            DropPrimaryKey("dbo.SettingsDbModels");
            AlterColumn("dbo.SettingsDbModels", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.SettingsDbModels", "Id");
            CreateIndex("dbo.SettingsDbModels", "Id");
            RenameTable(name: "dbo.SettingsDbModels", newName: "Settings");
        }
    }
}
