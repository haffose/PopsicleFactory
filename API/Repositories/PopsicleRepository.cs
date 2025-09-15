using API.Models;
using API.Models.DTOs.PopsicleDTOs;
using API.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace API.Repositories;

public class PopsicleRepository : IPopsicleRepository
{
    private readonly ConcurrentDictionary<int, Popsicle> Popsicles;
    private int Next;

    public PopsicleRepository()
    {
        Popsicles = new ConcurrentDictionary<int, Popsicle>();
        Next = 1;
        PopsicleSampleData();
    }

    public async Task<IEnumerable<Popsicle>> GetAllPopsiclesAsync()
    {
        return await Task.FromResult(Popsicles.Values.OrderBy(p => p.Id));
    }

    public async Task<Popsicle?> GetPopsicleByIdAsync(int id)
    {
        Popsicles.TryGetValue(id, out var popsicle);
        return await Task.FromResult(popsicle);
    }

    public async Task<Popsicle> CreatePopsicleAsync(Popsicle popsicle)
    {
        popsicle.Id = Next++;
        popsicle.CreatedAt = DateTime.UtcNow;
        popsicle.UpdatedAt = DateTime.UtcNow;

        Popsicles.TryAdd(popsicle.Id, popsicle);
        return await Task.FromResult(popsicle);
    }

    public async Task<Popsicle?> UpdatePopsicleAsync(int id, Popsicle popsicle)
    {
        if (!Popsicles.ContainsKey(id))
            return null;

        popsicle.Id = id;
        popsicle.UpdatedAt = DateTime.UtcNow;

        // Preserve the original creation date
        if (Popsicles.TryGetValue(id, out var existingPopsicle))
        {
            popsicle.CreatedAt = existingPopsicle.CreatedAt;
        }

        Popsicles.TryUpdate(id, popsicle, Popsicles[id]);
        return await Task.FromResult(popsicle);
    }

    public async Task<Popsicle?> PartialUpdatePopsicleAsync(int id, UpdatePopsicleDto updateDto)
    {
        if (!Popsicles.TryGetValue(id, out var existingPopsicle))
            return null;

        // Update only the fields that are provided
        if (!string.IsNullOrEmpty(updateDto.Name))
            existingPopsicle.Name = updateDto.Name;

        if (!string.IsNullOrEmpty(updateDto.Flavor))
            existingPopsicle.Flavor = updateDto.Flavor;

        if (updateDto.Price.HasValue)
            existingPopsicle.Price = updateDto.Price.Value;

        if (updateDto.Description is not null)
            existingPopsicle.Description = updateDto.Description;

        if (updateDto.Quantity.HasValue)
            existingPopsicle.Quantity = updateDto.Quantity.Value;

        existingPopsicle.UpdatedAt = DateTime.UtcNow;

        return await Task.FromResult(existingPopsicle);
    }

    public async Task<bool> DeletePopsicleAsync(int id)
    {
        var result = Popsicles.TryRemove(id, out _);
        return await Task.FromResult(result);
    }

    public async Task<bool> PopsicleExistsAsync(int id)
    {
        return await Task.FromResult(Popsicles.ContainsKey(id));
    }

    public async Task<IEnumerable<Popsicle>> SearchPopsiclesAsync(PopsicleSearchDto searchCriteria)
    {
        var query = Popsicles.Values.AsEnumerable();

        if (!string.IsNullOrEmpty(searchCriteria.Name))
        {
            query = query.Where(p => p.Name.Contains(searchCriteria.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(searchCriteria.Flavor))
        {
            query = query.Where(p => p.Flavor.Contains(searchCriteria.Flavor, StringComparison.OrdinalIgnoreCase));
        }

        if (searchCriteria.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= searchCriteria.MinPrice.Value);
        }

        if (searchCriteria.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= searchCriteria.MaxPrice.Value);
        }

        if (searchCriteria.MinQuantity.HasValue)
        {
            query = query.Where(p => p.Quantity >= searchCriteria.MinQuantity.Value);
        }

        if (searchCriteria.MaxQuantity.HasValue)
        {
            query = query.Where(p => p.Quantity <= searchCriteria.MaxQuantity.Value);
        }

        return await Task.FromResult(query.OrderBy(p => p.Id));
    }

    private void PopsicleSampleData()
    {
        var samplePopsicles = new List<Popsicle>
        {
            new() { Name = "Classic Vanilla", Flavor = "Vanilla", Price = 2.99m, Description = "Creamy vanilla popsicle", Quantity = 50 },
            new() { Name = "Strawberry Delight", Flavor = "Strawberry", Price = 3.49m, Description = "Fresh strawberry flavor", Quantity = 30 },
            new() { Name = "Chocolate Fudge", Flavor = "Chocolate", Price = 3.99m, Description = "Rich chocolate with fudge swirl", Quantity = 25 },
            new() { Name = "Orange Burst", Flavor = "Orange", Price = 2.79m, Description = "Zesty orange citrus", Quantity = 40 }
        };

        foreach (var popsicle in samplePopsicles)
        {
            CreatePopsicleAsync(popsicle).Wait();
        }
    }
}