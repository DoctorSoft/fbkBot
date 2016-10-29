using System.ComponentModel;

namespace Constants
{
    public enum Urls
    {
        [Description("https://www.facebook.com")]
        HomePage,
        [Description("https://www.facebook.com/messaging/send/?dpr=1")]
        SendMessage,
        [Description("https://www.facebook.com/ajax/mercury/threadlist_info.php?dpr=1")]
        NewMessages
    }
}
