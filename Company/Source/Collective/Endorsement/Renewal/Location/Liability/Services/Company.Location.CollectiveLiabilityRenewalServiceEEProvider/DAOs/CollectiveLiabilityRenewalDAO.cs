using Newtonsoft.Json;
using Company.Location.CollectiveLiabilityRenewalService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Configuration;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.CollectiveLiabilityRenewalService.EEProvider.DAOs
{
    public class CollectiveLiabilityRenewalDAO
    {
        public CollectiveEmission CreateMassiveLoad(CollectiveEmission collectiveRenewal)
        {
            ValidateFile(collectiveRenewal);
            collectiveRenewal.Status = MassiveLoadStatus.Validating;
            collectiveRenewal = DelegateService.collectiveService.CreateCollectiveEmission(collectiveRenewal);
            try
            {
                if (collectiveRenewal != null)
                {
                    TP.Task.Run(() => ValidateData(collectiveRenewal));
                }
            }
            catch (Exception ex)
            {
                collectiveRenewal.HasError = true;
                collectiveRenewal.ErrorDescription = StringHelper.ConcatenateString(Errors.ErrorValidatingFile, "|", ex.ToString());
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveRenewal);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
            return collectiveRenewal;
        }

        private void ValidateData(CollectiveEmission collectiveRenewal)
        {
            try
            {
                collectiveRenewal.File = DelegateService.commonService.ValidateDataFile(collectiveRenewal.File, collectiveRenewal.User.AccountName);
                collectiveRenewal.TotalRows = collectiveRenewal.File.Templates.First(p => p.IsPrincipal).Rows.Count();
                collectiveRenewal.File.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).IsPrincipal = false;
                collectiveRenewal.File.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveRenewal);
                List<File> files = DelegateService.commonService.GetDataTemplates(collectiveRenewal.File.Templates);
                collectiveRenewal.File.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).IsPrincipal = true;
                collectiveRenewal.File.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = false;
                CollectiveLiabilityRenewalValidationDAO collectiveLoadPropertyValidationDAO = new CollectiveLiabilityRenewalValidationDAO();
                List<Validation> validations = collectiveLoadPropertyValidationDAO.GetValidationsByFiles(files, collectiveRenewal);
                if (validations.Count > 0)
                {
                    Validation validation;
                    foreach (File file in files)
                    {
                        validation = validations.Find(x => x.Id == file.Id);
                        if (validation != null)
                        {
                            file.Templates[0].Rows[0].HasError = true;
                            file.Templates[0].Rows[0].ErrorDescription = validation.ErrorMessage;
                        }
                    }
                }
                CreateModels(collectiveRenewal, files);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveRenewal.Id);

            }
            catch (Exception ex)
            {
                collectiveRenewal.Status = MassiveLoadStatus.Validated;
                collectiveRenewal.HasError = true;
                collectiveRenewal.ErrorDescription = StringHelper.ConcatenateString("Error validando archivo|", ex.ToString());
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveRenewal);
            }
            finally
            {
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveRenewal);
                DataFacadeManager.Dispose();
            }
        }

        private void ValidateFile(CollectiveEmission collectiveLoad)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveEmission,
                Key2 = (int)EndorsementType.Renewal,
                Key4 = collectiveLoad.Prefix.Id,
                Key5 = collectiveLoad.LoadType.Id
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

        private void CreateModels(CollectiveEmission collectiveEmission, List<File> files)
        {
            Row rowRenewal = files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).Rows.FirstOrDefault();

            decimal policyId = (decimal)DelegateService.commonService.GetValueByField<decimal>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal));
            int branchId = (int)DelegateService.commonService.GetValueByField<int>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            int prefixId = (int)DelegateService.commonService.GetValueByField<int>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
            Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyId);
            
            if (policy == null)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = "Póliza no existe";
            }
            else
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Id, policy.Endorsement.Id);
                companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlansByProductId(companyPolicy.CompanyProduct.Id).First();
                companyPolicy.Id = policy.Id;
                collectiveEmission.Product.Id = policy.Product.Id;
                collectiveEmission.Branch.Id = branchId;
                collectiveEmission.Agency = companyPolicy.Agencies.FirstOrDefault();

                TP.Parallel.ForEach(files, file =>
                {
                    CreateModel(collectiveEmission, file, companyPolicy);
                });
            }
        }

        private void CreateModel(CollectiveEmission collectiveEmission, File file, CompanyPolicy companyPolicy)
        {
            Row rowRenewal = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).Rows.First();
            DateTime currentTo = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
            string policyText = (string)DelegateService.commonService.GetValueByField<string>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));
            decimal PremiumAmount = (decimal)DelegateService.commonService.GetValueByField<decimal>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PremiumAmount));
            companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
            companyPolicy.CurrentTo = currentTo;
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.Endorsement.IsMassive = false;
            companyPolicy.TemporalType = TemporalType.Endorsement;
            if (companyPolicy.Text == null)
            {
                companyPolicy.Text = new Text();
            }
            companyPolicy.Text.TextBody = policyText;
            companyPolicy.UserId = collectiveEmission.User.UserId;
            companyPolicy.CompanyProduct = DelegateService.underwritingService.GetCompanyProductByProductIdPrefixId(collectiveEmission.Product.Id, companyPolicy.Prefix.Id);
            List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetLiabilityByPolicyId(companyPolicy.Id);
            companyPolicy.Summary = new Summary
            {
                RiskCount = companyLiabilityRisks.Count
            };
            collectiveEmission.TotalRows = companyLiabilityRisks.Count;
            PendingOperation pendingOperation = new PendingOperation
            {
                Operation = JsonConvert.SerializeObject(companyPolicy),
                UserId = companyPolicy.UserId
            };
            pendingOperation = DelegateService.commonService.CreatePendingOperation(pendingOperation);
            companyPolicy.Id = pendingOperation.Id;
            collectiveEmission.TemporalId = companyPolicy.Id;
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            pendingOperation = new PendingOperation();
            ParallelHelper.ForEach(companyLiabilityRisks, companyPropertyRisk =>
            {
                CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
                try
                {
                    bool hasError = false;
                    List<string> errorList = new List<string>();
                    if (file != null)
                    {
                        hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                        errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription))).ToList();
                    }
                    collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                    collectiveEmissionRow.RowNumber = companyPropertyRisk.Number;
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                    collectiveEmissionRow.HasError = hasError;
                    collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                    collectiveEmissionRow.SerializedRow = file != null ? JsonConvert.SerializeObject(file) : "";
                    DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                    if (companyPropertyRisk.CompanyRisk == null)
                    {
                        companyPropertyRisk.CompanyRisk = new CompanyRisk()
                        {
                            CompanyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPropertyRisk.RiskId)
                        };
                    }
                    companyPropertyRisk.CompanyRisk.CompanyCoverages.ForEach(x =>
                    {
                        x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        x.CurrentFrom = companyPolicy.CurrentFrom;
                        x.CurrentTo = companyPolicy.CurrentTo;
                        x.CoverStatus = CoverageStatusType.Original;
                    });
                    if (!hasError)
                    {
                        if (file != null)
                        {
                            Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();
                            companyPropertyRisk.CompanyRisk.Status = RiskStatusType.Modified;
                            companyPropertyRisk.Text.TextBody = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));
                            decimal RiskRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));

                            //companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred = new UniquePersonServices.Models.ClaimIncurred();
                            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value))
                            //{
                            //    companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage));
                            //}
                            //else
                            //{
                            //    companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = DelegateService.uniquePersonService.GetSinisterPercentageByInsuredId(companyPropertyRisk.CompanyRisk.CompanyInsured.Id).AccidentRate;
                            //}
                            ModifyCoverages(companyPropertyRisk, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.ModifyCoverages));
                            ModifyInsuredObjects(companyPropertyRisk, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.InsuredObjects), RiskRateTri);
                        }
                        PendingOperation pendingOperationRisk = new PendingOperation
                        {
                            ParentId = companyPolicy.Id,
                            UserId = companyPolicy.UserId,
                            OperationName = "Temporal",
                            Operation = JsonConvert.SerializeObject(companyPropertyRisk)
                        };
                        DelegateService.commonService.CreatePendingOperation(pendingOperationRisk);
                        if (collectiveEmissionRow.Risk == null)
                        {
                            collectiveEmissionRow.Risk = new Risk();
                        }
                        collectiveEmissionRow.Risk.RiskId = pendingOperationRisk.Id;
                    }
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
                catch (Exception ex)
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = "Riesgo: " + companyPropertyRisk.RiskId + " - " + ex.ToString();
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
            });
        }

        private void ModifyCoverages(CompanyLiabilityRisk companyLiabilityRisk, Template modiffiedCoveragesTemplate)
        {
            if (modiffiedCoveragesTemplate != null)
            {
                foreach (Row row in modiffiedCoveragesTemplate.Rows)
                {
                    int id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    int insuredObjectId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    int deductibleId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    CompanyCoverage companyCoverage = companyLiabilityRisk.CompanyRisk.CompanyCoverages.Find(c => c.Id == id && c.CompanyInsuredObject.Id == insuredObjectId && c.Deductible.Id == deductibleId);
                    if (companyCoverage != null)
                    {
                        decimal rate = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CoverageRate));
                        companyCoverage.Rate = rate;
                        companyCoverage.CoverStatus = CoverageStatusType.Modified;
                    }
                    else
                    {
                        CompanyCoverage newCompanyCoverage = new CompanyCoverage()
                        {
                            Id = id,
                            CompanyInsuredObject = new CompanyInsuredObject()
                            {
                                Id = insuredObjectId
                            },
                            Deductible = new Deductible()
                            {
                                Id = deductibleId
                            },
                            CoverStatus = CoverageStatusType.Included
                        };
                        companyLiabilityRisk.CompanyRisk.CompanyCoverages.Add(newCompanyCoverage);
                    }
                }
            }
        }

        private void ModifyInsuredObjects(CompanyLiabilityRisk companyLiabilityRisk, Template insuredObjectTemplate, decimal RiskRateTri)
        {
            if (insuredObjectTemplate != null)
            {
                foreach (Row row in insuredObjectTemplate.Rows)
                {
                    int id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    CompanyInsuredObject companyInsuredObject = companyLiabilityRisk.CompanyRisk.CompanyCoverages.Find(c => c.CompanyInsuredObject.Id == id).CompanyInsuredObject;
                    int amount = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));
                    if (amount > 0)
                    {
                        companyInsuredObject.Amount = amount;
                    }
                    decimal rateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                    if (rateTri > 0)
                    {
                        companyInsuredObject.RateTRI = rateTri;
                    }
                    decimal percentageVariableIndex = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RiskPercentageVariableIndex));
                    if (percentageVariableIndex > 0)
                    {
                        companyInsuredObject.PercentageVariableIndex = percentageVariableIndex;
                    }
                    int recoupmentPeriodId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId));
                    if (recoupmentPeriodId > 0)
                    {
                        companyInsuredObject.RecoupmentPeriod.Id = recoupmentPeriodId;
                    }
                }
                if (RiskRateTri > 0)
                {
                    companyLiabilityRisk.CompanyRisk.CompanyCoverages.ForEach(x => x.CompanyInsuredObject.RateTRI = RiskRateTri);
                }
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
                        propertyName = FieldPropertyName.InsuredObjectCode;
                        int insuredId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
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
    }
}