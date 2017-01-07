using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class AccountStatisticsConfiguration : EntityTypeConfiguration<AccountStatisticsDbModel>
    {
        public AccountStatisticsConfiguration()
        {
            ToTable("AccountStatistics");

            HasKey(model => model.Id);
            
            Property(model => model.AccountId);
            Property(model => model.CountReceivedFriends);
            Property(model => model.CountRequestsSentToFriends);
            Property(model => model.DateTimeUpdateStatistics);
            Property(model => model.CreateDateTime);
        }
    }
}
