using Sistran.Company.Application.AdjustmentBusinessService;
using Sistran.Company.Application.AdjustmentBusinessServiceEEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentBusinessServiceEEProvider
{
    public class AdjustmentBusinessServiceEEProvider: IAdjustmentBusinessService
    {
        public int go(int a) { return a; }

        public CompanyPolicy CreateEndorsementAdjustment(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
        {
            try
            {
                AjustmentBusiness adjustmentBusiness = new AjustmentBusiness();
                return adjustmentBusiness.CreateEndorsementAdjustment(companyPolicy, formValues);
            }
            catch (Exception ex)
            {
                throw;
                //throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdjustmentEndorsement), ex);
            }

        }
    }
}
