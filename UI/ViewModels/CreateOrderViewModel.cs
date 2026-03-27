using System.Collections.ObjectModel;
using System.Windows.Input;
using Himchistka.Core.Models;
using Himchistka.Data.Repositories;
using Himchistka.Services;
using Himchistka.Services.Dto;

namespace Himchistka.UI.ViewModels;

public class CreateOrderViewModel : ViewModelBase
{
    private readonly ServiceRepository _serviceRepository;
    private readonly UserRepository _userRepository;
    private readonly OrderService _orderService;

    private User? _selectedClient;
    private decimal _total;
    private string _clientSearch = string.Empty;

    public ObservableCollection<Service> Services { get; } = new();
    public ObservableCollection<OrderItemDto> Cart { get; } = new();

    public User? SelectedClient { get => _selectedClient; set => Set(ref _selectedClient, value); }
    public string ClientSearch { get => _clientSearch; set => Set(ref _clientSearch, value); }
    public decimal Total { get => _total; set => Set(ref _total, value); }

    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand SaveOrderCommand { get; }

    public CreateOrderViewModel(ServiceRepository serviceRepository, UserRepository userRepository, OrderService orderService)
    {
        _serviceRepository = serviceRepository;
        _userRepository = userRepository;
        _orderService = orderService;

        AddItemCommand = new RelayCommand(param =>
        {
            if (param is Service service)
            {
                Cart.Add(new OrderItemDto { ServiceId = service.Id, Quantity = 1 });
                RecalculateTotal();
            }
        });

        RemoveItemCommand = new RelayCommand(param =>
        {
            if (param is OrderItemDto item)
            {
                Cart.Remove(item);
                RecalculateTotal();
            }
        });

        SaveOrderCommand = new RelayCommand(async _ => await SaveOrder());
    }

    public async Task LoadServices()
    {
        var services = await _serviceRepository.GetActiveAsync();
        Services.Clear();
        foreach (var service in services)
            Services.Add(service);
    }

    public async Task<IReadOnlyList<User>> FindClients() => await _userRepository.FindAsync(x => x.FullName.Contains(ClientSearch) || x.Phone.Contains(ClientSearch));

    public void RecalculateTotal()
    {
        var serviceIndex = Services.ToDictionary(x => x.Id);
        Total = Cart.Sum(item =>
        {
            if (!serviceIndex.TryGetValue(item.ServiceId, out var service)) return 0;
            return (service.BasePrice * item.Quantity) + (item.IsUrgent ? service.BasePrice * item.Quantity * 0.5m : 0m);
        });
    }

    private async Task SaveOrder()
    {
        if (SelectedClient is null)
            throw new InvalidOperationException("Выберите клиента");

        var dto = new OrderDto
        {
            ClientId = SelectedClient.Id,
            Items = Cart.ToList()
        };

        await _orderService.CreateOrder(dto);
    }
}
