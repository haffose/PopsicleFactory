using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Popsicle
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Flavor is required")]
    [StringLength(50, ErrorMessage = "Flavor cannot exceed 50 characters")]
    public string Flavor { get; set; } = string.Empty;

    [Range(0.01, 999.99, ErrorMessage = "Price must be between 0.01 and 999.99")]
    public decimal Price { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}