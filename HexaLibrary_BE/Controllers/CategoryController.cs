using HexaLibrary_BE.DTOs;
using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HexaLibrary_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // GET: api/category
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            try
            {
                var categories = await _categoryRepo.GetAllAsync();
                var dtos = categories.Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching categories", Details = ex.Message });
            }
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category == null) return NotFound(new { Message = "Category not found" });

                var dto = new CategoryDTO
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching category by ID", Details = ex.Message });
            }
        }

        // POST: api/category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> Create([FromBody] CategoryDTO dto)
        {
            try
            {
                if (dto == null) return BadRequest(new { Message = "Invalid category data" });

                var category = new Category
                {
                    CategoryName = dto.CategoryName
                };

                await _categoryRepo.AddAsync(category);

                dto.CategoryId = category.CategoryId;
                return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating category", Details = ex.Message });
            }
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO dto)
        {
            try
            {
                var existing = await _categoryRepo.GetByIdAsync(id);
                if (existing == null) return NotFound(new { Message = "Category not found" });

                existing.CategoryName = dto.CategoryName;
                await _categoryRepo.UpdateAsync(existing);

                return Ok(new { Message = "Category updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating category", Details = ex.Message });
            }
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _categoryRepo.GetByIdAsync(id);
                if (existing == null) return NotFound(new { Message = "Category not found" });

                await _categoryRepo.DeleteAsync(id);
                return Ok(new { Message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting category", Details = ex.Message });
            }
        }
    }
}
