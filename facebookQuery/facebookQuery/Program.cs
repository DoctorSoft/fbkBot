using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CommonModels;
using Constants.FriendTypesEnum;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountStatistics;
using DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand;
using DataBase.QueriesAndCommands.Commands.FriendsBlackList.AddToFriendsBlackListCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWinkFriendsFriends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.AddToGroupEngine;
using Engines.Engines.AddToPageEngine;
using Engines.Engines.ConfirmFriendshipEngine;
using Engines.Engines.GetFriendInfoEngine;
using Engines.Engines.GetFriendsByCriteriesEngine;
using Engines.Engines.GetFriendsEngine.CheckFriendInfoBySeleniumEngine;
using Engines.Engines.GetFriendsEngine.GetRandomFriendFriends;
using Engines.Engines.GetFriendsEngine.GetRecommendedFriendsEngine;
using Engines.Engines.GetMessagesEngine.GetUnreadMessages;
using Engines.Engines.GetNewCookiesEngine;
using Engines.Engines.GetNewWinks;
using Engines.Engines.JoinTheGroupsAndPagesEngine.JoinThePagesBySeleniumEngine;
using Engines.Engines.WinkEngine;
using Jobs.Jobs.FriendJobs;
using Jobs.JobsService;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using RequestsHelpers;
using Services.Interfaces.ServiceTools;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;
using Services.ViewModels.HomeModels;
using Cookie = OpenQA.Selenium.Cookie;

namespace FacebookApp
{
    internal class Program
    {
        private static IAccountSettingsManager _accountSettingsManager;
        private static IAccountManager _accountManager;
        private static ISeleniumManager _selenium = new SeleniumManager();

 
        private static void Main(string[] args)
        {

            var homeService = new HomeService(new JobService(), new BackgroundJobService());

            var accounts = homeService.GetAccounts();

            _accountSettingsManager = new AccountSettingsManager();
            _accountManager = new AccountManager();

            /*new AddToFriendsBlackListCommandHandler(new DataBaseContext()).Handle(new AddToFriendsBlackListCommand
            {
                GroupSettingsId = 3,
                FriendName = "tesd",
                FriendFacebookId = 23232323
            });

            */

            foreach (var accountViewModel in accounts)
            {
                if (!accountViewModel.IsDeleted)
                {
                    if (accountViewModel.Id == 24)
                    {
                        new FriendsService(new NoticeService()).CheckFriendsAtTheEndTimeConditions(accountViewModel);
                        //new FriendsService(new NoticeService()).GetCurrentFriends(accountViewModel);


                        break;
                    }
                    /*new FacebookMessagesService(new NoticeService()).GetUnreadMessages(new AccountModel()
                    {
                        Proxy = accountViewModel.Proxy,
                        ProxyLogin = accountViewModel.ProxyLogin,
                        ProxyPassword = accountViewModel.ProxyPassword,
                        Cookie = new CookieModel
                        {
                            CookieString = accountViewModel.Cookie
                        },
                        FacebookId = accountViewModel.FacebookId,
                        Id = accountViewModel.Id,
                        
                    });

                    /*new JoinThePagesBySeleniumEngine().Execute(new JoinThePagesBySeleniumModel
                    {
                        Driver = seleniumManager.RegisterNewDriver(accountViewModel),
                        Cookie = accountViewModel.Cookie,
                        Pages = new List<string>
                        {
                            "https://www.facebook.com/belbeercom/",
                            "https://www.facebook.com/etradeconf/"
                        }
                    });

                    /*
                    new AddToPageEngine().Execute(new AddToPageModel
                    {
                        Cookie = accountViewModel.Cookie,
                        Proxy = _accountManager.GetAccountProxy(new AccountModel()
                        {
                            Proxy = accountViewModel.Proxy,
                            ProxyLogin = accountViewModel.ProxyLogin,
                            ProxyPassword = accountViewModel.ProxyPassword
                        }),
                        FacebookId = accountViewModel.FacebookId,
                        FacebookPageUrl = "https://www.facebook.com/hc.neman/",
                        Friend = 
                        new FriendModel
                            {
                                FacebookId = 100015663996105,
                                FriendName = "Loly"
                        },
                        UrlParameters =
                            new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                            {
                                NameUrlParameter = NamesUrlParameter.AddFriendsToPage
                            }),
                    });
                

                /* var account =
                        new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
                        {
                            FacebookUserId = accountViewModel.FacebookId
                        });

                    var driver = new ChromeDriver();

                    driver.Navigate().GoToUrl("https://www.facebook.com/");
                    var currCookies = driver.Manage().Cookies.AllCookies;
                    var path = "/";
                    var domain = ".facebook.com";

                    var cookies = ParseCookieString(account.Cookie.CookieString);

                    foreach (var keyValuePair in cookies)
                    {
                        driver.Manage()
                            .Cookies.AddCookie(new Cookie(keyValuePair.Key, keyValuePair.Value, domain, path, null));
                    }

                    driver.Navigate()
                        .GoToUrl(
                            string.Format(
                                "https://www.facebook.com/profile.php?id=100008195580815&lst={0}%3A100008195580815%3A1485883089&sk=about",
                                account.FacebookId));

                    Thread.Sleep(500);

                    var divs = driver.FindElements(By.CssSelector(".uiList._1pi3._4kg._6-h._703._4ks>li"));
                    var links = driver.FindElements(By.CssSelector("._c24._50f4>.profileLink"));

                    var action = new Actions(driver);

                    _userInfo = new UserInfo();
                    int i = 0;
                    foreach (var webElement in divs)
                    {
                        switch (i)
                        {
                            case 0:
                            {
                                if (webElement.GetCssValue("Class") != null && webElement.GetCssValue("Id") == null)
                                {
                                    _userInfo.Work = null;
                                }
                                else
                                {
                                    action.MoveToElement(links[0]);
                                    Thread.Sleep(3000);

                                    action.Perform();
                                    Thread.Sleep(2000);

                                    var t1 = new Task(() => GetCity(driver, i));

                                    t1.Start();
                                }
                                break;
                            }
                            case 1:
                            {
                                if (webElement.GetCssValue("Class") != null && webElement.GetCssValue("Id") == null)
                                {
                                    _userInfo.Study = null;
                                }
                                else
                                {
                                    action.MoveToElement(links[1]);
                                    Thread.Sleep(3000);

                                    action.Perform();
                                    Thread.Sleep(2000);

                                    var t1 = new Task(() => GetCity(driver, i));

                                    t1.Start();
                                }
                                break;
                            }
                            case 3:
                            {
                                if (webElement.GetCssValue("Class") != null && webElement.GetCssValue("Id") == null)
                                {
                                    _userInfo.Live = null;
                                }
                                else
                                {
                                    action.MoveToElement(links[1]);
                                    Thread.Sleep(3000);

                                    action.Perform();
                                    Thread.Sleep(2000);

                                    var t1 = new Task(() => GetCity(driver, i));

                                    t1.Start();
                                }
                                break;
                            }
                        }
                        i++;
                    }


                    foreach (var webElement in links)
                    {
                        action.MoveToElement(webElement);
                        Thread.Sleep(3000);

                        action.Perform();
                        Thread.Sleep(2000);


                        var city = "";
                        //var t1 = new Task(() => GetCity(driver));

                        //t1.Start();

                        //Thread.Sleep(3000);
                    }

                    /*
                    var friends = GetFriendLinks(driver);
                    var currentCount = friends.Count;

                    while (true)
                    {
                        ScrollPage(driver);
                        friends = GetFriendLinks(driver);
                        if (friends.Count > currentCount)
                        {
                            currentCount = friends.Count;
                        }
                        else
                        {
                            break;
                        }
                    }

                    friends = GetFriendLinks(driver);

                    foreach (var webElement in friends)
                    {
                        var name = webElement.Text;
                        var id = ParseFacebookId(webElement.GetAttribute("data-gt"));
                    }
            */
//homeService.RefreshCookies(accountViewModel);

//                    spyService.AnalyzeFriends(accountViewModel);


//                    var friendList = new GetRecommendedFriendsEngine().Execute(new GetRecommendedFriendsModel()
//                    {
//                        Cookie = account.Cookie.CookieString,
//                        Proxy = _accountManager.GetAccountProxy(account)
//                    });
//                    new SaveFriendsForAnalysisCommandHandler(new DataBaseContext()).Handle(new SaveFriendsForAnalysisCommand
//                    {
//                        AccountId = account.Id,
//                        Friends = friendList.Select(model => new AnalysisFriendData
//                        {
//                            AccountId = account.Id,
//                            FacebookId = model.FacebookId,
//                            Type = model.Type,
//                            Status = StatusesFriend.ToAnalys,
//                            FriendName = model.FriendName
//                        }).ToList()
//                    });

//                     var proxy = new WebProxy(accountViewModel.Proxy)
//                        {
//                            Credentials =
//                                new NetworkCredential(accountViewModel.ProxyLogin, accountViewModel.ProxyPassword)
//                        };

//RequestsHelper.Get("https://www.facebook.com/friends/requests/?fcref=jwl", account.Cookie.CookieString, proxy);

//new HomeService(null, null).RefreshCookies(accountViewModel);

//homeService.RefreshCookies(accountViewModel);
//

//                    new ConfirmFriendshipEngine().Execute(new ConfirmFriendshipModel()
//                    {
//                        AccountFacebookId = account.FacebookId,
//                        FriendFacebookId = 100014431878138,
//                        Proxy = _accountManager.GetAccountProxy(account),
//                        Cookie = account.Cookie.CookieString,
//                        UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
//                        {
//                            NameUrlParameter = NamesUrlParameter.ConfirmFriendship
//                        }),
//                    });

//                    new WinkEngine().Execute(new WinkModel
//                    {
//                            AccountFacebookId = account.FacebookId,
//                            FriendFacebookId = 100005708075966,
//                            Proxy = _accountManager.GetAccountProxy(account),
//                            Cookie = account.Cookie.CookieString,
//                            UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
//                            {
//                                NameUrlParameter = NamesUrlParameter.Wink
//                            }),
//                    });

//                        new AddFriendEngine().Execute(new AddFriendModel()
//                        {
//                            AccountFacebookId = account.FacebookId,
//                            FriendFacebookId = 100011608590882,
//                            Proxy = _accountManager.GetAccountProxy(account),
//                            Cookie = account.Cookie.CookieString,
//                            AddFriendExtraUrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
//                            {
//                                NameUrlParameter = NamesUrlParameter.AddFriendExtra
//                            }),
//                            AddFriendUrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
//                            {
//                                NameUrlParameter = NamesUrlParameter.AddFriend
//                            })
//                        });
////                        new GetFriendsByCriteriesEngine().Execute(new GetFriendsByCriteriesModel
//                        {
//                            AccountId = account.FacebookId,
//                            Proxy = _accountManager.GetAccountProxy(account),
//                            Cookie = account.Cookie.CookieString,
//                            UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
//                            {
//                                NameUrlParameter = NamesUrlParameter.GetFriendsByCriteries
//                            })
//                        });

//RequestsHelper.Get("https://www.2ip.ru", "", proxy);
/*var driver = homeService.RegisterNewDriver(accountViewModel);
driver.Navigate().GoToUrl("https://2ip.ru/");


        }
        }
 }*/
                }
            }
        }
    }
}




/*
           CheckPatternChanges();

           var driver = new ChromeDriver();

           driver.Navigate().GoToUrl("https://www.facebook.com/login.php?login_attempt=1&lwv=110");
            
           Thread.Sleep(500);

           var email = driver.FindElementById("email"); 
           Thread.Sleep(300);
           var pass = driver.FindElementById("pass"); 
           Thread.Sleep(300);
           var button = driver.FindElementById("loginbutton");

           email.SendKeys("ms.nastasia.1983@mail.ru");
           pass.SendKeys("Ntvyjnf123");
           Thread.Sleep(300);

           button.Click();

           var cookies = driver.Manage().Cookies;

           //get

           var data = new NameValueCollection();
           var userId = cookies.GetCookieNamed("c_user").Value;
           data.Add("client", "web_messenger");
           data.Add("inbox[offset]", "0");
           data.Add("inbox[limit]", "1000");
           data.Add("inbox[filter]", "unread");
           data.Add("__user", cookies.GetCookieNamed("c_user").Value);
           data.Add("__a", "1");
           data.Add("__be", "-1");
           data.Add("__pc", "PHASED:DEFAULT");

           //data.Add("fb_dtsg", fb_dtsg);
           var cookiesResult = cookies.AllCookies.Aggregate("", (current, cookie) => current + (cookie.Name + "=" + cookie.Value + ";"));

           var client = new WebClient();
            
           client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
           client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
           client.Headers[HttpRequestHeader.Accept] = "#1#*";
           client.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
           client.Headers.Add(HttpRequestHeader.Cookie, cookiesResult);
           var answer = client.DownloadString("https://www.facebook.com");
       }
       private static string ToQueryString(NameValueCollection source)
       {
           return String.Join("&", source.AllKeys.Where(m => m != null)
               .SelectMany(key => source.GetValues(key)
                   .Where(val => val != null).Select(value => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))))
               .ToArray());
       }

       private static void CheckPatternChanges()
       {
           var accountId = 4;
           var pattern = "{Hi|Hello}, my name is {$MY_NAME|John}, please follow this link {$LINK|www.a.b|www.ololo.lo}!";
           var result = new CalculateMessageTextQueryHandler(new DataBaseContext()).Handle(new CalculateMessageTextQuery
           {
               AccountId = accountId,
               TextPattern = pattern
           });
       }*/
        























//
//
//
//
//
//
//
//
//        string fr = null;
//            string result;
//
//            var request = (HttpWebRequest) WebRequest.Create("https://www.facebook.com/");
//            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
//            request.Headers.Add("ContentType:application/x-www-form-urlencoded");
//            //request.Headers("Accept = */*";
//            request.Headers.Add("AcceptLanguage:ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
//            request.CookieContainer = new CookieContainer();
//
//            var response1 = (HttpWebResponse) request.GetResponse();
//            foreach (Cookie cook in response1.Cookies)
//            {
//                fr = cook.Value;
//            }
//
//            using (var stream = response1.GetResponseStream())
//            {
//                var reader = new StreamReader(stream, Encoding.UTF8);
//                result = reader.ReadToEnd();
//            }
//
//            const string loginUrl = "https://www.facebook.com/login.php?login_attempt=1&lwv=110";
//
//            var client = new WebClient();
//
//            var data = new NameValueCollection();
//            var lsd = GetAttrsFromSource(result, "input", "value", "lsd");
//            var persistent = GetAttrsFromSource(result, "input", "value", "persistent");
//            var default_persistent = GetAttrsFromSource(result, "input", "value", "default_persistent");
//            var timezone = GetAttrsFromSource(result, "input", "value", "timezone");
//            var lgndim = GetAttrsFromSource(result, "input", "value", "lgndim");
//            var lgnrnd = GetAttrsFromSource(result, "input", "value", "lgnrnd");
//            var lgnjs = GetAttrsFromSource(result, "input", "value", "lgnjs");
//            var ab_test_data = GetAttrsFromSource(result, "input", "value", "ab_test_data");
//            var locale = GetAttrsFromSource(result, "input", "value", "locale");
//            var next = GetAttrsFromSource(result, "input", "value", "next");
//            var qsstamp = GetAttrsFromSource(result, "input", "value", "qsstamp");
//
//            var js_datr = GetJsDatr(result);
//            var cookie = "datr=" + js_datr + ";" +
//             "locale=" + locale + ";" +
//             "fr=" + fr;
//
//            var bz = "https://www.facebook.com/ajax/bz";
//
//            client.Headers[HttpRequestHeader.UserAgent] =
//            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
//            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
//            client.Headers[HttpRequestHeader.Accept] = "*/*";
//            client.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
//            client.Headers.Add(HttpRequestHeader.Cookie, cookie);
//
//            var dataBz = new NameValueCollection();
//            dataBz.Add("q", "[{'user':'0','page_id':'lgyy9f','posts':[['time_spent_bit_array',{'tos_id':'lgyy9f','start_time':1477503803,'tos_array':[3,0],'tos_len':64,'tos_seq':1,'tos_cum':4},1477503867435,0]],'trigger':'time_spent_bit_array'}]");
//            var htmlResultBz = client.UploadString(bz, ToQueryString(dataBz));
//
//
//            data.Add("lsd", lsd ?? "");
//            data.Add("persistent", persistent ?? "");
//            data.Add("default_persistent", default_persistent ?? "");
//            data.Add("timezone", "-180");
//            data.Add("lgndim", lgndim ?? "");
//            data.Add("lgnrnd", lgnrnd ?? "");
//            data.Add("lgnjs", lgnjs ?? "");
//            data.Add("ab_test_data", ab_test_data ?? "");
//            data.Add("locale", locale ?? "");
//            data.Add("next", next ?? "");
//            data.Add("qsstamp", "W1tbMjksMzEsMzgsNDUsNzIsODAsODUsODksOTYsMTYzLDE5MCwyMjMsMjMxLDIzMywyMzQsMjUzLDI1NSwyNzgsMzE1LDMyOSwzMzMsMzM3LDM1MywzODAsMzgzLDQxMSw0MTIsNDc2LDQ5OSw1MDMsNTA2LDUwOCw1NDAsNTQ3LDU2MSw1NjIsNTczLDYyMyw2MzAsNjU2LDY3MSw3NzFdXSwiQVpsU2xYcWVGYm54TEhyUVVLdk95WVdZTklJWndWRkxsUFpKZ1drRHBTcmlRallVSS1mZGVNRFREZThrZ3AwNGNvT2ZUdnIzUFNqQ01lU000TmJvNVdpZ0dmZFU5d3VKNFlPWW5mcWR1Z0x4czFpOUNhckVHa1pnSVpRaWNMeFNCYmNUeGM5Z0JWeTRkVVVXM0w2QTBNYkhuTUdqT1U0bUhCSTYwTTM1X3VqZGowWGdzWWQ4TUZPWHpuSklONFNIVG5hcHVVWkF0XzY3R2doNlphaDgzWEw4WGhkVGVKdUoyQ0xFOXVhYzczQkdIUSJd");
//            data.Add("email", "ms.nastasia.1983@mail.ru");
//            data.Add("pass", "Ntvyjnf123");
//
//
//
//            client.Headers[HttpRequestHeader.UserAgent] =
//                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
//            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
//            client.Headers[HttpRequestHeader.Accept] = "*/*";
//            client.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
//            client.Headers.Add(HttpRequestHeader.Cookie, cookie);
//
//            var htmlResult1 = client.UploadString(loginUrl, ToQueryString(data));
//
//            string s = ";";
//
//
//            var rnd = new Random();
//
//            var accountId = "100013726390504";
//            var interlocutor = "100013532889680";
//            var body = "Hello!";
//            var masssageId = "6195967268237640333"; // + rnd.Next(10, 99);
//
//
//
//            var url = "https://www.facebook.com/messaging/send/?dpr=1";
//            var home = "https://www.facebook.com/profile.php";
//            var friends = "https://www.facebook.com/friends/requests/log_impressions?dpr=1";
//
//            var parameters =
//                "client=mercury" +
//                "&action_type=ma-type:user-generated-message" +
//                "&body=" + body +
//                "&ephemeral_ttl_mode=0" +
//                //"&force_sms=true" +
//                "&has_attachment=false" +
//                "&message_id=" + masssageId +
//                "&offline_threading_id=" + masssageId +
//                "&other_user_fbid=" + interlocutor +
//                "&source=source:titan:web" +
//                "&signature_id=6ec32383" +
//                "&specific_to_list[0]=fbid:" + interlocutor +
//                "&specific_to_list[1]=fbid:" + accountId +
//                "&timestamp=1477228400458" + //+
//                "&ui_push_phase=V3" +
//
//                "&__user=" + accountId +
//                "&__a=1" +
//                "&__dyn=aihoFeyfyGmagngDxyG9gigmzkqbxqbAKGiBAy8Z9LFwxBxvyui9wWhE98nwgUy22Ea-dgjyR88y8ixvrzEgVrDG4XzFE8oiGt0h9VojxCVEiGqdgkxvDAzUO5u5od8a8Cium8yUgx66EK2m5FUgC_UrQ4bBG7awxV8F3qzE" +
//                //+
//                "&__af=o" + //1
//                "&__req=96" + //x
//                "&__be=-1" +
//                "&__pc=PHASED:DEFAULT" +
//                "&fb_dtsg=AQHXI-PMhJk_:AQEgGrp3bo1j" +
//                "&ttstamp=2658169885487834570706751586581701191115310411557738383" + //+
//                "&__rev=2638327" + //+
//                "&__srp_t=1477219353"; //+
//
//            var parameters1 =
//                "client=mercury" +
//                "&action_type=ma-type%3Auser-generated-message" +
//                "&body=2222" +
//                "&ephemeral_ttl_mode=0" +
//                "&has_attachment=false" +
//                "&message_id=6195967268237640333" +
//                "&offline_threading_id=6195967268237640333" +
//                "&other_user_fbid=100013532889680" +
//                "&signature_id=6ec32383" +
//                "&source=source%3Achat%3Aweb" +
//                "&specific_to_list[0]=fbid%3A100013532889680" +
//                "&specific_to_list[1]=fbid%3A100013726390504" +
//                "&timestamp=1477233712253" +
//                "&ui_push_phase=V3";
//
//            var sub = "&__user=100013726390504" +
//                      "&__a=1" +
//                      "&__dyn=aihoFeyfyGmagngDxyG9giolzkqbxqbAKGiBAy8Z9LFwxBxvyui9wWhE98nwgUy22Ea-dhUKbkwy8xa5ZKex3BKuEjKeCxPG4GDg4ium4UpKq4GCzk58nVV8-cxnxm3i2y9ADBy8K48hxGbwBxqu49LZ1uJ12VqxOEqCV8F3qzE" +
//                      "&__af=o" +
//                      "&__req=co" +
//                      "&__be=-1" +
//                      "&__pc=PHASED%3ADEFAULT" +
//                      "&fb_dtsg=AQEX6WS-FFC3%3AAQFwo5hs9ISS" +
//                      "&ttstamp=2658169885487834570706751586581701191115310411557738383" +
//                      "&__rev=2638327" +
//                      "&__srp_t=1477219353";
//
//            var coockies = "locale=ru_RU;" +
//                           //"av=1;" +
//                           "datr=rREdV7eqIbgh25Gal6Q9J9ZC;" +
//                           "sb=ZA1EV8pIHILc8m8MpfzyOHHA;" +
//                           "c_user=100013726390504;" +
//                           "xs=40%3AGL9vgtMygUyU1A%3A2%3A1476897375%3A-1;" +
//                           "fr=0almW4fUXZ8jobYqn.AWVYXI3zNlvFwzAWhD3xrdKYdo8.BXRA1l.uM.AAA.0.0.BYDL58.AWUZL_Pq;" + //+
//                           "csm=2;" +
//                           "s=Aa4JxX8cIBNkkbrq.BYB6pf;" +
//                           "pl=n;" +
//                           "lu=ggAF8SUUdyIdnGp-mB0GMomA;" +
//                           "p=-2;" +
//                           "act=1477233709048%2F40;" +
//                           "wd=1920x974;" +
//                           "presence=EDvF3EtimeF1477233711EuserFA21B13726390504A2EstateFDt2F_5bDiFA2user_3a1B13532889680A2ErF1C_5dElm2FA2user_3a1B13532889680A2Euct2F1477232021084EtrFnullEtwF3602623524EatF1477233710932G477233711418CEchFDp_5f1B13726390504F63CC;";
//
////            var data = new NameValueCollection();
////            data.Add("client", "web_messenger");
////            data.Add(l + "[offset]", "0");
////            data.Add(l + "[limit]", "1000");
////            data.Add(l + "[filter]", "unread");
////            data.Add("__user", accountId);
////            data.Add("__a", "1");
////            data.Add("__be", "-1");
////            data.Add("__pc", "PHASED:DEFAULT");
////            data.Add("fb_dtsg", fb_dtsg);
////            p.Post("https://www.facebook.com/ajax/mercury/threadlist_info.php?dpr=1", data);
//
//            var wb = new WebClient();
//            wb.Headers.Add(HttpRequestHeader.Cookie, coockies);
//            wb.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
//            wb.Headers[HttpRequestHeader.Accept] = "*/*";
//            //wb.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
//            wb.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
//            //wb.Headers[HttpRequestHeader.Referer] = "https://www.facebook.com/profile.php?id=100013532889680&sk=about"; //+
//            wb.Headers[HttpRequestHeader.UserAgent] =
//                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
//
////            var parameters =
////                "client=mercury&action_type=ma-type%3Auser-generated-message&body=2222&ephemeral_ttl_mode=0&has_attachment=false&message_id=6195967268237640741&offline_threading_id=6195967268237640741&other_user_fbid=100013532889680&signature_id=6ec32383&source=source%3Achat%3Aweb&specific_to_list[0]=fbid%3A100013532889680&specific_to_list[1]=fbid%3A100013726390504&timestamp=1477233712253&ui_push_phase=V3&__user=100013726390504&__a=1&__dyn=aihoFeyfyGmagngDxyG9giolzkqbxqbAKGiBAy8Z9LFwxBxvyui9wWhE98nwgUy22Ea-dhUKbkwy8xa5ZKex3BKuEjKeCxPG4GDg4ium4UpKq4GCzk58nVV8-cxnxm3i2y9ADBy8K48hxGbwBxqu49LZ1uJ12VqxOEqCV8F3qzE&__af=o&__req=co&__be=-1&__pc=PHASED%3ADEFAULT&fb_dtsg=AQEX6WS-FFC3%3AAQFwo5hs9ISS&ttstamp=2658169885487834570706751586581701191115310411557738383&__rev=2638327&__srp_t=1477219353";
//            var htmlResult = wb.UploadString(friends, sub);
//
//
//            /*    var path = @"answer.txt";
//            var sw = new StreamWriter(path, parameters);
//            sw.WriteLine(htmlResult);*/
//        }
//
//        public static string GetAttrsFromSource(string htmlSource, string tag, string attr, string name)
//        {
//            var regexImgSrc = @"<" + tag + @"[^>]*?" + attr + @"\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
//            var matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc,
//                RegexOptions.IgnoreCase | RegexOptions.Singleline);
//            var list = new List<string>();
//            foreach (Match m in matchesImgSrc)
//            {
//                //list.Add(m.Groups[1].Value);
//                var s1 = m.ToString();
//                if (!s1.Contains(name)) continue;
//                var a = s1.Remove(0, s1.IndexOf("value=\"", System.StringComparison.Ordinal) + 7);
//                var endIndex = a.IndexOf(" ", System.StringComparison.Ordinal);
//                var b = a.Remove(endIndex - 1, a.Length - endIndex + 1);
//
//                return b;
//            }
//            return null;
//        }
//
//        public static string GetJsDatr(string htmlSource)
//        {
//            var regexImgSrc = @"\[""_js_datr"",\""([^\""]*)";
//            var js_datr = Regex.Matches(htmlSource, regexImgSrc,
//                RegexOptions.IgnoreCase | RegexOptions.Singleline);
//            return (from Match m in js_datr select m.ToString() into s select s.Remove(0, s.IndexOf(",\"", System.StringComparison.Ordinal) + 2)).FirstOrDefault();
//        }
//
//        /* bool FacebookAuth(Parser p)
//        {
//            try
//            {
//                p.Go("https://www.facebook.com/");
//                var s = "blue_bar_profile_link";
//                if (!string.IsNullOrEmpty(p.Content) && !p.Contains(s))
//                {
//                    var data = new NameValueCollection();
//                    data.Add("lsd", p.SelectSingleNode("//input[@name='lsd']").GetAttributeValue("value", ""));
//                    data.Add("persistent", p.SelectSingleNode("//input[@name='persistent']").GetAttributeValue("value", ""));
//                    data.Add("default_persistent", p.SelectSingleNode("//input[@name='default_persistent']").GetAttributeValue("value", ""));
//                    data.Add("timezone", p.SelectSingleNode("//input[@name='timezone']").GetAttributeValue("value", ""));
//                    data.Add("lgndim", p.SelectSingleNode("//input[@name='lgndim']").GetAttributeValue("value", ""));
//                    data.Add("lgnrnd", p.SelectSingleNode("//input[@name='lgnrnd']").GetAttributeValue("value", ""));
//                    data.Add("lgnjs", p.SelectSingleNode("//input[@name='lgnjs']").GetAttributeValue("value", ""));
//                    data.Add("ab_test_data", p.SelectSingleNode("//input[@name='ab_test_data']").GetAttributeValue("value", ""));
//                    data.Add("locale", p.SelectSingleNode("//input[@name='locale']").GetAttributeValue("value", ""));
//                    data.Add("next", p.SelectSingleNode("//input[@name='next']").GetAttributeValue("value", ""));
//                    data.Add("email", "");
//                    data.Add("pass", "");
//                    p.Post("https://www.facebook.com/login.php?login_attempt=1&lwv=110", data);
//                }
//                if (p.Contains(s)) return true;
//            }
//            catch (Exception ex) {}
//            return false;*/
//    }
//}
//
//public class WebClientEx : WebClient
//{
//    private CookieContainer _cookieContainer = new CookieContainer();
//
//    protected override WebRequest GetWebRequest(Uri address)
//    {
//        WebRequest request = base.GetWebRequest(address);
//        if (request is HttpWebRequest)
//        {
//            (request as HttpWebRequest).CookieContainer = _cookieContainer;
//        }
//        return request;
//    }
//}
//
//
//
//
//
////            var parameters =
////                "&client=mercury" +
////                "&action_type=ma-type:user-generated-message" +
////                "&body=Hello" +
////                "&ephemeral_ttl_mode=0" +
////                "&force_sms=true" +
////                "&has_attachment=false" +
////                "&message_id=6194557044580485791" +
////                "&offline_threading_id=6194557044580485791" +
////                "&other_user_fbid=223500233" +
////                "&source=source:titan:web" +
////                "&signature_id=6f66bfb5" +
////                "&specific_to_list[0]=fbid:223500233" +
////                "&specific_to_list[1]=fbid:100013726390504" +
////                "&timestamp=1476818665788" +
////                "&ui_push_phase=V3" +
////                "&__user=100013726390504" +
////                "&__a=1" +
////                "&__dyn=aihoFeyfyGmagngDxyG8EigmzFEbFbGA8Ay8Z9LFwxBxCbzEeAq2i5U4e2CEaUgxebkwy8wGFeex2uVWxeUWq264EK14DBwJKq4GCzEkxvDAzUO5u5o5S9ADBy8K48hxGbwYDx2r_xLggKm7U9eiax6ew" +
////                "&__af=-1" +
////                "&__req=x" +
////                "&__be=-1" +
////                "&__pc=PHASED:DEFAULT" +
////                "&fb_dtsg=AQHXI-PMhJk_:AQEgGrp3bo1j" +
////                "&ttstamp=2658172887345807710474107955865816910371114112519811149106" +
////                "&__rev=2631735" +
////                "&__srp_t=1476897375";