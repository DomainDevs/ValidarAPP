using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.PropertyEndorsementModificationService;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ServiceModel;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Company.Application.PropertyModificationService
{
    [ServiceContract]
    public interface IPropertyModificationServiceCia : IPropertyModificationService
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
        /// <param name="vehiclePolicy">The vehicle policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        [OperationContract]
        PEM.CompanyPropertyRisk GetDataModification(PEM.CompanyPropertyRisk risk,  CoverageStatusType coverageStatusType);


    }
}
