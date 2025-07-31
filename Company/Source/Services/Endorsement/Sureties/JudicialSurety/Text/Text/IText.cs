using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.JudicialSuretyTextService
{
    [ServiceContract]
    public interface IJudicialSuretyTextServiceCompany
    {

        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateTexts(CompanyEndorsement companyEndorsement);

    }
}
