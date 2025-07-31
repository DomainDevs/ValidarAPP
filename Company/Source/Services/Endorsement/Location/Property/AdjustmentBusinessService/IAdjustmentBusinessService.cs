using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentBusinessService
{
    [ServiceContract]
    public interface IAdjustmentBusinessService
    {   
        [OperationContract]
        int go(int a);

        [OperationContract]
        CompanyPolicy CreateEndorsementAdjustment(CompanyPolicy companyPolicy, Dictionary<string, object> formValues);
    }
}
