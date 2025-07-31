using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.ChangeTermEndorsement;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ChangeTermEndorsement
{
    [ServiceContract]
    public interface IChangeTermEndorsementCompany : IChangeTermEndorsement
    {
        /// <summary>
        /// Tarifar Traslado de Vigencia de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyRisk> QuotateChangeTermCia(CompanyPolicy policy);

    }
}
