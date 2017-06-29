using System;

namespace Services.ViewModels.RunnerModels
{
    public class RunnerViewModel
    {
        public long Id { get; set; }

        public string DeviceName { get; set; }

        public bool IsAllowed { get; set; }

        public DateTime LastAction { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
