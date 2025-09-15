using API.Models.DTOs.PopsicleDTOs;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PopsicleController : ControllerBase
{
    private readonly IPopsicleService PopsicleService;
    private readonly ILogger<PopsicleController> Logger;

    public PopsicleController(IPopsicleService popsicleService, ILogger<PopsicleController> logger)
    {
        PopsicleService = popsicleService;
        Logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PopsicleViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<PopsicleViewModel>>> GetAllPopsicles()
    {
        try
        {
            var popsicles = await PopsicleService.GetAllPopsiclesAsync();
            return Ok(popsicles);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while retrieving all popsicles");
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PopsicleViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PopsicleViewModel>> GetPopsicle(int id)
    {
        try
        {
            var popsicle = await PopsicleService.GetPopsicleByIdAsync(id);
            if (popsicle == null)
            {
                return NotFound(new { message = $"Popsicle with ID {id} does not exist" });
            }

            return Ok(popsicle);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while retrieving popsicle with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(PopsicleViewModel), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PopsicleViewModel>> CreatePopsicle([FromBody] CreatePopsicleDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "The popsicle request is invalid",
                errors = ModelState.SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            var popsicle = await PopsicleService.CreatePopsicleAsync(createDto);
            return CreatedAtAction(nameof(GetPopsicle), new { id = popsicle.Id }, popsicle);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while creating popsicle");
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PopsicleViewModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PopsicleViewModel>> ReplacePopsicle(int id, [FromBody] CreatePopsicleDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "The popsicle request is invalid",
                errors = ModelState.SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            var exists = await PopsicleService.PopsicleExistsAsync(id);
            if (!exists)
            {
                return NotFound(new { message = $"Popsicle with ID {id} does not exist" });
            }

            var popsicle = await PopsicleService.UpdatePopsicleAsync(id, updateDto);
            return Ok(popsicle);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while replacing popsicle with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(PopsicleViewModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PopsicleViewModel>> UpdatePopsicle(int id, [FromBody] UpdatePopsicleDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "The popsicle request is invalid",
                errors = ModelState.SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            var exists = await PopsicleService.PopsicleExistsAsync(id);
            if (!exists)
            {
                return NotFound(new { message = $"Popsicle with ID {id} does not exist" });
            }

            var popsicle = await PopsicleService.PartialUpdatePopsicleAsync(id, updateDto);
            return Ok(popsicle);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while updating popsicle with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeletePopsicle(int id)
    {
        try
        {
            var exists = await PopsicleService.PopsicleExistsAsync(id);
            if (!exists)
            {
                return NotFound(new { message = $"Popsicle with ID {id} does not exist" });
            }

            await PopsicleService.DeletePopsicleAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while deleting popsicle with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<PopsicleViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<PopsicleViewModel>>> SearchPopsicles(
        [FromQuery] string? name = null,
        [FromQuery] string? flavor = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int? minQuantity = null,
        [FromQuery] int? maxQuantity = null)
    {
        try
        {
            var searchCriteria = new PopsicleSearchDto
            {
                Name = name,
                Flavor = flavor,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinQuantity = minQuantity,
                MaxQuantity = maxQuantity
            };

            var popsicles = await PopsicleService.SearchPopsiclesAsync(searchCriteria);
            return Ok(popsicles);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while searching popsicles");
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }
}