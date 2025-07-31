using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.JudicialSuretyClauseService
{
    [ServiceContract]
    public interface IJudicialSuretyClauseServiceCompany
    {
        /// <summary>
        /// Creacion Temporal, endoso Modificacion
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive);
        [OperationContract]
        CompanyPolicy CreateClauses(CompanyPolicy companyEndorsement);

    }
}
