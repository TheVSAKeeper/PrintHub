namespace PrintHub.Domain.Exceptions;

public class PrintHubDatabaseSaveException : Exception
{
    public PrintHubDatabaseSaveException(string entityName)
        : base($"Saving data error for entity name {entityName}")
    {
    }

    public PrintHubDatabaseSaveException(string entityName, Exception? exception)
        : base($"Saving data error for entity name {entityName}", exception)
    {
    }
}