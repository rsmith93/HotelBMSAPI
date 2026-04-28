using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBMSModels.LogModels
{
    public class LogSearchDTO
    {
        [SwaggerSchema(Description = "From in format YYYY-MM-DD (e.g. 2026-07-21)")]
        public DateTime? From { get; set; }
        [SwaggerSchema(Description = "To in format YYYY-MM-DD (e.g. 2026-07-21)")]
        public DateTime? To { get; set; }
        [SwaggerSchema(Description = "Information, Warning, Error are the available options")]
        public string? Level { get; set; }
        [SwaggerSchema(Description = "Filter error messages based on specific input")]
        public string? Message { get; set; }
    }
}
