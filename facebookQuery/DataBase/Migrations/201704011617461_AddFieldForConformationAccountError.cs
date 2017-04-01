namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldForConformationAccountError : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "ConformationIsFailed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "ConformationIsFailed");
        }
    }
}
