using System.Collections.ObjectModel;
using System.Windows.Input;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;
using PrintHub.WPF.Endpoints.OrderEndpoints;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Pages.Login;
using PrintHub.WPF.Pages.Profile;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Pages.Client;

public class ClientViewModel : ViewModelBase
{
    private readonly AuthenticationManager _authenticationManager;

    private ObservableCollection<OrderViewModel>? _orders;

    public ClientViewModel(
        AuthenticationManager authenticationManager,
        NavigationService<ProfileViewModel> profileNavigationService,
        NavigationService<LoginViewModel> loginNavigationService,
        ICallbackNavigationService<OrderViewModel> order,
        OrderCreateFormViewModel orderCreateFormViewModel)
    {
        _authenticationManager = authenticationManager;
        NavigateProfileCommand = new NavigateCommand(profileNavigationService);
        LogoutCommand = new LogoutCommand(authenticationManager, loginNavigationService);

        OrderCreateFormViewModel = orderCreateFormViewModel;
        CreateOrderCommand = new CallbackNavigateCommand<OrderViewModel>(order, OnOrderCreated);
    }

    public ObservableCollection<OrderViewModel>? Orders
    {
        get => _orders;
        set => Set(ref _orders, value);
    }

    public ICommand NavigateProfileCommand { get; }
    public ICommand CreateOrderCommand { get; }
    public ICommand LogoutCommand { get; }
    public OrderCreateFormViewModel OrderCreateFormViewModel { get; }
    public string Username => _authenticationManager.Username;

    private void OnOrderCreated(OrderViewModel obj)
    {
        List<OrderViewModel> old = [obj];

        if (Orders != null)
            old.AddRange(Orders);

        Orders = new ObservableCollection<OrderViewModel>(old);
    }
}