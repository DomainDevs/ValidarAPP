using AutoMapper;
using Ionic.Zip;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.CancellationMsvEndorsementServices.EEProvider.Resources;
using Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider.Assemblers;
using Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider.DAOs;
using Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider.Entities.Views;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.CancellationMassiveEndorsement.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Parameter = Sistran.Core.Application.CommonService.Models.Parameter;
using TP = Sistran.Core.Application.Utilities.Utility;


namespace Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider.DAOs
{
    using Core.Application.AuthorizationPoliciesServices.Enums;

    /// <summary>
    /// Cancelacion Masiva
    /// </summary>
    public class CancellationMassiveDAO
    {
        private readonly object objCache = new object();
        private int userId { get; set; }
        #region tarifacion
        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>
        /// Riesgos
        /// </returns>
        public List<CompanyRisk> QuotateCancellation(CompanyPolicy policy, int cancellationFactor)
        {
            if (policy == null)
            {
                throw new ArgumentException(Errors.PolicyNotFound);
            }
            var mapper = ModelAssembler.CreateMapEndorsement();
            List<CompanyEndorsement> endorsements = mapper.Map<List<Endorsement>, List<CompanyEndorsement>>(DelegateService.underwritingService.GetEffectiveEndorsementsByPolicyId(policy.Endorsement.PolicyId));
            object localLockObject = new object();
            if (endorsements != null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filter.In();
                filter.ListValue();
                foreach (CompanyEndorsement endorsement in endorsements)
                {
                    filter.Constant(endorsement.Id);
                }
                filter.EndList();
                MassiveCoverageCancellationView view = new MassiveCoverageCancellationView();
                ViewBuilder builder = new ViewBuilder("MassiveCoverageCancellationView");
                builder.Filter = filter.GetPredicate();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }
                if (view?.RiskCoverages != null && view.RiskCoverages.Count > 0)
                {
                    int cancellationDays = Convert.ToInt32((policy.CurrentTo - policy.CurrentFrom).TotalDays);
                    List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                    List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                    List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    //Riesgos Activos
                    Parallel.ForEach<ISSEN.EndorsementRisk, List<CompanyRisk>>(endorsementRisks.Where(x => x.IsCurrent == true && x.RiskStatusCode != (int)RiskStatusType.Excluded),
                        () => { return new List<CompanyRisk>(); },
                        (risk, state, localrisk) =>
                        {
                            var currenRiks = new CompanyRisk
                            {
                                Id = 0,
                                RiskId = risk.RiskId,
                                Number = risk.RiskNum,
                                Status = RiskStatusType.Excluded

                            };
                            var coverages = from er in endorsementRiskCoverages
                                            join rc in riskCoverages
                                            on er.RiskCoverId equals rc.RiskCoverId
                                            where er.RiskNum == risk.RiskNum
                                            select rc;
                            currenRiks.Coverages = CoveragesByCoverages(coverages.ToList());
                            TP.Parallel.ForEach(currenRiks.Coverages, (coverage) =>
                            {
                                //Coberturas del riesgo todos los endosos

                                var endorsementCoverages = from er in endorsementRiskCoverages
                                                           join rc in riskCoverages
                                                           on er.RiskCoverId equals rc.RiskCoverId
                                                           where er.RiskNum == risk.RiskNum
                                                           && rc.CoverageId == coverage.Id
                                                           select rc;
                                var coverageCurrent = new CompanyCoverage();
                                decimal premiuntTotal = Decimal.Zero;
                                object lockData = new object();
                                Parallel.ForEach<ISSEN.RiskCoverage, CompanyCoverage>(endorsementCoverages.ToList(),
                                     () => { return new CompanyCoverage(); },
                                    (subCoverage, State, coverageResult) =>
                                    {
                                        decimal premiumCoverage = Decimal.Zero;
                                        if (policy.CurrentFrom >= subCoverage.CurrentFrom.GetValueOrDefault() && policy.CurrentFrom < subCoverage.CurrentTo.GetValueOrDefault())
                                        {
                                            int originalDays = Convert.ToInt32((subCoverage.CurrentTo.Value - subCoverage.CurrentFrom.Value).TotalDays);
                                            if (policy.CurrentTo > subCoverage.CurrentTo)
                                            {
                                                lock (localLockObject)
                                                {
                                                    cancellationDays = originalDays;
                                                }
                                            }
                                            premiumCoverage = (subCoverage.PremiumAmount / originalDays) * cancellationDays;
                                            coverageResult.CurrentFrom = policy.CurrentFrom;
                                        }
                                        else if (policy.CurrentFrom < subCoverage.CurrentFrom.GetValueOrDefault())
                                        {
                                            premiumCoverage = subCoverage.PremiumAmount;
                                            coverageResult.CurrentFrom = policy.CurrentFrom;
                                        }
                                        else if (policy.CurrentFrom > (subCoverage.CurrentTo ?? DateTime.MinValue))
                                        {
                                            coverageResult.PremiumAmount = 0;
                                        }
                                        lock (lockData)
                                        {
                                            premiuntTotal += premiumCoverage;
                                        }
                                        return coverageResult;
                                    }
                                ,
                                (finalResult) => { lock (localLockObject) coverageCurrent = finalResult; }
                                );
                                coverage.PremiumAmount = decimal.Round(premiuntTotal * cancellationFactor, QuoteManager.RoundValue);
                                coverage.EndorsementLimitAmount = coverage.EndorsementLimitAmount * cancellationFactor;
                                coverage.EndorsementSublimitAmount = coverage.EndorsementLimitAmount * cancellationFactor;
                                coverage.LimitOccurrenceAmount = coverage.LimitOccurrenceAmount * cancellationFactor;
                                coverage.LimitClaimantAmount = coverage.LimitClaimantAmount * cancellationFactor;
                                coverage.CurrentFrom = policy.CurrentFrom > endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault() ? policy.CurrentFrom : endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault();
                                coverage.CoverStatus = CoverageStatusType.Excluded;
                                coverage.EndorsementType = EndorsementType.Cancellation;
                            }
                            );
                            localrisk.Add(currenRiks);

                            if ((CancellationType)policy.Endorsement.CancellationTypeId == CancellationType.ShortTerm)
                            {
                                localrisk = CalculateShortTerm(policy, localrisk);
                            }
                            return localrisk;
                        },
                         (riskresult) => { lock (localLockObject) risks.AddRange(riskresult); }
                         );
                    return risks;
                }
                else
                {
                    return null;
                }


            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///Cancelacion Corto Plazo
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="risks">The risks.</param>
        /// <returns></returns>
        private List<CompanyRisk> CalculateShortTerm(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                RuleProcessRuleSet rules = DelegateService.rulesService.GetRulestByRulsetProcessType((int)ProcessTypes.ShortTerm);
                var dynamicConceptId = rules.ConceptId;
                decimal shortTermPercentage = 0;
                Policy policyCore;

                var config = ModelAssembler.CreateMapPolicy();
                policyCore = config.Map<CompanyPolicy, Policy>(policy);

                policyCore = DelegateService.underwritingService.RunRulesPolicy(policyCore, rules.PosRuleSet);

                foreach (var item in policyCore.DynamicProperties)
                {
                    if (item.Id == dynamicConceptId)
                    {
                        shortTermPercentage = (decimal)item.Value;
                    }
                }

                if (shortTermPercentage > 0)
                {
                    foreach (CompanyRisk item in risks)
                    {
                        item.Coverages.ForEach(c => c.PremiumAmount = (c.PremiumAmount - (c.PremiumAmount * shortTermPercentage / 100)));
                        item.Coverages.ForEach(c => c.ShortTermPercentage = shortTermPercentage);
                        item.Premium = item.Coverages.Sum(x => x.PremiumAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("WARM UP SHORT TERM:", ex.Message);

            }
            return risks;
        }

        /// <summary>
        /// Cancellations the massive by massive load identifier.
        /// </summary>
        /// <param name="massiveLoadId">The massive load identifier.</param>
        /// <returns></returns>
        public MassiveLoad CancellationMassiveByMassiveLoadId(MassiveLoad massiveLoad)
        {
            try
            {
                if (massiveLoad != null)
                {
                    massiveLoad.Status = MassiveLoadStatus.Tariffing;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                    List<MassiveCancellationRow> massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(massiveLoad.Id, null, MassiveLoadProcessStatus.Validation, false, null);
                    List<AuthorizationRequest> authorizationRequest = new List<AuthorizationRequest>();
                    if (massiveCancellationRows != null && massiveCancellationRows.Count > 0)
                    {
                        TP.Task.Run(() =>
                        {
                            if (massiveCancellationRows.Any())
                            {
                                MassiveCancellationRow massiveCancellationRow = massiveCancellationRows.FirstOrDefault();
                                ExecuteTariffed(massiveLoad, massiveCancellationRow, authorizationRequest);
                                massiveCancellationRows.RemoveAt(0);
                                if (massiveCancellationRows.Count > 0)
                                {
                                    ParallelHelper.ForEach(massiveCancellationRows, row =>
                                    {
                                        ExecuteTariffed(massiveLoad, row, authorizationRequest);
                                    });
                                }
                            }
                            DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoad.Id);
                            DataFacadeManager.Dispose();
                            if (authorizationRequest.Any())
                            {
                                DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequest);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                massiveLoad.ErrorDescription = ex.GetBaseException().Message;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            return massiveLoad;
        }

        /// <summary>
        /// Ejecutar Tarifacion
        /// </summary>
        /// <param name="massiveLoadProcess">The massive load process.</param>
        public void ExecuteTariffed(MassiveLoad massiveLoad, MassiveCancellationRow massiveCancellationRow, List<AuthorizationRequest> authorizationRequest)
        {
            try
            {
                CompanyPolicy companyPolicy = new CompanyPolicy();
                PendingOperation pendingOperation;
                CancellationModel cancellationModel = CancellationModel(massiveCancellationRow.SerializedRow);
                if (massiveCancellationRow != null && massiveCancellationRow.Risk != null && massiveCancellationRow.Risk.Policy != null && massiveCancellationRow.Risk.Policy.Id != 0)
                {
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.UtilitiesService.GetPendingOperationById(massiveCancellationRow.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperation = DelegateService.UtilitiesService.GetPendingOperationById(massiveCancellationRow.Risk.Policy.Id);
                    }
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.Endorsement.IsMassive = true;
                    companyPolicy.UserId = massiveLoad.User.UserId;

                    CompanyPolicy companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                    companyPolicy.Id = companyPolicyTemp.Id;

                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Events;
                    int cancellationFactor = -1;
                    if ((CancellationType)cancellationModel.CancelationTypeId == CancellationType.Nominative)
                    {
                        cancellationFactor = 0;
                    }
                    List<CompanyRisk> risks = QuotateCancellation(companyPolicy, cancellationFactor);
                    if (risks != null && risks.Count > 0)
                    {
                        risks.AsParallel().ForAll(x => x.Policy = companyPolicy);
                        switch (massiveCancellationRow.SubcoveredRiskType)
                        {
                            case SubCoveredRiskType.JudicialSurety:
                                break;
                            //case SubCoveredRiskType.Liability:
                            //    DelegateService.massiveLiabilityCancellationService.CreateTemporalEndorsementCancellation(companyPolicy, risks, massiveCancellationRow);
                            //    break;
                            //case SubCoveredRiskType.Property:
                            //    authorizationRequest.AddRange(DelegateService.massivePropertyCancellationService.CreateTemporalEndorsementCancellation(massiveLoad, companyPolicy, risks, massiveCancellationRow));
                            //    break;
                            case SubCoveredRiskType.Surety:
                                break;
                            case SubCoveredRiskType.ThirdPartyLiability:
                                DelegateService.massiveTplCancellationService.CreateTemporalEndorsementCancellation(massiveLoad, companyPolicy, risks, massiveCancellationRow);
                                break;
                            case SubCoveredRiskType.Vehicle:
                                authorizationRequest.AddRange(DelegateService.massiveVehicleCancellationService.CreateTemporalEndorsementCancellation(massiveLoad, companyPolicy, risks, massiveCancellationRow));
                                break;
                        }
                        //if (massiveCancellationRow.Risk != null & massiveCancellationRow.Risk.Policy != null)
                        //{
                        //    massiveCancellationRow.Risk.Policy.Branch = new Branch { Id = cancellationModel.BranchId };
                        //    massiveCancellationRow.Risk.Policy.Prefix = new Prefix { Id = cancellationModel.PrefixId };
                        //}
                    }
                    else
                    {
                        massiveCancellationRow.HasError = true;
                        massiveCancellationRow.Observations = Errors.ErrorPolicyData + KeySettings.ReportErrorSeparatorMessage();
                        massiveCancellationRow.Status = MassiveLoadProcessStatus.Events;
                        DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                    }

                }
                else
                {
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = Errors.ErrorNotExistsRisk + KeySettings.ReportErrorSeparatorMessage();
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Events;
                    DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                }
            }
            catch (Exception ex)
            {
                massiveCancellationRow.Status = MassiveLoadProcessStatus.Events;
                massiveCancellationRow.HasError = true;
                massiveCancellationRow.Observations = ex.GetBaseException().Message;
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }
        #endregion tarifacion
        #region Creacion Modelos y validacion
        /// <summary>
        /// Crear cargue Masivo Cancelacion
        /// </summary>
        /// <param name="massiveLoad">The massive load.</param>
        /// <returns></returns>
        public MassiveLoad CreateMassiveLoad(MassiveLoad massiveLoad)
        {
            massiveLoad.Status = MassiveLoadStatus.Validating;
            massiveLoad = DelegateService.massiveService.CreateMassiveLoad(massiveLoad);
            if (massiveLoad != null)
            {
                TP.Task.Run(() => ValidateMassiveFile(massiveLoad));
            }

            return massiveLoad;
        }

        /// <summary>
        /// Validar cargue
        /// </summary>
        /// <param name="massiveLoad">The massive load.</param>
        private void ValidateMassiveFile(MassiveLoad massiveLoad)
        {
            try
            {

                FileProcessValue fileProcessValue = new FileProcessValue();
                fileProcessValue.Key1 = (int)FileProcessType.CancellationMassive;
                string fileName = massiveLoad.File.Name;
                massiveLoad.File = DelegateService.UtilitiesService.GetFileByFileProcessValue(fileProcessValue);
                massiveLoad.File.Name = fileName;
                massiveLoad.File = DelegateService.UtilitiesService.ValidateFile(massiveLoad.File, massiveLoad.User.AccountName);

                if (massiveLoad.File != null)
                {
                    massiveLoad.File.Name = fileName;
                    var cancellationTemplate = massiveLoad.File.Templates.First(x => x.PropertyName == TemplatePropertyName.MassiveCancellation);
                    cancellationTemplate = DelegateService.UtilitiesService.ValidateDataTemplate(massiveLoad.File.Name, massiveLoad.User.AccountName, cancellationTemplate);

                    if (cancellationTemplate.HasError)
                    {
                        massiveLoad.HasError = true;
                        massiveLoad.ErrorDescription = cancellationTemplate.ErrorDescription;
                        DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                    }
                    else
                    {
                        massiveLoad.TotalRows = cancellationTemplate.Rows.Count;
                        DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                        CancellationValidate(cancellationTemplate);
                        CancellationMassiveValidationDAO cancellationMassiveValidationDAO = new CancellationMassiveValidationDAO();
                        List<Validation> validations = cancellationMassiveValidationDAO.GetValidationsByFiles(cancellationTemplate, massiveLoad);

                        if (validations.Count > 0)
                        {
                            Validation validation;
                            foreach (Row row in massiveLoad.File.Templates[0].Rows)
                            {
                                validation = validations.Find(x => x.Id == row.Number);
                                if (validation != null)
                                {
                                    row.HasError = true;
                                    row.ErrorDescription += validation.ErrorMessage;
                                }
                            }
                        }
                        CreateModels(massiveLoad, cancellationTemplate);

                        if (!Settings.UseReplicatedDatabase())
                        {
                            massiveLoad.Status = MassiveLoadStatus.Validated;
                            DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                        }
                    }
                }
                else
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = "NotFoundMassiveFile";
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                }
            }
            catch (Exception ex)
            {
                massiveLoad.HasError = true;
                massiveLoad.ErrorDescription = ex.GetBaseException().Message;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModels(MassiveLoad massiveLoad, Template cancellationTemplate)
        {
            if (cancellationTemplate != null)
            {
                ParallelHelper.ForEach(cancellationTemplate.Rows, row =>
                {
                    CreateModel(row, massiveLoad);
                });

                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoad.Id);    

            }
        }

        private void CreateModel(Row row, MassiveLoad massiveLoad)
        {
            MassiveCancellationRow massiveCancellationRow = new MassiveCancellationRow();
            try
            {
                massiveCancellationRow.MassiveLoadId = massiveLoad.Id;
                massiveCancellationRow.RowNumber = row.Number + 1;
                massiveCancellationRow.Status = MassiveLoadProcessStatus.Validation;
                massiveCancellationRow.HasError = row.HasError;
                massiveCancellationRow.Observations = row.ErrorDescription;
                massiveCancellationRow.SerializedRow = JsonConvert.SerializeObject(row);
                massiveCancellationRow = DelegateService.massiveUnderwritingService.CreateMassiveCancellationRow(massiveCancellationRow);

                if (row.HasError)
                {
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = row.ErrorDescription;
                    DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                    return;
                }

                int prefix = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixId));
                int branch = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                decimal documentNumber = (decimal)DelegateService.UtilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));

                Policy policy = new Policy();
                CompanyPolicy companyPolicy = new CompanyPolicy();
                PendingOperation pendingOperation = new PendingOperation();
                CancellationModel cancellationModel = CancellationModel(massiveCancellationRow.SerializedRow);


                policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(cancellationModel.PrefixId, cancellationModel.BranchId, cancellationModel.DocumentNumber);

                if (policy != null && policy.Endorsement.EndorsementType != EndorsementType.Cancellation && policy.Endorsement.EndorsementType != EndorsementType.AutomaticCancellation && policy.Endorsement.EndorsementType != EndorsementType.Nominative_cancellation)
                {
                    ValidateDateCancellation(massiveCancellationRow, policy, cancellationModel);
                    companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.Id = policy.Id;
                    companyPolicy.CurrentFrom = cancellationModel.CurrentFrom;
                    companyPolicy.IssueDate = DelegateService.underwritingService.GetQuotationDate(3, DateTime.Now);
                    companyPolicy.Endorsement.CancellationTypeId = cancellationModel.CancelationTypeId;
                    companyPolicy.Endorsement.EndorsementReasonId = cancellationModel.CancelationReasonId;
                    //agregado 
                    companyPolicy.Endorsement.TicketDate = (DateTime)DelegateService.UtilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
                    companyPolicy.Endorsement.TicketNumber = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));
                    if (cancellationModel.CancelationTypeId == (int)CancellationType.BeginDate || cancellationModel.CancelationTypeId == (int)CancellationType.FromDate || cancellationModel.CancelationTypeId == (int)CancellationType.ShortTerm)
                    {
                        companyPolicy.Endorsement.EndorsementType = EndorsementType.Cancellation;
                    }
                    else
                    {
                        if (cancellationModel.CancelationTypeId == (int)CancellationType.Nominative)
                        {
                            companyPolicy.Endorsement.EndorsementType = EndorsementType.Nominative_cancellation;
                        }
                        else
                        {
                            massiveCancellationRow.HasError = true;
                            massiveCancellationRow.Observations += Errors.ErrorCancellationType;
                            DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                            return;
                        }
                    }
                    companyPolicy.Endorsement.CurrentFrom = cancellationModel.CurrentFrom;
                    companyPolicy.Endorsement.CurrentTo = policy.CurrentTo;
                    companyPolicy.UserId = userId;
                    companyPolicy.SubMassiveProcessType = SubMassiveProcessType.CancellationMassive;
                }
                else
                {
                    if (policy != null)
                    {
                        throw new Exception(Errors.ErrorPolicyCanceled);
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorPolicy);
                    }
                }

                SubCoveredRiskType? subCoveredRiskType = DelegateService.underwritingService.GetSubcoverRiskTypeByPrefixIdBranchIdPolicyNumber(prefix, branch, documentNumber);
                if (subCoveredRiskType.HasValue)
                {
                    massiveCancellationRow.SubcoveredRiskType = subCoveredRiskType.Value;
                    massiveCancellationRow.Risk = new Risk
                    {
                        Policy = new Policy
                        {
                            Id = 0
                        }
                    };
                    massiveCancellationRow.HasEvents = false;


                    string pendingOperationJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy);
                    pendingOperation.UserId = companyPolicy.UserId;
                    pendingOperation.OperationName = "Temporal";
                    pendingOperation.Operation = pendingOperationJsonIsnotNull;
                    string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", JsonConvert.SerializeObject(pendingOperation), (char)007, JsonConvert.SerializeObject(massiveCancellationRow), (char)007, nameof(MassiveCancellationRow));
                    QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                }
                else
                {
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = Errors.ErrorPolicy + KeySettings.ReportErrorSeparatorMessage();
                    massiveCancellationRow = DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                }

            }
            catch (Exception ex)
            {
                massiveCancellationRow.HasError = true;
                massiveCancellationRow.Observations = ex.GetBaseException().Message;
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }

        /// <summary>
        /// Coverageses the by coverages.
        /// </summary>
        /// <param name="rc">RiskCoverage</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">rc</exception>
        private List<CompanyCoverage> CoveragesByCoverages(List<ISSEN.RiskCoverage> rc)
        {
            if (rc == null)
            {
                throw new ArgumentNullException(nameof(rc));
            }

            var resultCollection = new List<CompanyCoverage>();
            object localLockObject = new object();
            Parallel.ForEach<ISSEN.RiskCoverage, List<CompanyCoverage>>(rc, () => { return new List<CompanyCoverage>(); }
                                , (coverage, state, localCoverage) =>
                                {
                                    localCoverage.Add(ModelAssembler.CreateCoverage(coverage));
                                    return localCoverage;
                                }
                                ,
                (finalResult) => { lock (localLockObject) resultCollection.AddRange(finalResult); }
                );
            return resultCollection;
        }
        #endregion
        #region modelos
        private CancellationModel CancellationModel(string serializedRow)
        {
            Row row = JsonConvert.DeserializeObject<Row>(serializedRow);
            CancellationModel cancellationModel = new CancellationModel
            {
                DocumentNumber = Decimal.Round((Decimal)DelegateService.UtilitiesService.GetValueByField<Decimal>(row.Fields.First(x => x.PropertyName == FieldPropertyName.PolicyNumber)), 0),
                PrefixId = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.First(x => x.PropertyName == FieldPropertyName.PrefixId)),
                BranchId = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.First(x => x.PropertyName == FieldPropertyName.BranchId)),
                CancelationTypeId = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.First(x => x.PropertyName == FieldPropertyName.TypeCancellation)),
                CancelationReasonId = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.First(x => x.PropertyName == FieldPropertyName.CancellationReason)),
                CurrentFrom = (DateTime)DelegateService.UtilitiesService.GetValueByField<DateTime>(row.Fields.First(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom)),
                TicketNumber = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.First(x => x.PropertyName == FieldPropertyName.TicketNumber)),
                TicketDate = (DateTime)DelegateService.UtilitiesService.GetValueByField<DateTime>(row.Fields.First(x => x.PropertyName == FieldPropertyName.TicketDate)),
            };
            return cancellationModel;
        }
        #endregion

        #region emision

        /// <summary>
        /// Emision Cancelacion Masiva
        /// </summary>
        /// <param name="massiveLoadId">identificador del Cargue masiva</param>
        /// <returns></returns>
        public MassiveLoad CreateIssuePolicy(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                List<MassiveCancellationRow> massiveLoadProcesses = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(massiveLoad.Id, null, MassiveLoadProcessStatus.Events, false, false);

                if (massiveLoadProcesses.Count > 0)
                {
                    TP.Task.Run(() => IssuePolicy(massiveLoad, massiveLoadProcesses));
                }
                else
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = Errors.PolicyHasEvents;
                }
            }

            return massiveLoad;
        }

        public void IssuePolicy(MassiveLoad massiveLoad, List<MassiveCancellationRow> massiveLoadProcesses)
        {
            try
            {
                massiveLoad.Status = MassiveLoadStatus.Issuing;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                List<CompanyPolicy> companyPolicies = DelegateService.massiveUnderwritingService.GetCompanyPoliciesToIssueByOperationIds(massiveLoadProcesses.Where(x => !x.HasError).Select(x => x.Risk.Policy.Id).ToList());

                ParallelHelper.ForEach(companyPolicies, companyPolicy =>
                {
                    MassiveCancellationRow massiveLoadProcess = massiveLoadProcesses.FirstOrDefault(x => x.Risk.Policy.Id == companyPolicy.Id);

                    massiveLoadProcess.Status = MassiveLoadProcessStatus.Issuance;
                    DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveLoadProcess);
                    //Fecha de emisión
                    companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

                    PendingOperation pendingOperationPolicy = new PendingOperation();
                    pendingOperationPolicy.Id = companyPolicy.Id;
                    pendingOperationPolicy.UserId = massiveLoad.User.UserId;
                    pendingOperationPolicy.Operation = JsonConvert.SerializeObject(companyPolicy);

                    string riskType = string.Empty;
                    switch (massiveLoadProcess.SubcoveredRiskType)
                    {
                        case SubCoveredRiskType.Liability:
                            riskType = nameof(CompanyLiabilityRisk);
                            break;
                        case SubCoveredRiskType.Property:
                            riskType = nameof(CompanyPropertyRisk);
                            break;
                        case SubCoveredRiskType.Surety:
                            break;
                        case SubCoveredRiskType.ThirdPartyLiability:
                            riskType = nameof(CompanyTplRisk);
                            break;
                        case SubCoveredRiskType.Vehicle:
                            riskType = nameof(CompanyVehicle);
                            break;
                    }

                    string issuanceJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(massiveLoadProcess), (char)007, riskType, (char)007, nameof(MassiveCancellationRow));
                    QueueHelper.PutOnQueueJsonByQueue(issuanceJson, "CreatePolicyQuee");
                });

            }
            catch (Exception e)
            {
                massiveLoad.HasError = true;
                massiveLoad.ErrorDescription = e.InnerException.Message;
                massiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoad.Id.ToString(), null, null);
        }
        /// <summary>
        /// Creacion Polizas
        /// </summary>
        /// <param name="massiveLoadProcess">The massive load process.</param>
        private void ExecuteCreatePolicy(MassiveCancellationRow massiveCancellationRow)
        {
            try
            {
                massiveCancellationRow.Status = MassiveLoadProcessStatus.Issuance;

                if (massiveCancellationRow.Risk.Policy.Id > 0)
                {
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Finalized;
                    CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetPolicyByOperationId(massiveCancellationRow.Risk.Policy.Id);

                    List<PendingOperation> pendingOperations;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.UtilitiesService.GetPendingOperationsByParentId(massiveCancellationRow.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.UtilitiesService.GetPendingOperationsByParentId(massiveCancellationRow.Risk.Policy.Id);
                    }

                    if (companyPolicy.Id == 0)
                    {
                        companyPolicy.Id = massiveCancellationRow.Risk.Policy.Id;
                    }

                    switch (massiveCancellationRow.SubcoveredRiskType)
                    {
                        case SubCoveredRiskType.JudicialSurety:
                            break;
                        case SubCoveredRiskType.Liability:
                            companyPolicy = CreateIssueLiability(companyPolicy, pendingOperations);
                            break;
                        case SubCoveredRiskType.Property:
                            companyPolicy = CreateIssueProperties(companyPolicy, pendingOperations);
                            break;
                        case SubCoveredRiskType.Surety:
                            break;
                        case SubCoveredRiskType.ThirdPartyLiability:
                            companyPolicy = CreateIssueTPL(companyPolicy, pendingOperations);
                            break;
                        case SubCoveredRiskType.Vehicle:
                            companyPolicy = CreateIssueVehicles(companyPolicy, pendingOperations);
                            break;
                    }

                    var pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperation = DelegateService.UtilitiesService.GetPendingOperationById(massiveCancellationRow.Risk.Policy.Id);
                        pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                        DelegateService.UtilitiesService.UpdatePendingOperation(pendingOperation);

                        DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);

                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperation = DelegateService.UtilitiesService.GetPendingOperationById(massiveCancellationRow.Risk.Policy.Id);
                        PendingOperation pendingOperationPolicy = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyPolicy),
                            Id = companyPolicy.Id
                        };

                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, companyPolicy.Endorsement.Id, (char)007, companyPolicy.Id, (char)007, nameof(MassiveCancellationRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                        /* with Replicated Database */
                    }
                    massiveCancellationRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    massiveCancellationRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;

                }
                else
                {
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = Errors.ErrorTemporalNotFound;
                }
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            catch (Exception ex)
            {
                massiveCancellationRow.HasError = true;
                massiveCancellationRow.Observations = StringHelper.ConcatenateString(Errors.ErrorIssuing, "(", ex.Message, ")");
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }

        public CompanyPolicy CreateIssueVehicles(CompanyPolicy companyPolicy, List<PendingOperation> pendingOperations)
        {

            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
            foreach (PendingOperation po in pendingOperations)
            {
                var companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(po.Operation);
                //Validar modelo
                //companyVehicle.CompanyPolicy = companyPolicy;
                //if (companyVehicle.Id == 0)
                //{
                //    companyVehicle.Id = po.Id;
                //}
                companyVehicles.Add(companyVehicle);
            }
            companyPolicy = DelegateService.vehicleService.CreateEndorsement(companyPolicy, companyVehicles);
            return companyPolicy;
        }

        public CompanyPolicy CreateIssueProperties(CompanyPolicy companyPolicy, List<PendingOperation> pendingOperations)
        {

            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();
            foreach (PendingOperation po in pendingOperations)
            {
                var companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(po.Operation);
                //Validar modelo
                //companyPropertyRisk.CompanyPolicy = companyPolicy;
                //if (companyPropertyRisk.Id == 0)
                //{
                //    companyPropertyRisk.Id = po.Id;
                //}
                companyPropertyRisks.Add(companyPropertyRisk);
            }
            companyPolicy = DelegateService.propertyService.CreateEndorsement(companyPolicy, companyPropertyRisks);
            return companyPolicy;
        }

        public CompanyPolicy CreateIssueTPL(CompanyPolicy companyPolicy, List<PendingOperation> pendingOperations)
        {

            List<CompanyTplRisk> companyTplRisks = new List<CompanyTplRisk>();
            foreach (PendingOperation po in pendingOperations)
            {
                var companyTpl = JsonConvert.DeserializeObject<CompanyTplRisk>(po.Operation);
                //Validar modelo
                //companyTpl.CompanyPolicy = companyPolicy;
                //if (companyTpl.Id == 0)
                //{
                //    companyTpl.Id = po.Id;
                //}
                companyTplRisks.Add(companyTpl);
            }
            companyPolicy = null; /*DelegateService.tplService.CreateEndorsement(companyPolicy, companyTplRisks);*/
            return companyPolicy;
        }

        public CompanyPolicy CreateIssueLiability(CompanyPolicy companyPolicy, List<PendingOperation> pendingOperations)
        {

            List<CompanyLiabilityRisk> companyLiabilityRisks = new List<CompanyLiabilityRisk>();
            foreach (PendingOperation po in pendingOperations)
            {
                var companyLiabilityRisk = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(po.Operation);
                //Validar modelo
                //companyLiabilityRisk.CompanyPolicy = companyPolicy;
                //if (companyLiabilityRisk.Id == 0)
                //{
                //    companyLiabilityRisk.Id = po.Id;
                //}
                companyLiabilityRisks.Add(companyLiabilityRisk);
            }
            companyPolicy = DelegateService.liabilityService.CreateEndorsement(companyPolicy, companyLiabilityRisks);
            return companyPolicy;
        }

        #endregion
        #region validaciones    
        /// <summary>
        /// Validacion Datos duplicados en el cargue
        /// </summary>
        /// <param name="validatedFiles">Lista de Registros a Valida.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Lista Vacia</exception>
        private void CancellationValidate(Template cancellationTemplate)
        {
            var consolidatedPolicies =
                from p in cancellationTemplate.Rows
                where !p.HasError
                group p by new
                {
                    DocumentNumber = p.Fields.First(x => x.PropertyName == FieldPropertyName.PolicyNumber).Value.Trim(),
                    PrefixId = p.Fields.First(x => x.PropertyName == FieldPropertyName.PrefixCode).Value.Trim(),
                    BranchId = p.Fields.First(x => x.PropertyName == FieldPropertyName.BranchId).Value.Trim()
                }
                into policies
                where policies.Count() > 1
                select new
                {
                    DocumentNumber = policies.Key.DocumentNumber,
                    PrefixId = policies.Key.PrefixId,
                    BranchId = policies.Key.BranchId,
                    Total = policies.Count(),
                };

            TP.Parallel.ForEach(cancellationTemplate.Rows, row =>
            {
                var PolicyNumber = consolidatedPolicies.FirstOrDefault(z => z.DocumentNumber == row.Fields.First(y => y.PropertyName == FieldPropertyName.PolicyNumber).Value.Trim())?.DocumentNumber;
                var prefixId = consolidatedPolicies.FirstOrDefault(z => z.PrefixId == row.Fields.First(y => y.PropertyName == FieldPropertyName.PrefixId).Value.Trim())?.PrefixId;
                var branchId = consolidatedPolicies.FirstOrDefault(z => z.BranchId == row.Fields.First(y => y.PropertyName == FieldPropertyName.BranchId).Value.Trim())?.BranchId;
                if (row.Fields != null && !string.IsNullOrEmpty(PolicyNumber) && !string.IsNullOrEmpty(prefixId) && !string.IsNullOrEmpty(branchId))
                {
                    row.HasError = true;
                    row.ErrorDescription = String.Format("{0} : {1} : {4} {2} : {5} {3} : {6}", Errors.ErrorDuplicatePolicies, Errors.PolicyNumber, Errors.Prefix, Errors.Branch, row.Fields.First(u => u.PropertyName == FieldPropertyName.PolicyNumber).Value, row.Fields.First(u => u.PropertyName == FieldPropertyName.PrefixId).Value, row.Fields.First(u => u.PropertyName == FieldPropertyName.BranchId).Value);
                }

            });

        }

        /// <summary>
        /// Validates the policies.
        /// </summary>
        /// <param name="massiveLoadProcess">The massive load process.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="cancellationModel">The cancellation model.</param>
        private void ValidatePolicies(MassiveCancellationRow massiveLoadProcess, Policy policy, CancellationModel cancellationModel)
        {
            if (cancellationModel.CurrentFrom < policy.CurrentFrom || cancellationModel.CurrentFrom >= policy.CurrentTo)
            {
                massiveLoadProcess.HasError = true;
                massiveLoadProcess.Observations = String.Format("{1} : {0}", policy.CurrentFrom, Errors.ErrorCurrentFrom) + KeySettings.ReportErrorSeparatorMessage();
            }
        }
        private void ValidateDateCancellation(MassiveCancellationRow massiveLoadProcess, Policy policy, CancellationModel cancellationModel)
        {
            if (cancellationModel.CurrentFrom < policy.CurrentFrom || cancellationModel.CurrentFrom > policy.CurrentTo)
            {
                throw new Exception(String.Format("{0} : {1} : {2} {3} : {4} ", Errors.ErrorDate, Errors.FromDate, policy.CurrentFrom, Errors.ToDate, policy.CurrentTo) + KeySettings.ReportErrorSeparatorMessage());
            }
        }

        private async void CancellationValidateExtensions(MassiveLoad massiveLoad)
        {
            try
            {

                int parameterValidateVigencyExtended = 1012;
                //Validar
                //Parameter parameter = DelegateService.underwritingService.FindCptParameter(parameterValidateVigencyExtended);
                Parameter parameter = new Parameter();

                if (parameter.BoolParameter.Value && massiveLoad.File != null)
                {
                    List<int> prefixes = DelegateService.commonService.GetHardRiskTypeByCoveredRiskType(CoveredRiskType.Vehicle).Select(h => h.Id).ToList();

                    var template = massiveLoad.File.Templates.First(x => x.PropertyName == TemplatePropertyName.MassiveCancellation);
                    //Validar - Falta el método
                    List<CompanyVehicle> vehicles = new List<CompanyVehicle>();
                    /*DelegateService.vehicleService.GetVehiclesByPolicyNumBranchIdPrefixList(template.Rows
                                                .Where(p => prefixes.Any(i => i == System.Convert.ToInt16(p.Fields.First(y => y.PropertyName == FieldPropertyName.PrefixCode).Value)))
                                                .Select(x => new KeyValuePair<string, int>(x.Fields.First(y => y.PropertyName == FieldPropertyName.PolicyNumber).Value, System.Convert.ToInt16(x.Fields.First(y => y.PropertyName == FieldPropertyName.BranchId).Value)))
                                                .ToDictionary(z => z.Key, z => z.Value), prefixes);*/
                    // var extendedPolicies = await Task.Factory.StartNew(() => DelegateService.vehicleService.GetExtendListByVehiclePlateCollection(vehicles.Select(v => v.LicensePlate).ToList<string>()));
                    // LogExtends(massiveLoad.Id, extendedPolicies);
                }
            }
            catch (Exception e)
            {

            }

        }

        private void LogExtends(int loadId, List<VehiclePolicyExtend> vehicleExtends)
        {
            try
            {
                DataTable dtParameters = new DataTable("MASSIVE_LOAD_LOG_PARAMETER");
                dtParameters.Columns.Add("LOAD_ID", typeof(int));
                dtParameters.Columns.Add("BRANCH_ID", typeof(int));
                dtParameters.Columns.Add("PREFIX_ID", typeof(int));
                dtParameters.Columns.Add("POLICY_NUM", typeof(string));

                foreach (VehiclePolicyExtend extend in vehicleExtends)
                {
                    DataRow dataRow = dtParameters.NewRow();
                    dataRow["LOAD_ID"] = loadId;
                    dataRow["BRANCH_ID"] = extend.BranchCode;
                    dataRow["PREFIX_ID"] = extend.PrefixCode;
                    dataRow["POLICY_NUM"] = extend.DocumentNum;

                    dtParameters.Rows.Add(dataRow);
                }

                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("MASSIVE_LOAD_LOG_PARAMETER", dtParameters);

                DataTable dataTable;
                using (DynamicDataAccess dataAccess = new DynamicDataAccess())
                {
                    dataTable = dataAccess.ExecuteSPDataTable("MSV.LOG_VEHICLE_EXTENTIONS", parameters);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        #endregion

        #region reportes
        /// <summary>
        ///Generar Reporte de Cancelacion Masiva por Identificador y estado del Cargue
        /// </summary>
        /// <returns></returns>
        public string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus)
        {
            Dictionary<string, string> reportPaths = new Dictionary<string, string>();
            string filePathProperty = "";
            string filePathVehicle = "";
            string filePathTpl = "";
            string filePathLiability = "";
            string exportPath = DelegateService.commonService.GetKeyApplication("ReportExportPath") + MassiveLoadId + ".zip";
            //filePathProperty = DelegateService.massivePropertyCancellationService.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);
            filePathVehicle = DelegateService.massiveVehicleCancellationService.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);
            filePathTpl = DelegateService.massiveTplCancellationService.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);
            //filePathLiability = DelegateService.massiveLiabilityCancellationService.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);

            if (!string.IsNullOrEmpty(filePathProperty))
            {
                reportPaths.Add("Reporte_Hogar", filePathProperty);
            }
            if (!string.IsNullOrEmpty(filePathVehicle))
            {
                reportPaths.Add("Reporte_Autos", filePathVehicle);
            }
            if (!string.IsNullOrEmpty(filePathTpl))
            {
                reportPaths.Add("Reporte_Rc Pasajeros", filePathTpl);
            }
            if (!string.IsNullOrEmpty(filePathLiability))
            {
                reportPaths.Add("Reporte_Rc Ubicacion", filePathLiability);
            }
            if (reportPaths.Count > 0)
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (KeyValuePair<string, string> item in reportPaths)
                    {
                        zip.AddFile(item.Value, item.Key);
                    }
                    zip.Save(exportPath);
                }
                TP.Task.Run(() => DeleteFileNames(reportPaths.Values.ToList()));
                return exportPath;
            }
            else
            {
                return "";
            }

        }

        private void DeleteFileNames(List<string> fileNames)
        {
            foreach (string url in fileNames)
            {
                if (System.IO.File.Exists(@url))
                {
                    try
                    {
                        System.IO.File.Delete(@url);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

        }
        #endregion




    }
}
