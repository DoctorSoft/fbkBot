using System.Collections.Generic;

namespace Services.ViewModels.SpySettingsViewModels
{
    public class SpySettingsViewModel
    {
        public long SpyId { get; set; }

        public List<SpyFunctionViewModel> SpyFunctions { get; set; }

        public DetailedSpyStatisticsModel SpyStatistics { get; set; } 
    }
}
