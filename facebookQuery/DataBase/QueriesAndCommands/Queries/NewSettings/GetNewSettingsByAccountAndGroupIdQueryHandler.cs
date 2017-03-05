using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models.JsonModels;

namespace DataBase.QueriesAndCommands.Queries.NewSettings
{
    public class GetNewSettingsByAccountAndGroupIdQueryHandler : IQueryHandler<GetNewSettingsByAccountAndGroupIdQuery, List<NewSettingsData>>
    {
        private readonly DataBaseContext _context;

        public GetNewSettingsByAccountAndGroupIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<NewSettingsData> Handle(GetNewSettingsByAccountAndGroupIdQuery query)
        {
            var jsDeserializator = new JavaScriptSerializer();

            var databaseModels = _context.NewSettings
               .Where(data => data.AccountId == query.AccountId && data.SettingsGroupId == query.GroupId)
               .ToList();

            var result = databaseModels.Select(model => new NewSettingsData
                {
                    AccountId = model.AccountId,
                    Id = model.Id,
                    CommunityOptions = jsDeserializator.Deserialize<CommunityOptionsDbModel>(model.CommunityOptions),
                    SettingsGroupId = model.SettingsGroupId
                }).ToList();

            return result;
        }
    }
}
