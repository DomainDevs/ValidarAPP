using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using JSEM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
namespace Sistran.Company.Application.JudicialSuretyTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
    using Sistran.Company.Application.JudicialSuretyTextService.EEProvider.Resources;
    using Sistran.Company.Application.JudicialSuretyTextService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    using CVSE = Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider;
    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class JudicialSuretyTextBusinessCompany
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="JudicialSuretyEndorsementBusinessCompany" /> class.
        /// </summary>
        public JudicialSuretyTextBusinessCompany()
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
                CompanyPolicy policy = new CompanyPolicy();
                CTSE.CiaTextEndorsementEEProvider CompanyTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
                policy = CompanyTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<JSEM.CompanyJudgement> companySuretys = null;
                companySuretys = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyEndorsement.PolicyId);
                if (companySuretys != null && companySuretys.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    companySuretys.AsParallel().ForAll(item =>
                    {
                        item.Risk.Policy = policy;
                        item.Risk.Status = RiskStatusType.Modified;
                        item = DelegateService.judicialsuretyModificationService.GetDataModification(item, CoverageStatusType.Modified);
                        foreach (CompanyCoverage itemCoverage in item.Risk.Coverages)
                        {
                            itemCoverage.EndorsementLimitAmount = itemCoverage.LimitAmount;
                            itemCoverage.EndorsementSublimitAmount = itemCoverage.SubLimitAmount;
                        }
                        var surety = DelegateService.judicialsuretyService.CreateJudgementTemporal(item, false);
                    });
                    risks = companySuretys.Select(x => x.Risk).ToList();
                    policy = provider.CalculatePolicyAmounts(policy, risks);
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    List<JSEM.CompanyJudgement> suretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(policy.Id);

                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                    if (message != string.Empty)
                    {
                        if(message !="Las Fechas Hasta de la cobertura, no esta dentro de la vigencia de la poliza")
                        throw new Exception(message);
                    }
                    CVSE.JudicialSuretyServiceEEProvider judicialsuretyServiceEEProvider = new CVSE.JudicialSuretyServiceEEProvider();
                    return judicialsuretyServiceEEProvider.CreateEndorsement(policy, suretys);
                }
                else
                {
                    throw new Exception(Errors.ErrorSuretyNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
