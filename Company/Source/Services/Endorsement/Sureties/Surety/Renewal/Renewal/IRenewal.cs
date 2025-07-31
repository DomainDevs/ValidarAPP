using Sistran.Company.Application.UnderwritingServices.Models;

using System.ServiceModel;

namespace Sistran.Company.Application.SuretyEndorsementRenewalService
{
    [ServiceContract]
    public interface ISuretyRenewalServiceCompany
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);

    }
}
