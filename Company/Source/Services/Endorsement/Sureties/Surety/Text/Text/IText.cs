using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.SuretyTextService
{
    [ServiceContract]
    public interface ISuretyTextServiceCia
    {

        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicyResult CreateTexts(CompanyEndorsement companyEndorsement, CompanyModification companyModification);

    }
}
