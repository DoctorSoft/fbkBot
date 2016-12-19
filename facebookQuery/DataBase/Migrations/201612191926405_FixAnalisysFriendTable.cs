namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAnalisysFriendTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AnalisysFriendDbModels", newName: "AnalisysFriends");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AnalisysFriends", newName: "AnalisysFriendDbModels");
        }
    }
}
