using Sistran.Core.Application.ReversionEndorsement.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.JudicialEndorsementReversionService3GProvider.DAOs;
using System.Collections.Generic;

namespace Sistran.Core.Application.JudicialEndorsementReversionService.EEProvider
{
    public class JudicialReversionServiceEEProvider : ReversionEndorsementEEProvider, IJudicialReversionService
    {
        

        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        public UnderwritingServices.Models.Policy CreateEndorsementReversion(UnderwritingServices.Models.Policy policy, string userName)
        {
            try
            {
                ReversionDAO reversionDAO = new ReversionDAO();
                policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                return reversionDAO.CreateEndorsementReversion(policy, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, JudicialEndorsementReversionService3GProvider.Resources.Errors.CreateTemporalReversionJudicialSurety), ex);
            }
        }
    }
}
