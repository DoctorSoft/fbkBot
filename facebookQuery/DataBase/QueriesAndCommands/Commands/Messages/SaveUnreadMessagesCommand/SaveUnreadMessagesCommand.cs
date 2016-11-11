using System.Collections.Generic;
using Engines.Engines.GetMessagesEngine.GetUnreadMessages;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand
{
    public class SaveUnreadMessagesCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<GetUnreadMessagesResponseModel> UnreadMessages { get; set; }
    }
}
