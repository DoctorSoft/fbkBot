﻿namespace Constants.FunctionEnums
{
    public enum FunctionName
    {
        // Messages
        SendMessageToNewFriends = 1,
        SendMessageToUnanswered = 2,
        SendMessageToUnread = 3,

        // Friends
        RefreshFriends = 101,
        GetNewFriendsAndRecommended = 102,
        ConfirmFriendship = 103,
        SendRequestFriendship = 104,
        RemoveFromFriends = 105,

        //Spy
        AnalyzeFriends = 201,

        //Cookies
        RefreshCookies = 301,

        //Community
        JoinTheNewGroupsAndPages = 401,

        InviteToGroups = 403,
        InviteToPages = 404,

        //Checks
        CheckFriendsAtTheEndTimeConditions = 501,


        //Winks
        Wink = 601,
        WinkFriendFriends = 602,
        WinkBack = 603,

        // Conditions
        DialogIsOver = 1001,
        IsAddedToGroupsAndPages = 1002,
        IsWink = 1003,
        IsWinkFriendsOfFriends = 1004
    }
}
