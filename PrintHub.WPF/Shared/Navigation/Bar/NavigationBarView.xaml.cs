using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace PrintHub.WPF.Shared.Navigation.Bar;

/// <summary>
///     Interaction logic for NavigationBarView.xaml
/// </summary>
public partial class NavigationBarView : UserControl
{
    private readonly PaletteHelper _paletteHelper = new();

    public NavigationBarView()
    {
        InitializeComponent();
    }

    private void OnThemeToggleClicked(object sender, RoutedEventArgs e)
    {
        Theme theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(theme.GetBaseTheme() == BaseTheme.Light ? BaseTheme.Dark : BaseTheme.Light);
        _paletteHelper.SetTheme(theme);
    }
}