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
    }
}
