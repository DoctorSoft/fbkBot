namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlackListTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendsBlackList",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FriendFacebookId = c.Long(nullable: false),
                        FriendName = c.String(),
                        GroupId = c.Long(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FriendsBlackList");
        }
    }
}
