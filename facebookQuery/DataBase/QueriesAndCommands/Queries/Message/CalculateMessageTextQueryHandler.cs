using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Message
{
    public class CalculateMessageTextQueryHandler : IQueryHandler<CalculateMessageTextQuery, string>
    {
        private readonly DataBaseContext context;

        public CalculateMessageTextQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public string Handle(CalculateMessageTextQuery query)
        {
            var text = OpenBrackets(query.TextPattern);
            text = InsertRandomLink(text);
            text = InsertAccountName(text, query.AccountId);
            text = InsertFriendName(text, query.FriendId);

            return text;
        }

        private string OpenBrackets(string pattern)
        {
            var random = new Random();

            while (pattern.Contains('{') && pattern.Contains('}'))
            {
                var innerData = pattern.Split('}').FirstOrDefault().Split('{').LastOrDefault();

                var varaiables = innerData.Split('|');

                if (!varaiables.Any())
                {
                    pattern = pattern.Replace("{}", string.Empty);
                }

                var index = random.Next(varaiables.Length);

                pattern = pattern.Replace('{' + innerData + '}', varaiables[index]);
            }

            return pattern;
        }

        private string InsertRandomLink(string pattern)
        {
            var link = context.Links.OrderBy(model => Guid.NewGuid()).FirstOrDefault();

            var result = pattern.Replace("$LINK", link != null ? link.Link : string.Empty);

            return result;
        }

        private string InsertAccountName(string pattern, long? accountId)
        {
            var account = context.Accounts.FirstOrDefault(model => model.Id == accountId);

            var result = pattern.Replace("$MY_NAME", account != null ? account.Login : string.Empty);

            return result;
        }

        private string InsertFriendName(string pattern, long? friendId)
        {
            var friend = context.Friends.FirstOrDefault(model => model.Id == friendId);

            var result = pattern.Replace("$FRIEND", friend != null ? friend.FriendName : string.Empty);

            return result;
        }
    }
}
