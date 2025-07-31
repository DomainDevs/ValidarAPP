namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Business
{
    using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.UnderwritingServices.EEProvider;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Collections.Generic;

    public class PersonBusiness
    {
        UnderwritingServiceEEProviderCore underwritingServiceEEProviderCore = new UnderwritingServiceEEProviderCore();
        #region personas
        #region Beneficiarios 
        public System.Collections.Generic.List<CompanyBeneficiary> GetCompanyBeneficiariesByDescription(string individualId, InsuredSearchType insuredSearchType)
        {
            List<Beneficiary> companyBeneficiary = underwritingServiceEEProviderCore.GetBeneficiariesByDescription(individualId, insuredSearchType);
            return ModelAssembler.CreateCompanyBeneficiaries(companyBeneficiary);
        }

        #endregion Beneficiarios
        #endregion personas
    }
}
