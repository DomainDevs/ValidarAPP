using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using System.ServiceModel;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingComponentDistributionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="quotaNumber"></param>
        /// <param name="currencyId"></param>
        /// <param name="exchangeRate"></param>
        [OperationContract]
        void CreateApplicationPremiumComponent(ParamApplicationPremiumComponent paramApplicationPremiumComponent);
    }
}
