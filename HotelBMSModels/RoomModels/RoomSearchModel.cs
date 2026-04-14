using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBMSModels.RoomModels
{
    public class RoomSearchModel
    {
        [Required]
        [SwaggerSchema(Description = "These can be found by performing the api/Hotels/search call.")]
        public Guid HotelID { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "Start date in format YYYY-MM-DD (e.g. 2026-07-21)")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "End date in format YYYY-MM-DD (e.g. 2026-07-25)")]
        public DateTime EndDate { get; set; }
        
        [Required]
        [SwaggerSchema(Description = "Number of guests for the booking")]
        [Range(1, 6)]
        public int NumberOfGuestsOnBooking { get; set; }
    }
}
