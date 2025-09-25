using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //  Require authentication for all endpoints
    public class ActionLogController : ControllerBase
    {
        private readonly IActionLogRepository _actionLogRepo;

        public ActionLogController(IActionLogRepository actionLogRepo)
        {
            _actionLogRepo = actionLogRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")] //  Only Admin & Librarian can view all logs
        public async Task<ActionResult<IEnumerable<ActionLogDTO>>> GetAll()
        {
            try
            {
                var logs = await _actionLogRepo.GetAllAsync();
                var dto = logs.Select(x => new ActionLogDTO
                {
                    LogId = x.LogId,
                    Action = x.Action,
                    ActionDate = x.ActionDate,
                    UserId = x.UserId
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Librarian")] //  Restricted to Admin/Librarian
        public async Task<ActionResult<ActionLogDTO>> GetById(int id)
        {
            try
            {
                var log = await _actionLogRepo.GetByIdAsync(id);
                if (log == null) return NotFound($"Log with ID {id} not found");

                var dto = new ActionLogDTO
                {
                    LogId = log.LogId,
                    Action = log.Action,
                    ActionDate = log.ActionDate,
                    UserId = log.UserId
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")] //  Only staff can create logs
        public async Task<ActionResult> Create(CreateActionLogDTO dto)
        {
            try
            {
                var log = new ActionLog
                {
                    UserId = dto.UserId,
                    Action = dto.Action,
                    ActionDate = DateTime.UtcNow
                };

                await _actionLogRepo.AddAsync(log);
                var responseDto = new ActionLogDTO
                {
                    LogId = log.LogId,
                    UserId = log.UserId,
                    Action = log.Action,
                    ActionDate = log.ActionDate
                };
                return CreatedAtAction(nameof(GetById), new { id = log.LogId }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] //  Only Admin can update logs
        public async Task<ActionResult> Update(int id, ActionLogDTO dto)
        {
            try
            {
                var existing = await _actionLogRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Log with ID {id} not found");

                existing.Action = dto.Action;
                existing.ActionDate = dto.ActionDate;

                await _actionLogRepo.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] //  Only Admin can delete logs
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _actionLogRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Log with ID {id} not found");

                await _actionLogRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize] //  Any authenticated user can fetch their own logs
        public async Task<ActionResult<IEnumerable<ActionLogDTO>>> GetByUserId(string userId)
        {
            try
            {
                // Restrict normal users to only their own logs
                if (!User.IsInRole("Admin") && !User.IsInRole("Librarian"))
                {
                    var currentUserId = User?.FindFirst("sub")?.Value ?? User?.Identity?.Name;
                    if (currentUserId != userId)
                        return Forbid(); //  Block access if not the same user
                }

                var logs = await _actionLogRepo.GetLogsByUserIdAsync(userId);
                var dto = logs.Select(x => new ActionLogDTO
                {
                    LogId = x.LogId,
                    Action = x.Action,
                    ActionDate = x.ActionDate,
                    UserId = x.UserId
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
