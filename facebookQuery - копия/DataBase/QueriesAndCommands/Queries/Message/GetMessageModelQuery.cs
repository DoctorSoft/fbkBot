using System.Collections.Generic;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Message
{
    public class GetMessageModelQuery : IQuery<List<MessageModel>>
    {
        public long? AccountId { get; set; }

        public long? GroupId { get; set; }
    }
}
