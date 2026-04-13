using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using HotelBMSModels.RoomModels;

namespace HotelBMSRepository.Interfaces
{
    public interface IRoomRepository
    {
        IQueryable<Room> GetHotelRoomsBySearchData(RoomSearchModel searchModel);
        Room IsRoomAvailable(RoomSearchModel searchModel);
    }
}
