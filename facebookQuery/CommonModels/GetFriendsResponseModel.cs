using Constants.GendersUnums;

namespace CommonModels
{
    public class GetFriendsResponseModel
    {
        public long FacebookId { get; set; }

        public string FriendName { get; set; }

        public string Uri { get; set; }

        public GenderEnum Gender { get; set; }
    }
}
