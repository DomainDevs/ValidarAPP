using OperatingQuoteWebServices.Delegate;
using OperatingQuoteWebServices.Models;
using Sistran.Co.Previsora.Application.FullServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OperatingQuoteWebServices
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "OperatingQuoteWebServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione OperatingQuoteWebServices.svc o OperatingQuoteWebServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class OperatingQuoteWebServices : IOperatingQuoteWebServices
    {
      
        public double GetExchangeRateDate(DateTime operatingQuotaExchangeRateDate, int currencyCd)
        {
            try
            {
                return DelegateService.fullServicesSupProvider.GetExchangeRateDate(operatingQuotaExchangeRateDate, currencyCd);

            }
            catch (Exception ex)
            {
                throw new Exception("Error en consulta de la Tasa de Cambio", ex);
            }
        }

        public List<OperatingQuotaIndividual> GetIndividualOperatingQuota(int identificationType, string identificationId)
        {
            try
            {
                return DelegateService.fullServicesSupProvider.GetIndividualOperatingQuota(identificationType, identificationId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Base de Datos", ex);

            }
        }

        public bool GetStatusAplication(StatusAplicationDTO statusaplication)
        {
            try
            {
                return DelegateService.fullServicesSupProvider.GetStatusAplication(new StatusAplication(statusaplication.IdApplication, statusaplication.KeyApplication));
            }
            catch (Exception ex)
            {
                throw new Exception("Error en consulta SUP.APLICATIONS", ex);
            }

        }

        public List<TableMessage> ModifyOperatingQuota(List<OPERATING_QUOTA> operatingQuota, List<TableMessage> listTableMessage)
        {
            return DelegateService.fullServicesSupProvider.ModifyOperatingQuota(operatingQuota, listTableMessage);
        }

        public OperatingQuotaResponse RegisterOperativeQuota(WSOperatingQuota operatingQuota)
        {
            return DelegateService.fullServicesSupProvider.RegisterOperativeQuota(operatingQuota);
        }
    }
}
