using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.Friends.GetAnalisysFriendsByStatus;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.CancelFriendshipRequestEngine;
using Services.Interfaces.Notices;
using Services.Interfaces.ServiceTools;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Services.ServiceTools
{
    public class AnalysisFriendsManager : IAnalysisFriendsManager
    {
        private readonly IAccountManager _accountManager;
        private readonly NoticeService _noticesService;

        public AnalysisFriendsManager()
        {
            _accountManager = new AccountManager();
            _noticesService = new NoticeService();
        }
        public List<AnalysisFriendData> CheckForAnyInDataBase(AccountViewModel account, List<AnalysisFriendData> friends, INotices notices, string functionName)
        {
            var friendsInDb = new GetAllFriendByQueryHandler(new DataBaseContext()).Handle(new GetAllFriendByQuery());
            var analisysFriendsInDb = new GetAnalisysFriendsByStatusQueryHandler(new DataBaseContext()).Handle(new GetAnalisysFriendsByStatusQuery
            {
                Status = StatusesFriend.ToAdd
            });

            var refreshFriendList = new List<AnalysisFriendData>();
            notices.AddNotice(account.Id, _noticesService.ConvertNoticeText(functionName, string.Format("Друзей для проверки - {0}", friends.Count)));

            foreach (var analysisFriendData in friends)
            {
                var includeInFriends = friendsInDb.Any(data => data.FacebookId == analysisFriendData.FacebookId);
                var includeInAnalyseFriends = analisysFriendsInDb.Any(data => data.FacebookId == analysisFriendData.FacebookId);

                if (!includeInFriends)// && !includeInAnalyseFriends)
                {
                    refreshFriendList.Add(analysisFriendData);
                    continue;
                }
                if (analysisFriendData.Type == FriendTypes.Incoming)
                {
                    notices.AddNotice(account.Id, _noticesService.ConvertNoticeText(functionName, string.Format("С ним мы общались. Отменяем заявку- {0}({1})", analysisFriendData.FriendName, analysisFriendData.FacebookId)));

                    new CancelFriendshipRequestEngine().Execute(new CancelFriendshipRequestModel
                    {
                        Cookie = account.Cookie,
                        Proxy = _accountManager.GetAccountProxy(account),
                        FriendFacebookId = analysisFriendData.FacebookId,
                        AccountFacebookId = account.FacebookId,
                        UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.CancelRequestFriendship
                        })
                    });
                }
            }

            return refreshFriendList;
        }
    }
}
