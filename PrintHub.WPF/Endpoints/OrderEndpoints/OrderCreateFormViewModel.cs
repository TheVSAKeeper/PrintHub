using System.Collections.ObjectModel;
using System.Windows.Input;
using Calabonga.PagedListCore;
using Calabonga.Results;
using MediatR;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderCreateFormViewModel : ViewModelBase
{
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

    public OrderCreateFormViewModel(IMediator mediator)
    {
        _mediator = mediator;
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

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommand(() =>
        {
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