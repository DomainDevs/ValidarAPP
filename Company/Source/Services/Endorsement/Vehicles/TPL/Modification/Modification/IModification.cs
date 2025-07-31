using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.ThirdPartyLiabilityEndorsementModificationService;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ServiceModel;
using TPLEM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;

namespace Sistran.Company.Application.ThirdPartyLiabilityModificationService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityModificationServiceCia : IThirdPartyLiabilityModificationService
    {

        /// <summary>
        /// Creacion Temporal, endoso Modificacion
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive);

        /// <summary>
        /// Gets the data modification.
        /// </summary>
        /// <param name="risk">The risk.</param>       
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        [OperationContract]
        TPLEM.CompanyTplRisk GetDataModification(TPLEM.CompanyTplRisk risk,  CoverageStatusType coverageStatusType);


    }
}
