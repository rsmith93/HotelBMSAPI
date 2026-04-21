using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSModels.HotelModels;
using static HotelBMSHelpers.Enums.RoomEnum;

namespace HotelBMSModels.RoomModels
{
    public class RoomDTO
    {
        public Guid ID { get; set; }
        public Guid HotelID { get; set; }
        public int Capacity { get; set; }
        public RoomType Type { get; set; }
        public HotelDTO Hotel { get; set; }
    }
}
