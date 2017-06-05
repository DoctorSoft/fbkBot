namespace DataBase.QueriesAndCommands.Queries.Groups.GroupSettings
{
    public class GetGroupSettingsNameByIdQuery : IQuery<string>
    {
        public long? GroupId { get; set; }
    }
}
