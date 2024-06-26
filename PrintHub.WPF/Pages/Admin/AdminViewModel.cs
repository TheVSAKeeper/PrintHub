﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Calabonga.PagedListCore;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.Update;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.MaterialMessageBox;

namespace PrintHub.WPF.Pages.Admin;

public class AdminViewModel : ViewModelBase
{
    private readonly AuthenticationStore _authenticationStore;
    private readonly IMediator _mediator;

    private ICommand? _deleteOrderCommand;
    private ICommand? _loadOrdersCommand;

    private ObservableCollection<OrderViewModel>? _orders;

    public AdminViewModel(
        AuthenticationStore authenticationStore,
        IMediator mediator,
        NavigationService<AdminViewModel> back,
        ParameterNavigationService<Guid, NavigateCommand, OrderUpdateFormViewModel> orderUpdateNavigationService
    )
    {
        _authenticationStore = authenticationStore;
        _mediator = mediator;

        UpdateOrderNavigateCommand = new ParameterBackNavigateCommand<Guid>(orderUpdateNavigationService, new NavigateCommand(back));
    }

    public ICommand UpdateOrderNavigateCommand { get; }

    public ObservableCollection<OrderViewModel>? Orders
    {
        get => _orders;
        set => Set(ref _orders, value);
    }

    public ICommand LoadOrdersCommand => _loadOrdersCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<OrderViewModel>, string> result =
            await _mediator.Send(new GetOrderPaged.Request(0, 999, null));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        Orders = new ObservableCollection<OrderViewModel>(result.Result.Items.OrderByDescending(model => model.UpdatedAt));
    });

    private void OnOrderCreated(OrderViewModel obj)
    {
        List<OrderViewModel> old = [obj];

        if (Orders != null)
            old.AddRange(Orders);

        Orders = new ObservableCollection<OrderViewModel>(old);
    }
}