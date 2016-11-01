using System;
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
    }
}
