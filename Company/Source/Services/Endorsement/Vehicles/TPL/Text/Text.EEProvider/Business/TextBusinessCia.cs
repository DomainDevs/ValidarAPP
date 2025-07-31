using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TPLEM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;

namespace Sistran.Company.Application.ThirdPartyLiabilityTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.ThirdPartyLiabilityTextService.EEProvider.Resources;
    using Sistran.Company.Application.ThirdPartyLiabilityTextService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    using CTPLSE = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider;
    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class ThirdPartyLiabilityTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyLiabilityEndorsementBusinessCia" /> class.
        /// </summary>
        public ThirdPartyLiabilityTextBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicyResult CreateEndorsementText(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CTSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
                policy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<TPLEM.CompanyTplRisk> companyTpls = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(companyEndorsement.PolicyId);

                if (companyTpls != null && companyTpls.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    companyTpls.AsParallel().ForAll(item =>
                    {
                        item.Risk.Policy = policy;
                        item.Risk.Status = RiskStatusType.Modified;
                        item = DelegateService.tplModificationService.GetDataModification(item, CoverageStatusType.Modified);
                        var tpl = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(item, false);
                    });
                    risks = companyTpls.Select(x => x.Risk).ToList();
                    policy = provider.CalculatePolicyAmounts(policy, risks);
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                    if (message != string.Empty)
                    {
                        throw new Exception(message);
                    }
                    CTPLSE.ThirdPartyLiabilityServiceEEProvider TplServiceEEProvideR = new CTPLSE.ThirdPartyLiabilityServiceEEProvider();
                    return TplServiceEEProvideR.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                }
                else
                {
                    throw new Exception(Errors.ErrorThirdPartyLiabilityNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
