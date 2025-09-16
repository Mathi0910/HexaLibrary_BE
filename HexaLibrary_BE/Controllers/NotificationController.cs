using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ✅ Require authentication by default
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationController(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        // ✅ Only Admin & Librarian can see all notifications
        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetAll()
        {
            try
            {
                var notifications = await _notificationRepo.GetAllAsync();
                var dto = notifications.Select(x => new NotificationDTO
                {
                    NotificationId = x.NotificationId,
                    UserId = x.UserId,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin & Librarian can view by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            try
            {
                var notification = await _notificationRepo.GetByIdAsync(id);
                if (notification == null) return NotFound($"Notification with ID {id} not found");

                var dto = new NotificationDTO
                {
                    NotificationId = notification.NotificationId,
                    UserId = notification.UserId,
                    Message = notification.Message,
                    IsRead = notification.IsRead,
                    CreatedAt = notification.CreatedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can create notifications
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(NotificationDTO dto)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = dto.UserId,
                    Message = dto.Message,
                    IsRead = dto.IsRead,
                    CreatedAt = dto.CreatedAt
                };

                await _notificationRepo.AddAsync(notification);
                var responseDto = new NotificationDTO
                {
                    NotificationId = notification.NotificationId,
                    UserId = notification.UserId,
                    Message = notification.Message,
                    IsRead = notification.IsRead,
                    CreatedAt = notification.CreatedAt
                };
                return CreatedAtAction(nameof(GetById), new { id = notification.NotificationId }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can update notifications
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, NotificationDTO dto)
        {
            try
            {
                var existing = await _notificationRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Notification with ID {id} not found");

                existing.Message = dto.Message;
                existing.IsRead = dto.IsRead;

                await _notificationRepo.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can delete notifications
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _notificationRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Notification with ID {id} not found");

                await _notificationRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Users can only see their own notifications
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetByUserId(string userId)
        {
            try
            {
                if (!User.IsInRole("Admin") && !User.IsInRole("Librarian"))
                {
                    var currentUserId = User?.Identity?.Name;
                    if (currentUserId != userId) return Forbid();
                }

                var notifications = await _notificationRepo.GetByUserIdAsync(userId);
                var dto = notifications.Select(x => new NotificationDTO
                {
                    NotificationId = x.NotificationId,
                    UserId = x.UserId,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Users can only see their own unread notifications
        [HttpGet("user/{userId}/unread")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetUnreadByUserId(string userId)
        {
            try
            {
                if (!User.IsInRole("Admin") && !User.IsInRole("Librarian"))
                {
                    var currentUserId = User?.Identity?.Name;
                    if (currentUserId != userId) return Forbid();
                }

                var notifications = await _notificationRepo.GetUnreadByUserIdAsync(userId);
                var dto = notifications.Select(x => new NotificationDTO
                {
                    NotificationId = x.NotificationId,
                    UserId = x.UserId,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
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
