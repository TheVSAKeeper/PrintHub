namespace PrintHub.Domain.Exceptions;

public class PrintHubArgumentNullException : ArgumentNullException
{
    public PrintHubArgumentNullException(string argumentName)
        : base($"The {argumentName} is NULL")
    {
    }

    public PrintHubArgumentNullException(string argumentName, Exception? exception)
        : base($"The {argumentName} is NULL", exception)
    {
    }
}