namespace DataBase.QueriesAndCommands.Queries.Settings
{
    public class GetSettingsByGroupSettingsIdQuery : IQuery<SettingsData>
    {
        public long GroupSettingsId { get; set; }
    }
}
