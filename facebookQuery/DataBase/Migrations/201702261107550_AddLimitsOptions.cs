namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLimitsOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "LimitsOptions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "LimitsOptions");
        }
    }
}
