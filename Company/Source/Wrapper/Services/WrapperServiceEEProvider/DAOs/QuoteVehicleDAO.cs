using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.WrapperServices.EEProvider.Assemblers;
using Sistran.Company.Application.WrapperServices.Models;
using System.Diagnostics;

namespace Sistran.Company.Application.WrapperServices.EEProvider.DAOs
{
    public class QuoteVehicleDAO
    {
        /// <summary>
        /// Cotizar Póliza Autos
        /// </summary>
        /// <param name="requestQuoteVehicle">Datos Cotización</param>
        /// <returns>Cotización</returns>
        public QuoteVehicleResponse QuoteVehicle(QuoteVehicleRequest requestQuoteVehicle)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            CompanyVehicle companyVehicle = ModelAssembler.CreateCompanyVehicleFromRequestQuoteVehicle(requestQuoteVehicle);
            companyVehicle = DelegateService.quotationService.QuoteVehicle(companyVehicle);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.WrapperServices.EEProvider.DAOs.QuoteVehicle");

            return ModelAssembler.CreateQuoteVehicleResponseFromCompanyVehicle(companyVehicle);
        }

        /// <summary>
        /// Tarifar Póliza Autos sin reglas
        /// </summary>
        /// <param name="vehiclePolicy">poliza de vehiculos</param>
        /// <returns>Tarifación</returns>        
        public CompanyVehicle Quotate(CompanyVehicle companyVehicle)
        {
            return DelegateService.vehicleService.QuotateVehicle(companyVehicle, true, true, 0);
        }
    }
}