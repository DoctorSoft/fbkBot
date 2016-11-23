namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBlockedFieldType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Friends", "IsBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Friends", "IsBlocked", c => c.Int(nullable: false));
        }
    }
}
