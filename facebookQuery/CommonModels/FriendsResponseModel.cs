using Constants.FriendTypesEnum;
using Constants.GendersUnums;

namespace CommonModels
{
    public class FriendsResponseModel
    {
        public long FacebookId { get; set; }

        public string FriendName { get; set; }

        public string Uri { get; set; }

        public GenderEnum Gender { get; set; }

        public FriendTypes Type { get; set; }
    }
}
