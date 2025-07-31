using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    internal class CollectControlBusiness
    {
        public DateTime GetLastOpenDateByUserIdBranchId(int userId, int branchId)
        {
            CollectControlDAO collectControlDAO = new CollectControlDAO();
            return collectControlDAO.GetLastOpenDateByUserIdBranchId(userId, branchId);
        }

        public DateTime GetAccountingDateByCollectControlId(int collectControlId)
        {
            CollectControlDAO collectControlDAO = new CollectControlDAO();
            return collectControlDAO.GetCollectControlByCollectControlId(collectControlId).AccountingDate;
        }
        
        public CollectControl GetCollectControlById(int collectControlId)
        {
            CollectControlDAO collectControlDAO = new CollectControlDAO();
            return collectControlDAO.GetCollectControlByCollectControlId(collectControlId);
        }

        public DateTime GetAccountingDateByCollectId(int collectId)
        {
            CollectControlDAO collectControlDAO = new CollectControlDAO();
            return collectControlDAO.GetAccountingDateForCollectControlByCollectId(collectId);
        }
    }
}
