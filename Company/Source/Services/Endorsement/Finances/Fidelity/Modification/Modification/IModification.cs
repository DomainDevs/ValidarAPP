using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ServiceModel;
using LEM = Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Core.Application.FidelityEndorsementModificationService;

namespace Sistran.Company.Application.FidelityModificationService
{
    [ServiceContract]
    public interface IFidelityModificationServiceCia : IFidelityModificationService
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
        /// <param name="fidelityPolicy">The fidelity policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        [OperationContract]
        LEM.CompanyFidelityRisk GetDataModification(LEM.CompanyFidelityRisk risk,  CoverageStatusType coverageStatusType);


    }
}
