using System.Net;
using OpenQA.Selenium.Remote;

namespace Engines.Engines.GetMessagesEngine.GetCorrespondenceRequests
{
    public class GetCorrespondenceRequestsModel
    {
        public long AccountFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public RemoteWebDriver Driver { get; set; }
    }
}
