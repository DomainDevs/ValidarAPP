using AutoMapper;
using Company.Location.LiabilityCollectiveService.EEProvider.Resources;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Location.LiabilityCollectiveService.EEProvider;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Massive.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util = Sistran.Company.Application.Utilities.Constants;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Company.Location.LiabilityCollectiveService.EEProvider.DAOs
{
    public class CollectiveLoadLiabilityDAO
    {
        private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<ConstructionType> constructionTypes = new List<ConstructionType>();
        private static List<LineBusiness> lineBusiness = new List<LineBusiness>();
        private static List<SubLineBusiness> subLineBusiness = new List<SubLineBusiness>();
        private static List<CompanyInsuredObject> insuredObjects = new List<CompanyInsuredObject>();
        private static List<City> cities = new List<City>();
        private static List<State> states = new List<State>();
        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();

        public CollectiveEmission CreateCollectiveLoad(CollectiveEmission collectiveLoad)
        {
            ValidateFile(collectiveLoad);
            collectiveLoad.Status = MassiveLoadStatus.Validating;
            collectiveLoad = DelegateService.collectiveService.CreateCollectiveEmission(collectiveLoad);
            try
            {
                if (collectiveLoad != null)
                {
                    Task.Run(() => ValidateData(collectiveLoad));
                }
            }
            catch (Exception ex)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = StringHelper.ConcatenateString("Error Validar archivo", "|", ex.Message);
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
            return collectiveLoad;
        }

        private void ValidateData(CollectiveEmission collectiveEmission)
        {
            try
            {
                var file = collectiveEmission.File;
                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                file = DelegateService.commonService.ValidateDataFile(file, collectiveEmission.User.AccountName);
                Template emissionTemplate = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmisionAutos);
                file.Templates.Remove(emissionTemplate);
                collectiveEmission.TotalRows = file.Templates.First(p => p.IsPrincipal).Rows.Count;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                List<File> files = DelegateService.commonService.GetDataTemplates(file.Templates);
                Row emissionRow = emissionTemplate.Rows.First();
                int productId = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                int branchId = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                collectiveEmission.Branch.Id = branchId;
                collectiveEmission.Product.Id = productId;
                CollectiveLoadLiabilityValidationDAO collectiveLoadPropertyValidationDAO = new CollectiveLoadLiabilityValidationDAO();
                List<Validation> validations = collectiveLoadPropertyValidationDAO.GetValidationsByFiles(files, collectiveEmission);
                if (validations.Count > 0)
                {
                    int policyValidationsId = files.Count + 1;
                    var policyValidations = validations.Where(x => x.Id == policyValidationsId).ToList();
                    string policyValidationsMessage = "";
                    if (policyValidations.Any())
                    {
                        policyValidationsMessage = string.Join(",", policyValidations.Select(x => x.ErrorMessage)) + "|";
                    }
                    validations.RemoveAll(x => x.Id == policyValidationsId);
                    foreach (File validatedFile in files)
                    {
                        List<Validation> fileValidations = validations.Where(x => x.Id == validatedFile.Id).ToList();
                        string riskErrors = "";
                        if (fileValidations.Any())
                        {
                            riskErrors = string.Join(",", fileValidations.Select(x => x.ErrorMessage));
                        }
                        string error = policyValidationsMessage + riskErrors;
                        if (!string.IsNullOrEmpty(error))
                        {
                            validatedFile.Templates[0].Rows[0].HasError = true;
                            validatedFile.Templates[0].Rows[0].ErrorDescription = error;
                        }
                    }
                }
                CreateModels(collectiveEmission, files, emissionTemplate);

                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmission.Id);
            }
            catch (Exception ex)
            {
                collectiveEmission.Status = MassiveLoadStatus.Validated;
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = StringHelper.ConcatenateString("Error validando archivo|", ex.ToString());
                DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void ValidateFile(CollectiveEmission collectiveLoad)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveEmission,
                Key2 = (int)EndorsementType.Emission,
                Key4 = collectiveLoad.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Liability
            };

            string fileName = collectiveLoad.File.Name;
            collectiveLoad.File = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (collectiveLoad.File != null)
            {
                collectiveLoad.File.Name = fileName;
                collectiveLoad.File = DelegateService.commonService.ValidateFile(collectiveLoad.File, collectiveLoad.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void CreateModels(CollectiveEmission collectiveEmission, List<File> files, Template emissionTemplate)
        {
            //PendingOperation pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmission.TemporalId);
            //CompanyPolicy policy = new CompanyPolicy();
            ////User user = new User();

            ////if (collectiveEmission.IsAutomatic == true)
            ////{
            ////    user = DelegateService.uniqueUserService.GetUserById(collectiveEmission.User.UserId);
            ////}
            ////else
            ////{
            ////    policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
            ////    user = DelegateService.uniqueUserService.GetUserById(policy.UserId);
            ////}
            //List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetFilterIndividuals(collectiveEmission.User.UserId, collectiveEmission.Branch.Id, files, TemplatePropertyName.EmisionAutos);
            //Template riskDetail = files[0].Templates.First(t => t.PropertyName == TemplatePropertyName.RiskDetail);
            //collectiveEmission.Branch.Id = (int)DelegateService.commonService.GetValueByField<int>(files[0].Templates.First(t => t.PropertyName == TemplatePropertyName.EmisionAutos).Rows[0].Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            //collectiveEmission.Agency = DelegateService.uniquePersonService.GetAgencyByAgentCodeAgentTypeCode((int)DelegateService.commonService.GetValueByField<int>(files[0].Templates.First(t => t.PropertyName == TemplatePropertyName.EmisionAutos).Rows[0].Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode)),
            //    (int)DelegateService.commonService.GetValueByField<int>(files[0].Templates.First(t => t.PropertyName == TemplatePropertyName.EmisionAutos).Rows[0].Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType)));
            //int productId = (int)DelegateService.commonService.GetValueByField<int>(files[0].Templates.First(t => t.PropertyName == TemplatePropertyName.EmisionAutos).Rows[0].Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
            //collectiveEmission.Product.Id = productId;
            //if (collectiveEmission.IsAutomatic == true)
            //{
            //    policy = DelegateService.collectiveService.CreateCompanyPolicy(collectiveEmission, files[0], TemplatePropertyName.EmisionAutos, filtersIndividuals, riskDetail.Rows.Count);
            //    collectiveEmission.TemporalId = policy.Id;
            //    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            //}
            List<FilterIndividual> filterIndividuals = new List<FilterIndividual>();
            CompanyPolicy policy = new CompanyPolicy();
            Row emissionRow = emissionTemplate.Rows.First();
            if (!emissionRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
            {
                files.ForEach(x => x.Templates.Add(emissionTemplate));
                filterIndividuals = DelegateService.massiveService.GetFilterIndividuals(collectiveEmission.User.UserId, collectiveEmission.Branch.Id, files, TemplatePropertyName.EmisionAutos);
                if (collectiveEmission.IsAutomatic)
                {
                    int agentCode = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    int agentType = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    collectiveEmission.Agency = DelegateService.uniquePersonService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentType);
                    var emissionFile = new File
                    {
                        Templates = new List<Template> { emissionTemplate }
                    };
                    policy = DelegateService.collectiveService.CreateCompanyPolicy(collectiveEmission, emissionFile, TemplatePropertyName.EmisionAutos, filterIndividuals, files.Count);
                    collectiveEmission.TemporalId = policy.Id;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
                else
                {
                    PendingOperation pendingOperation;
                    if (Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }
                    else
                    {
                        pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }
                    policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                }
            }
            policy.Summary = new Summary
            {
                RiskCount = files.Count
            };

            TP.Parallel.ForEach(files, file =>
            {
                CreateModel(collectiveEmission, file, filterIndividuals, policy);
            });
        }
        private void CreateModel(CollectiveEmission colleciveLoad, File file, List<FilterIndividual> filtersIndividuals, CompanyPolicy policy)
        {
            string templateName = "";
            string propertyName = "";
            CollectiveEmissionRow collectiveLoadProcess = new CollectiveEmissionRow();
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow
            {
                Risk = new Risk()
            };
            try
            {
                //Template riskDetail = file.Templates.First(t => t.PropertyName == TemplatePropertyName.RiskDetail);
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription))).ToList();
                collectiveLoadProcess.MassiveLoadId = colleciveLoad.Id;
                collectiveLoadProcess.RowNumber = file.Id;
                collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Validation;
                collectiveLoadProcess.HasError = hasError;
                collectiveLoadProcess.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveLoadProcess.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveLoadProcess);
                //ParallelHelper.ForEach(riskDetail.Rows, risk =>
                //{
                if (!hasError)
                {
                    //templateName = TemplatePropertyName.EmissionProperty;
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();
                    CompanyLiabilityRisk companyLiability = new CompanyLiabilityRisk
                    {
                        Status = RiskStatusType.Original,
                        CoveredRiskType = CoveredRiskType.Location,
                        GroupCoverage = DelegateService.collectiveService.CreateGroupCoverage(row, policy.CompanyProduct.Id),
                        Beneficiaries = new List<Beneficiary>(),
                        CompanyRisk = new CompanyRisk()
                        {
                            CompanyInsured = DelegateService.massiveService.CreateInsured(row, policy.Holder, filtersIndividuals)
                        }
                    };
                    companyLiability.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyLiability.CompanyRisk.CompanyInsured, filtersIndividuals));
                    propertyName = FieldPropertyName.RiskAddress;
                    companyLiability.FullAddress = (string)(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskAddress).Value);
                    companyLiability.NomenclatureAddress = new Sistran.Core.Application.Locations.Models.NomenclatureAddress
                    {
                        Type = new Sistran.Core.Application.Locations.Models.RouteType
                        {
                            Id = 1
                        },
                    };
                    companyLiability.City = new City()
                    {
                        Id = 1
                    };
                    companyLiability.City.State = new State()
                    {
                        Id = 1
                    };
                    companyLiability.City.State.Country = new Country()
                    {
                        Id = 1
                    };
                    propertyName = FieldPropertyName.RiskActivity;
                    int riskActivityId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskActivity));
                    if (riskActivityId > 0)
                    {
                        companyLiability.RiskActivity = DelegateService.underwritingService.GetRiskActivityTypeByActivityId(riskActivityId).First();
                    }
                    companyLiability.CoveredRiskType = CoveredRiskType.Location;
                    propertyName = FieldPropertyName.RiskIsFacultative;
                    companyLiability.IsDeclarative = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsFacultative));
                    companyLiability.CompanyRisk.CompanyCoverages = DelegateService.collectiveService.CreateCoverages(policy.CompanyProduct.Id, companyLiability.GroupCoverage.Id, policy.Prefix.Id);
                    companyLiability.CompanyRisk.CompanyCoverages = companyLiability.CompanyRisk.CompanyCoverages.Where(x => x.IsSelected == true).ToList();
                    companyLiability.CompanyRisk.CompanyCoverages.ForEach(x =>
                    {
                        x.EndorsementType = policy.Endorsement.EndorsementType;
                        x.CurrentFrom = policy.CurrentFrom;
                        x.CurrentTo = policy.CurrentTo;
                        int compareFromResult = DateTime.Compare(policy.CurrentFrom, policy.CurrentTo);
                        if (compareFromResult >= 0)
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations = Errors.ErrorToDatePolicy + KeySettings.ReportErrorSeparatorMessage();
                        }
                    });
                    propertyName = Util.CompanyFieldPropertyName.RiskIncCoberAsist;
                    bool incCoverAsist = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == Util.CompanyFieldPropertyName.RiskIncCoberAsist));
                    if (incCoverAsist)
                    {
                        propertyName = Util.CompanyFieldPropertyName.RiskAsistType;
                        companyLiability.CompanyRisk.CompanyAssistanceType = new CompanyAssistanceType
                        {
                            Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == Util.CompanyFieldPropertyName.RiskAsistType))

                        };
                    }

                    propertyName = Util.CompanyFieldPropertyName.RiskInspectionRecomendation;
                    companyLiability.CompanyRisk.Inspection = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == Util.CompanyFieldPropertyName.RiskInspectionRecomendation));
                    propertyName = FieldPropertyName.RiskText;
                    companyLiability.Text = new Text
                    {
                        TextBody = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText))
                    };
                    //Plantillas Adicionales
                    propertyName = "";
                    templateName = TemplatePropertyName.InsuredProperty;
                    companyLiability.CompanyRisk.CompanyCoverages = CreateInsuredObject(companyLiability.CompanyRisk.CompanyCoverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.InsuredObjects));
                    templateName = TemplatePropertyName.AdditionalBeneficiaries;
                    List<Beneficiary> beneficiaries = DelegateService.massiveService.CreateAdditionalBeneficiaries(file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries), filtersIndividuals);
                    decimal beneficiariesParticipation = beneficiaries.Sum(x => x.Participation);
                    if (beneficiariesParticipation < 100)
                    {
                        companyLiability.Beneficiaries[0].Participation -= beneficiariesParticipation;
                        companyLiability.Beneficiaries.AddRange(beneficiaries);
                    }
                    else
                    {
                        throw new ValidationException("ErrorParticipationBeneficiary");
                    }
                    PendingOperation pendingOperation = new PendingOperation
                    {
                        ParentId = policy.Id,
                        UserId = policy.UserId,
                        Operation = JsonConvert.SerializeObject(companyLiability)
                    };
                    if (!Settings.UseReplicatedDatabase())
                    {
                        DelegateService.commonService.CreatePendingOperation(pendingOperation);
                        if (collectiveLoadProcess.Risk == null)
                        {
                            collectiveLoadProcess.Risk = new Risk();
                        }
                        collectiveLoadProcess.Risk.RiskId = pendingOperation.Id;
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                    }
                    else
                    {
                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", JsonConvert.SerializeObject(pendingOperation), (char)007, JsonConvert.SerializeObject(collectiveLoadProcess), (char)007, nameof(CollectiveEmissionRow));
                        var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePendingOperationQuee", routingKey: "CreatePendingOperationQuee", serialization: "JSON");
                        queue.PutOnQueue(pendingOperationJson);
                    }
                }

                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                //});
            }
            catch (Exception ex)
            {
                collectiveLoadProcess.HasError = true;
                collectiveLoadProcess.Observations = StringHelper.ConcatenateString(file.Id.ToString(), "-", templateName, "-", propertyName, "-", ex.ToString());
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
            }
        }

        /// <summary>
        /// Crear Bienes Asegurados
        /// </summary>
        /// <param name="coverages">Plantilla Bienes Asegurados</param>
        /// <param name="template"></param>
        /// <returns>Coberturas</returns>
        private List<CompanyCoverage> CreateInsuredObject(List<CompanyCoverage> coverages, Template template)
        {
            if (template != null)
            {
                foreach (Row row in template.Rows)
                {
                    int rowNumber = 1;
                    string propertyName = "";
                    try
                    {
                        propertyName = FieldPropertyName.DeductibleCodeCoverage;
                        int coverageId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));

                        propertyName = FieldPropertyName.InsuredObjectPropertyCode;
                        int insuredId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectPropertyCode));

                        propertyName = FieldPropertyName.InsuredObjectSumAssured;
                        int amount = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));

                        if (insuredId > 0 && amount > 0)
                        {
                            foreach (CompanyCoverage coverage in coverages.Where(u => u.Id == coverageId))
                            {
                                coverage.LimitAmount = amount;
                                coverage.LimitOccurrenceAmount = amount;
                                coverage.LimitClaimantAmount = amount;
                                coverage.DeclaredAmount = amount;
                                coverage.SubLimitAmount = amount;
                                coverage.CompanyInsuredObject.Amount = amount;
                                rowNumber++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(StringHelper.ConcatenateString(rowNumber.ToString(), "-", propertyName, "-", ex.ToString()));
                    }
                }
            }
            return coverages;
        }

        #region Reportes
        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <returns>string</returns>
        public string GenerateReportToCollectiveLoad(CollectiveEmission collectiveEmission, int endorsementType)
        {
            CollectiveLoadProcessStatus processStatus = CollectiveLoadProcessStatus.Validation;
            switch (collectiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = CollectiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = CollectiveLoadProcessStatus.Finalized;
                    break;
            }

            DelegateService.massiveService.LoadReportCacheList();
            LoadList(collectiveEmission);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            List<CollectiveEmissionRow> collectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadIdCollectiveLoadProcessStatus(collectiveEmission.Id, processStatus, false, null);

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = collectiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;
            File file = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (file == null && !collectiveEmissionRows.Any())
            {
                return "";
            }

            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            string key = Guid.NewGuid().ToString();
            string filePath = "";            
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

            file.FileType = FileType.CSV;
            TP.Parallel.ForEach(collectiveEmissionRows,
                    (process) =>
                    {
                        FillPropertyFields(collectiveEmission, process, serializeFields);
                        if (concurrentRows.Count >= bulkExcel || collectiveEmissionRows.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList();
                            file.Name = "Reporte Colectiva Hogar_" + key + "_" + collectiveEmission.Id;
                            filePath = DelegateService.commonService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();
                        }
                    });         
            return filePath;

        }

        private void FillPropertyFields(CollectiveEmission collectiveEmission, CollectiveEmissionRow proccess, string serializeFields)
        {
            try
            {
                Policy policy = new Policy();
                policy.Id = collectiveEmission.TemporalId;
                policy.Endorsement = new Endorsement { EndorsementType = EndorsementType.Emission, Id = collectiveEmission.EndorsementId ?? 0 };
                policy.DocumentNumber = collectiveEmission.DocumentNumber != null ? collectiveEmission.DocumentNumber.GetValueOrDefault() : 0;

                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(collectiveEmission.Status.Value, policy);
                if (companyPolicy != null)
                {
                    List<Field> fields = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                    List<CompanyLiabilityRisk> companyLiabilityRisks = GetCompanyLiabilityRisk(collectiveEmission, companyPolicy.Id);

                    foreach (CompanyLiabilityRisk liability in companyLiabilityRisks)
                    {
                        fields = CreateInsured(fields, liability);

                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = proccess.Id.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = collectiveEmission.Id.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyBranchCode).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyBranchDescription).Value = collectiveEmission.Branch.Id.ToString(); 
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPrefixCode).Value = collectiveEmission.Prefix.Id.ToString(); 
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyTemporal).Value = collectiveEmission.Branch.Id.ToString(); 
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyType).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyBusinessTypeDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.BillingGroup).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.RequestGroup).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyIssueDate).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyEndorsementTypeDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyNumber).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrentFrom).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrentTo).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyNumberDays).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExchangeRate).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyProductDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.HolderDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.HolderDocument).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.HolderAddressDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.HolderPhoneDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDocument).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.InsuredAddressDescription).Value = collectiveEmission.Branch.Id.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.InsuredPhoneDescription).Value = collectiveEmission.Branch.Id.ToString();

                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = liability.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = liability.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskAddress).Value = CreateAddress(liability);

                        //Asistencia
                        List<int> assistanceCoveragesIds = DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceProperty);
                        List<CompanyCoverage> coveragesAssistance = liability.CompanyRisk.CompanyCoverages.Where(u => assistanceCoveragesIds.Exists(id => id == u.Id)).ToList();
                        decimal assistancePremium = coveragesAssistance.Sum(x => x.PremiumAmount);
                        if (assistancePremium > 0)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value = (companyPolicy.Summary.Expenses + assistancePremium).ToString();
                        }

                        serializeFields = JsonConvert.SerializeObject(fields);
                        decimal valueRc = 0, insuredValue = 0;
                        //valueRc = liability.CompanyRisk.CompanyCoverages.Where(u => u.Description != null && u.Description.Contains("R.C.E")).Sum(u => u.LimitAmount);
                        //insuredValue = (companyPolicy.Summary.AmountInsured - valueRc);

                        foreach (CompanyCoverage coverage in liability.CompanyRisk.CompanyCoverages)
                        {

                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                            //fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = insuredValue.ToString();
                            //fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = valueRc.ToString();
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
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.InsuredObjectDescription).Value = insuredObjects.DefaultIfEmpty(new CompanyInsuredObject { Description = "" }).FirstOrDefault(u => u.Id == coverage.CompanyInsuredObject.Id).Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.DeclaredAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                            fields.Find(u => u.PropertyName == FieldPropertyName.Observations).Value = "";

                            //fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.DayPaymentFirstQuota).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.MethodOfPaymentDescription).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyVAT).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyTotalToPay).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.IntermediaryData).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.CoinsuranceData).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyUserName).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.PolicyAllianceDescription).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.PolicyAllianceBranchDescription).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.PolicyAllianceSalePointDescription).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.PolicyInformedPremiumValue).Value = liability.Number.ToString();
                            //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.PolicyExternalPolicyNumber).Value = liability.Number.ToString();

                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(coverage.Clauses);
                            concurrentRows.Add(new Row
                            {
                                Fields = fieldsC
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }
        private List<CompanyLiabilityRisk> GetCompanyLiabilityRisk(CollectiveEmission collectiveEmission, int tempId)
        {
            List<CompanyLiabilityRisk> companyLiabilities = new List<CompanyLiabilityRisk>();
            int endorsementId = collectiveEmission.EndorsementId ?? 0;
            switch (collectiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(tempId);
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyLiabilities.Add(JsonConvert.DeserializeObject<CompanyLiabilityRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyLiabilities.Add(JsonConvert.DeserializeObject<CompanyLiabilityRisk>(x)));
                        /* without Replicated Database */
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyLiabilities.Add(JsonConvert.DeserializeObject<CompanyLiabilityRisk>(x)));
                    }
                    break;
            }
            return companyLiabilities;

        }
        private void LoadList(CollectiveEmission massiveEmission)
        {
            documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            lineBusiness = DelegateService.commonService.GetLinesBusiness();
            subLineBusiness = DelegateService.commonService.GetSubLineBusiness();
            cities = DelegateService.commonService.GetCities();
            states = DelegateService.commonService.GetStates();
            insuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjects();

        }

        private List<Field> CreateInsured(List<Field> fields, CompanyLiabilityRisk liability)
        {
            CompanyInsured mainInsured = liability.CompanyRisk?.CompanyInsured;
            CompanyName companyName = mainInsured?.CompanyName;
            if (mainInsured != null)
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDocument).Value = documentTypes.DefaultIfEmpty(new DocumentType { SmallDescription = "CC" }).First(u => u.Id == mainInsured.IdentificationDocument.DocumentType.Id).SmallDescription + ". " + mainInsured.IdentificationDocument.Number;
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDescription).Value = mainInsured.Name;
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredPhoneDescription).Value = (companyName?.Phone != null) ? companyName.Phone.Description : "";
            }
            string address = "";
            if (companyName != null)
            {
                address = StringHelper.ConcatenateString(companyName.Address?.Description ?? "", "|", companyName.Address?.City?.State?.Description ?? "", "|", companyName.Address?.City?.Description ?? "");
            }
            fields.Find(u => u.PropertyName == FieldPropertyName.InsuredAddressDescription).Value = address;
            return fields;

        }

        private string CreateAddress(CompanyLiabilityRisk liabilityRisk)
        {
            string address = "";
            address += liabilityRisk.FullAddress;

            if (liabilityRisk.City != null && liabilityRisk.City.State != null)
            {
                address += StringHelper.ConcatenateString(" | ", states.DefaultIfEmpty(new State { Description = "" }).FirstOrDefault(u => u.Id == liabilityRisk.City.State.Id).Description,
                    " | ", cities.DefaultIfEmpty(new City { Description = "" }).FirstOrDefault(u => u.Id == liabilityRisk.City.Id).Description);
            }

            return address;

        }
        #endregion
    }
}