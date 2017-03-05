using System.Collections.Generic;

namespace Services.ViewModels.SpySettingsViewModels
{
    public class NewSpySettingsViewModel
    {
        public long SpyId { get; set; }

        public List<long> functions { get; set; }
    }
}
