using HotelBMSServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBMSAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/db")]
    public class TestController : ControllerBase
    {
        private readonly IHotelBMSService bmsService;

        public TestController(IHotelBMSService _bmsService)
        {
            bmsService = _bmsService;
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            bmsService.ResetDatabase();
            return Ok("Database has been reset.");
        }

        [HttpPost("seed")]
        public IActionResult Seed()
        {
            bmsService.ReseedDatabase();
            return Ok("Database has been reseeded with test data");
        }

    }
}
