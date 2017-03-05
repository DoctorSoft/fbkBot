namespace DataBase.QueriesAndCommands.Commands.FriendsBlackList.ClearFriendsBlackListByGroupIdCommand
{
    public class ClearFriendsBlackListByGroupIdCommand : IVoidCommand
    {
        public long GroupSettingsId { get; set; }
    }
}

