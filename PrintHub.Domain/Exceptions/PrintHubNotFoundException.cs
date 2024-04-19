namespace PrintHub.Domain.Exceptions;

public class PrintHubNotFoundException : Exception
{
    public PrintHubNotFoundException(string entityName, string id)
        : base($"Item {entityName} with {id} not found")
    {
    }

    public PrintHubNotFoundException(string entityName, string id, Exception? exception)
        : base($"Item {entityName} with {id} not found", exception)
    {
    }
}