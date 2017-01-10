namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpyFunctionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpyFunctionDbModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionId = c.Long(nullable: false),
                        SpyId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpyAccounts", t => t.SpyId, cascadeDelete: true)
                .ForeignKey("dbo.Functions", t => t.FunctionId, cascadeDelete: true)
                .Index(t => t.FunctionId)
                .Index(t => t.SpyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpyFunctionDbModels", "FunctionId", "dbo.Functions");
            DropForeignKey("dbo.SpyFunctionDbModels", "SpyId", "dbo.SpyAccounts");
            DropIndex("dbo.SpyFunctionDbModels", new[] { "SpyId" });
            DropIndex("dbo.SpyFunctionDbModels", new[] { "FunctionId" });
            DropTable("dbo.SpyFunctionDbModels");
        }
    }
}
