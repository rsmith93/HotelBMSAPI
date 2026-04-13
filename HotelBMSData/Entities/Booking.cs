using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBMSData.Entities
{
    public class Booking: BaseEntity
    {
        public Guid BookingReference { get; set; }
        [ForeignKey("RoomID")]
        public Guid RoomID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate{ get; set; }
        public int NumberOfGuests { get; set; }

        public virtual Room Room { get; set; }
    }
}
