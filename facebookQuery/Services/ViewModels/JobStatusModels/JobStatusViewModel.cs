using System;
using Constants.FunctionEnums;

namespace Services.ViewModels.JobStatusModels
{
    public class JobStatusViewModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string JobId { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public TimeSpan LaunchDateTime { get; set; }

        public DateTime AddDateTime { get; set; }
    }
}
