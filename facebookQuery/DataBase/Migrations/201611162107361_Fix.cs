namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "MessageGroupId", "dbo.MessageGroupDbModels");
            DropIndex("dbo.Messages", new[] { "MessageGroupId" });
            DropColumn("dbo.Messages", "MessageGroupId");
            DropTable("dbo.MessageGroupDbModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MessageGroupDbModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Messages", "MessageGroupId", c => c.Long());
            CreateIndex("dbo.Messages", "MessageGroupId");
            AddForeignKey("dbo.Messages", "MessageGroupId", "dbo.MessageGroupDbModels", "Id");
        }
    }
}
