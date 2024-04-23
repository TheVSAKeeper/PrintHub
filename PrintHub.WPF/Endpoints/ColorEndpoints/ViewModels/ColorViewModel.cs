namespace PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

public class ColorViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ColorCode { get; set; }

    public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(ColorCode)}: {ColorCode}";
}