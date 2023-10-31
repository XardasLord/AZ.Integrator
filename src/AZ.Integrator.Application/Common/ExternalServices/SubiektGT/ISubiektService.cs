namespace AZ.Integrator.Application.Common.ExternalServices.SubiektGT;

public interface ISubiektService
{
    Task<string> GenerateSaleDocument();
    Task PrintSaleDocument(string documentNumber);
}