namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGeoOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "Cities", c => c.String());
            AddColumn("dbo.Settings", "Countries", c => c.String());
            DropColumn("dbo.Settings", "LivesPlace");
            DropColumn("dbo.Settings", "SchoolPlace");
            DropColumn("dbo.Settings", "WorkPlace");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "WorkPlace", c => c.String());
            AddColumn("dbo.Settings", "SchoolPlace", c => c.String());
            AddColumn("dbo.Settings", "LivesPlace", c => c.String());
            DropColumn("dbo.Settings", "Countries");
            DropColumn("dbo.Settings", "Cities");
        }
    }
}
