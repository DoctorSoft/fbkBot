using DataBase.QueriesAndCommands.Models.JsonModels.AccountInformationModels;

namespace DataBase.QueriesAndCommands.Commands.AccountInformation
{
    public class AddAccountInformationCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }

        public AccountInformationDataDbModel AccountInformationData { get; set; }
    }
}
