using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBMSData.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Archived { get; set; }
    }
}
