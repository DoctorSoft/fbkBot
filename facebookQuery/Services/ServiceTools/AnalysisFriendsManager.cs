using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.Friends.GetAnalisysFriendsByStatus;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.CancelFriendshipRequestEngine;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class AnalysisFriendsManager : IAnalysisFriendsManager
    {
        private readonly IAccountManager _accountManager;
        public AnalysisFriendsManager()
        {
            _accountManager = new AccountManager();
        }
        public List<AnalysisFriendData> CheckForAnyInDataBase(AccountModel account, List<AnalysisFriendData> friends)
        {
            var friendsInDb = new GetAllFriendByQueryHandler(new DataBaseContext()).Handle(new GetAllFriendByQuery());
            var analisysFriendsInDb = new GetAnalisysFriendsByStatusQueryHandler(new DataBaseContext()).Handle(new GetAnalisysFriendsByStatusQuery
            {
                Status = StatusesFriend.ToAdd
            });

            var refreshFriendList = new List<AnalysisFriendData>();

            foreach (var analysisFriendData in friends)
            {
                if (friendsInDb.Any(data => data.FacebookId == analysisFriendData.FacebookId) != true && (analisysFriendsInDb.Any(data => data.FacebookId != analysisFriendData.FacebookId)) != true)
                {
                    refreshFriendList.Add(analysisFriendData);
                    continue;
                }
                if (analysisFriendData.Type == FriendTypes.Incoming)
                {
                    new CancelFriendshipRequestEngine().Execute(new CancelFriendshipRequestModel
                    {
                        Cookie = account.Cookie.CookieString,
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
