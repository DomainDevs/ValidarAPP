using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ChangePolicyHolderEndorsement
{
    [ServiceContract]
    public interface ICiaChangePolicyHolderEndorsement
    {
        /// <summary>
        /// Tarifar Traslado de Afianzado de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyRisk> QuotateChangePolicyHolderCia(CompanyPolicy policy);
    }
}
