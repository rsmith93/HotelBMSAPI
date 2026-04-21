using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Context;
using HotelBMSData.Entities;
using HotelBMSModels.BookingModels;
using HotelBMSModels.RoomModels;
using HotelBMSRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBMSRepository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataContext dbContext;

        public RoomRepository(DataContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<List<Room>> GetHotelRoomsBySearchData(RoomSearchModel searchModel)
        {
            if (searchModel.EndDate <= searchModel.StartDate)
                throw new Exception("End date must be after start date, please update and try again.");

            if (searchModel.StartDate.Date < DateTime.UtcNow.Date)
                throw new Exception("Start date must be from today onwards, please update and try again.");

            //get all of the rooms associated with the chosen hotel
            //for the purposes of capacity, I'am treating this as the maximum number of guests a room can hold.
            //So the check is simply whether the room can accommodate the requested number of guests
            var availableRooms = await dbContext.Rooms.AsNoTracking().Include(x => x.Hotel)
                .Where(x => !x.Archived && 
                       x.HotelID == searchModel.HotelID &&
                       x.Capacity >= searchModel.NumberOfGuestsOnBooking).ToListAsync();

            //find bookings that overlap with the search criteria, rendering the rooms unavailable
            var overlappingBookings = await dbContext.Bookings.AsNoTracking()
                .Where(x => !x.Archived && 
                       x.Room.HotelID == searchModel.HotelID &&
                       x.StartDate < searchModel.EndDate &&
                       x.EndDate > searchModel.StartDate).ToListAsync();

            //filter out the rooms that are not available and return any that are
            var usedRoomIds = overlappingBookings.Select(x => x.RoomID);
            availableRooms = availableRooms.Where(x => !usedRoomIds.Contains(x.ID)).ToList();
            
            return availableRooms;
        }

        public async Task<Room?> IsRoomAvailable(RoomBookingModel model)
        {
            var searchModel = new RoomSearchModel()
            {
                EndDate = model.EndDate,
                HotelID = model.HotelID,
                NumberOfGuestsOnBooking = model.NumberOfGuestsOnBooking,
                StartDate = model.StartDate,
            };
            var availableRooms = await GetHotelRoomsBySearchData(searchModel);
            return availableRooms.OrderBy(x => x.Capacity).FirstOrDefault();
        }

    }
}
