using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class AnalysisFriendConfiguration : EntityTypeConfiguration<AnalysisFriendDbModel>
    {
        public AnalysisFriendConfiguration()
        {
            ToTable("AnalysisFriends");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.FriendName);
            Property(model => model.FacebookId);
            Property(model => model.AddedDateTime);
            Property(model => model.Type);

            HasRequired(it => it.AccountWithFriend).WithMany(m => m.AnalysisFriends).HasForeignKey(model => model.AccountId);
        }
    }
}
