namespace AZ.Integrator.Application.Common.ExternalServices.SubiektGT;

public interface ISubiektService
{
    Task<string> GenerateInvoice();
    Task PrintInvoice(string documentNumber);
}