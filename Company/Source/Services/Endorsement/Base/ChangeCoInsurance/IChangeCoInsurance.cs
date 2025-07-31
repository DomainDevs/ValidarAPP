using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ChangeCoInsuranceEndorsement
{
    [ServiceContract]
    public interface ICiaChangeCoinsuranceEndorsement
    {
        /// <summary>
        /// Tarifar Traslado de Intermediarios de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyRisk> QuotateChangeCoinsuranceCia(CompanyPolicy policy);
    }
}
