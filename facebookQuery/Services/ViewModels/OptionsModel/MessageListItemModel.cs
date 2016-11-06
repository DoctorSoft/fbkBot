using System;

namespace Services.ViewModels.OptionsModel
{
    public class MessageListItemModel
    {
        public long Id { get; set; }

        public string Message { get; set; }
        
        public bool IsBotFirst { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int OrderNumber { get; set; }

        public bool IsEmergencyText { get; set; }

        public long ImportancyFactor { get; set; }
    }
}
