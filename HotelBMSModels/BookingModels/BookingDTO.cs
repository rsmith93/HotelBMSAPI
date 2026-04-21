using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSModels.RoomModels;

namespace HotelBMSModels.BookingModels
{
    public class BookingDTO
    {
        public Guid ID { get; set; }
        public Guid BookingReference { get; set; }
        public Guid RoomID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Archived { get; set; }

        public RoomDTO Room { get; set; }
    }
}
