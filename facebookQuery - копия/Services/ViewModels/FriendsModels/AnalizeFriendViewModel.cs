using System;
using Constants.FriendTypesEnum;

namespace Services.ViewModels.FriendsModels
{
    public class AnalizeFriendViewModel
    {
        public long Id { get; set; }

        public long FacebookId { get; set; }

        public string FriendName { get; set; }

        public long AccountId { get; set; }

        public DateTime AddedDateTime { get; set; }

        public StatusesFriend Status { get; set; }

        public FriendTypes Type { get; set; }
    }
}
