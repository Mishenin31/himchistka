using System.ComponentModel.DataAnnotations;
using Himchistka.Core.Enums;

namespace Himchistka.Core.Models;

public class User
{
    public int Id { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(512)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required, Phone, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    [Range(0, int.MaxValue)]
    public int OrdersCount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalSpent { get; set; }

    public DateTime? BirthDate { get; set; }

    public virtual ICollection<Order> ClientOrders { get; set; } = new List<Order>();
    public virtual ICollection<Order> CashierOrders { get; set; } = new List<Order>();
}
