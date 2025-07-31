using Sistran.Core.Application.ReversionEndorsement.EEProvider;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.ThirdPartyLiabilityEndorsementReversionService3GProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.ThirdPartyLiabilityEndorsementReversionService.EEProvider
{
    public class ThirdPartyLiabilityReversionServiceEEProvider : ReversionEndorsementEEProvider, IThirdPartyLiabilityReversionService
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        public Policy CreateEndorsementReversion(Policy policy, string userName)
        {
            try
            {
                ReversionDAO reversionDAO = new ReversionDAO();
                policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                return reversionDAO.CreateEndorsementReversion(policy, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}