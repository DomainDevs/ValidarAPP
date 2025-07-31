using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Massive.Models;
using Sistran.Company.Application.Location.MassivePropertyServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using System.Diagnostics;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.DAOs
{
    public class MassiveRenewalPropertyDAO
    {
        string templateName = "";

        #region Métodos para la validación y el cargue 
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
                massiveRenewal.ErrorDescription = string.Format(Errors.ErrorValidatingFile, ex.Message);
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
                Key5 = (int)SubCoveredRiskType.Property
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
                MassiveRenewalPropertyValidationDAO massivePropertyValidationDAO = new MassiveRenewalPropertyValidationDAO();

                List<Validation> validations = massivePropertyValidationDAO.GetValidationsByFiles(files, massiveRenewal);

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
                ValidatePolicy(massiveRenewal.File);
                CreateModels(massiveRenewal, files);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveRenewal.Id);
            }
            catch (Exception ex)
            {
                massiveRenewal.HasError = true;
                massiveRenewal.ErrorDescription = string.Format(Errors.ErrorValidatingFile, ex.Message);
                DelegateService.massiveRenewalService.UpdateMassiveRenewal(massiveRenewal);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModels(MassiveRenewal massiveRenewal, List<File> files)
        {
            List<FilterIndividual> filterIndividuals = DelegateService.massiveService.GetDataFilterIndividualRenewal(files, TemplatePropertyName.Renewal).ToList();

            CacheListForProperty cacheListForProperty = new CacheListForProperty();
            List<FilterIndividual> individualWithError = new List<FilterIndividual>();

            individualWithError.AddRange(filterIndividuals.Where(i => i.IsCLintonList == true));
            individualWithError.AddRange(filterIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));

            filterIndividuals.RemoveAll(i => i.IsCLintonList == true);
            filterIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            if (Settings.UseReplicatedDatabase())
            {
                filterIndividuals = DelegateService.externalProxyMirrorService.GetMassiveScoresCreditByLastValid(filterIndividuals, massiveRenewal.Prefix.Id, massiveRenewal.User.UserId);
            }
            else
            {
                filterIndividuals = DelegateService.externalProxyService.GetMassiveScoresCreditByLastValid(filterIndividuals, massiveRenewal.Prefix.Id, massiveRenewal.User.UserId);
            }

            cacheListForProperty.FilterIndividuals = filterIndividuals;
            cacheListForProperty.FilterIndividuals.AddRange(individualWithError);
            cacheListForProperty.Alliances = DelegateService.uniquePersonService.GetAlliances();
            cacheListForProperty.InsuredForScoreList = new List<int>();

            ParallelHelper.ForEach(files, file =>
            {
                CreateModel(massiveRenewal, file, cacheListForProperty);
            });
        }

        private void CreateModel(MassiveRenewal massiveRenewal, File file, CacheListForProperty cacheListForProperty)
        {
            MassiveRenewalRow massiveRenewalRow = new MassiveRenewalRow();
            try
            {
                bool hasError = file.Templates.Any(p => p.Rows.Any(x => x.HasError));
                List<string> errorList = file.Templates.Select(p => string.Join(",", p.Rows.Where(x => !string.IsNullOrEmpty(x.ErrorDescription)).Select(y => y.ErrorDescription).Distinct())).ToList();

                massiveRenewalRow.MassiveRenewalId = massiveRenewal.Id;
                massiveRenewalRow.RowNumber = file.Id;
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Validation;
                massiveRenewalRow.HasError = hasError;
                massiveRenewalRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(p => !string.IsNullOrEmpty(p)));
                massiveRenewalRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.massiveRenewalService.CreateMassiveRenewalRow(massiveRenewalRow);

                if (!hasError)
                {
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).Description;

                    Template principalTemplate = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Renewal);
                    Row principalRow = principalTemplate != null ? principalTemplate.Rows.First() : null;
                    if (principalRow == null)
                    {
                        throw new ValidationException(Errors.ErrorPrincipalRowNotFound);
                    }
                    CompanyPolicy companyPolicy = DelegateService.massiveRenewalService.CreateCompanyPolicy(file, TemplatePropertyName.Renewal, massiveRenewal.User.UserId, massiveRenewal.Prefix.Id);
                    companyPolicy.IsPersisted = true;
                    if (file.Templates.Count > 1)
                    {
                        companyPolicy.Endorsement.EndorsementReasonId = 4;
                    }

                    string textRisk = (string)DelegateService.commonService.GetValueByField<string>(principalRow.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));
                    massiveRenewalRow.TemporalId = companyPolicy.Id;
                    massiveRenewalRow.Risk = new Risk();
                    massiveRenewalRow.Risk.Policy = companyPolicy;
                    List<CompanyPropertyRisk> companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    List<Beneficiary> beneficiaries = companyPropertyRisks.SelectMany(x => x.Beneficiaries).ToList();
                    foreach (Beneficiary beneficiary in beneficiaries.GroupBy(x => x.IndividualId).Select(x => x.First()))
                    {
                        beneficiary.IdentificationDocument = DelegateService.uniquePersonService.GetIdentificationDocumentByIndividualIdCustomerType(beneficiary.IndividualId, (int)CustomerType.Individual);
                        //beneficiaries.ForEach(x => x.IdentificationDocument = identificationDocument);
                    }
                    Mapper.CreateMap<Coverage, CompanyCoverage>();
                    Mapper.CreateMap<InsuredObject, CompanyInsuredObject>();
                    Mapper.CreateMap<Insured, CompanyInsured>();
                    foreach (CompanyPropertyRisk property in companyPropertyRisks)
                    {
                        property.Status = RiskStatusType.Original;
                        property.Text = new Text { TextBody = textRisk };
                        List<CompanyInsuredObject> companyInsuredObjectsFromTemplate = GetInsuredObjectFromTemplate(file.Templates.Find(x => x.PropertyName == TemplatePropertyName.InsuredObjects), massiveRenewalRow);
                        List<CompanyInsuredObject> companyInsuredObjects = property.CompanyRisk.CompanyCoverages.GroupBy(y => y.CompanyInsuredObject.Id, x => x.CompanyInsuredObject).Select(x => x.First()).ToList();

                        // Los Objetos del seguro pueden haber cambiado en la parametrización del producto y es necesario obtenerlos nuevamente Jira AXP-428
                        property.CompanyRisk.CompanyCoverages =
                            DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(
                                companyInsuredObjects.Select(x => x.Id).ToList(), property.GroupCoverage.Id, companyPolicy.CompanyProduct.Id, true);

                        foreach (CompanyInsuredObject insuredObject in companyInsuredObjects)
                        {
                            property.CompanyRisk.CompanyCoverages.Where(i => i.CompanyInsuredObject.Id == insuredObject.Id).ToList()
                            .ForEach(y =>
                            {
                                y.CompanyInsuredObject = insuredObject;
                            });
                        }

                        bool modifiedInsuredObjects = companyInsuredObjectsFromTemplate.Any();
                        if (modifiedInsuredObjects)
                        {
                            foreach (CompanyInsuredObject insuredObject in companyInsuredObjects)
                            {
                                CompanyInsuredObject companyInsuredObject;

                                if ((companyInsuredObject = companyInsuredObjectsFromTemplate.Find(y => y.Id == insuredObject.Id)) != null)
                                {
                                    if (companyInsuredObject.PercentageVariableIndex.HasValue && companyInsuredObject.PercentageVariableIndex > 0)
                                    {
                                        insuredObject.PercentageVariableIndex = companyInsuredObject.PercentageVariableIndex;
                                    }
                                    if (companyInsuredObject.RecoupmentPeriod != null && companyInsuredObject.RecoupmentPeriod.Id > 0)
                                    {
                                        insuredObject.RecoupmentPeriod = companyInsuredObject.RecoupmentPeriod;
                                    }
                                    if (companyInsuredObject.RateTRI.HasValue && companyInsuredObject.RateTRI > 0)
                                    {
                                        insuredObject.RateTRI = companyInsuredObject.RateTRI;
                                    }

                                    insuredObject.Amount = companyInsuredObject.Amount;
                                    companyInsuredObjectsFromTemplate.Remove(companyInsuredObject);
                                    if (insuredObject.Amount == 0)
                                    {
                                        if (companyInsuredObjects.Count == 1)
                                        {
                                            massiveRenewalRow.HasError = true;
                                            massiveRenewalRow.Observations += Errors.ErrorExcludedInsuredObject + insuredObject.Id + KeySettings.ReportErrorSeparatorMessage();
                                        }
                                        else
                                        {
                                            //Válida si la suma asegurada del objeto del seguro es cero, elimina la cobertura asociada a ese objeto del seguro.
                                            List<CompanyCoverage> coveragesRemove = property.CompanyRisk.CompanyCoverages.Where(x => x.CompanyInsuredObject.Id == insuredObject.Id).ToList();
                                            foreach (CompanyCoverage coveragesToRemove in coveragesRemove)
                                            {
                                                property.CompanyRisk.CompanyCoverages.Remove(coveragesToRemove);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        property.CompanyRisk.CompanyCoverages.Where(x => x.CompanyInsuredObject.Id == insuredObject.Id).ToList()
                                        .ForEach(y =>
                                        {
                                            y.CompanyInsuredObject = insuredObject;
                                        });
                                    }
                                }
                            }
                            if (companyInsuredObjectsFromTemplate.Any())
                            {
                                foreach (CompanyInsuredObject insuredObjectAdded in companyInsuredObjectsFromTemplate)
                                {
                                    // este llamado es suceptible de mejorar enviando el parámetro de filtro inicialmente incluidas
                                    List<Coverage> coverages = DelegateService.underwritingService.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectAdded.Id, property.GroupCoverage.Id, companyPolicy.CompanyProduct.Id);
                                    //Inicilmente incluida/ Obligatoria
                                    coverages = coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                                    List<CompanyCoverage> coveragesToAdd = Mapper.Map<List<Coverage>, List<CompanyCoverage>>(coverages);
                                    coveragesToAdd.ForEach(x =>
                                    {
                                        x.CompanyInsuredObject = insuredObjectAdded;
                                    });
                                    property.CompanyRisk.CompanyCoverages.AddRange(coveragesToAdd);
                                    companyInsuredObjects.Add(insuredObjectAdded);
                                }
                            }

                        }

                        var principalRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(principalRow.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                        if (principalRateTri > 0)
                        {
                            property.CompanyRisk.CompanyCoverages.ForEach(x => x.CompanyInsuredObject.RateTRI = principalRateTri);
                        }

                        property.CompanyRisk.CompanyInsured.ClaimIncurred = new ClaimIncurred();
                        decimal? accidentRate = DelegateService.uniquePersonService.GetSinisterPercentageByInsuredId(property.CompanyRisk.CompanyInsured.InsuredId).AccidentRate;

                        if (accidentRate == null)
                        {
                            if (!string.IsNullOrEmpty(principalRow.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value))
                            {
                                property.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = (decimal)DelegateService.commonService.GetValueByField<decimal>(principalRow.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage));
                                if (property.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate < 0 || property.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate > 100)
                                {
                                    massiveRenewalRow.HasError = true;
                                    massiveRenewalRow.Observations += string.Format(Errors.ErrorAccidentRateValue) + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                            else
                            {
                                massiveRenewalRow.HasError = true;
                                massiveRenewalRow.Observations += string.Format(Errors.AccidentRateMandatory) + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }
                        else
                        {
                            property.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = accidentRate.Value;
                        }

                        if (Settings.ImplementWebServices())
                        {
                            //Vaidación Externos 
                            Alliance validateAlliance = new Alliance();
                            if (companyPolicy.Alliance != null)
                            {
                                validateAlliance = cacheListForProperty.Alliances.Find(x => x.Id == companyPolicy.Alliance.Id);
                            }

                            companyPolicy.CompanyProduct.IsScore = DelegateService.externalProxyService.ValidateApplyScoreCreditByProduct(companyPolicy.CompanyProduct, validateAlliance, companyPolicy.Prefix.Id);
                            if (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault())
                            {
                                property.CompanyRisk.CompanyInsured.ScoreCredit = cacheListForProperty.FilterIndividuals.Find(x => x.InsuredCode == property.CompanyRisk.CompanyInsured.InsuredId).ScoreCredit;
                            }
                            //Vaidación Externos 
                        }
                        else
                        {
                            property.CompanyRisk.CompanyInsured.ScoreCredit = DelegateService.externalProxyService.GetScoreDefault();
                        }
                        //List<CompanyCoverage> coveragesFromTemplate = GetCoveragesFromTemplate(file.Templates.Find(x => x.PropertyName == TemplatePropertyName.ModifyCoverages), companyInsuredObjects,companyPolicy.Product.Id,property.GroupCoverage.Id);

                        foreach (CompanyCoverage coverage in property.CompanyRisk.CompanyCoverages)
                        {
                            // Ajuste temporal se deberían sacar los paquetes de reglas al recuperar las coberturas
                            var coverageInfo = DelegateService.underwritingService.GetCoverageByCoverageIdProductIdGroupCoverageId(coverage.Id, companyPolicy.CompanyProduct.Id, property.GroupCoverage.Id);
                            coverage.RuleSetId = coverageInfo.RuleSetId;
                            coverage.PosRuleSetId = coverageInfo.PosRuleSetId;
                            coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                            coverage.CurrentFrom = companyPolicy.CurrentFrom;
                            coverage.CurrentTo = companyPolicy.CurrentTo;
                            coverage.CoverStatus = CoverageStatusType.Original;
                            coverage.LimitAmount = 0;
                            coverage.SubLimitAmount = 0;
                            coverage.DeclaredAmount = 0;
                            coverage.ExcessLimit = 0;
                            coverage.LimitClaimantAmount = 0;
                            coverage.LimitOccurrenceAmount = 0;
                        }

                        Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);
                        if (templateClauses != null)
                        {
                            templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Clauses).Description;
                            property.Clauses = DelegateService.massiveService.GetClauses(templateClauses, EmissionLevel.Risk);
                            property.CompanyRisk.CompanyCoverages.ForEach(p => p.Clauses = DelegateService.massiveService.GetClausesByCoverageId(templateClauses, p.Id));
                        }

                        PendingOperation pendingOperation = new PendingOperation();
                        pendingOperation.UserId = companyPolicy.UserId;
                        pendingOperation.ParentId = companyPolicy.Id;
                        pendingOperation.Operation = JsonConvert.SerializeObject(property);

                        massiveRenewalRow.TemporalId = companyPolicy.Id;
                        massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
                        massiveRenewalRow.Risk.Policy.Branch = companyPolicy.Branch;
                        massiveRenewalRow.Risk = property;
                        massiveRenewalRow.Risk.Policy = massiveRenewalRow.Risk.Policy ?? companyPolicy;

                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* Without Replicated Database */
                            DelegateService.commonService.CreatePendingOperation(pendingOperation);
                            DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
                            /* Without Replicated Database */
                        }
                        else
                        {
                            /* with Replicated Database */
                            PendingOperation policyPendingOperation = new PendingOperation
                            {
                                Operation = JsonConvert.SerializeObject(companyPolicy),
                                UserId = companyPolicy.UserId
                            };
                            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(policyPendingOperation), (char)007, JsonConvert.SerializeObject(pendingOperation), (char)007, JsonConvert.SerializeObject(massiveRenewalRow), (char)007, nameof(MassiveRenewalRow));
                            var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePendingOperationQuee", routingKey: "CreatePendingOperationQuee", serialization: "JSON");
                            queue.PutOnQueue(pendingOperationJson);
                            /* with Replicated Database */
                        }

                        if (Settings.ImplementWebServices() &&
                            (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault() && property.CompanyRisk.CompanyInsured.ScoreCredit == null))
                        {
                            CheckExternalServices(cacheListForProperty, companyPolicy, property, massiveRenewal, file, principalRow);
                        }
                        if (Settings.ImplementValidate2G())
                        {
                            DelegateService.sybaseEntityService.InsertTempPolicySinister2G(
                                massiveRenewal.Id, massiveRenewalRow.Id, companyPolicy.Branch.Id, companyPolicy.Prefix.Id, companyPolicy.DocumentNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    massiveRenewalRow.Observations += Errors.ErrorCreateRisk;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    massiveRenewalRow.Observations += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }


        private void CheckExternalServices(CacheListForProperty cacheListForProperty, CompanyPolicy companyPolicy, CompanyPropertyRisk companyProperty, MassiveRenewal massiveRenewal, File file, Row row)
        {
            FilterIndividual filterIndividual = cacheListForProperty.FilterIndividuals.Find(x => x.InsuredCode == companyProperty.CompanyRisk.CompanyInsured.InsuredId);
            bool scoreAlreadyQueried = false; bool simitAlreadyQueried = false; bool requireScore = false; bool requireSimit = false; bool requireFasecolda = false;
            string licencePlate = string.Empty; string surname = string.Empty;
            IdentificationDocument identificationDocument = new IdentificationDocument();

            if (filterIndividual.IndividualType == Core.Application.UniquePersonService.Enums.IndividualType.LegalPerson)
            {
                surname = filterIndividual.Company.Name;
                identificationDocument = filterIndividual.Company.IdentificationDocument;
            }
            else if (filterIndividual.IndividualType == Core.Application.UniquePersonService.Enums.IndividualType.Person)
            {
                surname = filterIndividual.Person.Surname;
                identificationDocument = filterIndividual.Person.IdentificationDocument;
            }

            if (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault())
            {
                requireScore = true;


                if (!cacheListForProperty.InsuredForScoreList.Contains(companyProperty.CompanyRisk.CompanyInsured.InsuredId))
                {
                    cacheListForProperty.InsuredForScoreList.Add(companyProperty.CompanyRisk.CompanyInsured.InsuredId);
                }
                else
                {
                    scoreAlreadyQueried = true;
                }
            }

            if (requireScore)
            {
                DelegateService.externalProxyService.CheckExternalServices(filterIndividual.Person.IdentificationDocument, surname, filterIndividual.InsuredCode.Value, licencePlate, massiveRenewal.Id, row.Number, (int)SubCoveredRiskType.Property, massiveRenewal.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
            }
        }
        private List<CompanyInsuredObject> GetInsuredObjectFromTemplate(Template insuredObjectTemplate, MassiveRenewalRow massiveRenewalRow)
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
                CompanyInsuredObject insuredObject = new CompanyInsuredObject
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
            var consolidatedinsuredObject =
                   from p in insuredObjectTemplate.Rows
                   group p by new
                   {
                       Id = Convert.ToInt32(p.Fields.First(x => x.PropertyName == FieldPropertyName.InsuredObjectCode).Value)
                   }
                   into policies
                   where policies.Count() > 1
                   select new
                   {
                       Id = policies.Key.Id,
                       Total = policies.Count(),
                   };

            foreach (var item in consolidatedinsuredObject)
            {
                if (insuredObjects.Select(i => i.Id).Distinct().Contains(item.Id))
                {
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations += Errors.ErrorInsuredObjectDuplicated + item.Id + KeySettings.ReportErrorSeparatorMessage();
                }
            }
            List<CompanyInsuredObject> companyInsuredObject = DelegateService.underwritingService.GetRecoupmentPeriodAndPercentageVariableIndex(insuredObjects);
            foreach (CompanyInsuredObject itemInsuredObject in companyInsuredObject)
            {
                if (itemInsuredObject.RecoupmentPeriod.Enable)
                {
                    if (insuredObjects.Find(x => x.Id == itemInsuredObject.Id).RecoupmentPeriod == null)
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations += Errors.ErrorRecoupmentPeriod + " " + itemInsuredObject.Id + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                if (itemInsuredObject.RequiredPercentageVariableIndex)
                {
                    if (insuredObjects.Find(x => x.Id == itemInsuredObject.Id).PercentageVariableIndex == 0)
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations += Errors.ErrorPercentageVariableIndex + " " + itemInsuredObject.Id + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
            }
            return insuredObjects;
        }

        private string ValidateInsuredObject(int insuredObjectId, decimal insuredAmount)
        {
            List<string> validationErrors = new List<string>();
            if (insuredObjectId <= 0)
            {
                validationErrors.Add(Errors.TheInsuredObjectColumnOfTheInsuranceObjectsTemplateIsRequired);
            }
            if (insuredAmount < 0)
            {
                validationErrors.Add(Errors.TheSumInsuredColumnOfTheInsuranceObjectsTemplateIsRequired);
            }
            if (!validationErrors.Any())
            {
                //DelegateService.underwritingService.ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId()
            }
            return string.Join("|", validationErrors);
        }

        private List<CompanyCoverage> GetCoveragesFromTemplate(Template coverageModificationsTemplate, List<CompanyInsuredObject> insuredObjects, int productId, int groupCoveragesId)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            if (coverageModificationsTemplate == null)
            {
                return new List<CompanyCoverage>();
            }
            foreach (Row row in coverageModificationsTemplate.Rows)
            {
                int insuredObjectId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                int coverageId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                int deductibleId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible));
                decimal coverageRate = (decimal)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CoverageRate));

                if (insuredObjectId == 0 || insuredObjects.All(x => x.Id != insuredObjectId))
                {
                    throw new ValidationException(Errors.ErrorMissingInsuredObjectForCoverage);
                }
                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoveragesId);
                coverage.Deductible = new Deductible
                {
                    Id = deductibleId
                };
                coverage.Rate = coverageRate;
                coverages.Add(coverage);
            }
            coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(coverages);
            foreach (var companyCoverage in coverages)
            {
                var insuredObject = insuredObjects.Find(x => x.Id == companyCoverage.CompanyInsuredObject.Id);
                companyCoverage.CompanyInsuredObject = insuredObject;
            }
            return coverages;
        }

        public void ValidatePolicy(File validatedFile)
        {
            Template template = validatedFile.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal);
            if (!validatedFile.Templates.Any(t => t.Rows.Any(r => r.HasError)))
            {
                var consolidatedPolicies =
                                      from p in template.Rows
                                      group p by new
                                      {
                                          DocumentNumber = Convert.ToDecimal(p.Fields.First(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal).Value)
                                      }
                                      into policies
                                      where policies.Count() > 1
                                      select new
                                      {
                                          DocumentNumber = policies.Key.DocumentNumber,
                                          Total = policies.Count(),
                                      };

                ParallelHelper.ForEach(template.Rows, row =>
                {

                    var PolicyNumber = consolidatedPolicies.FirstOrDefault(z => z.DocumentNumber == Convert.ToDecimal(row.Fields.First(y => y.PropertyName == FieldPropertyName.PolicyNumberRenewal).Value))?.DocumentNumber;
                    if (row.Fields != null && PolicyNumber.HasValue)
                    {
                        row.HasError = true;
                        row.ErrorDescription = String.Format("{0} : {1} {2}", Errors.ErrorDuplicatePolicies, Errors.PolicyNumber, row.Fields.First(u => u.PropertyName == FieldPropertyName.PolicyNumberRenewal).Value);
                    }

                });
            }
        }
        #endregion

        #region Métodos y propiedades para la generación del reporte

        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<CompanyIrregularHeight> irregularHeights = new List<CompanyIrregularHeight>();
        private static List<CompanyIrregularPlant> irregularPlants = new List<CompanyIrregularPlant>();
        private static List<CompanyLevelZone> levelzones = new List<CompanyLevelZone>();
        private static List<CompanyLocation> locations = new List<CompanyLocation>();
        private static List<CompanyReinforcedStructureType> reinforcedStructureTypes = new List<CompanyReinforcedStructureType>();
        private static List<CompanyRepair> companyRepairs = new List<CompanyRepair>();
        private static List<CompanyStructureType> companyStructureTypes = new List<CompanyStructureType>();
        private static List<RiskUse> riskUse = new List<RiskUse>();
        private static List<Core.Application.Locations.Models.RiskType> riskTypes = new List<Core.Application.Locations.Models.RiskType>();
        private static List<ConstructionType> constructionTypes = new List<ConstructionType>();
        private static List<LineBusiness> lineBusiness = new List<LineBusiness>();
        private static List<SubLineBusiness> subLineBusiness = new List<SubLineBusiness>();
        private static List<InsuredObject> insuredObjects = new List<InsuredObject>();
        private static List<City> cities = new List<City>();
        private static List<State> states = new List<State>();
        private static List<RatingZone> ratingZones = new List<RatingZone>();
        private static List<CompanyDamage> damages = new List<CompanyDamage>();
        private static List<CompanyMicroZone> companyMicroZones = new List<CompanyMicroZone>();
        private static List<Currency> currency = new List<Currency>();
        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();
        private static bool _cacheLoaded;

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveRenewal"></param>
        /// <returns>string</returns>
        public string GenerateReportToMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            MassiveLoadProcessStatus processStatus = MassiveLoadProcessStatus.Validation;
            switch (massiveRenewal.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = MassiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = MassiveLoadProcessStatus.Finalized;
                    break;
            }
            DelegateService.massiveService.LoadReportCacheList();
            if (!_cacheLoaded)
            {
                LoadList();
            }
            List<MassiveRenewalRow> massiveEmissionRows = DelegateService.massiveRenewalService.GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveRenewal.Id, processStatus, false, null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;

            File file = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null)
            {
                return "";
            }

            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            string key = Guid.NewGuid().ToString();
            string filePath = "";
            
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

            file.FileType = FileType.CSV;

            TP.Parallel.ForEach(massiveEmissionRows,
                    (process) =>
                    {
                        FillPropertyFields(massiveRenewal, process, serializeFields);

                        if (concurrentRows.Count >= bulkExcel || massiveEmissionRows.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte Hogar_" + key + "_" + massiveRenewal.Id;
                            filePath = DelegateService.commonService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();
                        }
                    });
            
            return filePath;

        }
        public void FillPropertyFields(MassiveRenewal massiveRenewal, MassiveRenewalRow proccess, string serializeFields)
        {
            try
            {
                Policy policy = proccess.Risk.Policy;
                policy.Endorsement = new Endorsement { EndorsementType = EndorsementType.Renewal, Id = proccess.Risk.Policy.Endorsement.Id };
                policy.Id = proccess.TemporalId.GetValueOrDefault();
                policy.Prefix = massiveRenewal.Prefix;

                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveRenewal.Status.Value, policy);
                if (companyPolicy == null)
                {
                    return;
                }
                List<CompanyPropertyRisk> risks = GetCompanyPropertyRisk(massiveRenewal, proccess, companyPolicy.Id);

                List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);

                foreach (CompanyPropertyRisk property in risks)
                {
                    fields = DelegateService.massiveService.FillInsuredFields(fields, property.CompanyRisk?.CompanyInsured);
                    fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = proccess.RowNumber.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveRenewal.Id.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = property.AmountInsured.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = "1";// property.Number.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskAddress).Value = CreateAddress(property);
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskFloorNumber).Value = property.FloorNumber.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskConstructionYear).Value = property.ConstructionYear.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskLongitude).Value = property.Longitude.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskLatitude).Value = property.Latitude.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(property.Beneficiaries);
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskUseDescription).Value = (property.RiskUse != null && property.RiskUse.Id > 0) ? riskUse.FirstOrDefault(u => u.Id == property.RiskUse.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskTypeDescription).Value = (property.CompanyRiskTypeEarthquake != null && property.CompanyRiskTypeEarthquake.Id > 0) ? riskTypes.FirstOrDefault(u => u.Id == property.CompanyRiskTypeEarthquake.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(property.Clauses);
                    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = companyPolicy.ExchangeRate != null ? currency.FirstOrDefault(l => l.Id == companyPolicy.ExchangeRate.Currency.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyNumberDays).Value = companyPolicy.Endorsement.EndorsementDays == 0 ? companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days.ToString() : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.RaitingZoneDescription).Value = (property.RatingZone != null && property.RatingZone.Id > 0) ? ratingZones.FirstOrDefault(u => u.Id == property.RatingZone.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.Observations).Value = "";

                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.StructureDescription).Value = (property.CompanyStructureType != null && property.CompanyStructureType.Id > 0) ? companyStructureTypes.FirstOrDefault(u => u.TypeCD == property.CompanyStructureType.Id).Description : ""; ;
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularHeightDescription).Value = (property.CompanyIrregularHeight != null) ? irregularHeights.FirstOrDefault(u => u.Id == property.CompanyIrregularHeight.Id).Description : "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularPlantDescription).Value = (property.CompanyIrregularPlant != null) ? irregularPlants.FirstOrDefault(u => u.Id == property.CompanyIrregularPlant.Id).Description : "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskRepairedDescripcion).Value = (property.CompanyRepair != null) ? companyRepairs.FirstOrDefault(u => u.Id == property.CompanyRepair.Id).Description : "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureDescription).Value = (property.CompanyReinforcedStructureType != null) ? reinforcedStructureTypes.FirstOrDefault(u => u.Id == property.CompanyReinforcedStructureType.Id).Description : "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskPreviousDamageDescription).Value = (property.CompanyDamage != null) ? damages.FirstOrDefault(u => u.Id == property.CompanyDamage.Id).Description : "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskLocationDescription).Value = "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskNeighborhoodDescription).Value = "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCOfConstructionDescription).Value = (property.ConstructionType != null && property.ConstructionType.Id > 0) ? constructionTypes.FirstOrDefault(u => u.Id == property.ConstructionType.Id).Description : "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone).Value = (property.CompanyMicroZone != null && property.CompanyMicroZone.Id > 0 && companyMicroZones.Count > 0) ? companyMicroZones.FirstOrDefault(u => u.Id == property.CompanyMicroZone.Id).Description : "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value = property.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate.ToString();

                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = "";
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = "";

                    //Asistencia
                    List<int> assistanceCoveragesIds = DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceProperty);
                    List<CompanyCoverage> coveragesAssistance = property.CompanyRisk.CompanyCoverages.Where(u => assistanceCoveragesIds.Exists(id => id == u.Id)).ToList();
                    decimal assistancePremium = coveragesAssistance.Sum(x => x.PremiumAmount);
                    if (assistancePremium > 0)
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                            (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                            (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                    }

                    serializeFields = JsonConvert.SerializeObject(fields);
                    decimal valueRc = 0, insuredValue = 0;
                    valueRc = property.CompanyRisk.CompanyCoverages.Where(u => !string.IsNullOrEmpty(u.Description) && u.Description.Contains("R.C.E")).Sum(u => u.LimitAmount);
                    insuredValue = (companyPolicy.Summary.AmountInsured - valueRc);

                    foreach (CompanyCoverage coverage in property.CompanyRisk.CompanyCoverages)
                    {

                        List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                        fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = insuredValue.ToString();
                        fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = valueRc.ToString();
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
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(coverage.Clauses);
                        concurrentRows.Add(new Row
                        {
                            Fields = fieldsC,
                            Number = proccess.RowNumber
                        });
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
        private List<CompanyPropertyRisk> GetCompanyPropertyRisk(MassiveRenewal massiveRenewal, MassiveRenewalRow proccess, int tempId)
        {
            List<CompanyPropertyRisk> companyProperties = new List<CompanyPropertyRisk>();
            switch (massiveRenewal.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    var pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(tempId);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(tempId);
                        /* with Replicated Database */
                    }
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    var policy = proccess.Risk.Policy;
                    if (policy.Endorsement != null && policy.Endorsement.EndorsementType.HasValue)
                    {
                        //companyProperties = DelegateService.propertyService.GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(massiveRenewal.Prefix.Id, policy.Branch.Id, policy.DocumentNumber, policy.Endorsement.EndorsementType.Value);
                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* without Replicated Database */
                            DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));

                        }
                        else
                        {
                            DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));
                        }
                    }
                    break;
            }
            return companyProperties;

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
            riskTypes = DelegateService.propertyService.GetRiskTypes();
            ratingZones = DelegateService.commonService.GetRatingZones();
            companyMicroZones = DelegateService.propertyService.GetCompanyMicroZones();
            damages = DelegateService.propertyService.GetCompanyDamages();
            currency = DelegateService.commonService.GetCurrencies();
            _cacheLoaded = true;
        }

        private string CreateAddress(CompanyPropertyRisk propertyRisk)
        {
            string address = "";
            address += propertyRisk.FullAddress;

            if (propertyRisk.City != null && propertyRisk.City.State != null)
            {
                address += StringHelper.ConcatenateString(" | ",
                    states.DefaultIfEmpty(new State { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.State.Id).Description,
                    " | ",
                    cities.DefaultIfEmpty(new City { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.Id && u.State.Id == propertyRisk.City.State.Id).Description);
            }

            return address;

        }
        #endregion
    }
}