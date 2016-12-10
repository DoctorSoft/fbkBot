using System;
using System.Collections.Generic;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;

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

                    var lastBotMessageDateTime = lastBotMessage.MessageDateTime;

                    var lastFriendMessage = context
                        .FriendMessages.Where(
                            model =>
                                model.Friend.AccountId == query.AccountId && model.Friend.IsBlocked == false &&
                                model.FriendId == friendMessageDbModel.FriendId)
                        .Where(model => model.MessageDirection == MessageDirection.FromFriend)
                        .OrderByDescending(model => model.MessageDateTime).FirstOrDefault();


                    if (lastFriendMessage == null)
                    {
                        if (CheckDelay(lastBotMessageDateTime, query.DelayTime))
                        {
                            unansweredMessages.Add(new FriendMessageData()
                            {
                                Id = lastBotMessage.Id,
                                FriendId = lastBotMessage.FriendId,
                                OrderNumber = lastBotMessage.OrderNumber,
                                Message = lastBotMessage.Message,
                                MessageDateTime = lastBotMessage.MessageDateTime,
                                MessageDirection = lastBotMessage.MessageDirection
                            });
                        }
                        continue;

                    }
                    var lastFriendMessageDateTime = lastFriendMessage.MessageDateTime;

                    var answered = lastFriendMessageDateTime > lastBotMessageDateTime;
                    if (answered)
                    {
                        continue;
                    }

                    if (CheckDelay(lastFriendMessageDateTime, query.DelayTime))
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
            catch (Exception ex)
            {
                return unansweredMessages;
            }
            return unansweredMessages;
        }

        private bool CheckDelay(DateTime friendAddedDateTime, int delay)
        {
            var differenceTime = DateTime.Now - friendAddedDateTime;
            var summ = differenceTime.Days * 24 * 60 + differenceTime.Hours * 60 + differenceTime.Minutes;
            return summ >= delay;
        }
    }
}
