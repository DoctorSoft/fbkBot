using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class MessageConfiguration:EntityTypeConfiguration<MessageDbModel>
    {
        public MessageConfiguration()
        {
            ToTable("Messages");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.ImportancyFactor);
            Property(model => model.IsStopped);
            Property(model => model.Message);
            Property(model => model.IsEmergencyText);
            Property(model => model.MessageRegime);
            Property(model => model.StartTime);
            Property(model => model.EndTime);

            HasOptional(it => it.Account).WithMany(model => model.Messages).HasForeignKey(model => model.AccountId);
            HasOptional(it => it.GroupSettings).WithMany(model => model.Messages).HasForeignKey(model => model.MessageGroupId);
        }
    }
}
