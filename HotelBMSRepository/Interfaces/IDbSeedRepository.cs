using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBMSRepository.Interfaces
{
    public interface IDbSeedRepository
    {
        void ResetDatabase();
        void ReSeedDatabase();
    }
}
