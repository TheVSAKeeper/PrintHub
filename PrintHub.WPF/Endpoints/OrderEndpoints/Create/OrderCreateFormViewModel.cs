using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.PagedListCore;
using FluentValidation;
using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.Select;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Create;

public class OrderCreateFormViewModel(
    IMediator mediator,
    AuthenticationStore authenticationStore,
    CloseModalNavigationService closeNavigationService,
    SelectAdditionalServicesFormViewModel additionalServicesFormViewModel,
    IValidator<OrderCreateFormViewModel> validator)
    : ValidationViewModel<OrderCreateFormViewModel>(validator), ICallbackViewModel<OrderViewModel>
{
    private Action<OrderViewModel>? _callback;
    private ObservableCollection<CheckableColor> _chosenColors = null!;
    private string? _description;

    public SelectAdditionalServicesFormViewModel SelectAdditionalServicesFormViewModel { get; } = additionalServicesFormViewModel;

    protected override OrderCreateFormViewModel ViewModel => this;

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public ObservableCollection<CheckableColor> ChosenColors
    {
        get => _chosenColors;
        private set => Set(ref _chosenColors, value);
    }

    public void SetCallback(Action<OrderViewModel> callback) => _callback ??= callback;

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked)
    {
        public ColorViewModel ColorViewModel { get; } = colorViewModel;
        public bool IsChecked { get; set; } = isChecked;
    }

    #region Commands

    private ICommand? _confirmCommand;
    private ICommand? _loadColorsCommand;

    private ICommand CloseCommand { get; } = new NavigateCommand(closeNavigationService);

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        if (authenticationStore.User is { ClientId: null })
        {
            MaterialMessageBox.Show("Client is null", "Create order error");
            return;
        }

        OrderCreateViewModel model = new()
        {
            ClientId = (Guid)authenticationStore.User!.ClientId!,
            Description = Description!,
            RequiredColors = ChosenColors.Where(color => color.IsChecked).Select(color => color.ColorViewModel).ToList(),
            ServiceDetails = SelectAdditionalServicesFormViewModel.GetSelectedServices()
        };

        Operation<OrderViewModel, string> result = await mediator.Send(new CreateOrder.Request(model, authenticationStore.User));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        _callback?.Invoke(result.Result);
        MessageBoxResult boxResult = MaterialMessageBox.Show(result.Result.ToString(), "Order created");

        if (boxResult == MessageBoxResult.OK)
            CloseCommand.Execute(null);
    });

    public ICommand LoadColorsCommand => _loadColorsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<ColorViewModel>, string> colors = await mediator.Send(new GetColorPaged.Request(0, 10, null));
        ChosenColors = new ObservableCollection<CheckableColor>(colors.Result.Items.Select(model => new CheckableColor(model, false)));

        SelectAdditionalServicesFormViewModel.LoadAdditionalServicesCommand.Execute(null);
    });

    #endregion
}