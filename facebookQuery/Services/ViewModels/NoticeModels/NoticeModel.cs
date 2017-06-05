using System;

namespace Services.ViewModels.NoticeModels
{
    public class NoticeModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string NoticeText { get; set; }

        public DateTime DateTime { get; set; }
    }
}
