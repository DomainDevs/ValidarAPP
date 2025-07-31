using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Transports.TransportBusinessService;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices
{
    [ServiceContract]
    public interface ICompanyTransportDeclarationBusinessService 
    {
        /// <summary>
        /// Crea un Endorsement
        /// </summary>
        /// <param name="companyTransport"></param>}
        /// <param name="formValues">Diccionario <String-object></String></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy, Dictionary<string,object> formValues);
    }
}