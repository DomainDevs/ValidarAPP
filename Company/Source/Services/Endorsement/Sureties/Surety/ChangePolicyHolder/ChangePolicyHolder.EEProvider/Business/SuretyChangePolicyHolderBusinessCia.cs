using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider.Assemblers;
using Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using ENUMPOLICIES = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider.Business
{
    public class SuretyChangePolicyHolderBusinessCia
    {
        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyPolicyHolderBusinessCia" /> class.
        /// </summary>
        public SuretyChangePolicyHolderBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }
        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicyBase">The company policy base.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        public CompanyChangePolicyHolder CreateTemporal(CompanyChangePolicyHolder companyChangePolicyHolderBase, bool isMassive = false)
        {
            try
            {
                if (companyChangePolicyHolderBase == null)
                {
                    throw new Exception("Datos de la Poliza no Enviados");
                }
                companyChangePolicyHolderBase.Endorsement.EndorsementType = EndorsementType.ChangePolicyHolderEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyChangePolicyHolderBase.Endorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyChangePolicyHolderBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyChangePolicyHolderBase.Endorsement.Id);
                }
                if (companyPolicy != null)
                {
                    if ((companyPolicy.CurrentFrom != Convert.ToDateTime(companyChangePolicyHolderBase.Endorsement.CurrentFrom)) || (companyPolicy.Holder != companyChangePolicyHolderBase.holder))
                    {
                        companyPolicy.UserId = BusinessContext.Current?.UserId ?? companyChangePolicyHolderBase.UserId;
                        companyPolicy.TemporalType = TemporalType.Endorsement;
                        companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                        companyPolicy.CurrentFrom = companyChangePolicyHolderBase.CurrentFrom;
                        companyPolicy.CurrentTo = companyChangePolicyHolderBase.CurrentTo;
                        companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                        companyPolicy.BeginDate = companyChangePolicyHolderBase.Endorsement.CurrentFrom;
                        
                        companyPolicy.Endorsement.CancelationCurrentFrom = companyChangePolicyHolderBase.Endorsement.CancelationCurrentFrom;
                        companyPolicy.Endorsement.CancelationCurrentTo = companyChangePolicyHolderBase.Endorsement.CancelationCurrentTo;
                        companyPolicy.Endorsement.CurrentFrom = companyChangePolicyHolderBase.Endorsement.CurrentFrom;
                        companyPolicy.Endorsement.CurrentTo = companyChangePolicyHolderBase.Endorsement.CurrentTo;
                        companyPolicy.Endorsement.EndorsementDays = companyChangePolicyHolderBase.Endorsement.EndorsementDays;
                        companyPolicy.Endorsement.EndorsementType = companyChangePolicyHolderBase.Endorsement.EndorsementType;
                        companyPolicy.Endorsement.IssueDate = companyChangePolicyHolderBase.Endorsement.IssueDate;
                        companyPolicy.Endorsement.TicketDate = companyChangePolicyHolderBase.Endorsement.TicketDate;
                        companyPolicy.Endorsement.TicketNumber = companyChangePolicyHolderBase.Endorsement.TicketNumber;
                        companyPolicy.Endorsement.UserId = companyChangePolicyHolderBase.Endorsement.UserId;
                        companyPolicy.Endorsement.EndorsementReasonId = companyChangePolicyHolderBase.Endorsement.EndorsementReasonId;
                        companyPolicy.Endorsement.PrevPolicyId = companyChangePolicyHolderBase.Endorsement.PrevPolicyId;
                        companyPolicy.Endorsement.Text = companyChangePolicyHolderBase.Endorsement.Text;
                        companyPolicy.Endorsement.OnlyCancelation = companyChangePolicyHolderBase.Endorsement.OnlyCancelation;
                        companyPolicy.Endorsement.ExchangeRate = companyChangePolicyHolderBase.Endorsement.ExchangeRate;

                        companyPolicy.Holder = companyChangePolicyHolderBase.holder;
                        if (companyPolicy.Holder.PaymentMethod.PaymentId == 0)
                        {
                            companyPolicy.Holder.PaymentMethod.PaymentId = 1;
                        }
                        if (companyChangePolicyHolderBase.companyContract != null)
                        {
                            companyPolicy.Summary.companyContract = companyChangePolicyHolderBase.companyContract;
                        }
                        companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                        var insured = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(companyPolicy.Holder.IndividualId);
                        if (insured != null)
                        {
                            companyPolicy.Holder.RegimeType = insured.RegimeType;
                        }
                        companyPolicy = ChangePolicyHolderPolicy(companyPolicy);
                        return ModelAssembler.CreateCompanyChangePolicyHolder(companyPolicy);
                    }

                    return null;
                }
                else
                {
                    throw new Exception("Poliza No encontrada");
                }
            }
            catch (Exception ex)
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
        private CompanyPolicy ChangePolicyHolderPolicy(CompanyPolicy companyPolicy)
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
                if (companyPolicy.Summary.companyContract != null)
                {
                    foreach (CompanyContract item in companySuretys)
                    {
                        item.Contractor.CompanyName = companyPolicy.Summary.companyContract.CompanyName;
                        item.Contractor.Name = companyPolicy.Summary.companyContract.Name;
                        item.Contractor.IndividualId = companyPolicy.Summary.companyContract.IndividualId;
                    }
                }
                var risks = DelegateService.ciaChangePolicyHolderEndorsement.QuotateChangePolicyHolderCia(companyPolicy);
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
        public List<CompanyPolicy> CreateEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder, bool clearPolicies)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyChangePolicyHolder.Endorsement.TemporalId;
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

                var originalCurrentFrom = companyChangePolicyHolder.Endorsement.CurrentFrom;
                companyChangePolicyHolder.Endorsement.CancelationCurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyChangePolicyHolder.Endorsement.CancelationCurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyChangePolicyHolder.Endorsement.CurrentFrom = companyPolicy.CurrentFrom;
                companyChangePolicyHolder.Endorsement.CurrentTo = companyPolicy.CurrentTo;
                companyChangePolicyHolder.Endorsement.CancellationTypeId = companyPolicy.Endorsement.CancellationTypeId;

                var cancelation = CancellationPolicy(companyChangePolicyHolder.Endorsement);
                companyChangePolicyHolder.Endorsement.CurrentFrom = originalCurrentFrom;
                companyChangePolicyHolder.Endorsement.TemporalId = TempId;
                try
                {
                    companyChangePolicyHolder.Endorsement.Id = cancelation.Id;
                    var createEndorsementChangePolicyHolder = CreateSuretyEndorsementChangePolicyHolder(companyChangePolicyHolder);
                    companyPolicies.Add(createEndorsementChangePolicyHolder);
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
        private CompanyPolicy CreateSuretyEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyChangePolicyHolder.Endorsement.TemporalId, false);
            List<CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
            if (companyChangePolicyHolder.companyContract != null)
            {
                foreach (CompanyContract item in suretys)
                {
                    item.Contractor.CompanyName = companyChangePolicyHolder.companyContract.CompanyName;
                    item.Contractor.IndividualId = companyChangePolicyHolder.companyContract.IndividualId;
                    item.Contractor.Name = companyChangePolicyHolder.companyContract.Name;
                }
            }
            companyPolicy.InfringementPolicies = new List<PoliciesAut>();
            suretys.ForEach(x => x.Risk.InfringementPolicies = new List<PoliciesAut>());
            companyPolicy.CurrentFrom = companyChangePolicyHolder.Endorsement?.CurrentFrom ?? companyPolicy.CurrentFrom;
            return DelegateService.suretyService.CreateEndorsement(companyPolicy, suretys);
        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.EndorsementReasonId = 30;
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
