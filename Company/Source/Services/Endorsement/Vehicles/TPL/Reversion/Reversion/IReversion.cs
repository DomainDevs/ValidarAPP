using Sistran.Company.Application.ReversionEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ThirdPartyLiabilityReversionService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityReversionServiceCia : ICiaReversionEndorsement
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        CompanyPolicy CreateEndorsementReversion(CompanyEndorsement policy, bool clearPolicies);
    }
}