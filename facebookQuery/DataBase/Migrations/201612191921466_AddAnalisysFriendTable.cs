namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnalisysFriendTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnalisysFriendDbModels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FacebookId = c.Long(nullable: false),
                        FriendName = c.String(),
                        AccountId = c.Long(nullable: false),
                        AddedDateTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnalisysFriendDbModels", "AccountId", "dbo.Accounts");
            DropIndex("dbo.AnalisysFriendDbModels", new[] { "AccountId" });
            DropTable("dbo.AnalisysFriendDbModels");
        }
    }
}
