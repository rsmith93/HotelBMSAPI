using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Context;
using HotelBMSData.Entities;
using HotelBMSModels.RoomModels;
using HotelBMSRepository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HotelsBMSUnitTests
{
    public  class RoomRepositoryTests
    {
        private DataContext dbContext;
        private RoomRepository roomRepo;
        private SqliteConnection connection;
        private Guid hotelID;

        #region SetUp

        [SetUp]
        public void Setup()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection)
            .Options;
            hotelID = Guid.NewGuid();
            dbContext = new DataContext(options);
            dbContext.Database.EnsureCreated();
            SeedData();

            roomRepo = new RoomRepository(dbContext);
        }

        private void SeedData()
        {
            var hotel = new Hotel
            {
                ID = hotelID,
                Name = "Test Hotel",
                Archived = false
            };

            dbContext.Hotels.Add(hotel);

            var room1 = new Room
            {
                ID = Guid.NewGuid(),
                HotelID = hotelID,
                Capacity = 2,
                Archived = false,
                Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double
            };

            var room2 = new Room
            {
                ID = Guid.NewGuid(),
                HotelID = hotelID,
                Capacity = 4,
                Archived = false,
                Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe
            };

            dbContext.Rooms.AddRange(room1, room2);
            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
            connection.Close();
        }

        #endregion


        [Test]
        public void GetRooms_ShouldThrow_WhenEndDateBeforeStartDate()
        {
            var model = new RoomSearchModel
            {
                HotelID = hotelID,
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(3),
                NumberOfGuestsOnBooking = 2
            };

            Assert.Throws<Exception>(() => roomRepo.GetHotelRoomsBySearchData(model).ToList());
        }

        [Test]
        public void GetRooms_ShouldReturnAvailableRooms()
        {
            var model = new RoomSearchModel
            {
                HotelID = hotelID,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(3),
                NumberOfGuestsOnBooking = 2
            };

            var result = roomRepo.GetHotelRoomsBySearchData(model).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void GetRooms_ShouldFilterRooms_ByCapacity()
        {
            var model = new RoomSearchModel
            {
                HotelID = hotelID,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(3),
                NumberOfGuestsOnBooking = 3
            };

            var result = roomRepo.GetHotelRoomsBySearchData(model).ToList();

            Assert.IsTrue(result.All(r => r.Capacity >= 3));
        }

        [Test]
        public void GetRooms_ShouldExcludeRooms_WithOverlappingBookings()
        {
            var room = dbContext.Rooms.First();

            dbContext.Bookings.Add(new Booking
            {
                RoomID = room.ID,
                StartDate = DateTime.UtcNow.AddDays(2),
                EndDate = DateTime.UtcNow.AddDays(4),
                Archived = false
            });

            dbContext.SaveChanges();

            var model = new RoomSearchModel
            {
                HotelID = hotelID,
                StartDate = DateTime.UtcNow.AddDays(3),
                EndDate = DateTime.UtcNow.AddDays(5),
                NumberOfGuestsOnBooking = 1
            };

            var result = roomRepo.GetHotelRoomsBySearchData(model).ToList();

            Assert.IsFalse(result.Any(r => r.ID == room.ID));
        }

        [Test]
        public void GetRooms_ShouldReturnEmpty_WhenNoRoomsAvailable()
        {
            var model = new RoomSearchModel
            {
                HotelID = hotelID,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(3),
                NumberOfGuestsOnBooking = 10
            };

            var result = roomRepo.GetHotelRoomsBySearchData(model).ToList();

            Assert.IsEmpty(result);
        }

    }
}
