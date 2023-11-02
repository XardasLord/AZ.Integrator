﻿using AZ.Integrator.Application.Common.ExternalServices.SubiektGT;
using AZ.Integrator.Infrastructure.ExternalServices.SubiektGT.Exceptions;
using InsERT;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Infrastructure.ExternalServices.SubiektGT
{
    internal class SubiektService : ISubiektService, IDisposable
    {
        private readonly SubiektOptions _subiektOptions;
        private Thread _subiektThread;

        public SubiektService(IOptions<SubiektOptions> subiektSettings)
        {
            _subiektOptions = subiektSettings.Value;
        }

        public Task<string> GenerateInvoice()
        {
            if (_subiektThread is not null)
                throw new SubiektOperationException("Subiekt's Thread is already running.");
            
            _subiektThread = new Thread(() =>
            {
                try
                {
                    var subiekt = RunSubiekt();
                
                    var oneTimeClient = subiekt.KontrahenciManager.DodajKontrahentaJednorazowego();
                    oneTimeClient.Nazwa = "Test kontrahent";
                    oneTimeClient.Zapisz();
                
                    var invoice = subiekt.SuDokumentyManager.DodajFS();
                    invoice.KontrahentId = oneTimeClient.Identyfikator;
                
                    invoice.Zapisz();
                    invoice.Wyswietl();
                    invoice.Zamknij();
                }
                catch (Exception e)
                {
                    throw new SubiektOperationException(e.Message);
                }
            });

            _subiektThread.SetApartmentState(ApartmentState.STA);
            _subiektThread.Start();
            _subiektThread.Join();

            return Task.FromResult("Test");
        }

        public Task PrintInvoice(string documentNumber)
        {
            throw new NotImplementedException();
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

        public void Dispose()
        {
            if (_subiektThread is null)
                return;

            _subiektThread = null;
        }
    }
}
