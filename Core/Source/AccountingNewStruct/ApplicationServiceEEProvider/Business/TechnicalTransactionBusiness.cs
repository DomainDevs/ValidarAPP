using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class TechnicalTransactionBusiness
    {
        public int GetTechnicalTransaction(int branchId)
        {
            TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
            {
                BranchId = branchId
            };
            TechnicalTransactionDTO technicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter);
            return technicalTransaction.Id;
        }
    }
}
