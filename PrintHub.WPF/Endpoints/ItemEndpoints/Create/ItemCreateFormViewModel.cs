using System.Windows;
using System.Windows.Input;
using Calabonga.Results;
using FluentValidation;
using MediatR;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.ItemEndpoints.Queries;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Create;

public class ItemCreateFormViewModel : ValidationViewModel<ItemCreateFormViewModel>, ICallbackViewModel<ItemViewModel>
{
    private readonly AuthenticationStore _authenticationStore;
    private readonly IMediator _mediator;
    private Action<ItemViewModel>? _callback;
    private ICommand? _confirmCommand;
    private PrintingDetailsViewModel? _printingDetails;
    private string? _description;

    public ItemCreateFormViewModel(
        IMediator mediator,
        AuthenticationStore authenticationStore,
        CloseModalNavigationService closeNavigationService,
        ICallbackNavigationService<PrintingDetailsViewModel> detailsNavigationService,
        IValidator<ItemCreateFormViewModel> validator) : base(validator)
    {
        _mediator = mediator;
        _authenticationStore = authenticationStore;
        CloseCommand = new NavigateCommand(closeNavigationService);
        AddPrintingDetailsCommand = new CallbackNavigateCommand<PrintingDetailsViewModel>(detailsNavigationService, OnPrintingDetailsAdded);
    }

    protected override ItemCreateFormViewModel ViewModel => this;

    public ICommand AddPrintingDetailsCommand { get; }

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public PrintingDetailsViewModel? PrintingDetails
    {
        get => _printingDetails;
        set => Set(ref _printingDetails, value);
    }

    private ICommand CloseCommand { get; }

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        if (_authenticationStore.User is { ClientId: null })
        {
            MaterialMessageBox.Show("Client is null", "Create item error");
            return;
        }

        ItemCreateViewModel model = new()
        {
            // ClientId = (Guid)authenticationStore.User!.ClientId!,
            Description = Description!
            //RequiredColors = ChosenColors.Where(color => color.IsChecked).Select(color => color.ColorViewModel).ToList()
        };

        Operation<ItemViewModel, string> result = await _mediator.Send(new CreateItem.Request(model, _authenticationStore.User));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        _callback?.Invoke(result.Result);
        MessageBoxResult boxResult = MaterialMessageBox.Show(result.Result.ToString(), "Item created");

        if (boxResult == MessageBoxResult.OK)
            CloseCommand.Execute(null);
    });

    public void SetCallback(Action<ItemViewModel> callback) => _callback ??= callback;

    private void OnPrintingDetailsAdded(PrintingDetailsViewModel obj)
    {
        PrintingDetails = obj;
    }
}