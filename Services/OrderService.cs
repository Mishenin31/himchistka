using Himchistka.Core.Enums;
using Himchistka.Core.Models;
using Himchistka.Data.Repositories;
using Himchistka.Services.Dto;

namespace Himchistka.Services;

public class OrderService
{
    private readonly OrderRepository _orders;
    private readonly UserRepository _users;
    private readonly ServiceRepository _services;
    private readonly ValidationService _validation;

    public OrderService(OrderRepository orders, UserRepository users, ServiceRepository services, ValidationService validation)
    {
        _orders = orders;
        _users = users;
        _services = services;
        _validation = validation;
    }

    public async Task<Order> CreateOrder(OrderDto dto)
    {
        _validation.ValidateOrder(dto);
        var client = await _users.GetByIdAsync(dto.ClientId) ?? throw new InvalidOperationException("Клиент не найден.");

        var order = new Order
        {
            ClientId = dto.ClientId,
            CashierId = dto.CashierId,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Accepted
        };

        foreach (var itemDto in dto.Items)
        {
            var service = await _services.GetByIdAsync(itemDto.ServiceId) ?? throw new InvalidOperationException("Услуга не найдена.");
            var urgentExtra = itemDto.IsUrgent ? service.BasePrice * itemDto.Quantity * 0.5m : 0m;
            order.OrderItems.Add(new OrderItem
            {
                ServiceId = itemDto.ServiceId,
                Quantity = itemDto.Quantity,
                UnitPrice = service.BasePrice,
                ItemDescription = itemDto.ItemDescription,
                Color = itemDto.Color,
                Fabric = itemDto.Fabric,
                StainDescription = itemDto.StainDescription,
                IsUrgent = itemDto.IsUrgent,
                UrgentExtra = urgentExtra
            });
        }

        order.TotalAmount = CalculateTotal(order);
        var discountValue = CalculateDiscount(client, order.TotalAmount);
        order.Discount = order.TotalAmount == 0 ? 0 : discountValue / order.TotalAmount;
        order.TotalAmount -= discountValue;

        await _orders.AddAsync(order);
        await _orders.SaveChangesAsync();
        return order;
    }

    public decimal CalculateTotal(Order order) => order.OrderItems.Sum(x => x.TotalPrice);

    public decimal CalculateDiscount(User client, decimal total)
    {
        decimal discountRate = 0;

        if (client.BirthDate.HasValue && (DateTime.Today.Year - client.BirthDate.Value.Year) >= 60)
            discountRate = Math.Max(discountRate, 0.10m);

        if (client.OrdersCount > 10)
            discountRate = Math.Max(discountRate, Math.Min(0.20m, 0.05m + (client.OrdersCount - 10) * 0.01m));

        return total * discountRate;
    }

    public async Task AcceptOrder(int orderId, int cashierId)
    {
        var order = await _orders.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Заказ не найден.");
        order.CashierId = cashierId;
        order.AcceptanceDate = DateTime.UtcNow;
        order.Status = OrderStatus.InProgress;
        _orders.Update(order);
        await _orders.SaveChangesAsync();
    }

    public async Task MarkAsReady(int orderId)
    {
        var order = await _orders.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Заказ не найден.");
        order.Status = OrderStatus.Ready;
        order.ReadyDate = DateTime.UtcNow;
        _orders.Update(order);
        await _orders.SaveChangesAsync();
    }

    public async Task IssueOrder(int orderId)
    {
        var order = await _orders.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Заказ не найден.");
        if (order.PaidAmount < order.TotalAmount)
            throw new InvalidOperationException("Нельзя выдать неоплаченный заказ.");

        order.Status = OrderStatus.Issued;
        order.IssueDate = DateTime.UtcNow;
        _orders.Update(order);
        await _orders.SaveChangesAsync();
    }

    public async Task CancelOrder(int orderId, string reason)
    {
        var order = await _orders.GetByIdAsync(orderId) ?? throw new InvalidOperationException("Заказ не найден.");
        order.Status = OrderStatus.Cancelled;
        order.Notes = $"{order.Notes}\nОтмена: {reason}";
        _orders.Update(order);
        await _orders.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Order>> GetUrgentOrders()
    {
        var acceptedOrders = await _orders.GetByStatus(OrderStatus.Accepted);
        return acceptedOrders.Where(x => x.OrderItems.Any(i => i.IsUrgent)).ToList();
    }
}
