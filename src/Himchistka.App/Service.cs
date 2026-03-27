namespace Himchistka.App;

public sealed record Service(
    string Name,
    decimal PriceRub,
    TimeSpan EstimatedProcessing);
