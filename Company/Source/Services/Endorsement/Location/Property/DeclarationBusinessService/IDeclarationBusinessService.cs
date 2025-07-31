using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.DeclarationBusinessService
{
    [ServiceContract]
    public interface IDeclarationBusinessService
    {
        [OperationContract]
        CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy, Dictionary<string, object> formValues);
        /// <summary>
        /// Valida si algun objeto del seguro de todos los riesgos activos de una poliza es declarativo
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateDeclarativeInsuredObjects(decimal policyId);
    }
}
