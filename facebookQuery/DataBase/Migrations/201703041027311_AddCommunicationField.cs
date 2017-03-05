namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommunicationField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "CommunityOptions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "CommunityOptions");
        }
    }
}
