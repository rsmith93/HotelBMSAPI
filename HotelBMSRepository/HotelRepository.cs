using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Context;
using HotelBMSData.Entities;
using HotelBMSModels.BaseModels;
using HotelBMSModels.HotelModels;
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

        public async Task<PagedResult<HotelDTO>> GetHotels(HotelQueryModel query)
        {
            var dbQuery = dbContext.Hotels.AsNoTracking().Where(h => !h.Archived);

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var name = query.Name.Trim();
                dbQuery = dbQuery.Where(h => EF.Functions.Like(h.Name, $"%{name}%"));
            }

            var totalCount = await dbQuery.CountAsync();

            dbQuery = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDesc
                    ? dbQuery.OrderByDescending(h => h.Name)
                    : dbQuery.OrderBy(h => h.Name),
                    //DEFAULT
                _ => dbQuery.OrderBy(h => h.Name)
            };


            var items = await dbQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(h => new HotelDTO()
                {
                    ID = h.ID,  
                    Name = h.Name,
                })
                .ToListAsync();

            return new PagedResult<HotelDTO>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<List<Hotel>> GetAllAvailableHotels()
        {
            return await dbContext.Hotels.AsNoTracking()
                .Where(x => !x.Archived)
                .OrderBy(x => x.Name).ToListAsync();
        }

    }
}
