using System.Collections.Generic;
using Constants.MessageEnums;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IMessageManager
    {
        List<MessageModel> GetAllMessagesWhereUserWritesFirst(long accountId);

        List<MessageModel> GetAllMessagesWhereBotWritesFirst(long accountId);

        MessageModel GetRandomMessage(long accountId, int orderNumber, bool isEmergencyText, MessageRegime? regime);
    }
}
