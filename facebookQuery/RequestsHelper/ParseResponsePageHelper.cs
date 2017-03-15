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

        public static string GetInputValueById(string pageRequest, string inputName)
        {
            var regex = new Regex("name=\"" + inputName + "\"[^>]*");
            if (!regex.IsMatch(pageRequest)) return null;
            var collection = regex.Matches(pageRequest);
            var fullString = (from Match m in collection select m.Groups[0].Value).FirstOrDefault();

            var regexForValue = new Regex("value=\"*[^\")]*");
            var values = regexForValue.Matches(fullString);
            var value = (from Match m in values select m.Groups[0].Value).FirstOrDefault();

            return value != null ? value.Remove(0, 7) : null;
        }

        public static string GetGroupId(string pageRequest)
        {
            var regexForValue = new Regex("entity_id\":\"\"*[^\")]*");
            var values = regexForValue.Matches(pageRequest);
            var value = (from Match m in values select m.Groups[0].Value).FirstOrDefault();

            return value != null ? value.Remove(0, 12) : null;
        }
    }
}
