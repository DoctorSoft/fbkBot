using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetNewCookiesEngine;
using Engines.Engines.GetNewNoticesEngine;
using Engines.Engines.SendMessageEngine;
using RequestsHelpers;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class HomeService
    {
        public List<AccountViewModel> GetAccounts()
        {
            var accounts = new GetAccountsQueryHandler(new DataBaseContext()).Handle(new GetAccountsQuery
            {
                Count = 10,
                Page = 0
            });

            return accounts.Select(model => new AccountViewModel
            {
                Id = model.Id,
                PageUrl = model.PageUrl,
                Login = model.Login,
                Password = model.Password,
                UserId = model.UserId
            }).ToList();
        }


        public bool RefreshCookies(long accountId, string login, string password)
        {
            var newCookie = new GetNewCookiesEngine().Execute(new GetNewCookiesModel()
            {
                Login = login,
                Password = password
            }).CookiesString;

            new UpdateCookiesHandler(new DataBaseContext()).Handle(new UpdateCookiesCommand()
            {
                AccountId = accountId,
                NewCookieString = newCookie
            });

            return true;
        }

        public GetNewNoticesResponseModel GetNewNotices(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });
            var statusModel = new GetNewNoticesEngine().Execute(new GetNewNoticesModel()
            {
                ResponsePage = RequestsHelper.Get(Urls.HomePage.GetDiscription(), account.Cookie.CookieString) 
            });

            return statusModel;
        }

        public AccountActionModel GetAccountByUserId(long userId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = userId
            });

            return new AccountActionModel
            {
                Id = account.Id,
                Login = account.Login,
                Password = account.Password,
                UserId = account.UserId,
                PageUrl = account.PageUrl
            };
        }

        public void SendMessage(long senderId, long friendId)
        {
            var rnd = new Random();
            var mockMessageList = new List<string>()
            {
                "Hello", "Hi", "How are u?", "What's up?", "What are you up to?", "Nice to meet you" 
            };
            var messageText = mockMessageList[rnd.Next(mockMessageList.Count)];

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = senderId
            });

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.SendMessage
            }); 

            new SendMessageEngine().Execute(new SendMessageModel
            {
                AccountId = account.UserId,
                Cookie = account.Cookie.CookieString,
                FriendId = friendId,
                Message = messageText,
                UrlParameters = urlParameters
            });
        }
    }
}


