using System.Collections.Generic;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Message;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class MessageManager : IMessageManager
    {
        public List<MessageModel> GetAllMessagesWhereUserWritesFirst(long accountId)
        {
            return new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery()
            {
                AccountId = accountId
            }).Where(model => model.MessageRegime == MessageRegime.UserFirstMessage).ToList();
        }

        public List<MessageModel> GetAllMessagesWhereBotWritesFirst(long accountId)
        {
            return new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery()
            {
                AccountId = accountId
            }).Where(model => model.MessageRegime == MessageRegime.BotFirstMessage).ToList();
        }
    }
}
