using System.Collections.ObjectModel;
using System.Windows.Input;
using Himchistka.Core.Enums;
using Himchistka.Core.Models;
using Himchistka.Data.Repositories;
using Himchistka.Services;

namespace Himchistka.UI.ViewModels;

public class OrderListViewModel : ViewModelBase
{
    private readonly OrderRepository _orders;
    private readonly OrderService _orderService;
    private string _searchText = string.Empty;
    private OrderStatus? _selectedStatus;
    private DateTime? _startDate;
    private DateTime? _endDate;

    public ObservableCollection<Order> Orders { get; } = new();

    public string SearchText { get => _searchText; set => Set(ref _searchText, value); }
    public OrderStatus? SelectedStatus { get => _selectedStatus; set => Set(ref _selectedStatus, value); }
    public DateTime? StartDate { get => _startDate; set => Set(ref _startDate, value); }
    public DateTime? EndDate { get => _endDate; set => Set(ref _endDate, value); }

    public ICommand AcceptOrderCommand { get; }
    public ICommand ReadyOrderCommand { get; }
    public ICommand IssueOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public ICommand FilterCommand { get; }

    public OrderListViewModel(OrderRepository orders, OrderService orderService)
    {
        _orders = orders;
        _orderService = orderService;

        AcceptOrderCommand = new RelayCommand(async id => await _orderService.AcceptOrder((int)id!, 1));
        ReadyOrderCommand = new RelayCommand(async id => await _orderService.MarkAsReady((int)id!));
        IssueOrderCommand = new RelayCommand(async id => await _orderService.IssueOrder((int)id!));
        CancelOrderCommand = new RelayCommand(async id => await _orderService.CancelOrder((int)id!, "Отменено пользователем"));
        FilterCommand = new RelayCommand(async _ => await Load());
    }

    public async Task Load()
    {
        var all = await _orders.GetAllAsync();
        var filtered = all.AsEnumerable();

        if (SelectedStatus.HasValue)
            filtered = filtered.Where(x => x.Status == SelectedStatus.Value);

        if (StartDate.HasValue)
            filtered = filtered.Where(x => x.CreatedAt >= StartDate.Value);

        if (EndDate.HasValue)
            filtered = filtered.Where(x => x.CreatedAt <= EndDate.Value);

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(x => x.Id.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                                        || x.Client.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        Orders.Clear();
        foreach (var order in filtered)
            Orders.Add(order);
    }
}
