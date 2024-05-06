using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.Results;
using FluentValidation;
using MediatR;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Update;

public class OrderUpdateFormViewModel : ValidationViewModel<OrderUpdateFormViewModel>, ICallbackViewModel<OrderViewModel>, IParameterViewModel<Guid>
{
    private readonly AuthenticationStore _authenticationStore;
    private readonly IMediator _mediator;
    private Action<OrderViewModel>? _callback;

    private ICommand? _confirmCommand;

    private ObservableCollection<ItemViewModel>? _items;
    private string? _description;

    public OrderUpdateFormViewModel(
        IMediator mediator,
        AuthenticationStore authenticationStore,
        CloseModalNavigationService closeNavigationService,
        ICallbackNavigationService<ItemViewModel> detailsNavigationService,
        IValidator<OrderUpdateFormViewModel> validator) : base(validator)
    {
        _mediator = mediator;
        _authenticationStore = authenticationStore;
        CloseCommand = new NavigateCommand(closeNavigationService);
        AddItemCommand = new CallbackNavigateCommand<ItemViewModel>(detailsNavigationService, OnItemAdded);
    }

    protected override OrderUpdateFormViewModel ViewModel => this;
    public Guid OrderId { get; set; }

    public ICommand AddItemCommand { get; }

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public ObservableCollection<ItemViewModel>? Items
    {
        get => _items;
        private set => Set(ref _items, value);
    }

    public ICommand CloseCommand { get; }

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        if (_authenticationStore.User is { ClientId: null })
        {
            MaterialMessageBox.Show("Client is null", "Update order error");
            return;
        }

        OrderUpdateViewModel model = new()
        {
            //  ClientId = (Guid)authenticationStore.User!.ClientId!,
            Description = Description!
            // RequiredColors = ChosenColors.Where(color => color.IsChecked).Select(color => color.ColorViewModel).ToList()
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
            CloseCommand.Execute(null);
    });

    public void SetCallback(Action<OrderViewModel> callback) => _callback ??= callback;

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

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked)
    {
        public ColorViewModel ColorViewModel { get; } = colorViewModel;
        public bool IsChecked { get; set; } = isChecked;
    }
}