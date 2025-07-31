using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;
namespace Sistran.Company.Application.LiabilityTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.LiabilityTextService.EEProvider.Resources;
    using Sistran.Company.Application.LiabilityTextService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    using ClibialitySE = Sistran.Company.Application.Location.LiabilityServices.EEProvider;
    using System.Collections.Concurrent;
    using Sistran.Core.Services.UtilitiesServices.Enums;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class LiabilityTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiabilityEndorsementBusinessCia" /> class.
        /// </summary>
        public LiabilityTextBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicyResult CreateEndorsementText(CompanyEndorsement companyEndorsement, CompanyModification companyModification)
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
                List<LEM.CompanyLiabilityRisk> companyLibialitys = null;
                if (companyEndorsement.TemporalId != 0)
                {
                    companyLibialitys = DelegateService.LibialityService.GetCompanyLiabilitiesByTemporalId(companyEndorsement.TemporalId);
                    if (companyLibialitys == null || companyLibialitys.Count < 1)
                    {
                        companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyEndorsement.PolicyId);
                        companyLibialitys.AsParallel().ForAll(
                            x =>
                            {
                                x.Risk.Id = 0;
                                x.Risk.OriginalStatus = x.Risk.Status;
                                x.Risk.Status = RiskStatusType.NotModified;
                            });
                    }

                }
                else
                {
                    companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyEndorsement.PolicyId);

                    companyLibialitys.AsParallel().ForAll(
                           x =>
                           {
                               x.Risk.Id = 0;
                               x.Risk.OriginalStatus = x.Risk.Status;
                               x.Risk.Status = RiskStatusType.NotModified;
                           });

                }
                if (companyLibialitys != null && companyLibialitys.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    ConcurrentBag<string> errorsData = new ConcurrentBag<string>();
                    companyLibialitys.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            item.Risk.Policy = companyPolicy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.libialityModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            foreach (CompanyCoverage itemCoverage in item.Risk.Coverages)
                            {
                                itemCoverage.EndorsementLimitAmount = itemCoverage.LimitAmount;
                                itemCoverage.EndorsementSublimitAmount = itemCoverage.SubLimitAmount;
                            }
                            var libiality = DelegateService.LibialityService.CreateLiabilityTemporal(item, false);
                        }
                        catch (Exception)
                        {

                            errorsData.Add(Errors.ErrorEndorsement);
                        }

                    });
                    risks = companyLibialitys.Select(x => x.Risk).ToList();
                    companyPolicy = provider.CalculatePolicyAmounts(companyPolicy, risks);
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    companyPolicy.IssueDate = companyEndorsement.IssueDate;
                    companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                    List<LEM.CompanyLiabilityRisk> libialitys = DelegateService.LibialityService.GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);

                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(companyPolicy.Id);
                    if (message != string.Empty)
                    {
                        throw new Exception(message);
                    }
                    ClibialitySE.LiabilityServiceEEProvider LibialityServiceEEProvider = new ClibialitySE.LiabilityServiceEEProvider();

                    return LibialityServiceEEProvider.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, false, companyModification);
                }
                else
                {
                    throw new Exception(Errors.ErrorLibialityNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
