using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using LEM = Sistran.Company.Application.Finances.FidelityServices.Models;
namespace Sistran.Company.Application.FidelityTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.FidelityTextService.EEProvider.Resources;
    using Sistran.Company.Application.FidelityTextService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    using CfidelitySE = Sistran.Company.Application.Finances.FidelityServices.EEProvider;
    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class FidelityTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FidelityEndorsementBusinessCia" /> class.
        /// </summary>
        public FidelityTextBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementText(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy companyPolicy = new CompanyPolicy();
                CTSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
                companyPolicy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<LEM.CompanyFidelityRisk> companyFidelitys = null;
                companyFidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByPolicyId(companyEndorsement.PolicyId);
                if (companyFidelitys != null && companyFidelitys.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    companyFidelitys.AsParallel().ForAll(item =>
                    {
                        item.Risk.Policy = companyPolicy;
                        item.Risk.Status = RiskStatusType.Modified;
                        item = DelegateService.fidelityModificationService.GetDataModification(item, CoverageStatusType.Modified);
                        var fidelity = DelegateService.FidelityService.CreateFidelityTemporal(item, false);
                    });
                    risks = companyFidelitys.Select(x => x.Risk).ToList();
                    companyPolicy = provider.CalculatePolicyAmounts(companyPolicy, risks);
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    List<LEM.CompanyFidelityRisk> fidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByTemporalId(companyPolicy.Id);

                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(companyPolicy.Id);
                    if (message != string.Empty)
                    {
                        throw new Exception(message);
                    }
                    CfidelitySE.FidelityServiceEEProvider FidelityServiceEEProvider = new CfidelitySE.FidelityServiceEEProvider();

                    return FidelityServiceEEProvider.CreateEndorsement(companyPolicy, fidelitys);
                }
                else
                {
                    throw new Exception(Errors.ErrorFidelityNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
