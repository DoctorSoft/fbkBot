using System;
using CommonModels;
using Constants.FunctionEnums;

namespace Services.ViewModels.QueueViewModels
{
    public class JobQueueViewModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public long? FriendId { get; set; }

        public bool IsProcessed { get; set; }

        public FunctionName FunctionName { get; set; }

        public string FunctionStringName { get; set; }

        public DateTime AddedDateTime { get; set; }

        public string JobId { get; set; }

        public TimeModel LaunchDateTime { get; set; }
    }
}
