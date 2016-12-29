using Constants.FriendTypesEnum;

namespace DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendStatusCommand
{
    public class ChangeAnalysisFriendStatusCommand : IVoidCommand
    {
        public long FriendFacebookId { get; set; }

        public long AccountId { get; set; }

        public StatusesFriend NewStatus { get; set; }
    }
}
