using Sistran.Core.Application.ReversionEndorsement.EEProvider;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.VehicleEndorsementReversionService3GProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.VehicleEndorsementReversionService.EEProvider
{
    public class VehicleReversionServiceEEProvider : ReversionEndorsementEEProvider, IVehicleReversionService
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