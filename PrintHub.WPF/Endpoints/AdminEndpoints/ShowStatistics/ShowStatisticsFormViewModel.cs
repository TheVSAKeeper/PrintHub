using System.Collections.ObjectModel;
using System.Windows.Input;
using Calabonga.PagedListCore;
using PrintHub.WPF.Endpoints.ItemEndpoints.Queries;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.MaterialMessageBox;

namespace PrintHub.WPF.Endpoints.AdminEndpoints.ShowStatistics;

public class ShowStatisticsFormViewModel(IMediator mediator) : ViewModelBase
{
    private decimal _totalAmount;
    private double _averageCompletionTime;
    private ObservableCollection<OrdersCount>? _ordersByStatus;

    public ObservableCollection<OrdersCount>? OrdersByStatus
    {
        get => _ordersByStatus;
        private set => Set(ref _ordersByStatus, value);
    }

    public double AverageCompletionTime
    {
        get => _averageCompletionTime;
        private set => Set(ref _averageCompletionTime, value);
    }

    public decimal TotalAmount
    {
        get => _totalAmount;
        private set => Set(ref _totalAmount, value);
    }

    public record OrdersCount
    {
        public string Message { get; set; }
        public required int Count { get; set; }
        public required OrderStatus Status { get; set; }
    }

    #region Commands

    private ICommand? _resetCommand;

    public ICommand ResetCommand => _resetCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<OrderViewModel>, string> ordersResult = await mediator.Send(new GetOrderPaged.Request(0, 999, null));

        if (ordersResult.Ok == false)
        {
            MaterialMessageBox.ShowError(ordersResult.Error);
            return;
        }

        IList<OrderViewModel> orders = ordersResult.Result.Items;

        Dictionary<OrderStatus, int> ordersByStatus = orders.GroupBy(order => order.Status)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

        AverageCompletionTime = orders.Where(order => order.Status == OrderStatus.Completed).Average(order => (order.UpdatedAt - order.CreatedAt).Days);

        List<OrdersCount> descriptions = [];

        descriptions.AddRange(ordersByStatus.Select(status => new OrdersCount
            {
                Message = $"Status '{status.Key}': {status.Value} count",
                Status = status.Key,
                Count = status.Value
            })
            .OrderBy(count => count.Status));

        OrdersByStatus = new ObservableCollection<OrdersCount>(descriptions);

        Operation<IPagedList<ItemViewModel>, string> itemsResult = await mediator.Send(new GetItemPaged.Request(0, 999, null));

        if (itemsResult.Ok == false)
        {
            MaterialMessageBox.ShowError(itemsResult.Error);
            return;
        }

        TotalAmount = itemsResult.Result.Items.Sum(item => item.DevelopmentCost + item.PrintingDetails.MaterialViewModel.Price * item.Weight);
    });

    #endregion
}