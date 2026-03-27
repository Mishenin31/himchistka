using Himchistka.Core.Enums;

namespace Himchistka.Services.Dto;

public class OrderDto
{
    public int ClientId { get; set; }
    public int? CashierId { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int ServiceId { get; set; }
    public int Quantity { get; set; }
    public string? ItemDescription { get; set; }
    public string? Color { get; set; }
    public string? Fabric { get; set; }
    public string? StainDescription { get; set; }
    public bool IsUrgent { get; set; }
}

public class PaymentDto
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public int? CashierId { get; set; }
}

public record DailyFinancialReport(DateTime Date, decimal Revenue, int PaymentsCount, decimal AvgTicket);
public record DailyReport(DateTime Date, int OrdersTotal, int ReadyOrders, int IssuedOrders);
public record MonthlyReport(int Year, int Month, int OrdersCount, decimal Revenue);
public record PopularServiceReport(string ServiceName, int Count);
public record ClientReport(string FullName, int OrdersCount, decimal TotalSpent, int LoyaltyPoints);
