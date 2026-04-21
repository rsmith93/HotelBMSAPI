using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using HotelBMSModels.BookingModels;
using HotelBMSModels.RoomModels;

namespace HotelBMSRepository.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetHotelRoomsBySearchData(RoomSearchModel searchModel);
        Task<Room?> IsRoomAvailable(RoomBookingModel searchModel);
    }
}
