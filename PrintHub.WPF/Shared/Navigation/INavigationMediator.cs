﻿using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Shared.Navigation;

public interface INavigationMediator
{
    ViewModelBase CurrentViewModel { set; }
}