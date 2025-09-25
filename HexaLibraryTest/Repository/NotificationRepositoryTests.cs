using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Implementations;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HexaLibrary.Tests
{
    public class NotificationRepositoryTests : TestBase
    {
        [Test]
        public async Task AddNotification_ShouldAddNotification()
        {
            var repo = new NotificationRepository(_context);
            var notification = new Notification
            {
                UserId = "user1",
                Message = "Test notification",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await repo.AddAsync(notification);

            var saved = await _context.Notifications.FirstOrDefaultAsync();
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved!.Message, Is.EqualTo("Test notification"));
        }


        [Test]
        public async Task DeleteNotification_ShouldRemoveNotification()
        {
            var repo = new NotificationRepository(_context);
            var notification = new Notification
            {
                UserId = "user1",
                Message = "Delete me",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await repo.AddAsync(notification);
            var id = notification.NotificationId;

            await repo.DeleteAsync(id);

            var deleted = await repo.GetByIdAsync(id);
            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task GetByUserId_ShouldReturnNotificationsForUser()
        {
            var repo = new NotificationRepository(_context);

            await repo.AddAsync(new Notification
            {
                UserId = "special-user",
                Message = "User-specific",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            var result = await repo.GetByUserIdAsync("special-user");

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("User-specific"));
        }

        [Test]
        public async Task GetUnreadByUserId_ShouldReturnOnlyUnread()
        {
            var repo = new NotificationRepository(_context);

            await repo.AddAsync(new Notification
            {
                UserId = "special-user",
                Message = "Unread message",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            await repo.AddAsync(new Notification
            {
                UserId = "special-user",
                Message = "Read message",
                IsRead = true,
                CreatedAt = DateTime.UtcNow
            });

            var result = await repo.GetUnreadByUserIdAsync("special-user");

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("Unread message"));
        }
    }
}