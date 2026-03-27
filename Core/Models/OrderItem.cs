using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Himchistka.Core.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;

    public int ServiceId { get; set; }
    public virtual Service Service { get; set; } = null!;

    [Range(1, 50)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [MaxLength(500)]
    public string? ItemDescription { get; set; }

    [MaxLength(40)]
    public string? Color { get; set; }

    [MaxLength(60)]
    public string? Fabric { get; set; }

    [MaxLength(400)]
    public string? StainDescription { get; set; }

    public bool IsUrgent { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UrgentExtra { get; set; }

    [NotMapped]
    public decimal TotalPrice => (UnitPrice * Quantity) + UrgentExtra;
}
