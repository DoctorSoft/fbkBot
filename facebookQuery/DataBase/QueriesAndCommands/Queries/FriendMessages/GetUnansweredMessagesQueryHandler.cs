using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.WebPages.Html;
using Constants.MessageEnums;
using DataBase.Context;
using Engines.Engines.GetMessagesEngine.ChangeMessageStatus;

namespace DataBase.QueriesAndCommands.Queries.FriendMessages
{
    public class GetUnansweredMessagesQueryHandler: IQueryHandler<GetUnansweredMessagesQuery, List<FriendMessageData>>
    {
        private readonly DataBaseContext context;

        public GetUnansweredMessagesQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FriendMessageData> Handle(GetUnansweredMessagesQuery query)
        {
            var currentTime = DateTime.Now;
            var unansweredMessages = new List<FriendMessageData>();
            try
            {
                if (!context.FriendMessages
                    .Where(model => model.Friend.AccountId == query.AccountId && model.Friend.IsBlocked == false)
                    .Any(model => model.MessageDirection == MessageDirection.ToFriend))
                {
                    return unansweredMessages;
                }

                var friendsMessages = context.FriendMessages.GroupBy(models => models.FriendId).Select(models => models.OrderByDescending(model=>model.MessageDateTime).FirstOrDefault()).ToList(); 
                foreach (var friendMessageDbModel in friendsMessages)
                {
                    var lastBotMessageDateTime = new DateTime();
                    var lastFriendMessageDateTime = new DateTime();

                    var lastBotMessage = context
                        .FriendMessages.Where(
                            model =>
                                model.Friend.AccountId == query.AccountId && model.Friend.IsBlocked == false &&
                                model.FriendId == friendMessageDbModel.FriendId)
                        .Where(model => model.MessageDirection == MessageDirection.ToFriend)
                        .OrderByDescending(model => model.MessageDateTime)
                        .FirstOrDefault();

                    if (lastBotMessage == null)
                    {
                        continue;
                    }

                    lastBotMessageDateTime = lastBotMessage.MessageDateTime;

                    var lastFriendMessage = context
                        .FriendMessages.Where(
                            model =>
                                model.Friend.AccountId == query.AccountId && model.Friend.IsBlocked == false &&
                                model.FriendId == friendMessageDbModel.FriendId)
                        .Where(model => model.MessageDirection == MessageDirection.FromFriend)
                        .OrderByDescending(model => model.MessageDateTime).FirstOrDefault();
                    
                    if (lastFriendMessage == null)
                    {
                        continue;
                    }

                    lastFriendMessageDateTime = lastFriendMessage.MessageDateTime;

                    var answered = lastFriendMessageDateTime > lastBotMessageDateTime;

                    if (!answered)
                    {
                        var differenceTime = currentTime - lastFriendMessage.MessageDateTime;
                        if (differenceTime.Minutes >= query.DelayTime)
                        {
                            unansweredMessages.Add(new FriendMessageData()
                            {
                                Id = lastFriendMessage.Id,
                                FriendId = lastFriendMessage.FriendId,
                                OrderNumber = lastFriendMessage.OrderNumber,
                                Message = lastFriendMessage.Message,
                                MessageDateTime = lastFriendMessage.MessageDateTime,
                                MessageDirection = lastFriendMessage.MessageDirection
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return unansweredMessages;
            }
            return unansweredMessages;
        }
    }
}
