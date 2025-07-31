using Sistran.Company.Application.ReversionEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.JudicialSuretyReversionService.EEProvider.Business;
using Sistran.Company.Application.JudicialSuretyReversionService.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.JudicialSuretyReversionService.EEProvider
{
    public class JudicialSuretyReversionServiceEEProvider : CiaReversionEndorsementEEProvider, IJudicialSuretyReversionServiceCompany
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        public CompanyPolicy CreateEndorsementReversion(CompanyEndorsement policy, bool clearPolicies)
        {
            try
            {
                SuretyReversionBusinessCompany suretyReversionBusinessCompany = new SuretyReversionBusinessCompany();
                return suretyReversionBusinessCompany.CreateTemporal(policy, clearPolicies);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}