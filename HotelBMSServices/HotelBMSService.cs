using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using HotelBMSModels.RoomModels;
using HotelBMSRepository.Interfaces;
using HotelBMSServices.Interfaces;

namespace HotelBMSServices
{
    public class HotelBMSService : IHotelBMSService
    {
        private readonly IBookingRepository bookingRepo;
        private readonly IHotelRepository hotelRepo;
        private readonly IRoomRepository roomRepo;
        private readonly IDbSeedRepository seedRepo;

        public HotelBMSService(IBookingRepository _bookingRepo, 
            IHotelRepository _hotelRepo, 
            IRoomRepository _roomRepo,
            IDbSeedRepository _seedRepo)
        {
            bookingRepo = _bookingRepo;
            hotelRepo = _hotelRepo;
            roomRepo = _roomRepo;
            seedRepo = _seedRepo;
        }


        public Guid CreateRoomBooking(RoomSearchModel searchModel)
        {
            var roomToBook = roomRepo.IsRoomAvailable(searchModel);
            if (roomToBook == null)
                throw new Exception("Sorry, there are no available rooms matching your criteria");

            var newBookingRef = bookingRepo.CreateBooking(new HotelBMSData.Entities.Booking()
            {
                Archived = false,
                BookingReference = Guid.NewGuid(),
                EndDate = searchModel.EndDate,
                NumberOfGuests = searchModel.NumberOfGuestsOnBooking,
                RoomID = roomToBook.ID,
                StartDate = searchModel.StartDate,
                Timestamp = DateTime.UtcNow
            });   
            return newBookingRef;
        }


        public IQueryable<Room> GetAvailableHotelRoomsBySearch(RoomSearchModel searchModel)
        {
            return roomRepo.GetHotelRoomsBySearchData(searchModel);
        }

        public IQueryable<Hotel> GetAllAvailableHotels()
        {
            return hotelRepo.GetAllAvailableHotels();
        }

        public IQueryable<Hotel> GetHotelByName(string name)
        {
            return hotelRepo.GetHotelByName(name.ToLower().Trim());
        }

        public Booking GetBookingByBookingRef(Guid bookingRef)
        {
            return bookingRepo.GetBookingByBookingRef(bookingRef);
        }

        public void ReseedDatabase()
        {
            seedRepo.ReSeedDatabase();
        }

        public void ResetDatabase()
        {
            seedRepo.ResetDatabase();
        }

    }
}
