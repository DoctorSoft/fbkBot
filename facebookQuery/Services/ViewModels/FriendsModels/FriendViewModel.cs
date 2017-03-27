using System;
using Constants.MessageEnums;

namespace Services.ViewModels.FriendsModels
{
    public class FriendViewModel
    {
        public long FacebookId { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public long Id { get; set; }

        public bool MessagesEnded { get; set; }

        public DateTime AddDateTime { get; set; }

        public string Href { get; set; }
        
        public bool IsAddedToGroups { get; set; }

        public bool IsAddedToPages { get; set; }

        public bool IsWinked { get; set; }

        public MessageRegime? MessageRegime { get; set; }
    }
}
