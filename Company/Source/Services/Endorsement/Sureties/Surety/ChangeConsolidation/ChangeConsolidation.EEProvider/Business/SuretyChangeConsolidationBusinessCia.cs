using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.SuretyChangeConsolidationService.EEProvider.Assemblers;
using Sistran.Company.Application.SuretyChangeConsolidationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using ENUMPOLICIES = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.SuretyChangeConsolidationService.EEProvider.Business
{
    public class SuretyChangeConsolidationBusinessCia
    {
        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyPolicyHolderBusinessCia" /> class.
        /// </summary>
        public SuretyChangeConsolidationBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }
        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyChangeConsolidationBase">The company policy base.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        public CompanyChangeConsolidation CreateTemporal(CompanyChangeConsolidation companyChangeConsolidationBase, bool isMassive = false)
        {
            try
            {
                if (companyChangeConsolidationBase == null)
                {
                    throw new Exception("Datos de la Poliza no Enviados");
                }
                companyChangeConsolidationBase.Endorsement.EndorsementType = EndorsementType.ChangeConsolidationEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyChangeConsolidationBase.Endorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyChangeConsolidationBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyChangeConsolidationBase.Endorsement.Id);
                }
                if (companyPolicy != null)
                {
                    companyPolicy.UserId = BusinessContext.Current?.UserId ?? companyChangeConsolidationBase.UserId;
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.CurrentFrom = companyChangeConsolidationBase.CurrentFrom;
                    companyPolicy.CurrentTo = companyChangeConsolidationBase.CurrentTo;
                    companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                    companyPolicy.BeginDate = companyChangeConsolidationBase.Endorsement.CurrentFrom;
                    
                    companyPolicy.Endorsement.CancelationCurrentFrom = companyChangeConsolidationBase.Endorsement.CancelationCurrentFrom;
                    companyPolicy.Endorsement.CancelationCurrentTo = companyChangeConsolidationBase.Endorsement.CancelationCurrentTo;
                    companyPolicy.Endorsement.CurrentFrom = companyChangeConsolidationBase.Endorsement.CurrentFrom;
                    companyPolicy.Endorsement.CurrentTo = companyChangeConsolidationBase.Endorsement.CurrentTo;
                    companyPolicy.Endorsement.EndorsementDays = companyChangeConsolidationBase.Endorsement.EndorsementDays;
                    companyPolicy.Endorsement.EndorsementType = companyChangeConsolidationBase.Endorsement.EndorsementType;
                    companyPolicy.Endorsement.IssueDate = companyChangeConsolidationBase.Endorsement.IssueDate;
                    companyPolicy.Endorsement.TicketDate = companyChangeConsolidationBase.Endorsement.TicketDate;
                    companyPolicy.Endorsement.TicketNumber = companyChangeConsolidationBase.Endorsement.TicketNumber;
                    companyPolicy.Endorsement.UserId = companyChangeConsolidationBase.Endorsement.UserId;
                    companyPolicy.Endorsement.EndorsementReasonId = companyChangeConsolidationBase.Endorsement.EndorsementReasonId;
                    companyPolicy.Endorsement.PrevPolicyId = companyChangeConsolidationBase.Endorsement.PrevPolicyId;
                    companyPolicy.Endorsement.Text = companyChangeConsolidationBase.Endorsement.Text;
                    companyPolicy.Endorsement.OnlyCancelation = companyChangeConsolidationBase.Endorsement.OnlyCancelation;
                    companyPolicy.Endorsement.ExchangeRate = companyChangeConsolidationBase.Endorsement.ExchangeRate;

                    companyPolicy.Summary.companyContract = companyChangeConsolidationBase.companyContract;
                    companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                    companyPolicy = ChangeConsolidationPolicy(companyPolicy);
                    return ModelAssembler.CreateCompanyChangeConsolidation(companyPolicy);
                }
                else
                {
                    throw new Exception("Poliza No encontrada");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Changes the Coinsurance policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Poliza Vacia
        /// or
        /// or
        /// Vehiculos no ERncontrados
        /// </exception>
        private CompanyPolicy ChangeConsolidationPolicy(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new Exception("Poliza Vacia");
                }

                var originalCurrentFrom = companyPolicy.CurrentFrom;
                companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.Endorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.Endorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyPolicy.Endorsement.CancellationTypeId = originalCurrentFrom == companyPolicy.CurrentFrom ? 1 : 0;

                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                List<SEM.CompanyContract> companySuretys = new List<SEM.CompanyContract>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                }
                else
                {
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                }
                companySuretys[0].Contractor.CompanyName = companyPolicy.Summary.companyContract.CompanyName;
                companySuretys[0].Contractor.Name = companyPolicy.Summary.companyContract.Name;
                companySuretys[0].Contractor.IndividualId = companyPolicy.Summary.companyContract.IndividualId;
                var risks = DelegateService.ciaChangeConsolidationEndorsement.QuotateChangeConsolidationCia(companyPolicy);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                if (companySuretys != null && companySuretys.Count > 0 && risks != null)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companySuretys.Where(a => a != null).AsParallel().ForAll(
                    z =>
                    {
                        var companyRisk = risks.FirstOrDefault(x => x.RiskId == z.Risk.RiskId);
                        if (companyRisk != null)
                        {
                            z.Risk.Policy = companyPolicy;
                            z.Risk.RiskId = companyRisk.RiskId;
                            z.Risk.Number = companyRisk.Number;
                            z.Risk.Status = companyRisk.Status;
                            z.Risk.Coverages = companyRisk.Coverages;
                            z.Risk.AmountInsured = companyRisk.AmountInsured;
                            z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                        }
                        else
                        {
                            errors.Add("Error Riesgo no encontrado");
                        }
                    }
                    );
                    companySuretys.AsParallel().ForAll(item =>
                    {
                        item = DelegateService.suretyModificationService.GetDataModification(item, CoverageStatusType.Included);
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
                        item = DelegateService.suretyService.CreateSuretyTemporal(item, false);
                    });

                    if (companySuretys != null && companySuretys.Select(x => x.Risk).Any())
                    {
                        riskInfringementPolicies.AddRange(companySuretys.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                        companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companySuretys.Select(x => x.Risk).ToList());
                        companyPolicy.CurrentFrom = originalCurrentFrom;
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
                    throw new Exception("Vehiculos no ERncontrados");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Creates the endorsement change Coinsurance.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        public List<CompanyPolicy> CreateEndorsementChangeConsolidation(CompanyChangeConsolidation companyChangeConsolidation, bool clearPolicies)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyChangeConsolidation.Endorsement.TemporalId;
                var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(TempId, false);

                if (!clearPolicies)
                {
                    List<CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                    suretys.ForEach(x => companyPolicy.InfringementPolicies.AddRange(x.Risk.InfringementPolicies));
                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive))
                    {
                        companyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies.Where(x => x.Type == ENUMPOLICIES.TypePolicies.Authorization || x.Type == ENUMPOLICIES.TypePolicies.Restrictive).ToList();
                        companyPolicies.Add(companyPolicy);
                        return companyPolicies;
                    }
                }

                var originalCurrentFrom = companyChangeConsolidation.Endorsement.CurrentFrom;
                companyChangeConsolidation.Endorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyChangeConsolidation.Endorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyChangeConsolidation.Endorsement.CurrentFrom = companyPolicy.CurrentFrom;
                companyChangeConsolidation.Endorsement.CurrentTo = companyPolicy.CurrentTo;
                companyChangeConsolidation.Endorsement.CancellationTypeId = companyPolicy.Endorsement.CancellationTypeId;

                var cancelation = CancellationPolicy(companyChangeConsolidation.Endorsement);
                companyChangeConsolidation.Endorsement.CurrentFrom = originalCurrentFrom;
                companyChangeConsolidation.Endorsement.TemporalId = TempId;
                try
                {
                    companyChangeConsolidation.Endorsement.Id = cancelation.Id;
                    var createEndorsementChangeConsolidation = CreateSuretyEndorsementChangeConsolidation(companyChangeConsolidation, clearPolicies);
                    companyPolicies.Add(createEndorsementChangeConsolidation);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(cancelation.Endorsement.PolicyId, cancelation.Endorsement.Id, EndorsementType.Cancellation);
                    throw new Exception("Error Creando Endoso Cambio de Afianzado");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the surety endorsement change Coinsurance.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CreateSuretyEndorsementChangeConsolidation(CompanyChangeConsolidation companyChangeConsolidation, bool clearPolicies)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyChangeConsolidation.Endorsement.TemporalId, false);
            List<CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
            if (!clearPolicies)
            {
                suretys[0].Contractor.CompanyName = companyChangeConsolidation.companyContract.CompanyName;
                suretys[0].Contractor.IndividualId = companyChangeConsolidation.companyContract.IndividualId;
                suretys[0].Contractor.Name = companyChangeConsolidation.companyContract.Name;
            }

            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            companyPolicy.CurrentFrom = companyChangeConsolidation.Endorsement?.CurrentFrom ?? companyPolicy.CurrentFrom;
            return DelegateService.suretyService.CreateEndorsement(companyPolicy, suretys);
        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.EndorsementReasonId = 31;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementSuretyCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            return DelegateService.suretyService.CreateEndorsement(companyPolicy, suretys);
        }

        public CompanyContract GetCompanyContractByTemporalId(int temporalId, bool isMasive)
        {
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            List<PendingOperation> pendingOperation = new List<PendingOperation>();

            if (isMasive && boolUseReplicatedDatabase)
            {
            }
            else
            {
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);
            }

            if (pendingOperation != null)
            {
                try
                {
                    CompanyContract companyContract = COMUT.JsonHelper.DeserializeJson<CompanyContract>(pendingOperation[0].Operation);
                    return companyContract;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
