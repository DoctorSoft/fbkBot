using System.Collections.Generic;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using Engines.Engines.Models;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand
{
    public class SaveUnreadMessagesCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<FacebookMessageModel> UnreadMessages { get; set; }
    }
}
