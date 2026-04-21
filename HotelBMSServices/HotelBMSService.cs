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


        public async Task<Guid> CreateRoomBooking(RoomBookingModel model)
        {
            var roomToBook = await roomRepo.IsRoomAvailable(model);
            if (roomToBook == null)
                throw new Exception("Sorry, there are no available rooms matching your criteria");

            var newBookingRef = await bookingRepo.CreateBooking(new HotelBMSData.Entities.Booking()
            {
                Archived = false,
                BookingReference = Guid.NewGuid(),
                EndDate = model.EndDate,
                NumberOfGuests = model.NumberOfGuestsOnBooking,
                RoomID = roomToBook.ID,
                StartDate = model.StartDate,
                Timestamp = DateTime.UtcNow
            });   
            return newBookingRef;
        }


        public async Task<List<RoomDTO>> GetAvailableHotelRoomsBySearch(RoomSearchModel searchModel)
        {
            var rooms = await roomRepo.GetHotelRoomsBySearchData(searchModel);

            return rooms.Select(x => new RoomDTO()
            {
                Capacity = x.Capacity,
                HotelID = x.HotelID,
                ID = x.ID,
                Type = x.Type,
                Hotel = new HotelBMSModels.HotelModels.HotelDTO()
                {
                    ID = x.Hotel.ID,
                    Name = x.Hotel.Name
                }
            }).ToList();
        }

        public async Task<List<HotelDTO>> GetAllAvailableHotels()
        {
            var hotels = await hotelRepo.GetAllAvailableHotels();

            return hotels.Select(x => new HotelDTO()
            {
                Name = x.Name,
                ID = x.ID,
            }).ToList();
        }

        public async Task<PagedResult<HotelDTO>> GetHotels(HotelQueryModel query)
        {
            return await hotelRepo.GetHotels(query);
        }

        public async Task<BookingDTO?> GetBookingByBookingRef(Guid bookingRef)
        {
            var booking = await bookingRepo.GetBookingByBookingRef(bookingRef);

            if (booking == null)
                return null;

            return new BookingDTO()
            {
                Archived = booking.Archived,
                BookingReference = booking.BookingReference,
                EndDate = booking.EndDate,
                NumberOfGuests = booking.NumberOfGuests,
                RoomID = booking.RoomID,
                StartDate = booking.StartDate,
                Timestamp = booking.Timestamp,
                Room = new RoomDTO()
                {
                    Capacity = booking.Room.Capacity,
                    Type = booking.Room.Type,
                    HotelID = booking.Room.HotelID,
                    ID = booking.Room.ID,
                    Hotel = new HotelDTO()
                    {
                        ID = booking.Room.Hotel.ID,
                        Name = booking.Room.Hotel.Name
                    }
                }
            };
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
