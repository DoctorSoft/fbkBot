namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataCreateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cookies", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cookies", "CreateDate");
        }
    }
}
