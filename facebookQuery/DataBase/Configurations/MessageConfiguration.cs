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

            HasRequired(it => it.Account).WithMany(model => model.Messages).HasForeignKey(model => model.AccountId);
        }
    }
}
