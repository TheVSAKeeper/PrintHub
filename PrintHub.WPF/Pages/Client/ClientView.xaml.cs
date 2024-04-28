using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

namespace PrintHub.WPF.Pages.Client;

public partial class ClientView : UserControl
{
    private const string OrdersCollection = nameof(OrdersCollection);

    public ClientView()
    {
        InitializeComponent();
    }

    private void FilterOrders(object sender, FilterEventArgs e)
    {
        if (e.Item is not OrderViewModel order)
        {
            e.Accepted = false;
            return;
        }

        if (IsStatusMatching(order) == false || IsUpdatedAtMatching(order) == false || IsFilterTextMatching(order) == false)
        {
            e.Accepted = false;
            return;
        }

        e.Accepted = true;
    }

    private bool IsStatusMatching(OrderViewModel order)
    {
        string? selectedStatus = SelectedStatus.SelectedValue as string;
        return string.IsNullOrEmpty(selectedStatus) || string.Equals(order.Status.ToString(), selectedStatus, StringComparison.OrdinalIgnoreCase);
    }

    private bool IsUpdatedAtMatching(OrderViewModel order) => UpdatedAtFilter.SelectedDate.HasValue == false || order.UpdatedAt >= UpdatedAtFilter.SelectedDate;

    private bool IsFilterTextMatching(OrderViewModel order)
    {
        string filterText = OrdersFilterText.Text;
        return string.IsNullOrWhiteSpace(filterText) || order.Description.Contains(filterText, StringComparison.OrdinalIgnoreCase) || order.Status.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase);
    }

    private void RefreshOrders()
    {
        CollectionViewSource collection = (CollectionViewSource)FindResource(nameof(OrdersCollection));
        collection.View?.Refresh();
    }

    private void SelectedStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshOrders();
    }

    private void UpdatedAtFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshOrders();
    }

    private void OrdersFilterText_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshOrders();
    }

    private void ResetFilters(object sender, RoutedEventArgs e)
    {
        SelectedStatus.SelectedValue = null;
        UpdatedAtFilter.SelectedDate = null;
        OrdersFilterText.Text = string.Empty;
    }
}