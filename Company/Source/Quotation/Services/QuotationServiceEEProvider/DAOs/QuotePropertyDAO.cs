using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.QuotationServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Company.Application.QuotationServices.EEProvider.Assemblers;

namespace Sistran.Company.Application.QuotationServices.EEProvider.DAOs
{
    public class QuotePropertyDAO
    {
        public CompanyPropertyRisk QuoteProperty(CompanyPropertyRisk companyProperty)
        {
            SetDataPolicy(companyProperty.Risk.Policy);
            SetDataProperty(companyProperty);
            companyProperty = DelegateService.propertyService.QuotateProperty(companyProperty, true, true);
            CreateSummary(companyProperty);
            return companyProperty;
        }

        public CompanyPropertyRisk GetCompanyPropertyRiskByTemporalId(int temporalId)
        {
            throw new NotImplementedException();
        }

        private void SetDataPolicy(CompanyPolicy policy)
        {
            List<UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByUserId(policy.UserId);

            if (agencies.Count > 0)
            {

                policy.Agencies = new List<IssuanceAgency>();
                policy.Agencies.Add(new IssuanceAgency
                {
                    Id = agencies[0].Id,
                    FullName = agencies[0].FullName,
                    Agent = new IssuanceAgent
                    {
                        IndividualId = agencies[0].Agent.IndividualId,
                        FullName = agencies[0].Agent.FullName
                    },
                    IsPrincipal = true,
                    Participation = 100,
                    Commissions = new List<IssuanceCommission>()
                });

                policy.IsPersisted = true;
                policy.TemporalType = TemporalType.Quotation;
                policy.BusinessType = BusinessType.CompanyPercentage;
                policy.BeginDate = DateTime.Now;
                policy.CurrentFrom = DateTime.Now;
                policy.CurrentTo = policy.CurrentFrom.AddYears(1);
                policy.Endorsement.EndorsementType = EndorsementType.Emission;
                policy.ExchangeRate = new ExchangeRate
                {
                    Currency = new Currency(),
                    SellAmount = 1
                };

                policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, policy.Holder.CustomerType).First();

                CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(policy.Holder.IndividualId, (CustomerType)policy.Holder.CustomerType).OrderBy(x => x.IsMain).First();
                policy.Holder.CompanyName = new IssuanceCompanyName
                {
                    NameNum = companyName.NameNum,
                    TradeName = companyName.TradeName,
                    Address = new IssuanceAddress
                    {
                        Id = companyName.Address.Id,
                        Description = companyName.Address.Description,
                        City = companyName.Address.City
                    },
                    Phone = new IssuancePhone
                    {
                        Id = companyName.Phone.Id,
                        Description = companyName.Phone.Description
                    },
                    Email = new IssuanceEmail
                    {
                        Id = companyName.Email.Id,
                        Description = companyName.Email.Description
                    }
                };

                policy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);


                var imapper = MapperCache.GetMapper<PolicyType, CompanyPolicyType>(cfg =>
               {
                   cfg.CreateMap<PolicyType, CompanyPolicyType>();
               });

                policy.PolicyType = imapper.Map<PolicyType, CompanyPolicyType>(DelegateService.commonService.GetPolicyTypesByProductId(policy.Product.Id).FirstOrDefault());
                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);

                policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                policy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
                {
                    ParticipationPercentageOwn = 100
                });

                var imapperPaymentPlan = MapperCache.GetMapper<PaymentPlan, CompanyPaymentPlan>(cfg =>
                {
                    cfg.CreateMap<PaymentPlan, CompanyPaymentPlan>();
                });
                policy.PaymentPlan = imapperPaymentPlan.Map<PaymentPlan, CompanyPaymentPlan>(DelegateService.underwritingService.GetPaymentPlansByProductId(policy.Product.Id).OrderBy(x => x.IsDefault).Take(1).First());

                var imapperBranch = MapperCache.GetMapper<Branch, CompanyBranch>(cfg =>
                {
                    cfg.CreateMap<Branch, CompanyBranch>();
                });
                CompanyBranch branch = imapperBranch.Map<Branch, CompanyBranch>(DelegateService.uniqueUserService.GetDefaultBranchesByUserId(policy.UserId));
                if (branch == null)
                {
                    List<CompanyBranch> branches = imapperBranch.Map<List<Branch>, List<CompanyBranch>>(DelegateService.uniqueUserService.GetBranchesByUserId(policy.UserId));
                    branch = branches.First();
                }
                policy.Branch = branch;

                var imapperSalesPoint = MapperCache.GetMapper<SalePoint, CompanySalesPoint>(cfg =>
               {
                   cfg.CreateMap<SalePoint, CompanySalesPoint>();
               });

                List<CompanySalesPoint> salepoints = imapperSalesPoint.Map<List<SalePoint>, List<CompanySalesPoint>>(DelegateService.uniqueUserService.GetSalePointsByBranchIdUserId(branch.Id, policy.UserId));
                if (salepoints.Count > 0)
                {
                    policy.Branch.SalePoints = salepoints;
                }
            }
            else
            {
                throw new ValidationException(Errors.ValidationUserWithoutIntermediates);
            }
        }

        private void SetDataProperty(CompanyPropertyRisk companyProperty)
        {
            companyProperty.Risk.IsPersisted = true;
            companyProperty.Risk.Status = RiskStatusType.Original;
            companyProperty.Risk.CoveredRiskType = CoveredRiskType.Location;
            companyProperty.HasNomenclature = false;
            companyProperty.NomenclatureAddress = new CompanyNomenclatureAddress
            {
                Type = new CompanyRouteType
                {
                    Id = 1
                }
            };
            companyProperty.Risk.RiskActivity = new CompanyRiskActivity
            {
                Id = 3
            };

            companyProperty.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(companyProperty.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, companyProperty.Risk.MainInsured.CustomerType);

            companyProperty.Risk.Beneficiaries = new List<CompanyBeneficiary>();
            companyProperty.Risk.Beneficiaries.Add(new CompanyBeneficiary
            {
                IdentificationDocument = companyProperty.MainInsured.IdentificationDocument,
                CustomerType = companyProperty.Risk.MainInsured.CustomerType,
                IndividualId = companyProperty.Risk.MainInsured.IndividualId,
                BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.LeasingBeneficiaryTypeId },
                Participation = 100,
                CompanyName = companyProperty.Risk.MainInsured.CompanyName
            });

            List<CompanyCoverage> addCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyProperty.Risk.Coverages)
            {
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();

                coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(coverage.InsuredObject.Id, companyProperty.Risk.GroupCoverage.Id, companyProperty.Risk.Policy.Product.Id);
                coverages.RemoveAll(x => !x.IsSelected);

                foreach (CompanyCoverage addCoverage in coverages)
                {
                    addCoverage.CurrentFrom = companyProperty.Risk.Policy.CurrentFrom;
                    addCoverage.CurrentTo = companyProperty.Risk.Policy.CurrentTo;
                    addCoverage.InsuredObject = coverage.InsuredObject;
                    addCoverage.CoverStatus = CoverageStatusType.Original;
                    addCoverage.EndorsementType = companyProperty.Risk.Policy.Endorsement.EndorsementType;
                }

                addCoverages.AddRange(coverages);
            }

            companyProperty.Risk.Coverages = addCoverages;
        }

        private void CreateSummary(CompanyPropertyRisk companyProperty)
        {
            if (companyProperty.Risk.Premium > 0)
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                risks.Add(companyProperty.Risk);

                companyProperty.Risk.Policy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyProperty.Risk.Policy, risks);
                companyProperty.Risk.Policy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyProperty.Risk.Policy, risks);
                var companyPolicy = companyProperty.Risk.Policy;
                companyProperty.Risk.Policy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
                companyProperty.Risk.Policy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyProperty.Risk.Policy, risks);

                CreateQuotation(companyProperty);
            }
            else
            {
                companyProperty.Risk.Policy.Summary = new CompanySummary();
            }
        }

        private void CreateQuotation(CompanyPropertyRisk companyProperty)
        {
            PendingOperation pendingOperation = new PendingOperation();

            if (companyProperty.Risk.Policy.Id == 0)
            {
                pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty.Risk.Policy);
                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                companyProperty.Risk.Policy.Id = pendingOperation.Id;
            }
            else
            {
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyProperty.Risk.Policy.Id);
                CompanyPolicy policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyProperty.Risk.Policy.Endorsement.TemporalId = policy.Endorsement.TemporalId;
            }

            DelegateService.underwritingService.CreatePolicyTemporal(companyProperty.Risk.Policy, false);
            pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty.Risk.Policy);
            pendingOperation = DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(companyProperty.Risk.Policy.Id);
            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy = companyProperty.Risk.Policy;
            companyProperty.Risk.Policy = null;

            if (pendingOperations.Count == 0)
            {
                pendingOperation = new PendingOperation();
                pendingOperation.ParentId = companyPolicy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty);

                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                companyProperty.Risk.Id = pendingOperation.Id;
            }
            else
            {
                pendingOperation = new PendingOperation();
                pendingOperation = pendingOperations[0];
                CompanyPropertyRisk property = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperation.Operation);
                companyProperty.Risk.Id = property.Risk.Id;
            }

            companyProperty.Risk.Policy = companyPolicy;
            DelegateService.propertyService.CreatePropertyTemporal(companyProperty, false);
            pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty);
            pendingOperation = DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
        }


        /// <summary>
        /// Obtener Cotización Por Identificador
        /// </summary>
        /// <param name="quotationId">Identificador</param>
        /// <returns>Cotización</returns>
        public CompanyPropertyRisk GetCompanyPropertyByQuotationId(int quotationId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(TMPEN.TempSubscription.Properties.QuotationId, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(quotationId);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(TMPEN.TempSubscription), filter.GetPredicate());

            if (businessCollection.Count > 0)
            {
                int operationId = ((TMPEN.TempSubscription)businessCollection[0]).OperationId.Value;
                int temporalId = ((TMPEN.TempSubscription)businessCollection[0]).TempId;

                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                CompanyPolicy companyPolicy = new CompanyPolicy();
                companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyPolicy.Id = operationId;
                companyPolicy.Endorsement.QuotationId = quotationId;
                companyPolicy.Endorsement.TemporalId = temporalId;

                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(operationId).FirstOrDefault();
                CompanyPropertyRisk risk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperation.Operation);
                risk.Risk.Policy = companyPolicy;

                return risk;
            }
            else
            {
                return null;
            }
        }



        public List<CompanyPropertyRisk> GetCompanyPropertyByCompanyPolicy(CompanyPropertyRisk temporal)
        {
            List<CompanyPropertyRisk> risks = new List<CompanyPropertyRisk>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (temporal.Risk.Policy.Holder.IndividualId > 0)
            {
                filter.Property(TMPEN.TempSubscription.Properties.PolicyHolderId, "TEMP_SUBSCRIPTION");
                filter.Equal();
                filter.Constant(temporal.Risk.Policy.Holder.IndividualId);
            }
            if (temporal.Risk.Policy.UserId > 0)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(TMPEN.TempSubscription.Properties.UserId, "TEMP_SUBSCRIPTION");
                filter.Equal();
                filter.Constant(temporal.Risk.Policy.UserId);
            }
            if (temporal.Risk.Policy.CurrentFrom > Convert.ToDateTime("01/01/0001"))
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(TMPEN.TempSubscription.Properties.CurrentFrom, "TEMP_SUBSCRIPTION");
                filter.GreaterEqual();
                filter.Constant(temporal.Risk.Policy.CurrentFrom);
            }
            if (temporal.Risk.Policy.CurrentTo > Convert.ToDateTime("01/01/0001"))
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(TMPEN.TempSubscription.Properties.CurrentFrom, "TEMP_SUBSCRIPTION");
                filter.LessEqual();
                filter.Constant(temporal.Risk.Policy.CurrentTo);
            }
            if (temporal.Risk.Policy.Product.Id > 0)
            {
                if (filter.GetPredicate() != null)
                {
                    filter.And();
                }
                filter.Property(TMPEN.TempSubscription.Properties.ProductId, "TEMP_SUBSCRIPTION");
                filter.Equal();
                filter.Constant(temporal.Risk.Policy.Product.Id);
            }
            if (filter.GetPredicate() != null)
            {
                filter.And();
            }
            filter.Property(TMPEN.TempSubscription.Properties.QuotationId, "TEMP_SUBSCRIPTION");
            filter.IsNotNull();

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(TMPEN.TempSubscription), filter.GetPredicate());

            if (businessCollection.Count > 0)
            {

                for (int i = 0; i < businessCollection.Count; i++)
                {
                    int operationId = ((TMPEN.TempSubscription)businessCollection[i]).OperationId.Value;
                    int temporalId = ((TMPEN.TempSubscription)businessCollection[i]).TempId;
                    int quotationId = (int)((TMPEN.TempSubscription)businessCollection[i]).QuotationId;

                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                    CompanyPolicy companyPolicy = new CompanyPolicy();
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = operationId;
                    companyPolicy.Endorsement.QuotationId = quotationId;
                    companyPolicy.Endorsement.TemporalId = temporalId;

                    pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(operationId).FirstOrDefault();
                    CompanyPropertyRisk risk = new CompanyPropertyRisk();
                    if (pendingOperation != null)
                    {
                        risk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperation.Operation);
                    }
                    risk.Risk.Policy = companyPolicy;
                    risks.Add(risk);
                }
                return risks;
            }
            else
            {
                return null;
            }
        }
        public CompanyPolicy CreateCompanyTemporalFromQuotation(int operationId)
        {
            PendingOperation pendingOperation = new PendingOperation();
            pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);
            CompanyPolicy policy = new CompanyPolicy();

            if (pendingOperation != null)
            {
                policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                policy.TemporalType = TemporalType.Policy;
                policy.Endorsement.TemporalId = 0;
                policy.Endorsement.QuotationVersion = 0;

                pendingOperation.Id = 0;
                pendingOperation.Operation = JsonConvert.SerializeObject(policy);
                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                policy.Id = pendingOperation.Id;

                List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(operationId);

                foreach (PendingOperation item in pendingOperations)
                {
                    item.Id = 0;
                    item.ParentId = policy.Id;
                    DelegateService.utilitiesServiceCore.CreatePendingOperation(item);
                }
            }
            return policy;
        }
    }
}
