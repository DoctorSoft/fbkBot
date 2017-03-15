using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class CommunityStatisticsConfiguration : EntityTypeConfiguration<CommunityStatisticsDbModel>
    {
        public CommunityStatisticsConfiguration()
        {
            ToTable("CommunityStatistics");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.AccountId);
            Property(model => model.GroupId);
            Property(model => model.CountOfGroupInvitations);
            Property(model => model.CountOfPageInvitations);
            Property(model => model.CountOfPageInvitations);
            Property(model => model.UpdateDateTime);
        }
    }
}
