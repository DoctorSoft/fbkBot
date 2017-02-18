using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand
{
    public class SaveUnreadMessagesCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<FacebookMessageDbModel> UnreadMessages { get; set; }
    }
}
