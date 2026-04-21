using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using HotelBMSModels.BaseModels;
using HotelBMSModels.HotelModels;

namespace HotelBMSRepository.Interfaces
{
    public interface IHotelRepository
    {
        Task<PagedResult<HotelDTO>> GetHotels(HotelQueryModel query);
        Task<List<Hotel>> GetAllAvailableHotels();
    }
}
