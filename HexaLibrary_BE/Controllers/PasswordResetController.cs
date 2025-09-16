using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ✅ Require authentication unless overridden
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetRepository _passwordResetRepo;

        public PasswordResetController(IPasswordResetRepository passwordResetRepo)
        {
            _passwordResetRepo = passwordResetRepo;
        }

        // ✅ Only Admin can see all reset requests
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PasswordResetDTO>>> GetAll()
        {
            try
            {
                var resets = await _passwordResetRepo.GetAllAsync();
                var dto = resets.Select(x => new PasswordResetDTO
                {
                    ResetId = x.ResetId,
                    UserId = x.UserId,
                    ResetToken = x.ResetToken,
                    ExpiryDate = x.ExpiryDate,
                    IsUsed = x.IsUsed
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can view reset by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PasswordResetDTO>> GetById(int id)
        {
            try
            {
                var reset = await _passwordResetRepo.GetByIdAsync(id);
                if (reset == null) return NotFound($"Password reset request with ID {id} not found");

                var dto = new PasswordResetDTO
                {
                    ResetId = reset.ResetId,
                    UserId = reset.UserId,
                    ResetToken = reset.ResetToken,
                    ExpiryDate = reset.ExpiryDate,
                    IsUsed = reset.IsUsed
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can create reset records (normally triggered by forgot-password workflow)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(PasswordResetDTO dto)
        {
            try
            {
                var reset = new PasswordReset
                {
                    UserId = dto.UserId,
                    ResetToken = dto.ResetToken,
                    ExpiryDate = dto.ExpiryDate,
                    IsUsed = dto.IsUsed
                };

                await _passwordResetRepo.AddAsync(reset);
                var responseDto = new PasswordResetDTO
                {
                    ResetId = reset.ResetId,
                    UserId = reset.UserId,
                    ResetToken = reset.ResetToken,
                    ExpiryDate = reset.ExpiryDate,
                    IsUsed = reset.IsUsed
                };
                return CreatedAtAction(nameof(GetById), new { id = reset.ResetId }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can update reset records
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, PasswordResetDTO dto)
        {
            try
            {
                var existing = await _passwordResetRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Password reset request with ID {id} not found");

                existing.ResetToken = dto.ResetToken;
                existing.ExpiryDate = dto.ExpiryDate;
                existing.IsUsed = dto.IsUsed;

                await _passwordResetRepo.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can delete reset records
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _passwordResetRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Password reset request with ID {id} not found");

                await _passwordResetRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Any user (even not logged in) can validate token → needed for forgot password flow
        [HttpGet("validate/{userId}/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult<PasswordResetDTO>> ValidateToken(string userId, string token)
        {
            try
            {
                var reset = await _passwordResetRepo.GetValidTokenAsync(userId, token);
                if (reset == null) return NotFound("Invalid or expired reset token");

                var dto = new PasswordResetDTO
                {
                    ResetId = reset.ResetId,
                    UserId = reset.UserId,
                    ResetToken = reset.ResetToken,
                    ExpiryDate = reset.ExpiryDate,
                    IsUsed = reset.IsUsed
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
