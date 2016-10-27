using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Engines.Engines.GetAccountStatusEngine
{
    public class GetAccountStatusEngine : AbstractEngine<GetAccountStatusModel, GetAccountStatusResponseModel>
    {
        protected override GetAccountStatusResponseModel ExecuteEngine(GetAccountStatusModel model)
        {
            var newFriends = GetAttrsFromSource(model.ResponsePage, "requestsCountValue");
            var newMessages = GetAttrsFromSource(model.ResponsePage, "mercurymessagesCountValue");
            var newNotices = GetAttrsFromSource(model.ResponsePage, "notificationsCountValue");

            return new GetAccountStatusResponseModel()
            {
                NumberNewFriends = newFriends,
                NumberNewMessages = newMessages,
                NumberNewNotifications = newNotices
            };
        }

        public static string GetAttrsFromSource(string pageRequest, string spanId)
        {
            var regex = new Regex("id=\"" + spanId + "\"*>(.*?)</span>");
            if (!regex.IsMatch(pageRequest)) return null;
            var collection = regex.Matches(pageRequest);
            return (from Match m in collection select m.Groups[1].Value).FirstOrDefault();
        }
    }
}
