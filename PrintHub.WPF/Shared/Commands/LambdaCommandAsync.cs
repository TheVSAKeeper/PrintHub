namespace PrintHub.WPF.Shared.Commands;

public class LambdaCommandAsync(Func<object?, Task> executeAsync, Func<object?, bool>? canExecuteAsync = null) : CommandBase
{
    private readonly Func<object?, Task> _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));

    private volatile Task? _executingTask;

    public LambdaCommandAsync(Func<Task> executeAsync, Func<bool>? canExecute = null)
        : this(executeAsync is null ? throw new ArgumentNullException(nameof(executeAsync)) : new Func<object?, Task>(_ => executeAsync()),
            canExecute is null ? null : _ => canExecute!())
    {
    }

    public LambdaCommandAsync(Func<object?, Task> executeAsync, Func<bool>? canExecute)
        : this(executeAsync, canExecute is null ? null : _ => canExecute!())
    {
    }

    /// <summary>Выполнять задачу принудительно в фоновом потоке</summary>
    public bool Background { get; set; }

    protected override bool CanExecute(object? parameter) =>
        (_executingTask is null || _executingTask.IsCompleted)
        && (canExecuteAsync?.Invoke(parameter) ?? true);

    protected override async void Execute(object? parameter)
    {
        bool background = Background;

        bool canExecute = background
            ? await Task.Run(() => CanExecute(parameter))
            : CanExecute(parameter);

        if (!canExecute)
            return;

        Task executeAsync = background ? Task.Run(() => _executeAsync(parameter)) : _executeAsync(parameter);
        _ = Interlocked.Exchange(ref _executingTask, executeAsync);
        _executingTask = executeAsync;
        OnCanExecuteChanged();

        try
        {
            await executeAsync.ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
        }

        OnCanExecuteChanged();
    }
}