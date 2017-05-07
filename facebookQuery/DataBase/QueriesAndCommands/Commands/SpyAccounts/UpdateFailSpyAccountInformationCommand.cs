namespace DataBase.QueriesAndCommands.Commands.SpyAccounts
{
    public class UpdateFailSpyAccountInformationCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }
        
        public bool? ProxyDataIsFailed { get; set; }

        public bool? AuthorizationDataIsFailed { get; set; }

        public bool? ConformationIsFailed { get; set; }
    }
}
