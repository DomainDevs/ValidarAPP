using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ServiceModel;

namespace Sistran.Company.Application.TransportModificationService
{
    [ServiceContract]
    public interface ITransportModificationServiceCia
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
        CompanyTransport GetDataModification(CompanyTransport risk, CoverageStatusType coverageStatusType);
    }
}
