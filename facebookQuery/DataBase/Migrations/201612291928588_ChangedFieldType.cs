namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFieldType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccountSettings", "Gender", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccountSettings", "Gender", c => c.Int(nullable: false));
        }
    }
}
