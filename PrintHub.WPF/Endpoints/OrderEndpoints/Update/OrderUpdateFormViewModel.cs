using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.Results;
using FluentValidation;
using MediatR;
using PrintHub.Domain;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Update;

public class OrderUpdateFormViewModel : ValidationViewModel<OrderUpdateFormViewModel>, ICallbackViewModel<OrderViewModel>, IParameterViewModel<Guid>, IParameterViewModel<Guid, NavigateCommand>
{
    private readonly IMediator _mediator;
    private Action<OrderViewModel>? _callback;

    private int _statusId;
    private ObservableCollection<ItemViewModel>? _items;
    private OrderStatus _status;
    private string? _description;

    public OrderUpdateFormViewModel(
        IMediator mediator,
        ICallbackNavigationService<ItemViewModel> detailsNavigationService,
        IValidator<OrderUpdateFormViewModel> validator) : base(validator)
    {
        _mediator = mediator;
        AddItemCommand = new CallbackNavigateCommand<ItemViewModel>(detailsNavigationService, OnItemAdded);
    }

    protected override OrderUpdateFormViewModel ViewModel => this;
    public Guid OrderId { get; set; }

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    private OrderStatus Status
    {
        get => _status;
        set
        {
            if (Set(ref _status, value) == false)
                return;

            StatusId = StatusList.IndexOf(Status.ToString());
        }
    }

    public int StatusId
    {
        get => _statusId;
        set => Set(ref _statusId, value);
    }

    public ObservableCollection<ItemViewModel>? Items
    {
        get => _items;
        private set => Set(ref _items, value);
    }

    public List<string> StatusList => Enum.GetNames(typeof(OrderStatus)).Prepend(string.Empty).ToList();

    public void SetCallback(Action<OrderViewModel> callback) => _callback ??= callback;

    public void SetParameter(Guid parameter, NavigateCommand? navigateCommand)
    {
        SetParameter(parameter);
        CloseCommand = navigateCommand;
    }

    public void SetParameter(Guid parameter)
    {
        OrderId = parameter;
    }

    private void OnItemAdded(ItemViewModel obj)
    {
        List<ItemViewModel> old = [obj];

        if (Items != null)
            old.AddRange(Items);

        Items = new ObservableCollection<ItemViewModel>(old);
    }

    #region Commands

    private ICommand? _confirmCommand;
    private ICommand? _loadOrderCommand;

    public ICommand AddItemCommand { get; }
    private ICommand? CloseCommand { get; set; }

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        OrderUpdateViewModel model = new()
        {
            Id = OrderId,
            Description = Description!,
            Status = Status
        };

        Operation<OrderViewModel, string> result = await _mediator.Send(new UpdateOrder.Request(model.Id, model));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        _callback?.Invoke(result.Result);

        MessageBoxResult boxResult = MaterialMessageBox.ShowWithCancel(result.Result.ToString(), "Order updated");

        if (boxResult == MessageBoxResult.OK)
            CloseCommand?.Execute(null);
    });

    public ICommand LoadOrderCommand => _loadOrderCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<OrderViewModel, string> result = await _mediator.Send(new GetOrderById.Request(OrderId));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        Description = result.Result.Description;
        Status = result.Result.Status;
        Items = new ObservableCollection<ItemViewModel>(result.Result.Items!);
    });

    #endregion
}