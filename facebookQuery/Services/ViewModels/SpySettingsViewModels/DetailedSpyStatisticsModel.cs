using CommonModels;

namespace Services.ViewModels.SpySettingsViewModels
{
    public class DetailedSpyStatisticsModel
    {
        public SpyStatisticsModel LastHourStatistic { get; set; }

        public SpyStatisticsModel AllTimeStatistic { get; set; }
    }
}
