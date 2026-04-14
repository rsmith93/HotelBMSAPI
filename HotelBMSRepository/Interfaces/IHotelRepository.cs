using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;

namespace HotelBMSRepository.Interfaces
{
    public interface IHotelRepository
    {
        IQueryable<Hotel> GetHotelByName(string name);
        IQueryable<Hotel> GetAllAvailableHotels();
    }
}
