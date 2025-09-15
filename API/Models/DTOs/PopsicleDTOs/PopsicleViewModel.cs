namespace API.Models.DTOs.PopsicleDTOs;

public class PopsicleViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Flavor { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}