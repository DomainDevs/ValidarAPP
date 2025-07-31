using Sistran.Core.Application.ReversionEndorsement.EEProvider;
using Sistran.Core.Application.SuretyEndorsementReversionService.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System.Collections.Generic;

namespace Sistran.Core.Application.SuretyEndorsementReversionService.EEProvider
{
    public class SuretyReversionServiceEEProvider : ReversionEndorsementEEProvider, ISuretyReversionService
    {
        

        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        public List<UnderwritingServices.Models.Policy> CreateTemporalEndorsementReversion(UnderwritingServices.Models.Policy policy, string userName)
        {
            try
            {
                ReversionDAO reversionDAO = new ReversionDAO();
                policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                return reversionDAO.CreateTemporalsEndorsementReversion(policy, userName);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, SuretyEndorsementReversionService3GProvider.Resources.Errors.CreateTemporalReversionSurety), ex);
            }
        }

        public Policy CreateEndorsementReversion(Policy policy, string userName)
        {
            try
            {
                ReversionDAO reversionDAO = new ReversionDAO();
                policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                return reversionDAO.CreateEndorsementReversion(policy, userName);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, SuretyEndorsementReversionService3GProvider.Resources.Errors.CreateTemporalReversionSurety), ex);
            }
        }
    }
}
