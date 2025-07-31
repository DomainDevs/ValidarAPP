using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Newtonsoft.Json;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework;
using Util = Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.Resources;
using Sistran.Company.Application.Massive.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.MassiveServices.Models;
using System.Collections.Concurrent;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Core.Application.Utilities.Configuration;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.DAOs
{
    public class MassiveLoadLiabilityDAO
    {
        private List<DocumentType> documentTypes = null;
        private List<Deductible> deductibles = null;
        private List<City> cities = null;
        private List<State> states = null;
        private List<LineBusiness> lineBusiness = null;
        private List<SubLineBusiness> subLineBusiness = null;
        private ConcurrentBag<Row> concurrentRows = null;
        /// <summary>
        /// Tarifar Cargue Masivo
        /// </summary>
        /// <param name="massiveLoad">Id Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        public MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            ValidateFile(massiveEmission);

            massiveEmission.Status = MassiveLoadStatus.Validating;
            massiveEmission = DelegateService.massiveUnderwritingService.CreateMassiveEmission(massiveEmission);
            try
            {
                if (massiveEmission != null)
                {
                    TP.Task.Run(() => ValidateData(massiveEmission));
                }
            }
            catch (Exception ex)
            {
                massiveEmission.HasError = true;
                massiveEmission.ErrorDescription = StringHelper.ConcatenateString("Error Validar archivo", "|", ex.Message);
                DelegateService.massiveService.UpdateMassiveLoad(massiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
            return massiveEmission;
        }

        private void ValidateData(MassiveEmission massiveEmission)
        {
            try
            {
                massiveEmission.File = DelegateService.commonService.ValidateDataFile(massiveEmission.File, massiveEmission.User.AccountName);
                massiveEmission.TotalRows = massiveEmission.File.Templates.First(p => p.IsPrincipal).Rows.Count();
                Row row = massiveEmission.File.Templates.First(p => p.IsPrincipal).Rows.First();
                if (row.Fields.Any(p => p.PropertyName == FieldPropertyName.RequestGroup))
                {
                    massiveEmission.BillingGroupId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BillingGroup));
                    massiveEmission.RequestId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RequestGroup));
                    CompanyRequest companyRequest = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber((int)massiveEmission.BillingGroupId, massiveEmission.RequestId.ToString(), null).First();

                    if (companyRequest == null)
                    {
                        throw new ValidationException(Errors.ErrorProductNotFoundForTheRequestSent);
                    }
                    else
                    {
                        massiveEmission.Branch = companyRequest.Branch;
                        CompanyRequestEndorsement endorsement = companyRequest.CompanyRequestEndorsements.OrderBy(p => p.DocumentNumber).Last();
                        massiveEmission.Product = endorsement.Product;
                        massiveEmission.Agency = endorsement.Agencies.First(a => a.IsPrincipal);
                    }

                }
                DelegateService.massiveService.UpdateMassiveLoad(massiveEmission);
                List<File> files = DelegateService.commonService.GetDataTemplates(massiveEmission.File.Templates);
                MassiveLoadLiabilityValidationDAO massiveLoadPropertyValidationDAO = new MassiveLoadLiabilityValidationDAO();
                List<Validation> validations = massiveLoadPropertyValidationDAO.GetValidationsByFiles(files, massiveEmission);
                if (validations.Count > 0)
                {
                    Validation validation;
                    foreach (File file in files)
                    {
                        validation = validations.Find(x => x.Id == file.Id);
                        if (validation != null)
                        {
                            file.Templates[0].Rows[0].HasError = true;
                            file.Templates[0].Rows[0].ErrorDescription += validation.ErrorMessage;
                        }
                    }
                }
                CreateModels(massiveEmission, files);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveEmission.Id);
            }
            catch (Exception ex)
            {
                massiveEmission.Status = MassiveLoadStatus.Validated;
                massiveEmission.HasError = true;
                massiveEmission.ErrorDescription = StringHelper.ConcatenateString("Error validando archivo|", ex.ToString());
                DelegateService.massiveService.UpdateMassiveLoad(massiveEmission);
            }
            finally
            {

                DataFacadeManager.Dispose();
            }
        }

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="massiveLoad">Cargue Masivo</param>
        private void ValidateFile(MassiveEmission massiveEmission)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.MassiveEmission,
                Key3 = massiveEmission.LoadType.Id,
                Key4 = massiveEmission.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Liability
            };

            string fileName = massiveEmission.File.Name;
            massiveEmission.File = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (massiveEmission.File != null)
            {
                massiveEmission.File.Name = fileName;
                massiveEmission.File = DelegateService.commonService.ValidateFile(massiveEmission.File, massiveEmission.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void CreateModels(MassiveEmission massiveEmission, List<File> files)
        {
            List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetFilterIndividuals(massiveEmission.User.UserId, massiveEmission.Branch.Id, files, TemplatePropertyName.EmissionLiability);
            TP.Parallel.ForEach(files, file =>
            {
                CreateModel(massiveEmission, file, filtersIndividuals);
            });
        }

        /// <summary>
        /// Crear Modelos
        /// </summary>
        /// <param name="massiveEmission">Cargue Masivo</param>
        /// <param name="file">Datos Archivo</param>
        /// <param name="filtersIndividuals"></param>
        /// <param name="correlativePolicyBranchId"></param>
        private void CreateModel(MassiveEmission massiveEmission, File file, List<FilterIndividual> filtersIndividuals)
        {
            string templateName = String.Empty;
            string propertyName = String.Empty;
            MassiveEmissionRow massiveEmissionRow = new MassiveEmissionRow();
            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription))).ToList();
                massiveEmissionRow.MassiveLoadId = massiveEmission.Id;
                massiveEmissionRow.RowNumber = file.Id;
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Validation;
                massiveEmissionRow.HasError = hasError;
                massiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                massiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.massiveUnderwritingService.CreateMassiveEmissionRow(massiveEmissionRow);
                if (!hasError)
                {
                    templateName = TemplatePropertyName.EmissionLiability;
                    CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.CreateCompanyPolicy(massiveEmission, massiveEmissionRow, file, TemplatePropertyName.EmissionLiability, filtersIndividuals);
                    massiveEmissionRow.Risk = new Risk
                    {
                        Policy = new Policy
                        {
                            Id = companyPolicy.Id
                        }
                    };
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmissionLiability).Rows.First();
                    if (companyPolicy.CorrelativePolicyNumber.HasValue && !ValidateCorrelativePolicy(companyPolicy.CorrelativePolicyNumber.Value, massiveEmission.Prefix.Id, massiveEmission.Branch.Id, companyPolicy.CurrentTo))
                    {
                        throw new ValidationException(Errors.ErrorInvalidCorrelativePolicy);
                    }
                    CompanyLiabilityRisk companyLiability = new CompanyLiabilityRisk
                    {
                        Status = RiskStatusType.Original,
                        CoveredRiskType = CoveredRiskType.Location
                    };
                    companyLiability.CompanyRisk = new CompanyRisk();
                    companyLiability.CompanyRisk.CompanyInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, filtersIndividuals);
                    companyLiability.GroupCoverage = DelegateService.massiveUnderwritingService.CreateGroupCoverage(row, companyPolicy.CompanyProduct.Id);
                    //---------------------Prueba validacion a externos -----------------------
                    UniquePersonServices.Models.Alliance validateAlliance = new UniquePersonServices.Models.Alliance();
                    if (companyPolicy.Alliance != null)
                    {
                        validateAlliance = DelegateService.uniquePersonService.GetAlliances().Find(x => x.Id == companyPolicy.Alliance.Id);
                    }
                    companyPolicy.CompanyProduct.IsScore = DelegateService.externalProxyService.ValidateApplyScoreCreditByProduct(companyPolicy.CompanyProduct, validateAlliance, companyPolicy.Prefix.Id);
                    if (!companyPolicy.CompanyProduct.IsScore.GetValueOrDefault())
                    {
                        companyLiability.CompanyRisk.CompanyInsured.ScoreCredit = filtersIndividuals.Find(x => x.InsuredCode == companyLiability.CompanyRisk.CompanyInsured.InsuredId).ScoreCredit;
                    }
                    //-------------------------------------------------------------------------
                    companyLiability.Beneficiaries = new List<Beneficiary>();
                    companyLiability.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyLiability.CompanyRisk.CompanyInsured, filtersIndividuals));
                    propertyName = FieldPropertyName.RiskAddress;
                    companyLiability.FullAddress = (row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskAddress).Value);
                    companyLiability.NomenclatureAddress = new Core.Application.Locations.Models.NomenclatureAddress
                    {
                        Type = new Core.Application.Locations.Models.RouteType
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
                    companyLiability.CompanyRisk.CompanyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.CompanyProduct.Id, companyLiability.GroupCoverage.Id, companyPolicy.Prefix.Id);
                    companyLiability.CompanyRisk.CompanyCoverages = companyLiability.CompanyRisk.CompanyCoverages.Where(x => x.IsSelected).ToList();
                    companyLiability.CompanyRisk.CompanyCoverages = CreateInsuredObject(companyLiability.CompanyRisk.CompanyCoverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.InsuredProperty), companyPolicy.CompanyProduct.Id, companyLiability.GroupCoverage.Id);
                    companyLiability.CompanyRisk.CompanyCoverages.ForEach(x =>
                    {
                        x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        x.CurrentFrom = companyPolicy.CurrentFrom;
                        x.CurrentTo = companyPolicy.CurrentTo;
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
                    templateName = TemplatePropertyName.AdditionalBeneficiaries;
                    List<Beneficiary> beneficiaries = new List<Beneficiary>();
                    if (file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries) != null)
                    {
                        beneficiaries = DelegateService.massiveService.CreateAdditionalBeneficiaries(file.Templates.FirstOrDefault(p => p.PropertyName == TemplatePropertyName.AdditionalBeneficiaries), filtersIndividuals);
                    }
                    companyLiability.Beneficiaries.AddRange(beneficiaries);
                    if (companyLiability.Beneficiaries.GroupBy(b => b.IndividualId, ben => ben).Select(b => b.First())
                            .ToList().Count != companyLiability.Beneficiaries.Count)
                    {
                        throw new ValidationException(Errors.ErrorDuplicatedAdditionalBeneficiary);
                    }
                    decimal beneficiariesParticipation = beneficiaries.Sum(x => x.Participation);
                    if (beneficiariesParticipation < 100)
                    {
                        companyLiability.Beneficiaries[0].Participation -= beneficiariesParticipation;
                    }
                    else
                    {
                        throw new ValidationException(Errors.ErrorParticipationBeneficiary + " " + row.Number);
                    }

                    if (!Settings.UseReplicatedDatabase())
                    {
                        PendingOperation pendingOperation = new PendingOperation
                        {
                            ParentId = companyPolicy.Id,
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyLiability)
                        };
                        DelegateService.commonService.CreatePendingOperation(pendingOperation);
                        DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                    }
                    else
                    {
                        /* with Replicated Database */
                        PendingOperation pendingOperationPolicy = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyPolicy)
                        };

                        PendingOperation pendingOperationRisk = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyLiability)
                        };

                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(massiveEmissionRow), (char)007, nameof(MassiveEmissionRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                       
                    }
                }

            }
            catch (Exception ex)
            {
                massiveEmissionRow.HasError = true;
                massiveEmissionRow.Observations = StringHelper.ConcatenateString(file.Id.ToString(), "-", templateName, "-", propertyName, "-", ex.ToString());
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
        }

        private bool ValidateCorrelativePolicy(decimal documentNumber, int prefixId, int branchId, DateTime currentTo)
        {
            Policy correlativePolicy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, documentNumber);

            if (correlativePolicy == null || correlativePolicy.Endorsement == null)
            {
                return false;
            }
            var endorsementType = correlativePolicy.Endorsement.EndorsementType;
            if (!endorsementType.HasValue
                || endorsementType.Value == EndorsementType.Cancellation
                || endorsementType.Value == EndorsementType.AutomaticCancellation
                || endorsementType.Value == EndorsementType.Nominative_cancellation)
            {
                return false;
            }
            return currentTo <= correlativePolicy.CurrentTo;
        }

        /// <summary>
        /// Crear Bienes Asegurados
        /// </summary>
        /// <param name="template"></param>
        /// <returns>Coberturas</returns>
        private List<CompanyCoverage> CreateInsuredObject(List<CompanyCoverage> coverages, Template template, int productId, int groupCoverageId)
        {
            if (template == null)
            {
                throw new ValidationException(Errors.ErrorInsuredProperty);
            }
            foreach (Row row in template.Rows)
            {
                try
                {
                    int coverageId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    int insuredObjectId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectPropertyCode));
                    int deductibleCode = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible));
                    decimal amount = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));
                    decimal percentage = 0;//(decimal) DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectPercentage));
                    if (coverageId == 0 || insuredObjectId == 0)
                    {
                        continue;
                    }
                    CompanyCoverage coverage;
                    if ((coverage = coverages.Find(x => x.Id == coverageId)) == null)
                    {
                        coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
                        string validationErrors = ValidateCoverage(insuredObjectId, coverage);
                        if (!string.IsNullOrEmpty(validationErrors))
                        {
                            throw new ValidationException(validationErrors);
                        }
                        coverages.Add(coverage);
                    }
                    if (coverageId > 0 && amount > 0)
                    {
                        coverage.LimitAmount = amount;
                        coverage.LimitOccurrenceAmount = amount;
                        coverage.LimitClaimantAmount = amount;
                        coverage.DeclaredAmount = amount;
                        coverage.SubLimitAmount = amount;
                        coverage.CompanyInsuredObject.Amount = amount;
                    }
                    if (percentage > 0)
                    {
                        coverage.Rate = percentage;
                    }
                    if (deductibleCode > 0)
                    {
                        coverage.Deductible = new Deductible
                        {
                            Id = deductibleCode
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new ValidationException();
                }
            }
            return coverages;
        }

        private string ValidateCoverage(int insuredObjectId, CompanyCoverage coverage)
        {
            if (coverage == null)
            {
                return "Errors.CoverageNotRelatedToProductAndGroupCoverage";
            }
            if (coverage.CompanyInsuredObject.Id != insuredObjectId)
            {
                return "Errors.ErrorInsuredObjectDiferentFromCoverageInsuredObject;";
            }
            if (coverage.MainCoverageId != 0)
            {
                return "Errors.CovegareIsNotPrincipal";
            }
            return "";
        }

        public MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoad.Id, MassiveLoadProcessStatus.Events, false, false);
                if (massiveEmissionRows.Count > 0)
                {
                    TP.Task.Run(() => IssuanceMassiveEmissionRows(massiveLoad, massiveEmissionRows));
                }
                else
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
                }
            }
            return massiveLoad;
        }

        public void IssuanceMassiveEmissionRows(MassiveLoad massiveLoad, List<MassiveEmissionRow> massiveEmissionRows)
        {
            try
            {
                massiveLoad.Status = MassiveLoadStatus.Issuing;
                massiveLoad.TotalRows = massiveEmissionRows.Count + DelegateService.massiveUnderwritingService.GetFinalizedMassiveEmissionRowsByMassiveLoadId(massiveLoad.Id).Count;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                List<CompanyPolicy> companyPolicies = DelegateService.massiveUnderwritingService.GetCompanyPoliciesToIssueByOperationIds(massiveEmissionRows.Where(x => !x.HasError).Select(x => x.Risk.Policy.Id).ToList());
                ParallelHelper.ForEach(companyPolicies, companyPolicy =>
                {
                    ExecuteCreatePolicy(companyPolicy, massiveEmissionRows.FirstOrDefault(x => x.Risk.Policy.Id == companyPolicy.Id));
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

        private void ExecuteCreatePolicy(CompanyPolicy companyPolicy, MassiveEmissionRow massiveEmissionRow)
        {
            try
            {
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Issuance;

                if (massiveEmissionRow.Risk.Policy.Id > 0)
                {
                    massiveEmissionRow.Status = MassiveLoadProcessStatus.Finalized;

                    List<PendingOperation> pendingOperations;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(companyPolicy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(companyPolicy.Id);
                    }
                    List<CompanyLiabilityRisk> companyLiabilityRisks = new List<CompanyLiabilityRisk>();
                    foreach (PendingOperation po in pendingOperations)
                    {
                        var companyLiabilityRisk = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(po.Operation);
                        companyLiabilityRisk.CompanyPolicy = companyPolicy;
                        companyLiabilityRisks.Add(companyLiabilityRisk);
                    }
                    companyPolicy = DelegateService.liabilityService.CreateEndorsement(companyPolicy, companyLiabilityRisks);
                    UpdateJSONPolicy(massiveEmissionRow, companyPolicy);
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
                    massiveEmissionRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    massiveEmissionRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;

                }
                else
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            catch (Exception ex)
            {
                massiveEmissionRow.HasError = true;
                massiveEmissionRow.Observations = StringHelper.ConcatenateString(Errors.ErrorIssuing, "(", ex.Message, ")");
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }


        private void UpdateJSONPolicy(MassiveEmissionRow massiveEmissionRow, CompanyPolicy companyPolicy)
        {
            var pendingOperation = new PendingOperation();
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);

                /* Without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                /* with Replicated Database */
            }

            if (!Settings.UseReplicatedDatabase())
            {
                /* without Replicated Database */
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                /* without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                PendingOperation pendingOperationPolicy = new PendingOperation
                {
                    UserId = companyPolicy.UserId,
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    Id = companyPolicy.Id
                };


                string pendingOperationJson = JsonConvert.SerializeObject(pendingOperationPolicy);
                var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee", serialization: "JSON");
                queue.PutOnQueue(pendingOperationJson);
                /* with Replicated Database */
            }

        }

        #region Reportes
        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <returns>string</returns>
        public string GenerateReportToMassiveLoad(MassiveEmission massiveEmission)
        {
            MassiveLoadProcessStatus processStatus = MassiveLoadProcessStatus.Validation;
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = MassiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = MassiveLoadProcessStatus.Finalized;
                    break;
            }
            concurrentRows = new ConcurrentBag<Row>();
            DelegateService.massiveService.LoadReportCacheList();
            LoadList(massiveEmission);
            List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveEmission.Id, processStatus, false, null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;

            File file = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null)
            {
                return "";
            }
            else
            {
                file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
                string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
                string key = Guid.NewGuid().ToString();
                string filePath = "";
                
                int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

                file.FileType = FileType.CSV;

                TP.Parallel.ForEach(massiveEmissionRows,
                    (process) =>
                    {
                        string serialize = serializeFields;
                        FillLiabilityFields(massiveEmission, process, serialize);

                        if (concurrentRows.Count >= bulkExcel || massiveEmissionRows.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte RC_" + key + "_" + massiveEmission.Id;
                            filePath = DelegateService.commonService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();

                        }
                    });

                return filePath;
            }
        }

        public void FillLiabilityFields(MassiveEmission massiveEmission, MassiveEmissionRow massiveEmissionRow, string serializeFields)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyByMassiveLoadStatus(massiveEmission.Status.Value, massiveEmissionRow.Risk.Policy);
                if (companyPolicy != null)
                {
                    List<CompanyLiabilityRisk> risks = GetCompanyLiabilityRisk(massiveEmission, massiveEmissionRow, companyPolicy.Id);

                    List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);

                    foreach (CompanyLiabilityRisk liability in risks)
                    {

                        fields = DelegateService.massiveService.FillInsuredFields(fields, liability.CompanyRisk.CompanyInsured);

                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = massiveEmissionRow.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
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
                                Fields = fieldsC,
                                Number = massiveEmissionRow.RowNumber
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
        private List<CompanyLiabilityRisk> GetCompanyLiabilityRisk(MassiveEmission massiveEmission, MassiveEmissionRow proccess, int tempId)
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

        private void LoadList(MassiveEmission massiveEmission)
        {
            if (documentTypes == null)
            {
                documentTypes = new List<DocumentType>();
                deductibles = new List<Deductible>();
                cities = new List<City>();
                states = new List<State>();
                lineBusiness = new List<LineBusiness>();
                subLineBusiness = new List<SubLineBusiness>();

                documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
                deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
                cities = DelegateService.commonService.GetCities();
                states = DelegateService.commonService.GetStates();
                lineBusiness = DelegateService.commonService.GetLinesBusiness();
                subLineBusiness = DelegateService.commonService.GetSubLineBusiness();

            }

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

        #endregion
    }
}