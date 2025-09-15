using API.Models.DTOs.PopsicleDTOs;

namespace API.Services.Interfaces;

public interface IPopsicleService
{
    Task<IEnumerable<PopsicleViewModel>> GetAllPopsiclesAsync();

    Task<PopsicleViewModel?> GetPopsicleByIdAsync(int id);

    Task<PopsicleViewModel> CreatePopsicleAsync(CreatePopsicleDto createDto);

    Task<PopsicleViewModel?> UpdatePopsicleAsync(int id, CreatePopsicleDto updateDto);

    Task<PopsicleViewModel?> PartialUpdatePopsicleAsync(int id, UpdatePopsicleDto updateDto);

    Task<bool> DeletePopsicleAsync(int id);

    Task<bool> PopsicleExistsAsync(int id);

    Task<IEnumerable<PopsicleViewModel>> SearchPopsiclesAsync(PopsicleSearchDto searchCriteria);
}