using System.Dynamic;

namespace Engines.Engines.GetNewCookiesEngine
{
    public class GetNewCookiesResponse
    {
        public bool ConfirmationError { get; set; }

        public bool AuthorizationError { get; set; }

        public string CookiesString { get; set; }
    }
}
