using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Admin-only
    public class ActionLogController : ControllerBase
    {
        private readonly IActionLogRepository _logRepo;

        public ActionLogController(IActionLogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        // GET: api/actionlog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActionLog>>> GetAll()
        {
            try
            {
                var logs = await _logRepo.GetAllAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching logs", Details = ex.Message });
            }
        }

        // GET: api/actionlog/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ActionLog>>> GetByUser(int userId)
        {
            try
            {
                var logs = await _logRepo.GetLogsByUserAsync(userId);
                if (logs == null || !logs.Any())
                    return NotFound(new { Message = $"No logs found for user {userId}" });

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching user logs", Details = ex.Message });
            }
        }

        // GET: api/actionlog/date/{date}
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<ActionLog>>> GetByDate(DateTime date)
        {
            try
            {
                var logs = await _logRepo.GetLogsByDateAsync(date);
                if (logs == null || !logs.Any())
                    return NotFound(new { Message = $"No logs found for date {date:yyyy-MM-dd}" });

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching logs by date", Details = ex.Message });
            }
        }

        // GET: api/actionlog/recent/{count}
        [HttpGet("recent/{count}")]
        public async Task<ActionResult<IEnumerable<ActionLog>>> GetRecent(int count)
        {
            try
            {
                var logs = await _logRepo.GetRecentLogsAsync(count);
                if (logs == null || !logs.Any())
                    return NotFound(new { Message = $"No recent logs available (Count: {count})" });

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching recent logs", Details = ex.Message });
            }
        }
    }
}
