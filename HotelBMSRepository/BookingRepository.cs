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
    public class BookingRepository : IBookingRepository
    {
        private readonly DataContext dbContext;

        public BookingRepository(DataContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public Booking GetBookingByBookingRef(Guid bookingRef)
        {
            var booking = dbContext.Bookings.Include(x => x.Room).Include(x => x.Room.Hotel).AsNoTracking().FirstOrDefault(x => x.BookingReference == bookingRef);
            if (booking == null)
                throw new Exception("Sorry, we could not find a booking to match your booking reference.");
            return booking;
        }

        public Guid CreateBooking(Booking entity)
        {            
            //double check that the room has not already been booked since the start of the booking process
            //this is just a simple check before submitting to db
            //in a real world example, using a transaction would be a more future-proof strategy to avoid double bookings
            var roomStillAvailable = !dbContext.Bookings.AsNoTracking()
                .Any(b =>
                        b.RoomID == entity.RoomID &&
                        b.StartDate < entity.EndDate &&
                        b.EndDate > entity.StartDate);

            if (!roomStillAvailable)
                throw new Exception("Sorry, this room was just booked, please try again.");

            dbContext.Bookings.Add(entity);
            dbContext.SaveChanges();

            return entity.BookingReference;            
        }
    }
}
