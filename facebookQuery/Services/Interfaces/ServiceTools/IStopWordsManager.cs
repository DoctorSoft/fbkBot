using DataBase.QueriesAndCommands.Queries.FriendMessages;

namespace Services.Interfaces.ServiceTools
{
    public interface IStopWordsManager
    {
        bool CheckMessageOnEmergencyFaktor(FriendMessageData messageModel);
    }
}
