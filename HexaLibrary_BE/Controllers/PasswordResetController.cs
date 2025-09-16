using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetRepository _resetRepo;

        public PasswordResetController(IPasswordResetRepository resetRepo)
        {
            _resetRepo = resetRepo;
        }

        // POST: api/passwordreset/request
        [HttpPost("request")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestReset(int userId)
        {
            try
            {
                // Invalidate old tokens
                await _resetRepo.InvalidateOldTokensAsync(userId);

                var reset = new PasswordReset
                {
                    UserId = userId,
                    Token = Guid.NewGuid().ToString(),
                    ExpiresAt = DateTime.Now.AddMinutes(30),
                    Used = false
                };

                await _resetRepo.AddAsync(reset);

                return Ok(new { Message = "Password reset token generated", Token = reset.Token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error generating reset token", Details = ex.Message });
            }
        }

        // GET: api/passwordreset/verify
        [HttpGet("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyToken(int userId, string token)
        {
            try
            {
                var activeToken = await _resetRepo.GetActiveResetTokenAsync(userId);
                if (activeToken == null || activeToken.Token != token)
                    return BadRequest(new { Message = "Invalid or expired token" });

                return Ok(new { Message = "Token is valid" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error verifying token", Details = ex.Message });
            }
        }

        // POST: api/passwordreset/complete
        [HttpPost("complete")]
        [AllowAnonymous]
        public async Task<IActionResult> CompleteReset(int userId, string token)
        {
            try
            {
                var activeToken = await _resetRepo.GetActiveResetTokenAsync(userId);
                if (activeToken == null || activeToken.Token != token)
                    return BadRequest(new { Message = "Invalid or expired token" });

                activeToken.Used = true;
                await _resetRepo.UpdateAsync(activeToken);

                return Ok(new { Message = "Password reset completed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error completing password reset", Details = ex.Message });
            }
        }
    }
}
