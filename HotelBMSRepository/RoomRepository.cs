using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Context;
using HotelBMSData.Entities;
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

        public IQueryable<Room> GetHotelRoomsBySearchData(RoomSearchModel searchModel)
        {
            if (searchModel.EndDate <= searchModel.StartDate)
                throw new Exception("End date must be after start date, please update and try again");

            //get all of the rooms associated with the chosen hotel
            //for the purposes of capacity, I'am treating this as the maximum number of guests a room can hold.
            //So the check is simply whether the room can accommodate the requested number of guests
            IQueryable<Room> availableRooms = dbContext.Rooms.AsNoTracking()
                .Where(x => !x.Archived && 
                       x.HotelID == searchModel.HotelID &&
                       x.Capacity >= searchModel.NumberOfGuestsOnBooking);

            //find bookings that overlap with the search criteria, rendering the rooms unavailable
            var overlappingBookings = dbContext.Bookings.AsNoTracking()
                .Where(x => !x.Archived && 
                       x.Room.HotelID == searchModel.HotelID &&
                       x.StartDate < searchModel.EndDate &&
                       x.EndDate > searchModel.StartDate);

            //filter out the rooms that are not available and return any that are
            var usedRoomIds = overlappingBookings.Select(x => x.RoomID);
            availableRooms = availableRooms.Where(x => !usedRoomIds.Contains(x.ID));
            
            return availableRooms;
        }

        public Room IsRoomAvailable(RoomSearchModel searchModel)
        {
            var availableRooms = GetHotelRoomsBySearchData(searchModel);
            if (availableRooms.Any())
            {
                var closestMatchedRoom = availableRooms.OrderBy(x => x.Capacity).FirstOrDefault();
                if (closestMatchedRoom != null)
                    return closestMatchedRoom;
            }
            throw new Exception("Sorry, there are no available rooms for your criteria.");
        }

    }
}
