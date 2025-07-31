using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.DAOs;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Models;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Business
{
    public class TechnicalTransactionBusiness
    {
        public TechnicalTransaction GetTechnicalTransaction(TechnicalTransactionParameter technicalTransactionParameter)
        {
            TechnicalTransactionDAO technicalTransactionDAO = new TechnicalTransactionDAO();
            return technicalTransactionDAO.GetTechnicalTransaction(technicalTransactionParameter);
        }
    }
}
