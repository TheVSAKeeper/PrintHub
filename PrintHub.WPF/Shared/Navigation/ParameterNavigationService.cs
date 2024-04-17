﻿using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Shared.Navigation;

public class ParameterNavigationService<TParameter, TViewModel>(INavigationMediator navigationMediator, Func<TParameter, TViewModel> createViewModel)
    : IParameterNavigationService<TParameter> where TViewModel : ViewModelBase
{
    public void Navigate(TParameter parameter)
    {
        navigationMediator.CurrentViewModel = createViewModel(parameter);
    }
}