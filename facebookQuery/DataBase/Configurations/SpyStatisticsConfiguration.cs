using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class SpyStatisticsConfiguration : EntityTypeConfiguration<SpyStatisticsDbModel>
    {
        public SpyStatisticsConfiguration()
        {
            ToTable("SpyStatistics");

            HasKey(model => model.Id);

            Property(model => model.SpyId);
            Property(model => model.CountAnalizeFriends);
            Property(model => model.DateTimeUpdateStatistics);
            Property(model => model.CreateDateTime);
        }
    }
}
