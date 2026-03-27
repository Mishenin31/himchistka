using Himchistka.App;

var services = new List<Service>
{
    new("Рубашка", 350, TimeSpan.FromHours(24)),
    new("Пальто", 1300, TimeSpan.FromHours(48)),
    new("Костюм", 1600, TimeSpan.FromHours(72)),
    new("Платье", 1200, TimeSpan.FromHours(48))
};

Console.WriteLine("Химчистка — стартовый проект для Visual Studio");
Console.WriteLine("Доступные услуги:");

foreach (var service in services)
{
    Console.WriteLine($"- {service.Name}: {service.PriceRub} ₽, срок {service.EstimatedProcessing.TotalHours:0} ч.");
}

Console.WriteLine();
Console.WriteLine("Откройте Himchistka.sln в Visual Studio и запускайте проект через F5.");
