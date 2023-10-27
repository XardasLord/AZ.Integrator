using System.Globalization;
using AZ.Integrator.Application.Common.ExternalServices.Dpd;
using AZ.Integrator.Application.Common.ExternalServices.Dpd.Models;
using AZ.Integrator.Application.UseCases.Shipments.Commands.CreateDpdShipment;
using AZ.Integrator.Domain.Aggregates.DpdShipment.ValueObjects;
using AZ.Integrator.Infrastructure.DPDPackageObjServicesService;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Infrastructure.ExternalServices.Dpd;

public class DpdApiService : IDpdService
{
    private readonly DpdOptions _dpdOptions;

    public DpdApiService(IOptions<DpdOptions> dpdSettings)
    {
        _dpdOptions = dpdSettings.Value;
    }

    public async Task<CreateDpdShipmentResponse> CreateShipment(CreateDpdShipmentCommand shipment)
    {
        var request = PrepareRequestForPackagesGeneration(shipment);

        var dpdClient = new DPDPackageObjServicesClient();

        var response = await dpdClient.generatePackagesNumbersV9Async(
            request.openUMLFeV11,
            request.pkgNumsGenerationPolicyV1,
            request.langCode,
            request.authDataV1);
        
        if (response.@return.Status != "OK")
        {
            var errorDetails = GetErrorDetails(response);

            throw new InvalidOperationException(string.Join(';', errorDetails));
        }

        return PrepareResponse(response);
    }

    public async Task<byte[]> GenerateLabel(SessionNumber sessionNumber)
    {
        var request = PrepareRequestForSpedLabelsGeneration(sessionNumber);
        
        var dpdClient = new DPDPackageObjServicesClient();

        var response = await dpdClient.generateSpedLabelsV4Async(
            request.dpdServicesParamsV1, 
            request.outputDocFormatV1,
            request.outputDocPageFormatV1,
            request.outputLabelType,
            request.labelVariant,
            request.authDataV1);

        return response.@return.documentData;
    }

    private generatePackagesNumbersV9 PrepareRequestForPackagesGeneration(CreateDpdShipmentCommand shipment)
    {
        var parcelList = new List<parcelOpenUMLFeV3>();
        
        shipment.Parcels.ForEach(parcel =>
        {
            parcelList.Add(new parcelOpenUMLFeV3
            {
                sizeX = parcel.Dimensions.Length,
                sizeY = parcel.Dimensions.Width,
                sizeZ = parcel.Dimensions.Height,
                weight = parcel.Weight.Amount,
                customerData1 = shipment.Receiver.Email
            });
        });

        var request = new generatePackagesNumbersV9
        {
            langCode = "PL",
            authDataV1 = new authDataV1
            {
                login = _dpdOptions.Login,
                password = _dpdOptions.Password,
                masterFid = _dpdOptions.MasterFid,
                masterFidSpecified = true
            },
            pkgNumsGenerationPolicyV1 = pkgNumsGenerationPolicyV1.ALL_OR_NOTHING,
            openUMLFeV11 = new []
            {
                new packageOpenUMLFeV11
                {
                    receiver = new packageAddressOpenUMLFeV1
                    {
                        address = $"{shipment.Receiver.Address.Street} {shipment.Receiver.Address.BuildingNumber}",
                        city = shipment.Receiver.Address.City,
                        postalCode = NormalizePostalCode(shipment.Receiver.Address.PostCode),
                        email = shipment.Receiver.Email,
                        name = $"{shipment.Receiver.FirstName} {shipment.Receiver.LastName}",
                        company = shipment.Receiver.CompanyName,
                        phone = shipment.Receiver.Phone,
                        countryCode = "PL",
                        fidSpecified = false
                    },
                    sender = new packageAddressOpenUMLFeV1
                    {
                        address = _dpdOptions.Sender.Address,
                        city = _dpdOptions.Sender.City,
                        postalCode = NormalizePostalCode(_dpdOptions.Sender.PostalCode),
                        countryCode = _dpdOptions.Sender.CountryCode,
                        company = _dpdOptions.Sender.Company,
                        name = _dpdOptions.Sender.Name,
                        email = _dpdOptions.Sender.Email,
                        phone = _dpdOptions.Sender.PhoneNumber,
                        fid = _dpdOptions.MasterFid,
                        fidSpecified = true,
                    },
                    services = new servicesOpenUMLFeV11
                    {
                        cod = shipment.Cod is null ? null : new serviceCODOpenUMLFeV1 
                        { 
                            amount = shipment.Cod.Amount.ToString(CultureInfo.InvariantCulture), 
                            currency = serviceCurrencyEnum.PLN, 
                            currencySpecified = true
                        },
                        declaredValue = shipment.Insurance is null ? null : new serviceDeclaredValueOpenUMLFeV1 
                        { 
                            amount  = shipment.Insurance.Amount.ToString(CultureInfo.InvariantCulture), 
                            currency = serviceCurrencyEnum.PLN, 
                            currencySpecified = true 
                        }
                    },
                    ref1 = shipment.Receiver.Email,
                    payerType = payerTypeEnumOpenUMLFeV1.SENDER,
                    payerTypeSpecified = true,
                    parcels = parcelList.ToArray()
                }
            }
        };

        return request;
    }
    
    private generateSpedLabelsV4 PrepareRequestForSpedLabelsGeneration(SessionNumber sessionNumber)
    {
        var request = new generateSpedLabelsV4
        {
            authDataV1 = new authDataV1
            {
                login = _dpdOptions.Login,
                password = _dpdOptions.Password,
                masterFid = _dpdOptions.MasterFid,
                masterFidSpecified = true
            },
            outputDocFormatV1 = outputDocFormatDSPEnumV1.PDF,
            outputDocPageFormatV1 = outputDocPageFormatDSPEnumV1.A4,
            outputLabelType = outputLabelTypeEnumV1.BIC3,
            labelVariant = null,
            dpdServicesParamsV1 = new dpdServicesParamsV1
            {
                session = new sessionDSPV1
                {
                    sessionId = sessionNumber.Value,
                    sessionIdSpecified = true,
                    sessionType = sessionTypeDSPEnumV1.DOMESTIC,
                    sessionTypeSpecified = true,
                    packages = null
                },
                policy = policyDSPEnumV1.STOP_ON_FIRST_ERROR,
                policySpecified = true
            }
        };

        return request;
    }

    private static CreateDpdShipmentResponse PrepareResponse(generatePackagesNumbersV9Response response)
    {
        var packagesResponse = new List<CreateDpdShipmentPackageResponse>();
        
        foreach (var package in response.@return.Packages)
        {
            var parcelsResponse = package
                .Parcels
                .Select(parcel => new CreateDpdShipmentParcelResponse
                {
                    ParcelId = parcel.ParcelId, 
                    Waybill = parcel.Waybill
                }).ToList();

            packagesResponse.Add(new CreateDpdShipmentPackageResponse
            {
                PackageId = package.PackageId,
                Parcels = parcelsResponse
            });
        }
        
        return new CreateDpdShipmentResponse
        {
            SessionId = response.@return.SessionId,
            Packages = packagesResponse
        };
    }

    private static string NormalizePostalCode(string postalCode)
    {
        return postalCode
            .Replace("-", "")
            .Replace(" ", "");
    }

    private static IEnumerable<string> GetErrorDetails(generatePackagesNumbersV9Response response)
    {
        var errorDetails = new List<string>();

        foreach (var packageResponse in response.@return.Packages)
        {
            if (packageResponse.ValidationDetails is not null && packageResponse.ValidationDetails.Any())
            {
                errorDetails.AddRange(packageResponse.ValidationDetails.Select(x => x.Info));
            }
                
            foreach (var parcelResponse in packageResponse.Parcels)
            {
                if (parcelResponse.ValidationDetails is not null && parcelResponse.ValidationDetails.Any())
                {
                    errorDetails.AddRange(parcelResponse.ValidationDetails.Select(x => x.Info));
                }
            }
        }

        return errorDetails;
    }
}