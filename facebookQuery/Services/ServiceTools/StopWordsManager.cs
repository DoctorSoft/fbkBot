using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.StopWords;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class StopWordsManager : IStopWordsManager
    {
        public bool CheckMessageOnEmergencyFaktor(FriendMessageData messageModel)
        {
            var stopWords = new GetStopWordsQueryHandler(new DataBaseContext()).Handle(new GetStopWordsQuery());
            return stopWords.Any(data => messageModel.Message.Contains(data.Name));
        }
    }
}
