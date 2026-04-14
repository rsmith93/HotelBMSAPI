using System;
using System.Data.Entity;
using System.Linq;
using HotelBMSData.Context;
using HotelBMSData.Entities;
using HotelBMSRepository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using NUnit.Framework;

namespace HotelsBMSUnitTests
{   

    public class HotelRepositoryTests
    {
        private DataContext dbContext;
        private HotelRepository hotelRepo;
        private SqliteConnection connection;

        #region SetUp

        [SetUp]
        public void Setup()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection)
            .Options;

            dbContext = new DataContext(options);
            dbContext.Database.EnsureCreated();
            SeedData();

            hotelRepo = new HotelRepository(dbContext);
        }

        private void SeedData()
        {
            var glasgowHotelGuid = Guid.NewGuid();
            var edinburghHotelGuid = Guid.NewGuid();
            var stirlingHotelGuid = Guid.NewGuid();

            dbContext.Hotels.AddRange(
                new Hotel { ID = glasgowHotelGuid, Name = "Alpha Hotel", Archived = false },
                new Hotel { ID = edinburghHotelGuid, Name = "Beta Hotel", Archived = false },
                new Hotel { ID = stirlingHotelGuid, Name = "  alpha boutique  ", Archived = false }
            );

            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
            connection.Close();
        }

        #endregion

        // =========================
        // GetAllAvailableHotels
        // =========================

        [Test]
        public void GetAllAvailableHotels_ShouldExcludeArchived()
        {
            var result = hotelRepo.GetAllAvailableHotels().ToList();

            Assert.That(result.Any(h => h.Archived), Is.False);
        }

        [Test]
        public void GetAllAvailableHotels_ShouldBeOrderedByName()
        {
            var result = hotelRepo.GetAllAvailableHotels().ToList();

            var ordered = result.Select(x => x.Name).ToList();
            var sorted = ordered.OrderBy(x => x).ToList();

            Assert.That(ordered, Is.EqualTo(sorted));
        }

        // =========================
        // GetHotelByName
        // =========================

        [Test]
        public void GetHotelByName_ShouldReturnMatchingHotels()
        {
            var result = hotelRepo.GetHotelByName("alpha").ToList();

            Assert.That(result.Count, Is.EqualTo(2)); // Alpha Hotel + alpha boutique
        }

        [Test]
        public void GetHotelByName_ShouldBeCaseInsensitiveDueToToLower()
        {
            var result = hotelRepo.GetHotelByName("ALPHA").ToList();

            Assert.That(result.Any(h => h.Name.Contains("Alpha")), Is.True);
        }

        [Test]
        public void GetHotelByName_ShouldExcludeArchivedHotels()
        {
            var result = hotelRepo.GetHotelByName("gamma").ToList();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetHotelByName_ShouldReturnEmpty_WhenNoMatch()
        {
            var result = hotelRepo.GetHotelByName("nonexistent").ToList();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetHotelByName_ShouldThrow_WhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                hotelRepo.GetHotelByName(null).ToList();
            });
        }

        [Test]
        public void GetHotelByName_ShouldHandleEmptyString_ReturnAllNonArchived()
        {
            var result = hotelRepo.GetHotelByName("").ToList();

            Assert.That(result.All(h => !h.Archived), Is.True);
        }

        [Test]
        public void GetHotelByName_ShouldIgnoreWhitespaceInDbNameButNotInput()
        {
            var result = hotelRepo.GetHotelByName("alpha").ToList();

            Assert.That(result.Any(h => h.Name.Contains("alpha")), Is.True);
        }

        // =========================
        // Security / injection-style tests
        // =========================

        [Test]
        public void GetHotelByName_ShouldTreatSqlInjectionStringAsLiteral()
        {
            var maliciousInput = "' OR 1=1 --";

            var result = hotelRepo.GetHotelByName(maliciousInput).ToList();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetHotelByName_ShouldNotThrow_OnVeryLongInput()
        {
            var longInput = new string('a', 10000);

            Assert.DoesNotThrow(() =>
            {
                hotelRepo.GetHotelByName(longInput).ToList();
            });
        }

        // =========================
        // Ordering validation for search
        // =========================

        [Test]
        public void GetHotelByName_ShouldReturnOrderedResults()
        {
            var result = hotelRepo.GetHotelByName("hotel").ToList();

            var names = result.Select(x => x.Name).ToList();
            var sorted = names.OrderBy(x => x).ToList();

            Assert.That(names, Is.EqualTo(sorted));
        }

    }
}
