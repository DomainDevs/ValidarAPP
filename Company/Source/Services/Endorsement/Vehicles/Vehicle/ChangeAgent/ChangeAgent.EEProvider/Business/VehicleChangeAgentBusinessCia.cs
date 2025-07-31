using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleChangeAgentService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.VehicleChangeAgentService.EEProvider.Business
{

    public class VehicleChangeAgentBusinessCia
    {
        BaseBusinessCia baseBusinessCia;
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleChangeAgentBusinessCia"/> class.
        /// </summary>
        public VehicleChangeAgentBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicyBase">The company policy base.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicyBase, bool isMassive = false)
        {
            try
            {
                if (companyPolicyBase == null)
                {
                    throw new Exception(Errors.ErrorDataPolicyEmpty);
                }
                companyPolicyBase.Endorsement.EndorsementType = EndorsementType.ChangeAgentEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();
                if (companyPolicyBase.Endorsement?.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicyBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicyBase.Endorsement.Id);
                    companyPolicy.Id = 0;
                    companyPolicy.Endorsement.TemporalId = 0;
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                }
                if (companyPolicy != null)
                {
                    if ((companyPolicy.CurrentFrom != Convert.ToDateTime(companyPolicyBase.CurrentFrom))
                       || (companyPolicy.Agencies != companyPolicyBase.Agencies))
                    {
                        //companyPolicy.UserId = BusinessContext.Current.UserId;
                        companyPolicy.TemporalType = TemporalType.Endorsement;
                        companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                        companyPolicy.CurrentFrom = companyPolicyBase.CurrentFrom;
                        companyPolicy.CurrentTo = companyPolicyBase.CurrentTo;
                        companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                        companyPolicy.BeginDate = Convert.ToDateTime(companyPolicyBase.CurrentFrom);
                        companyPolicy.Endorsement = companyPolicyBase.Endorsement;
                        companyPolicy.Agencies = companyPolicyBase.Agencies;
                        companyPolicy = ChangeAgentPolicy(companyPolicy);
                        return companyPolicy;
                    }

                    return null;
                }
                else
                {
                    throw new Exception(Errors.ErrorPolicyNotFound);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Changes the agent policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Poliza Vacia
        /// or
        /// or
        /// Vehiculos no ERncontrados
        /// </exception>
        private CompanyPolicy ChangeAgentPolicy(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new Exception(Errors.ErrorPolicyEmpty);
                }
                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                List<VEM.CompanyVehicle> companyVehicles = new List<VEM.CompanyVehicle>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    companyVehicles.AsParallel().ForAll(
                        x =>
                        {
                            x.Risk.Id = 0;
                            x.Risk.OriginalStatus = x.Risk.Status;
                            x.Risk.Status = RiskStatusType.NotModified;
                            if (x.Accesories?.Count > 0)
                            {
                                x.Accesories.AsParallel().ForAll(y =>
                                {
                                    y.Premium = 0;
                                });
                            }
                        }
                        );
                }
                else
                {
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyPolicy.Id);
                    if (companyVehicles == null && companyVehicles.Count < 1)
                    {
                        companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                }
                var risks = DelegateService.changeAgentEndorsementService.QuotateChangeAgentCia(companyPolicy);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                if (companyPolicy != null && companyVehicles?.Count > 0 && risks?.Count > 0)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companyVehicles.Where(a => a != null).AsParallel().ForAll(
                    z =>
                    {
                        var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                        if (companyRisk != null)
                        {
                            z.Risk.Policy = companyPolicy;
                            z.Risk.Status = companyRisk.Status;
                            z.Risk.Coverages = companyRisk.Coverages;
                            z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                        }
                        else
                        {
                            errors.Add(Errors.ErrorRiskNotFound);
                        }
                    }
                    );
                    companyVehicles.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            item = DelegateService.vehicleModificationService.GetDataModification(item, CoverageStatusType.Included);
                            item.Risk.Status = RiskStatusType.Included;
                            var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                            if (coverages.Count < item.Risk.Coverages.Count)
                            {
                                throw new Exception("Coberturas no existe en el Grupo de Coberturas");
                            }
                            TP.Parallel.ForEach(item.Risk.Coverages, coverage =>
                            {
                                if (coverages != null && coverages.Where(x => x.Id == coverage.Id).FirstOrDefault() != null)
                                {
                                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                }
                            });
                            item = DelegateService.vehicleService.CreateVehicleTemporal(item, false);
                            if (item.Risk.InfringementPolicies != null)
                                riskInfringementPolicies.AddRange(item.Risk?.InfringementPolicies.Where(x => x != null));
                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.Message);
                        }
                    });
                    if (errors?.Count() > 0)
                    {
                        throw new Exception(Errors.ErrorCreateTemporalChangeAgentVehicle);
                    }
                    if (companyVehicles != null && companyVehicles.Select(x => x.Risk).Any())
                    {

                        riskInfringementPolicies.AddRange(companyVehicles.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());

                        companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companyVehicles.Select(x => x.Risk).ToList());

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
                    throw new Exception(Errors.ErrorRiskNotFound);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Creates the endorsement change agent.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyEndorsement)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyEndorsement.TemporalId;
                var cancelation = CancellationPolicy(companyEndorsement);
                companyEndorsement.TemporalId = TempId;
                try
                {
                    companyEndorsement.Id = cancelation.Endorsement.Id;
                    var createEndorsementChangeAgent = CreateVehicleEndorsementChangeAgent(companyEndorsement);
                    companyPolicies.Add(createEndorsementChangeAgent);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(cancelation.Endorsement.PolicyId, cancelation.Endorsement.Id, EndorsementType.Cancellation);
                    throw new Exception("Error Creando Endoso Cambio Agente");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the vehicle endorsement change agent.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CreateVehicleEndorsementChangeAgent(CompanyEndorsement companyEndorsement)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
            List<CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyPolicy.Id);
            return DelegateService.vehicleService.CreateEndorsement(companyPolicy, vehicles);

        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.CancellationTypeId = (int)CancellationType.FromDate;
            companyEndorsement.EndorsementReasonId = 1;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementVehicleCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var vehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyPolicy.Id);
            return DelegateService.vehicleService.CreateEndorsement(companyPolicy, vehicles);
        }
    }
}
