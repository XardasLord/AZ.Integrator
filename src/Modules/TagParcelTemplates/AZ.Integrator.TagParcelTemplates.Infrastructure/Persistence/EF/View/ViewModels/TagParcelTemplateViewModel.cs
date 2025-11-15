namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.ViewModels;

public class TagParcelTemplateViewModel
{
    public string Tag { get; set; }
    public Guid TenantId { get; set; }
    public ICollection<TagParcelViewModel> Parcels { get; set; }
}

public class TagParcelViewModel
{
    public int Id { get; set; }
    public Guid TenantId { get; set; }
    public string Tag { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
}