using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.AccountSettings
{
    public class GetAccountSettingsQueryHandler : IQueryHandler<GetAccountSettingsQuery, AccountOptionsData>
    {
        private readonly DataBaseContext context;

        public GetAccountSettingsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public AccountOptionsData Handle(GetAccountSettingsQuery query)
        {
            var settings =
                context.AccountSettings.Where(model => model.Id == query.AccountId)
                    .Select(model => new AccountOptionsData
                    {
                        AccountId = model.Id,
                        Gender = model.Gender,
                        LivesPlace = model.LivesPlace,
                        SchoolPlace = model.SchoolPlace,
                        WorkPlace = model.WorkPlace
                    }).FirstOrDefault();

            return settings;
        }
    }
}
