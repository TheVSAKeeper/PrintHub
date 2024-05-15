using System.Collections.ObjectModel;
using System.Windows.Input;
using Calabonga.PagedListCore;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ItemEndpoints.Queries;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.MaterialMessageBox;

namespace PrintHub.WPF.Endpoints.AdminEndpoints.ShowStatistics;

public class ShowStatisticsFormViewModel(IMediator mediator) : ViewModelBase
{
    private bool _isFilterByDate;
    private DateTime? _endDate;
    private DateTime? _startDate;
    private decimal _totalAmount;
    private double _averageCompletionTime;
    private ObservableCollection<OrdersCount>? _ordersByStatus;

    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            Set(ref _startDate, value);
            ReloadCommand.Execute(null);
        }
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set
        {
            Set(ref _endDate, value);
            ReloadCommand.Execute(null);
        }
    }

    public bool IsFilterByDate
    {
        get => _isFilterByDate;
        set
        {
            if (Set(ref _isFilterByDate, value))
                ReloadCommand.Execute(null);
        }
    }

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

    private ICommand? _reloadCommand;

    public ICommand ReloadCommand => _reloadCommand ??= new LambdaCommandAsync(async () =>
    {
        Operation<IPagedList<OrderViewModel>, string> ordersResult = await mediator.Send(new GetOrderPaged.Request(0, 999, null));

        if (ordersResult.Ok == false)
        {
            MaterialMessageBox.ShowError(ordersResult.Error);
            return;
        }

        IList<OrderViewModel> orders = ordersResult.Result.Items
            .Where(Predicate)
            .ToList();

        Dictionary<OrderStatus, int> ordersByStatus = orders
            .GroupBy(order => order.Status)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

        OrderViewModel[] completedOrders = orders.Where(order => order.Status == OrderStatus.Completed).ToArray();

        AverageCompletionTime = completedOrders.Length == 0
            ? 0
            : completedOrders.Average(order => (order.UpdatedAt!.Value - order.CreatedAt).Days);

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

        ItemViewModel[] filteredItems = itemsResult.Result.Items.Where(Predicate).ToArray();

        TotalAmount = filteredItems.Length == 0
            ? 0
            : filteredItems.Sum(item => item.DevelopmentCost + item.PrintingDetails.MaterialViewModel.Price * item.Weight);

        return;

        bool Predicate(IAuditable model) => IsFilterByDate == false || EndDate != null && StartDate != null && model.CreatedAt >= StartDate && model.CreatedAt <= EndDate;
    });

    #endregion
}