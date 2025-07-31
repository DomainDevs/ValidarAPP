using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.MarineTextService.EEProvider.Services;

namespace Sistran.Company.Application.MarineTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
    using Sistran.Company.Application.MarineTextService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class MarineTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarineEndorsementBusinessCia" /> class.
        /// </summary>
        public MarineTextBusinessCia()
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
                CTSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
                policy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<CompanyMarine> companyMarines = null;
                companyMarines = DelegateService.marineService.GetCompanyMarinesByPolicyId(companyEndorsement.PolicyId);
                if (companyMarines != null && companyMarines.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    companyMarines.AsParallel().ForAll(item =>
                    {
                        item.Risk.Policy = policy;
                        item.Risk.Status = RiskStatusType.Modified;
                        item = DelegateService.marineModificationService.GetDataModification(item, CoverageStatusType.Modified);
                        var marine = DelegateService.marineService.CreateCompanyMarineTemporal(item);
                    });
                    risks = companyMarines.Select(x => x.Risk).ToList();
                    policy = provider.CalculatePolicyAmounts(policy, risks);
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    List<CompanyMarine> marines = DelegateService.marineService.GetCompanyMarinesByTemporalId(policy.Id);

                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                    if (message != string.Empty)
                    {
                        throw new Exception(message);
                    }
                    /// CVSE.MarineServiceEEProvider marineServiceEEProvider = new CVSE.MarineServiceEEProvider();
                    return DelegateService.marineService.CreateEndorsement(policy, marines);
                    
                }
                else
                {
                    throw new Exception(Errors.ErrorMarineNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
