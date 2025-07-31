using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ClauseEndorsement
{
    [ServiceContract]
    public interface ICiaClauseEndorsement
    {

        /// <summary>
        /// Creates the cia clause.
        /// </summary>
        /// <param name="endorsement">The endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateCiaClause(CompanyPolicy endorsement);
    }
}
