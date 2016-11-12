using System.Collections.Generic;
using CommonModels;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand
{
    public class SaveUnreadMessagesCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<GetUnreadMessagesResponseModel> UnreadMessages { get; set; }
    }
}
