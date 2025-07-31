using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.PropertyCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TM=System.Threading.Tasks;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.PropertyCancellationService.EEProvider.Business
{
    public class PropertyCancellationBusinessCia
    {

        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEndorsementBusinessCia" /> class.
        /// </summary>
        public PropertyCancellationBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement)
        {
            try
            {
                var companyPolicy = new CompanyPolicy();

                if (companyEndorsement == null)
                {
                    throw new ArgumentException("");
                }

                if (companyEndorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);

                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                    companyPolicy.Id = 0;
                    companyPolicy.Endorsement.TemporalId = 0;
                    companyPolicy.UserId = companyEndorsement.UserId;
                }
                    companyPolicy.Endorsement = new CompanyEndorsement
                {

                    Id = companyEndorsement.Id,
                    PolicyId = companyEndorsement.PolicyId,
                    CancellationTypeId = companyEndorsement.CancellationTypeId,
                    EndorsementReasonId = companyEndorsement.EndorsementReasonId,
                    EndorsementType = EndorsementType.Cancellation,
                    Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    },
                    EndorsementDays = companyEndorsement.EndorsementDays,
                    CurrentTo = companyEndorsement.CurrentTo,
                    CurrentFrom = companyEndorsement.CurrentFrom
                };
                companyPolicy.CurrentTo = companyEndorsement.CurrentTo;
                companyPolicy.CurrentFrom = companyEndorsement.CurrentFrom;
                companyPolicy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                companyPolicy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                companyPolicy.Endorsement.IsMassive = companyEndorsement.IsMassive;
                companyPolicy.Endorsement.TemporalId = companyEndorsement.TemporalId;
                companyPolicy = CancellationPolicy(companyPolicy);
                return companyPolicy;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }


        private CompanyPolicy CancellationPolicy(CompanyPolicy companyPolicy)
        {
            List<PEM.CompanyPropertyRisk> companyProperty = null;
            if (companyPolicy == null)
            {
                throw new ArgumentException(Errors.EmptyPolicy);
            }
            if (companyPolicy?.Endorsement != null)
            {
                int cancellationFactor = -1;
                List<CompanyRisk> companyRisk = new List<CompanyRisk>();
                var riskInfringementPolicies = new List<PoliciesAut>();
                if ((CancellationType)companyPolicy.Endorsement.CancellationTypeId == CancellationType.Nominative)
                {
                    cancellationFactor = 0;
                }
                if (companyPolicy.Endorsement.TemporalId == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    companyProperty.AsParallel().ForAll(
                          x =>
                          {
                              x.Risk.OriginalStatus = x.Risk.Status;
                              x.Risk.Status = RiskStatusType.Modified;
                          });
                }
                else
                {
                    companyProperty = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyPolicy.Endorsement.TemporalId);
                    if (companyProperty == null || companyProperty.Count == 0)
                    {
                        companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        companyProperty.AsParallel().ForAll(
                         x =>
                         {
                             x.Risk.OriginalStatus = x.Risk.Status;
                             x.Risk.Status = RiskStatusType.Modified;
                         });
                    }
                }
                var companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                companyPolicy.Id = companyPolicyTemp.Id;
                companyRisk = CreatePropertyCancelation(companyPolicy, companyProperty, cancellationFactor);
                if (companyRisk != null && companyRisk.Any())
                {
                    riskInfringementPolicies.AddRange(companyRisk.SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                    companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companyRisk);
                    if ((CancellationType)companyPolicy.Endorsement.CancellationTypeId == CancellationType.Nominative)
                    {
                        companyPolicy.Endorsement.EndorsementType = EndorsementType.Nominative_cancellation;
                    }
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    if (companyPolicy.InfringementPolicies != null)
                    {
                        companyPolicy.InfringementPolicies.AddRange(riskInfringementPolicies);
                    }
                    return companyPolicy;
                }
                else
                {
                    throw new Exception(Errors.UnquotedRisks);
                }
            }
            else
            {
                throw new Exception(Errors.EmptyEndorsement);
            }

        }

        private List<CompanyRisk> CreatePropertyCancelation(CompanyPolicy companyPolicy, List<PEM.CompanyPropertyRisk> propertyRisk, int cancellationFactor)
        {
            if (companyPolicy == null || propertyRisk == null || propertyRisk.Count < 1)
            {
                throw new ArgumentException(Errors.EmptyRisks);
            }

            if (propertyRisk != null && propertyRisk.Count > 0)
            {
                var risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                var companyPropertyRisk = propertyRisk.Select(x => x.Risk).ToList();
                if (risks != null && risks.Count > 0)
                {

                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    propertyRisk.Where(a => a != null).AsParallel().ForAll(
                        z =>
                        {

                            var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                            if (z.Risk.Coverages != null)
                            {
                                companyRisk.Coverages.AsParallel().ForAll(
                                    y =>
                                    {
                                        y.InsuredObject = z.Risk.Coverages.FirstOrDefault(m => m.Id == y.Id)?.InsuredObject;
                                    });
                            }
                            if (companyRisk != null)
                            {
                                z.Risk.Policy = companyPolicy;
                                z.Risk.RiskId = companyRisk.RiskId;
                                z.Risk.Number = companyRisk.Number;
                                z.Risk.Status = companyRisk.Status;
                                z.Risk.Coverages = companyRisk.Coverages;
                                z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                            }
                            else
                            {
                                errors.Add(Errors.RiskNotFound);
                            }
                        }
                    );

                    TP.Parallel.ForEach(propertyRisk.Where(z => risks.Select(x => x.Number).Contains(z.Risk.Number)), item =>
                       {
                           item = DelegateService.propertyModificationService.GetDataModification(item, CoverageStatusType.Modified);


                           var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                           item.Risk.Coverages.AsParallel().ForAll(coverage =>
                           {
                               coverage.SubLineBusiness = coverages?.First(x => x.Id == coverage.Id).SubLineBusiness;
                               coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                               coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                           });
                           if ((CancellationType)item.Risk.Policy.Endorsement.CancellationTypeId == CancellationType.Nominative)
                           {
                               item.Risk.Policy.Endorsement.EndorsementType = EndorsementType.Nominative_cancellation;
                           }
                           var property = DelegateService.propertyService.CreatePropertyTemporal(item, false);
                           if (property != null && property.Risk != null)

                           {
                               item.Risk.Id = property.Risk.Id;
                               item.Risk.InfringementPolicies = property.Risk.InfringementPolicies;
                           }
                       });
                    return propertyRisk.Select(x => x.Risk).ToList();
                }
                else
                {
                    throw new ArgumentException(Errors.CancelingHomeError);
                }
            }
            else
            {
                throw new ArgumentException(Errors.RecoveringHomeError);
            }

        }


        public CompanyPolicy ExecuteThread(List<PEM.CompanyPropertyRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (PEM.CompanyPropertyRisk property in risksThread)
            {
                if (property.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in property.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                property.Risk.Status = RiskStatusType.Excluded;
                property.Risk.Coverages = risks.First(x => x.RiskId == property.Risk.RiskId).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, property.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in property.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }


                DelegateService.propertyService.CreatePropertyTemporal(property, false);

            }
            return policy;
        }
    }

}
