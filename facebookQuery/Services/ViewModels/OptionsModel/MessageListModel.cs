using System.Collections.Generic;

namespace Services.ViewModels.OptionsModel
{
    public class MessageListModel
    {
        public List<MessageListItemModel> Messages { get; set; }

        public long? AccountId { get; set; }

        public long? GroupId { get; set; }
    }
}