using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calabonga.PagedListCore;
using FluentValidation;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.MaterialEndpoints.Queries;
using PrintHub.WPF.Endpoints.MaterialEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;
using PrintHub.WPF.Shared;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Navigation.Modal;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Create;

public class PrintingDetailsCreateFormViewModel(
    IMediator mediator,
    CloseModalNavigationService closeNavigationService,
    IValidator<PrintingDetailsCreateFormViewModel> validator)
    : ValidationViewModel<PrintingDetailsCreateFormViewModel>(validator), ICallbackViewModel<PrintingDetailsViewModel>
{
    private Action<PrintingDetailsViewModel>? _callback;

    private ObservableCollection<CheckableColor> _allColors = null!;
    private ObservableCollection<CheckableMaterial> _allMaterials = null!;
    private ObservableCollection<CheckableMaterial>? _materials;

    private string? _description;

    protected override PrintingDetailsCreateFormViewModel ViewModel => this;

    public string? Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public ObservableCollection<CheckableColor> AllColors
    {
        get => _allColors;
        private set => Set(ref _allColors, value);
    }

    public ObservableCollection<CheckableMaterial> AllMaterials
    {
        get => _allMaterials;
        private set => Set(ref _allMaterials, value);
    }

    public ObservableCollection<CheckableMaterial>? Materials
    {
        get => _materials;
        set => Set(ref _materials, value);
    }

    public void SetCallback(Action<PrintingDetailsViewModel> callback) => _callback ??= callback;

    public class CheckableColor(ColorViewModel colorViewModel, bool isChecked) : Checkable(isChecked)
    {
        public ColorViewModel ViewModel { get; } = colorViewModel;
    }

    public class CheckableMaterial(MaterialViewModel colorViewModel, bool isChecked) : Checkable(isChecked)
    {
        public MaterialViewModel ViewModel { get; } = colorViewModel;
    }

    #region Commands

    private ICommand? _confirmCommand;
    private ICommand? _loadColorsCommand;
    private ICommand? _loadMaterialsCommand;

    public ICommand ConfirmCommand => _confirmCommand ??= new LambdaCommandAsync(async () =>
    {
        Validate();

        if (HasErrors)
            return;

        CheckableColor color = AllColors.First(checkableColor => checkableColor.IsChecked);
        CheckableMaterial material = AllMaterials.First(checkableMaterial => checkableMaterial.IsChecked);

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

        Operation<PrintingDetailsViewModel, string> newResult = await mediator.Send(new GetPrintingDetailsById.Request(result.Result.Id));

        if (newResult.Ok == false)
        {
            MaterialMessageBox.ShowError(newResult.Error);
            return;
        }

        _callback?.Invoke(newResult.Result);
        MessageBoxResult boxResult = MaterialMessageBox.Show(newResult.Result.ToString()!, "PrintingDetails created");

        if (boxResult == MessageBoxResult.OK)
            CloseCommand.Execute(null);
    }, () => HasErrors == false);

    public ICommand LoadColorsCommand => _loadColorsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<ColorViewModel>, string> colors = await mediator.Send(new GetColorPaged.Request(0, 10, null));
        AllColors = new ObservableCollection<CheckableColor>(colors.Result.Items.Select(model => new CheckableColor(model, false)));

        foreach (CheckableColor color in AllColors)
            color.OnCheckChanged += () =>
            {
                if (color.IsChecked)
                {
                    foreach (CheckableColor checkableColor in AllColors.Where(checkableColor => checkableColor != color))
                        checkableColor.IsChecked = false;

                    Materials = new ObservableCollection<CheckableMaterial>(AllMaterials.Where(material =>
                        material.ViewModel.AvailableColors.FirstOrDefault(model => model.Id == color.ViewModel.Id) != null));

                    OnPropertyChanged(nameof(AllColors));
                }

                ValidateProperty(nameof(AllColors));
            };

        LoadMaterialsCommand.Execute(null);
    });

    public ICommand LoadMaterialsCommand => _loadMaterialsCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<MaterialViewModel>, string> result = await mediator.Send(new GetMaterialPaged.Request(0, 10, null));
        AllMaterials = new ObservableCollection<CheckableMaterial>(result.Result.Items.Select(model => new CheckableMaterial(model, false)));

        foreach (CheckableMaterial material in AllMaterials)
            material.OnCheckChanged += () => ValidateProperty(nameof(AllMaterials));
    });

    private ICommand CloseCommand { get; } = new NavigateCommand(closeNavigationService);

    #endregion
}