using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using Services.ViewModels.FriendMessagesModels;

namespace Services.Services
{
    public class FriendMessagesService
    {
        public FriendMessageList GetFriendsMessages(long accountId, long friendId)
        {
            var messages = new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery
            {
                AccountId = accountId,
                FriendId = friendId
            });

            return new FriendMessageList
            {
                AccountId = accountId,
                FriendId = friendId,
                FriendMessages = messages.Select(data => new FriendMessage
                {
                    Id = data.Id,
                    Message = data.Message,
                    MessageDateTime = data.MessageDateTime,
                    MessageDirection = data.MessageDirection
                }).ToList()
            };
        }
    }
}
