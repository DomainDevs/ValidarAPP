using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.MarineEndorsementRenewalService
{
    [ServiceContract]
    public interface ICiaMarineRenewalService
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);
    }
}
