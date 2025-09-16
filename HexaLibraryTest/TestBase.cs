
using HexaLibrary_BE.Authentication;
using Microsoft.EntityFrameworkCore;


namespace HexaLibrary.Tests
{
    public class TestBase
    {
        protected ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "HexaLibraryTestDB")
                .Options;

            _context = new ApplicationDbContext(options);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}