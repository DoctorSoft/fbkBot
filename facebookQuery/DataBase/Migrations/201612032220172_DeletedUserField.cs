namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedUserField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "IsDeleted");
        }
    }
}
