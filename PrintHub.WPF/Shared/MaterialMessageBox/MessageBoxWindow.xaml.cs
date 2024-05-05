using System.Windows;
using System.Windows.Input;

namespace PrintHub.WPF.Shared.MaterialMessageBox;

/// <summary>
///     Interaction logic for MessageBoxWindow.xaml
/// </summary>
public partial class MessageBoxWindow : IDisposable
{
    public MessageBoxWindow()
    {
        InitializeComponent();
        Result = MessageBoxResult.Cancel;
    }

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