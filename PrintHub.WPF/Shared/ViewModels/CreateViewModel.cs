namespace PrintHub.WPF.Shared.ViewModels;

public delegate TViewModel CreateViewModel<out TViewModel>() where TViewModel : ViewModelBase;

public delegate TViewModel CreateViewModel<in TParameter, out TViewModel>(TParameter parameter) where TViewModel : ViewModelBase;

public delegate TViewModel CreateViewModel<in TParameter, in T, out TViewModel>(TParameter parameter, T parameter1) where TViewModel : ViewModelBase;