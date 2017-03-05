using System.Collections.Generic;
using Services.ViewModels.GroupModels;

namespace Services.ViewModels.OptionsModel
{
    public class MessageListModel
    {
        public List<MessageListItemModel> Messages { get; set; }

        public long? AccountId { get; set; }

        public long? GroupId { get; set; }

        public GroupList GroupList { get; set; }
    }
}