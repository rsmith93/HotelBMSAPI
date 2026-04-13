using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBMSModels.RoomModels
{
    public class RoomSearchModel
    {
        public Guid HotelID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuestsOnBooking { get; set; }
    }
}
