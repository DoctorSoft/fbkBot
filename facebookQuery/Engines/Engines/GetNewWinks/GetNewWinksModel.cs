using System.Net;

namespace Engines.Engines.GetNewWinks
{
    public class GetNewWinksModel
    {
        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }
        public string UserAgent { get; set; }
    }
}
