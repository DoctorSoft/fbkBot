using System;

namespace Services.ViewModels.PageModels
{
    public class PageInfoModel
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages => (int) Math.Ceiling((decimal) TotalItems / PageSize);
    }
}
