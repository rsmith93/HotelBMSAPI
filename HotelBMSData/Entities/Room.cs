using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HotelBMSHelpers.Enums.RoomEnum;

namespace HotelBMSData.Entities
{
    public class Room: BaseEntity
    {
        [ForeignKey("HotelID")]
        public Guid HotelID { get; set; }
        public int Capacity { get; set; }
        public RoomType Type { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
