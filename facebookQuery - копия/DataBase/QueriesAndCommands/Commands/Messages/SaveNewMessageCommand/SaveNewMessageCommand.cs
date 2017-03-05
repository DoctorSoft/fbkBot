using System;
using Constants.MessageEnums;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveNewMessageCommand
{
    public class SaveNewMessageCommand : IVoidCommand
    {
        public string Message { get; set; }

        public long? AccountId { get; set; }

        public long? GroupId { get; set; }

        public MessageRegime MessageRegime { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int OrderNumber { get; set; }

        public bool IsEmergencyText { get; set; }

        public long ImportancyFactor { get; set; }
    }
}
