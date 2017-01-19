using System.Data.Entity.ModelConfiguration;
using DataBase.Models;

namespace DataBase.Configurations
{
    public class SettingsConfiguration : EntityTypeConfiguration<SettingsDbModel>
    {
        public SettingsConfiguration()
        {
            ToTable("Settings");

            HasKey(model => model.Id);
            Property(model => model.LivesPlace);
            Property(model => model.SchoolPlace);
            Property(model => model.WorkPlace);
            Property(model => model.Gender);
            Property(model => model.DelayTimeSendUnread);
            Property(model => model.DelayTimeSendUnanswered);
            Property(model => model.DelayTimeSendNewFriend);
            Property(model => model.UnansweredDelay);

            HasRequired(it => it.SettingsGroup).WithOptional(m=>m.Settings);
        }
    }
}
