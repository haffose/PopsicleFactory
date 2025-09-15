using API.Models;
using API.Repositories;
using Xunit;

namespace API.Tests
{
    public class PopsicleRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_ValidPopsicle_AssignsIdAndReturns()
        {
            var repository = new PopsicleRepository();
            var popsicle = new Popsicle
            {
                Name = "Test Popsicle",
                Flavor = "Vanilla",
                Price = 2.99m,
                Quantity = 10
            };

            var result = await repository.CreatePopsicleAsync(popsicle);

            Assert.True(result.Id > 0);
            Assert.Equal(popsicle.Name, result.Name);
            Assert.True(result.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPopsicles()
        {
            var repository = new PopsicleRepository();

            var result = await repository.GetAllPopsiclesAsync();

            Assert.NotNull(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task ExistsAsync_ExistingId_ReturnsTrue()
        {
            var repository = new PopsicleRepository();
            var popsicle = new Popsicle { Name = "Test", Flavor = "Test", Price = 1.99m, Quantity = 1 };
            var created = await repository.CreatePopsicleAsync(popsicle);

            var exists = await repository.PopsicleExistsAsync(created.Id);

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_NonExistingId_ReturnsFalse()
        {
            var repository = new PopsicleRepository();

            var exists = await repository.PopsicleExistsAsync(999);

            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_ReturnsTrue()
        {
            var repository = new PopsicleRepository();
            var popsicle = new Popsicle { Name = "Test", Flavor = "Test", Price = 1.99m, Quantity = 1 };
            var created = await repository.CreatePopsicleAsync(popsicle);

            var deleted = await repository.DeletePopsicleAsync(created.Id);

            Assert.True(deleted);
            Assert.False(await repository.PopsicleExistsAsync(created.Id));
        }
    }
}