using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PrintHub.Domain;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared;

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
        bool isFilterEntered = string.IsNullOrEmpty(selectedStatus) == false;

        if (order.Status == OrderStatus.Completed)
            return isFilterEntered && string.Equals(OrderStatus.Completed.ToString(), selectedStatus, StringComparison.OrdinalIgnoreCase);

        return isFilterEntered == false || string.Equals(order.Status.ToString(), selectedStatus, StringComparison.OrdinalIgnoreCase);
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

    private void OnResetFiltersButtonClicked(object sender, RoutedEventArgs e)
    {
        SelectedStatus.SelectedValue = null;
        UpdatedAtFilter.SelectedDate = null;
        OrdersFilterText.Text = string.Empty;
    }

    private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
    {
        CollectionViewSource collection = (CollectionViewSource)FindResource(nameof(OrdersCollection));

        if (collection.Source is not ObservableCollection<OrderViewModel> source)
        {
            MessageBox.Show("collection.Source is not ObservableCollection<OrderViewModel> source", "Error");
            return;
        }

        List<OrderViewModel> list = source.Where(order => !(IsStatusMatching(order) == false || IsUpdatedAtMatching(order) == false || IsFilterTextMatching(order) == false)).ToList();
        Unities.SaveToXml(list);
        Unities.SaveToExcel(list);
    }
}