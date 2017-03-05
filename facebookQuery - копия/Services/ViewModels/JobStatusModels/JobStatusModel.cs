using System;
using Constants.FunctionEnums;

namespace Services.ViewModels.JobStatusModels
{
    public class JobStatusModel
    {
        public long Id { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime LastLaunchDateTime { get; set; }
    }
}
