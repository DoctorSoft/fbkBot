using System;
using Constants.FunctionEnums;

namespace Services.ViewModels.QueueViewModels
{
    public class QueueViewModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime AddedDateTime { get; set; }
    }
}
