using System.ComponentModel.DataAnnotations;
using Himchistka.Core.Enums;

namespace Himchistka.Core.Models;

public class Discount
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DiscountType Type { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Value { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [MaxLength(120)]
    public string? Condition { get; set; }
}
