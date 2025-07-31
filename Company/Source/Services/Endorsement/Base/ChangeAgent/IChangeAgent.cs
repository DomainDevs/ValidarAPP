using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.ChangeAgentEndorsement;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ChangeAgentEndorsement
{
    [ServiceContract]
    public interface ICiaChangeAgentEndorsement : IChangeAgentEndorsement
    {
        /// <summary>
        /// Tarifar Traslado de Intermediarios de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyRisk> QuotateChangeAgentCia(CompanyPolicy policy);

        /// <summary>
        /// Emitir Cambio de Intermediario de la Póliza
        /// </summary>
        /// <param name="Id">Temporal</param>
        /// <returns>Numero Endoso</returns>
        [OperationContract]
        CompanyPolicy ExecuteCia(int Id);
    }
}
