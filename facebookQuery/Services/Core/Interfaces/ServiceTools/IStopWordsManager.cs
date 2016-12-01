using DataBase.QueriesAndCommands.Queries.FriendMessages;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IStopWordsManager
    {
        bool CheckMessageOnEmergencyFaktor(FriendMessageData messageModel);
    }
}
