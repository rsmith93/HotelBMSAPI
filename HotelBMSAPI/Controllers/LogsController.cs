using HotelBMSData.Context;
using HotelBMSData.Entities;
using HotelBMSModels.LogModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<List<LogEntry>>> GetLogs(
            [FromQuery] string? level,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = @"
                SELECT Timestamp, Level, RenderedMessage, Exception
                FROM Logs";

            var parameters = new List<object>();

            if (!string.IsNullOrWhiteSpace(level))
            {
                query += " AND Level = {0}";
                parameters.Add(level);
            }

            if (from.HasValue)
            {
                query += $" AND Timestamp >= {{{parameters.Count}}}";
                parameters.Add(from.Value);
            }

            if (to.HasValue)
            {
                query += $" AND Timestamp <= {{{parameters.Count}}}";
                parameters.Add(to.Value);
            }

            query += $" ORDER BY Timestamp DESC LIMIT {{{parameters.Count}}} OFFSET {{{parameters.Count + 1}}}";
            parameters.Add(pageSize);
            parameters.Add((page - 1) * pageSize);

            var logs = await context.Logs
                .FromSqlRaw(query, parameters.ToArray())
                .ToListAsync();

            return Ok(logs);
        }
    }
}
