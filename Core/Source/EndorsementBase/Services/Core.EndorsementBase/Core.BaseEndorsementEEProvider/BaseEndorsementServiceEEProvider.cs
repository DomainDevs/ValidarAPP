using Newtonsoft.Json;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.BaseEndorsementService.DTOs;
using Sistran.Core.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Core.Application.BaseEndorsementService.EEProvider.DAOs;
using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using EmAutPolicies = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;

namespace Sistran.Core.Application.BaseEndorsementService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class BaseEndorsementServiceEEProvider : IBaseEndorsementService
    {



        /// <summary>
        /// Obtener lista de motivos del endoso
        /// </summary>
        /// <param name="EndorsementType">Tipo de endoso</param>
        /// <returns>Lista de motivos del endoso</returns>
        public List<EndorsementReason> GetEndorsementReasonsByEndorsementType(EndorsementType endorsementType)
        {
            try
            {
                EndorsementReasonDAO endorsementReasonDAO = new EndorsementReasonDAO();
                return endorsementReasonDAO.GetEndorsementReasonsByEndorsementType(endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener temporal de una póliza
        /// </summary>
        /// <param name="policyId">Id póliza</param>
        /// <returns>modelo Endorsement</returns>
        public Endorsement GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                EndorsementDAO endorsementDAO = new EndorsementDAO();
                return endorsementDAO.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Grabar endoso de una póliza
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Número de endoso</returns>
        public Policy CreateEndorsement(int temporalId)
        {
            try
            {
                EndorsementDAO endorsementDAO = new EndorsementDAO();

                Policy policy = new Policy
                {
                    Endorsement = new Endorsement
                    {
                        Number = endorsementDAO.CreateEndorsement(temporalId)
                    },
                    InfringementPolicies = new List<PoliciesAut>()
                };

                return policy;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        protected List<PoliciesAut> GetPendingAuthorizationPolicies(int temporalId)
        {
            try
            {
                var commonService = DelegateService.utilitiesServiceCore;
                PendingOperation pendingOperation = commonService.GetPendingOperationByIdParentId(temporalId, 0);

                Policy policy = JsonConvert.DeserializeObject<Policy>(pendingOperation.Operation);
                policy.Id = pendingOperation.Id;

                List<PendingOperation> pendingOperations = commonService.GetPendingOperationsByParentId(policy.Id);

                foreach (var pendingOperationRisk in pendingOperations)
                {
                    Risk risk = JsonConvert.DeserializeObject<Risk>(pendingOperationRisk.Operation);
                    if (risk.InfringementPolicies != null)
                    {
                        if (policy.InfringementPolicies != null)
                            policy.InfringementPolicies.AddRange(risk.InfringementPolicies);
                        else
                            policy.InfringementPolicies = risk.InfringementPolicies;
                    }
                }

                if (policy.InfringementPolicies == null)
                    return new List<PoliciesAut>();

                return policy.InfringementPolicies.Where(x => x.Type == EmAutPolicies.TypePolicies.Authorization || x.Type == EmAutPolicies.TypePolicies.Restrictive).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException("Error in GetPendingPolicies", ex);
            }
        }

        /// <summary>
        /// Gets the type of the modification.
        /// </summary>
        /// <returns></returns>
        public List<EndorsementTypeDTO> GetModificationType()
        {
            try
            {
                return EndorsementTypeBusiness.GetModificationType();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
