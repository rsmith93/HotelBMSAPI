using System.Xml.Linq;
using HotelBMSAPI.Helpers;
using HotelBMSData.Entities;
using HotelBMSModels.BaseModels;
using HotelBMSModels.BookingModels;
using HotelBMSModels.HotelModels;
using HotelBMSModels.RoomModels;
using HotelBMSServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelBMSAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelBMSService bmsService;

        public HotelsController(IHotelBMSService _bmsService)
        {
            bmsService = _bmsService;
        }

        [HttpGet("hotels")]
        [SwaggerOperation(
            Summary = "Get hotels with filtering, sorting, and pagination",
            Description = "Supports filtering by name, sorting, and pagination"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Hotels retrieved successfully", typeof(PagedResult<HotelDTO>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HotelPagedResultExample))]
        public async Task<ActionResult<PagedResult<HotelDTO>>> GetHotels([FromQuery] HotelQueryModel query)
        {
            var hotels = await bmsService.GetHotels(query);
            return Ok(hotels);
        }

        [HttpGet("bookings/{bookingRef:guid}")]
        [SwaggerOperation(
            Summary = "Get hotels bookings using a unique booking reference"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Booking retrieved successfully", typeof(ActionResult<BookingDTO>))]
        public async Task<ActionResult<BookingDTO>> GetBooking(Guid bookingRef)
        {
            var booking = await bmsService.GetBookingByBookingRef(bookingRef);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpPost("bookings")]
        [SwaggerOperation(
            Summary = "Create a booking at a hotel for available room only"
        )]
        public async Task<ActionResult> CreateBooking([FromBody] RoomBookingModel model)
        {
            var bookingRef = await bmsService.CreateRoomBooking(model);

            return CreatedAtAction(
                nameof(GetBooking),
                new { bookingRef },
                new { bookingRef }
            );
        }

        [HttpGet("rooms/available")]
        [SwaggerOperation(
            Summary = "Check to see if rooms are available"
        )]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetAvailableRooms([FromQuery] RoomSearchModel searchModel)
        {
            var rooms = await bmsService.GetAvailableHotelRoomsBySearch(searchModel);
            return Ok(rooms);
        }
    }
}
