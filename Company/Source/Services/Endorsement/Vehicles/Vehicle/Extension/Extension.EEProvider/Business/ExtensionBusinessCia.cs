using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleEndorsementExtensionService.EEProvider.Services;
using Sistran.Company.Application.VehicleEndorsementExtensionService3GProvider.Assemblers;
using Sistran.Company.Application.VehicleEndorsementExtensionService3GProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using baf = Sistran.Core.Framework.BAF;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.VehicleEndorsementExtensionService.EEProvider.Business
{
    class ExtensionBusinessCia
    {
        BaseBusinessCia provider;
        public static object obj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleEndorsementBusinessCia" /> class.
        /// </summary>
        public ExtensionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }
        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementExtension(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new ArgumentException(Errors.ErrorPolicyEmpty);
                }
                bool isCollective = false;
                List<CompanyRisk> risks = new List<CompanyRisk>();
                CompanyPolicy policy;
                List<VEM.CompanyVehicle> companyVehicles = new List<VEM.CompanyVehicle>();
                PendingOperation pendingOperation = new PendingOperation();

                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };
                        #region REQ_220
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        #endregion

                        if (companyPolicy.Endorsement.BusinessTypeDescription != 0)
                        {
                            policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyPolicy.Endorsement.BusinessTypeDescription.ToString());
                        }

                        List<VEM.CompanyVehicle> companyVehiclesTemporal = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(policy.Id);
                        if (companyVehiclesTemporal != null && companyVehiclesTemporal.Any())
                        {
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyVehiclesTemporal.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            risks = CreateExtension(companyVehiclesTemporal, 0, isCollective);
                        }
                        else
                        {
                            companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyVehicles != null && companyVehicles.Any())
                            {
                                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                                companyVehicles.AsParallel().ForAll(

                                    x =>
                                    {
                                        x.Risk.Policy = policy;
                                        if (x.Accesories?.Count > 0)
                                        {
                                            x.Accesories.AsParallel().ForAll(y =>
                                            {
                                                y.Premium = 0;
                                            });
                                        }
                                    });
                                risks = CreateExtension(companyVehicles, 0, isCollective);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo vehiculos");
                            }
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Temporal poliza no encontrado");
                    }

                }
                else
                {
                    int Current_UserId = baf.BusinessContext.Current.UserId;
                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
                    var version = policy.Endorsement.AppRelation == null ? 1 : policy.Endorsement.AppRelation;
                    if (policy != null)
                    {
                        policy.Id = 0;
                        policy.Endorsement.TemporalId = 0;
                        policy.UserId = Current_UserId;
                        policy.IssueDate = companyPolicy.IssueDate;
                        policy.UserId = policy.UserId;
                        policy.CurrentFrom = policy.CurrentTo;
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                        if (policy.Endorsement == null)
                        {
                            policy.Endorsement = new CompanyEndorsement();
                        }
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text.TextBody,
                            Observations = companyPolicy.Endorsement.Text.Observations
                        };

                        policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.EndorsementType = EndorsementType.EffectiveExtension;
                        policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                        policy.TemporalType = TemporalType.Endorsement;
                        policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);

                        var immaper = AutoMapperAssembler.CreateMapCompanyClause();
                        policy.Clauses = immaper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                        #region REQ_220
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        #endregion

                        if (companyPolicy.Endorsement.BusinessTypeDescription != 0)
                        {
                            policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyPolicy.Endorsement.BusinessTypeDescription.ToString());
                        }
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                        if (policy != null)
                        {
                            companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyVehicles != null && companyVehicles.Any())
                            {
                                companyVehicles.AsParallel().ForAll(

                                    x =>
                                    {
                                        x.Risk.OriginalStatus = x.Risk.Status;
                                        x.Risk.Status = RiskStatusType.Modified;
                                        x.Risk.Policy = policy;
                                    }
                                    );
                                risks = CreateExtension(companyVehicles, version, isCollective);
                            }
                            else
                            {
                                throw new Exception("Error Obteniendo vehiculos");
                            }


                        }
                        else
                        {
                            throw new Exception("Error Creando temporal");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Poliza no encontrada");
                    }
                }
                if (policy.InfringementPolicies != null && policy.InfringementPolicies.Count() > 0)
                {
                    risks.AsParallel().ForAll(x => policy.InfringementPolicies.AddRange(x.InfringementPolicies));
                }
                if (risks != null && risks.Count != 0)
                {
                    policy.Summary = risks.First().Policy.Summary;
                }
                return policy;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private CompanyPolicy CreateTempExtensionByTempId(CompanyEndorsement companyEndorsement)
        {
            return null;

        }
        private CompanyPolicy CreateTempExtensionByEndorsement(CompanyEndorsement companyEndorsement)
        {
            return null;
        }
        private List<CompanyRisk> CreateExtension(List<VEM.CompanyVehicle> companyVehicles, int? version, bool isCollective)
        {
            if (companyVehicles != null && companyVehicles.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if (isCollective)
                {
                    return CreateRiskCollective(companyVehicles, version);
                }
                else
                {
                    TP.Parallel.ForEach(companyVehicles, item =>
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataExtension(item, version);
                        if (item?.Accesories?.Count > 0)
                        {

                            List<AccessoryDTO> accessoryDTOs = DelegateService.vehicleService.GetPremiumAccesory(item.Risk.Policy.Endorsement.PolicyId, item.Risk.Number, QuoteManager.CalculateEffectiveDays(item.Risk.Policy.CurrentFrom, item.Risk.Policy.CurrentTo));
                            foreach (VEM.CompanyAccessory accessory in item.Accesories)
                            {
                                accessory.AccumulatedPremium = accessoryDTOs.Where(m => m.Id == accessory.RiskDetailId).First().premium;
                            }
                        }
                        risk.Risk.Status = RiskStatusType.Original;
                        risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, false);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        risks.Add(risk.Risk);
                    });
                    var vehiclePolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                    /*Actualiza el Pending Operation de la Poliza*/
                    vehiclePolicy = DelegateService.underwritingService.CreatePolicyTemporal(vehiclePolicy, false);

                    return risks;
                }
            }
            else
            {
                throw new Exception(Errors.RiskNotFound);
            }

        }
        private List<CompanyRisk> CreateRiskCollective(List<VEM.CompanyVehicle> companyVehicles, int? version)
        {
            List<CompanyRisk> risks = new List<CompanyRisk>();
            if (companyVehicles.First().Risk.Policy.Id > 0)
            {
                companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyVehicles.First().Risk.Policy.Endorsement.TemporalId);
                TP.Parallel.ForEach(companyVehicles, item =>
                {
                    item.Risk.IsPersisted = true;
                    var risk = GetDataExtension(item, version);
                    risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, false);
                    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                    risks.Add(risk.Risk);
                });

                var vehiclePolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                vehiclePolicy = DelegateService.underwritingService.CreatePolicyTemporal(vehiclePolicy, false);

                return risks;
            }
            else
            {
                companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyVehicles.First().Risk.Policy.Endorsement.PolicyId);
                TP.Parallel.ForEach(companyVehicles, item =>
                {
                    item.Risk.IsPersisted = true;
                    var risk = GetDataExtension(item, version);
                    risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, false);
                    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                    risks.Add(risk.Risk);
                });

                var vehiclePolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                vehiclePolicy = DelegateService.underwritingService.CreatePolicyTemporal(vehiclePolicy, false);
            }

            return risks;
        }
        private VEM.CompanyVehicle GetDataExtension(VEM.CompanyVehicle risk, int? version)
        {
            risk.StandardVehiclePrice = risk.Price;
            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiary != null)
                            {
                                item.IdentificationDocument = beneficiary.IdentificationDocument;
                                item.Name = beneficiary.Name;
                            }
                            else
                            {
                                error.Add(Errors.ErrorBeneficiaryNotFound);
                            }
                        }
                        );
                    if (error.Any())
                    {
                        throw new Exception(string.Join(",", error));
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorBeneficiaryEmpty);
                }
            }
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
            if (coverages != null && coverages.Count > 0)
            {
                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();
                var ciaCoverages = risk.Risk.Coverages.Where(x => coverages.Select(z => z.Id).Contains(x.Id)).ToList();
                ciaCoverages.AsParallel().ForAll(item =>
                {
                    String coverStatusName = String.Empty;
                    if (item.CoverageOriginalStatus.HasValue)
                    {
                        if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value)) == null)
                        {
                            coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value);
                        }
                        else
                        {
                            coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value));
                        }
                    }
                    var coverageLocal = coverages.FirstOrDefault(u => u.Id == item.Id);
                    item.CoverStatus = item.CoverageOriginalStatus;
                    item.CoverStatusName = coverStatusName;
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverageLocal.Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.IsSelected = coverageLocal.IsSelected;
                    item.IsMandatory = coverageLocal.IsMandatory;
                    item.IsVisible = coverageLocal.IsVisible;
                    item.EndorsementLimitAmount = 0;
                    item.EndorsementSublimitAmount = 0;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);

            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }


            risk = DelegateService.vehicleService.QuotateVehicle(risk, false, false, version);

            return risk;
        }
    }
}
