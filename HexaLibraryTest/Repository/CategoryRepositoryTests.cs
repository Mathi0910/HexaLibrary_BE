using HexaLibrary_BE.Models;
using HexaLibrary_BE.Repository.Implementations;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HexaLibrary.Tests
{
    public class CategoryRepositoryTests : TestBase
    {
        [Test]
        public async Task AddCategory_ShouldAddCategory()
        {
            var repo = new CategoryRepository(_context);
            var category = new Category { Name = "Programming" };

            await repo.AddAsync(category);

            Assert.That(_context.Categories.Count(), Is.EqualTo(1));
            Assert.That(_context.Categories.First().Name, Is.EqualTo("Programming"));
        }

        [Test]
        public async Task GetById_ShouldReturnCategory()
        {
            var repo = new CategoryRepository(_context);
            var category = new Category { Name = "Databases" };

            await repo.AddAsync(category);
            await _context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(category.CategoryId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Databases"));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllCategories()
        {
            var repo = new CategoryRepository(_context);
            await repo.AddAsync(new Category { Name = "Cloud" });
            await repo.AddAsync(new Category { Name = "AI" });

            var result = await repo.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(c => c.Name == "Cloud"), Is.True);
            Assert.That(result.Any(c => c.Name == "AI"), Is.True);
        }

        [Test]
        public async Task UpdateCategory_ShouldModifyCategory()
        {
            var repo = new CategoryRepository(_context);
            var category = new Category { Name = "Networks" };

            await repo.AddAsync(category);
            category.Name = "Computer Networks";

            await repo.UpdateAsync(category);

            var updated = await repo.GetByIdAsync(category.CategoryId);
            Assert.That(updated!.Name, Is.EqualTo("Computer Networks"));
        }

        [Test]
        public async Task DeleteCategory_ShouldRemoveCategory()
        {
            var repo = new CategoryRepository(_context);
            var category = new Category { Name = "Security" };

            await repo.AddAsync(category);
            var categoryId = category.CategoryId;

            await repo.DeleteAsync(categoryId);

            var deleted = await repo.GetByIdAsync(categoryId);
            Assert.That(deleted, Is.Null);
        }
    }
}