using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class JobStateConfiguration : EntityTypeConfiguration<JobStateDbModel>
    {
        public JobStateConfiguration()
        {
            ToTable("JobsState");

            HasKey(model => model.Id);
            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.AccountId);
            Property(model => model.FunctionName);
            Property(model => model.AddedDateTime);
            Property(model => model.FriendId);
            Property(model => model.IsForSpy);
            Property(model => model.JobId);
        }
    }
}
