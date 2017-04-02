using DataBase.QueriesAndCommands.Models.JsonModels.AccountInformationModels;

namespace DataBase.QueriesAndCommands.Queries.AccountInformation
{
    public class AccountInformation
    {
        public long Id { get; set; }

        public AccountInformationDataDbModel AccountInformationData { get; set; }
    }
}
