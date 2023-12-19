namespace AZ.Integrator.TagParcelTemplates.Application.UseCases.Commands.SaveParcelTemplate;

public record ParcelTemplateModel(string Id, DimensionTemplateModel Dimensions, double Weight);

public record DimensionTemplateModel(double Length, double Width, double Height);
