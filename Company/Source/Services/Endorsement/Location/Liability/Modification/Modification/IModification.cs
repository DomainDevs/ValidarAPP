using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ServiceModel;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Core.Application.LiabilityEndorsementModificationService;

namespace Sistran.Company.Application.LiabilityModificationService
{
    [ServiceContract]
    public interface ILiabilityModificationServiceCia : ILiabilityModificationService
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
        /// <param name="liabilityPolicy">The liability policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        [OperationContract]
        LEM.CompanyLiabilityRisk GetDataModification(LEM.CompanyLiabilityRisk risk,  CoverageStatusType coverageStatusType);


    }
}
