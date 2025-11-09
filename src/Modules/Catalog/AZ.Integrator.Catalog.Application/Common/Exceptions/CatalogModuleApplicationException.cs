namespace AZ.Integrator.Catalog.Application.Common.Exceptions;

public abstract class CatalogModuleApplicationException : Exception
{
    public abstract string Code { get; }

    protected CatalogModuleApplicationException()
    {
    }

    protected CatalogModuleApplicationException(string message) : base(message)
    {
    }
    
    protected CatalogModuleApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}