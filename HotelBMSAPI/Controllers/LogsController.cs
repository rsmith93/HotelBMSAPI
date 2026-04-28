using HotelBMSData.Context;
using HotelBMSData.Entities;
using HotelBMSModels.LogModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelBMSAPI.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : Controller
    {
        private readonly LogsContext context;

        public LogsController(LogsContext _context)
        {
            context = _context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Allows API logs to be queried and retreived."
        )]
        public async Task<ActionResult<List<LogDTO>>> GetLogs(
            [FromQuery] LogSearchDTO searchModel,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = @"
                SELECT Timestamp, Level, RenderedMessage, Exception
                FROM Logs
                WHERE 1 = 1";

            var parameters = new List<object>();
            int paramIndex = 0;

            if (searchModel != null)
            {
                if (!string.IsNullOrWhiteSpace(searchModel.Level))
                {
                    query += $" AND Level LIKE @p{paramIndex}";
                    parameters.Add($"%{searchModel.Level}%");
                    paramIndex++;
                }

                if (searchModel.From.HasValue)
                {
                    query += $" AND Timestamp >= @p{paramIndex}";
                    parameters.Add(searchModel.From.Value);
                    paramIndex++;
                }

                if (searchModel.To.HasValue)
                {
                    query += $" AND Timestamp <= @p{paramIndex}";
                    parameters.Add(searchModel.To.Value);
                    paramIndex++;
                }
            }

            query += $" ORDER BY Timestamp DESC LIMIT @p{paramIndex} OFFSET @p{paramIndex + 1}";
            parameters.Add(pageSize);
            parameters.Add((page - 1) * pageSize);

            var logs = await context.Logs
                .FromSqlRaw(query, parameters.ToArray()).Select(x => new LogDTO()
                {
                    Exception = x.Exception,
                    Level = x.Level,
                    Message = x.RenderedMessage,
                    Timestamp = x.Timestamp
                })
                .ToListAsync();

            return Ok(logs);
        }
    }
}
