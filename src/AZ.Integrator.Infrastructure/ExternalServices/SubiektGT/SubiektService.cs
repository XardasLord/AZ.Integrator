using System.Globalization;
using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using AZ.Integrator.Application.Common.ExternalServices.SubiektGT;
using AZ.Integrator.Infrastructure.ExternalServices.SubiektGT.Exceptions;
using InsERT;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Infrastructure.ExternalServices.SubiektGT
{
    internal class SubiektService : ISubiektService
    {
        private readonly SubiektOptions _subiektOptions;

        public SubiektService(IOptions<SubiektOptions> subiektSettings)
        {
            _subiektOptions = subiektSettings.Value;
        }

        public async Task<string> GenerateInvoice(string allegroOrderNumber, BuyerDetails buyerDetails, List<LineItemDetails> lineItems, SummaryDetails summary, PaymentDetails paymentDetails)
        {
            var invoiceNumber = await RunSTATask(() =>
            {
                var subiekt = RunSubiekt();
                
                var oneTimeClient = subiekt.KontrahenciManager.DodajKontrahentaJednorazowego();
                oneTimeClient.Nazwa = buyerDetails.CompanyName?.ToString() ?? $"{buyerDetails.FirstName} {buyerDetails.LastName}";
                oneTimeClient.Ulica = buyerDetails.Address.Street;
                oneTimeClient.Miejscowosc = buyerDetails.Address.City;
                oneTimeClient.KodPocztowy = buyerDetails.Address.PostCode;
                oneTimeClient.NIP = buyerDetails.PersonalIdentity ?? "";
                oneTimeClient.Zapisz();
                    
                var invoice = subiekt.SuDokumentyManager.DodajFS();
                invoice.KontrahentId = oneTimeClient.Identyfikator;

                foreach (var lineItem in lineItems)
                {
                    Towar product;

                    if (subiekt.TowaryManager.IstniejeWg(lineItem.Offer.Id, TowarParamWyszukEnum.gtaTowarWgSymbolu))
                    {
                        product = subiekt.TowaryManager.WczytajTowarWg(lineItem.Offer.Id, TowarParamWyszukEnum.gtaTowarWgSymbolu);
                    }
                    else
                    {
                        product = subiekt.TowaryManager.DodajTowar();
                        product.Symbol = lineItem.Offer.Id;
                        product.Nazwa = lineItem.Offer.Name;
                        product.Opis = lineItem.Offer.External?.Id;
                        product.Zapisz();
                        product.Zamknij();
                    }
                        
                    var invoiceItem = invoice.Pozycje.Dodaj(product) as SuPozycja;
                    invoiceItem.IloscJm = lineItem.Quantity;
                    invoiceItem.CenaBruttoPrzedRabatem = double.Parse(lineItem.Price.Amount, CultureInfo.InvariantCulture);
                }

                if (paymentDetails.Type == "CASH_ON_DELIVERY")
                {
                    invoice.PlatnoscGotowkaKwota = 0;
                    invoice.PlatnoscPrzelewKwota = 0;
                }

                if (paymentDetails.Type == "ONLINE")
                {
                    invoice.PlatnoscGotowkaKwota = 0;
                    invoice.PlatnoscPrzelewKwota = double.Parse(summary.TotalToPay.Amount, CultureInfo.InvariantCulture);
                }
                
                invoice.Uwagi = $"Dotyczy zamówienia z Allegro nr {allegroOrderNumber}";
                
                invoice.Zapisz();
                
                var invoiceNumber = invoice.NumerPelny?.ToString();
                
                invoice.Drukuj(true);
                invoice.Zamknij();

                return invoiceNumber;
            });

            return invoiceNumber;
        }

        private static Task<T> RunSTATask<T>(Func<T> function)
        {
            var task = new Task<T>(function, TaskCreationOptions.DenyChildAttach);
            var thread = new Thread(task.RunSynchronously)
            {
                IsBackground = true
            };
            
#pragma warning disable CA1416
            thread.SetApartmentState(ApartmentState.STA);
#pragma warning restore CA1416
            
            thread.Start();
            
            return task;
        }

        private Subiekt RunSubiekt()
        {
            var gt = new GT
            {
                Produkt = ProduktEnum.gtaProduktSubiekt,
                Serwer = _subiektOptions.Server,
                Baza = _subiektOptions.Database,
                Autentykacja = AutentykacjaEnum.gtaAutentykacjaWindows,
                Uzytkownik = _subiektOptions.User,
                Operator = _subiektOptions.Operator,
                OperatorHaslo = _subiektOptions.OperatorPassword
            };

            if (gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (int)UruchomEnum.gtaUruchom) is not Subiekt subiekt)
                throw new SubiektOperationException("Subiekt cannot be opened.");

            return subiekt;
        }
    }
}
