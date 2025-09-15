namespace API.Models.DTOs.PopsicleDTOs;

public class PopsicleSearchDto
{
    public string? Name { get; set; }
    public string? Flavor { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinQuantity { get; set; }
    public int? MaxQuantity { get; set; }
}