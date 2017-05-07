namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWinksBackCounter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountStatistics", "CountOfWinksBack", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountStatistics", "CountOfWinksBack");
        }
    }
}
