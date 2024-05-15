using System.Collections.ObjectModel;
using System.Windows.Input;
using Calabonga.PagedListCore;
using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.Queries;
using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;
using PrintHub.WPF.Shared;
using PrintHub.WPF.Shared.MaterialMessageBox;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.Select;

public class SelectAdditionalServicesFormViewModel(IMediator mediator, IMapper mapper) : ViewModelBase
{
    private decimal? _totalPrice;
    private ObservableCollection<AdditionalServiceSelectViewModel>? _additionalServices;

    public ObservableCollection<AdditionalServiceSelectViewModel>? AdditionalServices
    {
        get => _additionalServices;
        set => Set(ref _additionalServices, value);
    }

    public decimal? TotalPrice
    {
        get => _totalPrice;
        set => Set(ref _totalPrice, value);
    }

    public List<ServiceDetailViewModel> GetSelectedServices()
    {
        return AdditionalServices!.SelectMany(model => model.ServiceDetails!.Where(checkable => checkable.IsChecked).Select(checkable => checkable.ViewModel)).ToList();
    }

    #region Commands

    private ICommand? _loadAdditionalServicesCommand;
    private ICommand? _calculatePriceCommand;

    public ICommand LoadAdditionalServicesCommand => _loadAdditionalServicesCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<AdditionalServiceViewModel>, string> result = await mediator.Send(new GetAdditionalServicePaged.Request(0, 9999, null));

        if (result.Ok == false)
        {
            MaterialMessageBox.ShowError(result.Error);
            return;
        }

        AdditionalServices = new ObservableCollection<AdditionalServiceSelectViewModel>(result.Result.Items.Select(mapper.Map<AdditionalServiceSelectViewModel>));

        foreach (AdditionalServiceSelectViewModel additionalService in AdditionalServices)
            foreach (Checkable<ServiceDetailViewModel> serviceDetail in additionalService.ServiceDetails!)
                serviceDetail.OnCheckChanged += () => CalculatePriceCommand.Execute(null);
    });

    public ICommand CalculatePriceCommand => _calculatePriceCommand ??= new LambdaCommand(() =>
    {
        TotalPrice = GetSelectedServices().Sum(model => model.Price);
    });

    #endregion
}