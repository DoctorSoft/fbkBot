using Constants.GendersUnums;

namespace Services.ViewModels.OptionsModel.JsonModels
{
    public class GeoOptionsModel
    {
        public string Cities { get; set; }

        public string Countries { get; set; }

        public GenderEnum? Gender { get; set; }
    }
}
