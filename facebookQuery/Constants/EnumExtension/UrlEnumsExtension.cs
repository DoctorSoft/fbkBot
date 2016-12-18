﻿using System;
using Constants.UrlEnums;

namespace Constants.EnumExtension
{
    public static class UrlEnumsExtension
    {
        public static string GetAttributeName(this SendMessageEnum messageEnum)
        {
            switch (messageEnum)
            {
                case SendMessageEnum.Client:
                    return "client="; 
                case SendMessageEnum.ActionType:
                    return "action_type="; 
                case SendMessageEnum.Body:
                    return "body="; 
                case SendMessageEnum.EphemeralTtlMode:
                    return "ephemeral_ttl_mode="; 
                case SendMessageEnum.HasAttachment:
                    return "has_attachment="; 
                case SendMessageEnum.MessageId:
                    return "message_id= ="; 
                case SendMessageEnum.OfflineThreadingId:
                    return "offline_threading_id="; 
                case SendMessageEnum.OtherUserFbid:
                    return "other_user_fbid="; 
                case SendMessageEnum.Source:
                    return "source="; 
                case SendMessageEnum.SignatureId:
                    return "signature_id="; 
                case SendMessageEnum.SpecificToListOne:
                    return "specific_to_list[0]=fbid:"; 
                case SendMessageEnum.SpecificToListTwo:
                    return "specific_to_list[1]=fbid:"; 
                case SendMessageEnum.Timestamp:
                    return "timestamp="; 
                case SendMessageEnum.UiPushPhase:
                    return "ui_push_phase="; 
                case SendMessageEnum.UserId:
                    return "__user="; 
                case SendMessageEnum.A:
                    return "__a="; 
                case SendMessageEnum.Dyn:
                    return "__dyn="; 
                case SendMessageEnum.Af:
                    return "__af="; 
                case SendMessageEnum.Req:
                    return "__req="; 
                case SendMessageEnum.Be:
                    return "__be="; 
                case SendMessageEnum.Pc:
                    return "__pc="; 
                case SendMessageEnum.FbDtsg:
                    return "fb_dtsg="; 
                case SendMessageEnum.Ttstamp:
                    return "ttstamp="; 
                case SendMessageEnum.Rev:
                    return "__rev="; 
                case SendMessageEnum.SrpT:
                    return "__srp_t="; 
                default:
                    throw new ArgumentOutOfRangeException("messageEnum");
            }
        }

        public static string GetAttributeName(this GetUnreadMessagesEnum messageEnum)
        {
            switch (messageEnum)
            {
                case GetUnreadMessagesEnum.Client:
                    return "client=";
                case GetUnreadMessagesEnum.InboxOffset:
                    return "inbox[offset]=";
                case GetUnreadMessagesEnum.InboxLimit:
                    return "inbox[limit]=";
                case GetUnreadMessagesEnum.InboxFilter:
                    return "inbox[filter]="; 
                case GetUnreadMessagesEnum.User:
                    return "__user="; 
                case GetUnreadMessagesEnum.A:
                    return "__a=";
                case GetUnreadMessagesEnum.Be:
                    return "__be=";
                case GetUnreadMessagesEnum.Pc:
                    return "__pc=";
                case GetUnreadMessagesEnum.FbDtsg:
                    return "fb_dtsg="; 
                default:
                    throw new ArgumentOutOfRangeException("messageEnum");
            }
        }
        public static string GetAttributeName(this GetMessagesEnum messageEnum)
        {
            switch (messageEnum)
            {
                case GetMessagesEnum.Client:
                    return "client=";
                case GetMessagesEnum.InboxOffset:
                    return "inbox[offset]=";
                case GetMessagesEnum.InboxLimit:
                    return "inbox[limit]=";
                case GetMessagesEnum.InboxFilter:
                    return "inbox[filter]=";
                case GetMessagesEnum.User:
                    return "__user=";
                case GetMessagesEnum.A:
                    return "__a=";
                case GetMessagesEnum.Be:
                    return "__be=";
                case GetMessagesEnum.Pc:
                    return "__pc=";
                case GetMessagesEnum.FbDtsg:
                    return "fb_dtsg=";
                default:
                    throw new ArgumentOutOfRangeException("messageEnum");
            }
        }
        public static string GetAttributeName(this GetCorrespondenceEnum messageEnum)
        {
            switch (messageEnum)
            {
                case GetCorrespondenceEnum.User:
                    return "__user=";
                case GetCorrespondenceEnum.A:
                    return "__a=";
                case GetCorrespondenceEnum.Dyn:
                    return "__dyn=";
                case GetCorrespondenceEnum.Af:
                    return "__af=";
                case GetCorrespondenceEnum.Req:
                    return "__req=";
                case GetCorrespondenceEnum.Be:
                    return "__be=";
                case GetCorrespondenceEnum.Pc:
                    return "__pc=";
                case GetCorrespondenceEnum.FbDtsg:
                    return "fb_dtsg=";
                case GetCorrespondenceEnum.Ttstamp:
                    return "ttstamp=";
                case GetCorrespondenceEnum.Rev:
                    return "__rev=";
                case GetCorrespondenceEnum.SrpT:
                    return "__srp_t=";
                default:
                    throw new ArgumentOutOfRangeException("messageEnum", messageEnum, null);
            }
        }

        public static string GetAttributeName(this GetFriendsEnum messageEnum)
        {
            switch (messageEnum)
            {
                case GetFriendsEnum.Id:
                    return "id=";
                case GetFriendsEnum.Sk:
                    return "sk=";
                default:
                    throw new ArgumentOutOfRangeException("messageEnum");
            }
        }
        
        public static string GetAttributeName(this ChangeStatusForMesagesEnum changeStatusEnum)
        {
            switch (changeStatusEnum)
            {
                case ChangeStatusForMesagesEnum.Ids:
                    return "ids[";
                case ChangeStatusForMesagesEnum.WatermarkTimestamp:
                    return "watermarkTimestamp=";
                case ChangeStatusForMesagesEnum.TitanOriginatedThreadId:
                    return "titanOriginatedThreadId=";
                case ChangeStatusForMesagesEnum.ShouldSendReadReceipt:
                    return "shouldSendReadReceipt=";
                case ChangeStatusForMesagesEnum.CommerceLastMessageType:
                    return "commerce_last_message_type=";
                case ChangeStatusForMesagesEnum.User:
                    return "__user=";
                case ChangeStatusForMesagesEnum.A:
                    return "__a=";
                case ChangeStatusForMesagesEnum.Dyn:
                    return "__dyn=";
                case ChangeStatusForMesagesEnum.Af:
                    return "__af=";
                case ChangeStatusForMesagesEnum.Req:
                    return "__req=";
                case ChangeStatusForMesagesEnum.Be:
                    return "__be=";
                case ChangeStatusForMesagesEnum.Pc:
                    return "__pc=";
                case ChangeStatusForMesagesEnum.Rev:
                    return "__rev=";
                case ChangeStatusForMesagesEnum.FbDtsg:
                    return "fb_dtsg=";
                case ChangeStatusForMesagesEnum.Ttstamp:
                    return "ttstamp=";
                default:
                    throw new ArgumentOutOfRangeException("changeStatusEnum", changeStatusEnum, null);
            }
        }

        public static string GetAttributeName(this GetFriendsByCriteriesEnum getFriendsByCriteriesEnum)
        {
            switch (getFriendsByCriteriesEnum)
            {
                case GetFriendsByCriteriesEnum.FbDtsg:
                    return "fb_dtsg=";
                case GetFriendsByCriteriesEnum.FriendBrowserIdZero:
                    return "friend_browser_id[0]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId1:
                    return "friend_browser_id[1]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId2:
                    return "friend_browser_id[2]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId3:
                    return "friend_browser_id[3]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId4:
                    return "friend_browser_id[4]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId5:
                    return "friend_browser_id[5]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId6:
                    return "friend_browser_id[6]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId7:
                    return "friend_browser_id[7]=";
                case GetFriendsByCriteriesEnum.FriendBrowserId8:
                    return "friend_browser_id[8]=";
                case GetFriendsByCriteriesEnum.ExtraData:
                    return "extra_data=";
                case GetFriendsByCriteriesEnum.HowFound:
                    return "how_found=";
                case GetFriendsByCriteriesEnum.Page:
                    return "page=";
                case GetFriendsByCriteriesEnum.InstanceName:
                    return "instance_name=";
                case GetFriendsByCriteriesEnum.BigPics:
                    return "big_pics=";
                case GetFriendsByCriteriesEnum.SocialContext:
                    return "social_context=";
                case GetFriendsByCriteriesEnum.NetworkContext:
                    return "network_context=";
                case GetFriendsByCriteriesEnum.NameIdsZero:
                    return "name_ids[0]=";
                case GetFriendsByCriteriesEnum.NameInput:
                    return "name_input=";
                case GetFriendsByCriteriesEnum.HomeTownIdsZero:
                    return "hometown_ids[0]=";
                case GetFriendsByCriteriesEnum.CityIdsZero:
                    return "city_ids[0]=";
                case GetFriendsByCriteriesEnum.GradschoolIdsZero:
                    return "gradschool_ids[0]=";
                case GetFriendsByCriteriesEnum.UsedTypeahead:
                    return "used_typeahead=";
                case GetFriendsByCriteriesEnum.User:
                    return "__user=";
                case GetFriendsByCriteriesEnum.A:
                    return "__a=";
                case GetFriendsByCriteriesEnum.Dyn:
                    return "__dyn=";
                case GetFriendsByCriteriesEnum.Af:
                    return "__af=";
                case GetFriendsByCriteriesEnum.Req:
                    return "__req=";
                case GetFriendsByCriteriesEnum.Be:
                    return "__be=";
                case GetFriendsByCriteriesEnum.Pc:
                    return "__pc=";
                case GetFriendsByCriteriesEnum.Rev:
                    return "__rev=";
                case GetFriendsByCriteriesEnum.Ttstamp:
                    return "ttstamp=";
                default:
                    throw new ArgumentOutOfRangeException("getFriendsByCriteriesEnum");
            }
        }

        public static string GetAttributeName(this AddFriendEnum addFriendEnum)
        {
            switch (addFriendEnum)
            {
                case AddFriendEnum.FbDtsg:
                    return "fb_dtsg=";
                case AddFriendEnum.ToFriend:
                    return "to_friend=";
                case AddFriendEnum.Action:
                    return "action=";
                case AddFriendEnum.HowFound:
                    return "how_found=";
                case AddFriendEnum.RefParam:
                    return "ref_param=";
                case AddFriendEnum.OutgoingId:
                    return "outgoing_id=";
                case AddFriendEnum.LoggingLocation:
                    return "logging_location=";
                case AddFriendEnum.NoFlyoutOnClick:
                    return "no_flyout_on_click=";
                case AddFriendEnum.Floc:
                    return "floc=";
                case AddFriendEnum.Frefs0:
                    return "frefs[0]=";
                case AddFriendEnum.Frefs1:
                    return "frefs[1]=";
                case AddFriendEnum.User:
                    return "__user=";
                case AddFriendEnum.A:
                    return "__a=";
                case AddFriendEnum.Dyn:
                    return "__dyn=";
                case AddFriendEnum.Af:
                    return "__af=";
                case AddFriendEnum.Req:
                    return "__req=";
                case AddFriendEnum.Be:
                    return "__be=";
                case AddFriendEnum.Pc:
                    return "__pc=";
                case AddFriendEnum.Rev:
                    return "__rev=";
                case AddFriendEnum.Ttstamp:
                    return "ttstamp=";
                case AddFriendEnum.EgoLogData:
                    return "ego_log_data=";
                case AddFriendEnum.HttpReferer:
                    return "http_referer=";
                default:
                    throw new ArgumentOutOfRangeException("addFriendEnum");
            }
        }

        public static string GetAttributeName(this AddFriendExtraEnum addFriendExtraEnum)
        {
            switch (addFriendExtraEnum)
            {
                case AddFriendExtraEnum.User:
                    return "__user=";
                case AddFriendExtraEnum.A:
                    return "__a=";
                case AddFriendExtraEnum.Dyn:
                    return "__dyn=";
                case AddFriendExtraEnum.Af:
                    return "__af=";
                case AddFriendExtraEnum.Req:
                    return "__req=";
                case AddFriendExtraEnum.Be:
                    return "__be=";
                case AddFriendExtraEnum.Pc:
                    return "__pc=";
                case AddFriendExtraEnum.Rev:
                    return "__rev=";
                case AddFriendExtraEnum.Ttstamp:
                    return "ttstamp=";
                case AddFriendExtraEnum.FbDtsg:
                    return "fb_dtsg=";
                default:
                    throw new ArgumentOutOfRangeException("addFriendExtraEnum");
            }
        }

        public static string GetAttributeName(this WinkEnum winkEnum)
        {
            switch (winkEnum)
            {
                case WinkEnum.PokeTarget:
                    return "poke_target=";
                case WinkEnum.Nctr:
                    return "nctr[_mod]=";
                case WinkEnum.AsyncDialog:
                    return "__asyncDialog=";
                case WinkEnum.User:
                    return "__user=";
                case WinkEnum.A:
                    return "__a=";
                case WinkEnum.Dyn:
                    return "__dyn=";
                case WinkEnum.Af:
                    return "__af=";
                case WinkEnum.Req:
                    return "__req=";
                case WinkEnum.Be:
                    return "__be=";
                case WinkEnum.Pc:
                    return "__pc=";
                case WinkEnum.Rev:
                    return "__rev=";
                case WinkEnum.Ttstamp:
                    return "ttstamp=";
                case WinkEnum.FbDtsg:
                    return "fb_dtsg=";
                default:
                    throw new ArgumentOutOfRangeException("winkEnum");
            }
        }
    }
}
