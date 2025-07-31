using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.DAOs;
using Sistran.Company.Application.EndorsementBaseService.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.BaseEndorsementService.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
namespace Sistran.Company.Application.BaseEndorsementService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class BaseCiaEndorsementServiceEEProvider : BaseEndorsementServiceEEProvider, IBaseCiaEndorsementService
    {
        /// <summary>
        /// Obtener lista de motivos del endoso
        /// </summary>
        /// <param name="EndorsementType">Tipo de endoso</param>
        /// <returns>Lista de motivos del endoso</returns>
        public List<CiaEndorsementReason> GetCiaEndorsementReasonsByEndorsementType(EndorsementType endorsementType)
        {
            try
            {
                CiaEndorsementReasonDAO endorsementReasonDAO = new CiaEndorsementReasonDAO();
                return endorsementReasonDAO.GetEndorsementReasonsByEndorsementType(endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calculates the policy amounts.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="risks">The risks.</param>
        /// <returns></returns>
        public CompanyPolicy CalculatePolicyAmounts(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            BaseBusinessCia baseBusinessCia = new BaseBusinessCia();
            return baseBusinessCia.CalculatePolicyAmounts(policy, risks);

        }

        /// <summary>
        /// Validates the endorsement.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        public string ValidateEndorsement(int temporalId)
        {
            BaseBusinessCia baseBusinessCia = new BaseBusinessCia();
            return baseBusinessCia.ValidateEndorsement(temporalId);
        }

    }
}
