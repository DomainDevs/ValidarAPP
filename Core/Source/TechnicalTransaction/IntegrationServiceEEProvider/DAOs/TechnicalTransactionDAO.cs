using Sistran.Co.Application.Data;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Models;
using System.Data;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.DAOs
{
    public class TechnicalTransactionDAO
    {
        public TechnicalTransaction GetTechnicalTransaction(TechnicalTransactionParameter parameter)
        {
            if (parameter.BranchId <= 0)
            {
                throw new BusinessException("BranchId es 0");
            }

            TechnicalTransaction technicalTransaction = new TechnicalTransaction();

            DataTable result = null;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("BRANCH_CD", parameter.BranchId);

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("COMM.GET_TRANSACTION_CD", parameters);
            }

            if (result.Rows.Count > 0)
            {
                technicalTransaction.Id = (int)result.Rows[0].ItemArray[0];
            }
            return technicalTransaction;
        }
    }
}
