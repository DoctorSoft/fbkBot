namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSyntaxysBugAnalisysFriendTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AnalisysFriends", newName: "AnalysisFriends");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AnalysisFriends", newName: "AnalisysFriends");
        }
    }
}
