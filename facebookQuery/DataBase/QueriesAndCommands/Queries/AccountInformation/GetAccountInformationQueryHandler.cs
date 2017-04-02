using System;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models.JsonModels.AccountInformationModels;

namespace DataBase.QueriesAndCommands.Queries.AccountInformation
{
    public class GetAccountInformationQueryHandler : IQueryHandler<GetAccountInformationQuery, AccountInformation>
    {
        private readonly DataBaseContext _context;

        public GetAccountInformationQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public AccountInformation Handle(GetAccountInformationQuery query)
        {
            try
            {
                var accountInformationDbModel = _context.AccountInformation.FirstOrDefault(model => model.Id == query.AccountId);

                if (accountInformationDbModel == null)
                {
                    return null;
                }

                var jsDeserializator = new JavaScriptSerializer();

                var informationModel = jsDeserializator.Deserialize<AccountInformationDataDbModel>(accountInformationDbModel.Information);
                return new AccountInformation
                {
                    Id = query.AccountId,
                    AccountInformationData = informationModel
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
