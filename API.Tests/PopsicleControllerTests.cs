using API.Controllers;
using API.Models.DTOs.PopsicleDTOs;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.Tests;

public class PopsicleControllerTests
{
    private readonly Mock<IPopsicleService> MockService;
    private readonly Mock<ILogger<PopsicleController>> MockLogger;
    private readonly PopsicleController PopsicleController;

    public PopsicleControllerTests()
    {
        MockService = new Mock<IPopsicleService>();
        MockLogger = new Mock<ILogger<PopsicleController>>();
        PopsicleController = new PopsicleController(MockService.Object, MockLogger.Object);
    }

    [Fact]
    public async Task GetAllPopsicles_ReturnsOkResult_WithPopsicles()
    {
        var popsicles = new List<PopsicleViewModel>
        {
            new() { Id = 1, Name = "Test Popsicle", Flavor = "Vanilla", Price = 2.99m, Quantity = 10 }
        };
        MockService.Setup(s => s.GetAllPopsiclesAsync()).ReturnsAsync(popsicles);

        var result = await PopsicleController.GetAllPopsicles();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPopsicles = Assert.IsType<List<PopsicleViewModel>>(okResult.Value);
        Assert.Single(returnedPopsicles);
    }

    [Fact]
    public async Task GetPopsicle_ExistingId_ReturnsOkResult()
    {
        var popsicle = new PopsicleViewModel { Id = 1, Name = "Test Popsicle", Flavor = "Vanilla", Price = 2.99m };
        MockService.Setup(s => s.GetPopsicleByIdAsync(1)).ReturnsAsync(popsicle);

        var result = await PopsicleController.GetPopsicle(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPopsicle = Assert.IsType<PopsicleViewModel>(okResult.Value);
        Assert.Equal(1, returnedPopsicle.Id);
    }

    [Fact]
    public async Task GetPopsicle_NonExistingId_ReturnsNotFound()
    {
        MockService.Setup(s => s.GetPopsicleByIdAsync(999)).ReturnsAsync((PopsicleViewModel?)null);

        var result = await PopsicleController.GetPopsicle(999);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreatePopsicle_ValidData_ReturnsCreatedResult()
    {
        var createDto = new CreatePopsicleDto
        {
            Name = "New Popsicle",
            Flavor = "Strawberry",
            Price = 3.49m,
            Quantity = 20
        };
        var createdPopsicle = new PopsicleViewModel
        {
            Id = 1,
            Name = createDto.Name,
            Flavor = createDto.Flavor,
            Price = createDto.Price,
            Quantity = createDto.Quantity
        };
        MockService.Setup(s => s.CreatePopsicleAsync(createDto)).ReturnsAsync(createdPopsicle);

        var result = await PopsicleController.CreatePopsicle(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedPopsicle = Assert.IsType<PopsicleViewModel>(createdResult.Value);
        Assert.Equal(createDto.Name, returnedPopsicle.Name);
    }

    [Fact]
    public async Task CreatePopsicle_InvalidData_ReturnsBadRequest()
    {
        var createDto = new CreatePopsicleDto();
        PopsicleController.ModelState.AddModelError("Name", "Name is required");

        var result = await PopsicleController.CreatePopsicle(createDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeletePopsicle_ExistingId_ReturnsNoContent()
    {
        MockService.Setup(s => s.PopsicleExistsAsync(1)).ReturnsAsync(true);
        MockService.Setup(s => s.DeletePopsicleAsync(1)).ReturnsAsync(true);

        var result = await PopsicleController.DeletePopsicle(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePopsicle_NonExistingId_ReturnsNotFound()
    {
        MockService.Setup(s => s.PopsicleExistsAsync(999)).ReturnsAsync(false);

        var result = await PopsicleController.DeletePopsicle(999);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}