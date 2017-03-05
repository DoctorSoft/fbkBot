using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.NewSettings
{
    public class GetNewSettingsByAccountAndGroupIdQuery : IQuery<List<NewSettingsData>>
    {
        public long AccountId { get; set; }

        public long GroupId { get; set; }
    }
}
