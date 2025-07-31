using Sistran.Company.Application.UnderwritingBusinessService;
using Sistran.Company.Application.UnderwritingBusinessService.Model;
using Sistran.Company.Application.UnderwritingBusinessServiceProvider.Resources;
using Sistran.Company.Application.UnderwritingBusinessServiceProvider.Business;
using Sistran.Company.Application.UnderwritingBusinessServiceProvider.DAO;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingBusinessServiceProvider.Assemblers;
using Sistran.Company.Application.UnderwritingBusinessServiceProvider.Services;
using EnumsCompany = Sistran.Company.Application.UnderwritingBusinessService.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using EnumsCore = Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.BAF;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sistran.Core.Framework;
using System;
using System.ServiceModel;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.QuotationServicesBusinessServiceProvider.Business;

namespace Sistran.Company.Application.UnderwritingBusinessServiceProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]

    public class CompanyUnderwritingBusinessServiceProvider : Sistran.Core.Application.UnderwritingServices.EEProvider.UnderwritingServiceEEProviderCore, ICompanyUnderwritingBusinessService
    {

        #region  AuthorizationPolicies

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyPolicy policy)
        {
            try
            {
                Core.Framework.Rules.Facade facade = new Core.Framework.Rules.Facade();
                EntityAssembler.CreateFacadeGeneral(facade, policy);
                return DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, policy.Prefix.Id + "," + (int)policy.Product.CoveredRisk.CoveredRiskType, facade, FacadesType.FacadeGeneral);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public CompanyPolicy RunRulesCompanyPolicyPre(CompanyPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentException(Errors.ErrorGetCompanyPolicy);
            }

            try
            {
                CompanyProduct product = DelegateService.productService.GetCompanyProductById(policy.Product.Id);
                policy.Product = Assemblers.ModelAssembler.CreateMapCompanyProduct(product);
                policy.ExchangeRate.Currency = DelegateService.productService.GetCurrenciesByProductId(policy.Product.Id).FirstOrDefault();
                if (policy != null && policy.Product != null && policy.Product.PreRuleSetId.HasValue)
                {
                    RunRulesCompanyPolicy(policy, policy.Product.PreRuleSetId.Value);
                }

                return policy;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        private void RunRulesCompanyPolicy(CompanyPolicy policy, int ruleId)
        {
            try
            {
                PolicyBusiness businessPolicy = new PolicyBusiness();
                businessPolicy.RunRulesCompanyPolicy(policy, ruleId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion


        #region  Crear o actualizar CompanyPolicy


        /// <summary>
        /// Guardar Temporal de la Poliza
        /// </summary>
        /// <param name="policy">Modelo policy</param>

        public CompanyPolicy CompanySavePolicyTemporal(CompanyPolicy policy, bool isMasive)
        {
            if (policy.Id == 0)
            {
                return CreatePolicy(ref policy);
            }
            else
            {
                return UpdatePolicy(ref policy);
            }
        }

        private CompanyPolicy CreatePolicy(ref CompanyPolicy policy)
        {
            policy.Endorsement.EndorsementTypeDescription = Errors.ResourceManager.GetString(Core.Application.Utilities.Helper.EnumHelper.GetItemName<Core.Application.UnderwritingServices.Enums.EndorsementType>(policy.Endorsement.EndorsementType));
            policy.BusinessType = Core.Application.UnderwritingServices.Enums.BusinessType.CompanyPercentage;
            policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsurance>
                {
                    new CompanyIssuanceCoInsurance
                    {
                        ParticipationPercentageOwn = 100
                    }
                };
            if (policy.Request == null && policy.Agencies != null && policy.Agencies.Count == 1)
            {
                PaymentPlanActions(policy);
            }
            else
            {
                GetCommissByAgentIdAgency(policy);
            }
            GetClauses(policy);

            SetDataPolicy(policy);
            policy = CompanySavePolicyTemporal(policy);
            return policy;
        }

    
        private CompanyPolicy UpdatePolicy(ref CompanyPolicy policy)
        {
            PendingOperation pendingOperation = new PendingOperation();
            pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(policy.Id);
            if (pendingOperation != null && pendingOperation.Operation != null)
            {
                CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyPolicy.Id = pendingOperation.Id;

                switch (companyPolicy.Endorsement.EndorsementType)
                {
                    case EnumsCore.EndorsementType.Emission:
                    case EnumsCore.EndorsementType.Renewal:
                        policy = SetDataEmission(policy, companyPolicy);
                        break;

                    case EnumsCore.EndorsementType.Modification:
                        policy = SetDataModification(policy, companyPolicy);
                        break;

                    default:
                        break;
                }
                policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                try
                {
                    PolicyDAO policyDAO = new PolicyDAO();
                    policy = policyDAO.CreateTemporalCompanyPolicy(policy);
                    pendingOperation.Operation = JsonConvert.SerializeObject(policy);
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                    policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    policy.Id = pendingOperation.Id;
                    return policy;
                }
                catch (Exception)
                {

                    throw new BusinessException(Errors.ErrorRecordTemporal);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }
        }

        /// <summary>
        /// Reserva consecutivos de cotizacion
        /// </summary>
        /// <param name="countQuotation">cantidad de cotizaciones</param>
        /// <param name="branchCode">sucursal</param>
        /// <param name="prefixCode">ramo</param>
        /// <returns>lista de reserva numeración cotizaciones</returns>
        private List<int> GetReserveListQuotes(int countQuotation, int branchCode, int prefixCode)
        {
            BusinessQuotationNumber businessQuotationNumber = new BusinessQuotationNumber();
            businessQuotationNumber.ValidateArguments(countQuotation, branchCode, prefixCode);
            List<int> quotationNumber = new List<int>();

            try
            {
                QuotationNumberDAO quotationNumberDAO = new QuotationNumberDAO();
                quotationNumber = quotationNumberDAO.GetReserveListQuotes(countQuotation, branchCode, prefixCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return quotationNumber;
        }


        /// <summary>
        /// Se graba Json y Tablas Temporales
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        private CompanyPolicy CompanySavePolicyTemporal(CompanyPolicy companyPolicy)
        {
            try
            {
                PendingOperation pendingOperation = new PendingOperation
                {
                    OperationName = companyPolicy.TemporalTypeDescription,
                    UserId = companyPolicy.UserId,
                    UserName = companyPolicy.User?.AccountName,
                    Operation = JsonConvert.SerializeObject(companyPolicy)
                };
                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                companyPolicy.Id = pendingOperation.Id;
                PolicyDAO policyDAO = new PolicyDAO();
                companyPolicy = policyDAO.CreateTemporalCompanyPolicy(companyPolicy);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                pendingOperation.AdditionalInformation = companyPolicy.Endorsement.TemporalId.ToString();
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                return companyPolicy;
            }
            catch (Exception ex)
            {
                if (ex is BusinessException || ex is ValidationException)
                {
                    throw;
                }
                throw new BusinessException(ex.Message);
            }
        }



        #endregion


        #region  SetDataPolicy

        private void GetClauses(CompanyPolicy policy)
        {
            ModelAssembler.CreateMapCompanyClause();
            List<CompanyClause> clauses = Mapper.Map<List<Core.Application.UnderwritingServices.Models.Clause>, List<CompanyClause>>(GetClausesByEmissionLevelConditionLevelId(EnumsCore.EmissionLevel.General, policy.Prefix.Id));

            if (policy.Clauses != null)
            {
                policy.Clauses = policy.Clauses.Where(x => x.IsMandatory == false).ToList();
            }
            else
            {
                policy.Clauses = new List<CompanyClause>();
            }

            if (clauses.Count > 0)
            {
                policy.Clauses.AddRange(clauses.Where(x => x.IsMandatory == true).ToList());
            }
        }

        private void SetDataPolicy(CompanyPolicy policy)
        {
            policy.BeginDate = Convert.ToDateTime(String.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now));
            policy.EffectPeriod = policy.EffectPeriod == 0 ? DelegateService.commonService.GetExtendedParameterByParameterId((int)ParametersTypes.DaysValidity).NumberParameter.Value : policy.EffectPeriod;
            ProductServices.Models.CompanyProduct companyProduct = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
            policy.Product = ModelAssembler.CreateMapCompanyProduct(companyProduct);
            policy.CalculateMinPremium = DelegateService.productService.GetCalculateMinPremiumByProductId(policy.Product.Id);
            policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);

            if (policy.TemporalType == Core.Application.UnderwritingServices.Enums.TemporalType.Quotation)
            {
                policy.Endorsement.QuotationId = GetReserveListQuotes(1, policy.Branch.Id, policy.Prefix.Id).FirstOrDefault();
                policy.Endorsement.QuotationVersion = 0;
            }
        }

        private static void GetCommissByAgentIdAgency(CompanyPolicy policy)
        {
            foreach (var item in policy.Agencies)
            {
                Core.Application.ProductServices.Models.ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(item.Code, item.Id, policy.Product.Id);
                item.Commissions.Add(new CompanyIssuanceCommission
                {
                    Percentage = productAgencyCommiss.CommissPercentage,
                    PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault(0),
                    SubLineBusiness = new CompanySubLineBusiness
                    {
                        LineBusiness = new CompanyLineBusiness
                        {
                            Id = policy.Prefix.Id
                        },
                    }
                });

            }
        }

        private void PaymentPlanActions(CompanyPolicy policy)
        {
            ModelAssembler.CreateMapCompanyPaymentPlan();
            policy.PaymentPlan = Mapper.Map<Core.Application.UnderwritingServices.Models.PaymentPlan, CompanyPolicyPaymentPlan>(GetDefaultPaymentPlanByProductId(policy.Product.Id));
            policy.Agencies[0].Participation = 100;

            Core.Application.ProductServices.Models.ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(policy.Agencies[0].Agent.IndividualId, policy.Agencies[0].Id, policy.Product.Id);
            policy.Agencies[0].Commissions.Add(new CompanyIssuanceCommission
            {
                Percentage = productAgencyCommiss.CommissPercentage,
                PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault(0),
                SubLineBusiness = new CompanySubLineBusiness
                {
                    LineBusiness = new CompanyLineBusiness
                    {
                        Id = policy.Prefix.Id
                    },
                },
            });
        }


        /// <summary>
        /// Sets the data emission.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="policyOld">The policy old.</param>
        /// <returns></returns>
        private CompanyPolicy SetDataEmission(CompanyPolicy policy, CompanyPolicy policyOld)
        {
            policy.Endorsement = policyOld.Endorsement;
            policy.DocumentNumber = policyOld.DocumentNumber;
            policy.TemporalType = policyOld.TemporalType;
            policy.TemporalTypeDescription = policyOld.TemporalTypeDescription;
            policy.BusinessType = policyOld.BusinessType;
            policy.CoInsuranceCompanies = policyOld.CoInsuranceCompanies;
            if (policy.Agencies != null)
            {
                CompanyIssuanceAgency agency = policy.Agencies.First(x => x.IsPrincipal);
                policy.Agencies = policyOld.Agencies;

                if (policy.Agencies.Exists(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id))
                {
                    policy.Agencies.AsParallel().ForAll(x => x.IsPrincipal = false);
                    policy.Agencies.First(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id).IsPrincipal = true;
                }
                else
                {
                    policy.Agencies.First(x => x.IsPrincipal == true).Id = agency.Id;
                    policy.Agencies.First(x => x.IsPrincipal == true).Code = agency.Code;
                    policy.Agencies.First(x => x.IsPrincipal == true).FullName = agency.FullName;
                    policy.Agencies.First(x => x.IsPrincipal == true).Agent = agency.Agent;
                    policy.Agencies.First(x => x.IsPrincipal == true).Branch = agency.Branch;

                    ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, policy.Product.Id);
                    policy.Agencies.AsParallel().ForAll(x => x.Commissions.AsParallel().ForAll(y => { y.Percentage = productAgencyCommiss.CommissPercentage; y.Percentage = productAgencyCommiss.CommissPercentage; }));
                }
            }

            policy.PaymentPlan = policyOld.PaymentPlan;
            policy.PayerComponents = policyOld.PayerComponents;
            policy.Text = policyOld.Text;
            policy.Clauses = policyOld.Clauses;
            policy.DefaultBeneficiaries = policyOld.DefaultBeneficiaries;
            policy.BeginDate = policyOld.BeginDate;
            policy.EffectPeriod = policy.EffectPeriod == 0 ? DelegateService.commonService.GetExtendedParameterByParameterId((int)Core.Application.CommonService.Enums.ParametersTypes.DaysValidity).NumberParameter.Value : policy.EffectPeriod;
            policy.Product = ModelAssembler.CreateMapCompanyProduct(DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id));
            policy.Summary = policyOld.Summary;

            bool calculate = DelegateService.productService.GetCalculateMinPremiumByProductId(policy.Product.Id);
            if (!calculate)
            {
                policy.CalculateMinPremium = calculate;
            }
            else
            {
                policy.CalculateMinPremium = policyOld.CalculateMinPremium;
            }
            return policy;
        }


        private CompanyPolicy SetDataModification(CompanyPolicy policy, CompanyPolicy policyOld)
        {
            policy.CurrentFrom = policyOld.CurrentFrom;
            return policy;
        }




        #endregion

        #region  Consultar CompanyPolicy

        public CompanyPolicy CompanyGetTemporalByIdTemporalType(int id, EnumsCore.TemporalType temporalType)
        {
            CompanyPolicy policy = CompanyGetPolicyByTemporalId(id, false);
            if (policy != null && policy.Id > 0)
            {
                var settings = new JsonSerializerSettings();

                if (policy.TemporalType == temporalType)
                {
                    policy = GetPolicyDescriptions(policy);

                    var authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(policy.Id.ToString());

                    if (authorizationRequests.Any(x => x.Status == TypeStatus.Rejected))
                    {
                        throw new Exception(Errors.ErrorTempEventReject);
                    }
                    if (authorizationRequests.Count > 0)
                    {
                        throw new Exception(Errors.ErrorTempEventAutorize);
                    }

                    return policy;
                }
                else
                {
                    throw new BusinessException(string.Format(Errors.ErrorTemporalType, policy.TemporalTypeDescription));
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }
        }

        /// <summary>
		/// Obtener Póliza
		/// </summary>
		/// <param name="temporalId">Id Temporal</param>
		/// <returns>Póliza</returns>
		public CompanyPolicy CompanyGetPolicyByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCompanyPolicyByTemporalId(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        private CompanyPolicy GetPolicyDescriptions(CompanyPolicy policy)
        {
            if (policy.Holder.IdentificationDocument == null)
            {
                Holder holder = (DelegateService.underwritingServiceCore.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault());
                policy.Holder = ModelAssembler.CreateMapCompanyHolder(holder);



            }

            if (policy.Agencies[0].Branch == null || string.IsNullOrEmpty(policy.Agencies[0].FullName))
            {
                Parallel.ForEach(policy?.Agencies, item =>
                {
                    var agent = DelegateService.uniquePersonServiceV1.GetAgentByIndividualId(item.Agent.IndividualId);
                    item.Agent = new CompanyIssuanceAgent
                    {
                        IndividualId = agent.IndividualId,
                        FullName = agent.FullName,
                        AgentType = new CompanyIssuanceAgentType
                        {
                            Id = agent.AgentType.Id,
                            Description = agent.AgentType.Description
                        },
                    };
                    item.FullName = item.FullName;
                });
            }

            if (string.IsNullOrEmpty(policy.PaymentPlan.Description))
            {
                policy.PaymentPlan.Description = DelegateService.underwritingServiceCore.GetPaymentPlanByPaymentPlanId(policy.PaymentPlan.Id).Description;
            }
            if (policy.CoInsuranceCompanies != null && policy.BusinessType != EnumsCore.BusinessType.CompanyPercentage)
            {
                var insuranceCompanies = policy?.CoInsuranceCompanies.Where(x => string.IsNullOrEmpty(x.Description)).ToList();
                var insuranceCompaniesData = policy.CoInsuranceCompanies.Where(x => !string.IsNullOrEmpty(x.Description)).ToList();
                insuranceCompanies.AsParallel().ForAll(x => x.Description = DelegateService.underwritingServiceCore.GetCoInsuranceCompanyByCoinsuranceId((int)x.Id).Description);
                insuranceCompanies.AddRange(insuranceCompaniesData);
                policy.CoInsuranceCompanies = insuranceCompanies;
            }

           // policy = CompanyCreatePolicyTemporal(policy, false);

            return policy;
        }




       



        #endregion


        #region  Consultar Cotización

        public List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            QuotationBusiness bis = new QuotationBusiness();
            return bis.GetCompanyPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
        }

        /// <summary>
        /// Obtener Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Cotización</returns>
        public List<CompanyPolicy> GetPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region  Consultaa


        public CompanyIssuanceInsured GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            CompanyIssuanceInsured insured = null;
            var companyInsured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType).First();

            if (companyInsured != null)
            {
                Mapper.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
                insured = Mapper.Map<IssuanceInsured, CompanyIssuanceInsured>(companyInsured);
            }
            return insured;
        }


        public CompanyCoverage QuotateCompanyCoverage(CompanyCoverage companyCoverage, int policyId, int riskId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateCompanyCoverage(companyCoverage, policyId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }




        #endregion

        #region Surcharges
        public List<CompanySurchargeComponent> GetCompanySurcharges()
        {
            try
            {
                SurchargeDAO surchargeDao = new SurchargeDAO();
                return surchargeDao.GetSurcharges();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanySurchargeComponent> ExecuteOperationCompanySurcharges(List<CompanySurchargeComponent> surcharges)
        {
            try
            {
                SurchargeDAO surchargeDao = new SurchargeDAO();
                return null; //por ahora
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

    }


}
