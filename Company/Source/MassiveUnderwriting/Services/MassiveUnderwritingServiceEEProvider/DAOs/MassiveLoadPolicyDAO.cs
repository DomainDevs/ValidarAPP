using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.ProductServices.Models;
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassiveLoadPolicyDAO
    {
        string templateName = "";
        /// <summary>
        /// Crear CompanyPolicy Con Los Datos Del Archivo
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <param name="file">Archivo</param>
        /// <returns>CompanyPolicy</returns>
        public CompanyPolicy CreateCompanyPolicy(MassiveEmission massiveLoad, MassiveEmissionRow massiveEmissionRow, File file, string templatePropertyName, List<FilterIndividual> filtersIndividuals)
        {
            if (massiveLoad.LoadType.Id == (int)SubMassiveProcessType.MassiveEmissionWithRequest)
            {
                return CreateCompanyPolicyWithRequest(massiveLoad, file, filtersIndividuals);
            }
            else
            {
                return CreateCompanyPolicyWithOutRequest(massiveLoad, massiveEmissionRow, file, templatePropertyName, filtersIndividuals);
            }
        }

        /// <summary>
        /// Crear CompanyPolicy Sin Solicitud Con Los Datos Del Archivo
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <param name="file">Archivo</param>
        /// <returns>CompanyPolicy</returns>
        public CompanyPolicy CreateCompanyPolicyWithOutRequest(MassiveEmission massiveLoad, MassiveEmissionRow massiveEmissionRow, File file, string templatePropertyName, List<FilterIndividual> filtersIndividuals)
        {
            try
            {
                Row row = file.Templates.First(x => x.PropertyName == templatePropertyName).Rows.First();
                templateName = file.Templates.First(x => x.PropertyName == templatePropertyName).Description;
                CompanyPolicy companyPolicy = new CompanyPolicy
                {
                    Endorsement = new CompanyEndorsement
                    {
                        EndorsementType = EndorsementType.Emission,
                        IsMassive = true
                    },
                    TemporalType = TemporalType.Policy,
                    UserId = massiveLoad.User.UserId,
                    IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now),
                    BeginDate = DateTime.Now,
                    Prefix = new CompanyPrefix { Id = massiveLoad.Prefix.Id },
                    Summary = new CompanySummary
                    {
                        RiskCount = 1
                    },
                    Clauses = new List<CompanyClause>(),
                    Text =  new CompanyText()
                };
                //Agrega Clausulas a nivel de riesgo 
                companyPolicy.Clauses = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.General, companyPolicy.Prefix.Id, null);
                companyPolicy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;

                int productId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(productId, massiveLoad.Prefix.Id);
                if (companyPolicy.Product != null)
                {
                    if (companyPolicy.Product.IsCollective && !companyPolicy.Product.IsMassive)
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations = Errors.ErrorProductCollective;
                    }
                    else if (!companyPolicy.Product.IsCollective && !companyPolicy.Product.IsMassive)
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations = Errors.ErrorProductType;
                    }
                   
                    companyPolicy.Product.StandardCommissionPercentage = Convert.ToDecimal(DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.StandardCommissionPercentage)));
                }
                Branch branch = DelegateService.commonService.GetBranchById((int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId)));
                List<CompanySalesPoint> companySalesPoints = new List<CompanySalesPoint>() { new CompanySalesPoint() };
                if (branch.SalePoints != null)
                {
                    foreach (SalePoint item in branch.SalePoints)
                    {
                        companySalesPoints.Add(CreateCompanySalePoint(item));
                    }
                }

                companyPolicy.Branch = new CompanyBranch
                {
                    Id = branch.Id,
                    Description = branch.Description,
                    SalePoints = companySalesPoints
                };
                companyPolicy.Branch.SalePoints[0].Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.SalePoint));
                
                companyPolicy.PaymentPlan = GetPaymentPlan(companyPolicy.Product.Id, row);

                //IssuanceAgency
                int agencyCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                int agentType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                massiveLoad.Agency = DelegateService.underwritingService.GetAgencyByAgentCodeAgentTypeCode(agencyCode, agentType);
                if (massiveLoad.Agency == null)
                {
                    throw new ValidationException(Errors.AgentNotFound);
                }
                companyPolicy.Agencies = new List<IssuanceAgency>();
                companyPolicy.Agencies.Add(CreateAgencyPrincipal(massiveLoad, companyPolicy.Product.Id, companyPolicy.Product.StandardCommissionPercentage.GetValueOrDefault()));

                companyPolicy.Holder = DelegateService.massiveService.CreateHolder(row, filtersIndividuals);
                int holderPaymentMethod = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentMethod));
                if (holderPaymentMethod != 0 && companyPolicy.Holder.PaymentMethod.Id != holderPaymentMethod)
                {
                    throw new ValidationException(string.Format(Errors.ErrorPaymentMethodNotParameterizedHolder, holderPaymentMethod));
                }

                companyPolicy.SubMassiveProcessType = SubMassiveProcessType.MassiveEmissionWithoutRequest;
                companyPolicy.Endorsement.TicketDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
                companyPolicy.Endorsement.TicketNumber = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));
                companyPolicy.Text.TextBody = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));


                if (companyPolicy.Holder == null || row.HasError)
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations += row.ErrorDescription;
                    return null;
                }
                else
                {
                    companyPolicy.BusinessType = (BusinessType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyBusinessType));
                    if (companyPolicy.BusinessType == BusinessType.Accepted)
                    {
                        Template coinsuranceAcceptedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAccepted);
                        if (coinsuranceAcceptedTemplate == null)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations = Errors.MensageTemplateCoInsuredAceptdRequired + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    if (companyPolicy.BusinessType == BusinessType.Assigned)
                    {
                        Template coinsuranceAssignedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAssigned);
                        if (coinsuranceAssignedTemplate == null)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations = Errors.MensageTemplateCoInsuredAssignedRequired + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    companyPolicy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                    companyPolicy.CurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    companyPolicy.CurrentTo = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));

                    int compareFromResult = DateTime.Compare(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
                    if (compareFromResult >= 0)
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations = Errors.ErrorToDatePolicy + KeySettings.ReportErrorSeparatorMessage();
                    }

                    companyPolicy.Endorsement.EndorsementDays = (companyPolicy.CurrentTo - companyPolicy.CurrentFrom).Days;

                    int policyCurrency = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrency));
                    companyPolicy.ExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, policyCurrency);
                    PolicyType policyType = DelegateService.commonService.GetPolicyTypesByProductId(companyPolicy.Product.Id).FirstOrDefault(x => x.Id == (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyType)));
                    companyPolicy.PolicyType = new CompanyPolicyType
                    {
                        Id = policyType.Id,
                        Description = policyType.Description,
                        Prefix = new Prefix { Id = policyType.Prefix.Id }
                    };
                    //Plantillas Adicionales
                    switch (companyPolicy.BusinessType)
                    {
                        case BusinessType.CompanyPercentage:
                            companyPolicy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
                            {
                                ParticipationPercentageOwn = 100
                            });
                            break;
                        case BusinessType.Assigned:
                            companyPolicy.CoInsuranceCompanies = DelegateService.massiveService.CreateCoInsuranceAssigned(companyPolicy, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.CoinsuranceAssigned));

                            break;
                        case BusinessType.Accepted:

                            Template templateCoinsuranceAccepted = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAccepted);

                            if (templateCoinsuranceAccepted == null)
                            {
                                templateName = "";
                                throw new ValidationException(Errors.ErrorCoinsuranceAcceptedTemplateMandatory);
                            }

                            Template templateCoinsuranceAcceptedAgency = file.Templates.FirstOrDefault(x => x.PropertyName == CompanyTemplatePropertyName.CoinsuranceAcceptedAgency);

                            companyPolicy.CoInsuranceCompanies = DelegateService.massiveService.CreateCoInsuranceAccepted(companyPolicy, file);
                            break;
                    }

                    string errorAgency = String.Empty;
                    List<IssuanceAgency> agencies = DelegateService.massiveService.CreateAdditionalAgencies(file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries), ref errorAgency);

                    companyPolicy.Agencies.AddRange(agencies);

                    if (companyPolicy.Agencies.GroupBy(a => a.Code, agen => agen).Select(x => x.First()).Count() != companyPolicy.Agencies.Count)
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += Errors.MessageAgentsDuplicated;
                    }

                    decimal agenciesParticipation = agencies.Sum(x => x.Participation);
                    if (agencies.Any(x => x.Participation == 0))
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += Errors.MessageAgentsPercentageCantBeNull;
                    }
                    if (agenciesParticipation < 100)
                    {
                        companyPolicy.Agencies[0].Participation -= agenciesParticipation;
                        agencies.ForEach(x => x.Commissions = companyPolicy.Agencies[0].Commissions);
                    }
                    else
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += Errors.MessageAgentsPercentage;
                    }
                    var correlativePolicyNumber = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoCorrelativePolicyNumber));
                    if (correlativePolicyNumber != default(decimal))
                    {
                        companyPolicy.CorrelativePolicyNumber = correlativePolicyNumber;
                    }

                    Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);
                    if (templateClauses != null)
                    {
                        List<ConditionLevel> conditionLevels = DelegateService.underwritingService.GetConditionLevels();
                        templateName = templateClauses.Description;

                        foreach (Row clausesRow in templateClauses.Rows)
                        {
                            int levelCode = (int)DelegateService.utilitiesService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.LevelCode));
                            int clauseCode = (int)DelegateService.utilitiesService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.ClauseCode));
                            ConditionLevel conditionLevel = conditionLevels.FirstOrDefault(c => c.Id == levelCode);

                            if (conditionLevel.EmissionLevel == EmissionLevel.General)
                            {
                                Clause clause = new Clause();
                                clause = DelegateService.underwritingService.GetClauseByClauseId(clauseCode);

                                if (clause == null)
                                {
                                    throw new ValidationException(string.Format(Errors.ErrorClauseNotExists, clauseCode));
                                }
                                else if (!companyPolicy.Clauses.Exists(c => c.Id == clause.Id))
                                {
                                    CompanyClause companyClause = CreateCompanyClause(clause);
                                    companyPolicy.Clauses.Add(companyClause);
                                }
                            }
                        }
                    }

                    return companyPolicy;
                }
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');
                throw new ValidationException(messages[0]);
            }
        }
        private CompanyPaymentPlan GetPaymentPlan(int productId, Row row)
        {
            List<int> paymentPlanPremiumFinance = new List<int> { 38, 39, 40 }; //TODO: se debe es modificar la tabla de plan de pagos para que tenga una marca para saber si aplica o no financiamiento
            List<PaymentPlan> paymentPlans = DelegateService.underwritingService.GetPaymentPlansByProductId(productId);
            int holderPaymentPlanId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan));
            int numberQuotas = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PremiumFinanceQuotas));
            string quotasDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PremiumFinanceQuotas).Description;
            string error = "";

            PaymentPlan holderPaymentPlan = null;
            if (holderPaymentPlanId > 0)
            {
                holderPaymentPlan = paymentPlans.FirstOrDefault(p => p.Id == holderPaymentPlanId);
                if (holderPaymentPlan == null)
                {
                    throw new ValidationException(string.Format(Errors.ErrorPaymentPlanNotParameterized, holderPaymentPlanId));
                }
            }
            else
            {
                if (paymentPlans.Exists(u => u.IsDefault))
                {
                    holderPaymentPlan = paymentPlans.FirstOrDefault(u => u.IsDefault);
                }
                else
                {
                    holderPaymentPlan = paymentPlans.First();
                }
            }

            CompanyPaymentPlan companyPaymentPlan = new CompanyPaymentPlan
            {
                Id = holderPaymentPlan.Id,
                Description = holderPaymentPlan.Description,
                Quotas = holderPaymentPlan.Quotas
            };

            bool existPaymentPlan = paymentPlanPremiumFinance.Exists(u => u == companyPaymentPlan.Id);
            if (existPaymentPlan)
            {
                companyPaymentPlan.PremiumFinance = new CompanyPremiumFinance()
                {
                    NumberQuotas = numberQuotas
                };
            }
            else
            {
                if (numberQuotas > 0)
                {
                    error += string.Format(Errors.ErrorPaymentPlanFinancePremium, holderPaymentPlanId);
                }
            }

            if ((numberQuotas == 0 && existPaymentPlan) || (existPaymentPlan && (numberQuotas < 2 || numberQuotas > 10)))
            {
                error = error.Length > 0 ? error += "| " + string.Format(Errors.ErrorQuotasFinancePremium, quotasDescription, 2, 10) : string.Format(Errors.ErrorQuotasFinancePremium, quotasDescription, 2, 10);
            }

            if (!string.IsNullOrEmpty(error))
            {
                companyPaymentPlan.PremiumFinance = null;
                throw new ValidationException(error);
            }

            return companyPaymentPlan;

        }
        private CompanyClause CreateCompanyClause(Clause clause)
        {
            return new CompanyClause
            {
                Id = clause.Id,
                Name = clause.Name,
                Text = clause.Text,
                Title = clause.Title,
                IsMandatory = clause.IsMandatory,
                ConditionLevel = clause.ConditionLevel
            };
        }

        private CompanySalesPoint CreateCompanySalePoint(SalePoint salePoint)
        {
            return new CompanySalesPoint
            {
                Id = salePoint.Id,
                Description = salePoint.Description,
                IsDefault = salePoint.IsDefault,
                IsEnabled = salePoint.IsEnabled
            };
        }

        public CompanyPolicy CreateCompanyPolicyWithRequest(MassiveEmission massiveLoad, File file, List<FilterIndividual> filtersIndividuals)
        {
            try
            {
                Row row = file.Templates.First(x => x.IsPrincipal).Rows.First();

                CompanyPolicy companyPolicy = new CompanyPolicy
                {
                    TemporalType = TemporalType.Policy,
                    UserId = massiveLoad.User.UserId,
                    IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now),
                    BeginDate = DateTime.Now,
                    Summary = new CompanySummary
                    {
                        RiskCount = 1
                    }
                };

                int billingGroupRow = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BillingGroup));
                int requestGroupRow = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));

                companyPolicy.CurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));

                CompanyRequest companyRequest = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(billingGroupRow, requestGroupRow.ToString(), null).First();

                if (companyRequest == null)
                {
                    throw new ValidationException(Errors.MessageRequestNotFound);
                }

                companyPolicy.Request = new Request()
                {
                    BillingGroupId = billingGroupRow,
                    Id = requestGroupRow
                };
                CompanyRequestEndorsement companyRequestEndorsement = DelegateService.massiveService.GetCompanyRequestEndorsmentPolicyWithRequest(companyPolicy.CurrentFrom, companyRequest);

                if (companyRequestEndorsement == null)
                {
                    throw new ValidationException(Errors.CompanyRequestNotRenewed);
                }
                companyPolicy.RequestEndorsement = companyRequestEndorsement.Id;
                companyPolicy.Holder = DelegateService.massiveService.CreateHolder(row, filtersIndividuals);

                companyPolicy.BusinessType = companyRequest.BusinessType;
                companyPolicy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();

                switch (companyPolicy.BusinessType)
                {
                    case BusinessType.CompanyPercentage:
                        companyPolicy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
                        {
                            ParticipationPercentageOwn = 100
                        });
                        break;
                    default:
                        break;
                }
                if (companyPolicy.CoInsuranceCompanies == null || companyPolicy.CoInsuranceCompanies[0] == null)
                {
                    companyPolicy.BusinessType = BusinessType.CompanyPercentage;
                    companyPolicy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                    companyPolicy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
                    {
                        ParticipationPercentageOwn = 100
                    });
                }

                //DiasVigenciaCotizacion
                companyPolicy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                if (companyRequestEndorsement.IsOpenEffect)
                {
                    double days = (companyRequestEndorsement.CurrentTo - companyRequestEndorsement.CurrentFrom).TotalDays;
                    companyPolicy.CurrentTo = companyPolicy.CurrentFrom.AddDays(days);
                }
                else
                {
                    companyPolicy.CurrentTo = companyRequestEndorsement.CurrentTo;
                }
                companyPolicy.Endorsement.EndorsementDays = (companyPolicy.CurrentTo - companyPolicy.CurrentFrom).Days;

                Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);
                return DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');
                throw new ValidationException(messages[0]);
            }
        }

        /// <summary>
        /// Crear Agencia Principal
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Agencia</returns>
        private IssuanceAgency CreateAgencyPrincipal(MassiveEmission massiveLoad, int productId , decimal standardComission)
        {
            IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentIdAgentAgencyId(massiveLoad.Agency.Agent.IndividualId, massiveLoad.Agency.Id);

            if (agency != null)
            {
                ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, productId);

                agency.IsPrincipal = true;
                agency.Participation = 100;
                agency.Commissions = new List<IssuanceCommission>();
                IssuanceCommission issuanceCommission = new IssuanceCommission();

                if (agency.Agent.IndividualId == agency.Code)
                {
                    issuanceCommission.Percentage = 0;
                    issuanceCommission.PercentageAdditional = 0;
                }
                else
                {
                    if (standardComission > 0)
                    {
                        issuanceCommission.Percentage = standardComission;
                    }
                    else
                    {
                        issuanceCommission.Percentage = productAgencyCommiss.CommissPercentage;
                    }
                    issuanceCommission.PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault();
                }
                agency.Commissions.Add(issuanceCommission);
                return agency;
            }
            else
            {
                throw new ValidationException(Errors.AgentNotFound);
            }
        }

        private void UpdateInsuredByFile(Holder holder, Row row)
        {
            CompanyIssuanceInsured insured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);

            //if (holder.IndividualType == UPENUM.IndividualType.LegalPerson)
            //    UpdateCompanyByFile(insured, row);
            //else if (holder.IndividualType == UPENUM.IndividualType.Person)
            //    UpdatePersonByFile(insured, row);
        }

        //private void UpdatePersonByFile(IssuanceInsured insured, Row row)
        //{

        //    UPMO.Person currentPerson = DelegateService.uniquePersonService.GetPersonByDocumentNumber(insured.IdentificationDocument.Number);
        //    bool personChanged = false;

        //    string names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonName));
        //    if (!string.IsNullOrEmpty(names)
        //        && currentPerson.Names.Trim().ToLower() != names.Trim().ToLower())
        //    {
        //        currentPerson.Names = names;
        //        personChanged = true;
        //    }

        //    string surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSurname));
        //    if (!string.IsNullOrEmpty(surname)
        //        && currentPerson.Surname.Trim().ToLower() != surname.Trim().ToLower())
        //    {
        //        currentPerson.Surname = surname;
        //        personChanged = true;
        //    }

        //    string motherLastName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSecondSurname));
        //    if (!string.IsNullOrEmpty(motherLastName)
        //        && currentPerson.MotherLastName.Trim().ToLower() != motherLastName.Trim().ToLower())
        //    {
        //        currentPerson.MotherLastName = motherLastName;
        //        personChanged = true;
        //    }

        //    int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity));
        //    if (economicActivityId > 0
        //        && currentPerson.EconomicActivity != null && currentPerson.EconomicActivity.Id != economicActivityId)
        //    {
        //        currentPerson.EconomicActivity = new EconomicActivity()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity))
        //        };
        //        personChanged = true;
        //    }

        //    string gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonGender));
        //    if (!string.IsNullOrEmpty(gender)
        //        && currentPerson.Gender != gender)
        //    {
        //        currentPerson.Gender = gender;
        //        personChanged = true;
        //    }

        //    int maritalStatusId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonMaritalStatus));
        //    if (maritalStatusId > 0
        //        && currentPerson.MaritalStatus != null && currentPerson.MaritalStatus.Id != maritalStatusId)
        //    {
        //        currentPerson.MaritalStatus = new MaritalStatus
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonMaritalStatus))
        //        };
        //        personChanged = true;
        //    }

        //    int paymentMethodId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan));
        //    if (paymentMethodId > 0
        //        && currentPerson.PaymentMthodAccount != null
        //        && !currentPerson.PaymentMthodAccount.Any(p => p.PaymentMethod.Id == paymentMethodId))
        //    {
        //        currentPerson.PaymentMthodAccount.Add(new PaymentMethodAccount
        //        {
        //            Id = currentPerson.PaymentMthodAccount.Any(p => p.Id == 1) ? currentPerson.PaymentMthodAccount.Count() + 1 : 1,
        //            PaymentMethod = new PaymentMethod
        //            {
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
        //            }
        //        });
        //        personChanged = true;
        //    }

        //    int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType));
        //    string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription));
        //    int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity));
        //    int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState));
        //    int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry));
        //    if (addressTypeId > 0
        //        && !string.IsNullOrEmpty(addressDescription)
        //        && addresssCityId > 0
        //        && addresssStateId > 0
        //        && addresssCountryId > 0
        //        && !currentPerson.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
        //    {
        //        currentPerson.Addresses.Add(new Address()
        //        {
        //            AddressType = new AddressType()
        //            {
        //                Id = addressTypeId
        //            },
        //            Description = addressDescription,
        //            City = new City()
        //            {
        //                Id = addresssCityId,
        //                State = new State()
        //                {
        //                    Id = addresssStateId,
        //                    Country = new Country
        //                    {
        //                        Id = addresssCountryId
        //                    }
        //                }
        //            }
        //        });
        //        personChanged = true;
        //    }

        //    int phoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType));
        //    string phoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription));
        //    if (phoneTypeId > 0 && !string.IsNullOrEmpty(phoneDescription)
        //        && !currentPerson.Phones.Any(p => p.Description.Trim() == phoneDescription.Trim()))
        //    {
        //        currentPerson.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = phoneTypeId
        //            },
        //            Description = phoneDescription
        //        });
        //        personChanged = true;
        //    }

        //    int emailType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType));
        //    string emailDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription));

        //    if (emailType > 0 && !string.IsNullOrEmpty(emailDescription)
        //        && !currentPerson.Emails.Any(e => e.Description.Trim().ToLower() == emailDescription.Trim().ToLower()))
        //    {
        //        currentPerson.Emails.Add(new Email()
        //        {
        //            EmailType = new EmailType()
        //            {
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType))
        //            },
        //            Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
        //        });
        //        personChanged = true;
        //    }

        //    if (personChanged)
        //        DelegateService.uniquePersonService.UpdatePerson(currentPerson, null);
        //}

        //private void UpdateCompanyByFile(IssuanceInsured insured, Row row)
        //{
        //    UniquePersonServices.Models.Company currentCompany = DelegateService.uniquePersonService.GetCompanyByDocumentNumber(insured.IdentificationDocument.Number);
        //    bool companyChanged = false;

        //    string name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyTradeName));
        //    if (!string.IsNullOrEmpty(name)
        //        && currentCompany.Name.Trim().ToLower() != name.Trim().ToLower())
        //    {
        //        currentCompany.Name = name;
        //        companyChanged = true;
        //    }

        //    int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity));
        //    if (economicActivityId > 0
        //        && currentCompany.EconomicActivity != null && currentCompany.EconomicActivity.Id != economicActivityId)
        //    {
        //        currentCompany.EconomicActivity = new EconomicActivity()
        //        {
        //            Id = economicActivityId
        //        };
        //        companyChanged = true;
        //    }

        //    int companyTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyType));
        //    if (companyTypeId > 0
        //        && currentCompany.CompanyType != null && currentCompany.CompanyType.Id != companyTypeId)
        //    {
        //        currentCompany.CompanyType = new CompanyType()
        //        {
        //            Id = companyTypeId
        //        };
        //        companyChanged = true;
        //    }

        //    int companyCountryOriginId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry));
        //    if (companyCountryOriginId > 0
        //        && currentCompany.CountryOrigin != null && currentCompany.CountryOrigin.Id != companyCountryOriginId)
        //    {
        //        currentCompany.CountryOrigin = new Country
        //        {
        //            Id = companyCountryOriginId
        //        };
        //        companyChanged = true;
        //    }

        //    int companyPaymentMethodId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan));
        //    if (companyPaymentMethodId > 0
        //        && currentCompany.PaymentMethodAccount != null
        //        && !currentCompany.PaymentMethodAccount.Any(p => p.PaymentMethod.Id == companyPaymentMethodId))
        //    {
        //        currentCompany.PaymentMethodAccount.Add(new PaymentMethodAccount
        //        {
        //            Id = currentCompany.PaymentMethodAccount.Any(p => p.PaymentMethod.Id == 1) ? currentCompany.PaymentMethodAccount.Count() + 1 : 1,
        //            PaymentMethod = new PaymentMethod
        //            {
        //                Id = companyPaymentMethodId
        //            }
        //        });
        //        companyChanged = true;
        //    }

        //    int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType));
        //    string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription));
        //    int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity));
        //    int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState));
        //    int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry));
        //    if (addressTypeId > 0
        //        && !string.IsNullOrEmpty(addressDescription)
        //        && addresssCityId > 0
        //        && addresssStateId > 0
        //        && addresssCountryId > 0
        //        && currentCompany.Addresses != null
        //        && !currentCompany.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
        //    {
        //        currentCompany.Addresses.Add(new Address()
        //        {
        //            AddressType = new AddressType()
        //            {
        //                Id = addressTypeId
        //            },
        //            Description = addressDescription,
        //            City = new City()
        //            {
        //                Id = addresssCityId,
        //                State = new State()
        //                {
        //                    Id = addresssStateId,
        //                    Country = new Country
        //                    {
        //                        Id = addresssCountryId
        //                    }
        //                }
        //            }
        //        });
        //        companyChanged = true;
        //    }

        //    int companyPhoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType));
        //    string companyPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription));
        //    if (companyPhoneTypeId > 0 && !string.IsNullOrEmpty(companyPhoneDescription)
        //        && currentCompany.Phones != null
        //        && !currentCompany.Phones.Any(p => p.Description.Trim() == companyPhoneDescription.Trim()))
        //    {
        //        currentCompany.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = companyPhoneTypeId
        //            },
        //            Description = companyPhoneDescription
        //        });
        //    }

        //    string companyCellPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription));
        //    if (!string.IsNullOrEmpty(companyCellPhoneDescription)
        //        && currentCompany.Phones != null
        //        && !currentCompany.Phones.Any(p => p.Description.Trim() == companyCellPhoneDescription.Trim()))
        //    {
        //        currentCompany.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = 4
        //            },
        //            Description = companyCellPhoneDescription
        //        });
        //    }

        //    if (companyChanged)
        //        DelegateService.uniquePersonService.UpdateCompany(currentCompany);
        //}

        /// <summary>
        /// Crear Tomador Compañia
        /// </summary>
        /// <param name="row">Datos</param>
        /// <returns>Tomador</returns>
        //private Holder CreateHolderCompany(Row row)
        //{
        //    UPMO.Company company = new UPMO.Company
        //    {
        //        IndividualType = UPEN.IndividualType.LegalPerson,
        //        Name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyTradeName)),
        //        EconomicActivity = new EconomicActivity()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity))
        //        },
        //        IdentificationDocument = new IdentificationDocument()
        //        {
        //            DocumentType = new DocumentType()
        //            {
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentType))
        //            },
        //            Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber))
        //        },
        //        CompanyType = new CompanyType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyType))
        //        },
        //        CountryOrigin = new Country
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry))
        //        },
        //        PaymentMethodAccount = new List<PaymentMethodAccount>(),
        //        Addresses = new List<Address>(),
        //        Phones = new List<Phone>(),
        //        Emails = new List<Email>()
        //    };

        //    company.PaymentMethodAccount.Add(new PaymentMethodAccount
        //    {
        //        Id = 1,
        //        PaymentMethod = new PaymentMethod
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
        //        }
        //    });

        //    company.Addresses.Add(new Address()
        //    {
        //        AddressType = new AddressType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription)),
        //        City = new City()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity)),
        //            State = new State()
        //            {
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState)),
        //                Country = new Country
        //                {
        //                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry))
        //                }
        //            }
        //        }
        //    });

        //    company.Phones.Add(new Phone()
        //    {
        //        PhoneType = new PhoneType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
        //    });

        //    ///Inicio cargue masivo Company
        //    company.Phones.Add(new Phone()
        //    {
        //        PhoneType = new PhoneType()
        //        {
        //            Id = 4
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.HolderCellNumber))
        //    });
        //    ///Fin cargue masivo Company

        //    company.Emails.Add(new Email()
        //    {
        //        EmailType = new EmailType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
        //    });

        //    company = DelegateService.uniquePersonService.CreateCompany(company);

        //    //Validar modelo insured

        //    CompanyInsured companyInsured = CreateInsuredByCompany(company);

        //    return CreateHolderByInsuredCompany(companyInsured, company);
        //}

        private CompanyIssuanceInsured CreateInsuredByCompany(MassiveCompany company)
        {

            CompanyIssuanceInsured insured = new CompanyIssuanceInsured
            {
                IndividualId = company.IndividualId,
                IdentificationDocument = new IssuanceIdentificationDocument
                {
                    Number = company.DocumentNumber,
                    DocumentType = new IssuanceDocumentType
                    {
                        Id = company.DocumentTypeId
                    }
                },
                Name = company.FullName,
                EnteredDate = DateTime.Now,
                Profile = 2,
                PaymentMethod = new IssuancePaymentMethod
                {
                    Id = company.PaymentMethodId,
                    PaymentId = company.PaymentId
                },
            };
            return insured;
        }

        //private Holder CreateHolderByInsuredCompany(CompanyInsured insured, UPMO.Company company)
        //{
        //    Holder holder = new Holder
        //    {
        //        IndividualId = company.IndividualId,
        //        InsuredId = insured.Id,
        //        //IdentificationDocument = company.IdentificationDocument,
        //        //PaymentMethod = new IndividualPaymentMethod
        //        //{
        //        //    Id = company.PaymentMethodAccount[0].PaymentMethod.Id,
        //        //    PaymentId = company.PaymentMethodAccount[0].Id
        //        //},
        //        //CompanyName = new CompanyName()

        //    };

        //    //if (company.Addresses != null)
        //    //{
        //    //    holder.CompanyName.Address = company.Addresses.First();
        //    //}
        //    //if (company.Phones != null)
        //    //{
        //    //    holder.CompanyName.Phone = company.Phones.First();
        //    //}
        //    //if (company.Emails != null)
        //    //{
        //    //    holder.CompanyName.Email = company.Emails.First();
        //    //}
        //    return holder;
        //}

        //private Holder CreateHolderPerson(Row row)
        //{
        //    UPMO.Person person = new UPMO.Person
        //    {
        //        IndividualType = UPEN.IndividualType.Person,
        //        Names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonName)),
        //        Surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSurname)),
        //        MotherLastName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSecondSurname)),
        //        EconomicActivity = new EconomicActivity()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity))
        //        },
        //        IdentificationDocument = new IdentificationDocument()
        //        {
        //            DocumentType = new DocumentType()
        //            {
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentType))
        //            },
        //            Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentNumber))
        //        },
        //        Gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonGender)),
        //        MaritalStatus = new MaritalStatus
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonMaritalStatus))
        //        },
        //        BirthDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonBirthDate)),
        //        EducativeLevel = new EducativeLevel
        //        {
        //            Id = 1
        //        },
        //        HouseType = new HouseType
        //        {
        //            Id = 1
        //        },
        //        SocialLayer = new SocialLayer
        //        {
        //            Id = 1
        //        },
        //        LaborPerson = new LaborPerson
        //        {
        //            Id = 1,
        //            Occupation = new Occupation()
        //        },
        //        PaymentMthodAccount = new List<PaymentMethodAccount>(),
        //        Addresses = new List<Address>(),
        //        Phones = new List<Phone>(),
        //        Emails = new List<Email>()
        //    };

        //    person.PaymentMthodAccount.Add(new PaymentMethodAccount
        //    {
        //        Id = 1,
        //        PaymentMethod = new PaymentMethod
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
        //        }
        //    });

        //    person.Addresses.Add(new Address()
        //    {
        //        AddressType = new AddressType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription)),
        //        City = new City()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity)),
        //            State = new State()
        //            {
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState)),
        //                Country = new Country
        //                {
        //                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry))
        //                }
        //            }
        //        }
        //    });

        //    person.Phones.Add(new Phone()
        //    {
        //        PhoneType = new PhoneType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
        //    });

        //    ///Inicio cargue masivo Company
        //    person.Phones.Add(new Phone()
        //    {
        //        PhoneType = new PhoneType()
        //        {
        //            Id = 4
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.HolderCellNumber))
        //    });
        //    ///Fin cargue masivo Company

        //    person.Emails.Add(new Email()
        //    {
        //        EmailType = new EmailType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
        //    });

        //    person = DelegateService.uniquePersonService.CreatePerson(person, null);

        //    //CREAR ASEGURADO
        //    CompanyInsured companyInsured = CreateInsuredByPerson(person, row);

        //    return CreateHolderByInsuredPerson(companyInsured, person);
        //}

        //private Sistran.Company.Application.UniquePersonServices.Models.CompanyInsured CreateInsuredByPerson(UPMO.Person person, Row row)
        //{
        //    bool isCommercialClient = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCommercialAgreement));
        //    bool isMailAddress = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSendingMail));
        //    bool isSms = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSMSSending));

        //    Sistran.Company.Application.UniquePersonServices.Models.CompanyInsured insured = new Sistran.Company.Application.UniquePersonServices.Models.CompanyInsured
        //    {
        //        IndividualId = person.IndividualId,
        //        IdentificationDocument = person.IdentificationDocument,
        //        Name = StringHelper.ConcatenateString(person.Names, " ", person.Surname, " ", person.MotherLastName),
        //        EnteredDate = DateTime.Now,
        //        Profile = 2,
        //        BranchCode = 1,
        //        //IsCommercialClient = isCommercialClient == true ? 1 : 0,
        //        //IsMailAddress = isMailAddress == true ? 1 : 0,
        //        //IsSms = isSms == true ? 1 : 0,
        //    };

        //    insured = DelegateService.uniquePersonService.CreateCompanyInsured(insured);
        //    return insured;
        //}

        //private Holder CreateHolderByInsuredPerson(CompanyInsured insured, UPMO.Person person)
        //{
        //    Holder holder = new Holder
        //    {
        //        IndividualId = person.IndividualId,
        //        InsuredId = insured.InsuredId,
        //        //IdentificationDocument = person.IdentificationDocument,
        //        //PaymentMethod = new IndividualPaymentMethod
        //        //{
        //        //    Id = person.PaymentMthodAccount[0].PaymentMethod.Id,
        //        //    PaymentId = person.PaymentMthodAccount[0].Id
        //        //},
        //        //CompanyName = new CompanyName
        //        //{
        //        //    Address = person.Addresses.First(),
        //        //    Phone = person.Phones.First(),
        //        //    Email = person.Emails.First()
        //        //}
        //    };

        //    return holder;
        //}

        private bool ExistsHolderInExcel(Row row)
        {
            int individualId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderInsuredCode));
            string holderDocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber));
            if (string.IsNullOrEmpty(holderDocumentNumber))
                holderDocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentNumber));

            return (individualId > 0 || !string.IsNullOrEmpty(holderDocumentNumber));
        }

        private List<IssuanceAgency> CreateAgencies(CompanyRequestEndorsement companyRequestEndorsement)
        {
            List<IssuanceAgency> agencies = new List<IssuanceAgency>();
            for (int i = 0; i < companyRequestEndorsement.Agencies.Count; i++)
            {
                //validar metodo
                IssuanceAgency item = companyRequestEndorsement.Agencies[i];
                IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentIdAgentAgencyId(item.Agent.IndividualId, item.Id);
                agency.Commissions = new List<IssuanceCommission>();

                if (i == 0)
                    agency.IsPrincipal = true;
                agency.Participation = 100 / companyRequestEndorsement.Agencies.Count;
                agencies.Add(agency);
            }
            return agencies;
        }

        

        

        //private List<DynamicConcept> CreateDynamicConcepts(Template dynamicConceptsTemplate)
        //{
        //    List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();
        //    foreach (Row row in dynamicConceptsTemplate.Rows)
        //    {
        //        dynamicConcepts.Add(new DynamicConcept()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Script)),
        //            EntityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Entity)),
        //            //asd = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Question)),
        //            //asd = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Answer)),
        //        });
        //    }
        //    return dynamicConcepts;
        //}


        //private UPMO.Company CreateCompanyForInsured(Row row)
        //{
        //    UPMO.Company company = new UPMO.Company();

        //    Country country = new Country()
        //    {
        //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry))
        //    };

        //    company.Addresses = new List<Address>();
        //    company.Addresses.Add(new Address()
        //    {
        //        AddressType = new AddressType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressType))
        //        },

        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressDescription)),
        //        City = new City()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity)),
        //            State = new State()
        //            {
        //                Country = country,
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState))
        //            }
        //        }
        //    });

        //    company.CountryOrigin = country;

        //    company.CompanyType = new CompanyType()
        //    {
        //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyType))
        //    };
        //    company.IdentificationDocument = new IdentificationDocument()
        //    {
        //        DocumentType = new DocumentType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType))
        //        },
        //        Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentNumber))
        //    };
        //    company.EconomicActivity = new EconomicActivity()
        //    {
        //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity))
        //    };
        //    company.Name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyTradeName));
        //    //exonerationType
        //    company.Phones = new List<Phone>();
        //    company.Phones.Add(new Phone()
        //    {
        //        PhoneType = new PhoneType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType))

        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription))
        //    });
        //    company.Emails = new List<Email>();
        //    company.Emails.Add(new Email()
        //    {
        //        EmailType = new EmailType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription))
        //    });

        //    company = DelegateService.uniquePersonService.CreateCompany(company);
        //    return company;
        //}

        //private UPMO.Person CreatePersonForInsured(Row row)
        //{
        //    Company.Application.UniquePersonServices.Models.Person person = new UniquePersonServices.Models.Person();
        //    person.Addresses = new List<Address>();
        //    person.Addresses.Add(new Address()
        //    {
        //        AddressType = new AddressType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressType))
        //        },

        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressDescription)),
        //        City = new City()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity)),
        //            State = new State()
        //            {
        //                Country = new Country()
        //                {
        //                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry))
        //                },
        //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState))
        //            }
        //        }
        //    });

        //    person.Surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonSurname));
        //    person.SecondSurname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonSecondSurname));
        //    person.Names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonName));
        //    person.Gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonGender));
        //    person.MaritalStatus = new MaritalStatus()
        //    {
        //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus))
        //    };
        //    person.BirthDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonBirthDate));

        //    person.IdentificationDocument = new IdentificationDocument()
        //    {
        //        DocumentType = new DocumentType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType))
        //        },
        //        Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentNumber))
        //    };
        //    person.EconomicActivity = new EconomicActivity()
        //    {
        //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity))
        //    };
        //    person.Phones = new List<Phone>();
        //    person.Phones.Add(new Phone()
        //    {
        //        PhoneType = new PhoneType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType))

        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription))
        //    });
        //    person.Emails = new List<Email>();
        //    person.Emails.Add(new Email()
        //    {
        //        EmailType = new EmailType()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType))
        //        },
        //        Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription))
        //    });
        //    person.EducativeLevel = new EducativeLevel { Id = 1 };
        //    person.HouseType = new HouseType { Id = 1 };
        //    person.SocialLayer = new SocialLayer { Id = 1 };
        //    person.LaborPerson = new LaborPerson { Id = 1, Occupation = new Occupation { Id = 0 } };
        //    person.PaymentMthodAccount = new List<PaymentMethodAccount>();
        //    person.PaymentMthodAccount.Add(new PaymentMethodAccount { PaymentMethod = new PaymentMethod { Id = 1 }, AccountType = new PaymentAccountType { Id = 1 } });

        //    person = DelegateService.uniquePersonService.CreatePerson(person, null);
        //    return person;
        //}

        public MassiveLoad IssuanceMassiveEmission(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);

            if (massiveLoad != null)
            {
                List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, MassiveLoadProcessStatus.Events, false, false);

                if (massiveEmissionRows.Count > 0)
                {
                    massiveLoad.Status = MassiveLoadStatus.Issuing;
                    massiveLoad.TotalRows = massiveEmissionRows.Count + DelegateService.massiveUnderwritingService.GetFinalizedMassiveEmissionRowsByMassiveLoadId(massiveLoadId).Count;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                    ParallelHelper.ForEach(massiveEmissionRows, ExecuteCreatePolicy);

                    massiveLoad.Status = MassiveLoadStatus.Issued;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                }
                else
                {
                    throw new ValidationException(Errors.ErrorRecordsNotFoundToIssue);
                }
            }

            return massiveLoad;
        }

        private void ExecuteCreatePolicy(MassiveEmissionRow massiveEmissionRow)
        {
            try
            {
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Issuance;
                if (massiveEmissionRow.Risk.Policy.Id > 0)
                {
                    massiveEmissionRow.Status = MassiveLoadProcessStatus.Finalized;
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
                massiveEmissionRow.Observations = string.Format(Errors.ErrorIssuing, ex.Message);
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private IssuanceCoInsuranceCompany CreateCoInsuranceCompany(Row row)
        {
            IssuanceCoInsuranceCompany coInsuranceCompany = new IssuanceCoInsuranceCompany();

            coInsuranceCompany.Id = Convert.ToInt32(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoInsuranceCompanyId).Value);
            coInsuranceCompany.PolicyNumber = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoInsuranceCompanyPolicyNumber).Value.ToString();
            coInsuranceCompany.EndorsementNumber = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoInsuranceCompanyEndorsementNumber).Value.ToString();
            coInsuranceCompany.ParticipationPercentageOwn = Convert.ToDecimal(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoInsuranceCompanyParticipationPercentageOwn).Value);
            coInsuranceCompany.ParticipationPercentage = Convert.ToDecimal(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoInsuranceCompanyParticipationPercentage).Value);
            coInsuranceCompany.ExpensesPercentage = Convert.ToDecimal(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoInsuranceCompanyExpensesPercentage).Value);

            return coInsuranceCompany;
        }

        public CompanyPolicy GetPolicyByOperationId(int policyOperationId)
        {
            return DelegateService.underwritingService.CompanyGetPolicyByTemporalId(policyOperationId, false);
        }

        public List<CompanyPolicy> GetCompanyPoliciesToIssueByOperationIds(List<int> policiesOperationIds)
        {
            ConcurrentBag<CompanyPolicy> companyPolicies = new ConcurrentBag<CompanyPolicy>();

            ParallelHelper.ForEach(policiesOperationIds, policiesOperationId =>
            {
                companyPolicies.Add(GetPolicyByOperationId(policiesOperationId));
            });

            if (Settings.ImplementValidate2G())
            {
                var companyPoliciesByGroup = from r in companyPolicies
                                             orderby r.Id
                                             group r by r.Branch.Id into grp
                                             select new { key = grp.Key, cnt = grp.Count() };
                // SERVICIO SYBASE AXA
                //ParallelHelper.ForEach(companyPoliciesByGroup.ToList(), item =>
                //{
                //    ProcessPolicy processPolicy = new ProcessPolicy();
                //    processPolicy.BranchId = item.key;
                //    processPolicy.Count_Policies = item.cnt;
                //    processPolicy = DelegateService.syBaseEntityService.GetKeyPolicy2G(processPolicy);
                //    foreach (CompanyPolicy companyPolicy in companyPolicies.Where(x => x.Branch.Id == item.key))
                //    {
                //        companyPolicy.Policy2G = processPolicy.Id_Ini;
                //        processPolicy.Id_Ini++;
                //    }
                //});
            }
            return companyPolicies.ToList();
        }

        public CompanyPolicy GetCompanyPolicyToIssueByOperationId(int policyOperationId)
        {
            CompanyPolicy companyPolicy = GetPolicyByOperationId(policyOperationId);
            //SERVICIO SYBASE AXA
            //if (Settings.ImplementValidate2G())
            //{
            //    ProcessPolicy processPolicy = new ProcessPolicy();
            //    processPolicy.BranchId = companyPolicy.Branch.Id;
            //    processPolicy.Count_Policies = 1;
            //    processPolicy = DelegateService.syBaseEntityService.GetKeyPolicy2G(processPolicy);
            //    companyPolicy.Policy2G = processPolicy.Id_Ini;
            //}
            return companyPolicy;
        }
    }
}