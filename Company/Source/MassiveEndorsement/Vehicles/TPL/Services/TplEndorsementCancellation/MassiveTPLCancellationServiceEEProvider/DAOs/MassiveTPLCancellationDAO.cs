using Newtonsoft.Json;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.MassiveTPLCancellationService.EEProvider.Assemblers;
using Sistran.Company.Application.MassiveTPLCancellationServices.EEProvider.Resources;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUEN = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassiveTPLCancellationService.EEProvider.DAOs
{
    /// <summary>
    /// Cancelacion Masiva Autos 
    /// </summary>
    public class MassiveTPLCancellationDAO
    {
        private static readonly Object objCache = new object();
        #region propiedades
        private static List<Use> uses = new List<Use>();
        private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<Color> colors = new List<Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Accessory> accesoriesList = new List<Accessory>();
        private static List<Core.Application.Vehicles.Models.Type> types = new List<Core.Application.Vehicles.Models.Type>();
        private static List<RatingZone> ratingZones = new List<RatingZone>();
        private static List<BillingGroup> billingGroup = new List<BillingGroup>();

        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();
        readonly ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
        #endregion
        /// <summary>
        /// Crear Temporal endoso Cancelacion masiva Autos
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveCancellationRow">Proceso Masivo</param>
        public List<AuthorizationRequest> CreateTemporalEndorsementCancellation(MassiveLoad massiveLoad, CompanyPolicy companyPolicy, List<CompanyRisk> risks, MassiveCancellationRow massiveCancellationRow)
        {
            List<UserGroupModel>userGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(massiveLoad.User.UserId).Select(x => new UserGroupModel { UserId = massiveLoad.User.UserId, GroupId = x.GroupId }).ToList();
            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();
            if (risks != null && risks.Any())
            {
                bool updateRow = true;
                try
                {
                    Row row = JsonConvert.DeserializeObject<Row>(massiveCancellationRow.SerializedRow);
                    DateTime cancelationCurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    PendingOperation pendingOperation = new PendingOperation();
                    companyPolicy.Product.CoveredRisk = new CompanyCoveredRisk
                    {
                        CoveredRiskType = CoveredRiskType.Vehicle,
                        SubCoveredRiskType = massiveCancellationRow.SubcoveredRiskType
                    };
                    companyPolicy.UserId = massiveLoad.User.UserId;

                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days;
                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(cancelationCurrentFrom).Days;
                    companyPolicy.CurrentFrom = risks[0].Policy.CurrentFrom;
                    companyPolicy.CurrentTo = risks[0].Policy.CurrentTo;
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.Summary = new CompanySummary { RiskCount = risks.Count };
                    companyPolicy.PolicyType.Description = DelegateService.commonService.GetPolicyTypesByPrefixIdById(companyPolicy.Prefix.Id, companyPolicy.PolicyType.Id).Description;
                    List<Currency> LstCurrency = DelegateService.productService.GetCurrenciesByProductId(companyPolicy.Product.Id);
                    companyPolicy.ExchangeRate.Currency = LstCurrency.First(x => x.Id == companyPolicy.ExchangeRate.Currency.Id);
                    if (companyPolicy.PaymentPlan == null)
                    {
                        companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPolicyId(risks[0].Policy.Id);
                        if (companyPolicy.PaymentPlan == null)
                        {
                            throw new ValidationException(Errors.ErrorWithThePaymentPlanOfThePolicy);
                        }
                    }
                    companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(companyPolicy.PaymentPlan.Id);

                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);

                    companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                    List<CompanyTplRisk> companyTplRisks = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(risks[0].Policy.Endorsement.PolicyId);
                    List<CompanyTplRisk> filterCompanyTpl = new List<CompanyTplRisk>();

                    if (companyTplRisks == null || !companyTplRisks.Any())
                    {
                        throw new ValidationException(Errors.ErrorThePolicyPresentsDataProblems);
                    }

                    foreach (var item in companyTplRisks)
                    {
                        if (risks.Exists(u => u.Number == item.Risk.Number))
                        {
                            filterCompanyTpl.Add(item);
                        }
                    }
                    companyTplRisks = filterCompanyTpl;

                   
                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == AUEN.TypePolicies.Restrictive))
                    {
                        throw new Exception(string.Format(Errors.PoliciesRestrictive + "</br>", string.Join("</br>", companyPolicy.InfringementPolicies.Where(x => x.Type == AUEN.TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                    }

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == AUEN.TypePolicies.Authorization))
                    {
                        List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
                        policiesAuts.AddRange(companyPolicy.InfringementPolicies);
                        authorizationRequests.AddRange(DelegateService.massiveService.ValidateAuthorizationPolicies(policiesAuts, massiveLoad, companyPolicy.Id));
                        massiveCancellationRow.HasEvents = true;
                    }
                    if (authorizationRequests.Any())
                    {
                        DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                    }

                    foreach (CompanyTplRisk tplRisk in companyTplRisks)
                    {
                        tplRisk.Risk.Status = RiskStatusType.Excluded;
                        tplRisk.Risk.Policy = companyPolicy;
                        tplRisk.Risk.Id = (int)massiveCancellationRow.tempId;
                        var rsk = risks.FirstOrDefault(x => x.Number == tplRisk.Risk.Number);
                        if (rsk != null && rsk?.Coverages?.Count > 0)
                        {
                            List<CompanyCoverage> companyCoverage = rsk.Coverages;
                            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdByRiskId(risks[0].Policy.Endorsement.PolicyId, tplRisk.Risk.RiskId);
                            if (coverages?.Count > 0)
                            {
                                ParallelHelper.ForEach(companyCoverage, (coverage) =>
                            {
                                coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                                coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                coverage.Deductible = coverages.First(x => x.Id == coverage.Id).Deductible;
                                coverage.Description = coverages.First(x => x.Id == coverage.Id).Description;
                                coverage.DeclaredAmount = coverages.First(x => x.Id == coverage.Id).DeclaredAmount;
                                coverage.LimitAmount = coverages.First(x => x.Id == coverage.Id).LimitAmount;
                            });
                            }
                            else
                            {
                                throw new ValidationException(Errors.ErrorRiskCoverageNotFound);
                            }
                            tplRisk.Risk.Coverages = companyCoverage;

                        }
                        else
                        {
                            throw new Exception(Errors.ErrorRiskNotFound);
                        }
                    }

                    companyTplRisks = DelegateService.tplService.QuotateThirdPartyLiabilities(companyPolicy, companyTplRisks, false, false);
                    bool returnExpenses = companyPolicy.Endorsement.CancellationTypeId == (int)CancellationType.BeginDate;
                    companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyPolicy, risks);
                    companyPolicy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyPolicy, risks);
                    companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);

                    if (companyPolicy.Request != null
                            && companyPolicy.Request.Id > 0 && companyPolicy.Request.BillingGroupId > 0)
                    {
                        CompanyRequest companyRequest = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(companyPolicy.Request.BillingGroupId, companyPolicy.Request.Id.ToString(), null).FirstOrDefault();
                        CompanyRequestEndorsement companyRequestEndorsement = DelegateService.massiveService.GetCompanyRequestEndorsmentPolicyWithRequest(companyPolicy.CurrentFrom, companyRequest);

                        if (companyRequestEndorsement == null)
                        {
                            throw new ValidationException(Errors.CompanyRequestNotRenewed);
                        }

                        companyPolicy.PayerPayments = DelegateService.underwritingService.CalculatePayerPayment(companyPolicy, companyRequestEndorsement.IsOpenEffect, companyRequestEndorsement.CurrentFrom, companyRequestEndorsement.CurrentTo);
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotasWithrequestGroupig(companyPolicy.PayerPayments);
                    }
                    else
                    {
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
                    }

                    pendingOperation.Id = companyPolicy.Id;
                    pendingOperation.UserId = risks[0].Policy.UserId;
                    pendingOperation.OperationName = "Temporal";
                    pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);

                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                    }
                    else
                    {
                        string pendingOperationJson = JsonConvert.SerializeObject(pendingOperation);
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                        
                    }

                    CompanyTplRisk companyTplRisk;
                    PendingOperation pendingOperationTpl;
                    massiveCancellationRow.Observations = companyPolicy.Summary.Premium.ToString();
                    massiveCancellationRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                    massiveCancellationRow.HasEvents = (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count != 0);
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Events;

                    foreach (CompanyTplRisk tplRisk in companyTplRisks)
                    {
                        companyTplRisk = tplRisk;
                        tplRisk.Risk.Status = RiskStatusType.Excluded;
                        tplRisk.Rate = tplRisk.Rate * -1;
                        tplRisk.Risk.CoveredRiskType = CoveredRiskType.Vehicle;
                        tplRisk.Risk.LimitRc = DelegateService.underwritingService.GetCompanyLimitRcById(tplRisk.Risk.LimitRc.Id);

                        pendingOperationTpl = new PendingOperation();
                        pendingOperationTpl.ParentId = companyPolicy.Id;
                        pendingOperationTpl.Operation = JsonConvert.SerializeObject(tplRisk);

                        if (!Settings.UseReplicatedDatabase())
                        {
                            pendingOperationTpl = DelegateService.utilitiesService.CreatePendingOperation(pendingOperationTpl);
                            tplRisk.Risk.Id = pendingOperationTpl.Id;
                            tplRisk.Risk.Policy = companyPolicy;
                        }
                        else
                        {
                            pendingOperationTpl = DelegateService.pendingOperationEntity.CreatePendingOperation(pendingOperationTpl);
                            tplRisk.Risk.Id = pendingOperationTpl.Id;
                            tplRisk.Risk.Policy = companyPolicy;
                        }

                        tplRisk.Risk.Policy = companyPolicy;
                        companyTplRisk = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(tplRisk, true);

                    }
                    massiveCancellationRow.Risk = new Risk
                    {
                        Id = companyTplRisks[0].Risk.Id,
                        Description = companyTplRisks[0].LicensePlate,
                        Policy = new Policy
                        {
                            Id = companyPolicy.Id,
                            DocumentNumber = companyPolicy.DocumentNumber,
                            Summary = new Summary
                            {
                                FullPremium = companyPolicy.Summary.FullPremium
                            },
                            PolicyType = new PolicyType
                            {
                                Id = companyPolicy.PolicyType.Id,
                                Description = companyPolicy.PolicyType.Description
                            },
                            Branch = new Branch
                            {
                                Id = companyPolicy.Branch.Id
                            },
                            Prefix = new Prefix
                            {
                                Id = companyPolicy.Prefix.Id
                            }
                        }
                    };

                }
                catch (Exception ex)
                {
                    string[] messages = ex.Message.Split('|');
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = messages[0];
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                }
                finally
                {
                    if (updateRow)
                    {
                        DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                    }

                    DataFacadeManager.Dispose();
                }
            }
            else
            {
                massiveCancellationRow.HasError = true;
                massiveCancellationRow.Observations = Errors.ErrorRiskNotFound;
                massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            return authorizationRequests;
        }
        #region Reportes

        /// <summary>
        /// Generar Reporte de Cancelacion Masiva Autos Por identificador y estado del cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns>
        /// File
        /// </returns>
        public string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus)
        {
            MassiveLoadProcessStatus processStatus = MassiveLoadProcessStatus.Validation;
            switch (massiveLoadStatus)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = MassiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = MassiveLoadProcessStatus.Finalized;
                    break;
            }
            DelegateService.massiveService.LoadReportCacheList();
            LoadList();
            List<LineBusiness> lineBusiness = DelegateService.commonService.GetLineBusinessBySubCoveredRiskType(SubCoveredRiskType.ThirdPartyLiability);
            List<MassiveCancellationRow> massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(MassiveLoadId, SubCoveredRiskType.ThirdPartyLiability, processStatus, false, null);
            if (massiveCancellationRows == null || !massiveCancellationRows.Any())
            {
                return null;
            }

            MassiveEmission massiveEmission = new MassiveEmission();
            massiveEmission.Id = MassiveLoadId;
            massiveEmission.Prefix = new Prefix { Id = lineBusiness.FirstOrDefault().Id };
            List<Row> rows = new List<Row>();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = (int)massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)SubCoveredRiskType.ThirdPartyLiability;
            massiveEmission.Status = massiveLoadStatus;
            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null)
            {
                return null;
            }

            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            string key = Guid.NewGuid().ToString();
            string filePath = "";
            
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

            file.FileType = FileType.CSV;
            TP.Parallel.ForEach(massiveCancellationRows,
                (process) =>
                {
                    //string serialize = serializeFields;
                    FillVehicleFields(massiveEmission, process, serializeFields);

                    if (concurrentRows != null && concurrentRows.Count != 0 && concurrentRows.Count >= bulkExcel || massiveCancellationRows.Count == 0)
                    {
                        file.Templates[0].Rows = concurrentRows.ToList();
                        file.Name = "Reporte rcp_" + key + "_" + massiveEmission.Id;
                        filePath = DelegateService.utilitiesService.GenerateFile(file);
                        concurrentRows = new ConcurrentBag<Row>();
                    }
                });
            
            return filePath;
        }
        private void FillVehicleFields(MassiveEmission massiveEmission, MassiveCancellationRow massiveCancellationRow, string serializeFields)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveEmission.Status.Value, massiveCancellationRow.Risk.Policy);
                companyPolicy.Id = massiveCancellationRow.Risk.Policy.Id;
                //int goodExpNumPrinter = DelegateService.vehicleService.GetGoodExpNumPrinter();
                if (companyPolicy != null)
                {
                    List<CompanyTplRisk> tplRisks = GetCompanyTpl(massiveEmission, massiveCancellationRow, massiveCancellationRow.Risk.Id);
                    //CompletAgencyData(companyPolicy.Agencies);
                    List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
                    foreach (CompanyTplRisk tplRisk in tplRisks)
                    {
                        tplRisk.Risk.MainInsured.Name = tplRisk.Risk.MainInsured.Name + " " + tplRisk.Risk.MainInsured.Surname + " " + tplRisk.Risk.MainInsured.SecondSurname;
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = massiveCancellationRow.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
                        fields = DelegateService.massiveService.FillInsuredFields(fields, tplRisk.Risk.MainInsured);
                        //fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = tplRisk.Price.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = tplRisk.Risk.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = (tplRisk.Risk.RatingZone != null && tplRisk.Risk.RatingZone.Id > 0) ? ratingZones.FirstOrDefault(u => u.Id == tplRisk.Risk.RatingZone.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == tplRisk.Version.Type.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = tplRisk.Make.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = tplRisk.Model?.Description + " " + tplRisk.Version?.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.FirstOrDefault(u => u.Id == tplRisk?.Color?.Id)?.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = tplRisk.Year.ToString();
                        //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskFasecolda).Value = tplRisk.Fasecolda.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = tplRisk.LicensePlate;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = tplRisk.EngineSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = tplRisk.ChassisSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = uses?.FirstOrDefault(u => u.Id == tplRisk?.Use?.Id)?.Description ?? "";
                        //fields.Find(u => u.PropertyName == FieldPropertyName.RiskLimitRcDescription).Value = tplRisk.Risk.LimitRc.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = tplRisk.Rate.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.AccesoriesDescription).Value = CreateAccessories(tplRisk.Accesories);
                        //fields.Find(u => u.PropertyName == FieldPropertyName.TotalAccesories).Value = tplRisk.PriceAccesories.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = tplRisk.Risk.AmountInsured.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(tplRisk.Beneficiaries);
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(tplRisk.Clauses);
                        fields.Find(u => u.PropertyName == FieldPropertyName.BillingGroup).Value = (companyPolicy.Request != null && companyPolicy.Request.BillingGroupId > 0) ? billingGroup.FirstOrDefault(u => u.Id == companyPolicy.Request.BillingGroupId).Description + " (" + companyPolicy.Request.BillingGroupId + ")" : "";
                        //if (tplRisk.GoodExperienceYear != null)
                        //{
                        //    if (tplRisk.GoodExperienceYear.GoodExpNumPrinter < 0)
                        //    {
                        //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = "0";
                        //    }
                        //    else
                        //    {
                        //        //if (tplRisk.GoodExperienceYear.GoodExpNumPrinter >= goodExpNumPrinter)
                        //        //{
                        //        //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = String.Format("{0} o más", goodExpNumPrinter);
                        //        //}
                        //        //else
                        //        //{
                        //        //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = tplRisk.GoodExperienceYear.GoodExpNumPrinter.ToString();
                        //        //}
                        //    }
                        //}

                        //Asistencia 
                        CompanyCoverage coverageAsistance = tplRisk.Risk.Coverages.FirstOrDefault(u => u.Id == 9);
                        if (coverageAsistance != null)
                        {
                            var assistancePremium = coverageAsistance.PremiumAmount;
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                                (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                                (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                        }

                        serializeFields = JsonConvert.SerializeObject(fields);
                        foreach (CompanyCoverage coverage in tplRisk.Risk.Coverages.OrderByDescending(u => u.Number))
                        {
                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);

                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.EndorsementSublimitAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                            concurrentRows.Add(new Row
                            {
                                Number = massiveCancellationRow.RowNumber,
                                Fields = fieldsC
                            });

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        //private void CompletAgencyData(List<IssuanceAgency> agencies)
        //{
        //    foreach (IssuanceAgency agency in agencies)
        //    {
        //        string agencyFullName = String.Empty;
        //        var individual = DelegateService.uniquePersonService.GetIndividualByIndividualId(agency.Agent.IndividualId);
        //        if (individual.IndividualType == Core.Application.UniquePersonService.Enums.IndividualType.LegalPerson)
        //        {
        //            agencyFullName = DelegateService.underwritingService.geta GetAgencyByIndividualId(agency.Agent.IndividualId).FirstOrDefault().FullName;
        //        }

        //        agency.FullName = agencyFullName;
        //    }
        //}

        private List<CompanyTplRisk> GetCompanyTpl(MassiveEmission massiveEmission, MassiveCancellationRow proccess, int tempId)
        {
            List<CompanyTplRisk> companyTplRisks = new List<CompanyTplRisk>();
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                    }
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    //companyTplRisks = DelegateService.vehicleService.GetVehiclesByPrefixBranchDocumentNumberEndorsementType(massiveEmission.Prefix.Id, massiveEmission.Branch.Id, proccess.Risk.Policy.DocumentNumber, EndorsementType.Cancellation);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                    }
                    else
                    {
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                        /* with Replicated Database */
                    }
                    break;
            }
            return companyTplRisks;

        }


        private void LoadList()
        {
            if (uses == null || uses.Count == 0)
            {
                uses = new List<Use>();//; DelegateService.vehicleService.GetUses();
            }

            if (colors.Count == 0)
            {
                colors = DelegateService.tplService.GetColors();
            }
            if (types.Count == 0)
            {
                types = DelegateService.tplService.GetTypes();
            }

            documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            ratingZones = DelegateService.underwritingService.GetRatingZones();
            billingGroup = DelegateService.underwritingService.GetBillingGroup();
        }
        #endregion reportes


    }
}
