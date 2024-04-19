namespace PrintHub.Domain.Base;

public static partial class AppData
{
    public const string ServiceName = "PrintHub";
    public const string ServiceDescription = "PrintHub is a service for ordering 3D printing";

    public const string PolicyCorsName = "CorsPolicy";
    public const string PolicyDefaultName = "DefaultPolicy";

    public const string SystemAdministratorRoleName = "Administrator";
    public const string ClientRoleName = "Client";

    public static IEnumerable<string> Roles
    {
        get
        {
            yield return SystemAdministratorRoleName;
            yield return ClientRoleName;
        }
    }
}