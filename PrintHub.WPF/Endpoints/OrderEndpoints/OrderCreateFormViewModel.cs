using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.PagedListCore;
using Calabonga.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderCreateFormViewModel(
    IMediator mediator,
    AuthenticationManager authenticationManager,
    CloseModalNavigationService closeNavigationService,
    IValidator<OrderCreateFormViewModel> validator)
    : ViewModelBase, ICallbackViewModel<OrderViewModel>
{
    private Action<OrderViewModel>? _callback;

    private ICommand? _confirmCommand;
    private ICommand? _loadColorsCommand;

    private ObservableCollection<CheckableColor> _chosenColors = null!;
    private ObservableCollection<ValidationFailure>? _errors;

    private string? _description;

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public ObservableCollection<CheckableColor> ChosenColors
    {
        get => _chosenColors;
        set => Set(ref _chosenColors, value);
    }

    public ObservableCollection<ValidationFailure>? Errors
    {
        get => _errors;
        set => Set(ref _errors, value);
    }

    public ICommand CloseCommand { get; } = new NavigateCommand(closeNavigationService);

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
        {
            if (authenticationManager.User is { ClientId: null })
            {
                MaterialMessageBox.Show("Client is null", "Create order error");
                return;
            }

            ValidationResult validationResult = validator.Validate(this);

            if (validationResult.IsValid == false)
            {
                MaterialMessageBox.ShowError(string.Join(Environment.NewLine, validationResult.Errors));
                return;
            }

            OrderCreateViewModel model = new()
            {
                ClientId = (Guid)authenticationManager.User!.ClientId!,
                Description = Description!,
                RequiredColors = ChosenColors.Where(color => color.IsChecked).Select(color => color.ColorViewModel).ToList()
            };

            Operation<OrderViewModel, string> result = await mediator.Send(new CreateOrder.Request(model, authenticationManager.User));

            if (result.Ok == false)
            {
                MaterialMessageBox.ShowError(result.Error);
                return;
            }

            _callback?.Invoke(result.Result);
            MessageBoxResult boxResult = MaterialMessageBox.ShowWithCancel(result.Result.ToString(), "Order created");

            if (boxResult == MessageBoxResult.OK)
                CloseCommand.Execute(null);
        },
        () =>
        {
            ValidationResult validationResult = validator.Validate(this);
            Errors = new ObservableCollection<ValidationFailure>(validationResult.Errors);

            return validationResult.IsValid;
        });

    public ICommand LoadColorsCommand => _loadColorsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<ColorViewModel>, string> colors = await mediator.Send(new GetColorPaged.Request(0, 10, null));
        ChosenColors = new ObservableCollection<CheckableColor>(colors.Result.Items.Select(model => new CheckableColor(model, false)));
    });

    public void SetCallback(Action<OrderViewModel> callback) => _callback ??= callback;

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked)
    {
        public ColorViewModel ColorViewModel { get; } = colorViewModel;
        public bool IsChecked { get; set; } = isChecked;
    }
}