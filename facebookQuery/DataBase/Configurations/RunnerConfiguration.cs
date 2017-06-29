using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class RunnerConfiguration : EntityTypeConfiguration<RunnerDbModel>
    {
        public RunnerConfiguration()
        {
            ToTable("Runners");

            Property(model => model.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(model => model.DeviceName);
            Property(model => model.IsAllowed);
            Property(model => model.LastAction);
            Property(model => model.CreateDate);
        }
    }
}
