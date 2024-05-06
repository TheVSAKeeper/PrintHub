using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.PagedListCore;
using Calabonga.Results;
using FluentValidation;
using MediatR;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.ItemEndpoints.Queries;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Create;

public class ItemCreateFormViewModel(
    IMediator mediator,
    AuthenticationStore authenticationStore,
    CloseModalNavigationService closeNavigationService,
    IValidator<ItemCreateFormViewModel> validator)
    : ValidationViewModel<ItemCreateFormViewModel>(validator), ICallbackViewModel<ItemViewModel>
{
    private Action<ItemViewModel>? _callback;

    private ICommand? _confirmCommand;
    private ICommand? _loadColorsCommand;

    private ObservableCollection<CheckableColor> _chosenColors = null!;

    private string? _description;

    protected override ItemCreateFormViewModel ViewModel => this;

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

    private ICommand CloseCommand { get; } = new NavigateCommand(closeNavigationService);

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        if (authenticationStore.User is { ClientId: null })
        {
            MaterialMessageBox.Show("Client is null", "Create item error");
            return;
        }

        ItemCreateViewModel model = new()
        {
           // ClientId = (Guid)authenticationStore.User!.ClientId!,
            Description = Description!,
            //RequiredColors = ChosenColors.Where(color => color.IsChecked).Select(color => color.ColorViewModel).ToList()
        };

        Operation<ItemViewModel, string> result = await mediator.Send(new CreateItem.Request(model, authenticationStore.User));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        _callback?.Invoke(result.Result);
        MessageBoxResult boxResult = MaterialMessageBox.ShowWithCancel(result.Result.ToString(), "Item created");

        if (boxResult == MessageBoxResult.OK)
            CloseCommand.Execute(null);
    });

    public ICommand LoadColorsCommand => _loadColorsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<ColorViewModel>, string> colors = await mediator.Send(new GetColorPaged.Request(0, 10, null));
        ChosenColors = new ObservableCollection<CheckableColor>(colors.Result.Items.Select(model => new CheckableColor(model, false)));
    });

    public void SetCallback(Action<ItemViewModel> callback) => _callback ??= callback;

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked)
    {
        public ColorViewModel ColorViewModel { get; } = colorViewModel;
        public bool IsChecked { get; set; } = isChecked;
    }
}