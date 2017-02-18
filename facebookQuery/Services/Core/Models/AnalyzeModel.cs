using DataBase.QueriesAndCommands.Models;
using Services.ViewModels.GroupModels;

namespace Services.Core.Models
{
    public class AnalyzeModel
    {
        public long SpyAccountId { get; set; }

        public GroupSettingsViewModel Settings { get; set; }

        public AnalysisFriendData AnalysisFriend { get; set; }

        public bool GenderIsSuccess { get; set; }

        public bool InfoIsSuccess { get; set; }
    }
}
