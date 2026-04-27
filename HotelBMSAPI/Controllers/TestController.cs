using HotelBMSServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBMSAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class TestController : ControllerBase
    {
        private readonly IHotelBMSService bmsService;

        public TestController(IHotelBMSService _bmsService)
        {
            bmsService = _bmsService;
        }

        [HttpPost("dbReset")]
        [SwaggerOperation(
            Summary = "Resets the database to a blank state, removing all Hotels, Rooms and Bookings"
        )]
        public IActionResult Reset()
        {
            bmsService.ResetDatabase();
            return Ok("Database has been reset.");
        }

        [HttpPost("dbSeed")]
        [SwaggerOperation(
            Summary = "Reseed the database back to default setting with standard test data"
        )]
        public IActionResult Seed()
        {
            bmsService.ReseedDatabase();
            return Ok("Database has been reseeded with test data");
        }

    }
}
