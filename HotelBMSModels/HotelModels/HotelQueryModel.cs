using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBMSModels.HotelModels
{
    public class HotelQueryModel
    {
        [DefaultValue("Glasgow")]
        public string? Name { get; init; }

        [DefaultValue(1)]
        public int Page { get; init; } = 1;
        
        [DefaultValue(10)]
        public int PageSize { get; init; } = 10;

        [DefaultValue("name")]
        public string? SortBy { get; init; } = "name";

        [DefaultValue(false)]
        public bool SortDesc { get; init; } = false;
    }
}
