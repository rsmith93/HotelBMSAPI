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
    public class RoomRepository : IRoomRepository
    {
        private readonly DataContext dbContext;

        public RoomRepository(DataContext _dbContext)
        {
            dbContext = _dbContext;
        }


    }
}
