namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupFunctionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupFunctions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionId = c.Long(nullable: false),
                        GroupId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Functions", t => t.FunctionId, cascadeDelete: true)
                .ForeignKey("dbo.MessageGroupDbModels", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.FunctionId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FunctionName = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupFunctions", "GroupId", "dbo.MessageGroupDbModels");
            DropForeignKey("dbo.GroupFunctions", "FunctionId", "dbo.Functions");
            DropIndex("dbo.GroupFunctions", new[] { "GroupId" });
            DropIndex("dbo.GroupFunctions", new[] { "FunctionId" });
            DropTable("dbo.Functions");
            DropTable("dbo.GroupFunctions");
        }
    }
}
