using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ChangeConsolidationEndorsement
{
    [ServiceContract]
    public interface ICiaChangeConsolidationEndorsement
    {
        /// <summary>
        /// Tarifar Traslado de Afianzado de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyRisk> QuotateChangeConsolidationCia(CompanyPolicy policy);
    }
}
