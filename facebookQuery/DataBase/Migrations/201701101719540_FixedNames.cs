namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SpyFunctionDbModels", newName: "SpyFunctions");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SpyFunctions", newName: "SpyFunctionDbModels");
        }
    }
}
