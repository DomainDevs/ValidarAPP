using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.MarineCancellationService.EEProvider.Resources;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Core.Services.UtilitiesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MarineCancellationService.EEProvider.Business
{
    public class MarineCancellationBusinessCia
    {

        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarineEndorsementBusinessCia" /> class.
        /// </summary>
        public MarineCancellationBusinessCia()
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
                companyPolicy = CancellationPolicy(companyPolicy);
                return companyPolicy;


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private CompanyPolicy CancellationPolicy(CompanyPolicy companyPolicy)
        {
            List<CompanyMarine> companyMarines = null;
            if (companyPolicy == null)
            {
                throw new ArgumentException("Poliza Vacia");
            }
            if (companyPolicy.Endorsement != null)
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
                    companyMarines = DelegateService.marineService.GetCompanyMarinesByPolicyId(companyPolicy.Endorsement.PolicyId);
                }
                else
                {
                    companyMarines = DelegateService.marineService.GetCompanyMarinesByTemporalId(companyPolicy.Endorsement.TemporalId);
                    if (companyMarines == null)
                    {
                        companyMarines = DelegateService.marineService.GetCompanyMarinesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                }
                var companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                companyPolicy.Id = companyPolicyTemp.Id;
                companyRisk = CreateMarineCancelation(companyPolicy, companyMarines, cancellationFactor);
                if (companyRisk != null && companyRisk.Any())
                {

                    riskInfringementPolicies.AddRange(companyRisk.SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                    companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companyRisk);

                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                    companyPolicy.InfringementPolicies.AddRange(riskInfringementPolicies);

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

        private List<CompanyRisk> CreateMarineCancelation(CompanyPolicy companyPolicy, List<CompanyMarine> companyMarines, int cancellationFactor)
        {
            if (companyPolicy == null || companyMarines == null || companyMarines.Count < 1)
            {
                throw new ArgumentException("Tranportes vacios");
            }

            if (companyMarines != null && companyMarines.Count > 0)
            {
                var risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                if (risks != null && risks.Count > 0)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companyMarines.Where(a => a != null).AsParallel().ForAll(
                        z =>
                        {
                            try
                            {
                                var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                                if (companyRisk != null)
                                {
                                    //companyRisk.Coverages.AsParallel().ForAll(
                                    //    y =>
                                    //    {
                                    //        y.InsuredObject = z.Risk.Coverages.FirstOrDefault(m => m.Id == y.Id).InsuredObject;
                                    //    });

                                    foreach (var item in companyRisk.Coverages)
                                    {
                                        item.InsuredObject = z.Risk.Coverages.Where(x => x.Id == item.Id).FirstOrDefault()?.InsuredObject;
                                    }
                                    z.Risk.Policy = companyPolicy;
                                    z.Risk.RiskId = companyRisk.RiskId;
                                    z.Risk.Number = companyRisk.Number;
                                    z.Risk.Status = companyRisk.Status;
                                    z.Risk.Coverages = companyRisk.Coverages;
                                    z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                                }
                                else
                                {
                                    errors.Add(Errors.ErrorRiskNotFound);
                                }

                            }
                            catch (Exception)
                            {

                                errors.Add(Errors.ErrorCreateTemporalCancellationMarine);
                            }
                        }
                    );
                    ConcurrentBag<string> error = new ConcurrentBag<string>();
                    TP.Parallel.ForEach(companyMarines.Where(z => risks.Select(x => x.Number).Contains(z.Risk.Number)), item =>
                       {
                           try
                           {
                               item = DelegateService.marineModificationService.GetDataModification(item, CoverageStatusType.Modified);
                               //item.Rate = item.Rate * -1;

                               var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                               item.Risk.Coverages.AsParallel().ForAll(coverage =>
                               {
                                   coverage.SubLineBusiness = coverages?.First(x => x.Id == coverage.Id).SubLineBusiness;
                                   coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                   coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                   coverage.InsuredObject = coverages.First(x => x.Id == coverage.Id).InsuredObject; //aclavijo
                               });

                               var marine = DelegateService.marineService.CreateCompanyMarineTemporal(item);
                               if (marine != null && marine.Risk != null)
                               {
                                   item.Risk.Id = marine.Risk.Id;
                                   item.Risk.InfringementPolicies = marine.Risk.InfringementPolicies;
                               }
                           }
                           catch (Exception ex)
                           {

                               error.Add(ex.Message);
                           }
                       });
                    if (error != null && error.Count > 0)
                    {
                        throw new Exception(string.Join("-", error.ToList()));
                    }
                    return companyMarines.Select(x => x.Risk).ToList();
                }
                else
                {
                    throw new ArgumentException(Errors.ErrorMarineCancellation);
                }
            }
            else
            {
                throw new ArgumentException("Error Recuperando Marine");
            }

        }


        public CompanyPolicy ExecuteThread(List<CompanyMarine> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (CompanyMarine marine in risksThread)
            {
                if (marine.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in marine.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                marine.Risk.Status = RiskStatusType.Excluded;
                /// marine.Rate = marine.Rate * -1;
                marine.Risk.Coverages = risks.First(x => x.RiskId == marine.Risk.RiskId).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, marine.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in marine.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }
                DelegateService.marineService.CreateCompanyMarineTemporal(marine);

            }
            return policy;
        }
    }

}
