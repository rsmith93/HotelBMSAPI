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

        public async Task<Booking?> GetBookingByBookingRef(Guid bookingRef)
        {
            return await dbContext.Bookings.Include(x => x.Room).Include(x => x.Room.Hotel).AsNoTracking().FirstOrDefaultAsync(x => x.BookingReference == bookingRef);    
        }

        public async Task<Guid> CreateBooking(Booking entity)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var overlapExists = await dbContext.Bookings
            .AnyAsync(b =>
                b.RoomID == entity.RoomID &&
                b.StartDate < entity.EndDate &&
                b.EndDate > entity.StartDate);

            if (overlapExists)
                throw new InvalidOperationException("Room already booked.");

            dbContext.Bookings.Add(entity);
            dbContext.SaveChanges();

            return entity.BookingReference;            
        }
    }
}
