using Sistran.Company.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.BaseEndorsementService
{
    [ServiceContract]
    public interface IBaseCiaEndorsementService : IBaseEndorsementService
    {
        /// <summary>
        /// Gets the type of the cia endorsement reasons by endorsement.
        /// </summary>
        /// <param name="endorsementType">Type of the endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        List<CiaEndorsementReason> GetCiaEndorsementReasonsByEndorsementType(EndorsementType endorsementType);

        /// <summary>
        /// Validates the endorsement.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        [OperationContract]
        string ValidateEndorsement(int temporalId);
    }
}
