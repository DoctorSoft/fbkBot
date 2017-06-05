using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class NoticeConfiguration : EntityTypeConfiguration<NoticeDbModel>
    {
        public NoticeConfiguration()
        {
            ToTable("Notices");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.AccountId);
            Property(model => model.NoticeText);
            Property(model => model.DateTime);
        }
    }
}
