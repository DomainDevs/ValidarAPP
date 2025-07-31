using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalService.Models;
using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;

namespace Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.DAOs
{
    public class VehicleRenwalDAO
    {
        private static int coverIdAccesoryNoORig = 0;
        private static int coverIdAccesoryORig = 0;
        string templateName = "";
        public CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveEmission)
        {
            ValidateFile(collectiveEmission);
            collectiveEmission.Status = MassiveLoadStatus.Validating;
            collectiveEmission = DelegateService.collectiveService.CreateCollectiveEmission(collectiveEmission);
            if (collectiveEmission == null)
            {
                return null;
            }

            Task.Run(() => ValidateData(collectiveEmission));
            return collectiveEmission;
        }

        private void ValidateFile(CollectiveEmission collectiveLoad)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveRenewal,
                Key2 = (int)EndorsementType.Renewal,
                Key4 = collectiveLoad.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Vehicle
            };

            string fileName = collectiveLoad.File.Name;
            collectiveLoad.File = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            if (collectiveLoad.File != null)
            {
                Template riskDetailTemplate = collectiveLoad.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail);
                collectiveLoad.File.Name = fileName;
                collectiveLoad.File = DelegateService.utilitiesService.ValidateFile(collectiveLoad.File, collectiveLoad.User.AccountName);

                //Si la plantilla de Detalle de Riesgos viene vacía se pierde, entonces se debe recuperar
                if (collectiveLoad.File.Templates.Where(t => t.PropertyName == TemplatePropertyName.RiskDetail).FirstOrDefault() == null)
                {
                    collectiveLoad.File.Templates.Add(riskDetailTemplate);
                }
                else
                {
                    Template RiskDetailTemplate = collectiveLoad.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail);
                    collectiveLoad.File.Templates.Remove(RiskDetailTemplate);
                    if (RiskDetailTemplate != null)
                    {
                        RiskDetailTemplate = DelegateService.utilitiesService.ValidateDataTemplate(fileName, collectiveLoad.User.AccountName, RiskDetailTemplate);
                        collectiveLoad.File.Templates.Add(RiskDetailTemplate);
                    }
                }
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void ValidateData(CollectiveEmission collectiveLoad)
        {
            try
            {
                var file = collectiveLoad?.File;
                if (file != null)
                {
                    Template policyTemplate = file?.Templates?.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Renewal);
                    file.Templates.Remove(policyTemplate);
                    policyTemplate = DelegateService.utilitiesService.ValidateDataTemplate(collectiveLoad.File.Name, collectiveLoad.User.AccountName, policyTemplate);
                    Row rowRenewal = policyTemplate.Rows.First();
                    if (rowRenewal.HasError)
                    {
                        collectiveLoad.HasError = true;
                        collectiveLoad.ErrorDescription = rowRenewal.ErrorDescription.Replace("|", ",");
                        DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                        return;
                    }

                    decimal policyId = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                    int branchId = (int)DelegateService.utilitiesService.GetValueByField<int>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                    int prefixId = (int)DelegateService.utilitiesService.GetValueByField<int>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));

                    CompanyPolicy policy = DelegateService.underwritingService.GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyId);
                    if (policy == null)
                    {
                        collectiveLoad.HasError = true;
                        collectiveLoad.ErrorDescription = Errors.PolicyNotFound;
                        DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                        return;
                    }

                    if (policy.Endorsement == null || !policy.Endorsement.EndorsementType.HasValue
                        || policy.Endorsement.EndorsementType.Value == EndorsementType.Cancellation
                        || policy.Endorsement.EndorsementType.Value == EndorsementType.AutomaticCancellation
                        || policy.Endorsement.EndorsementType.Value == EndorsementType.Nominative_cancellation)
                    {
                        throw new ValidationException(Errors.NullOrCancelledPolicyEndorsement);
                    }

                    VehicleRenwalValidationDAO vehicleRenwalValidationDAO = new VehicleRenwalValidationDAO();
                    List<Validation> emissionValidations = vehicleRenwalValidationDAO.GetValidationsByEmissionTemplate(policyTemplate, collectiveLoad);

                    templateName = policyTemplate.Description;
                    if (emissionValidations != null && emissionValidations.Count > 0)
                    {

                        collectiveLoad.Status = MassiveLoadStatus.Validated;
                        collectiveLoad.HasError = true;

                        string formatedError = string.Join(" ", emissionValidations.Select(x => x.ErrorMessage)).Replace("|", ",");

                        int maxLength = formatedError.Length;
                        if (maxLength > 300)
                        {
                            formatedError = formatedError.Substring(0, 300) + "...";
                        }

                        if (string.IsNullOrEmpty(templateName))
                        {
                            collectiveLoad.ErrorDescription += $"{Errors.ErrorCreateMassiveLoad} {formatedError}";
                        }
                        else
                        {
                            collectiveLoad.ErrorDescription += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                        }
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                        return;
                    }
                    List<File> validatedFiles = new List<File>();
                    if (file.Templates.Count > 0)
                    {
                        if (file?.Templates?.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail) != null)
                        {
                            if (file.Templates.FirstOrDefault(t => t.PropertyName == TemplatePropertyName.RiskDetail).Rows.Count() > 0)
                            {
                                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                                file = DelegateService.utilitiesService.ValidateDataFile(file, collectiveLoad.User.AccountName);
                                Template deductibleTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Deductible);
                                Template accesoriosTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Accesories);
                                Template clausulasTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Clauses);
                                Template scriptTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.DinamicConcepts);

                                validatedFiles = DelegateService.utilitiesService.GetDataTemplates(file.Templates);

                                if (validatedFiles.Any())
                                {
                                    if (deductibleTemplate != null)
                                    {
                                        validatedFiles[0].Templates.Add(deductibleTemplate);
                                    }
                                    if (accesoriosTemplate != null)
                                    {
                                        validatedFiles[0].Templates.Add(accesoriosTemplate);
                                    }
                                    if (clausulasTemplate != null)
                                    {
                                        validatedFiles[0].Templates.Add(clausulasTemplate);
                                    }
                                    if (scriptTemplate != null)
                                    {
                                        validatedFiles[0].Templates.Add(scriptTemplate);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(Errors.ErrorTemplatelNotFound + TemplatePropertyName.RiskDetail + " " + Errors.ErrorTemplatelEmpty);
                        }
                    }

                    DateTime currentTo = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                    string policyText = (string)DelegateService.utilitiesService.GetValueByField<string>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));

                    if (currentTo <= policy.CurrentTo)
                    {
                        throw new ValidationException(Errors.ErrorDateToNotValid);
                    }

                    List<CompanyVehicle> companyVehicleRisks = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(policy.Endorsement.PolicyId);
                    companyVehicleRisks.ForEach(r => r.Risk.Clauses = new List<CompanyClause>());

                    CompanyClause clause = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.Risk, policy.Prefix.Id, null).Find(x => x.Id != companyVehicleRisks[0].Risk.Clauses[0].Id);
                    if (clause != null)
                    {
                        companyVehicleRisks[0].Risk.Clauses.Add(clause);
                    }
                    policy.Endorsement.TicketDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
                    policy.Endorsement.TicketNumber = (int)DelegateService.utilitiesService.GetValueByField<int>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));
                    policy.Product.StandardCommissionPercentage = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(rowRenewal.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.StandardCommissionPercentage));

                    if (companyVehicleRisks.Count == 0)
                    {
                        throw new ValidationException(Errors.ErrorWithOutRisks);
                    }
                    //Recrea el template de riesgos por renovar
                    Template riskDetailTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail);

                    int cont = 1;
                    string[] licensePlatesToEdit = new string[] { };

                    if (riskDetailTemplate.Rows.Any())
                    {
                        licensePlatesToEdit = riskDetailTemplate.Rows.SelectMany(r => r.Fields.Where(f => f.PropertyName == FieldPropertyName.RiskLicensePlate).Select(f => f.Value.ToUpper())).ToArray();
                        cont = riskDetailTemplate.Rows.SelectMany(r => r.Fields.Where(f => f.PropertyName == FieldPropertyName.Identificator).Select(f => Convert.ToInt32(f.Value))).Max() + 1;
                    }

                    foreach (var item in companyVehicleRisks.Where(r => !licensePlatesToEdit.Contains(r.LicensePlate)).OrderBy(r => r.Risk.Number))
                    {
                        Row rowRisk = new Row()
                        {
                            ErrorDescription = null,
                            ExtendedProperties = new List<Core.Application.Extensions.ExtendedProperty>(),
                            HasError = false,
                            Id = cont,
                            Number = item.Risk.Number,
                            Fields = new List<Field>()
                        };

                        rowRisk.Fields.Add(new Field()
                        {
                            FieldType = FieldType.Int32,
                            Value = cont.ToString(),
                            PropertyName = FieldPropertyName.Identificator,
                            Description = "Identificador"
                        });

                        rowRisk.Fields.Add(new Field()
                        {
                            FieldType = FieldType.String,
                            Value = item.LicensePlate,
                            PropertyName = FieldPropertyName.RiskLicensePlate,
                            Description = "Placa"
                        });

                        rowRisk.Fields.Add(new Field()
                        {
                            FieldType = FieldType.String,
                            Value = item.Risk.Text.TextBody,
                            PropertyName = FieldPropertyName.RiskText,
                            Description = "Texto de Riesgo"
                        });

                        rowRisk.Fields.Add(new Field()
                        {
                            FieldType = FieldType.Decimal,
                            Value = item.Price.ToString(),
                            PropertyName = FieldPropertyName.RiskPrice,
                            Description = "Valor Asegurado"
                        });

                        rowRisk.Fields.Add(new Field()
                        {
                            FieldType = FieldType.Decimal,
                            Value = "0",
                            PropertyName = FieldPropertyName.PolicyPremium,
                            Description = "Valor de Prima"
                        });

                        riskDetailTemplate.Rows.Add(rowRisk);

                        cont += 1;
                    }

                    validatedFiles = DelegateService.utilitiesService.GetDataTemplates(file.Templates);

                    if (policy == null)
                    {
                        collectiveLoad.HasError = true;
                        collectiveLoad.ErrorDescription = Errors.PolicyNotFound;
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                        return;
                    }

                    policy.Clauses = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.General, policy.Prefix.Id, null);
                    List<CompanyClause> generalClauses = DelegateService.massiveService.GetClauses(file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses), EmissionLevel.General);

                    if (generalClauses != null && generalClauses.Any())
                    {
                        foreach (CompanyClause c in generalClauses)
                        {
                            if (!policy.Clauses.Exists(cl => cl.Id == c.Id))
                            {
                                policy.Clauses.Add(c);
                            }
                        }
                    }

                    if (validatedFiles.Any())
                    {
                        List<Validation> validations = vehicleRenwalValidationDAO.GetValidationsByFiles(validatedFiles, collectiveLoad, policy.Product.Id, companyVehicleRisks);
                        if (validations.Count > 0)
                        {
                            foreach (File validatedFile in validatedFiles)
                            {
                                Validation validation = validations.Find(x => x.Id == validatedFile.Id);
                                if (validation != null)
                                {
                                    validatedFile.Templates[0].Rows[0].HasError = true;
                                    validatedFile.Templates[0].Rows[0].ErrorDescription = validation.ErrorMessage;
                                }
                            }
                        }
                    }
                    policy.CurrentTo = currentTo;
                    policy.Text = new CompanyText
                    {
                        TextBody = policyText
                    };
                    CreateModels(collectiveLoad, validatedFiles, policy, policyTemplate, companyVehicleRisks);
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveLoad.Id);
                }
            }
            catch (Exception ex)
            {
                collectiveLoad.Status = MassiveLoadStatus.Validated;
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = ex.Message;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, CompanyPolicy policy, Template policyTemplate, List<CompanyVehicle> companyVehicleRisks)
        {
            // Solo se renueva una póliza
            var policyFile = new List<File>
            {
                new File
                {
                    Templates = new List<Template>{ policyTemplate }
                }
            };
            List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetDataFilterIndividualRenewalWithPropertyNames(policyFile, TemplatePropertyName.Renewal, FieldPropertyName.PolicyNumber, FieldPropertyName.PolicyPrefixCode);
            CacheListForVehicle cacheListForVehicle = new CacheListForVehicle();
            cacheListForVehicle.VehicleFilterIndividuals = CreateVehicleFilterIndividual(filtersIndividuals, collectiveLoad.Prefix.Id, collectiveLoad.User.UserId);

            //cacheListForVehicle.Alliances = DelegateService.uniquePersonService.GetAlliances();
            cacheListForVehicle.InsuredForScoreList = new List<int>();
            cacheListForVehicle.InsuredForSimitList = new List<int>();

            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);
            companyPolicy.Clauses = policy.Clauses;

            //Validación sarlaft
            FilterIndividual indiv = filtersIndividuals.Where(i => (i.IndividualType == IndividualType.Person && i.Person.IndividualId == companyPolicy.Holder.IndividualId) || (i.IndividualType == IndividualType.Company && i.Company.IndividualId == companyPolicy.Holder.IndividualId)).FirstOrDefault();

            if (indiv != null && !string.IsNullOrEmpty(indiv.SarlaftError))
            {
                if (indiv.SarlaftError == "ValidateSarlaftExpired")
                {
                    throw new ValidationException(Errors.ValidateSarlaftExpired);
                }
                else if (indiv.SarlaftError == "ValidateSarlaftOvercome")
                {
                    throw new ValidationException(Errors.ValidateSarlaftOvercome);
                }
                else if (indiv.SarlaftError == "ValidateSarlaftPending")
                {
                    throw new ValidationException(Errors.ValidateSarlaftPending);
                }
            }
            else
            {
                throw new ValidationException(Errors.ValidateSarlaftExists);
            }

            IssuanceAgency validationAgent = DelegateService.underwritingService.GetAgencyAll().Find(x => x.Agent.IndividualId == companyPolicy.Agencies[0].Agent.IndividualId);
            collectiveLoad.Agency = companyPolicy.Agencies.FirstOrDefault(u => u.IsPrincipal);
            
            if (validationAgent.DateDeclined != null)
            {
                throw new Exception(Errors.ErrorValidationIntermediaryDisabled);
            }

            if (validationAgent.DateDeclined == null)
            {
                IssuanceAgency validationAgencyes = DelegateService.underwritingService.GetAgencyByAgentIdAgentAgencyId(companyPolicy.Agencies[0].Agent.IndividualId, validationAgent.Id);
                if (validationAgencyes.DateDeclined != null)
                {
                    throw new Exception(Errors.ErrorValidationIntermediaryDisabled);
                }
            }

            if (collectiveLoad.Agency == null)
            {
                throw new Exception(Errors.PrincipalAgencyNotFound);
            }
            
           

            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
            companyPolicy.CurrentTo = policy.CurrentTo;
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.Endorsement.IsMassive = true;
            companyPolicy.Endorsement.EndorsementDays = (companyPolicy.CurrentTo - companyPolicy.CurrentFrom).Days;
            companyPolicy.TemporalType = TemporalType.Endorsement;
            if (companyPolicy.Text == null)
            {
                companyPolicy.Text = new CompanyText();
            }
            companyPolicy.Text.TextBody = policy.Text.TextBody;
            companyPolicy.UserId = collectiveLoad.User.UserId;
            companyPolicy.SubMassiveProcessType = SubMassiveProcessType.CollectiveRenewal;

            decimal standardCommissionPercentage = policy.Product.StandardCommissionPercentage.Value;
            companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, policy.Prefix.Id);
            
            if (companyPolicy.Product.StandardCommissionPercentage != standardCommissionPercentage && standardCommissionPercentage != 0)
            {
                companyPolicy.Agencies.FirstOrDefault(u => u.IsPrincipal).Commissions[0].Percentage = standardCommissionPercentage;
                companyPolicy.Product.StandardCommissionPercentage = standardCommissionPercentage;
            }
            else
            {
                companyPolicy.Agencies.FirstOrDefault(u => u.IsPrincipal).Commissions[0].Percentage = companyPolicy.Product.StandardCommissionPercentage.Value;
            }
            
            companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(companyPolicy.PaymentPlan.Id);
            companyPolicy.Endorsement.TicketDate = policy.Endorsement.TicketDate;
            companyPolicy.Endorsement.TicketNumber = policy.Endorsement.TicketNumber;


            if (companyPolicy.Holder.DeclinedDate != null)
            {
                throw new ValidationException(Errors.InactiveTaker);
            }

            int totalRisks = companyVehicleRisks.Count;
            companyPolicy.Summary = new CompanySummary
            {
                RiskCount = totalRisks
            };
            collectiveLoad.TotalRows = totalRisks;

            if (totalRisks == 1)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = Errors.PolicyIsNotCollective;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                return;
            }

            PendingOperation pendingOperation = new PendingOperation
            {
                Operation = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy),
                UserId = companyPolicy.UserId
            };
            if (!Settings.UseReplicatedDatabase())
            {
                pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
            }
            else
            {
                pendingOperation = DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperation);
            }

            companyPolicy.Id = pendingOperation.Id;

            collectiveLoad.TemporalId = companyPolicy.Id;
            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);

            if (files != null && files.Count > 0)
            {
                foreach (var itemfile in files)
                {
                    itemfile.Templates.Add(policyTemplate);
                    Template riskTemplate = itemfile.Templates.First(y => y.PropertyName == TemplatePropertyName.RiskDetail);
                    string riskText = (string)DelegateService.utilitiesService.GetValueByField<string>(riskTemplate.Rows[0].Fields.Find(f => f.PropertyName == FieldPropertyName.RiskText));
                    string licensePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(riskTemplate.Rows[0].Fields.Find(f => f.PropertyName == FieldPropertyName.RiskLicensePlate));
                    decimal price = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(riskTemplate.Rows[0].Fields.Find(x => x.PropertyName == FieldPropertyName.RiskPrice));
                    CompanyVehicle risk = companyVehicleRisks.Find(v => v.LicensePlate.ToUpper() == licensePlate.ToUpper());
                    if (risk == null)
                    {
                        CollectiveEmissionRow collectiveLoadProcess = new CollectiveEmissionRow
                        {
                            Risk = new Risk()
                        };
                        List<string> errorList = new List<string>();
                        errorList = itemfile.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription))
                            .Select(r => r.ErrorDescription))).ToList();

                        collectiveLoadProcess.MassiveLoadId = collectiveLoad.Id;
                        collectiveLoadProcess.RowNumber = itemfile.Id;
                        collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Validation;
                        collectiveLoadProcess.HasError = true;
                        collectiveLoadProcess.Observations = "El riesgo no pertenece a la póliza.";
                        collectiveLoadProcess.SerializedRow = COMUT.JsonHelper.SerializeObjectToJson(itemfile);
                        collectiveLoad.TotalRows++; 
                        DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveLoadProcess);
                    }
                    else
                    {
                        risk.Risk.Text.TextBody = riskText;

                        if (price > 0)
                        {
                            risk.Price = price;
                        }
                    }
                }
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            }

            coverIdAccesoryNoORig = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
            coverIdAccesoryORig = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories).NumberParameter.Value;

            List<CompanyClause> companyRiskClauses = new List<CompanyClause>();
            List<CompanyClause> companyClauses = new List<CompanyClause>();
            List<Clause> clauses = new List<Clause>();
            List<Clause> riskClauses = new List<Clause>();

            riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Vehicle);
            clauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

            foreach (var riskClause in riskClauses)
            {
                companyRiskClauses.Add(MappCompanyClause(riskClause));
            }

            foreach (var clause in clauses)
            {
                companyClauses.Add(MappCompanyClause(clause));
            }
            //ParallelHelper.ForEach(companyVehicleRisks, risk =>
            foreach (var risk in  companyVehicleRisks)
            {
                CreateModel(collectiveLoad, files, companyPolicy, risk, cacheListForVehicle, companyRiskClauses, companyClauses);
            }//);
        }

        private List<VehicleFilterIndividual> CreateVehicleFilterIndividual(List<FilterIndividual> filtersIndividuals, int prefixId, int userId)
        {
            List<FilterIndividual> individualWithError = new List<FilterIndividual>();

            individualWithError.AddRange(filtersIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));

            filtersIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            filtersIndividuals.AddRange(individualWithError);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FilterIndividual, VehicleFilterIndividual>();

            });

            List<VehicleFilterIndividual> vehicleFilterIndividuals = config.CreateMapper().Map<List<FilterIndividual>, List<VehicleFilterIndividual>>(filtersIndividuals);

            return vehicleFilterIndividuals;
        }

        private void CreateModel(CollectiveEmission collectiveLoad, List<File> files, CompanyPolicy companyPolicy, CompanyVehicle companyVehicleRisk, CacheListForVehicle cacheListForVehicle, List<CompanyClause> riskClauses, List<CompanyClause> coverageClauses)
        {
            string templateName = "";
            string propertyName = "";

            CollectiveEmissionRow collectiveLoadProcess = new CollectiveEmissionRow
            {
                Risk = new Risk()
            };

            try
            {
                bool hasError = false;
                List<string> errorList = new List<string>();

                File file = files.Find(x => x.Templates.First(y => y.PropertyName == TemplatePropertyName.RiskDetail)
                .Rows.Exists(z => z.Fields.First(a => a.PropertyName == FieldPropertyName.RiskLicensePlate).Value.ToUpper() == companyVehicleRisk.LicensePlate.ToUpper()));

                List<CompanyCoverage> parameterCoverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyVehicleRisk.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                companyVehicleRisk.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdByRiskId(companyPolicy.Endorsement.PolicyId, companyVehicleRisk.Risk.RiskId);
                companyVehicleRisk.Risk.Coverages.ForEach(x =>
                {
                    x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    x.CurrentFrom = companyPolicy.CurrentFrom;
                    x.CurrentTo = companyPolicy.CurrentTo;
                    x.CoverStatus = CoverageStatusType.Original;
                    x.RuleSetId = parameterCoverages.First(y => y.Id == x.Id).RuleSetId;
                    x.PosRuleSetId = parameterCoverages.First(y => y.Id == x.Id).PosRuleSetId;
                    x.FlatRatePorcentage = parameterCoverages.First(y => y.Id == x.Id).FlatRatePorcentage;
                    x.IsMandatory = parameterCoverages.First(y => y.Id == x.Id).IsMandatory;
                    x.IsSelected = parameterCoverages.First(y => y.Id == x.Id).IsSelected;
                });

                if (file != null)
                {
                    hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                    errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription))
                        .Select(r => r.ErrorDescription))).ToList();

                    collectiveLoadProcess.MassiveLoadId = collectiveLoad.Id;
                    collectiveLoadProcess.RowNumber = file.Id;
                    collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Validation;
                    collectiveLoadProcess.HasError = hasError;
                    collectiveLoadProcess.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                    collectiveLoadProcess.SerializedRow = COMUT.JsonHelper.SerializeObjectToJson(file);

                    DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveLoadProcess);

                    if (hasError)
                    {
                        DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveLoad.Id);
                        return;
                    }

                    ///
                    ///Sumary Validar beneficiarios inactivos. 
                    ///
                    if (companyVehicleRisk.Risk.Beneficiaries != null && companyVehicleRisk.Risk.Beneficiaries.Count > 0)
                    {

                        foreach (var item in (companyVehicleRisk.Risk.Beneficiaries))
                        {
                            FilterIndividual filterIndividual = null;
                            switch (item.IndividualType)
                            {
                                case IndividualType.Person:
                                    filterIndividual = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Person && p.Person.IndividualId == item.IndividualId);

                                    break;

                                case IndividualType.Company:
                                    filterIndividual = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Company && p.Company.IndividualId == item.IndividualId);
                                    break;

                            }
                            if (filterIndividual != null)
                            {
                                if (filterIndividual.DeclinedDate != null)
                                {
                                    collectiveLoadProcess.HasError = true;
                                    collectiveLoadProcess.Observations += Errors.BeneficiaryInactive + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }

                        }

                    }


                    Template tempalteAccesory = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Accesories);
                    if (tempalteAccesory != null)
                    {
                        string errorScript = string.Empty;
                        List<CompanyAccessory> companyAccessories = DelegateService.massiveService.GetAccesorysByTemplate(tempalteAccesory, companyPolicy, companyVehicleRisk, coverIdAccesoryNoORig, coverIdAccesoryNoORig, ref errorScript);
                        if (string.IsNullOrEmpty(errorScript))
                        {
                            companyVehicleRisk.Accesories = companyAccessories;
                        }
                        else
                        {
                            collectiveLoadProcess.HasError = true;
                            collectiveLoadProcess.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    templateName = CompanyTemplatePropertyName.AdditionalCoverages;
                    Template templateAdditionalCoverages = file.Templates.Find(p => p.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages);

                    if (templateAdditionalCoverages != null)
                    {

                        companyVehicleRisk.Risk.Coverages = DelegateService.massiveService.CreateAdditionalCoverages(parameterCoverages, companyVehicleRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList(), templateAdditionalCoverages.Rows);
                        if (templateAdditionalCoverages.Rows.Any(x => x.HasError))
                        {
                            collectiveLoadProcess.HasError = true;
                            errorList = templateAdditionalCoverages.Rows.Select(t => string.Join(",", t.ErrorDescription)).ToList();
                            collectiveLoadProcess.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                        }
                    }
                    else
                    {
                        companyVehicleRisk.Risk.Coverages = companyVehicleRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                    }
                    companyVehicleRisk.Risk.Coverages.ForEach(x =>
                    {
                        x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        x.CurrentFrom = companyPolicy.CurrentFrom;
                        x.CurrentTo = companyPolicy.CurrentTo;
                        x.CoverStatus = CoverageStatusType.Original;
                        x.RuleSetId = parameterCoverages.First(y => y.Id == x.Id).RuleSetId;
                        x.PosRuleSetId = parameterCoverages.First(y => y.Id == x.Id).PosRuleSetId;
                        x.FlatRatePorcentage = parameterCoverages.First(y => y.Id == x.Id).FlatRatePorcentage;
                        x.IsMandatory = parameterCoverages.First(y => y.Id == x.Id).IsMandatory;
                        x.IsSelected = parameterCoverages.First(y => y.Id == x.Id).IsSelected;
                    });


                    Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);
                    if (companyVehicleRisk.Risk.MainInsured.DeclinedDate != null)
                    {
                        collectiveLoadProcess.HasError = true;
                        collectiveLoadProcess.Observations += Errors.MainInsuredInactive + KeySettings.ReportErrorSeparatorMessage();
                    }


                    if (templateClauses != null)
                    {
                        string errorClause = string.Empty;
                        List<CompanyClause> companyClauses = new List<CompanyClause>();
                        List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
                        DelegateService.massiveService.GetClausesByTemplate(templateClauses, ref companyClauses, ref companyCoverages, riskClauses, coverageClauses, ref errorClause);
                        if (string.IsNullOrEmpty(errorClause))
                        {
                            if (companyClauses.Count > 0)
                            {
                                companyVehicleRisk.Risk.Clauses = companyClauses.Distinct().ToList();
                            }
                            if (companyCoverages.Count > 0)
                            {
                                companyVehicleRisk.Risk.Coverages = companyCoverages;
                            }
                        }
                        else
                        {
                            collectiveLoadProcess.HasError = true;
                            collectiveLoadProcess.Observations += errorClause + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    Template templateScript = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.DinamicConcepts);

                    if (companyPolicy.Product.CoveredRisk.ScriptId.HasValue && companyPolicy.Product.CoveredRisk.ScriptId > 0)
                    {
                        if (templateScript == null)
                        {
                            collectiveLoadProcess.HasError = true;
                            collectiveLoadProcess.Observations += string.Format(Errors.TemplateScriptRequired, Errors.TemplateScript, companyPolicy.Product.Id);
                        }
                    }
                    if (templateScript != null)
                    {
                        string errorScript = string.Empty;
                        List<DynamicConcept> dynamicConcepts = DelegateService.massiveService.GetDynamicConceptsByTemplate(companyPolicy.Product.CoveredRisk.ScriptId, templateScript, ref errorScript);

                        if (string.IsNullOrEmpty(errorScript))
                        {
                            companyVehicleRisk.Risk.DynamicProperties = dynamicConcepts;
                        }
                        else
                        {
                            collectiveLoadProcess.HasError = true;
                            collectiveLoadProcess.Observations += errorScript;
                        }
                    }

                    templateName = "";
                }


                companyVehicleRisk.Risk.Status = RiskStatusType.Original;
                string pendingOperationJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyVehicleRisk);
                PendingOperation pendingOperationRisk = new PendingOperation
                {
                    ParentId = companyPolicy.Id,
                    UserId = companyPolicy.UserId,
                    OperationName = "Temporal",
                    Operation = pendingOperationJsonIsnotNull
                };

                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperationRisk = DelegateService.utilitiesService.CreatePendingOperation(pendingOperationRisk);
                    collectiveLoadProcess.Risk.RiskId = pendingOperationRisk.Id;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                }
                else
                {
                    string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", COMUT.JsonHelper.SerializeObjectToJson(pendingOperationRisk), (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveLoadProcess), (char)007, nameof(CollectiveEmissionRow));
                    QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                }
            }
            catch (Exception ex)
            {
                collectiveLoadProcess.HasError = true;
                collectiveLoadProcess.Observations = StringHelper.ConcatenateString(companyVehicleRisk.Risk.Id.ToString(), "-", templateName, "-", propertyName, "-", ex.ToString());
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
            }
        }

        private void ModifyCoverages(CompanyVehicle companyVehicleRisk, Template modiffiedCoveragesTemplate)
        {
            if (modiffiedCoveragesTemplate != null)
            {
                foreach (Row row in modiffiedCoveragesTemplate.Rows)
                {
                    int id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    int insuredObjectId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    int deductibleId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    CompanyCoverage companyCoverage = companyVehicleRisk.Risk.Coverages.Find(c => c.Id == id && c.InsuredObject.Id == insuredObjectId && c.Deductible.Id == deductibleId);
                    if (companyCoverage != null)
                    {
                        decimal rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CoverageRate));
                        companyCoverage.Rate = rate;
                    }
                    else
                    {
                        CompanyCoverage newCompanyCoverage = new CompanyCoverage()
                        {
                            Id = id,
                            InsuredObject = new CompanyInsuredObject()
                            {
                                Id = insuredObjectId
                            },
                            Deductible = new CompanyDeductible()
                            {
                                Id = deductibleId
                            }
                        };
                        companyVehicleRisk.Risk.Coverages.Add(newCompanyCoverage);
                    }
                }
            }
        }

        private File CreateFileByRiskAndFile(CompanyVehicle vehicleRisk, File modelFile)
        {
            File file = modelFile;
            file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail)
                .Rows[0].Fields.FirstOrDefault(y => y.PropertyName == FieldPropertyName.Identificator).Value = Convert.ToString(vehicleRisk.Risk.Number);
            file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail)
                .Rows[0].Fields.FirstOrDefault(y => y.PropertyName == FieldPropertyName.RiskText).Value = vehicleRisk.LicensePlate;
            file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail)
                .Rows[0].Fields.FirstOrDefault(y => y.PropertyName == FieldPropertyName.Prima).Value = Convert.ToString(vehicleRisk.Risk.Premium);
            return file;
        }
        private CompanyClause MappCompanyClause(Clause clause)
        {
            CompanyClause companyClause = new CompanyClause();
            companyClause.Id = clause.Id;
            companyClause.IsMandatory = clause.IsMandatory;
            companyClause.Name = clause.Name;
            companyClause.Text = clause.Text;
            companyClause.Title = clause.Title;
            return companyClause;
        }
    }
}