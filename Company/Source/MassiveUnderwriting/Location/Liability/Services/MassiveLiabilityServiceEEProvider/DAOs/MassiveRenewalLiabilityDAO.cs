using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.DAOs
{
    public class MassiveRenewalLiabilityDAO
    {
        public MassiveRenewal CreateMassiveLoad(MassiveRenewal massiveRenewal)
        {
            ValidateFile(massiveRenewal);
            massiveRenewal.Status = MassiveLoadStatus.Validating;
            massiveRenewal = DelegateService.massiveRenewalService.CreateMassiveRenewal(massiveRenewal);
            try
            {
                if (massiveRenewal != null)
                {
                    TP.Task.Run(() => ValidateData(massiveRenewal));
                }
            }
            catch (Exception ex)
            {
                massiveRenewal.HasError = true;
                massiveRenewal.ErrorDescription = StringHelper.ConcatenateString(Errors.ErrorValidatingFile, "|", ex.ToString());
                DelegateService.massiveRenewalService.UpdateMassiveRenewal(massiveRenewal);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
            return massiveRenewal;
        }

        private void ValidateFile(MassiveRenewal massiveRenewal)
        {
            FileProcessValue fileProcessValue = new FileProcessValue
            {
                Key1 = (int)FileProcessType.MassiveRenewal,
                Key2 = (int)EndorsementType.Renewal,
                Key4 = massiveRenewal.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Liability
            };
            string fileName = massiveRenewal.File.Name;
            massiveRenewal.File = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (massiveRenewal.File != null)
            {
                massiveRenewal.File.Name = fileName;
                massiveRenewal.File = DelegateService.commonService.ValidateFile(massiveRenewal.File, massiveRenewal.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void ValidateData(MassiveRenewal massiveRenewal)
        {
            try
            {
                massiveRenewal.File = DelegateService.commonService.ValidateDataFile(massiveRenewal.File, massiveRenewal.User.AccountName);
                massiveRenewal.TotalRows = massiveRenewal.File.Templates.First(p => p.IsPrincipal).Rows.Count;
                DelegateService.massiveRenewalService.UpdateMassiveRenewal(massiveRenewal);
                List<File> files = DelegateService.commonService.GetDataTemplates(massiveRenewal.File.Templates);
                MassiveRenewalLiabilityValidationDAO massiveLiabilityValidationDAO = new MassiveRenewalLiabilityValidationDAO();
                List<Validation> validations = massiveLiabilityValidationDAO.GetValidationsByFiles(files, massiveRenewal);
                if (validations.Count > 0)
                {
                    Validation validation;
                    foreach (File file in files)
                    {
                        validation = validations.Find(p => p.Id == file.Id);
                        if (validation != null)
                        {
                            file.Templates[0].Rows[0].HasError = true;
                            file.Templates[0].Rows[0].ErrorDescription = validation.ErrorMessage;
                        }
                    }
                }
                CreateModels(massiveRenewal, files);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveRenewal.Id);
            }
            catch (Exception ex)
            {
                massiveRenewal.Status = MassiveLoadStatus.Validated;
                massiveRenewal.HasError = true;
                massiveRenewal.ErrorDescription = StringHelper.ConcatenateString(Errors.ErrorValidatingFile, "|", ex.ToString());
                DelegateService.massiveRenewalService.UpdateMassiveRenewal(massiveRenewal);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModels(MassiveRenewal massiveLoad, List<File> files)
        {
            ParallelHelper.ForEach(files, file =>
            {
                CreateModel(massiveLoad, file);
            });
        }

        private void CreateModel(MassiveRenewal massiveRenewal, File file)
        {
            MassiveRenewalRow massiveRenewalRow = new MassiveRenewalRow();
            try
            {
                bool hasError = file.Templates.Any(p => p.Rows.Any(x => x.HasError));
                List<string> errorList = file.Templates.Select(p => string.Join(",", p.Rows.Where(x => !string.IsNullOrEmpty(x.ErrorDescription)).Select(y => y.ErrorDescription))).ToList();
                massiveRenewalRow.MassiveRenewalId = massiveRenewal.Id;
                massiveRenewalRow.RowNumber = file.Id;
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Validation;
                massiveRenewalRow.HasError = hasError;
                massiveRenewalRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(p => !string.IsNullOrEmpty(p)));
                massiveRenewalRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.massiveRenewalService.CreateMassiveRenewalRow(massiveRenewalRow);
                if (!hasError)
                {
                    Template principalTemplate = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Renewal);
                    Row principalRow = principalTemplate != null ? principalTemplate.Rows.First() : null;
                    if (principalRow == null)
                    {
                        throw new ValidationException(string.Format(Errors.ErrorPrincipalRowNotFound, TemplatePropertyName.Renewal));
                    }
                    CompanyPolicy companyPolicy = DelegateService.massiveRenewalService.CreateCompanyPolicy(file, TemplatePropertyName.Renewal, massiveRenewal.User.UserId, massiveRenewal.Prefix.Id);
                    List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiabilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    List<Beneficiary> beneficiaries = companyLiabilityRisks.SelectMany(x => x.Beneficiaries).ToList();
                    foreach (Beneficiary beneficiary in beneficiaries.GroupBy(x => x.IndividualId).Select(x => x.First()))
                    {
                        IdentificationDocument identificationDocument = DelegateService.uniquePersonService.GetIdentificationDocumentByIndividualIdCustomerType(beneficiary.IndividualId, (int)CustomerType.Individual);
                        beneficiaries.ForEach(x => x.IdentificationDocument = identificationDocument);
                    }
                    Mapper.CreateMap<Coverage, CompanyCoverage>();
                    Mapper.CreateMap<InsuredObject, CompanyInsuredObject>();
                    Mapper.CreateMap<Insured, CompanyInsured>();
                    foreach (CompanyLiabilityRisk liabilityRisk in companyLiabilityRisks)
                    {
                        liabilityRisk.Status = RiskStatusType.Original;
                        List<CompanyInsuredObject> companyInsuredObjects = new List<CompanyInsuredObject>();
                        liabilityRisk.CompanyRisk.CompanyInsured = Mapper.Map<Insured, CompanyInsured>(liabilityRisk.CompanyRisk.CompanyInsured);
                        companyInsuredObjects = liabilityRisk.CompanyRisk.CompanyCoverages.GroupBy(y => y.CompanyInsuredObject.Id, x => x.CompanyInsuredObject).Select(x => x.First()).ToList();
                        List<CompanyInsuredObject> companyInsuredObjectsFromTemplate = GetInsuredObjectFromTemplate(file.Templates.Find(x => x.PropertyName == TemplatePropertyName.InsuredObjects));
                        bool modifiedInsuredObjects = companyInsuredObjectsFromTemplate.Any();
                        if (modifiedInsuredObjects)
                        {
                            foreach (var insuredObject in companyInsuredObjects)
                            {
                                CompanyInsuredObject companyInsuredObject;
                                if ((companyInsuredObject = companyInsuredObjectsFromTemplate.Find(y => y.Id == insuredObject.Id)) != null)
                                {
                                    insuredObject.PercentageVariableIndex = companyInsuredObject.PercentageVariableIndex;
                                    insuredObject.RecoupmentPeriod = companyInsuredObject.RecoupmentPeriod;
                                    insuredObject.RateTRI = companyInsuredObject.RateTRI;
                                    insuredObject.Amount = companyInsuredObject.Amount;
                                    companyInsuredObjectsFromTemplate.Remove(companyInsuredObject);
                                }
                            }
                            if (companyInsuredObjectsFromTemplate.Any())
                            {
                                foreach (CompanyInsuredObject insuredObjectAdded in companyInsuredObjectsFromTemplate)
                                {
                                    // este llamado es suceptible de mejorar enviando el parámetro de filtro inicialmente incluidas
                                    List<Coverage> coverages = DelegateService.underwritingService.GetCoveragesByInsuredObjectId(insuredObjectAdded.Id);
                                    coverages = coverages.Where(x => x.IsSelected).ToList();
                                    List<CompanyCoverage> coveragesToAdd = Mapper.Map<List<Coverage>, List<CompanyCoverage>>(coverages);
                                    coveragesToAdd.ForEach(x =>
                                    {
                                        x.CompanyInsuredObject = insuredObjectAdded;
                                    });
                                    liabilityRisk.CompanyRisk.CompanyCoverages.AddRange(coveragesToAdd);
                                    companyInsuredObjects.Add(insuredObjectAdded);
                                }
                            }
                        }
                        var principalRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(principalRow.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                        if (principalRateTri > 0)
                        {
                            companyInsuredObjects.ForEach(x => x.RateTRI = principalRateTri);
                        }
                        if (liabilityRisk.CompanyRisk != null)
                        {
                            foreach (CompanyCoverage x in liabilityRisk.CompanyRisk.CompanyCoverages)
                            {
                                x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                x.CurrentFrom = companyPolicy.CurrentFrom;
                                x.CurrentTo = companyPolicy.CurrentTo;
                                x.CoverStatus = CoverageStatusType.Original;
                                if (modifiedInsuredObjects)
                                {
                                    var a = companyInsuredObjects.Find(y => y.Id == x.CompanyInsuredObject.Id);
                                    x.CompanyInsuredObject = a;
                                }
                            }
                        }
                        PendingOperation pendingOperation = new PendingOperation
                        {
                            ParentId = companyPolicy.Id,
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(liabilityRisk)
                        };
                        massiveRenewalRow.Risk = liabilityRisk;
                        massiveRenewalRow.Risk.Policy = massiveRenewalRow.Risk.Policy ?? companyPolicy;
                        massiveRenewalRow.TemporalId = companyPolicy.Id;
                        massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
                        massiveRenewalRow.Risk.Policy.Branch = companyPolicy.Branch;

                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.commonService.CreatePendingOperation(pendingOperation);// Without Replicated Database
                            DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
                        }
                        else // with Replicated Database
                        {
                            PendingOperation policyPendingOperation = new PendingOperation
                            {
                                Operation = JsonConvert.SerializeObject(companyPolicy),
                                UserId = companyPolicy.UserId
                            };

                            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(policyPendingOperation), (char)007, JsonConvert.SerializeObject(pendingOperation), (char)007, JsonConvert.SerializeObject(massiveRenewalRow), (char)007, nameof(MassiveRenewalRow));
                            var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePendingOperationQuee", routingKey: "CreatePendingOperationQuee", serialization: "JSON");
                            queue.PutOnQueue(pendingOperationJson);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                string error = string.Empty;
                if (ex is ValidationException)
                {
                    error = ex.Message;
                }
                else if (ex is BusinessException)
                {
                    var baseException = ex.GetBaseException();
                    if (baseException is ValidationException || baseException is BusinessException)
                    {
                        error = ex.Message;
                    }
                }
                if (string.IsNullOrEmpty(error))
                {
                    error = StringHelper.ConcatenateString(Errors.ErrorCreatingModels, "|", ex.Message);
                }
                massiveRenewalRow.Observations = error;
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private List<CompanyInsuredObject> GetInsuredObjectFromTemplate(Template insuredObjectTemplate)
        {
            List<CompanyInsuredObject> insuredObjects = new List<CompanyInsuredObject>();
            if (insuredObjectTemplate == null)
            {
                return insuredObjects;
            }
            foreach (Row row in insuredObjectTemplate.Rows)
            {
                int id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                decimal amount = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));
                decimal rateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                decimal percentageVariableIndex = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RiskPercentageVariableIndex));
                int recoupmentPeriodId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId));
                string validationError = ValidateInsuredObject(id, amount);
                if (!string.IsNullOrEmpty(validationError))
                {
                    throw new ValidationException(StringHelper.ConcatenateString(validationError, "|", row.Number.ToString()));
                }
                var insuredObject = new CompanyInsuredObject
                {
                    Id = id,
                    Amount = amount,
                    RateTRI = rateTri,
                    PercentageVariableIndex = percentageVariableIndex
                };
                if (recoupmentPeriodId != 0)
                {
                    insuredObject.RecoupmentPeriod = new RecoupmentPeriod
                    {
                        Id = recoupmentPeriodId
                    };
                }
                insuredObjects.Add(insuredObject);
            }
            return insuredObjects;
        }

        private string ValidateInsuredObject(int insuredObjectId, decimal insuredAmount)
        {
            List<string> validationErrors = new List<string>();
            if (insuredObjectId <= 0)
            {
                validationErrors.Add(Errors.InvalidInsuredObjectId);
            }
            if (insuredAmount <= 0)
            {
                validationErrors.Add(Errors.InvalidInsuredObjectAmountValue);
            }
            if (!validationErrors.Any())
            {
                //DelegateService.underwritingService.ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId()
            }
            return string.Join("|", validationErrors);
        }

        public MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                List<MassiveRenewalRow> massiveRenewalRows = DelegateService.massiveRenewalService.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveLoad.Id, MassiveLoadProcessStatus.Tariff);
                if (massiveRenewalRows.Count > 0)
                {
                    TP.Task.Run(() => IssuanceRenewalMassiveEmissionRows(massiveLoad, massiveRenewalRows));
                }
                else
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
                }
            }
            return massiveLoad;
        }

        public void IssuanceRenewalMassiveEmissionRows(MassiveLoad massiveLoad, List<MassiveRenewalRow> massiveRenewalRows)
        {
            try
            {
                massiveLoad.Status = MassiveLoadStatus.Issuing;
                massiveLoad.TotalRows = massiveRenewalRows.Count + DelegateService.massiveUnderwritingService.GetFinalizedMassiveEmissionRowsByMassiveLoadId(massiveLoad.Id).Count;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                List<CompanyPolicy> companyPolicies = DelegateService.massiveUnderwritingService.GetCompanyPoliciesToIssueByOperationIds(massiveRenewalRows.Where(x => !x.HasError ?? false).Select(x => x.Risk.Policy.Id).ToList());
                ParallelHelper.ForEach(companyPolicies, companyPolicy =>
                {
                    ExecuteCreatePolicy(companyPolicy, massiveRenewalRows.FirstOrDefault(x => x.Risk.Policy.Id == companyPolicy.Id));
                });

                massiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            catch (Exception e)
            {
                massiveLoad.HasError = true;
                massiveLoad.ErrorDescription = e.InnerException.Message;
                massiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
        }

        private void ExecuteCreatePolicy(CompanyPolicy companyPolicy, MassiveRenewalRow massiveRenewalRow)
        {
            try
            {
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Issuance;

                if (massiveRenewalRow.Risk.Policy.Id > 0)
                {
                    massiveRenewalRow.Status = MassiveLoadProcessStatus.Finalized;

                    List<PendingOperation> pendingOperations;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(massiveRenewalRow.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(massiveRenewalRow.Risk.Policy.Id);
                    }
                    List<CompanyLiabilityRisk> companyLiabilityRisks = new List<CompanyLiabilityRisk>();
                    foreach (PendingOperation po in pendingOperations)
                    {
                        var companyLiabilityRisk = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(po.Operation);
                        companyLiabilityRisk.CompanyPolicy = companyPolicy;
                        companyLiabilityRisks.Add(companyLiabilityRisk);
                    }
                    companyPolicy = DelegateService.liabilityService.CreateEndorsement(companyPolicy, companyLiabilityRisks);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
                    }
                    else
                    {
                        /* with Replicated Database */
                        DelegateService.pendingOperationEntityService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
                    }
                    massiveRenewalRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    massiveRenewalRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;


                }
                else
                {
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.Observations = StringHelper.ConcatenateString(Errors.ErrorIssuing, "(", ex.Message, ")");
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
    }
}