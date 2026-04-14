using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Context;
using HotelBMSData.Entities;
using HotelBMSRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBMSRepository
{
    public class DbSeedRepository : IDbSeedRepository
    {
        private readonly DataContext dbContext;

        public DbSeedRepository(DataContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void ResetDatabase()
        {            
            dbContext.Bookings.RemoveRange(dbContext.Bookings);
            dbContext.Rooms.RemoveRange(dbContext.Rooms);
            dbContext.Hotels.RemoveRange(dbContext.Hotels);

            dbContext.SaveChanges();
        }

        public void ReSeedDatabase()
        {
            var bookings = dbContext.Bookings;
            var rooms = dbContext.Rooms;
            var hotels = dbContext.Hotels;

            if(!bookings.Any() && !rooms.Any() && !hotels.Any())
            {
                var glasgowHotelGuid = Guid.NewGuid();
                var edinburghHotelGuid = Guid.NewGuid();
                var stirlingHotelGuid = Guid.NewGuid();

                var glasgowRoom1Guid = Guid.NewGuid();
                var glasgowRoom2Guid = Guid.NewGuid();
                var glasgowRoom3Guid = Guid.NewGuid();
                var glasgowRoom4Guid = Guid.NewGuid();
                var glasgowRoom5Guid = Guid.NewGuid();
                var glasgowRoom6Guid = Guid.NewGuid();

                var edinburghRoom1Guid = Guid.NewGuid();
                var edinburghRoom2Guid = Guid.NewGuid();
                var edinburghRoom3Guid = Guid.NewGuid();
                var edinburghRoom4Guid = Guid.NewGuid();
                var edinburghRoom5Guid = Guid.NewGuid();
                var edinburghRoom6Guid = Guid.NewGuid();

                var stirlingRoom1Guid = Guid.NewGuid();
                var stirlingRoom2Guid = Guid.NewGuid();
                var stirlingRoom3Guid = Guid.NewGuid();
                var stirlingRoom4Guid = Guid.NewGuid();
                var stirlingRoom5Guid = Guid.NewGuid();
                var stirlingRoom6Guid = Guid.NewGuid();


                var newHotels = new List<Hotel> {
                    new Hotel { ID = glasgowHotelGuid, Name = "Glasgow Hotel", Timestamp = DateTime.UtcNow },
                    new Hotel { ID = edinburghHotelGuid, Name = "Edinburgh Hotel", Timestamp = DateTime.UtcNow },
                    new Hotel { ID = stirlingHotelGuid, Name = "Stirling Hotel", Timestamp = DateTime.UtcNow }
                };

                dbContext.Hotels.AddRange(newHotels);

                var allHotelRooms = new List<Room>();
                allHotelRooms.AddRange(new List<Room> {
                    new Room { ID = glasgowRoom1Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                    new Room { ID = glasgowRoom2Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2 },
                    new Room { ID = glasgowRoom3Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2 },
                    new Room { ID = glasgowRoom4Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4 },
                    new Room { ID = glasgowRoom5Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4 },
                    new Room { ID = glasgowRoom6Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 6 }
                });

                allHotelRooms.AddRange(new List<Room> {
                    new Room { ID = edinburghRoom1Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                    new Room { ID = edinburghRoom2Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                    new Room { ID = edinburghRoom3Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2 },
                    new Room { ID = edinburghRoom4Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 4 },
                    new Room { ID = edinburghRoom5Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4 },
                    new Room { ID = edinburghRoom6Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 6 }
                });

                allHotelRooms.AddRange(new List<Room> {
                    new Room { ID = stirlingRoom1Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                    new Room { ID = stirlingRoom2Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                    new Room { ID = stirlingRoom3Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2 },
                    new Room { ID = stirlingRoom4Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 4 },
                    new Room { ID = stirlingRoom5Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4 },
                    new Room { ID = stirlingRoom6Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 6 }
                });
                                
                dbContext.Rooms.AddRange(allHotelRooms);
                dbContext.SaveChanges();
            }
        }
    }
}
