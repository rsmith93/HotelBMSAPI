using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSModels.BaseModels;
using HotelBMSModels.HotelModels;
using Swashbuckle.AspNetCore.Filters;

namespace HotelBMSAPI.Helpers
{
    public class HotelPagedResultExample : IExamplesProvider<PagedResult<HotelDTO>>
    {
        public PagedResult<HotelDTO> GetExamples()
        {
            return new PagedResult<HotelDTO>
            {
                Page = 1,
                PageSize = 10,
                TotalCount = 2,
                Items = new[]
                {
                new HotelDTO(){ ID = Guid.NewGuid(), Name = "Glasgow Hotel"},
                new HotelDTO(){ ID = Guid.NewGuid(), Name = "Edinburgh Hotel" }
            }
            };
        }
    }
}
