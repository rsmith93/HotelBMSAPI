using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using HotelBMSModels.BaseModels;
using HotelBMSModels.BookingModels;
using HotelBMSModels.HotelModels;
using HotelBMSModels.RoomModels;

namespace HotelBMSServices.Interfaces
{
    public  interface IHotelBMSService
    {
        Task<Guid> CreateRoomBooking(RoomBookingModel searchModel);
        Task<List<RoomDTO>> GetAvailableHotelRoomsBySearch(RoomSearchModel searchModel);
        Task<List<HotelDTO>> GetAllAvailableHotels();
        Task<PagedResult<HotelDTO>> GetHotels(HotelQueryModel query);
        Task<BookingDTO?> GetBookingByBookingRef(Guid bookingRef);
        void ResetDatabase();
        void ReseedDatabase();        
    }
}
