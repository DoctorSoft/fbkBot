using CommonModels;

namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class MessageOptionsDbModel
    {
        public TimeModel RetryTimeSendUnread { get; set; }

        public TimeModel RetryTimeSendUnanswered { get; set; }

        public TimeModel RetryTimeSendNewFriend { get; set; }

        public int UnansweredDelay { get; set; }
    }
}
