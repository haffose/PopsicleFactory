using API.Models;
using API.Models.DTOs.PopsicleDTOs;

namespace API.Repositories.Interfaces;

public interface IPopsicleRepository
{
    Task<IEnumerable<Popsicle>> GetAllPopsiclesAsync();

    Task<Popsicle?> GetPopsicleByIdAsync(int id);

    Task<Popsicle> CreatePopsicleAsync(Popsicle popsicle);

    Task<Popsicle?> UpdatePopsicleAsync(int id, Popsicle popsicle);

    Task<Popsicle?> PartialUpdatePopsicleAsync(int id, UpdatePopsicleDto updateDto);

    Task<bool> DeletePopsicleAsync(int id);

    Task<bool> PopsicleExistsAsync(int id);

    Task<IEnumerable<Popsicle>> SearchPopsiclesAsync(PopsicleSearchDto searchCriteria);
}