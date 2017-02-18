using System.ComponentModel;

namespace Constants.JobsEnums
{
    public enum JobNames
    {
        [Description("Refresh friends")]
        RefreshFriends = 1, 
        [Description("Confirm friendship")]
        ConfirmFriendship = 2,
        [Description("Send message to unread")]
        SendMessageToUnread = 3,
        [Description("Send message to unanswered")]
        SendMessageToUnanswered = 4,
        [Description("Send message to new friends")]
        SendMessageToNewFriends = 5,
        [Description("Get new friends and recommended")]
        GetNewFriendsAndRecommended = 6,
        [Description("Send request friendship")]
        SendRequestFriendship = 7,
        [Description("Refresh cookies")]
        RefreshCookies = 8
    }
}
