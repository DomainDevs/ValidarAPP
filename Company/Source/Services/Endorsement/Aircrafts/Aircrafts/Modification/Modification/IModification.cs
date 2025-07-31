using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AircraftEndorsementModificationService;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ServiceModel;

namespace Sistran.Company.Application.AircraftModificationService
{
    [ServiceContract]
    public interface IAircraftModificationServiceCia : IAircraftModificationService
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
        CompanyAircraft GetDataModification(CompanyAircraft risk, CoverageStatusType coverageStatusType);
    }
}
