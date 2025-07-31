using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.Finances.Models;

namespace Sistran.Core.Application.Finances
{
    [ServiceContract]
    public interface IFinancesCore
    {
        /// <summary>
        /// Listado de profesiones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<IssuanceOccupation> GetIssuanceOccupations();
    }
}
