using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Script.Serialization;
using Constants.UrlEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.Migrations
{
    internal sealed class DataBaseConfiguration : DbMigrationsConfiguration<DataBaseContext>
    {
        public DataBaseConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(DataBaseContext context)
        {/*
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }

            var accountsList = new List<AccountDbModel>()
            {
                new AccountDbModel()
                {
                    Id = 1,
                    Login = "ms.nastasia.1983@mail.ru",
                    Password = "Ntvyjnf123",
                    Name = "Настя",
                    PageUrl = "https://www.facebook.com/profile.php?id=100013726390504",
                    FacebookId = 100013726390504,
                    Cookies = new CookiesDbModel()
                    {
                        CookiesString = "",
                        CreateDate = DateTime.Now
                    }
                },
                new AccountDbModel()
                {
                    Id = 1,
                    Login = "petya-pervyy-1999@mail.ru",
                    Password = "nWE#w(Qb",
                    Name = "Петя",
                    PageUrl = "https://www.facebook.com/profile.php?id=100013532889680",
                    FacebookId = 100013532889680,
                    Cookies = new CookiesDbModel()
                    {
                        CookiesString = "",
                        CreateDate = DateTime.Now
                    }
                },
            };

            
            context.Accounts.AddRange(accountsList);

            
            var sendMessageParameters = new Dictionary<SendMessageEnum, string>
            {
                {SendMessageEnum.Client, "mercury"},
                {SendMessageEnum.ActionType, "ma-type%3Auser-generated-message"},
                {SendMessageEnum.Body, ""},
                {SendMessageEnum.EphemeralTtlMode, "0"},
                {SendMessageEnum.HasAttachment, "false"},
                {SendMessageEnum.MessageId, ""},
                {SendMessageEnum.OfflineThreadingId, ""},
                {SendMessageEnum.OtherUserFbid, ""},
                {SendMessageEnum.Source, "source%3Achat%3Aweb"},
                {SendMessageEnum.SignatureId, "6ec32383"},
                {SendMessageEnum.SpecificToListOne, ""},
                {SendMessageEnum.SpecificToListTwo, ""},
                {SendMessageEnum.Timestamp, "1477233712253"},
                {SendMessageEnum.UiPushPhase, "V3"},
                {SendMessageEnum.UserId, ""},
                {SendMessageEnum.A, "1"},
                {
                    SendMessageEnum.Dyn,
                    "aihoFeyfyGmagngDxyG9giolzkqbxqbAKGiBAy8Z9LFwxBxvyui9wWhE98nwgUy22Ea-dhUKbkwy8xa5ZKex3BKuEjKeCxPG4GDg4ium4UpKq4GCzk58nVV8-cxnxm3i2y9ADBy8K48hxGbwBxqu49LZ1uJ12VqxOEqCV8F3qzE"
                },
                {SendMessageEnum.Af, "o"},
                {SendMessageEnum.Req, "co"},
                {SendMessageEnum.Be, "-1"},
                {SendMessageEnum.Pc, "PHASED%3ADEFAULT"},
                {SendMessageEnum.FbDtsg, "AQEX6WS-FFC3%3AAQFwo5hs9ISS"},
                {SendMessageEnum.Ttstamp, "2658169885487834570706751586581701191115310411557738383"},
                {SendMessageEnum.Rev, "2638327"},
                {SendMessageEnum.SrpT, "1477219353"}
            };
           
            var parametersUnread = new Dictionary<GetUnreadMessagesEnum, string>
            {
                {GetUnreadMessagesEnum.Client, "web_messenger"},
                {GetUnreadMessagesEnum.InboxOffset, "0"},
                {GetUnreadMessagesEnum.InboxFilter, "unread"},
                {GetUnreadMessagesEnum.InboxLimit, "1000"},
                {GetUnreadMessagesEnum.User, ""},
                {GetUnreadMessagesEnum.A, "1"},
                {GetUnreadMessagesEnum.Be, "-1"},
                {GetUnreadMessagesEnum.Pc, "PHASED:DEFAULT"},
                {GetUnreadMessagesEnum.FbDtsg, ""}
            };

            var parametersChangeStatus = new Dictionary<ChangeStatusForMesagesEnum, string>
            {
                {ChangeStatusForMesagesEnum.Ids, "true"},
                {ChangeStatusForMesagesEnum.WatermarkTimestamp, ""},
                {ChangeStatusForMesagesEnum.TitanOriginatedThreadId, ""},
                {ChangeStatusForMesagesEnum.ShouldSendReadReceipt, "true"},
                {ChangeStatusForMesagesEnum.CommerceLastMessageType, "non_ad"},
                {ChangeStatusForMesagesEnum.User, ""},
                {ChangeStatusForMesagesEnum.A, "1"},
                {ChangeStatusForMesagesEnum.Dyn, ""},
                {ChangeStatusForMesagesEnum.Af, "o"},
                {ChangeStatusForMesagesEnum.Req, "2g"},
                {ChangeStatusForMesagesEnum.Be, "-1"},
                {ChangeStatusForMesagesEnum.Pc, "PHASED:DEFAULT"},
                {ChangeStatusForMesagesEnum.Rev, "2679734"},
                {ChangeStatusForMesagesEnum.FbDtsg, ""}
            };

            /*var parametersCorrespondence = new Dictionary<GetCorrespondenceEnum, string>
            {
                {GetCorrespondenceEnum.User, ""},
                {GetCorrespondenceEnum.A, "1"},
                {GetCorrespondenceEnum.Dyn, "7AmajEzUGByAZ112u6aEyx91qeCwKAKGhVoyfirWo8popyUW3F6wAxu13wFG2K48jyR88y8aGjzEgDKuEjKeCwxxaagpwGDwPKq4GCzEkxvDAzUO5u5o5S9ADBy8K48hxGbwYDx2r_gqQ59ovwAV8G4oWfx2m"},
                {GetCorrespondenceEnum.Af, "o"},
                {GetCorrespondenceEnum.Req, "11"},
                {GetCorrespondenceEnum.Be, "-1"},
                {GetCorrespondenceEnum.Pc, "PHASED:DEFAULT"},
                {GetCorrespondenceEnum.FbDtsg, ""},
                {GetCorrespondenceEnum.Ttstamp, "265817071514881107105122895158658172108984971659910282121"},
                {GetCorrespondenceEnum.Rev, "2665999"},
                {GetCorrespondenceEnum.SrpT, ""}
            };

            
            var parametersFriends = new Dictionary<GetFriendsEnum, string>
            {
                {GetFriendsEnum.Id, ""},
                {GetFriendsEnum.Sk, "friends"}
            };
            
            

            var getFriendsByCriteriesParameters = new Dictionary<GetFriendsByCriteriesEnum, string>
            {
                {GetFriendsByCriteriesEnum.FbDtsg, ""},
                {GetFriendsByCriteriesEnum.FriendBrowserIdZero, ""},
                //{GetFriendsByCriteriesEnum.FriendBrowserId1, "100005851170388"},
                //{GetFriendsByCriteriesEnum.FriendBrowserId2, "100002032854809"},
                //{GetFriendsByCriteriesEnum.FriendBrowserId3, "100006615665493"},
                //{GetFriendsByCriteriesEnum.FriendBrowserId4, "100006478482694"},
                //{GetFriendsByCriteriesEnum.FriendBrowserId5, "100000636217168"},
                //{GetFriendsByCriteriesEnum.FriendBrowserId6, "758966725"},
                //{GetFriendsByCriteriesEnum.FriendBrowserId7, "10913044"},
                //{GetFriendsByCriteriesEnum.FriendBrowserId8, "45008220"},
                //{GetFriendsByCriteriesEnum.ExtraData, "AQKHL0W2UkdZ1NcEahRGFfjYQA47D-zqXURmRjclfcc_ue8ZmufARg6I24y0_TcRd4BRlzo4-UWxdlPNxvEzXCI0aDdjr33sopHzlzjjE5ILx60797TvS5cv-13-o2Mc9pDhQrIJ-96838oOsUETNrnlI2kGt9yCAFpCF4N29Tnhw8gyWsynxKqCytJ-grsfGsTpbfQPODRhxph5QxBN7TkyDAIi0bB5Q2OR7UffWIEQ9gVDU8fFunkZqsxe8CNhJTW_SdJH2a2sBw8m-uUid-wKAF2NOUodRPbA1aWzI_JyzEHz0Rcwm1cCSTXDe6e0ahANh_VP6dx5y4E-FRb5UmTgrygPaZ0MsMuOSJ_IrMjy7eQFJSqToTdfZ_aGj-D89Gz1CE35uG5LKXaHbl14ST0xcbQ5EQ4oDXxXknGiTbJtp-tPXvSmBdAJgD-vKMyyNAmrKZDsAgFGa1UpYNoLcoFqkIctXGt10cVOTnIV7JxQ0pMpS7wbgC59ZWdLRLbhPwuZSNCx-qKoiSGG4H0bpOOd0HGYvzY5o3hFZ6HZr-1HewaiS-zm18k674K75QKHnPfmARFb8ifFRpQq60ZLtOUAO8wYtqou5TDnos9rGX_HcOxcXFYJUL00Ej-SgFXke_U9PKN4-Yh1Isc-OcMKr4gznqNYMez7pPoeynsUZmng8n-zkhFMdeepYZhx8borrBuwEZwyV1iyz8LF_6OGyK2ISdZcO7-9gdAML6Tfz-XZHTWHqJBuMtyj4tGhkeRVt3g"},
                //{GetFriendsByCriteriesEnum.HowFound, "requests_page_pymk"},
                {GetFriendsByCriteriesEnum.Page, "friend_browser_list"},
                {GetFriendsByCriteriesEnum.InstanceName, "friend-browser"},
                {GetFriendsByCriteriesEnum.BigPics, "1"},
                {GetFriendsByCriteriesEnum.SocialContext, "1"},
                {GetFriendsByCriteriesEnum.NetworkContext, "1"},
                {GetFriendsByCriteriesEnum.NameIdsZero, ""},
                {GetFriendsByCriteriesEnum.NameInput, ""},
                {GetFriendsByCriteriesEnum.HomeTownIdsZero, ""},
                {GetFriendsByCriteriesEnum.CityIdsZero, ""},
                {GetFriendsByCriteriesEnum.GradschoolIdsZero, ""},
                {GetFriendsByCriteriesEnum.UsedTypeahead, "true"},
                {GetFriendsByCriteriesEnum.User, ""},
                //{GetFriendsByCriteriesEnum.Dyn, "aihoFeyfyGmagngDxyG9giolzFEbFbGA8AyedirWo8popyUWdwIhE98nwgUy22EaUgDyUJi28y4EnFeex2uVWxeUW2fG4GDg4bDBxe6rCxaLGqu58nVV8-cxnxm1iyECQum8yUgx66EK3O69LZ1uJ12VovG6GiV8FoWezu5EG9z8CqnCxeEgAw"},
                {GetFriendsByCriteriesEnum.A, "1"},
                {GetFriendsByCriteriesEnum.Af, "i0"},
                {GetFriendsByCriteriesEnum.Req, "2u"},
                {GetFriendsByCriteriesEnum.Be, "-1"},
                {GetFriendsByCriteriesEnum.Pc, "PHASED:DEFAULT"},
                {GetFriendsByCriteriesEnum.Rev, "2739930"},
                {GetFriendsByCriteriesEnum.Ttstamp, "26581704884105717899984911458658169529552102116122815199"}
            };

            var addFriendParameters = new Dictionary<AddFriendEnum, string>
            {
                {AddFriendEnum.FbDtsg, ""},
                {AddFriendEnum.ToFriend, ""},
                {AddFriendEnum.Action, "add_friend"},
                {AddFriendEnum.HowFound, "requests_page_pymk"},
                {AddFriendEnum.RefParam, "none"},
                {AddFriendEnum.OutgoingId, ""},
                {AddFriendEnum.LoggingLocation, "friends_center"},
                {AddFriendEnum.NoFlyoutOnClick, "true"},
                {AddFriendEnum.Floc, ""},
                //{AddFriendEnum.Frefs0, "friends_center"},
                //{AddFriendEnum.Frefs1, "ft"},
                {AddFriendEnum.User, ""},
                {AddFriendEnum.A, "1"},
                {AddFriendEnum.Af, "i0"},
                {AddFriendEnum.Req, "19"},
                {AddFriendEnum.Be, "-1"},
                {AddFriendEnum.Pc, "PHASED:DEFAULT"},
                {AddFriendEnum.Rev, "2748071"},
                {AddFriendEnum.EgoLogData, ""},
                {AddFriendEnum.HttpReferer, ""},
                {AddFriendEnum.Dyn, "7AmajEzUGByA5Q9UoGya4A5EWq2WiWF298yfirWo8popyUWdwIhE98nwgUaqwHx24UJi28y4EnFeex2uVWxeUW2y4GDg4bDBxe6rCxaLGqu58nVV8-cxnxm1iyECQum8yUgx66EK3O69L-6Z1im7WAxxbAyBzEWdxyayoO9CBQm4Wx2ii"},
                {AddFriendEnum.Ttstamp, "2658171121121110575211998828658658170788951705276987953"}
            };

            var addFriendExtraParameters = new Dictionary<AddFriendExtraEnum, string>
            {
                {AddFriendExtraEnum.User, ""},
                {AddFriendExtraEnum.A, "1"},
                {AddFriendExtraEnum.Af, "i0"},
                {AddFriendExtraEnum.Req, "19"},
                {AddFriendExtraEnum.Be, "-1"},
                {AddFriendExtraEnum.Pc, "PHASED:DEFAULT"},
                {AddFriendExtraEnum.Rev, "2748071"},
                {AddFriendExtraEnum.Dyn, "7AmajEzUGByA5Q9UoGya4A5EWq2WiWF298yfirWo8popyUWdwIhE98nwgUaqwHx24UJi28y4EnFeex2uVWxeUW2y4GDg4bDBxe6rCxaLGqu58nVV8-cxnxm1iyECQum8yUgx66EK3O69L-6Z1im7WAxxbAyBzEWdxyayoO9CBQm4Wx2ii"},
                {AddFriendExtraEnum.Ttstamp, "2658171121121110575211998828658658170788951705276987953"},
                {AddFriendExtraEnum.FbDtsg, ""},
            };
            var js = new JavaScriptSerializer();
            var jsonAddFriend = js.Serialize(addFriendParameters.Select(pair => pair).ToList());
            var jsonGetFriendsExtra= js.Serialize(addFriendExtraParameters.Select(pair => pair).ToList());

            var js = new JavaScriptSerializer();
            var jsonSendMessage = js.Serialize(sendMessageParameters.Select(pair => pair).ToList());
            var jsonUnread = js.Serialize(parametersUnread.Select(pair => pair).ToList());
            var jsonChangeStatus = js.Serialize(parametersChangeStatus.Select(pair => pair).ToList());
            var jsonFriends = js.Serialize(parametersFriends.Select(pair => pair).ToList());
            

            var winkParameters = new Dictionary<WinkEnum, string>
            {
                {WinkEnum.PokeTarget, ""},
                {WinkEnum.Nctr, "pagelet_timeline_profile_actions"},
                {WinkEnum.AsyncDialog, "1"},
                {WinkEnum.User, ""},
                {WinkEnum.A, "1"},
                {WinkEnum.Af, "i0"},
                {WinkEnum.Req, "19"},
                {WinkEnum.Be, "-1"},
                {WinkEnum.Pc, "PHASED:DEFAULT"},
                {WinkEnum.Rev, "2748071"},
                {WinkEnum.Dyn, "aihoFeyfyGmagngDxyG9giolzFEbFbGA8AyedirWo8popyUWdwIhE98nwgUy22EaUgDyUJi28y4EnFeex2uVWxeUW2fG4GDg4bDBxe6rCxaLGqu58nVV8-cxnxm1iyECQum8yUgx66EK3O69LZ1uJ12VovGi5qiV8FoWezooyECcypFt5xeEgAAw"},
                {WinkEnum.Ttstamp, "265817199789774539555113995865817111410777122119456774112"},
                {WinkEnum.FbDtsg, ""},
            };
            
            var confirmFriendshipParameters = new Dictionary<ConfirmFriendshipEnum, string>
            {
                {ConfirmFriendshipEnum.Action, "confirm"},
                {ConfirmFriendshipEnum.Id, ""},
                {ConfirmFriendshipEnum.Ref, "/reqs.php"},
                {ConfirmFriendshipEnum.User, ""},
                {ConfirmFriendshipEnum.A, "1"},
                {ConfirmFriendshipEnum.Af, "i0"},
                {ConfirmFriendshipEnum.Req, "12"},
                {ConfirmFriendshipEnum.Be, "-1"},
                {ConfirmFriendshipEnum.Pc, "PHASED:DEFAULT"},
                {ConfirmFriendshipEnum.Rev, "2748123"},
                {ConfirmFriendshipEnum.Dyn, "aihoFeyfyGmagngDxyG9giolzkqbxqbAKGiBAyedirWo8popyui9zob4q2i5U4e8wwG2KfhUKbkwy8xa5WjzEgVrDG4XzE8-EiGt0gKum4UpKq4G-FFUkgmVV8-cxnxm3i7oG9J7By8K48hxGbwYxyr_gnHggKm7WAxmAKiambGezooyECcyqKnhojG4998"},
                {ConfirmFriendshipEnum.Ttstamp, "2658172851181101199855991021085865816911210076106681001028580"},
                {ConfirmFriendshipEnum.FbDtsg, ""},
                {ConfirmFriendshipEnum.Floc, "friend_center_requests"},
                {ConfirmFriendshipEnum.Frefs0, "ft"},
                {ConfirmFriendshipEnum.ViewerId, ""},
            };

            var js = new JavaScriptSerializer();
            var jsonWink = js.Serialize(confirmFriendshipParameters.Select(pair => pair).ToList());

            var urlParametersList = new List<UrlParametersDbModel>()
            {
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.ConfirmFriendship,
                    ParametersSet = jsonWink
                },/*
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.AddFriendExtra,
                    ParametersSet = jsonGetFriendsExtra
                },
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.SendMessage,
                    ParametersSet = jsonSendMessage
                },
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.GetUnreadMessages,
                    ParametersSet = jsonUnread
                },
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.ChangeMessageStatus,
                    ParametersSet = jsonChangeStatus
                },
                new UrlParametersDbModel
                {
                    CodeParameters = (int)NamesUrlParameter.GetFriends,
                    ParametersSet = jsonFriends
                },
            };

            context.UrlParameters.AddRange(urlParametersList);
             
            context.SaveChanges(); 
            */
        }
    }
}
