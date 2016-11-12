using System.Collections.Generic;
using Constants;

namespace Engines.Engines.GetFriendsEngine
{
    public class GetFriendsModel
    {
        public long AccountId { get; set; }

        public string Cookie { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }
    }
}
