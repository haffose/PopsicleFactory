using API.Models.DTOs.PopsicleDTOs;
using API.Repositories;
using API.Services;
using Xunit;

namespace API.Tests
{
    public class PopsicleServiceTests
    {
        private readonly PopsicleRepository PopsicleRepository;
        private readonly PopsicleService PopsicleService;

        public PopsicleServiceTests()
        {
            PopsicleRepository = new PopsicleRepository();
            PopsicleService = new PopsicleService(PopsicleRepository);
        }

        [Fact]
        public async Task CreatePopsicleAsync_ValidData_ReturnsPopsicleViewModel()
        {
            var createDto = new CreatePopsicleDto
            {
                Name = "Test Popsicle",
                Flavor = "Vanilla",
                Price = 2.99m,
                Description = "A test popsicle",
                Quantity = 10
            };

            var result = await PopsicleService.CreatePopsicleAsync(createDto);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Flavor, result.Flavor);
            Assert.Equal(createDto.Price, result.Price);
        }

        [Fact]
        public async Task GetPopsicleByIdAsync_ExistingId_ReturnsPopsicle()
        {
            var createDto = new CreatePopsicleDto
            {
                Name = "Test Popsicle",
                Flavor = "Vanilla",
                Price = 2.99m,
                Quantity = 10
            };
            var created = await PopsicleService.CreatePopsicleAsync(createDto);

            var result = await PopsicleService.GetPopsicleByIdAsync(created.Id);

            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal(createDto.Name, result.Name);
        }

        [Fact]
        public async Task GetPopsicleByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await PopsicleService.GetPopsicleByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task SearchPopsiclesAsync_ByName_ReturnsMatchingPopsicles()
        {
            var searchCriteria = new PopsicleSearchDto { Name = "Vanilla" };

            var result = await PopsicleService.SearchPopsiclesAsync(searchCriteria);

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.All(result, p => Assert.Contains("Vanilla", p.Name, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task PartialUpdatePopsicleAsync_ValidData_UpdatesOnlySpecifiedFields()
        {
            var createDto = new CreatePopsicleDto
            {
                Name = "Original Name",
                Flavor = "Original Flavor",
                Price = 1.99m,
                Quantity = 5
            };
            var created = await PopsicleService.CreatePopsicleAsync(createDto);

            var updateDto = new UpdatePopsicleDto
            {
                Name = "Updated Name",
                Price = 3.99m
            };

            var result = await PopsicleService.PartialUpdatePopsicleAsync(created.Id, updateDto);

            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal(3.99m, result.Price);
            Assert.Equal("Original Flavor", result.Flavor);
            Assert.Equal(5, result.Quantity);
        }
    }
}