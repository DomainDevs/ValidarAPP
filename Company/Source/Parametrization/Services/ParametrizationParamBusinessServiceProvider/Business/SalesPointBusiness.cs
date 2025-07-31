using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs;

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Business
{
    public class SalesPointBusiness
    {
        public bool GetSalesPointIdBusiness(int SalesPointId, int BranchId)
        {
            try
            {
                SalesPointDAO salesPointDAO = new SalesPointDAO();
                return salesPointDAO.GetSalesPointDAO(SalesPointId, BranchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
