using System.Windows.Input;
using MediatR;
using PrintHub.WPF.Endpoints.ColorEndpoints;
using PrintHub.WPF.Endpoints.MaterialEndpoints;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints;

public class PrintingDetailsFormViewModel : ViewModelBase, IParameterViewModel<Guid>
{
    private ColorViewModel _colorViewModel;
    private Guid _loadedId;
    private MaterialViewModel _materialViewModel;
    private string _technology;

    public PrintingDetailsFormViewModel(IMediator mediator)
    {
        LoadCommand = new PrintingDetailsLoadCommand(this, mediator);
    }

    public string Technology
    {
        get => _technology;
        set => Set(ref _technology, value);
    }

    public ColorViewModel ColorViewModel
    {
        get => _colorViewModel;
        set => Set(ref _colorViewModel, value);
    }

    public MaterialViewModel MaterialViewModel
    {
        get => _materialViewModel;
        set => Set(ref _materialViewModel, value);
    }

    public Guid LoadedId
    {
        get => _loadedId;
        private set => Set(ref _loadedId, value);
    }

    public ICommand LoadCommand { get; }

    public void SetParameter(Guid parameter)
    {
        LoadedId = parameter;
        LoadCommand.Execute(parameter);
    }
}