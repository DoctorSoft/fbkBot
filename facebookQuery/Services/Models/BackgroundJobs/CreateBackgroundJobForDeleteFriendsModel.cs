using System;
using CommonInterfaces.Interfaces.Models;
using Constants.FunctionEnums;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Models.BackgroundJobs
{
    public class CreateBackgroundJobForDeleteFriendsModel : ICreateBackgroundJobForDeleteFriends
    {
        public AccountViewModel Account { get; set; }

        public FunctionName FunctionName { get; set; }

        public int LaunchHourTime { get; set; }

        public FriendViewModel Friend { get; set; } 
    }
}
