using System.Windows;
using System.Windows.Input;
using FluentValidation;
using PrintHub.WPF.Endpoints.ItemEndpoints.Queries;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Update;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;
using PrintHub.WPF.Pages.Admin;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Create;

public class ItemCreateFormViewModel : ValidationViewModel<ItemCreateFormViewModel>, ICallbackViewModel<ItemViewModel, Guid>
{
    private readonly IMediator _mediator;
    private Action<ItemViewModel>? _callback;

    private decimal _developmentCost;
    private decimal _weight;
    private PrintingDetailsViewModel? _printingDetails;
    private string? _description;

    public ItemCreateFormViewModel(
        IMediator mediator,
        CloseModalNavigationService closeNavigationService,
        ICallbackNavigationService<PrintingDetailsViewModel> detailsNavigationService,
        NavigationService<AdminViewModel> back,
        ParameterNavigationService<Guid, NavigateCommand, OrderUpdateFormViewModel> orderUpdateNavigationService,
        IValidator<ItemCreateFormViewModel> validator) : base(validator)
    {
        _mediator = mediator;
        CloseCommand = new NavigateCommand(closeNavigationService);
        AddPrintingDetailsCommand = new CallbackNavigateCommand<PrintingDetailsViewModel>(detailsNavigationService, OnPrintingDetailsAdded);
        CloseCommand = new ParameterBackNavigateCommand<Guid>(orderUpdateNavigationService, new NavigateCommand(back));
    }

    protected override ItemCreateFormViewModel ViewModel => this;

    private Guid OrderId { get; set; }

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public decimal Weight
    {
        get => _weight;
        set => Set(ref _weight, value);
    }

    public decimal DevelopmentCost
    {
        get => _developmentCost;
        set => Set(ref _developmentCost, value);
    }

    public PrintingDetailsViewModel? PrintingDetails
    {
        get => _printingDetails;
        set => Set(ref _printingDetails, value);
    }

    public void SetCallback(Action<ItemViewModel> callback, Guid parameter)
    {
        _callback ??= callback;
        OrderId = parameter;
    }

    private void OnPrintingDetailsAdded(PrintingDetailsViewModel obj)
    {
        PrintingDetails = obj;
    }

    #region Commands

    private ICommand? _confirmCommand;

    public ICommand AddPrintingDetailsCommand { get; }

    private ICommand CloseCommand { get; }

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        ItemCreateViewModel model = new()
        {
            OrderId = OrderId,
            Description = Description!,
            PrintingDetailsId = PrintingDetails!.Id,
            Weight = Weight,
            DevelopmentCost = DevelopmentCost
        };

        Operation<ItemViewModel, string> result = await _mediator.Send(new CreateItem.Request(model));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        _callback?.Invoke(result.Result);

        MessageBoxResult boxResult = MaterialMessageBox.Show(result.Result.ToString()!, "Item created");

        if (boxResult == MessageBoxResult.OK)
            CloseCommand.Execute(OrderId);
    });

    #endregion
}