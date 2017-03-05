using Constants.FriendTypesEnum;

namespace DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendTypeCommand
{
    public class ChangeAnalysisFriendTypeCommand : IVoidCommand
    {
        public long FriendFacebookId { get; set; }

        public long AccountId { get; set; }

        public FriendTypes NewType { get; set; }
    }
}
