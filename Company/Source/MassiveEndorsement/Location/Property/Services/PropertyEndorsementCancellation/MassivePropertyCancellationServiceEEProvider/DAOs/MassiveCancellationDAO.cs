using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.MassivePropertyCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Framework;
using Sistran.Company.Application.MassiveServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassivePropertyCancellationService.EEProvider.DAOs
{
    /// <summary>
    /// Cancelacion Masiva
    /// </summary>
    public class MassiveCancellationDAO
    {
        private static readonly object objCache = new object();
        #region reportes
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<CompanyIrregularHeight> irregularHeights = new List<CompanyIrregularHeight>();
        private static List<CompanyIrregularPlant> irregularPlants = new List<CompanyIrregularPlant>();
        private static List<CompanyLevelZone> levelzones = new List<CompanyLevelZone>();
        private static List<CompanyLocation> locations = new List<CompanyLocation>();
        private static List<CompanyReinforcedStructureType> reinforcedStructureTypes = new List<CompanyReinforcedStructureType>();
        private static List<CompanyRepair> companyRepairs = new List<CompanyRepair>();
        private static List<CompanyStructureType> companyStructureTypes = new List<CompanyStructureType>();
        private static List<RiskUse> riskUse = new List<RiskUse>();
        private static List<ConstructionType> constructionTypes = new List<ConstructionType>();
        private static List<LineBusiness> lineBusiness = new List<LineBusiness>();
        private static List<SubLineBusiness> subLineBusiness = new List<SubLineBusiness>();
        private static List<InsuredObject> insuredObjects = new List<InsuredObject>();
        private static List<City> cities = new List<City>();
        private static List<State> states = new List<State>();
        private static List<CompanyRiskTypeEarthquake> riskTypes = null;
        private static List<CompanyDamage> damages = null;
        private static List<RatingZone> ratingZones = null;
        private static List<BillingGroup> billingGroup = new List<BillingGroup>();

        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();
        readonly ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
        #endregion reportes
        /// <summary>
        /// Crear Temporal endoso Cancelacion masiva Autos
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveCancellationRow">Proceso Masivo</param>
        /// <returns></returns>
        public List<AuthorizationRequest> CreateTemporalEndorsementCancellation(MassiveLoad massiveLoad, CompanyPolicy companyPolicy, List<Risk> risks, MassiveCancellationRow massiveCancellationRow)
        {
            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();
            if (risks != null && risks.Any())
            {
                bool updateRow = true;
                try
                {
                    Row row = JsonConvert.DeserializeObject<Row>(massiveCancellationRow.SerializedRow);
                    DateTime cancelationCurrentFrom = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    PendingOperation pendingOperation = new PendingOperation();
                    companyPolicy.CompanyProduct.CoveredRisk = new CoveredRisk
                    {
                        CoveredRiskType = CoveredRiskType.Location,
                        SubCoveredRiskType = massiveCancellationRow.SubcoveredRiskType
                    };
                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(cancelationCurrentFrom).Days;
                    companyPolicy.UserId = massiveLoad.User.UserId;
                    companyPolicy.Endorsement = risks[0].Policy.Endorsement;
                    companyPolicy.CurrentFrom = risks[0].Policy.CurrentFrom;
                    companyPolicy.CurrentTo = risks[0].Policy.CurrentTo;
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.PolicyType.Description = DelegateService.commonService.GetPolicyTypesByPrefixIdById(companyPolicy.Prefix.Id, companyPolicy.PolicyType.Id).Description;
                    List<Currency> LstCurrency = DelegateService.underwritingService.GetCurrenciesByProductId(companyPolicy.CompanyProduct.Id);
                    companyPolicy.ExchangeRate.Currency = LstCurrency.First(x => x.Id == companyPolicy.ExchangeRate.Currency.Id);
                    if (companyPolicy.PaymentPlan == null)
                    {
                        throw new ValidationException(Errors.ErrorWithThePaymentPlanOfThePolicy);
                    }
                    companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(companyPolicy.PaymentPlan.Id);
                    companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                    List<CompanyPropertyRisk> companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(risks[0].Policy.Endorsement.PolicyId);
                    List<CompanyPropertyRisk> filterCompanyRisk = new List<CompanyPropertyRisk>() ;
                    
                    if (companyPropertyRisks == null )
                    {
                        throw new ValidationException(Errors.ErrorThePolicyPresentsDataProblems);
                    }
                    foreach (var item in companyPropertyRisks)
                    {
                        if (risks.Exists(u => u.RiskId == item.RiskId))
                        {
                            filterCompanyRisk.Add(item);
                        }
                    }
                    companyPropertyRisks = filterCompanyRisk;
                    if ((companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Any()))
                    {
                        CompanyPropertyRisk cp = companyPropertyRisks.First();
                        cp.Policy = companyPolicy;
                        authorizationRequests.AddRange(DelegateService.massiveService.GetAuthorizationPolicies(cp, massiveLoad));
                    }
                    //Crea poliza

                    foreach (CompanyPropertyRisk property in companyPropertyRisks)
                    {
                        List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(risks[0].Policy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, property.RiskId);
                        List<InsuredObject> insuredObjects = DelegateService.underwritingService.GetInsuredObjectsByRiskId(property.RiskId);
                        var rsk = risks.First(x => x.RiskId == property.RiskId);
                        rsk.Status = RiskStatusType.Excluded;
                        List<Coverage> cancellationCoverages = rsk.Coverages;
                        property.Status = RiskStatusType.Excluded;
                        ParallelHelper.ForEach(cancellationCoverages, (coverage) =>
                        {
                            coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                            coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                            coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                            coverage.Deductible = coverages.First(x => x.Id == coverage.Id).Deductible;
                            coverage.Description = coverages.First(x => x.Id == coverage.Id).Description;
                            coverage.DeclaredAmount = coverages.First(x => x.Id == coverage.Id).DeclaredAmount;
                            coverage.LimitAmount = coverages.First(x => x.Id == coverage.Id).LimitAmount;
                            coverage.InsuredObject = coverages.First(u => u.Id == coverage.Id).CompanyInsuredObject;
                            coverage.InsuredObject.Amount = insuredObjects.First(u => u.Id == coverage.InsuredObject.Id).Amount;
                        });
                        property.CompanyRisk.CompanyCoverages = DelegateService.underwritingService.CreateCompanyCoverages(cancellationCoverages);
                    }
                    companyPolicy.Summary = new Summary { RiskCount = risks.Count };
                    bool returnExpenses = companyPolicy.Endorsement.CancellationTypeId == (int)CancellationType.BeginDate;
                    companyPolicy.PayerComponents = DelegateService.underwritingService.CompanyCalculatePayerComponents(companyPolicy, risks, returnExpenses);
                    Policy policy = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                    companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policy, risks);
                    companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policy, risks);
                    
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
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policy);
                    }
                    pendingOperation.Id = companyPolicy.Id;
                    pendingOperation.UserId = risks[0].Policy.UserId;
                    pendingOperation.OperationName = "Temporal";
                    pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);

                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                    }
                    else
                    {
                        string pendingOperationJson = JsonConvert.SerializeObject(pendingOperation);
                        var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee", serialization: "JSON");
                        queue.PutOnQueue(pendingOperationJson);
                    }

                    DelegateService.underwritingService.CreateTempSubscription(companyPolicy); 

                    if (companyPropertyRisks != null && companyPropertyRisks.Any())
                    {
                        PendingOperation pendingOperationProperty = new PendingOperation();
                        massiveCancellationRow.Observations = companyPolicy.Summary.Premium.ToString();
                        massiveCancellationRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        massiveCancellationRow.Status = MassiveLoadProcessStatus.Events;
                        massiveCancellationRow.Risk = new Risk { Id = companyPolicy.Id, Description = companyPropertyRisks[0].Description, Policy = new Policy { Id = companyPolicy.Id, DocumentNumber = companyPolicy.DocumentNumber, Summary = new Summary { FullPremium = companyPolicy.Summary.FullPremium }, PolicyType = new PolicyType { Id = companyPolicy.PolicyType.Id, Description = companyPolicy.PolicyType.Description }, Branch = companyPolicy.Branch, Prefix = companyPolicy.Prefix } };
                        massiveCancellationRow.HasEvents = (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count != 0);

                        foreach (CompanyPropertyRisk property in companyPropertyRisks)
                        {
                            property.Status = RiskStatusType.Excluded;
                            property.CoveredRiskType = CoveredRiskType.Location;
                            pendingOperationProperty = new PendingOperation();
                            pendingOperationProperty.ParentId = companyPolicy.Id;
                            pendingOperationProperty.Operation = JsonConvert.SerializeObject(property);

                            if (!Settings.UseReplicatedDatabase())
                            {
                                pendingOperationProperty = DelegateService.commonService.CreatePendingOperation(pendingOperationProperty);
                                property.Id = pendingOperationProperty.Id;
                                property.CompanyPolicy = companyPolicy;
                            }
                            else
                            {
                                string pendingOperationJson = JsonConvert.SerializeObject(pendingOperationProperty);
                                var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePendingOperationQuee", routingKey: "CreatePendingOperationQuee", serialization: "JSON");
                                queue.PutOnQueue(pendingOperationJson);
                            }
                        }
                    }
                    else
                    {
                        massiveCancellationRow.HasError = true;
                        massiveCancellationRow.Observations = Errors.ErrorRiskNotExist;
                        massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                    }
                }
                catch (System.Exception ex)
                {
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = ex.Message;
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
                massiveCancellationRow.Observations = Errors.ErrorRiskNotExist;
                massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            return authorizationRequests;
        }

        /// <summary>
        /// Generar Reporte de Cancelacion Masiva Multiriesgo Por identificador y estado del cargue
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
            List<LineBusiness> lineBusiness = DelegateService.commonService.GetLineBusinessBySubCoveredRiskType(SubCoveredRiskType.Property);
            List<MassiveCancellationRow> massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(MassiveLoadId, SubCoveredRiskType.Property, processStatus, false, null);
            MassiveEmission massiveEmission = new MassiveEmission();
            massiveEmission.Id = MassiveLoadId;
            massiveEmission.Prefix = new Prefix { Id = lineBusiness.FirstOrDefault().Id };
            List<Row> rows = new List<Row>();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = (int)massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)SubCoveredRiskType.Property;
            massiveEmission.Status = massiveLoadStatus;
            File file = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (file == null)
            {
                return null;
            }
            else
            {
                file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
                string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
                string key = Guid.NewGuid().ToString();
                string filePath = "";
                 
                int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

                file.FileType = FileType.CSV;
                TP.Parallel.ForEach(massiveCancellationRows,
                    (process) =>
                    {
                        string serialize = serializeFields;
                        FillPropertyFields(massiveEmission, process, serialize);

                        if (concurrentRows.Count >= bulkExcel || massiveCancellationRows.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte Hogar_" + key + "_" + massiveEmission.Id;
                            filePath = DelegateService.commonService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();

                        }
                    });
                
                return filePath;
            }
        }

        public void FillPropertyFields(MassiveEmission massiveEmission, MassiveCancellationRow massiveCancellationRow, string serializeFields)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyByMassiveLoadStatus(massiveEmission.Status.Value, massiveCancellationRow.Risk.Policy);
                companyPolicy.Id = massiveCancellationRow.Risk.Policy.Id;
                if (companyPolicy != null)
                {
                    List<CompanyPropertyRisk> risks = GetCompanyPropertyRisk(massiveEmission, massiveCancellationRow, companyPolicy.Id);

                    List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);

                    foreach (CompanyPropertyRisk property in risks)
                    {
                        property.CompanyRisk.CompanyInsured.Name = property.CompanyRisk.CompanyInsured.Name + " " + property.CompanyRisk.CompanyInsured.Surname + " " + property.CompanyRisk.CompanyInsured.SecondSurname;
                        fields = DelegateService.massiveService.FillInsuredFields(fields, property.CompanyRisk.CompanyInsured);
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = massiveCancellationRow.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = property.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = property.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskAddress).Value = CreateAddress(property);
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskFloorNumber).Value = property.FloorNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskConstructionYear).Value = property.ConstructionYear.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLongitude).Value = property.Longitude.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLatitude).Value = property.Latitude.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(property.Beneficiaries);
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskUseDescription).Value = (property.RiskUse != null && property.RiskUse.Id > 0) ? riskUse.FirstOrDefault(u => u.Id == property.RiskUse.Id).Description : ""; ;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskTypeDescription).Value = (property.CompanyRiskTypeEarthquake != null && property.CompanyRiskTypeEarthquake.Id > 0) ? riskTypes.FirstOrDefault(u => u.Id == property.CompanyRiskTypeEarthquake.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.BillingGroup).Value = (companyPolicy.Request != null && companyPolicy.Request.BillingGroupId > 0) ? billingGroup.FirstOrDefault(u => u.Id == companyPolicy.Request.BillingGroupId).Description + " (" + companyPolicy.Request.BillingGroupId + ")" : "";

                        fields.Find(u => u.PropertyName == FieldPropertyName.RaitingZoneDescription).Value = (property.RatingZone != null && property.RatingZone.Id > 0) ? ratingZones.FirstOrDefault(u => u.Id == property.RatingZone.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.Observations).Value = "";

                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.StructureDescription).Value = (property.CompanyStructureType != null && property.CompanyStructureType.Id > 0) ? companyStructureTypes.FirstOrDefault(u => u.TypeCD == property.CompanyStructureType.Id).Description : ""; ;
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularHeightDescription).Value = (property.CompanyIrregularHeight != null && property.CompanyIrregularHeight.Id > 0) ? irregularHeights.FirstOrDefault(u => u.Id == property.CompanyIrregularHeight.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularPlantDescription).Value = (property.CompanyIrregularPlant != null && property.CompanyIrregularPlant.Id > 0) ? irregularPlants.FirstOrDefault(u => u.Id == property.CompanyIrregularPlant.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskRepairedDescripcion).Value = (property.CompanyRepair != null && property.CompanyRepair.Id > 0) ? companyRepairs.FirstOrDefault(u => u.Id == property.CompanyRepair.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureDescription).Value = (property.CompanyReinforcedStructureType != null && property.CompanyReinforcedStructureType.Id > 0) ? reinforcedStructureTypes.FirstOrDefault(u => u.Id == property.CompanyReinforcedStructureType.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskPreviousDamageDescription).Value = (property.CompanyDamage != null && property.CompanyDamage.Id > 0) ? damages.FirstOrDefault(u => u.Id == property.CompanyDamage.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskLocationDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskNeighborhoodDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCOfConstructionDescription).Value = (property.ConstructionType != null && property.ConstructionType.Id > 0) ? constructionTypes.FirstOrDefault(u => u.Id == property.ConstructionType.Id).Description : "";
                        //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskDangerDescription).Value = "";
                        //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskPercentageClaims).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskNeighborhoodDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(property.Clauses);

                        //Asistencia
                        List<int> assistanceCoveragesIds = DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceProperty);
                        List<CompanyCoverage> coveragesAssistance = property.CompanyRisk.CompanyCoverages.Where(u => assistanceCoveragesIds.Exists(id => id == u.Id)).ToList();
                        decimal assistancePremium = coveragesAssistance.Sum(x => x.PremiumAmount);
                        if (assistancePremium != 0)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                                (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                                (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                        }

                        serializeFields = JsonConvert.SerializeObject(fields);
                        decimal valueRc = 0, insuredValue = 0;
                        valueRc = property.CompanyRisk.CompanyCoverages.Where(u => u.Description.Contains("R.C.E")).Sum(u => u.LimitAmount);
                        insuredValue = (companyPolicy.Summary.AmountInsured - valueRc);

                        foreach (CompanyCoverage coverage in property.CompanyRisk.CompanyCoverages)
                        {
                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                            fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = insuredValue.ToString();
                            fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = valueRc.ToString();
                            if (coverage.SubLineBusiness != null && coverage.SubLineBusiness.LineBusiness != null)
                            {
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.LineBusinessDescripcion).Value = lineBusiness.DefaultIfEmpty(new LineBusiness { Description = "" }).FirstOrDefault(u => u.Id == coverage.SubLineBusiness.LineBusiness.Id).Description;
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.SubLineBusinessDescripcion).Value = subLineBusiness.DefaultIfEmpty(new SubLineBusiness { Description = "" }).FirstOrDefault(u => u.Id == coverage.SubLineBusiness.Id).Description;
                            }
                            else
                            {
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.LineBusinessDescripcion).Value = "";
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.SubLineBusinessDescripcion).Value = "";

                            }
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.InsuredObjectDescription).Value = insuredObjects.DefaultIfEmpty(new InsuredObject { Description = "" }).FirstOrDefault(u => u.Id == coverage.CompanyInsuredObject.Id).Description;
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
        private List<CompanyPropertyRisk> GetCompanyPropertyRisk(MassiveEmission massiveEmission, MassiveCancellationRow proccess, int tempId)
        {
            List<CompanyPropertyRisk> companyProperties = new List<CompanyPropertyRisk>();
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                    }
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(item.Operation));
                    }


                    break;
                case MassiveLoadStatus.Issued:
                    //companyProperties = DelegateService.propertyService.GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(massiveEmission.Prefix.Id, massiveEmission.Branch.Id, proccess.Risk.Policy.DocumentNumber, EndorsementType.Cancellation);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));
                    }
                    break;
            }
            return companyProperties;

        }

        private string CreateAddress(CompanyPropertyRisk propertyRisk)
        {
            string address = "";
            address += propertyRisk.FullAddress;

            if (propertyRisk.City != null && propertyRisk.City.State != null)
            {
                address += StringHelper.ConcatenateString(
                    " | ", states.DefaultIfEmpty(new State { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.State.Id).Description,
                    " | ", cities.DefaultIfEmpty(new City { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.Id && u.State.Id == propertyRisk.City.State.Id).Description);
            }

            return address;

        }
        private void LoadList()
        {
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            irregularHeights = DelegateService.propertyService.GetCompanyIrregularHeights();
            irregularPlants = DelegateService.propertyService.GetCompanyIrregularPlants();
            levelzones = new List<CompanyLevelZone>();
            locations = new List<CompanyLocation>();
            reinforcedStructureTypes = DelegateService.propertyService.GetCompanyReinforcedStructureTypes();
            companyRepairs = DelegateService.propertyService.GetCompanyRepairs();
            companyStructureTypes = DelegateService.propertyService.GetCompanyStructureTypes();
            riskUse = DelegateService.propertyService.GetRiskUses();
            constructionTypes = DelegateService.propertyService.GetConstructionTypes();
            lineBusiness = DelegateService.commonService.GetLinesBusiness();
            subLineBusiness = DelegateService.commonService.GetSubLineBusiness();
            insuredObjects = DelegateService.underwritingService.GetInsuredObjects();
            cities = DelegateService.commonService.GetCities();
            states = DelegateService.commonService.GetStates();
            riskTypes = DelegateService.propertyService.GetCompanyRiskTypeEarthquakes();
            damages = DelegateService.propertyService.GetCompanyDamages();
            ratingZones = DelegateService.commonService.GetRatingZones();
            billingGroup = DelegateService.underwritingService.GetBillingGroup();
        }
    }
}
