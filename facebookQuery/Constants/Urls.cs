﻿using System.ComponentModel;

namespace Constants
{
    public enum Urls
    {
        [Description("https://www.facebook.com")] HomePage,
        [Description("https://www.facebook.com/messaging/send/?dpr=1")] SendMessage,
        [Description("https://www.facebook.com/ajax/mercury/threadlist_info.php?dpr=1")] NewMessages,
        [Description("https://www.facebook.com/ajax/presence/reconnect.php?reason=6")] NewCorrespondenceRequests,
        [Description("https://www.facebook.com/rtc/callability/")] GetСorrespondenceByFriendId,
        [Description("https://www.facebook.com/profile.php")] GetFriends,
        [Description("https://www.facebook.com/friends/requests/?split=1&fcref=ft")] GetRecommendedFriends,
        [Description("https://www.facebook.com/ajax/mercury/change_read_status.php?dpr=1")] ChangeReadStatus,
        [Description("https://www.facebook.com/ajax/growth/friend_browser/checkbox.php?dpr=1")] GetFriendsByCriteries,
        [Description("https://www.facebook.com/ajax/add_friend/action.php?dpr=1")] AddFriend,
        [Description("https://www.facebook.com/pubcontent/chained_suggestions/")] AddFriendExtra,
        [Description("https://www.facebook.com/pokes/dialog/")] Wink,
        [Description("https://www.facebook.com/requests/friends/ajax/?dpr=1")] ConfirmFriendship,
        [Description("https://www.facebook.com/ajax/profile/removefriendconfirm.php?dpr=1")] RemoveFriend,
        [Description("https://www.facebook.com/ajax/reqs.php?dpr=1 ")] CancelFriendshipRequest,
        [Description("https://www.facebook.com/ajax/groups/members/add_post.php?source=dialog_typeahead&group_id={0}&refresh=1")] AddFriendToGroup,
        [Description("https://www.facebook.com/ajax/pages/invite/send_single/?dpr=1")] AddFriendToPage,

        //Winks
        [Description("https://www.facebook.com/pokes")] GetNewWinks,
    }
}
