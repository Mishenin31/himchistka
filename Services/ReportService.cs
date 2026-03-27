using Himchistka.Core.Enums;
using Himchistka.Data.Database;
using Himchistka.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace Himchistka.Services;

public class ReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DailyReport> GenerateDailyReport()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var orders = await _context.Orders.Where(x => x.CreatedAt >= today && x.CreatedAt < tomorrow).ToListAsync();
        return new DailyReport(today, orders.Count, orders.Count(x => x.Status == OrderStatus.Ready), orders.Count(x => x.Status == OrderStatus.Issued));
    }

    public async Task<MonthlyReport> GenerateMonthlyReport()
    {
        var now = DateTime.Today;
        var start = new DateTime(now.Year, now.Month, 1);
        var end = start.AddMonths(1);

        var orders = await _context.Orders.Where(x => x.CreatedAt >= start && x.CreatedAt < end).ToListAsync();
        var revenue = await _context.Payments.Where(x => x.PaymentDate >= start && x.PaymentDate < end).SumAsync(x => x.Amount);
        return new MonthlyReport(start.Year, start.Month, orders.Count, revenue);
    }

    public async Task<IReadOnlyList<PopularServiceReport>> GeneratePopularServicesReport()
    {
        return await _context.OrderItems
            .Include(x => x.Service)
            .GroupBy(x => x.Service.Name)
            .Select(x => new PopularServiceReport(x.Key, x.Sum(i => i.Quantity)))
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ClientReport>> GenerateClientReport()
    {
        return await _context.Users
            .Where(x => x.Role == UserRole.Client)
            .Select(x => new ClientReport(x.FullName, x.OrdersCount, x.TotalSpent, (int)Math.Floor(x.TotalSpent / 100)))
            .OrderByDescending(x => x.TotalSpent)
            .ToListAsync();
    }
}
