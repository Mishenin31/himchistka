using Himchistka.Core.Enums;
using Himchistka.Core.Models;
using Himchistka.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Himchistka.Data.Repositories;

public class OrderRepository : Repository<Order>
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Order>> GetByClient(int clientId) =>
        await Context.Orders.Include(x => x.OrderItems).Where(x => x.ClientId == clientId).ToListAsync();

    public async Task<IReadOnlyList<Order>> GetByStatus(OrderStatus status) =>
        await Context.Orders.Include(x => x.Client).Where(x => x.Status == status).ToListAsync();

    public async Task<IReadOnlyList<Order>> GetByDateRange(DateTime start, DateTime end) =>
        await Context.Orders.Include(x => x.Client)
            .Where(x => x.CreatedAt >= start && x.CreatedAt <= end)
            .ToListAsync();

    public async Task<IReadOnlyList<Order>> GetTodayOrders()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        return await GetByDateRange(today, tomorrow);
    }

    public async Task<decimal> CalculateDailyRevenue(DateTime date)
    {
        var start = date.Date;
        var end = start.AddDays(1);
        return await Context.Payments
            .Where(x => x.PaymentDate >= start && x.PaymentDate < end)
            .SumAsync(x => x.Amount);
    }
}
