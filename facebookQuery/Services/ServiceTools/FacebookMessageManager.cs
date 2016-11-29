using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class FacebookMessageManager : IFacebookMessageManager
    {
        public FriendMessageData GetLastFriendMessageModel(long senderId, long friendId)
        {
            return new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery()
            {
                AccountId = senderId,
                FriendId = friendId
            }).Where(data => data.MessageDirection == MessageDirection.FromFriend)
                .OrderByDescending(data => data.OrderNumber)
                .FirstOrDefault();
        }
    }
}
