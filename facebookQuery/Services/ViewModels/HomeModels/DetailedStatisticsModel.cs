using CommonModels;

namespace Services.ViewModels.HomeModels
{
    public class DetailedStatisticsModel
    {
        public AccountStatisticsModel LastHourStatistic { get; set; }

        public AccountStatisticsModel AllTimeStatistic { get; set; }
    }
}
