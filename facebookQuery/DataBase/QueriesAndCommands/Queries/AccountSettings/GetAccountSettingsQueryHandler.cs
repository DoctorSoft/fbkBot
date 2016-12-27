using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.AccountSettings
{
    public class GetAccountSettingsQueryHandler : IQueryHandler<GetAccountSettingsQuery, AccountSettingsModel>
    {
        private readonly DataBaseContext context;

        public GetAccountSettingsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public AccountSettingsModel Handle(GetAccountSettingsQuery query)
        {
            var settings =
                context.AccountSettings.Where(model => model.Id == query.AccountId)
                    .Select(model => new AccountSettingsModel()
                    {
                        Gender = model.Gender,
                        LivesPlace = model.LivesPlace,
                        SchoolPlace = model.SchoolPlace,
                        WorkPlace = model.WorkPlace
                    }).FirstOrDefault();

            return settings;
        }
    }
}
