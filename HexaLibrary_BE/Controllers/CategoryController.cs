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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // ✅ Allow everyone (even anonymous) to see categories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            try
            {
                var categories = await _categoryRepo.GetAllAsync();
                var dto = categories.Select(x => new CategoryDTO
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    BookCount = x.Books.Count
                }).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Allow everyone (even anonymous) to view by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category == null) return NotFound($"Category with ID {id} not found");

                var dto = new CategoryDTO
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    BookCount = category.Books.Count
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin/Librarian can create categories
        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> Create(CategoryDTO dto)
        {
            try
            {
                var category = new Category
                {
                    Name = dto.Name
                };

                await _categoryRepo.AddAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin/Librarian can update categories
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult> Update(int id, CategoryDTO dto)
        {
            try
            {
                var existing = await _categoryRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Category with ID {id} not found");

                existing.Name = dto.Name;

                await _categoryRepo.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ Only Admin can delete categories
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _categoryRepo.GetByIdAsync(id);
                if (existing == null) return NotFound($"Category with ID {id} not found");

                await _categoryRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
