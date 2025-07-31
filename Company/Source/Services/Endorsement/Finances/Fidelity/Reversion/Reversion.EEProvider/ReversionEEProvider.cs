using System;
using System.Collections.Generic;
using Sistran.Company.Application.FidelityReversionService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.FidelityReversionService.EEProvider
{
    public class FidelityReversionServiceEEProvider : IFidelityReversionServiceCia
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
                FidelityReversionBusinessCia FidelityReversionBusinessCia = new FidelityReversionBusinessCia();
                return FidelityReversionBusinessCia.CreateTemporal(policy);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool CreateEndorsementWorkFlow(int? PolicyId, int? EndorsementId, string filingNumber, DateTime filingDate)
        {
            throw new NotImplementedException();
        }

        public List<string> GetEndorsementWorkFlow(int PolyciId)
        {
            throw new NotImplementedException();
        }
    }
}