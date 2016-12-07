namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtraMessagesTable2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtraMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExtraMessages");
        }
    }
}
