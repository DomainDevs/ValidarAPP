using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.SuretyEndorsementModificationService;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ServiceModel;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;

namespace Sistran.Company.Application.SuretyModificationService
{
    [ServiceContract]
    public interface ISuretyModificationServiceCia : ISuretyModificationService
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
        /// <param name="suretyPolicy">The surety policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        [OperationContract]
        SEM.CompanyContract GetDataModification(SEM.CompanyContract risk,  CoverageStatusType coverageStatusType);

    }
}
