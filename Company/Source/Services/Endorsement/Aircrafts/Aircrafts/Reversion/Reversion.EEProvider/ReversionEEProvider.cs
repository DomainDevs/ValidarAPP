using Sistran.Company.Application.ReversionEndorsement.EEProvider;
using Sistran.Company.Application.TranportReversionService.EEProvider.Business;
using Sistran.Company.Application.AircraftReversionService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.TranportReversionService.EEProvider
{
    public class AircraftReversionServiceEEProvider : CiaReversionEndorsementEEProvider, IAircraftReversionService
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        public CompanyPolicy CreateEndorsementReversion(CompanyEndorsement policy)
        {
            try
            {
                TranportReversionBusinessCia aircraftReversionBusinessCia = new TranportReversionBusinessCia();
                return aircraftReversionBusinessCia.CreateTemporal(policy);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}