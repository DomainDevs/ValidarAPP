using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;
using System;
using System.Data;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class CommisionDAO
    {
        /// <summary>
        /// GetPendingCommission
        /// Obtiene la comision pendiente, a partir de la poliza y el endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>PendingCommissionDTO</returns>
        public static SEARCH.PendingCommissionDTO GetPendingCommission(int policyId, int endorsementId)
        {

            decimal pendingCommission = 0;
            decimal commissionPercentage = 0;
            decimal agentParticipationPercentage = 0;

            SEARCH.PendingCommissionDTO pendingCommissionDTO = new SEARCH.PendingCommissionDTO();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.PolicyId, policyId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.EndorsementId, endorsementId);

            UIView agentCommissions = DataFacadeManager.Instance.GetDataFacade().GetView("AgentCommissionView",
                         criteriaBuilder.GetPredicate(), null, 0, 10, null, true, out int rows);

            foreach (DataRow dataRow in agentCommissions)
            {
                pendingCommission = pendingCommission + Convert.ToDecimal(Convert.ToDecimal(dataRow["AgentCommissionAmount"]));
                commissionPercentage = Convert.ToDecimal(Convert.ToDecimal(dataRow["CommissionPercentage"]));
                agentParticipationPercentage = Convert.ToDecimal(Convert.ToDecimal(dataRow["AgentParticipationPercentage"]));
            }

            pendingCommissionDTO.PendingCommission = pendingCommission;
            pendingCommissionDTO.CommissionPercentage = commissionPercentage;
            pendingCommissionDTO.AgentParticipationPercentage = agentParticipationPercentage;

            return pendingCommissionDTO;


        }
    }
}
