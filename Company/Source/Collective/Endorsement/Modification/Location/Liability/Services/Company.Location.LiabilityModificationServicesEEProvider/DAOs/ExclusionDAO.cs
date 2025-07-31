using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.DAOs
{
    public class ExclusionDAO
    {
        public CollectiveEmission CreateCollectiveExclusion(CollectiveEmission collectiveEmission)
        {
            ValidateFile(collectiveEmission);
            collectiveEmission.Status = MassiveLoadStatus.Validating;
            collectiveEmission = DelegateService.collectiveService.CreateCollectiveEmission(collectiveEmission);

            if (collectiveEmission != null)
            {
                TP.Task.Run(() => ValidateData(collectiveEmission));
            }
            return collectiveEmission;
        }

        private void ValidateFile(CollectiveEmission collectiveEmission)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveExclusion,
                Key2 = (int)EndorsementType.Modification,
                Key4 = collectiveEmission.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Liability
            };

            string fileName = collectiveEmission.File.Name;
            collectiveEmission.File = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (collectiveEmission.File != null)
            {
                collectiveEmission.File.Name = fileName;
                collectiveEmission.File = DelegateService.commonService.ValidateFile(collectiveEmission.File, collectiveEmission.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void ValidateData(CollectiveEmission collectiveEmission)
        {
            try
            {
                collectiveEmission.File = DelegateService.commonService.ValidateDataFile(collectiveEmission.File, collectiveEmission.User.AccountName);
                collectiveEmission.TotalRows = collectiveEmission.File.Templates.First(p => p.IsPrincipal).Rows.Count;
                Template policyTemplate = collectiveEmission.File.Templates.First(x => x.PropertyName == TemplatePropertyName.Policy);
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.RiskExclusion).IsPrincipal = true;
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.Policy).IsPrincipal = false;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                List<File> validatedFiles = DelegateService.commonService.GetDataTemplates(collectiveEmission.File.Templates);
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.RiskExclusion).IsPrincipal = false;
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.Policy).IsPrincipal = true;
                ExclusionValidationDAO exclusionValidationDAO = new ExclusionValidationDAO();
                List<Validation> validationsPolicy = exclusionValidationDAO.GetValidationsByFilesPolicy(collectiveEmission.File, collectiveEmission);
                if (validationsPolicy.Count > 0)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = string.Join(",", validationsPolicy.Select(x => x.ErrorMessage));
                    return;
                }
                Row rowExclusion = policyTemplate.Rows.First();

                int policyNum = (int)DelegateService.commonService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                int branchId = (int)DelegateService.commonService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                int prefixId = (int)DelegateService.commonService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
                Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNum);
                if (policy == null)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = "Póliza no existe";
                }
                else
                {
                    CreateModels(collectiveEmission, validatedFiles, policy);
                    collectiveEmission.Status = MassiveLoadStatus.Validated;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = StringHelper.ConcatenateString("Error validando archivo|", ex.Message);
            }
            finally
            {
                collectiveEmission.Status = MassiveLoadStatus.Validated;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                DataFacadeManager.Dispose();
            }

        }

        private void CreateModels(CollectiveEmission collectiveEmission, List<File> files, Policy policy)
        {
            if (files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).HasError)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).ErrorDescription;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
            else if (files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).Rows[0].HasError)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).Rows[0].ErrorDescription;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
            else
            {
                Row rowExclusion = files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).Rows.First();
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Id, policy.Endorsement.Id);

                decimal premium = (decimal)DelegateService.commonService.GetValueByField<decimal>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PremiumAmount));
                string policyText = (string)DelegateService.commonService.GetValueByField<string>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));

                companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
                companyPolicy.Endorsement.EndorsementType = EndorsementType.Modification;
                companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
                companyPolicy.Endorsement.IsMassive = true;
                companyPolicy.TemporalType = TemporalType.Endorsement;
                companyPolicy.Text = new Text { TextBody = policyText };
                companyPolicy.UserId = policy.UserId;
                companyPolicy.Branch = policy.Branch;
                companyPolicy.CompanyBranch = new CompanyBranch()
                {
                    Id = policy.Branch.Id,
                    Description = policy.Branch.Description,
                    IsDefault = policy.Branch.IsDefault,
                    SalePoints = policy.Branch.SalePoints,
                    SmallDescription = policy.Branch.SmallDescription
                };
                companyPolicy.CompanyProduct = DelegateService.underwritingService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                companyPolicy.Summary = new Summary
                {
                    RiskCount = collectiveEmission.TotalRows
                };
                PendingOperation pendingOperation = new PendingOperation
                {
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    UserId = companyPolicy.UserId
                };
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.commonService.CreatePendingOperation(pendingOperation);
                }
                else
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperation);
                }
                companyPolicy.Id = pendingOperation.Id;
                collectiveEmission.TemporalId = companyPolicy.Id;
                collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiabilitiesByPolicyId(policy.Id);
                if (companyLiabilityRisks.Count == 0)
                    throw new Exception("La poliza no existe");
                if (files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.RiskExclusion) != null)
                {
                    ParallelHelper.ForEach(files, file =>
                    {
                        string templateName = "";
                        string propertyName = "";
                        CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
                        try
                        {
                            templateName = TemplatePropertyName.RiskExclusion;
                            Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskExclusion).Rows.First();
                            int RiskNum = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Identificator));
                            bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                            List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription))).ToList();

                            collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                            collectiveEmissionRow.RowNumber = RiskNum;
                            collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                            collectiveEmissionRow.HasError = hasError;
                            collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                            collectiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);
                            DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                            if (!hasError)
                            {
                                CompanyLiabilityRisk companyLiabilityRisk = companyLiabilityRisks.Find(x => x.Number == RiskNum);
                                companyLiabilityRisk.Text = new Text { TextBody = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText)) };
                                DateTime currentFrom = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));

                                companyLiabilityRisk.CompanyRisk.Status = RiskStatusType.Excluded;
                                if (companyLiabilityRisk.CompanyRisk.CompanyCoverages != null)
                                {
                                    companyLiabilityRisk.CompanyRisk.CompanyCoverages.ForEach(c => c.CurrentFrom = currentFrom);
                                }
                                PendingOperation pendingOperationRisk = new PendingOperation
                                {
                                    ParentId = companyPolicy.Id,
                                    UserId = companyPolicy.UserId,
                                    Operation = JsonConvert.SerializeObject(companyLiabilityRisk)
                                };
                                if (!Settings.UseReplicatedDatabase())
                                {
                                    DelegateService.commonService.CreatePendingOperation(pendingOperationRisk);
                                }
                                else
                                {
                                    DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperationRisk);
                                }
                                collectiveEmissionRow.Risk = new Risk { RiskId = pendingOperationRisk.Id };
                            }
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                        }
                        catch (Exception ex)
                        {
                            collectiveEmissionRow.HasError = true;
                            if (string.IsNullOrEmpty(templateName))
                            {
                                collectiveEmissionRow.Observations += Errors.ErrorCreateModel;
                            }
                            else
                            {
                                string[] messages = ex.Message.Split('|');
                                collectiveEmissionRow.Observations += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                            }
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                        }
                        finally
                        {
                            DataFacadeManager.Dispose();
                        }
                    });
                }
            }

        }
    }
}