namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountInformationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountInformation",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Information = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountInformation", "Id", "dbo.Accounts");
            DropIndex("dbo.AccountInformation", new[] { "Id" });
            DropTable("dbo.AccountInformation");
        }
    }
}
