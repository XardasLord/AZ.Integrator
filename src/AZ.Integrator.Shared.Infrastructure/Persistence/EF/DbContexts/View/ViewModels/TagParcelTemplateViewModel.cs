namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;

public class TagParcelTemplateViewModel
{
    public string Tag { get; set; }
    public ICollection<TagParcelViewModel> Parcels { get; set; }
}

public class TagParcelViewModel
{
    public int Id { get; set; }
    public string Tag { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
}