using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.PagedListCore;
using Calabonga.Results;
using FluentValidation;
using MediatR;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.MaterialEndpoints.Queries;
using PrintHub.WPF.Endpoints.MaterialEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Create;

public class PrintingDetailsCreateFormViewModel(
    IMediator mediator,
    CloseModalNavigationService closeNavigationService,
    IValidator<PrintingDetailsCreateFormViewModel> validator)
    : ValidationViewModel<PrintingDetailsCreateFormViewModel>(validator), ICallbackViewModel<PrintingDetailsViewModel>
{
    private Action<PrintingDetailsViewModel>? _callback;

    private ICommand? _confirmCommand;
    private ICommand? _loadColorsCommand;
    private ICommand? _loadMaterialsCommand;

    private ObservableCollection<CheckableColor> _chosenColors = null!;
    private ObservableCollection<CheckableMaterial> _chosenMaterials = null!;

    private string? _description;

    protected override PrintingDetailsCreateFormViewModel ViewModel => this;

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

    public ObservableCollection<CheckableMaterial> ChosenMaterials
    {
        get => _chosenMaterials;
        private set => Set(ref _chosenMaterials, value);
    }

    public ICommand LoadColorsCommand => _loadColorsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<ColorViewModel>, string> colors = await mediator.Send(new GetColorPaged.Request(0, 10, null));
        ChosenColors = new ObservableCollection<CheckableColor>(colors.Result.Items.Select(model => new CheckableColor(model, false)));
    });

    public ICommand LoadMaterialsCommand => _loadMaterialsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<MaterialViewModel>, string> colors = await mediator.Send(new GetMaterialPaged.Request(0, 10, null));
        ChosenMaterials = new ObservableCollection<CheckableMaterial>(colors.Result.Items.Select(model => new CheckableMaterial(model, false)));
    });

    private ICommand CloseCommand { get; } = new NavigateCommand(closeNavigationService);

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        CheckableColor color = ChosenColors.First(checkableColor => checkableColor.IsChecked);
        CheckableMaterial material = ChosenMaterials.First(checkableMaterial => checkableMaterial.IsChecked);

        PrintingDetailsCreateViewModel model = new()
        {
            Description = Description ?? string.Empty,
            ColorId = color.ViewModel.Id,
            MaterialId = material.ViewModel.Id,
            Technology = material.ViewModel.Technology
        };

        Operation<PrintingDetailsViewModel, string> result = await mediator.Send(new CreatePrintingDetails.Request(model));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        _callback?.Invoke(result.Result);
        MessageBoxResult boxResult = MaterialMessageBox.Show(result.Result.ToString(), "PrintingDetails created");

        if (boxResult == MessageBoxResult.OK)
            CloseCommand.Execute(null);
    }, () => HasErrors == false);

    public void SetCallback(Action<PrintingDetailsViewModel> callback) => _callback ??= callback;

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked) : Checkable(isChecked)
    {
        public ColorViewModel ViewModel { get; } = colorViewModel;
    }

    public class CheckableMaterial(MaterialViewModel colorViewModel, bool isChecked) : Checkable(isChecked)
    {
        public MaterialViewModel ViewModel { get; } = colorViewModel;
    }
}

public abstract class Checkable(bool isChecked)
{
    public bool IsChecked { get; set; } = isChecked;
}