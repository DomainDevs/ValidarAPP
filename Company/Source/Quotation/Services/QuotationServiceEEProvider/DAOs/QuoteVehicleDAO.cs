using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.QuotationServices.EEProvider.Assemblers;
using Sistran.Company.Application.QuotationServices.EEProvider.Entities.Views;
using Sistran.Company.Application.QuotationServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using COMM = Sistran.Core.Application.Common.Entities;
using PROD = Sistran.Core.Application.Product.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;

namespace Sistran.Company.Application.QuotationServices.EEProvider.DAOs
{
    public class QuoteVehicleDAO
    {
        /// <summary>
        /// Cotizar póliza autos
        /// </summary>
        /// <param name="companyPolicy">Datos vehiculo</param>
        /// <returns>Cotización</returns>
        public CompanyVehicle QuoteVehicle(CompanyVehicle companyVehicle)
        {
            SetVehiclePolicyData(companyVehicle.Risk.Policy);
            SetVehicleData(companyVehicle);
            companyVehicle = DelegateService.vehicleService.QuotateVehicle(companyVehicle, true, true, 0);
            return GetSummary(companyVehicle);
        }

        private void SetVehiclePolicyData(CompanyPolicy companyPolicy)
        {
            List<UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByUserId(companyPolicy.UserId);

            if (agencies.Count > 0)
            {
                companyPolicy.Agencies = new List<IssuanceAgency>();
                companyPolicy.Agencies.Add(new IssuanceAgency
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

                companyPolicy.IsPersisted = true;
                companyPolicy.TemporalType = TemporalType.Quotation;
                companyPolicy.BusinessType = BusinessType.CompanyPercentage;
                companyPolicy.BeginDate = DateTime.Now.Date;
                companyPolicy.CurrentFrom = DateTime.Now.Date;
                companyPolicy.CurrentTo = companyPolicy.CurrentFrom.AddYears(1);
                companyPolicy.Endorsement.EndorsementType = EndorsementType.Emission;
                companyPolicy.ExchangeRate = new ExchangeRate
                {
                    Currency = new Currency(),
                    SellAmount = 1
                };

                companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, companyPolicy.Holder.CustomerType).First();

                CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(companyPolicy.Holder.IndividualId, (CustomerType)companyPolicy.Holder.CustomerType).OrderBy(x => x.IsMain).First();
                companyPolicy.Holder.CompanyName = new IssuanceCompanyName
                {
                    NameNum = companyName.NameNum,
                    TradeName = companyName.TradeName,
                    Address = companyName.Address == null ? null : new IssuanceAddress
                    {
                        Id = companyName.Address.Id,
                        Description = companyName.Address.Description,
                        City = companyName.Address.City
                    },
                    Phone = companyName.Phone == null ? null : new IssuancePhone
                    {
                        Id = companyName.Phone.Id,
                        Description = companyName.Phone.Description
                    },
                    Email = companyName.Email == null ? null : new IssuanceEmail
                    {
                        Id = companyName.Email.Id,
                        Description = companyName.Email.Description
                    }
                };

                companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

                var imapper = MapperCache.GetMapper<PolicyType, CompanyPolicyType>(cfg =>
                {
                    cfg.CreateMap<PolicyType, CompanyPolicyType>();
                });
                companyPolicy.PolicyType = imapper.Map<PolicyType, CompanyPolicyType>(DelegateService.commonService.GetPolicyTypesByProductId(companyPolicy.Product.Id).FirstOrDefault());
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);

                companyPolicy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                companyPolicy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
                {
                    ParticipationPercentageOwn = 100
                });

                var imapperPaymentPlan = MapperCache.GetMapper<PaymentPlan, CompanyPaymentPlan>(cfg =>
              {
                  cfg.CreateMap<PaymentPlan, CompanyPaymentPlan>();
              });
                companyPolicy.PaymentPlan = imapperPaymentPlan.Map<PaymentPlan, CompanyPaymentPlan>(DelegateService.underwritingService.GetPaymentPlansByProductId(companyPolicy.Product.Id).OrderBy(x => x.IsDefault).Take(1).First());



                var imapperBranch = MapperCache.GetMapper<Branch, CompanyBranch>(cfg =>
                {
                    cfg.CreateMap<Branch, CompanyBranch>();
                });
                CompanyBranch branch = imapperBranch.Map<Branch, CompanyBranch>(DelegateService.uniqueUserService.GetDefaultBranchesByUserId(companyPolicy.UserId));
                if (branch == null)
                {
                    List<CompanyBranch> branches = imapperBranch.Map<List<Branch>, List<CompanyBranch>>(DelegateService.uniqueUserService.GetBranchesByUserId(companyPolicy.UserId));
                    branch = branches.First();
                }
                companyPolicy.Branch = branch;
                var imapperSalesPoint = MapperCache.GetMapper<SalePoint, CompanySalesPoint>(cfg =>
               {
                   cfg.CreateMap<SalePoint, CompanySalesPoint>();
               });
                List<CompanySalesPoint> salepoints = imapperSalesPoint.Map<List<SalePoint>, List<CompanySalesPoint>>(DelegateService.uniqueUserService.GetSalePointsByBranchIdUserId(branch.Id, companyPolicy.UserId));
                if (salepoints.Count > 0)
                {
                    companyPolicy.Branch.SalePoints = salepoints;
                }

            }
            else
            {
                throw new ValidationException("ErrorUserWithOutIntermediates");
            }
        }

        private void SetVehicleData(CompanyVehicle companyVehicle)
        {
            companyVehicle.Risk.Status = RiskStatusType.Original;
            companyVehicle.StandardVehiclePrice = companyVehicle.Price;
            companyVehicle.Risk.CoveredRiskType = CoveredRiskType.Vehicle;
            companyVehicle.Risk.IsPersisted = true;
            companyVehicle.ActualDateMovement = DateTime.Now;

            companyVehicle.NewPrice = DelegateService.vehicleService.GetYearsByMakeIdModelIdVersionId(companyVehicle.Make.Id, companyVehicle.Model.Id, companyVehicle.Version.Id).Where(x => x.Price != 0).OrderByDescending(x => x.Description).First().Price;

            var imapper = MapperCache.GetMapper<LimitRc, CompanyLimitRc>(cfg =>
            {
                cfg.CreateMap<LimitRc, CompanyLimitRc>();
            });

            if (companyVehicle.Risk.LimitRc == null || companyVehicle.Risk.LimitRc.Id == 0)
            {
                List<CompanyLimitRc> limitRcs = imapper.Map<List<LimitRc>, List<CompanyLimitRc>>(DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(companyVehicle.Risk.Policy.Prefix.Id, companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.Policy.PolicyType.Id));
                companyVehicle.Risk.LimitRc = limitRcs.OrderBy(x => x.IsDefault).FirstOrDefault();
                companyVehicle.Risk.LimitRc.LimitSum = companyVehicle.Risk.LimitRc.Limit1 + companyVehicle.Risk.LimitRc.Limit3;
            }
            else
            {
                if (companyVehicle.Risk.LimitRc.LimitSum == 0)
                {
                    companyVehicle.Risk.LimitRc = DelegateService.underwritingService.GetCompanyLimitRcById(companyVehicle.Risk.LimitRc.Id);
                }
            }

            companyVehicle.Risk.AmountInsured = companyVehicle.Risk.LimitRc.LimitSum + companyVehicle.Price + companyVehicle.PriceAccesories;

            companyVehicle.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(companyVehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, companyVehicle.Risk.MainInsured.CustomerType);

            companyVehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
            companyVehicle.Risk.Beneficiaries.Add(new CompanyBeneficiary
            {
                IdentificationDocument = companyVehicle.Risk.MainInsured.IdentificationDocument,
                CustomerType = companyVehicle.Risk.MainInsured.CustomerType,
                IndividualId = companyVehicle.Risk.MainInsured.IndividualId,
                BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.LeasingBeneficiaryTypeId },
                Participation = 100,
                IndividualType = IndividualType.Person,
                CompanyName = companyVehicle.Risk.MainInsured.CompanyName
            });
            companyVehicle.Accesories = new List<CompanyAccessory>();
            companyVehicle.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, companyVehicle.Risk.Policy.Prefix.Id);
            companyVehicle.Risk.Coverages = companyVehicle.Risk.Coverages.Where(x => x.IsSelected == true).ToList();

            foreach (CompanyCoverage item in companyVehicle.Risk.Coverages)
            {
                item.EndorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType;
                item.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                item.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;
            }
        }

        private CompanyVehicle GetSummary(CompanyVehicle companyVehicle)
        {
            if (companyVehicle.Risk.Premium > 0)
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                risks.Add(companyVehicle.Risk);
                companyVehicle.Risk.Policy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyVehicle.Risk.Policy, risks);
                companyVehicle.Risk.Policy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyVehicle.Risk.Policy, risks);
                var companyPolicy = companyVehicle.Risk.Policy;
                companyVehicle.Risk.Policy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
                //companyVehicle.Risk.Policy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyVehicle.Risk.Policy, risks);

                CreateQuotation(companyVehicle);
            }
            else
            {
                companyVehicle.Risk.Policy.Summary = new CompanySummary();
            }

            return companyVehicle;
        }

        /// <summary>
        /// Obtener Cotización Por Identificador
        /// </summary>
        /// <param name="quotationId">Identificador</param>
        /// <returns>Cotización</returns>
        public CompanyVehicle GetCompanyVehicleByQuotationId(int quotationId)
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
                CompanyVehicle companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperation.Operation);
                companyVehicle.Risk.Policy = companyPolicy;

                return companyVehicle;
            }
            else
            {
                return null;
            }
        }

        public CompanyVehicle CreateQuotation(CompanyVehicle companyVehicle)
        {
            companyVehicle.Risk.Policy = DelegateService.underwritingService.CompanySavePolicyTemporal(companyVehicle.Risk.Policy, false);
            DelegateService.vehicleService.CreateVehicleTemporal(companyVehicle, false);
            return companyVehicle;
        }

        #region Temporales
        public List<CompanyQuotationVehicleSearch> GetQuotationVehicleSearch(int TempId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TMPEN.TempSubscription.Properties.PolicyHolderId, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(TempId);
            filter.And();
            filter.Property(TMPEN.TempSubscription.Properties.QuotationId, typeof(TMPEN.TempSubscription).Name);
            filter.IsNotNull();

            SearchQuotationView view = new SearchQuotationView();
            ViewBuilder builder = new ViewBuilder("SearchQuotationView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.TempSubscriptions.Count > 0)
            {
                List<TMPEN.TempSubscription> entityTempSuscriptions = view.TempSubscriptions.Cast<TMPEN.TempSubscription>().ToList();

                List<TMPEN.TempRiskVehicle> entityTempRiskVehicles = view.TempRiskVehicles.Cast<TMPEN.TempRiskVehicle>().ToList();

                List<COMM.Branch> entityBranches = view.Branches.Cast<COMM.Branch>().ToList();

                List<COMM.Prefix> entityPrefix = view.Prefixes.Cast<COMM.Prefix>().ToList();

                List<TMPEN.TempPayerComponent> entityPayerComponents = view.TempPayerComponents.Cast<TMPEN.TempPayerComponent>().ToList();

                List<PROD.Product> entityProducts = view.Products.Cast<PROD.Product>().ToList();

                List<TMPEN.TempRiskCoverage> entityTempRiskCoverages = view.TempRiskCoverages.Cast<TMPEN.TempRiskCoverage>().ToList();

                foreach (var TempSuscriptions in entityTempSuscriptions)
                {
                    var list = entityTempSuscriptions.Where(x => x.QuotationId.Equals(TempSuscriptions.QuotationId)).ToList();
                    //TempSuscriptions. = Endorsements.Description + " (" + list.Count() + ")";

                }

                return EntityAssembler.CreateQuotationVehicleSearch(entityTempSuscriptions, entityTempRiskVehicles, entityBranches, entityPrefix, entityPayerComponents, entityProducts, entityTempRiskCoverages);
            }
            else
            {
                return null;
            }
        }

        public void UpdatePrintedDateByTempId(int tempId)
        {
            try
            {
                PrimaryKey key = TMPEN.TempSubscription.CreatePrimaryKey(tempId);
                TMPEN.TempSubscription entityTempSubscription = (TMPEN.TempSubscription)DataFacadeManager.GetObject(key);

                entityTempSubscription.PrintedDate = DateTime.Now;

                DataFacadeManager.Update(entityTempSubscription);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdatePrintedDateByTempId", ex);
            }
        }
        #endregion
    }
}