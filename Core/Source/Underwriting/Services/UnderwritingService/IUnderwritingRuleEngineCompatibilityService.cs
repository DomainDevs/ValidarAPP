using System.ServiceModel;

namespace Sistran.Core.Application.UnderwritingServices
{
    /// <summary>
    /// Funciones
    /// </summary>
    [ServiceContract]
    public interface IUnderwritingRuleEngineCompatibilityService
    {
        /// <summary>
        /// Creates the deductible to coverage.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract]
        object CreateDeductibleToCoverage(object request);

        /// <summary>
        /// Deletes the deductible from coverage.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract]
        object DeleteDeductibleFromCoverage(object request);

        /// <summary>
        /// Creates the coverage set to risk.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract]
        object CreateCoverageSetToRisk(object request);

        /// <summary>
        /// Gets the claims history from2 g.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract]
        object GetClaimsHistoryFrom2G(object request);

        /// <summary>
        /// Gets the count coverages.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract]
        object GetCountCoverages(object request);

        /// <summary>
        /// Gets the minimum premium coverage.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [OperationContract]
        object GetMinimumPremiumCoverage(object request);
    }
}
