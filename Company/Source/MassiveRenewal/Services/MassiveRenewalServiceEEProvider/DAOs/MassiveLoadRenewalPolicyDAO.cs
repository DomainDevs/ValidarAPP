using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveRenewalServices.EEProvider.Resources;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;

using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.MassiveRenewalServices.EEProvider.DAOs
{
    public class MassiveLoadRenewalPolicyDAO
    {
        public CompanyPolicy CreateCompanyPolicy(File file, string templateName, int userId, int selectedPrefixId)
        {
            Template principalTemplate = file.Templates.Find(x => x.PropertyName == templateName);
            Row principalRow = principalTemplate != null ? principalTemplate.Rows.First() : null;
            if (principalRow == null)
            {
                throw new ValidationException(string.Format(Errors.PrincipalRowNotFound, templateName));
            }
            var prefixId = GetFieldValueByRowPropertyName<int>(principalRow, FieldPropertyName.PrefixCode, templateName);
            if (prefixId != selectedPrefixId)
            {
                throw new ValidationException(Errors.CurrentPrefixIsDifferentFromSelectedPrefix);
            }
            var branchId = GetFieldValueByRowPropertyName<int>(principalRow, FieldPropertyName.BranchId, templateName);
            var policyNumber = GetFieldValueByRowPropertyName<decimal>(principalRow, FieldPropertyName.PolicyNumberRenewal, templateName);
            var policyCurrentTo = GetFieldValueByRowPropertyName<DateTime>(principalRow, FieldPropertyName.PolicyCurrentTo, templateName);
            var policyText = GetFieldValueByRowPropertyName<string>(principalRow, FieldPropertyName.PolicyText, templateName);
            var billingGroup = GetFieldValueByRowPropertyName<int>(principalRow, FieldPropertyName.BillingGroup, templateName);
            var requestGroup = GetFieldValueByRowPropertyName<int>(principalRow, FieldPropertyName.RequestGroup, templateName);
            // TODO definir en donde se debe dejar el valor de ésta prima
            var premiumValue = GetFieldValueByRowPropertyName<decimal>(principalRow, FieldPropertyName.PremiumAmount, templateName);
            var agentCode = GetFieldValueByRowPropertyName<int>(principalRow, FieldPropertyName.AgentCode, templateName);
            var agentTypeCode = GetFieldValueByRowPropertyName<int>(principalRow, FieldPropertyName.AgentType, templateName);
            bool hasGroupingRequest = billingGroup > 0 && requestGroup > 0;
            string policyValidationErrors = ValidateDataForPolicySearch(branchId, prefixId, policyNumber);
            if (!string.IsNullOrEmpty(policyValidationErrors))
            {
                throw new ValidationException(policyValidationErrors);
            }
            Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            if (policy == null)
            {
                throw new ValidationException(Errors.PolicyNotFound);
            }
            int riskCount = DelegateService.underwritingService.GetCurrentRiskNumByPolicyId(policy.Endorsement.PolicyId);
            if (riskCount != 1)
            {
                throw new ValidationException(Errors.PolicyIsNotMassive);
            }
            if (policy.Endorsement == null || !policy.Endorsement.EndorsementType.HasValue
                || policy.Endorsement.EndorsementType.Value == EndorsementType.Cancellation
                || policy.Endorsement.EndorsementType.Value == EndorsementType.AutomaticCancellation
                || policy.Endorsement.EndorsementType.Value == EndorsementType.Nominative_cancellation)
            {
                throw new ValidationException(Errors.NullOrCancelledPolicyEndorsement);
            }
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(policy.Endorsement.Id);
            
            
            if (companyPolicy == null)
            {
                throw new ValidationException(Errors.CompanyPolicyNotFound);
            }
            if (companyPolicy.Request == null && hasGroupingRequest)
            {
                throw new ValidationException(Errors.PolicyRequestIsNull);
            }
            if (companyPolicy.Request != null && !hasGroupingRequest)
            {
                throw new ValidationException(string.Format(Errors.PolicyRelatedToGroupingCode, companyPolicy.Request.BillingGroupId, companyPolicy.Request.Id));
            }
            if (!hasGroupingRequest && policyCurrentTo == default(DateTime))
            {
                throw new ValidationException(Errors.PolicyCurrentToIsEmpty);
            }
            

            var config = new MapperConfiguration(cfg =>{cfg.CreateMap<PolicyType, CompanyPolicyType>();}).CreateMapper();
            companyPolicy.PolicyType = config.Map<PolicyType, CompanyPolicyType>(DelegateService.commonService.GetPolicyTypesByPrefixIdById(companyPolicy.Prefix.Id, companyPolicy.PolicyType.Id));
            
            companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsMassive = true;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.TemporalType = TemporalType.Policy;
            companyPolicy.UserId = userId;
            companyPolicy.SubMassiveProcessType = SubMassiveProcessType.MassiveRenewal;
            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);           
            companyPolicy.Product.StandardCommissionPercentage = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(principalRow.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.StandardCommissionPercentage));
            companyPolicy.Endorsement.TicketNumber = (int)DelegateService.utilitiesService.GetValueByField<int>(principalRow.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));
            companyPolicy.Endorsement.TicketDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(principalRow.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
            companyPolicy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
            companyPolicy.Text = new CompanyText();
            companyPolicy.Text.TextBody = (string)DelegateService.utilitiesService.GetValueByField<string>(principalRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));

            companyPolicy.Clauses = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.General, companyPolicy.Prefix.Id, null);
            List<CompanyClause> clauses = DelegateService.massiveService.GetClauses(file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses), EmissionLevel.General);

            foreach (var item in clauses)
            {
                if (!companyPolicy.Clauses.Exists(u => u.Id == item.Id))
                {
                    companyPolicy.Clauses.Add(item);
                }
            }

            companyPolicy.Summary = new CompanySummary
            {
                RiskCount = 1
            };
            
            if (hasGroupingRequest)
            {
                FillPolicyWithRequest(companyPolicy, billingGroup, requestGroup, policyText);
            }
            else
            {
                FillPolicyWithoutRequest(file, companyPolicy, policyCurrentTo, policyText);
            }

            if (agentCode > 0 && agentTypeCode > 0)
            {
                IssuanceAgency newPrincipalAgency = GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeCode);
                string validationError = ValidateAgency(newPrincipalAgency, companyPolicy.Prefix.Id, companyPolicy.Product.Id, companyPolicy.Branch.Id);
                if (!string.IsNullOrEmpty(validationError))
                {
                    throw new ValidationException(validationError);
                }
                
                IssuanceAgency oldPrincipalAgency = companyPolicy.Agencies.Find(x => x.IsPrincipal);
                if (oldPrincipalAgency == null)
                {
                    throw new ValidationException(Errors.PrincipalAgencyNotFound);
                }
                companyPolicy.Agencies.RemoveAll(x => x.IsPrincipal);
                newPrincipalAgency.Participation = 100 - companyPolicy.Agencies.Sum(x => x.Participation);
                newPrincipalAgency.IsPrincipal = true;
                //Validar mapeo
                newPrincipalAgency.Commissions = oldPrincipalAgency.Commissions;
                newPrincipalAgency.Commissions[0].Percentage = (decimal)companyPolicy.Product.StandardCommissionPercentage == 0 ? (decimal)oldPrincipalAgency.Commissions[0].Percentage : (decimal)companyPolicy.Product.StandardCommissionPercentage;
                companyPolicy.Agencies.Add(newPrincipalAgency);
                Template templateAdditionalIntermediaries = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries);
                if (templateAdditionalIntermediaries != null)
                {
                    string error = string.Empty;
                    List<IssuanceAgency> issuanceAgencies = DelegateService.massiveService.GetAgenciesValidation(file, companyPolicy.Agencies, ref error);
                    if (string.IsNullOrEmpty(error))
                    {
                        companyPolicy.Agencies = issuanceAgencies;
                    }
                    else
                    {
                        throw new ValidationException(error);
                    }
                }

            }

            if (companyPolicy.Agencies.Sum(x => x.Participation) != 100)
            {
                throw new ValidationException(Errors.AgenciesTotalParticipationIsNot100);
            }

            companyPolicy.PaymentPlan = GetPaymentPlan(companyPolicy, principalRow);           
            companyPolicy.CalculateMinPremium = companyPolicy.Product.CalculateMinPremium;
            templateName = "";

            return companyPolicy;
        }
        public MassiveLoad IssuanceMassiveEmission(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);

            if (massiveLoad != null)
            {
                List<MassiveRenewalRow> massiveRenewalRows = DelegateService.massiveRenewalService.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveLoadId, MassiveLoadProcessStatus.Tariff);

                if (massiveRenewalRows.Count > 0)
                {
                    massiveLoad.Status = MassiveLoadStatus.Issuing;
                    massiveLoad.TotalRows = massiveRenewalRows.Count;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                    ParallelHelper.ForEach(massiveRenewalRows, ExecuteCreatePolicyRenewal);

                    massiveLoad.Status = MassiveLoadStatus.Issued;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                }
            }

            return massiveLoad;
        }

        #region Métodos privados creación de póliza de renovación
        private string ValidateDataForPolicySearch(int branchId, int prefixId, decimal policyNumber)
        {
            List<string> errors = new List<string>();
            if (branchId == 0)
            {
                errors.Add(Errors.PolicyBranchIdIsEmpty);
            }
            if (prefixId == 0)
            {
                errors.Add(Errors.PolicyPrefixIdIsEmpty);
            }
            if (policyNumber == 0)
            {
                errors.Add(Errors.PolicyDocumentNumberIsEmpty);
            }
            return string.Join(",", errors);
        }
        private void FillPolicyWithoutRequest(File file, CompanyPolicy companyPolicy, DateTime policyCurrentTo, string policyText)
        {

            if ((policyCurrentTo - companyPolicy.CurrentTo).Days < 1)
            {
                throw new ValidationException(Errors.ErrorDateToNotValid);
            }
            var additionalIntermediariesTemplate = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries);

            List<string> validationErrors = IncludeAdditionalAgencies(companyPolicy.Agencies, additionalIntermediariesTemplate, companyPolicy.Product.Id, companyPolicy.Prefix.Id, companyPolicy.Branch.Id);
            if (validationErrors.Any())
            {
                string errors = string.Join("||", validationErrors);
                throw new ValidationException(string.Format(Errors.AdditionalIntermediariesValidationErrors, errors));
            }
            decimal? StandardCommissionPercentage = companyPolicy.Product.StandardCommissionPercentage;
              companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
            if (companyPolicy.Product.StandardCommissionPercentage != StandardCommissionPercentage && StandardCommissionPercentage!= 0)
            {
                companyPolicy.Product.StandardCommissionPercentage = StandardCommissionPercentage;
            }
            else
            {
                companyPolicy.Product.StandardCommissionPercentage = companyPolicy.Product.StandardCommissionPercentage;
            }

            companyPolicy.CurrentTo = policyCurrentTo;
            companyPolicy.Text = new CompanyText
            {
                TextBody = policyText
            };
            if (companyPolicy.Agencies.GroupBy(a => a.Code, agen => agen).Select(x => x.First()).Count() != companyPolicy.Agencies.Count)
            {
                throw new ValidationException(Errors.MessageAgentsDuplicated);
            }

        }

        private CompanyPaymentPlan GetPaymentPlan(CompanyPolicy companyPolicy , Row row  )
        {

            List<int> paymentPlanPremiumFinance = new List<int> { 38, 39, 40 }; //TODO: se debe es modificar la tabla de plan de pagos para que tenga una marca para saber si aplica o no financiamiento
            List<PaymentPlan> paymentPlans = DelegateService.underwritingService.GetPaymentPlansByProductId(companyPolicy.Product.Id);

            int holderPaymentPlanId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan));
            int numberQuotas = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PremiumFinanceQuotas));
            string quotasDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PremiumFinanceQuotas).Description;
            string error = "";

            if (paymentPlans != null && paymentPlans.Count == 0)
            {
                throw new ValidationException(Errors.PaymentPlanIsNull);
            }

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
                if (companyPolicy.PaymentPlan == null)
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
                else
                {
                    return companyPolicy.PaymentPlan;
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
                error = error.Length > 0 ? error += "| " + string.Format(Errors.ErrorQuotasFinancePremium,quotasDescription, 2, 10) : string.Format(Errors.ErrorQuotasFinancePremium, quotasDescription, 2, 10);
            }

            if (!string.IsNullOrEmpty(error))
            {
                companyPaymentPlan.PremiumFinance = null;
                throw new ValidationException(error);
            }

            return companyPaymentPlan;

        }
        private void FillPolicyWithRequest(CompanyPolicy companyPolicy, int billingGroup, int requestGroup, string policyText)
        {
            if (companyPolicy.Request.BillingGroupId != billingGroup || companyPolicy.Request.Id != requestGroup)
            {
                throw new ValidationException(Errors.InvalidBillingGroupOrRequestGroup);
            }

            CompanyRequest companyRequest = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(companyPolicy.Request.BillingGroupId, companyPolicy.Request.Id.ToString(), null).FirstOrDefault();
            CompanyRequestEndorsement companyRequestEndorsement = DelegateService.massiveService.GetCompanyRequestEndorsmentPolicyWithRequest(companyPolicy.CurrentFrom, companyRequest);

            if (companyRequestEndorsement == null)
            {
                throw new ValidationException(Errors.CompanyRequestNotRenewed);
            }
            //Validar modelo
            //companyPolicy.BillingGroup = companyRequest.BillingGroup;
            //companyPolicy.Branch = companyRequest.Branch;
            companyPolicy.Holder = companyRequestEndorsement.Holder;
            //companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
            if (companyPolicy.Holder == null)
            {
                throw new ValidationException("HolderIsNull");
            }
            //List<CompanyName> companyNames = DelegateService.uniquePersonService.CompanyGetNotificationAddressesByIndividualId(companyPolicy.Holder.IndividualId, CustomerType.Individual);
            //Valiar conversi+on de CompanyName a IssuanceCompanyName
            //companyPolicy.Holder.CompanyName = companyNames.First(x => x.IsMain);
            //companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyRequestEndorsement.Product, companyRequestEndorsement.Prefix.Id);
            //companyPolicy.Agencies = CreateAgencies(companyRequestEndorsement.Agencies, companyRequestEndorsement.Product.Id);
            //companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(companyRequestEndorsement.PaymentPlan.Id);
            //companyPolicy.PolicyType = DelegateService.commonService.GetPolicyTypesByPrefixIdById(companyPolicy.Prefix.Id, companyRequestEndorsement.PolicyType.Id);
            companyPolicy.RequestEndorsement = companyRequestEndorsement.Id;
            companyPolicy.Text = new CompanyText
            {
                TextBody = companyRequestEndorsement.Annotations + (string.IsNullOrWhiteSpace(policyText) ? "" : Environment.NewLine + policyText)
            };
            if (companyRequestEndorsement.IsOpenEffect)
            {
                double days = (companyRequestEndorsement.CurrentTo - companyRequestEndorsement.CurrentFrom).TotalDays;
                companyPolicy.CurrentTo = companyPolicy.CurrentFrom.AddDays(days);
            }
            else
            {
                companyPolicy.CurrentTo = companyRequestEndorsement.CurrentTo;
            }
            switch (companyPolicy.BusinessType)
            {
                case BusinessType.CompanyPercentage:
                    companyPolicy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
                    {
                        ParticipationPercentageOwn = 100
                    });
                    break;

                default:
                    //Validar referencia
                    //companyPolicy.CoInsuranceCompanies = DelegateService.massiveService.GetCoRequestCoinsuranceByRequedIdByRequestEndorsementIdType(companyRequest.Id, companyRequestEndorsement.Id, companyPolicy.BusinessType.Value);
                    break;
            }
        }
        //private List<Agency> CreateAgencies(List<Agency> agencies, int productId)
        //{
        //    List<Agency> newAgencies = new List<Agency>();
        //    for (int i = 0, agenciesQuanty = agencies.Count; i < agenciesQuanty; i++)
        //    {
        //        Agency item = agencies[i];
        //        Agency agency = DelegateService.uniquePersonService.GetAgencyByAgentIdAgentAgencyId(item.Agent.IndividualId, item.Id);
        //        ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, productId);
        //        agency.Commissions = new List<Commission>();
        //        agency.Commissions.Add(new Commission
        //        {
        //            Percentage = productAgencyCommiss.CommissPercentage,
        //            PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault()
        //        });
        //        if (i == 0)
        //        {
        //            agency.IsPrincipal = true;
        //        }
        //        agency.Participation = 100M / agenciesQuanty;
        //        newAgencies.Add(agency);
        //    }
        //    return newAgencies;
        //}
        private T GetFieldValueByRowPropertyName<T>(Row row, string propertyName, string templateName)
        {
            try
            {
                Field field = row.Fields.Find(x => x.PropertyName == propertyName);
                return (T)DelegateService.utilitiesService.GetValueByField<T>(field);
            }
            catch (Exception ex)
            {
                throw new ValidationException(string.Format(Errors.ErrorGettingFieldValue, propertyName, templateName, ex.GetBaseException().Message));
            }
        }
        /// <summary>
        /// Agregar intermediarios adicionales
        /// </summary>
        /// <param name="policyAgencies"></param>
        /// <param name="additionalIntermediariesTemplate">Plantilla Agencias</param>
        /// <param name="productId"></param>
        /// <param name="prefixId"></param>
        /// <returns>Agencias</returns>
        private List<string> IncludeAdditionalAgencies(List<IssuanceAgency> policyAgencies, Template additionalIntermediariesTemplate, int productId, int prefixId, int branchId)
        {
            List<string> errors = new List<string>();
            List<IssuanceAgency> agencies = new List<IssuanceAgency>();
            IssuanceAgency agency = new IssuanceAgency();
            if (additionalIntermediariesTemplate == null || !additionalIntermediariesTemplate.Rows.Any())
            {
                return errors;
            }
            policyAgencies.RemoveAll(x => !x.IsPrincipal);
            IssuanceAgency principalAgency = policyAgencies.Find(x => x.IsPrincipal);
            if (principalAgency == null)
            {
                throw new ValidationException(Errors.PrincipalAgencyNotFound);
            }
            principalAgency.Participation = 100;
            foreach (Row row in additionalIntermediariesTemplate.Rows)
            {
                int agentCode = GetFieldValueByRowPropertyName<int>(row, FieldPropertyName.AgentCode, additionalIntermediariesTemplate.PropertyName);
                int agentTypeCode = GetFieldValueByRowPropertyName<int>(row, FieldPropertyName.AgentType, additionalIntermediariesTemplate.PropertyName);
                agency = GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeCode);
                string validationError = ValidateAgency(agency, prefixId, productId, branchId);
                if (!string.IsNullOrEmpty(validationError))
                {
                    errors.Add(StringHelper.ConcatenateString(validationError, "|", row.Number.ToString()));
                    continue;
                }
                agency.Participation = GetFieldValueByRowPropertyName<decimal>(row, FieldPropertyName.AgentParticipation, additionalIntermediariesTemplate.PropertyName);
                if (agency.Participation < 0)
                {
                    throw new ValidationException(Errors.MessageAgentsPercentageZero + " " + agentCode);
                }
                if (agency.Participation == 0)
                {
                    throw new ValidationException(Errors.MessageAgentsPercentageIsMandatory);
                }
                agency.Commissions = principalAgency.Commissions;
                agencies.Add(agency);
            }
            decimal agenciesParticipation = agencies.Sum(x => x.Participation);
            if (agenciesParticipation < 100)
            {
                principalAgency.Participation -= agenciesParticipation;
                policyAgencies.AddRange(agencies);
            }
            else
            {
                throw new ValidationException(Errors.MessageAgentsPercentage);
            }
            return errors;
        }
        private string ValidateAgency(IssuanceAgency agency, int prefixCode, int productId, int branchId)
        {
            if (agency == null)
            {
                return Errors.AgencyNotFound;
            }
            //if (agency.Branch.Id != branchId)
            //{
            //    return Errors.AgencyNotAssociatedWithPolicyBranch;
            //}
            if (!DelegateService.productService.ExistProductAgentByAgentIdPrefixIdProductId(agency.Agent.IndividualId, prefixCode, productId))
            {
                return Errors.AgencyNotRelatedToProductOrPrefix;
            }
            if (agency.DateDeclined.HasValue && agency.DateDeclined.Value <= GetStartProcessDate())
            {
                return Errors.DeclinedAgency;
            }
            return "";
        }
        private IssuanceAgency GetAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeCode)
        {
            return DelegateService.underwritingService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeCode);
        }
        private DateTime GetStartProcessDate()
        {
            return DelegateService.commonService.GetDate();
        }
        #endregion
        #region Métodos privados emisión de póliza renovada
        private void ExecuteCreatePolicyRenewal(MassiveRenewalRow massiveRenewalRow)
        {
            try
            {
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Issuance;

                if (massiveRenewalRow.TemporalId.HasValue && massiveRenewalRow.TemporalId > 0)
                {
                    //Policy policy = DelegateService.endorsementService.CreateEndorsement(massiveRenewalRow.TemporalId.Value);
                    //massiveRenewalRow.HasEvents = policy.InfringementPolicies.Any();
                    var pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(massiveRenewalRow.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(massiveRenewalRow.Risk.Policy.Id);
                    }
                    if (pendingOperation != null)
                    {
                        CompanyPolicy companyPolicy = new CompanyPolicy();
                        companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                        companyPolicy.Id = pendingOperation.Id;
                        CompanyPolicy policy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                        massiveRenewalRow.HasEvents = policy.InfringementPolicies.Any();

                        // massiveRenewalRow.Risk.Policy.Endorsement = policy.Endorsement;
                        massiveRenewalRow.Status = MassiveLoadProcessStatus.Finalized;
                    }
                }
                else
                {
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations = Errors.ErrorTemporalNotFound;
                }
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.Observations = string.Format(Errors.ErrorIssuing, ex.Message);

            }
            finally
            {
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
                DataFacadeManager.Dispose();
            }
        }
        #endregion
    }

}
