using Himchistka.Core.Models;
using Himchistka.Data.Database;
using Himchistka.Data.Repositories;
using Himchistka.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace Himchistka.Services;

public class PaymentService
{
    private readonly AppDbContext _context;
    private readonly OrderRepository _orders;

    public PaymentService(AppDbContext context, OrderRepository orders)
    {
        _context = context;
        _orders = orders;
    }

    public async Task<Payment> ProcessPayment(PaymentDto dto)
    {
        var order = await _orders.GetByIdAsync(dto.OrderId) ?? throw new InvalidOperationException("Заказ не найден.");
        if (dto.Amount <= 0)
            throw new InvalidOperationException("Сумма оплаты должна быть больше 0.");

        var payment = new Payment
        {
            OrderId = dto.OrderId,
            Amount = dto.Amount,
            Method = dto.Method,
            CashierId = dto.CashierId,
            PaymentDate = DateTime.UtcNow
        };

        order.PaidAmount += dto.Amount;
        _context.Payments.Add(payment);
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<DailyFinancialReport> GetDailyReport(DateTime date)
    {
        var start = date.Date;
        var end = start.AddDays(1);
        var payments = await _context.Payments.Where(x => x.PaymentDate >= start && x.PaymentDate < end).ToListAsync();
        var revenue = payments.Sum(x => x.Amount);
        var count = payments.Count;
        var avg = count == 0 ? 0 : revenue / count;
        return new DailyFinancialReport(start, revenue, count, avg);
    }
}
