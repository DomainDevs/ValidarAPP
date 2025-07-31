using Sistran.Company.Application.UnderwritingServices.Models;

using System.ServiceModel;

namespace Sistran.Company.Application.PropertyEndorsementRenewalService
{
    [ServiceContract]
    public interface IPropertyRenewalServiceCia
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);

    }
}
