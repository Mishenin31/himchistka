using System.ComponentModel.DataAnnotations;

namespace Himchistka.Core.Models;

public class Service
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Category { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string ClothingType { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal BasePrice { get; set; }

    [Required, MaxLength(10)]
    public string Unit { get; set; } = "шт";

    [Range(1, 30)]
    public int? ProcessingDays { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(260)]
    public string? ImagePath { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
