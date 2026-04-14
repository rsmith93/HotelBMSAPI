using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using HotelBMSModels.RoomModels;

namespace HotelBMSServices.Interfaces
{
    public  interface IHotelBMSService
    {
        Guid CreateRoomBooking(RoomSearchModel searchModel);
        IQueryable<Room> GetAvailableHotelRoomsBySearch(RoomSearchModel searchModel);
        IQueryable<Hotel> GetAllAvailableHotels();
        IQueryable<Hotel> GetHotelByName(string name);
        Booking GetBookingByBookingRef(Guid bookingRef);
        void ResetDatabase();
        void ReseedDatabase();        
    }
}
