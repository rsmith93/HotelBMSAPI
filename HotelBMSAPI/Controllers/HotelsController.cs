using System.Xml.Linq;
using HotelBMSData.Entities;
using HotelBMSModels.RoomModels;
using HotelBMSServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBMSAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelBMSService bmsService;

        public HotelsController(IHotelBMSService _bmsService)
        {
            _bmsService = bmsService;
        }

        [HttpGet("search")]
        public IActionResult GetHotelsByName([FromQuery]string name)
        {
            if (String.IsNullOrEmpty(name))
                return BadRequest("Name is required to continue searching");

            var hotels = bmsService.GetHotelByName(name).ToList();
            return Ok(hotels);
        }

        [HttpGet("bookings/{bookingRef}")]
        public IActionResult GetBookingByBookingRef(Guid bookingRef)
        {
            var booking = bmsService.GetBookingByBookingRef(bookingRef);
            return Ok(booking);
        }

        [HttpPost("bookings")]
        public IActionResult CreateRoomBooking([FromBody]RoomSearchModel searchModel)
        {
            var bookingRef = bmsService.CreateRoomBooking(searchModel);
            return Ok(new { message = $"Booking with reference {bookingRef} created successfully" });
        }

        [HttpGet("rooms/available")]
        public IActionResult GetAvailableHotelRoomsBySearch([FromQuery] RoomSearchModel searchModel)
        {
            var rooms = bmsService.GetAvailableHotelRoomsBySearch(searchModel).ToList();
            return Ok(rooms);
        }
    }
}
