using System.Windows;
using System.Windows.Media;

namespace PrintHub.WPF.Shared.MaterialMessageBox;

public static class MaterialMessageBox
{
    private const string MessageBoxTitle = "Message";

    public static void Show(string message, string title = MessageBoxTitle)
    {
        using MessageBoxWindow msg = new();

        msg.Title = title;
        msg.TxtTitle.Text = title;
        msg.TxtMessage.Text = message;
        msg.BtnCancel.Visibility = Visibility.Collapsed;

        msg.BtnOk.Focus();
        msg.ShowDialog();
    }

    public static void ShowError(string errorMessage, string errorTitle = "Error")
    {
        try
        {
            using MessageBoxWindow msg = new();

            msg.Title = errorTitle;
            msg.TxtTitle.Text = errorTitle;
            msg.TxtMessage.Text = errorMessage;
            msg.TitleBackgroundPanel.Background = Brushes.Red;
            msg.BorderBrush = Brushes.Red;
            msg.BtnCancel.Visibility = Visibility.Collapsed;

            msg.BtnOk.Focus();
            msg.ShowDialog();
        }
        catch (Exception)
        {
            MessageBox.Show(errorMessage);
        }
    }

    public static void ShowWarning(string warningMessage, string warningTitle = "Warning")
    {
        try
        {
            using MessageBoxWindow msg = new();

            msg.Title = warningTitle;
            msg.TxtTitle.Text = warningTitle;
            msg.TxtMessage.Text = warningMessage;
            msg.TitleBackgroundPanel.Background = Brushes.Orange;
            msg.BorderBrush = Brushes.Orange;
            msg.BtnCancel.Visibility = Visibility.Collapsed;

            msg.BtnOk.Focus();
            msg.ShowDialog();
        }
        catch (Exception)
        {
            MessageBox.Show(warningMessage, warningTitle);
        }
    }

    public static MessageBoxResult ShowWithCancel(string message, string title = MessageBoxTitle)
    {
        try
        {
            using MessageBoxWindow msg = new();

            msg.Title = title;
            msg.TxtTitle.Text = title;
            msg.TxtMessage.Text = message;

            msg.BtnOk.Focus();
            msg.ShowDialog();
            return msg.Result == MessageBoxResult.OK ? MessageBoxResult.OK : MessageBoxResult.Cancel;
        }
        catch (Exception)
        {
            MessageBox.Show(message);
            return MessageBoxResult.Cancel;
        }
    }

    public static MessageBoxResult ShowWarningWithCancel(string message, string title = "Warning")
    {
        try
        {
            using MessageBoxWindow msg = new();

            msg.Title = title;
            msg.TxtTitle.Text = title;
            msg.TxtMessage.Text = message;
            msg.TitleBackgroundPanel.Background = Brushes.Orange;
            msg.BorderBrush = Brushes.Orange;

            msg.BtnOk.Focus();
            msg.ShowDialog();
            return msg.Result == MessageBoxResult.OK ? MessageBoxResult.OK : MessageBoxResult.Cancel;
        }
        catch (Exception)
        {
            MessageBox.Show(message);
            return MessageBoxResult.Cancel;
        }
    }
}