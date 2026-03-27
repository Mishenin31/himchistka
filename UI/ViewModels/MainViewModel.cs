using System.Windows.Input;
using Himchistka.Core.Enums;
using Himchistka.Core.Models;

namespace Himchistka.UI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private User? _currentUser;
    private ViewModelBase? _currentPage;

    public User? CurrentUser
    {
        get => _currentUser;
        set => Set(ref _currentUser, value);
    }

    public ViewModelBase? CurrentPage
    {
        get => _currentPage;
        set => Set(ref _currentPage, value);
    }

    public ICommand NavigateOrdersCommand { get; }
    public ICommand NavigateClientsCommand { get; }
    public ICommand NavigateServicesCommand { get; }
    public ICommand NavigateReportsCommand { get; }
    public ICommand NavigateAdminCommand { get; }

    public MainViewModel()
    {
        NavigateOrdersCommand = new RelayCommand(_ => { });
        NavigateClientsCommand = new RelayCommand(_ => { });
        NavigateServicesCommand = new RelayCommand(_ => { });
        NavigateReportsCommand = new RelayCommand(_ => { }, _ => HasAccess(UserRole.Manager));
        NavigateAdminCommand = new RelayCommand(_ => { }, _ => HasAccess(UserRole.Administrator));
    }

    public bool HasAccess(UserRole requiredRole) => CurrentUser is not null && CurrentUser.Role >= requiredRole;
}
