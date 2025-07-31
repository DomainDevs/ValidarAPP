using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Co.Application.Data;
using System.Data;


namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs
{
    public class SalesPointDAO
    {
        public bool GetSalesPointDAO(int SalesPointId, int BranchId)
        {
            try
            {
                bool resultValue = false;
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@SALESPOINTID", SalesPointId);
                parameters[1] = new NameValue("@BRANCHID", BranchId);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                   var result = dynamicDataAccess.ExecuteSPDataTable("COMM.VALIDATE_SALESPOINT", parameters);
                    if (result != null && result.Rows.Count > 0)
                    {
                        resultValue = true;
                    }
                }
                return resultValue;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
