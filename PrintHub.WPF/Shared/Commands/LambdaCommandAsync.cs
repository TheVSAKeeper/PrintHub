namespace PrintHub.WPF.Shared.Commands;

public class LambdaCommandAsync : CommandBase
{
    private readonly Func<object?, bool>? _CanExecuteAsync;
    private readonly Func<object?, Task> _ExecuteAsync;

    private volatile Task? _ExecutingTask;

    public LambdaCommandAsync(Func<Task> ExecuteAsync, Func<bool>? CanExecute = null)
        : this(ExecuteAsync is null ? throw new ArgumentNullException(nameof(ExecuteAsync)) : new Func<object?, Task>(_ => ExecuteAsync()),
            CanExecute is null ? null : _ => CanExecute!())
    {
    }

    public LambdaCommandAsync(Func<object?, Task> ExecuteAsync, Func<bool>? CanExecute)
        : this(ExecuteAsync, CanExecute is null ? null : _ => CanExecute!())
    {
    }

    public LambdaCommandAsync(Func<object?, Task> ExecuteAsync, Func<object?, bool>? CanExecuteAsync = null)
    {
        _ExecuteAsync = ExecuteAsync ?? throw new ArgumentNullException(nameof(ExecuteAsync));
        _CanExecuteAsync = CanExecuteAsync;
    }

    /// <summary>Выполнять задачу принудительно в фоновом потоке</summary>
    public bool Background { get; set; }

    protected override bool CanExecute(object? parameter) =>
        (_ExecutingTask is null || _ExecutingTask.IsCompleted)
        && (_CanExecuteAsync?.Invoke(parameter) ?? true);

    protected override async void Execute(object? parameter)
    {
        bool background = Background;

        bool can_execute = background
            ? await Task.Run(() => CanExecute(parameter))
            : CanExecute(parameter);

        if (!can_execute)
            return;

        Task execute_async = background ? Task.Run(() => _ExecuteAsync(parameter)) : _ExecuteAsync(parameter);
        _ = Interlocked.Exchange(ref _ExecutingTask, execute_async);
        _ExecutingTask = execute_async;
        OnCanExecuteChanged();

        try
        {
            await execute_async.ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
        }

        OnCanExecuteChanged();
    }
}