namespace Services.ViewModels.OptionsModel.JsonModels
{
    public class MessageOptionsModel
    {
        public long RetryTimeSendUnread { get; set; }

        public long RetryTimeSendUnanswered { get; set; }

        public long RetryTimeSendNewFriend { get; set; }

        public long UnansweredDelay { get; set; }
    }
}
