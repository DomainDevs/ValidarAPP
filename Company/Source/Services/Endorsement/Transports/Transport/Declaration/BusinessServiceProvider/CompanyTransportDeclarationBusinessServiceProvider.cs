using Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider.Business;
using Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyTransportDeclarationBusinessServiceProvider : ICompanyTransportDeclarationBusinessService
    {
        public CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy, Dictionary<string, object> formValues)
        {
            try
            {
                DeclarationBusiness declarationTransportBusiness = new DeclarationBusiness();
                return declarationTransportBusiness.CreateEndorsementDeclaration(companyPolicy, formValues);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclarationEndorsement), ex);
            }
        }
    }
}