using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBMSData.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }


        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var glasgowHotelGuid = new Guid("11111111-1111-1111-1111-111111111111");
            var edinburghHotelGuid = new Guid("22222222-2222-2222-2222-222222222222");
            var stirlingHotelGuid = new Guid("33333333-3333-3333-3333-333333333333");

            var glasgowRoom1Guid = new Guid("44444444-4444-4444-4444-000000000001");
            var glasgowRoom2Guid = new Guid("44444444-4444-4444-4444-000000000002");
            var glasgowRoom3Guid = new Guid("44444444-4444-4444-4444-000000000003");
            var glasgowRoom4Guid = new Guid("44444444-4444-4444-4444-000000000004");
            var glasgowRoom5Guid = new Guid("44444444-4444-4444-4444-000000000005");
            var glasgowRoom6Guid = new Guid("44444444-4444-4444-4444-000000000006");

            var edinburghRoom1Guid = new Guid("55555555-5555-5555-5555-000000000001");
            var edinburghRoom2Guid = new Guid("55555555-5555-5555-5555-000000000002");
            var edinburghRoom3Guid = new Guid("55555555-5555-5555-5555-000000000003");
            var edinburghRoom4Guid = new Guid("55555555-5555-5555-5555-000000000004");
            var edinburghRoom5Guid = new Guid("55555555-5555-5555-5555-000000000005");
            var edinburghRoom6Guid = new Guid("55555555-5555-5555-5555-000000000006");

            var stirlingRoom1Guid = new Guid("66666666-6666-6666-6666-000000000001");
            var stirlingRoom2Guid = new Guid("66666666-6666-6666-6666-000000000002");
            var stirlingRoom3Guid = new Guid("66666666-6666-6666-6666-000000000003");
            var stirlingRoom4Guid = new Guid("66666666-6666-6666-6666-000000000004");
            var stirlingRoom5Guid = new Guid("66666666-6666-6666-6666-000000000005");
            var stirlingRoom6Guid = new Guid("66666666-6666-6666-6666-000000000006");

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { ID = glasgowHotelGuid, Name = "Glasgow Hotel", Timestamp = DateTime.UtcNow },
                new Hotel { ID = edinburghHotelGuid, Name = "Edinburgh Hotel", Timestamp = DateTime.UtcNow },
                new Hotel { ID = stirlingHotelGuid, Name = "Stirling Hotel", Timestamp = DateTime.UtcNow }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room { ID = glasgowRoom1Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1},
                new Room { ID = glasgowRoom2Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2},
                new Room { ID = glasgowRoom3Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2},
                new Room { ID = glasgowRoom4Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4},
                new Room { ID = glasgowRoom5Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4},
                new Room { ID = glasgowRoom6Guid, HotelID = glasgowHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 6}
                );

            modelBuilder.Entity<Room>().HasData(
                new Room { ID = edinburghRoom1Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                new Room { ID = edinburghRoom2Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                new Room { ID = edinburghRoom3Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2 },
                new Room { ID = edinburghRoom4Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 4 },
                new Room { ID = edinburghRoom5Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4 },
                new Room { ID = edinburghRoom6Guid, HotelID = edinburghHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 6 }
                );

            modelBuilder.Entity<Room>().HasData(
                new Room { ID = stirlingRoom1Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                new Room { ID = stirlingRoom2Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Single, Capacity = 1 },
                new Room { ID = stirlingRoom3Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 2 },
                new Room { ID = stirlingRoom4Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Double, Capacity = 4 },
                new Room { ID = stirlingRoom5Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 4 },
                new Room { ID = stirlingRoom6Guid, HotelID = stirlingHotelGuid, Timestamp = DateTime.UtcNow, Type = HotelBMSHelpers.Enums.RoomEnum.RoomType.Deluxe, Capacity = 6 }
                );
        }

    }
}
