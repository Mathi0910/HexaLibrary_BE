using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationController(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        // GET: api/notification/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetByUser(int userId)
        {
            try
            {
                var notifications = await _notificationRepo.GetNotificationsByUserAsync(userId);
                if (notifications == null || !notifications.Any())
                    return NotFound(new { Message = $"No notifications found for user {userId}" });

                var dtos = notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    Message = n.Message,
                    SentAt = n.SentAt,
                    Type = n.Type
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching notifications by user", Details = ex.Message });
            }
        }

        // GET: api/notification/user/{userId}/unread
        [HttpGet("user/{userId}/unread")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetUnread(int userId)
        {
            try
            {
                var notifications = await _notificationRepo.GetUnreadNotificationsAsync(userId);
                if (notifications == null || !notifications.Any())
                    return NotFound(new { Message = $"No unread notifications for user {userId}" });

                var dtos = notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    Message = n.Message,
                    SentAt = n.SentAt,
                    Type = n.Type
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching unread notifications", Details = ex.Message });
            }
        }

        // GET: api/notification/type/{type}
        [HttpGet("type/{type}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetByType(string type)
        {
            try
            {
                var notifications = await _notificationRepo.GetNotificationsByTypeAsync(type);
                if (notifications == null || !notifications.Any())
                    return NotFound(new { Message = $"No notifications of type '{type}' found" });

                var dtos = notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    Message = n.Message,
                    SentAt = n.SentAt,
                    Type = n.Type
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching notifications by type", Details = ex.Message });
            }
        }

        // POST: api/notification
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<NotificationDTO>> Create([FromBody] NotificationDTO dto)
        {
            try
            {
                if (dto == null) return BadRequest(new { Message = "Invalid notification data" });

                var notification = new Notification
                {
                    UserId = 1, // 👉 for demo, replace with dto.UserId if extended
                    Message = dto.Message,
                    SentAt = dto.SentAt,
                    Type = dto.Type
                };

                await _notificationRepo.AddAsync(notification);

                dto.NotificationId = notification.NotificationId;
                return CreatedAtAction(nameof(GetByUser), new { userId = notification.UserId }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating notification", Details = ex.Message });
            }
        }
    }
}
