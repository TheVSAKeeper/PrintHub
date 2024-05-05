using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace PrintHub.WPF.Shared.MaterialMessageBox;

/// <summary>
///     Interaction logic for MessageBoxWindow.xaml
/// </summary>
public partial class MessageBoxWindow : IDisposable
{
    private readonly PaletteHelper _paletteHelper = new();

    public MessageBoxWindow()
    {
        InitializeComponent();
        Result = MessageBoxResult.Cancel;

        ResourceDictionary = (BundledTheme)Dictionary.MergedDictionaries.First(x => x is BundledTheme);

        Theme theme = _paletteHelper.GetTheme();
        ResourceDictionary.BaseTheme = theme.GetBaseTheme();
    }

    public BundledTheme ResourceDictionary { get; }

    public MessageBoxResult Result { get; set; }

    public void Dispose()
    {
        Close();
        GC.SuppressFinalize(this);
    }

    private void BtnOk_OnClick(object sender, RoutedEventArgs e)
    {
        Result = MessageBoxResult.OK;
        Close();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Result = MessageBoxResult.Cancel;
        Close();
    }

    private void BtnCopyMessage_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Clipboard.SetText(TxtMessage.Text);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }

    private void TitleBackgroundPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}