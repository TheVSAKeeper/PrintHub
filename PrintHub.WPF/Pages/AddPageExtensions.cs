using Microsoft.Extensions.DependencyInjection;

namespace PrintHub.WPF.Pages;

public static class AddPageExtensions
{
    public static IServiceCollection AddPage<TViewModel>(this IServiceCollection serviceCollection)
        where TViewModel : ViewModelBase
    {
        serviceCollection.AddTransient<TViewModel>();
        serviceCollection.AddNavigationService<TViewModel>();

        return serviceCollection;
    }
}