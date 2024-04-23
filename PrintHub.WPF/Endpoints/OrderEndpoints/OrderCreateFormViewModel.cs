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
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderCreateFormViewModel : ViewModelBase
{
    private readonly AuthenticationManager _authenticationManager;
    private readonly IMediator _mediator;

    private ICommand? _confirmCommand;
    private ICommand? _loadColorsCommand;

    private ObservableCollection<CheckableColor> _chosenColors;
    private string? _description;

    /*
    public OrderCreateFormViewModel()
    {
        ChosenColors = [];

        Random random = new();

        for (int i = 0; i < 3; i++)
        {
            ColorViewModel color = new()
            {
                Name = "Color " + i,
                ColorCode = "#" + random.Next(0x1000000).ToString("X6")
            };

            ChosenColors.Add(new CheckableColor(color, false));
        }

        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
    }
    */

    public OrderCreateFormViewModel(IMediator mediator, AuthenticationManager authenticationManager)
    {
        _mediator = mediator;
        _authenticationManager = authenticationManager;
    }

    public ObservableCollection<CheckableColor> ChosenColors
    {
        get => _chosenColors;
        set => Set(ref _chosenColors, value);
    }

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
        {
            if (_authenticationManager.User is { ClientId: null })
                MessageBox.Show("Client is null", "Create order error");

            OrderCreateViewModel model = new()
            {
                ClientId = (Guid)_authenticationManager.User!.ClientId!,
                Description = Description!,
                RequiredColors = ChosenColors.Where(x => x.IsChecked).Select(x => x.ColorViewModel).ToList()
            };

            Operation<OrderViewModel, string> result = await _mediator.Send(new CreateOrder.Request(model, _authenticationManager.User));

            if (result.Ok)
                MessageBox.Show(result.Result.ToString(), "Order created");
        },
        () => Description is { Length: > 10 });

    public ICommand LoadColorsCommand => _loadColorsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<ColorViewModel>, string> colors = await _mediator.Send(new GetColorPaged.Request(0, 10, null));
        ChosenColors = new ObservableCollection<CheckableColor>(colors.Result.Items.Select(x => new CheckableColor(x, false)));
    });

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked)
    {
        public ColorViewModel ColorViewModel { get; } = colorViewModel;
        public bool IsChecked { get; set; } = isChecked;
    }
}