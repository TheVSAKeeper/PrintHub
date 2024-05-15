namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints.Update;

public class ApplicationUserUpdateDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}