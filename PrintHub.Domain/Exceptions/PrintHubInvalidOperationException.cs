namespace PrintHub.Domain.Exceptions;

public class PrintHubInvalidOperationException : InvalidOperationException
{
    public PrintHubInvalidOperationException(string operationName, string reason)
        : base($"The {operationName} cannot be executed because {reason}")
    {
    }

    public PrintHubInvalidOperationException(string operationName, string reason, Exception? exception)
        : base($"The {operationName} cannot be executed because {reason}", exception)
    {
    }
}