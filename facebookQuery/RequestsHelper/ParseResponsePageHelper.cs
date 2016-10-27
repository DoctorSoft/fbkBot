using System.Linq;
using System.Text.RegularExpressions;

namespace RequestsHelpers
{
    public static class ParseResponsePageHelper
    {
        public static string GetSpanValueById(string pageRequest, string spanId)
        {
            var regex = new Regex("id=\"" + spanId + "\"*>(.*?)</span>");
            if (!regex.IsMatch(pageRequest)) return null;
            var collection = regex.Matches(pageRequest);
            return (from Match m in collection select m.Groups[1].Value).FirstOrDefault();
        }
    }
}
