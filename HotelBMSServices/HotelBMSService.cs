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
using Microsoft.Extensions.Logging;

namespace HotelBMSServices
{
    public class HotelBMSService : IHotelBMSService
    {
        private readonly IBookingRepository bookingRepo;
        private readonly IHotelRepository hotelRepo;
        private readonly IRoomRepository roomRepo;
        private readonly IDbSeedRepository seedRepo;
        private readonly ILogger<HotelBMSService> logger;

        public HotelBMSService(IBookingRepository _bookingRepo, 
            IHotelRepository _hotelRepo, 
            IRoomRepository _roomRepo,
            IDbSeedRepository _seedRepo,
            ILogger<HotelBMSService> _logger)
        {
            bookingRepo = _bookingRepo;
            hotelRepo = _hotelRepo;
            roomRepo = _roomRepo;
            seedRepo = _seedRepo;
            logger = _logger;
        }


        public async Task<Guid> CreateRoomBooking(RoomBookingModel model)
        {
            logger.LogInformation(
                "Attempting to create booking for Hotel {HotelId} from {StartDate} to {EndDate} for {Guests} guests",
                model.HotelID,
                model.StartDate,
                model.EndDate,
                model.NumberOfGuestsOnBooking
            );

            var roomToBook = await roomRepo.IsRoomAvailable(model);
            if (roomToBook == null)
            {
                logger.LogWarning(
                    "No available rooms found for Hotel {HotelId} between {StartDate} and {EndDate}",
                    model.HotelID,
                    model.StartDate,
                    model.EndDate
                );
                throw new InvalidOperationException("Sorry, there are no available rooms matching your criteria");
            }

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

            logger.LogInformation(
                "Booking created successfully. BookingRef {BookingRef}, RoomId {RoomId}",
                newBookingRef,
                roomToBook.ID
            );

            return newBookingRef;
        }


        public async Task<List<RoomDTO>> GetAvailableHotelRoomsBySearch(RoomSearchModel searchModel)
        {
            logger.LogInformation(
                "Searching available rooms for Hotel {HotelId} between {StartDate} and {EndDate}",
                searchModel.HotelID,
                searchModel.StartDate,
                searchModel.EndDate
            );

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
            logger.LogInformation(
                "Retrieving hotels (Page {Page}, PageSize {PageSize}, Name filter: {Name})",
                query.Page,
                query.PageSize,
                query.Name
            );

            var result = await hotelRepo.GetHotels(query);

            logger.LogInformation(
                "Retrieved {Count} hotels (Total: {Total})",
                result.Items.Count(),
                result.TotalCount
            );

            return result;
        }

        public async Task<BookingDTO?> GetBookingByBookingRef(Guid bookingRef)
        {
            logger.LogInformation("Retrieving booking {BookingRef}", bookingRef);

            var booking = await bookingRepo.GetBookingByBookingRef(bookingRef);

            if (booking == null)
            {
                logger.LogWarning("Booking {BookingRef} not found", bookingRef);
                return null;
            }

            logger.LogInformation("Booking {BookingRef} retrieved successfully", bookingRef);
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
            logger.LogWarning("Resetting database");
            seedRepo.ReSeedDatabase();
        }

        public void ResetDatabase()
        {
            logger.LogWarning("Reseeding database with test data");
            seedRepo.ResetDatabase();
        }

    }
}
