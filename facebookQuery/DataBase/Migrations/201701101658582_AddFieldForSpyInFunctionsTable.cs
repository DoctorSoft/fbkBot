namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldForSpyInFunctionsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Functions", "ForSpy", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Functions", "ForSpy");
        }
    }
}
