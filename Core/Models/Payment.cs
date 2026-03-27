using System.ComponentModel.DataAnnotations;
using Himchistka.Core.Enums;

namespace Himchistka.Core.Models;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }

    public int? CashierId { get; set; }
    public virtual User? Cashier { get; set; }
}
