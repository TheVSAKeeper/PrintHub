﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.PagedListCore;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.Update;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;

namespace PrintHub.WPF.Pages.Client;

public class ClientViewModel : ViewModelBase
{
    private readonly AuthenticationStore _authenticationStore;
    private readonly IMediator _mediator;

    private ICommand? _deleteOrderCommand;
    private ICommand? _loadOrdersCommand;

    private ObservableCollection<OrderViewModel>? _orders;

    public ClientViewModel(
        AuthenticationStore authenticationStore,
        IMediator mediator,
        ICallbackNavigationService<OrderViewModel> order,
        NavigationService<ClientViewModel> back,
        ParameterNavigationService<Guid, NavigateCommand, OrderUpdateFormViewModel> orderUpdateNavigationService)
    {
        _authenticationStore = authenticationStore;
        _mediator = mediator;

        CreateOrderCommand = new CallbackNavigateCommand<OrderViewModel>(order, OnOrderCreated);
        UpdateOrderNavigateCommand = new ParameterBackNavigateCommand<Guid>(orderUpdateNavigationService, new NavigateCommand(back));

        IsAdministrator = authenticationStore.IsInRole(AppData.SystemAdministratorRoleName);
    }

    public ICommand UpdateOrderNavigateCommand { get; }
    public bool IsAdministrator { get; set; }

    public ObservableCollection<OrderViewModel>? Orders
    {
        get => _orders;
        private set => Set(ref _orders, value);
    }

    public IEnumerable<string> StatusList => Enum.GetNames(typeof(OrderStatus)).Prepend(string.Empty);

    public ICommand CreateOrderCommand { get; }

    public ICommand LoadOrdersCommand => _loadOrdersCommand ??= new LambdaCommandAsync(async () =>
    {
        if (_authenticationStore.User is { ClientId: null })
        {
            MaterialMessageBox.ShowError("Client is null", "Create order error");
            return;
        }

        Operation<IPagedList<OrderViewModel>, string> result =
            await _mediator.Send(new GetOrderPaged.Request(0, 999, _authenticationStore.User!.ClientId.ToString()));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        Orders = new ObservableCollection<OrderViewModel>(result.Result.Items.OrderByDescending(model => model.UpdatedAt));
    });

    public ICommand DeleteOrderCommand => _deleteOrderCommand ??= new LambdaCommandAsync(async id =>
        {
            if (id is null)
            {
                MaterialMessageBox.ShowError("id is null");
                return;
            }

            MessageBoxResult messageBoxResult = MaterialMessageBox.ShowWithCancel("Are you sure you want to delete this item?",
                "Delete Confirmation");

            if (messageBoxResult == MessageBoxResult.Cancel)
                return;

            Operation<OrderViewModel, string> result = await _mediator.Send(new DeleteOrder.Request((Guid)id));

            if (result.Ok == false)
            {
                MessageBox.Show(result.Error, "Error");
                return;
            }

            MaterialMessageBox.Show(result.Result.ToString(), "Order deleted");
            LoadOrdersCommand.Execute(null);
        },
        () => true);

    private void OnOrderCreated(OrderViewModel obj)
    {
        List<OrderViewModel> old = [obj];

        if (Orders != null)
            old.AddRange(Orders);

        Orders = new ObservableCollection<OrderViewModel>(old);
    }
}