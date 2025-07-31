using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.MassiveLiabilityCancellationService.EEProvider.Resources;
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
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassiveLiabilityCancellationService.EEProvider.DAOs
{
    /// <summary>
    /// Cancelacion Masiva
    /// </summary>
    public class MassiveCancellationDAO
    {
        private static readonly object objCache = new object();
        #region reportes
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<RiskUse> riskUse = new List<RiskUse>();
        private static List<ConstructionType> constructionTypes = new List<ConstructionType>();
        private static List<LineBusiness> lineBusiness = new List<LineBusiness>();
        private static List<SubLineBusiness> subLineBusiness = new List<SubLineBusiness>();
        private static List<InsuredObject> insuredObjects = new List<InsuredObject>();
        private static List<City> cities = new List<City>();
        private static List<State> states = new List<State>();
        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();
        readonly ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
        #endregion reportes
        /// <summary>
        /// Crear Temporal endoso Cancelacion masiva Autos
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveCancellationRow">Proceso Masivo</param>
        /// <returns></returns>
        public void CreateTemporalEndorsementCancellation(CompanyPolicy companyPolicy, List<Risk> risks, MassiveCancellationRow massiveCancellationRow)
        {
            if (risks != null && risks.Any())
            {
                try
                {
                    PendingOperation pendingOperation = new PendingOperation();
                    companyPolicy.CompanyProduct.CoveredRisk = new CoveredRisk
                    {
                        CoveredRiskType = CoveredRiskType.Vehicle
                    };
                    companyPolicy.UserId = risks[0].Policy.UserId;
                    companyPolicy.Endorsement = risks[0].Policy.Endorsement;
                    companyPolicy.CurrentFrom = risks[0].Policy.CurrentFrom;
                    companyPolicy.CurrentTo = risks[0].Policy.CurrentTo;
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.Id = 0;
                    companyPolicy.PolicyType.Description = DelegateService.commonService.GetPolicyTypesByPrefixIdById(companyPolicy.Prefix.Id, companyPolicy.PolicyType.Id).Description;
                    //Crea poliza
                    pendingOperation.UserId = risks[0].Policy.UserId;
                    pendingOperation.OperationName = "Temporal";
                    pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                    pendingOperation = DelegateService.commonService.CreatePendingOperation(pendingOperation);
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.Summary = new Summary { RiskCount = risks.Count };

                    List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiabilitiesByPolicyId(risks[0].Policy.Endorsement.PolicyId);
                    if (companyLiabilityRisks != null && companyLiabilityRisks.Any())
                    {
                        PendingOperation pendingOperationLiability = new PendingOperation();

                        foreach (CompanyLiabilityRisk liability in companyLiabilityRisks)
                        {
                            liability.Status = RiskStatusType.Excluded;
                            liability.CoveredRiskType = CoveredRiskType.Location;
                            List<Coverage> cancellationCoverages = risks.First(x => x.RiskId == liability.RiskId).Coverages;
                            List<Coverage> coverages = DelegateService.underwritingService.GetCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.CompanyProduct.Id, liability.GroupCoverage.Id, companyPolicy.Prefix.Id);
                            List<InsuredObject> insuredObjects = DelegateService.underwritingService.GetInsuredObjectsByRiskId(liability.RiskId);
                            ParallelHelper.ForEach(cancellationCoverages, (coverage) =>
                            {
                                coverage.InsuredObject = coverages.First(u => u.Id == coverage.Id).InsuredObject;
                                coverage.InsuredObject.Amount = insuredObjects.First(u => u.Id == coverage.InsuredObject.Id).Amount;
                                coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                                coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                            });

                            liability.CompanyRisk.CompanyCoverages = DelegateService.underwritingService.CreateCompanyCoverages(cancellationCoverages);

                            pendingOperationLiability = new PendingOperation();
                            pendingOperationLiability.ParentId = companyPolicy.Id;
                            pendingOperationLiability.Operation = JsonConvert.SerializeObject(liability);
                            pendingOperationLiability = DelegateService.commonService.CreatePendingOperation(pendingOperationLiability);
                            liability.Id = pendingOperationLiability.Id;
                            liability.CompanyPolicy = companyPolicy;

                        }

                        companyPolicy.PayerComponents = DelegateService.underwritingService.CompanyCalculatePayerComponents(companyPolicy, risks, true);
                        Policy policy = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policy, risks);
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policy);

                        pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                        if (companyPolicy.Id != 0)
                        {
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            pendingOperation = DelegateService.commonService.CreatePendingOperation(pendingOperation);
                            companyPolicy.Id = pendingOperation.Id;
                        }
                        massiveCancellationRow.Risk = new Risk { Id = companyPolicy.Id, Description = companyLiabilityRisks[0].Description, Policy = new Policy { Id = companyPolicy.Id, DocumentNumber = companyPolicy.DocumentNumber, Summary = new Summary { FullPremium = companyPolicy.Summary.FullPremium }, PolicyType = new PolicyType { Id = companyPolicy.PolicyType.Id, Description = companyPolicy.PolicyType.Description } } };
                        massiveCancellationRow.Observations = companyPolicy.Summary.Premium.ToString();
                        massiveCancellationRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                    }
                    else
                    {
                        massiveCancellationRow.HasError = true;
                        massiveCancellationRow.Observations = Errors.ErrorLiabilityNotExist;
                    }
                }
                catch (System.Exception ex)
                {
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = ex.Message;
                }
                finally
                {
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                    DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                }

            }
            else
            {
                massiveCancellationRow.HasError = true;
                massiveCancellationRow.Observations = Errors.ErrorRiskNotExist;
                massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
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
                    processStatus = MassiveLoadProcessStatus.Tariff;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = MassiveLoadProcessStatus.Finalized;
                    break;
            }
            DelegateService.massiveService.LoadReportCacheList();
            LoadList();
            List<LineBusiness> lineBusiness = DelegateService.commonService.GetLineBusinessBySubCoveredRiskType(SubCoveredRiskType.Liability);
            List<MassiveCancellationRow> massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(MassiveLoadId, SubCoveredRiskType.Liability, processStatus, false, null);
            if (massiveCancellationRows == null || !massiveCancellationRows.Any())
            {
                return null;
            }
            MassiveEmission massiveEmission = new MassiveEmission();
            massiveEmission.Prefix = new Prefix { Id = lineBusiness.FirstOrDefault().Id };
            List<Row> rows = new List<Row>();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = (int)massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)SubCoveredRiskType.Liability;
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
                        FillLiabilityFields(massiveEmission, process, serialize);

                        if (concurrentRows.Count >= bulkExcel || massiveCancellationRows.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte_RC_Ubicacion_" + key + "_" + massiveEmission.Id;
                            filePath = DelegateService.commonService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();

                        }
                    });
                
                return filePath;
            }
        }

        public void FillLiabilityFields(MassiveEmission massiveEmission, MassiveCancellationRow massiveCancellationRow, string serializeFields)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyByMassiveLoadStatus(massiveEmission.Status.Value, massiveCancellationRow.Risk.Policy);
                if (companyPolicy != null)
                {
                    List<CompanyLiabilityRisk> risks = GetCompanyLiabilityRisk(massiveEmission, massiveCancellationRow, companyPolicy.Id);

                    List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);

                    foreach (CompanyLiabilityRisk liability in risks)
                    {

                        fields = DelegateService.massiveService.FillInsuredFields(fields, liability.CompanyRisk.CompanyInsured);

                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = massiveCancellationRow.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = liability.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = liability.RiskId.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskAddress).Value = CreateAddress(liability);
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(liability.Beneficiaries);


                        fields.Find(u => u.PropertyName == FieldPropertyName.Observations).Value = "";

                        //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = "";
                        //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = "";

                        //Asistencia
                        List<int> assistanceCoveragesIds = DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceLiability);
                        List<CompanyCoverage> coveragesAssistance = liability.CompanyRisk.CompanyCoverages.Where(u => assistanceCoveragesIds.Exists(id => id == u.Id)).ToList();
                        decimal assistancePremium = coveragesAssistance.Sum(x => x.PremiumAmount);
                        if (assistancePremium != 0)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value = (companyPolicy.Summary.Expenses + assistancePremium).ToString();
                        }

                        serializeFields = JsonConvert.SerializeObject(fields);

                        foreach (CompanyCoverage coverage in liability.CompanyRisk.CompanyCoverages)
                        {
                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
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
                            // fieldsC.Find(u => u.PropertyName == FieldPropertyName.InsuredObjectDescription).Value = insuredObjects.DefaultIfEmpty(new InsuredObject { Description = "" }).FirstOrDefault(u => u.Id == coverage.CompanyInsuredObject.Id).Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.DeclaredAmount.ToString();
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
        private List<CompanyLiabilityRisk> GetCompanyLiabilityRisk(MassiveEmission massiveEmission, MassiveCancellationRow proccess, int tempId)
        {
            List<CompanyLiabilityRisk> companyProperties = new List<CompanyLiabilityRisk>();
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(tempId);
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyProperties.Add(JsonConvert.DeserializeObject<CompanyLiabilityRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    //companyProperties = DelegateService.liabilityService.GetCompanyLiabilityByPrefixBranchDocumentNumberEndorsementType(massiveEmission.Prefix.Id, massiveEmission.Branch.Id, proccess.Risk.Policy.DocumentNumber, EndorsementType.Cancellation);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyLiabilityRisk>(x)));
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyLiabilityRisk>(x)));
                    }
                    break;
            }
            return companyProperties;

        }

        private string CreateAddress(CompanyLiabilityRisk LiabilityRisk)
        {
            string address = "";
            address += LiabilityRisk.FullAddress;

            if (LiabilityRisk.City != null && LiabilityRisk.City.State != null)
            {
                address += StringHelper.ConcatenateString(
                    " | ", states.DefaultIfEmpty(new State { Description = "" }).FirstOrDefault(u => u.Id == LiabilityRisk.City.State.Id).Description,
                    " | ", cities.DefaultIfEmpty(new City { Description = "" }).FirstOrDefault(u => u.Id == LiabilityRisk.City.Id).Description);
            }

            return address;

        }
        private void LoadList()
        {
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            riskUse = DelegateService.liabilityService.GetRiskUses();
            constructionTypes = DelegateService.liabilityService.GetConstructionTypes();
            lineBusiness = DelegateService.commonService.GetLinesBusiness();
            subLineBusiness = DelegateService.commonService.GetSubLineBusiness();
            insuredObjects = DelegateService.underwritingService.GetInsuredObjects();
            cities = DelegateService.commonService.GetCities();
            states = DelegateService.commonService.GetStates();
        }
    }
}
