using Sistran.Company.Application.ExtensionEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.PropertytExtensionService
{
    [ServiceContract]
    public interface IPropertyExtensionServiceCia
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);


        /// <summary>
        /// Valida si algun objeto del seguro de todos los riesgos activos de una poliza es declarativo
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateDeclarativeInsuredObjects(decimal policyId);
    }
}
