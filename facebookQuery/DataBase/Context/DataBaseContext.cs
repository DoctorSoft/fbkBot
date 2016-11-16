using System.Data.Entity;
using DataBase.Configurations;
using DataBase.Models;

namespace DataBase.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            :base("DefaultConnection")
        {

        }
        
        public DbSet<AccountDbModel> Accounts { get; set; }

        public DbSet<CookiesDbModel> Cookies { get; set; }

        public DbSet<MessageDbModel> Messages { get; set; }

        public DbSet<UrlParametersDbModel> UrlParameters { get; set; }

        public DbSet<FriendDbModel> Friends { get; set; }

        public DbSet<FriendMessageDbModel> FriendMessages { get; set; }

        public DbSet<MessageGroupDbModel> MessageGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new CookiesConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new UrlParametersConfiguration());
            modelBuilder.Configurations.Add(new FriendConfiguration());
            modelBuilder.Configurations.Add(new FriendMessageConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
