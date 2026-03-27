using System.ComponentModel.DataAnnotations;
using Himchistka.Core.Enums;

namespace Himchistka.Core.Models;

public class Order
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public virtual User Client { get; set; } = null!;

    public int? CashierId { get; set; }
    public virtual User? Cashier { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? AcceptanceDate { get; set; }
    public DateTime? ReadyDate { get; set; }
    public DateTime? IssueDate { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Accepted;

    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PaidAmount { get; set; }

    [Range(0, 1)]
    public decimal Discount { get; set; }

    [MaxLength(200)]
    public string? DiscountReason { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
