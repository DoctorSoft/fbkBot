using Constants.FunctionEnums;

namespace Services.ViewModels.JobStateViewModels
{
    public class JobStateViewModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public string JobId { get; set; }
    }
}
