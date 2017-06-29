using System;
using CommonInterfaces.Interfaces.Models;
using Constants.FunctionEnums;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Models.BackgroundJobs
{
    public class CreateBackgroundJobModel : ICreateBackgroundJob
    {
        public AccountViewModel Account { get; set; }
        
        public bool CheckPermissions { get; set; }

        public FriendViewModel Friend { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName FunctionName { get; set; }

        public TimeSpan LaunchTime { get; set; }
    }
}
