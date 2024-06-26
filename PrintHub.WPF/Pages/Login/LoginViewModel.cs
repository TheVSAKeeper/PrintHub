﻿using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Login;
using PrintHub.WPF.Pages.Home;

namespace PrintHub.WPF.Pages.Login;

public class LoginViewModel(AuthenticationStore authenticationStore, NavigationService<HomeViewModel> homeNavigationService)
    : ViewModelBase
{
    public LoginFormViewModel LoginFormViewModel { get; } = new(authenticationStore, homeNavigationService);
}