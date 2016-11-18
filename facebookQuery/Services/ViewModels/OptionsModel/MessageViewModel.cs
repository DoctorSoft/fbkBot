using System;
using Constants.MessageEnums;

namespace Services.ViewModels.OptionsModel
{
    public class MessageViewModel
    {
        public string Message { get; set; }

        public long? AccountId { get; set; }

        public long? GroupId { get; set; }

        public bool IsBotFirst { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int OrderNumber { get; set; }

        public bool IsEmergencyText { get; set; }

        public long ImportancyFactor { get; set; }
    }
}
