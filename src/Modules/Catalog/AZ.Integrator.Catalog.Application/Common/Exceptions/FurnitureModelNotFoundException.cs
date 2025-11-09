namespace AZ.Integrator.Catalog.Application.Common.Exceptions;

public class FurnitureModelNotFoundException(string message) : CatalogModuleApplicationException(message)
{
    public override string Code => "furniture_model_not_found";
}