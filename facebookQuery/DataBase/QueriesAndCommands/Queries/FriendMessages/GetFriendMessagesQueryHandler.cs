using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.FriendMessages
{
    public class GetFriendMessagesQueryHandler : IQueryHandler<GetFriendMessagesQuery, List<FriendMessageData>>
    {
        private readonly DataBaseContext context;

        public GetFriendMessagesQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FriendMessageData> Handle(GetFriendMessagesQuery query)
        {
            var friendsData = context
                .FriendMessages
                .OrderByDescending(model => model.MessageDateTime)
                .Select(model => new FriendMessageData
                {
                    Message = model.Message,
                    Id = model.Id,
                    MessageDateTime = model.MessageDateTime,
                    FriendId = model.FriendId,
                    MessageDirection = model.MessageDirection,
                }).ToList();

            return friendsData;
        }
    }
}
