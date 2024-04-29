using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.PagedListCore;
using Calabonga.Results;
using MediatR;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderCreateFormViewModel(IMediator mediator, AuthenticationManager authenticationManager, CloseModalNavigationService closeNavigationService)
    : ViewModelBase, ICallbackViewModel<OrderViewModel>
{
    private Action<OrderViewModel>? _callback;

    private ICommand? _confirmCommand;
    private ICommand? _loadColorsCommand;

    private ObservableCollection<CheckableColor> _chosenColors = null!;
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

    public ICommand CancelCommand { get; } = new NavigateCommand(closeNavigationService);

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
        {
            if (authenticationManager.User is { ClientId: null })
            {
                MessageBox.Show("Client is null", "Create order error");
                return;
            }

            OrderCreateViewModel model = new()
            {
                ClientId = (Guid)authenticationManager.User!.ClientId!,
                Description = Description!,
                RequiredColors = ChosenColors.Where(x => x.IsChecked).Select(x => x.ColorViewModel).ToList()
            };

            Operation<OrderViewModel, string> result = await mediator.Send(new CreateOrder.Request(model, authenticationManager.User));

            if (result.Ok)
            {
                _callback?.Invoke(result.Result);
                MessageBoxResult boxResult = MessageBox.Show(result.Result.ToString(), "Order created");

                if (boxResult == MessageBoxResult.OK)
                    CancelCommand.Execute(null);
            }
        },
        () => Description is { Length: > 10 });

    public ICommand LoadColorsCommand => _loadColorsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<ColorViewModel>, string> colors = await mediator.Send(new GetColorPaged.Request(0, 10, null));
        ChosenColors = new ObservableCollection<CheckableColor>(colors.Result.Items.Select(x => new CheckableColor(x, false)));
    });

    public void SetCallback(Action<OrderViewModel> callback) => _callback ??= callback;

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked)
    {
        public ColorViewModel ColorViewModel { get; } = colorViewModel;
        public bool IsChecked { get; set; } = isChecked;
    }
}