using API.Models;
using API.Models.DTOs.PopsicleDTOs;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services;

public class PopsicleService : IPopsicleService
{
    private readonly IPopsicleRepository PopsicleRepository;

    public PopsicleService(IPopsicleRepository popsicleRepository)
    {
        PopsicleRepository = popsicleRepository;
    }

    public async Task<IEnumerable<PopsicleViewModel>> GetAllPopsiclesAsync()
    {
        var popsicles = await PopsicleRepository.GetAllPopsiclesAsync();
        return popsicles.Select(MapToViewModel);
    }

    public async Task<PopsicleViewModel?> GetPopsicleByIdAsync(int id)
    {
        var popsicle = await PopsicleRepository.GetPopsicleByIdAsync(id);
        return popsicle != null ? MapToViewModel(popsicle) : null;
    }

    public async Task<PopsicleViewModel> CreatePopsicleAsync(CreatePopsicleDto createDto)
    {
        var popsicle = MapToEntity(createDto);
        var createdPopsicle = await PopsicleRepository.CreatePopsicleAsync(popsicle);
        return MapToViewModel(createdPopsicle);
    }

    public async Task<PopsicleViewModel?> UpdatePopsicleAsync(int id, CreatePopsicleDto updateDto)
    {
        var popsicle = MapToEntity(updateDto);
        var updatedPopsicle = await PopsicleRepository.UpdatePopsicleAsync(id, popsicle);
        return updatedPopsicle != null ? MapToViewModel(updatedPopsicle) : null;
    }

    public async Task<PopsicleViewModel?> PartialUpdatePopsicleAsync(int id, UpdatePopsicleDto updateDto)
    {
        var updatedPopsicle = await PopsicleRepository.PartialUpdatePopsicleAsync(id, updateDto);
        return updatedPopsicle != null ? MapToViewModel(updatedPopsicle) : null;
    }

    public async Task<bool> DeletePopsicleAsync(int id)
    {
        return await PopsicleRepository.DeletePopsicleAsync(id);
    }

    public async Task<bool> PopsicleExistsAsync(int id)
    {
        return await PopsicleRepository.PopsicleExistsAsync(id);
    }

    public async Task<IEnumerable<PopsicleViewModel>> SearchPopsiclesAsync(PopsicleSearchDto searchCriteria)
    {
        var popsicles = await PopsicleRepository.SearchPopsiclesAsync(searchCriteria);
        return popsicles.Select(MapToViewModel);
    }

    private static Popsicle MapToEntity(CreatePopsicleDto dto)
    {
        return new Popsicle
        {
            Name = dto.Name,
            Flavor = dto.Flavor,
            Price = dto.Price,
            Description = dto.Description,
            Quantity = dto.Quantity
        };
    }

    private static PopsicleViewModel MapToViewModel(Popsicle popsicle)
    {
        return new PopsicleViewModel
        {
            Id = popsicle.Id,
            Name = popsicle.Name,
            Flavor = popsicle.Flavor,
            Price = popsicle.Price,
            Description = popsicle.Description,
            Quantity = popsicle.Quantity,
            CreatedAt = popsicle.CreatedAt,
            UpdatedAt = popsicle.UpdatedAt
        };
    }
}