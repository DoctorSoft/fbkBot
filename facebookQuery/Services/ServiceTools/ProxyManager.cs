using System;
using System.Text.RegularExpressions;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class ProxyManager : IProxyManager
    {
        public long GetAccountFacebookId(string proxy)
        {
            var parentPattern = new Regex("c_user=.*?;");
            var idData = parentPattern.Match(proxy).ToString();
            var step1 = idData.Remove(0, 7);
            var id = Convert.ToInt64(step1.Remove(step1.Length - 1));

            return id;
        }
    }
}
