using System.Collections.Generic;
using Constants.MessageEnums;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.ExtraMessages;

namespace Services.Interfaces.ServiceTools
{
    public interface IMessageManager
    {
        List<MessageModel> GetAllMessagesWhereUserWritesFirst(long accountId);

        List<MessageModel> GetAllMessagesWhereBotWritesFirst(long accountId);

        MessageModel GetRandomMessage(long accountId, int orderNumber, bool isEmergencyText, MessageRegime? regime);

        ExtraMessagesData GetRandomExtraMessage();

        int GetLasBotMessageOrderNumber(List<MessageModel> messages, long accountId);
    }
}
