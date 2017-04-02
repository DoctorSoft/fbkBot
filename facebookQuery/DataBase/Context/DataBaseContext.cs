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

        public DbSet<SettingsDbModel> Settings { get; set; }

        public DbSet<SpyAccountDbModel> SpyAccounts { get; set; }

        public DbSet<CookiesDbModel> Cookies { get; set; }

        public DbSet<FriendsBlackListDbModel> FriendsBlackList { get; set; }

        public DbSet<AccountStatisticsDbModel> AccountStatistics { get; set; }

        public DbSet<SpyStatisticsDbModel> SpyStatistics { get; set; }

        public DbSet<CookiesForSpyDbModel> CookiesForSpy { get; set; }
        
        public DbSet<MessageDbModel> Messages { get; set; }

        public DbSet<UrlParametersDbModel> UrlParameters { get; set; }

        public DbSet<FriendDbModel> Friends { get; set; }

        public DbSet<AnalysisFriendDbModel> AnalisysFriends { get; set; }

        public DbSet<FriendMessageDbModel> FriendMessages { get; set; }

        public DbSet<GroupSettingsDbModel> GroupSettings { get; set; }

        public DbSet<NewSettingsDbModel> NewSettings { get; set; }

        public DbSet<StopWordDbModel> StopWords { get; set; }

        public DbSet<ExtraMessageDbModel> ExtraMessages { get; set; }

        public DbSet<LinkDbModel> Links { get; set; }

        public DbSet<FunctionDbModel> Functions { get; set; }
        
        public DbSet<SpyFunctionDbModel> SpyFunctions { get; set; }

        public DbSet<GroupFunctionDbModel> GroupFunctions { get; set; }

        public DbSet<FunctionTypeDbModel> FunctionTypes { get; set; }

        public DbSet<JobQueueDbModel> JobsQueue { get; set; }

        public DbSet<JobStatusDbModel> JobStatus { get; set; }

        public DbSet<CommunityStatisticsDbModel> CommunityStatistics { get; set; }

        public DbSet<AccountInformationDbModel> AccountInformation { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new SettingsConfiguration());
            modelBuilder.Configurations.Add(new SpyAccountConfiguration());
            modelBuilder.Configurations.Add(new SpyStatisticsConfiguration());
            modelBuilder.Configurations.Add(new CookiesConfiguration());
            modelBuilder.Configurations.Add(new FriendsBlackListConfiguration());
            modelBuilder.Configurations.Add(new AccountStatisticsConfiguration());
            modelBuilder.Configurations.Add(new CookiesForSpyConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new GroupSettingsConfiguration());
            modelBuilder.Configurations.Add(new NewSettingsConfiguration());
            modelBuilder.Configurations.Add(new UrlParametersConfiguration());
            modelBuilder.Configurations.Add(new FriendConfiguration());
            modelBuilder.Configurations.Add(new AnalysisFriendConfiguration());
            modelBuilder.Configurations.Add(new FriendMessageConfiguration());
            modelBuilder.Configurations.Add(new LinkConfiguration());
            modelBuilder.Configurations.Add(new StopWordConfiguration());
            modelBuilder.Configurations.Add(new ExtraMessageConfiguration());
            modelBuilder.Configurations.Add(new FunctionConfiguration());
            modelBuilder.Configurations.Add(new SpyFunctionConfiguration());
            modelBuilder.Configurations.Add(new GroupFunctionConfiguration());
            modelBuilder.Configurations.Add(new FunctionTypeConfiguration());
            modelBuilder.Configurations.Add(new JobQueueConfiguration());
            modelBuilder.Configurations.Add(new JobStatusConfiguration());
            modelBuilder.Configurations.Add(new CommunityStatisticsConfiguration());
            modelBuilder.Configurations.Add(new AccountInformationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
