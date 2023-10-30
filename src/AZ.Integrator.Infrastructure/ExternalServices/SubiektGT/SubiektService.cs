using AZ.Integrator.Infrastructure.ExternalServices.SubiektGT.Exceptions;
using InsERT;

namespace AZ.Integrator.Infrastructure.ExternalServices.SubiektGT
{
    public class SubiektService
    {
        public SubiektService()
        {
            
        }

        public void OpenSubiekt()
        {
            var subiektThread = new Thread(() => RunSubiekt());

            subiektThread.SetApartmentState(ApartmentState.STA);
            subiektThread.Start();
            subiektThread.Join();
            
            // var subiekt = RunSubiekt();
        }

        private Subiekt RunSubiekt()
        {
            var gt = new GT
            {
                Produkt = ProduktEnum.gtaProduktSubiekt,
                Serwer = "localhost\\SQLEXPRESS",
                Baza = "TEST_GRATYFIKANT",
                Autentykacja = AutentykacjaEnum.gtaAutentykacjaWindows,
                Uzytkownik = "sa",
                Operator = "szef",
                OperatorHaslo = ""
            };

            if (gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (int)UruchomEnum.gtaUruchom) is not Subiekt subiekt)
                throw new SubiektOperationException("Subiekt cannot be opened");
            
            subiekt.Okno.Widoczne = true;

            return subiekt;
        }
    }
}
