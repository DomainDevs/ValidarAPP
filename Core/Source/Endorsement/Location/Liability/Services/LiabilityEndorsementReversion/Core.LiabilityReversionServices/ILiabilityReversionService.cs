using Sistran.Core.Application.ReversionEndorsement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.LiabilityEndorsementReversionService
{
    [ServiceContract]
    public interface ILiabilityReversionService : IReversionEndorsement
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        Policy CreateEndorsementReversion(Policy policy, string userName);
    }
}