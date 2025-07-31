
using Sistran.Company.Application.AdjustmentApplicationService.DTO;
using PROP = Sistran.Company.Application.Location.PropertyServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentApplicationService

{
    [ServiceContract]
    public interface IPropertyAdjustmentApplicationService
    {


        [OperationContract]
        AdjustmentDTO GetRisksByTemporalId(int temporalId, bool isMasive);
        /// <summary>
        /// crear temporal
        /// </summary>
        /// <param name="adjustmentDTO"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO CreateTemporal(AdjustmentDTO adjustmentDTO);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adjustmentDTO"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO QuotateAdjustment(AdjustmentDTO adjustmentDTO);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="currentFrom"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO GetPropertyRisksByPolicyId(int policyId, string currentFrom);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrentFrom"></param>
        /// <param name="CurrentTo"></param>
        /// <param name="BillingPeriodId"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO CalculateDays(string CurrentFrom, string CurrentTo, int BillingPeriodId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDetailsDTO GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="isMasive"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO GetPropertyRiskByTemporalId(int temporalId, bool isMasive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PROP.InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentDTO GetAdjustmentEndorsementByPolicyId(int policyId);
    }
}
