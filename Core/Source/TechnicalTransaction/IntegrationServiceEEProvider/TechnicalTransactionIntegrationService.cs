using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Assemblers;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Business;
using Sistran.Core.Application.TechnicalTransactionGeneratorServices.Resources;
using System;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider
{
    public class TechnicalTransactionIntegrationService : ITechnicalTransactionIntegrationService
    {
        public TechnicalTransactionDTO GetTechnicalTransaction(TechnicalTransactionParameterDTO parameters)
        {
            try
            {
                TechnicalTransactionBusiness technicalTransactionBusiness = new TechnicalTransactionBusiness();
                return technicalTransactionBusiness.GetTechnicalTransaction(parameters.ToModel()).ToDTO();
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.ErrorGetTechnicalTransaction, ex);
            }
        }
    }
}
