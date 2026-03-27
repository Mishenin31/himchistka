using Himchistka.Services.Dto;

namespace Himchistka.Services;

public class ValidationService
{
    public void ValidateOrder(OrderDto dto)
    {
        if (dto.ClientId <= 0)
            throw new InvalidOperationException("Не выбран клиент.");

        if (dto.Items.Count == 0)
            throw new InvalidOperationException("Добавьте хотя бы одну позицию.");

        if (dto.Items.Sum(x => x.Quantity) > 50)
            throw new InvalidOperationException("Нельзя принять больше 50 вещей за раз.");
    }
}
