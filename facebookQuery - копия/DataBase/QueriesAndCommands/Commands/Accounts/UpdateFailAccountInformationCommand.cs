namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class UpdateFailAccountInformationCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }
        
        public bool? ProxyDataIsFailed { get; set; }

        public bool? AuthorizationDataIsFailed { get; set; }
    }
}
