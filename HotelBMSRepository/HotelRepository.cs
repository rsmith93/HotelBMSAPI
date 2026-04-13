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
    public class HotelRepository : IHotelRepository
    {
        private readonly DataContext dbContext;

        public HotelRepository(DataContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<Hotel> GetHotelByName(string name)
        {
            var hotels = dbContext.Hotels.AsNoTracking()
                .Where(x => !x.Archived && 
                            x.Name.ToLower().Trim().Contains(name))
                .OrderBy(x => x.Name);
            return hotels;
        }

        public IQueryable<Guid> GetExistigHotelIds()
        {
            return dbContext.Hotels.AsNoTracking().Select(x => x.ID);
        }

    }
}
