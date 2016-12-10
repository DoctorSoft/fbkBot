namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFunctionTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FunctionTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionTypeName = c.Int(nullable: false),
                        TypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Functions", "FunctionTypeId", c => c.Long(nullable: false));
            CreateIndex("dbo.Functions", "FunctionTypeId");
            AddForeignKey("dbo.Functions", "FunctionTypeId", "dbo.FunctionTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Functions", "FunctionTypeId", "dbo.FunctionTypes");
            DropIndex("dbo.Functions", new[] { "FunctionTypeId" });
            DropColumn("dbo.Functions", "FunctionTypeId");
            DropTable("dbo.FunctionTypes");
        }
    }
}
