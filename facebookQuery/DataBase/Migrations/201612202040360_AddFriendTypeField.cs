namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendTypeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnalysisFriends", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnalysisFriends", "Type");
        }
    }
}
