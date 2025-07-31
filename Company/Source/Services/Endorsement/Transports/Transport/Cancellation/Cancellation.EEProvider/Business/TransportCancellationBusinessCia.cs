using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.TransportCancellationService.EEProvider.Resources;
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
using TM=System.Threading.Tasks;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Core.Services.UtilitiesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.TransportCancellationService.EEProvider.Business
{
    public class TransportCancellationBusinessCia
    {

        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportEndorsementBusinessCia" /> class.
        /// </summary>
        public TransportCancellationBusinessCia()
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
                }
                companyPolicy.Endorsement = new CompanyEndorsement
                {

                    Id = companyEndorsement.Id,
                    TemporalId = companyEndorsement.TemporalId,
                    PolicyId = companyEndorsement.PolicyId,
                    CancellationTypeId = companyEndorsement.CancellationTypeId,
                    EndorsementReasonId = companyEndorsement.EndorsementReasonId,
                    EndorsementReasonDescription = companyPolicy.Endorsement.EndorsementReasonDescription,
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
            List<CompanyTransport> companyTransports = null;
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
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                    companyTransports.AsParallel().ForAll(
                         x =>
                         {
                             x.Risk.OriginalStatus = x.Risk.Status;
                             x.Risk.Status = RiskStatusType.Modified;
                         });
                }
                else
                {
                    companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(companyPolicy.Endorsement.TemporalId);
                    if (companyTransports == null || companyTransports.Count < 1)
                    {
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                }
                var companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                companyPolicy.Id = companyPolicyTemp.Id;
                companyRisk = CreateTransportCancelation(companyPolicy, companyTransports, cancellationFactor);
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

        private List<CompanyRisk> CreateTransportCancelation(CompanyPolicy companyPolicy, List<CompanyTransport> companyTransports, int cancellationFactor)
        {
            if (companyPolicy == null || companyTransports == null || companyTransports.Count < 1)
            {
                throw new ArgumentException("Tranportes vacios");
            }

            if (companyTransports != null && companyTransports.Count > 0)
            {
                var risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                if (risks != null && risks.Count > 0)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companyTransports.Where(a => a != null).AsParallel().ForAll(
                        z =>
                        {
                            var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                            if (z.Risk.Coverages != null)
                            {
                                companyRisk.Coverages.AsParallel().ForAll(
                                    y =>
                                    {
                                        y.InsuredObject = z.Risk.Coverages.FirstOrDefault(m => m.Id == y.Id).InsuredObject;
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
                                errors.Add("Error Riesgo no encontrado");
                            }
                        }
                    );
                    TP.Parallel.ForEach(companyTransports.Where(z => risks.Select(x => x.Number).Contains(z.Risk.Number)), item =>
                       {
                           item = DelegateService.transportModificationService.GetDataModification(item, CoverageStatusType.Modified);


                           var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                           item.Risk.Coverages.AsParallel().ForAll(coverage =>
                           {
                               coverage.SubLineBusiness = coverages?.First(x => x.Id == coverage.Id).SubLineBusiness;
                               coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                               coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                               coverage.InsuredObject = coverages.First(x => x.Id == coverage.Id).InsuredObject;
                           });

                           var transport = DelegateService.transportService.CreateCompanyTransportTemporal(item);
                           if (transport != null && transport.Risk != null)
                           {
                               item.Risk.Id = transport.Risk.Id;
                               item.Risk.InfringementPolicies = transport.Risk.InfringementPolicies;
                           }
                       });
                    return companyTransports.Select(x => x.Risk).ToList();
                }
                else
                {
                    throw new ArgumentException("Error Cancelando Tranporte");
                }
            }
            else
            {
                throw new ArgumentException("Error Recuperando Tranporte");
            }

        }


        public CompanyPolicy ExecuteThread(List<CompanyTransport> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (CompanyTransport transport in risksThread)
            {
                if (transport.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in transport.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                transport.Risk.Status = RiskStatusType.Excluded;
                /// jhgomez
                /// transport.Rate = transport.Rate * -1;
                transport.Risk.Coverages = risks.First(x => x.RiskId == transport.Risk.RiskId).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, transport.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in transport.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }
                DelegateService.transportService.CreateCompanyTransportTemporal(transport);

            }
            return policy;
        }
    }

}
