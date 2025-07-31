using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using CiaCommonMoldel = Sistran.Company.Application.CommonServices.Models;
using COISSView = Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views;
using COMM = Sistran.Company.Application.Common.Entities;
using COMMMO = Sistran.Core.Application.CommonService.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using CORECOMM = Sistran.Core.Application.Common.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using ISSView = Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using QUO = Sistran.Core.Application.Quotation.Entities;
using quotationEntitiesCore = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
using UNIQUEUSER = Sistran.Core.Application.UniqueUser.Entities;
using UPENT = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    using System.Globalization;
    using System.Text;
    using Core.Application.AuthorizationPoliciesServices.Enums;
    using Core.Application.Utilities.Enums;
    using Core.Application.Utilities.RulesEngine;
    using Sistran.Company.Application.Issuance.Entities;
    using Sistran.Company.Application.UnderwritingServices.DTOs;
    using Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper;
    using Sistran.Core.Application.Product.Entities;
    using Sistran.Core.Application.RulesScriptsServices.Models;
    using Sistran.Core.Application.UniqueUserServices.Enums;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using COUN = Sistran.Company.Application.UnderwritingServices.Enums;

    public class PolicyDAO
    {
        /// <summary>
        /// Obtener Poliza por 
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="policyId">Id endoso</param>
        /// <returns>Objeto policy</returns>
        public List<CompanyJustificationSarlaft> GetJustificationSarlaft()
        {
            BusinessCollection<COMM.JustificationReason> listJustification = null;

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                listJustification = daf.List<COMM.JustificationReason>(null);
            }

            if (listJustification != null)
            {
                return ModelAssembler.JustificationReasonList(listJustification);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Obtener Poliza por 
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="policyId">Id endoso</param>
        /// <returns>Objeto policy</returns>
        public CompanyPolicy GetTemporalPolicyByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            PolicyView view = new PolicyView();
            ViewBuilder builder = new ViewBuilder("PolicyView");
            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (!view.Policies.Any())
            {
                return null;
            }
            ISSEN.Endorsement entityEndorsement = (ISSEN.Endorsement)view.Endorsements[0];
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.LoadDynamicProperties(entityEndorsement);
            }
            CompanyPolicy policy = null;
            if (view.Policies.Any() && view.CoPolicies.Any())
            {
                ISSEN.EndorsementPayer endorsementPayer = null;
                if (view.EndorsementPayers.Any())
                {
                    endorsementPayer = (ISSEN.EndorsementPayer)view.EndorsementPayers[0];
                }

                CompanyPolicyMapper companyPolicyMapper = new CompanyPolicyMapper();
                companyPolicyMapper.policy = (ISSEN.Policy)view.Policies[0];
                companyPolicyMapper.coPolicy = (ISSEN.CoPolicy)view.CoPolicies[0];
                companyPolicyMapper.endorsement = entityEndorsement;
                companyPolicyMapper.endorsementPayer = endorsementPayer;
                companyPolicyMapper.tempPolicyControl = (ISSEN.TempPolicyControl)view.TempPolicyControl[0];
                policy = ModelAssembler.CreateCompanyPolicy(companyPolicyMapper);
                policy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                int holderNameNum = policy.Holder.CompanyName.NameNum;

                List<CompanyName> companyNames = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(policy.Holder.IndividualId, CustomerType.Individual);
                CompanyName companyName = new CompanyName();
                HolderDAO holderDAO = new HolderDAO();
                policy.Holder = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                if (holderNameNum == 0)
                {
                    companyName = companyNames.First(x => x.IsMain);

                }
                else
                {
                    companyName = companyNames.First(x => x.NameNum == holderNameNum);
                }
                policy.Holder.CompanyName = new IssuanceCompanyName
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
                policy.Agencies = ModelAssembler.CreateAgencies(view.PolicyAgents);


                List<ISSEN.CommissAgent> commissions = view.CommissAgents.Cast<ISSEN.CommissAgent>().ToList();

                foreach (ISSEN.CommissAgent item in commissions)
                {
                    policy.Agencies.First(x => x.Id == item.AgentAgencyId && x.Agent.IndividualId == item.IndividualId).Participation = item.AgentPartPercentage;
                    policy.Agencies.First(x => x.Id == item.AgentAgencyId && x.Agent.IndividualId == item.IndividualId).Commissions.Add(
                        new IssuanceCommission
                        {
                            Percentage = item.StComissionPercentage,
                            PercentageAdditional = item.AdditCommissPercentage.GetValueOrDefault(0),
                            SubLineBusiness = new COMMMO.SubLineBusiness
                            {
                                Id = item.SubLineBusinessCode.GetValueOrDefault(0),
                                LineBusiness = new COMMMO.LineBusiness
                                {
                                    Id = item.LineBusinessCode.GetValueOrDefault(0)
                                }
                            }
                        });
                }

                if (policy.BusinessType == BusinessType.Assigned)
                {
                    if (view.CoinsurancesAssigned != null && view.CoinsurancesAssigned.Count() > 0)
                    {
                        policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                        policy.CoInsuranceCompanies = ModelAssembler.CreateCoInsurancesAssigned(view.CoinsurancesAssigned);
                    }
                }
                else
                {
                    if (policy.BusinessType == BusinessType.Accepted)
                    {
                        if (view.CoinsurancesAccepted != null && view.CoinsurancesAccepted.Count() > 0)
                        {
                            policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                            policy.CoInsuranceCompanies.Add(ModelAssembler.CreateCoInsuranceAccepted((ISSEN.CoinsuranceAccepted)view.CoinsurancesAccepted[0]));

                        }
                    }

                }
            }
            return policy;
        }

        /// <summary>
        /// Creates the filter.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        private Dictionary<ObjectCriteriaBuilder, bool> CreateFilter(CompanyPolicy companyPolicy, ObjectCriteriaBuilder filter)
        {
            bool emptyFilter = true;
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            filter.And();
            if (companyPolicy.Branch != null && companyPolicy.Branch.Id > 0)
            {
                filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(companyPolicy.Branch.Id);
                emptyFilter = false;
            }
            if (companyPolicy.Prefix != null && companyPolicy.Prefix.Id > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(companyPolicy.Prefix.Id);
                emptyFilter = false;
            }
            if (companyPolicy.DocumentNumber > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(companyPolicy.DocumentNumber);
                emptyFilter = false;
            }
            if (companyPolicy.Holder != null && companyPolicy.Holder.IndividualId > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.PolicyholderId, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(companyPolicy.Holder.IndividualId);
                emptyFilter = false;
            }

            if (companyPolicy.UserId > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Endorsement.Properties.UserId, typeof(ISSEN.Endorsement).Name);
                filter.Equal();
                filter.Constant(companyPolicy.UserId);
                emptyFilter = false;
            }

            if (companyPolicy.CurrentFrom > DateTime.MinValue)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.CurrentFrom, typeof(ISSEN.Policy).Name);
                filter.GreaterEqual();
                filter.Constant(companyPolicy.CurrentFrom);
                emptyFilter = false;
            }

            if (companyPolicy.CurrentTo > DateTime.MinValue)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.CurrentFrom, typeof(ISSEN.Policy).Name);
                filter.LessEqual();
                filter.Constant(companyPolicy.CurrentTo);
            }
            Dictionary<ObjectCriteriaBuilder, bool> dictionary = new Dictionary<ObjectCriteriaBuilder, bool>();
            dictionary.Add(filter, emptyFilter);
            return dictionary;
        }

        public List<CompanyPolicy> GetCiaPoliciesByPolicy(CompanyPolicy companyPolicy)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            bool emptyFilter = true;
            List<int> operationIds = new List<int>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Dictionary<ObjectCriteriaBuilder, bool> dataFilter = CreateFilter(companyPolicy, filter);
            filter = dataFilter.FirstOrDefault().Key;
            emptyFilter = dataFilter.FirstOrDefault().Value;
            if (!string.IsNullOrWhiteSpace(companyPolicy.Endorsement.DescriptionRisk))
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
                filter.Like();
                filter.Constant("%" + companyPolicy.Endorsement.DescriptionRisk + "%");
                return GetPolicyByFilter(filter);
            }


            ISSView.EndorsementView view = new ISSView.EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Policies.Count > 0)
            {
                ConcurrentBag<CompanyPolicy> policies = new ConcurrentBag<CompanyPolicy>();
                var endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>(), x => x.Endorsement.EndorsementId, y => y.EndorsementId, (dc, d) => new { EndorsementRisk = d }).Where(x => x.EndorsementRisk.IsCurrent == true).ToList();
                TP.Parallel.ForEach(view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(endorsements, z => z.Endorsement.PolicyId, w => w.EndorsementRisk.PolicyId, (u, n) => new { Endorsement = u, EndorSementRisk = n }).Select(b => new { Endorsement = b.Endorsement.Endorsement, Policy = b.Endorsement.Policy }).Distinct(), item =>
                {
                    CompanyPolicy currentPolicy = new CompanyPolicy
                    {
                        Id = item.Policy.PolicyId,
                        IssueDate = item.Policy.IssueDate,
                        CurrentFrom = item.Policy.CurrentFrom,
                        CurrentTo = item.Policy.CurrentTo ?? DateTime.Now,
                        DocumentNumber = item.Policy.DocumentNumber,
                        Endorsement = new CompanyEndorsement
                        {
                            Id = item.Endorsement.EndorsementId,
                            CurrentFrom = item.Endorsement.CurrentFrom,
                            CurrentTo = item.Endorsement.CurrentTo.Value,
                            Description = item.Endorsement.DocumentNum.ToString(),
                            PolicyId = item.Endorsement.PolicyId,
                            EndorsementType = (EndorsementType)item.Endorsement.EndoTypeCode,
                            IsCurrent = view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault() == null ? default(bool) : ((ISSEN.EndorsementRisk)view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault()).IsCurrent
                        }

                    };

                    Task<CompanyProduct> ProductModel = TP.Task.Run(() =>
                    {
                        CompanyProduct result = CreateCompanyProductByProductId(item.Policy.ProductId.GetValueOrDefault(), item.Policy.PrefixCode);
                        DataFacadeManager.Dispose();
                        return result;
                    });

                    Task<List<Holder>> holderTask = TP.Task.Run(() =>
                    {
                        HolderDAO holderDAO = new HolderDAO();
                        List<Holder> result = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(item.Policy.PolicyholderId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                        DataFacadeManager.Dispose();
                        return result;

                    });

                    Task<List<Agency>> AgenciesTask = TP.Task.Run(() =>
                    {
                        List<Agency> result = CreateAgent(item.Endorsement.EndorsementId, currentPolicy.Id);
                        DataFacadeManager.Dispose();
                        return result;
                    });

                    Task<CompanySummary> summaryTask = TP.Task.Run(() =>
                    {
                        CompanySummary result = CreateSummary(item.Endorsement.EndorsementId);
                        DataFacadeManager.Dispose();
                        return result;
                    });
                    Task<CiaCommonMoldel.CompanyPrefix> prefixTask = TP.Task.Run(() =>
                    {
                        CiaCommonMoldel.CompanyPrefix result = CreatePrefixByPrefixId(item.Policy.PrefixCode);
                        DataFacadeManager.Dispose();
                        return result;
                    });
                    Task<CiaCommonMoldel.CompanyBranch> branchTask = TP.Task.Run(() =>
                    {
                        CiaCommonMoldel.CompanyBranch result = CreateBranchById(item.Policy.BranchCode);
                        DataFacadeManager.Dispose();
                        return result;
                    });
                    ProductModel.Wait();
                    AgenciesTask.Wait();
                    holderTask.Wait();
                    summaryTask.Wait();
                    prefixTask.Wait();
                    branchTask.Wait();
                    currentPolicy.Product = ProductModel.Result;
                    currentPolicy.Agencies = new List<IssuanceAgency>();
                    foreach (Agency agency in AgenciesTask.Result)
                    {
                        currentPolicy.Agencies.Add(new IssuanceAgency
                        {
                            Id = agency.Id,
                            FullName = agency.FullName,
                            Agent = new IssuanceAgent
                            {
                                IndividualId = agency.Agent.IndividualId,
                                FullName = agency.Agent.FullName
                            }
                        });
                    }

                    currentPolicy.Summary = summaryTask.Result;
                    if (currentPolicy.Summary != null && view.EndorsementRisks != null && view.EndorsementRisks.Any())
                    {
                        currentPolicy.Summary.RiskCount = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().Where(x => x.PolicyId == item.Policy.PolicyId).Distinct().Count();
                    }
                    currentPolicy.Prefix = prefixTask.Result;
                    currentPolicy.Branch = branchTask.Result;
                    if (holderTask.Result.Count > 0)
                    {
                        currentPolicy.Holder = holderTask.Result[0];
                    }
                    policies.Add(currentPolicy);
                });
                return policies.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Póliza
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Póliza</returns>
        public CompanyPolicy GetCompanyPolicyByTemporalId(int temporalId, bool isMasive)
        {
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();

            if (isMasive && boolUseReplicatedDatabase)
            {
            }
            else
            {
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(temporalId);
            }

            if (pendingOperation != null)
            {
                try
                {
                    CompanyPolicy companyPolicy = COMUT.JsonHelper.DeserializeJson<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.IsPersisted = true;
                    return companyPolicy;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private CompanyPolicy SaveTableTemporal(CompanyPolicy companyPolicy)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            companyPolicy = policyDAO.SaveTemporalPolicy(companyPolicy);
            return companyPolicy;
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        public CompanyPolicy CreatePolicyTemporal(CompanyPolicy policy, bool isMasive)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();

            policy.UserId = policy.UserId == 0 ? policy.User.UserId : policy.UserId;
            policy.User = new CompanyPolicyUser { UserId = policy.UserId };

            if (policy.Id == 0)
            {
                pendingOperation.UserId = policy.UserId;
                if (policy.Endorsement.EndorsementType != EndorsementType.Emission)
                {
                    try
                    {
                        if (DelegateService.utilitiesServiceCore.GetEndorsementControlById(policy.Endorsement.Id, policy.UserId))
                        {
                            pendingOperation.CreationDate = DateTime.Now;
                            pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(policy);
                        }
                        else
                        {
                            throw new ValidationException(Errors.ErrorUpdatePolicy);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    pendingOperation.CreationDate = DateTime.Now;
                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(policy);
                }

                if (isMasive && boolUseReplicatedDatabase)
                {
                }
                else
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                }
            }
            else
            {
                pendingOperation.ModificationDate = DateTime.Now;
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(policy.Id);
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(policy);
                pendingOperation.UserId = policy.UserId;

                if (isMasive && boolUseReplicatedDatabase)
                {
                }
                else
                {
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                }
            }

            policy.Id = pendingOperation.Id;

            if (!isMasive)
            {
                policy = SaveTableTemporal(policy);
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(policy);
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            return policy;
        }

        public Endorsement GetTemporalEndorsementByPolicyId(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TMPEN.TempSubscription.Properties.PolicyId, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(policyId);
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = daf.List(typeof(TMPEN.TempSubscription), filter.GetPredicate());
            }

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreateEndorsementByTempSubscription((TMPEN.TempSubscription)businessCollection[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Obtener Póliza Por Identificador
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Póliza</returns>
        public CompanyPolicy GetCompanyPolicyByEndorsementId(int endorsementId)
        {
            CompanyPolicy companyPolicy = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.EndorsementId, typeof(ISSEN.EndorsementOperation).Name).Equal().Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name).IsNull();

            EndorsementOperationViewC view = new EndorsementOperationViewC();
            ViewBuilder builder = new ViewBuilder("EndorsementOperationViewC");
            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            try
            {
                if (view.EndorsementOperations.Count > 0)
                {
                    if (view.TempPolicyControl.Count() > 0)
                    {
                        companyPolicy = ModelAssembler.CreateCompanyPolicyByEndorsement(view.EndorsementOperations.Cast<ISSEN.EndorsementOperation>().First(), view.Policies.Cast<ISSEN.Policy>().First(), view.Endorsements.Cast<ISSEN.Endorsement>().First(), view.TempPolicyControl.Cast<ISSEN.TempPolicyControl>().First());
                    }
                    else
                    {
                        ISSEN.TempPolicyControl tempPolicyControl = new ISSEN.TempPolicyControl(1);
                        companyPolicy = ModelAssembler.CreateCompanyPolicyByEndorsement(view.EndorsementOperations.Cast<ISSEN.EndorsementOperation>().First(), view.Policies.Cast<ISSEN.Policy>().First(), view.Endorsements.Cast<ISSEN.Endorsement>().First(), tempPolicyControl);
                    }
                    if (companyPolicy.ExchangeRate != null && companyPolicy.ExchangeRate.Currency != null)
                    {
                        DateTime IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                        companyPolicy.ExchangeRate.SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId
                            (IssueDate, companyPolicy.ExchangeRate.Currency.Id).SellAmount;
                    }
                    var emails = DelegateService.uniquePersonServiceCore.GetEmailsByIndividualId(companyPolicy.Holder.IndividualId);
                    if (emails.Count > 0)
                    {
                        foreach (var email in emails)
                        {
                            if (email.IsPrincipal == false)
                            {
                                companyPolicy.Holder.Email = email.Description;
                            }

                        }
                    }
                    var fiscalResponsibilty = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityByIndividualId(companyPolicy.Holder.IndividualId);
                    if (fiscalResponsibilty.Count > 0)
                    {
                        companyPolicy.Holder.FiscalResponsibility = fiscalResponsibilty;
                    }

                    var insured = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(companyPolicy.Holder.IndividualId);
                    if (insured != null)
                    {
                        companyPolicy.Holder.RegimeType = insured.RegimeType;
                    }

                    foreach (IssuanceAgency agency in companyPolicy.Agencies)
                    {
                        if (agency.Branch == null)
                        {
                            agency.Branch = new COMMMO.Branch();
                            agency.Branch.Id = (int)companyPolicy.Branch.Id;
                            agency.Branch.SalePoints = new List<COMMMO.SalePoint>();
                            agency.Branch.SalePoints.Add(new COMMMO.SalePoint() { Id = (int)companyPolicy.Branch.SalePoints.FirstOrDefault().Id });
                        }
                    }

                    if (companyPolicy.Product != null)
                    {
                        return companyPolicy;
                    }

                }
                return GetCompanyPolicyFromTables(endorsementId);

            }
            catch (Exception)
            {
                return GetCompanyPolicyFromTables(endorsementId);
            }
        }


        public int GetAppSourceByEndorsementId(int endorsementId, int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.TempPolicyControl.Properties.EndorsementId, typeof(ISSEN.TempPolicyControl).Name).Equal().Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.TempPolicyControl.Properties.PolicyId, typeof(ISSEN.TempPolicyControl).Name).Equal().Constant(policyId);

            var aplication = 0;

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.TempPolicyControl), filter.GetPredicate()));
            ISSEN.TempPolicyControl coPolicyType = businessCollection.Cast<ISSEN.TempPolicyControl>().FirstOrDefault();
            if (coPolicyType != null)
            {
                aplication = coPolicyType.AppVersionId;
            }
            return aplication;
        }

        /// <summary>
        /// Metodo para devolver Json de poliza del esquema report
        /// </summary>
        /// <param name="prefixId">ramo </param>
        /// <param name="branchId">sucursal</param>
        /// <param name="documentNumber">numero de poliza</param>
        /// <param name="endorsementType"> tipo de endos</param>
        /// <returns>modelo company policy</returns>
        public String GetPolicyByEndorsementDocumentNumber(int endorsementId, decimal documentNumber)
        {
            CompanyPolicy companyPolicy = new CompanyPolicy();
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", endorsementId);
            parameters[1] = new NameValue("@ONLY_POLICY", 1);

            object result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("REPORT.REPORT_GET_OPERATION", parameters);
            }

            if (result != null)
            {
                return result.ToString();
            }
            return null;
        }

        /// <summary>
        /// Obtener el JSON de la Póliza Por Identificador
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Póliza</returns>
        public String GetCompanyPolicyJsonByEndorsementId(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.EndorsementId, typeof(ISSEN.EndorsementOperation).Name).Equal().Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name).IsNull();

            BusinessCollection businessCollection = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.EndorsementOperation), filter.GetPredicate()));
            }

            if (businessCollection != null && businessCollection.Count > 0)
            {
                return businessCollection.Cast<ISSEN.EndorsementOperation>().First().Operation;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Poliza por 
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="policyId">Id endoso</param>
        /// <returns>Objeto policy</returns>
        private CompanyPolicy GetCompanyPolicyFromTables(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            PolicyView view = new PolicyView();
            ViewBuilder builder = new ViewBuilder("PolicyView");
            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Endorsements.Count > 0)
            {
                ISSEN.Endorsement entityEndorsement = (ISSEN.Endorsement)view.Endorsements[0];

                using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.LoadDynamicProperties(entityEndorsement);
                }

                if (view.Policies.Count > 0 && view.CoPolicies.Count > 0)
                {
                    if (((ISSEN.Policy)view.Policies[0] != null) && ((ISSEN.CoPolicy)view.CoPolicies[0] != null) && (entityEndorsement != null))
                    {
                        CompanyPolicy policy = new CompanyPolicy();
                        if (view.TempPolicyControl.Count > 0)
                        {
                            CompanyPolicyMapper companyPolicyMapper = new CompanyPolicyMapper();
                            companyPolicyMapper.policy = (ISSEN.Policy)view.Policies[0];
                            companyPolicyMapper.coPolicy = (ISSEN.CoPolicy)view.CoPolicies[0];
                            companyPolicyMapper.endorsement = entityEndorsement;
                            if (view.EndorsementPayers != null & view.EndorsementPayers.Count > 0)
                            {
                                companyPolicyMapper.endorsementPayer = (ISSEN.EndorsementPayer)view.EndorsementPayers[0];
                            }
                            companyPolicyMapper.tempPolicyControl = (ISSEN.TempPolicyControl)view.TempPolicyControl[0];
                            policy = ModelAssembler.CreateCompanyPolicy(companyPolicyMapper);
                        }
                        else
                        {
                            ISSEN.TempPolicyControl tempPolicyControl = new ISSEN.TempPolicyControl(1);

                            CompanyPolicyMapper companyPolicyMapper = new CompanyPolicyMapper();
                            companyPolicyMapper.policy = (ISSEN.Policy)view.Policies[0];
                            companyPolicyMapper.coPolicy = (ISSEN.CoPolicy)view.CoPolicies[0];
                            companyPolicyMapper.endorsement = entityEndorsement;
                            if (view.EndorsementPayers != null & view.EndorsementPayers.Count > 0)
                            {
                                companyPolicyMapper.endorsementPayer = (ISSEN.EndorsementPayer)view.EndorsementPayers[0];
                            }
                            companyPolicyMapper.tempPolicyControl = tempPolicyControl;
                            policy = ModelAssembler.CreateCompanyPolicy(companyPolicyMapper);

                        }

                        policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);

                        int holderNameNum = policy.Holder.CompanyName.NameNum;

                        List<CompanyName> companyNames = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(policy.Holder.IndividualId, CustomerType.Individual);

                        HolderDAO holderDAO = new HolderDAO();
                        policy.Holder = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        CompanyName companyName = new CompanyName();
                        if (holderNameNum == 0)
                        {
                            companyName = companyNames.FirstOrDefault(x => x.Address != null);
                        }
                        else
                        {
                            companyName = companyNames.First(x => x.NameNum == holderNameNum);
                        }
                        policy.Holder.CompanyName = new IssuanceCompanyName
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
                        policy.Agencies = ModelAssembler.CreateAgencies(view.PolicyAgents);
                        foreach (IssuanceAgency agency in policy.Agencies)
                        {
                            List<Agency> agent = DelegateService.uniquePersonService.GetAgencyByIndividualId(agency.Agent.IndividualId);
                            if (agent != null && agent.Count > 0)
                            {
                                agency.Code = agent.FirstOrDefault().Code;
                                agency.Agent.AgentType = new IssuanceAgentType { Id = agent.FirstOrDefault().AgentType.Id };
                                agency.Agent.DateDeclined = agent.FirstOrDefault().DateDeclined;
                                agency.DateDeclined = agent.FirstOrDefault().DateDeclined;

                                if (agency.Branch == null)
                                {
                                    agency.Branch = new COMMMO.Branch();
                                    agency.Branch.Id = (int)policy.Branch.Id;
                                    agency.Branch.SalePoints = new List<COMMMO.SalePoint>();
                                    agency.Branch.SalePoints.Add(new COMMMO.SalePoint() { Id = (int)policy.Branch.SalePoints.FirstOrDefault().Id });
                                }
                            }
                        }
                        List<ISSEN.CommissAgent> commissions = view.CommissAgents.Cast<ISSEN.CommissAgent>().ToList();

                        foreach (ISSEN.CommissAgent item in commissions)
                        {
                            policy.Agencies.First(x => x.Id == item.AgentAgencyId && x.Agent.IndividualId == item.IndividualId).Participation = item.AgentPartPercentage;
                            policy.Agencies.First(x => x.Id == item.AgentAgencyId && x.Agent.IndividualId == item.IndividualId).Commissions.Add(
                                new IssuanceCommission
                                {
                                    Percentage = item.StComissionPercentage,
                                    PercentageAdditional = item.AdditCommissPercentage.GetValueOrDefault(0),
                                    SubLineBusiness = new COMMMO.SubLineBusiness
                                    {
                                        Id = item.SubLineBusinessCode.GetValueOrDefault(0),
                                        LineBusiness = new COMMMO.LineBusiness
                                        {
                                            Id = item.LineBusinessCode.GetValueOrDefault(0)
                                        }
                                    }
                                });
                        }

                        if (policy.BusinessType == BusinessType.Assigned)
                        {
                            if (view.CoinsurancesAssigned != null && view.CoinsurancesAssigned.Count() > 0)
                            {
                                policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                                policy.CoInsuranceCompanies = ModelAssembler.CreateCoInsurancesAssigned(view.CoinsurancesAssigned);
                            }
                        }
                        else
                        {
                            if (policy.BusinessType == BusinessType.Accepted)
                            {
                                if (view.CoinsurancesAccepted != null && view.CoinsurancesAccepted.Count() > 0)
                                {
                                    policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                                    policy.CoInsuranceCompanies.Add(ModelAssembler.CreateCoInsuranceAccepted((ISSEN.CoinsuranceAccepted)view.CoinsurancesAccepted[0]));
                                }
                            }
                        }
                        if (policy.ExchangeRate != null && policy.ExchangeRate.Currency != null)
                        {
                            DateTime IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                            policy.ExchangeRate.SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId
                                (IssueDate, policy.ExchangeRate.Currency.Id).SellAmount;
                        }

                        var emails = DelegateService.uniquePersonServiceCore.GetEmailsByIndividualId(policy.Holder.IndividualId);
                        if (emails.Count > 0)
                        {
                            foreach (var email in emails)
                            {
                                if (email.EmailType.Id == 23)
                                {
                                    policy.Holder.Email = email.Description;
                                }

                            }
                        }
                        var fiscalResponsibilty = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityByIndividualId(policy.Holder.IndividualId);
                        if (fiscalResponsibilty.Count > 0)
                        {
                            policy.Holder.FiscalResponsibility = fiscalResponsibilty;
                        }

                        var insured = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(policy.Holder.IndividualId);
                        if (insured != null)
                        {
                            policy.Holder.RegimeType = insured.RegimeType;
                        }


                        return policy;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public CompanyPolicy CreateCompanyPolicy(CompanyPolicy companyPolicy)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[49];
            parameters[0] = new NameValue("@DOCUMENT_NUM", companyPolicy.DocumentNumber);
            parameters[1] = new NameValue("@POLICYHOLDER_ID", companyPolicy.Holder.IndividualId);
            parameters[2] = new NameValue("@PREFIX_CD", companyPolicy.Prefix.Id);
            parameters[3] = new NameValue("@BRANCH_CD", companyPolicy.Branch.Id);
            parameters[4] = new NameValue("@ENDO_TYPE_CD", (int)companyPolicy.Endorsement.EndorsementType);
            parameters[5] = new NameValue("@CURRENCY_CD", companyPolicy.ExchangeRate.Currency.Id);
            parameters[6] = new NameValue("@USER_ID", companyPolicy.UserId);
            parameters[7] = new NameValue("@EXCHANGE_RATE", companyPolicy.ExchangeRate.SellAmount);
            parameters[8] = new NameValue("@ISSUE_DATE", companyPolicy.IssueDate);
            parameters[9] = new NameValue("@CURRENT_FROM", companyPolicy.CurrentFrom);
            parameters[10] = new NameValue("@CURRENT_TO", companyPolicy.CurrentTo);
            parameters[11] = new NameValue("@BEGIN_DATE", companyPolicy.BeginDate);
            if (companyPolicy.Holder.CompanyName.Address != null)
                parameters[12] = new NameValue("@MAIL_ADDRESS_ID", companyPolicy.Holder.CompanyName.Address.Id);
            else
                parameters[12] = new NameValue("@MAIL_ADDRESS_ID", 1);

            if (companyPolicy.Branch.SalePoints != null && companyPolicy.Branch.SalePoints.Count > 0 && companyPolicy.Branch.SalePoints[0].Id > 0)
            {
                parameters[13] = new NameValue("@SALE_POINT_CD", companyPolicy.Branch.SalePoints[0].Id);
            }
            else
            {
                parameters[13] = new NameValue("@SALE_POINT_CD", DBNull.Value, DbType.Int32);
            }

            parameters[14] = new NameValue("@PRODUCT_ID", companyPolicy.Product.Id);
            if (companyPolicy.Endorsement.PolicyId > 0)
            {
                parameters[15] = new NameValue("@PREV_POLICY_ID", companyPolicy.Endorsement.PolicyId);
            }
            else
            {
                parameters[15] = new NameValue("@PREV_POLICY_ID", DBNull.Value, DbType.Int32);
            }

            if (companyPolicy.Text != null && !string.IsNullOrEmpty(companyPolicy.Text.TextBody))
            {
                parameters[16] = new NameValue("@CONDITION_TEXT", companyPolicy.Text.TextBody);
            }
            else
            {
                parameters[16] = new NameValue("@CONDITION_TEXT", DBNull.Value, DbType.String);
            }

            if (companyPolicy.Endorsement.EndorsementReasonId > 0)
            {
                parameters[17] = new NameValue("@ENDO_REASON_CD", companyPolicy.Endorsement.EndorsementReasonId);
            }
            else
            {
                parameters[17] = new NameValue("@ENDO_REASON_CD", DBNull.Value, DbType.Int32);
            }

            if (companyPolicy.Endorsement.ReferenceEndorsementId > 0)
            {
                parameters[18] = new NameValue("@REF_ENDORSEMENT_ID", companyPolicy.Endorsement.ReferenceEndorsementId);
            }
            else
            {
                parameters[18] = new NameValue("@REF_ENDORSEMENT_ID", DBNull.Value, DbType.Int32);
            }

            if (companyPolicy.Endorsement.EndorsementReasonDescription != null && companyPolicy.Endorsement.EndorsementReasonId > 0)
            {
                parameters[19] = new NameValue("@TEXT_REASON", companyPolicy.Endorsement.EndorsementReasonDescription);
            }
            else
            {
                parameters[19] = new NameValue("@TEXT_REASON", DBNull.Value, DbType.String);
            }

            if (companyPolicy.Text != null && !string.IsNullOrEmpty(companyPolicy.Text.Observations))
            {
                parameters[20] = new NameValue("@ANNOTATIONS", companyPolicy.Text.Observations);
            }
            else
            {
                parameters[20] = new NameValue("@ANNOTATIONS", DBNull.Value, DbType.String);
            }

            if (companyPolicy.Endorsement.Text != null && !string.IsNullOrEmpty(companyPolicy.Endorsement.Text.TextBody))
            {
                parameters[16] = new NameValue("@CONDITION_TEXT", companyPolicy.Endorsement.Text.TextBody);
            }
            if (companyPolicy.Endorsement.Text != null && !string.IsNullOrEmpty(companyPolicy.Endorsement.Text.Observations))
            {
                parameters[20] = new NameValue("@ANNOTATIONS", companyPolicy.Endorsement.Text.Observations);
            }
            parameters[21] = new NameValue("@BUSINESS_TYPE_CD", (int)companyPolicy.BusinessType);
            if (companyPolicy.CoInsuranceCompanies != null && companyPolicy.CoInsuranceCompanies.Count > 0)
            {
                if (companyPolicy.BusinessType == BusinessType.Accepted)
                {
                    parameters[22] = new NameValue("@COISSUE_PCT", 100);
                }
                else
                {
                    parameters[22] = new NameValue("@COISSUE_PCT", companyPolicy.CoInsuranceCompanies[0].ParticipationPercentageOwn);
                }
            }
            else
            {
                parameters[22] = new NameValue("@COISSUE_PCT", 0);
            }
            parameters[23] = new NameValue("@POLICY_TYPE_CD", companyPolicy.PolicyType.Id);

            if (companyPolicy.Request != null && companyPolicy.Request.Id > 0)
            {
                parameters[24] = new NameValue("@REQUEST_ID", companyPolicy.Request.Id);
            }
            else
            {
                parameters[24] = new NameValue("@REQUEST_ID", DBNull.Value, DbType.Int32);
            }

            parameters[25] = new NameValue("@PAYMENT_SCHEDULE_ID", companyPolicy.PaymentPlan.Id);
            parameters[26] = new NameValue("@PAYMENT_METHOD_CD", companyPolicy.Holder.PaymentMethod.Id);
            parameters[27] = new NameValue("@PAYMENT_ID", companyPolicy.Holder.PaymentMethod.PaymentId);

            if (companyPolicy.Endorsement.QuotationId > 0)
            {
                parameters[28] = new NameValue("@QUOTATION_ID", companyPolicy.Endorsement.QuotationId);
            }
            else
            {
                parameters[28] = new NameValue("@QUOTATION_ID", DBNull.Value, DbType.Int32);
            }

            DataTable dtAgencies = new DataTable("INSERT_TEMP_SUBSCRIPTION_AGENT");
            dtAgencies.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dtAgencies.Columns.Add("AGENT_AGENCY_ID", typeof(int));
            dtAgencies.Columns.Add("IS_PRIMARY", typeof(byte));
            dtAgencies.Columns.Add("SALE_POINT_CD", typeof(int));

            DataTable dtCommissions = new DataTable("INSERT_TEMP_COMMISS_AGENT");
            dtCommissions.Columns.Add("COMMISS_AGENT_ID", typeof(int));
            dtCommissions.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dtCommissions.Columns.Add("AGENT_AGENCY_ID", typeof(int));
            dtCommissions.Columns.Add("COMMISS_NUM", typeof(int));
            dtCommissions.Columns.Add("AGENT_PART_PCT", typeof(decimal));
            dtCommissions.Columns.Add("ST_COMMISS_PCT", typeof(decimal));
            
            dtCommissions.Columns.Add("ADDIT_COMMISS_PCT", typeof(decimal));
            dtCommissions.Columns.Add("ST_DISC_COMMISS_PCT", typeof(decimal));
            dtCommissions.Columns.Add("ADDIT_DISC_COMMISS_PCT", typeof(decimal));
            dtCommissions.Columns.Add("LINE_BUSINESS_CD", typeof(int));
            dtCommissions.Columns.Add("SUB_LINE_BUSINESS_CD", typeof(int));
            dtCommissions.Columns.Add("PREFIX_CD", typeof(int));
            dtCommissions.Columns.Add("SCH_COMMISS_PCT", typeof(decimal));
            dtCommissions.Columns.Add("INC_COMMISS_AD_FAC_PCT", typeof(decimal));
            dtCommissions.Columns.Add("DIS_COMMISS_AD_FAC_PCT", typeof(decimal));
            dtCommissions.Columns.Add("ORI_ST_COMMISS_PCT", typeof(decimal));
            dtCommissions.Columns.Add("ORI_SCH_COMMISS_PCT", typeof(decimal));
            dtCommissions.Columns.Add("ORI_ST_AGENT_COMMISS_PCT", typeof(decimal));
            dtCommissions.Columns.Add("AGENT_ST_COMMISS_PCT", typeof(decimal));

            int commissionNumber = 1;
            int commissionid = 1;
            foreach (IssuanceAgency agency in companyPolicy.Agencies)
            {
                DataRow dataRowAgency = dtAgencies.NewRow();
                dataRowAgency["INDIVIDUAL_ID"] = agency.Agent.IndividualId;
                dataRowAgency["AGENT_AGENCY_ID"] = agency.Id;
                dataRowAgency["IS_PRIMARY"] = agency.IsPrincipal ? 1 : 0;

                if (agency.Branch.SalePoints != null && agency.Branch.SalePoints.Count > 0 && agency.Branch.SalePoints[0].Id > 0)
                {
                    dataRowAgency["SALE_POINT_CD"] = agency.Branch.SalePoints[0].Id;
                }

                dtAgencies.Rows.Add(dataRowAgency);

                foreach (IssuanceCommission commission in agency.Commissions)
                {
                    DataRow dataRowCommission = dtCommissions.NewRow();
                    dataRowCommission["COMMISS_AGENT_ID"] = commissionid;
                    dataRowCommission["INDIVIDUAL_ID"] = agency.Agent.IndividualId;
                    dataRowCommission["AGENT_AGENCY_ID"] = agency.Id;
                    dataRowCommission["COMMISS_NUM"] = commissionNumber;
                    dataRowCommission["AGENT_PART_PCT"] = agency.Participation;
                    if (companyPolicy.Endorsement.EndorsementType != EndorsementType.ChangeAgentEndorsement && commission.AgentPercentage > 0)
                    {
                        commission.Percentage = (decimal)commission.AgentPercentage;
                        commission.AgentPercentage = 0;
                    }
                    dataRowCommission["ST_COMMISS_PCT"] = commission.Percentage;
                    dataRowCommission["ADDIT_COMMISS_PCT"] = commission.PercentageAdditional;
                    if (commission.SubLineBusiness != null)
                    {
                        dataRowCommission["LINE_BUSINESS_CD"] = commission.SubLineBusiness.LineBusiness.Id;
                        dataRowCommission["SUB_LINE_BUSINESS_CD"] = commission.SubLineBusiness.Id;
                    }
                    dataRowCommission["PREFIX_CD"] = companyPolicy.Prefix.Id;
                    dataRowCommission["ADDIT_COMMISS_PCT"] = commission.PercentageAdditional;
                    dataRowCommission["ADDIT_COMMISS_PCT"] = commission.PercentageAdditional;
                    dataRowCommission["ORI_ST_AGENT_COMMISS_PCT"] = agency.Participation;
                    dataRowCommission["AGENT_ST_COMMISS_PCT"] = commission.AgentPercentage == null ? 0 : commission.AgentPercentage;

                    dtCommissions.Rows.Add(dataRowCommission);
                    commissionNumber++;
                    commissionid++;
                }
            }


            parameters[29] = new NameValue("@INSERT_TEMP_SUBSCRIPTION_AGENT", dtAgencies);
            parameters[30] = new NameValue("@INSERT_TEMP_COMMISS_AGENT", dtCommissions);

            DataTable dtQuotas = new DataTable("INSERT_TEMP_PAYER_PAYMENT");
            dtQuotas.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtQuotas.Columns.Add("PAYER_ID", typeof(int));
            dtQuotas.Columns.Add("PAYMENT_NUM", typeof(int));
            dtQuotas.Columns.Add("PAY_EXP_DATE", typeof(DateTime));
            dtQuotas.Columns.Add("PAYMENT_PCT", typeof(decimal));
            dtQuotas.Columns.Add("AMOUNT", typeof(decimal));
            dtQuotas.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtQuotas.Columns.Add("PREFIX_CD", typeof(int));
            dtQuotas.Columns.Add("AGT_PAY_EXP_DATE", typeof(DateTime));

            if (companyPolicy.PaymentPlan.Quotas != null)
            {
                foreach (Quota quota in companyPolicy.PaymentPlan.Quotas)
                {
                    DataRow dataRowQuota = dtQuotas.NewRow();
                    dataRowQuota["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    dataRowQuota["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    dataRowQuota["PAYMENT_NUM"] = quota.Number;
                    dataRowQuota["PAY_EXP_DATE"] = quota.ExpirationDate;
                    dataRowQuota["PAYMENT_PCT"] = quota.Percentage;
                    dataRowQuota["AMOUNT"] = quota.Amount;
                    dataRowQuota["PREFIX_CD"] = companyPolicy.Prefix.Id;

                    dtQuotas.Rows.Add(dataRowQuota);
                }
            }

            parameters[31] = new NameValue("@INSERT_TEMP_PAYER_PAYMENT", dtQuotas);

            DataTable dtPayerComponents = new DataTable("INSERT_TEMP_PAYER_COMP");
            dtPayerComponents.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtPayerComponents.Columns.Add("PAYER_ID", typeof(int));
            dtPayerComponents.Columns.Add("COMPONENT_CD", typeof(int));
            dtPayerComponents.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtPayerComponents.Columns.Add("RATE", typeof(decimal));
            dtPayerComponents.Columns.Add("CALC_BASE_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("COMPONENT_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("ADDIT_RATE_TYPE_CD", typeof(int));
            dtPayerComponents.Columns.Add("ADDIT_RATE", typeof(decimal));
            dtPayerComponents.Columns.Add("ADDIT_CALC_BASE_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("ADDIT_COMPONENT_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("LINE_BUSINESS_CD", typeof(int));
            dtPayerComponents.Columns.Add("STATE_CD", typeof(int));
            dtPayerComponents.Columns.Add("COUNTRY_CD", typeof(int));
            dtPayerComponents.Columns.Add("ECONOMIC_ACTIVITY_CD", typeof(int));
            dtPayerComponents.Columns.Add("COVERAGE_ID", typeof(int));
            dtPayerComponents.Columns.Add("TAX_CD", typeof(int));
            dtPayerComponents.Columns.Add("TAX_CATEGORY_CD", typeof(int));
            dtPayerComponents.Columns.Add("TAX_CONDITION_CD", typeof(int));
            dtPayerComponents.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtPayerComponents.Columns.Add("PREFIX_CD", typeof(int));
            dtPayerComponents.Columns.Add("EXEMPTION_PCT", typeof(decimal));
            dtPayerComponents.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dtPayerComponents.Columns.Add("COMPONENT_AMOUNT_LOCAL", typeof(decimal));
            if (companyPolicy.PayerComponents != null)
            {
                foreach (CompanyPayerComponent payerComponent in companyPolicy.PayerComponents)
                {
                    DataRow dataRow = dtPayerComponents.NewRow();
                    dataRow["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    dataRow["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    dataRow["COMPONENT_CD"] = payerComponent.Component.Id;
                    object rateTypeCd = payerComponent.Component.Id == 1 ? 1 : (object)payerComponent.RateType;
                    dataRow["RATE_TYPE_CD"] = rateTypeCd;
                    dataRow["RATE"] = Math.Round((double)payerComponent.Rate, 2);
                    dataRow["CALC_BASE_AMT"] = Math.Round((double)payerComponent.BaseAmount, 2);
                    dataRow["COMPONENT_AMT"] = Math.Round((double)payerComponent.Amount, 2);
                    dataRow["LINE_BUSINESS_CD"] = payerComponent.LineBusinessId;
                    if (companyPolicy.Holder.CompanyName.Address != null && companyPolicy.Holder.CompanyName.Address.City != null && companyPolicy.Holder.CompanyName.Address.City.State != null && companyPolicy.Holder.CompanyName.Address.City.State.Id > 0)
                    {
                        dataRow["STATE_CD"] = companyPolicy.Holder.CompanyName.Address.City.State.Id;
                        dataRow["COUNTRY_CD"] = companyPolicy.Holder.CompanyName.Address.City.State.Country.Id;
                    }
                    //dataRow["ECONOMIC_ACTIVITY_CD"] = companyPolicy.Holder.EconomicActivity.Id;

                    if (payerComponent != null && payerComponent.CoverageId > 0)
                    {
                        dataRow["COVERAGE_ID"] = payerComponent.CoverageId;
                    }

                    if (payerComponent.TaxId > 0)
                    {
                        dataRow["TAX_CD"] = payerComponent.TaxId;
                        dataRow["TAX_CONDITION_CD"] = payerComponent.TaxConditionId;
                    }

                    dataRow["PREFIX_CD"] = companyPolicy.Prefix.Id;

                    if (payerComponent.DynamicProperties != null && payerComponent.DynamicProperties.Count > 0)
                    {
                        DynamicPropertiesCollection dynamicCollectionPayer = new DynamicPropertiesCollection();
                        for (int i = 0; i < payerComponent.DynamicProperties.Count(); i++)
                        {
                            DynamicProperty dinamycProperty = new DynamicProperty();
                            dinamycProperty.Id = payerComponent.DynamicProperties[i].Id;
                            dinamycProperty.Value = payerComponent.DynamicProperties[i].Value;
                            dynamicCollectionPayer[i] = dinamycProperty;
                        }

                        byte[] serializedValuesPayer = dynamicPropertiesSerializer.Serialize(dynamicCollectionPayer);
                        dataRow["DYNAMIC_PROPERTIES"] = serializedValuesPayer;
                    }
                    else
                        dataRow["DYNAMIC_PROPERTIES"] = null;

                    dataRow["COMPONENT_AMOUNT_LOCAL"] = Math.Round((double)payerComponent.AmountLocal, 2);
                    dtPayerComponents.Rows.Add(dataRow);
                }
            }

            parameters[32] = new NameValue("@INSERT_TEMP_PAYER_COMP", dtPayerComponents);

            DataTable dtClauses = new DataTable("INSERT_TEMP_CLAUSE");
            dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyPolicy.Clauses != null)
            {
                foreach (var clause in companyPolicy.Clauses.ToList().GroupBy(x => x.Id).ToList())
                //  foreach (CompanyClause clause in companyPolicy.Clauses)
                {
                    DataRow dataRow = dtClauses.NewRow();
                    dataRow["CLAUSE_ID"] = clause.Key;

                    dtClauses.Rows.Add(dataRow);
                }
            }

            parameters[33] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);

            DataTable dtCoinsurances = new DataTable("INSERT_TEMP_COINSURANCE");
            dtCoinsurances.Columns.Add("INSURANCE_COMPANY_ID", typeof(int));
            dtCoinsurances.Columns.Add("PART_CIA_PCT", typeof(decimal));
            dtCoinsurances.Columns.Add("EXPENSES_PCT", typeof(decimal));
            dtCoinsurances.Columns.Add("PART_MAIN_PCT", typeof(decimal));
            dtCoinsurances.Columns.Add("ANNEX_NUM_MAIN", typeof(string));
            dtCoinsurances.Columns.Add("POLICY_NUM_MAIN", typeof(string));


            DataTable dtCoinsurancesAgents = new DataTable("INSERT_TEMP_COINSURANCE_AGENTS");

            dtCoinsurancesAgents.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtCoinsurancesAgents.Columns.Add("POLICY_ID", typeof(int));
            dtCoinsurancesAgents.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dtCoinsurancesAgents.Columns.Add("AGENT_AGENCY_ID", typeof(int));
            dtCoinsurancesAgents.Columns.Add("PART_CIA_PCT", typeof(decimal));

            if (companyPolicy.BusinessType != BusinessType.CompanyPercentage && companyPolicy.BusinessType != 0)
            {
                foreach (CompanyIssuanceCoInsuranceCompany coInsuranceCompany in companyPolicy.CoInsuranceCompanies)
                {
                    DataRow dataRow = dtCoinsurances.NewRow();
                    dataRow["INSURANCE_COMPANY_ID"] = coInsuranceCompany.Id;
                    dataRow["PART_CIA_PCT"] = coInsuranceCompany.ParticipationPercentage;
                    dataRow["EXPENSES_PCT"] = coInsuranceCompany.ExpensesPercentage;
                    dataRow["PART_MAIN_PCT"] = coInsuranceCompany.ParticipationPercentageOwn;
                    dataRow["ANNEX_NUM_MAIN"] = coInsuranceCompany.EndorsementNumber ?? string.Empty;
                    dataRow["POLICY_NUM_MAIN"] = coInsuranceCompany.PolicyNumber ?? string.Empty;

                    dtCoinsurances.Rows.Add(dataRow);
                    if (coInsuranceCompany.acceptCoInsuranceAgent != null)
                    {
                        foreach (CompanyAcceptCoInsuranceAgent AgentParticipation in coInsuranceCompany.acceptCoInsuranceAgent)
                        {
                            DataRow ParticipationAgent = dtCoinsurancesAgents.NewRow();
                            ParticipationAgent["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
                            ParticipationAgent["POLICY_ID"] = companyPolicy.Endorsement.PolicyId;
                            ParticipationAgent["INDIVIDUAL_ID"] = AgentParticipation.Agent.IndividualId;
                            ParticipationAgent["AGENT_AGENCY_ID"] = AgentParticipation.Agent.Id;
                            ParticipationAgent["PART_CIA_PCT"] = AgentParticipation.ParticipationPercentage;
                            dtCoinsurancesAgents.Rows.Add(ParticipationAgent);
                        }
                    }
                }
            }

            parameters[34] = new NameValue("@INSERT_TEMP_COINSURANCE", dtCoinsurances);
            parameters[35] = new NameValue("@DYNAMIC_PROPERTIES", DBNull.Value);
            parameters[36] = new NameValue("@NAME_NUM", DBNull.Value);

            if (companyPolicy.RequestEndorsement > 0)
            {
                parameters[37] = new NameValue("@REQUEST_ENDO", companyPolicy.RequestEndorsement);
            }
            else
            {
                parameters[37] = new NameValue("@REQUEST_ENDO", DBNull.Value);
            }

            DataTable dtDynamicProperties = new DataTable("INSERT_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));

            parameters[38] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES", dtDynamicProperties);

            DataTable dtDynamicPropertiesComponent = new DataTable("INSERT_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicPropertiesComponent.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicPropertiesComponent.Columns.Add("CONCEPT_VALUE", typeof(string));

            parameters[39] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES_COMPONENT", dtDynamicPropertiesComponent);

            DataTable dtPayerPaymentsComponents = new DataTable("PARAM_CO_TEMP_PAYER_PAYMENT_COMP");
            dtPayerPaymentsComponents.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("PAYER_ID", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("PAYMENT_NUM", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("COMPONENT_CD", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("PAYMENT_COMP_PCT", typeof(decimal));
            dtPayerPaymentsComponents.Columns.Add("COMPONENT_COMP_AMT", typeof(decimal));
            dtPayerPaymentsComponents.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("DATE_PAYMENT", typeof(DateTime));

            parameters[40] = new NameValue("@INSERT_CO_TEMP_PAYER_PAYMENT_COMP", dtPayerPaymentsComponents);

            if (companyPolicy.CorrelativePolicyNumber.GetValueOrDefault() > 0)
            {
                parameters[41] = new NameValue("@CORRELATIVE_POLICY_ID", companyPolicy.CorrelativePolicyNumber.Value);
            }
            else
            {
                parameters[41] = new NameValue("@CORRELATIVE_POLICY_ID", DBNull.Value, DbType.Byte);
            }

            object billingGroupCode = companyPolicy.Request != null ? (object)companyPolicy.Request.BillingGroupId : DBNull.Value;
            parameters[42] = new NameValue("BILLING_GROUP_CD", billingGroupCode);
            parameters[43] = new NameValue("OPERATION", JsonConvert.SerializeObject(companyPolicy));
            parameters[44] = new NameValue("OPERATION_ID", companyPolicy.Id);

            #region FillingWork_Flow 
            parameters[45] = new NameValue("FILING_NUMBER", companyPolicy.Endorsement.TicketNumber.ToString());
            DateTime date_now;

            if (companyPolicy.Endorsement.TicketDate != null)
            {
                if (companyPolicy.Endorsement.TicketDate == DateTime.MinValue)
                {
                    date_now = DateTime.Now;
                }
                else
                {
                    date_now = Convert.ToDateTime(companyPolicy.Endorsement.TicketDate.ToString());
                }
            }
            else
            {
                date_now = DateTime.Now;
            }

            parameters[46] = new NameValue("FILING_DATE", date_now);
            #endregion

            parameters[47] = new NameValue("@INSERT_TEMP_COINSURANCE_AGENTS", dtCoinsurancesAgents);
            parameters[48] = new NameValue("ENDO_TYPE_MOD_CD", companyPolicy.Endorsement.ModificationTypeId);
            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("ISS.RECORD_MSV_POLICY", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dataTable.Rows[0][0]))
                {
                    companyPolicy.DocumentNumber = (decimal)dataTable.Rows[0][1];
                    companyPolicy.Endorsement.Number = (int)dataTable.Rows[0][2];
                    companyPolicy.Endorsement.PolicyId = (int)dataTable.Rows[0][3];
                    companyPolicy.Endorsement.Id = (int)dataTable.Rows[0][4];
                    companyPolicy.Summary.RiskCount = (int)dataTable.Rows[0][5];
                    return companyPolicy;
                }
                else
                {
                    throw new ValidationException((string)dataTable.Rows[0][1]);
                }
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordEndorsement);
            }
        }

        public CompanyPolicy CreateCompanyPremiumFinance(CompanyPolicy companyPolicy)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[14];
            parameters[0] = new NameValue("@POLICY_ID", companyPolicy.Endorsement.PolicyId);
            parameters[1] = new NameValue("@ENDORSEMENT_ID", companyPolicy.Endorsement.Id);
            parameters[2] = new NameValue("@PROMISSORY_NOTE_NUM_CD", 0);
            parameters[3] = new NameValue("@PERCENTAGE_TO_FINANCE", companyPolicy.PaymentPlan.PremiumFinance.PercentagetoFinance);
            parameters[4] = new NameValue("@VALUE_TO_FINANCE", companyPolicy.PaymentPlan.PremiumFinance.FinanceToValue);
            parameters[5] = new NameValue("@FINANCING_RATE", companyPolicy.PaymentPlan.PremiumFinance.FinanceRate);
            parameters[6] = new NameValue("@FINANCE_MIN_VALUE", companyPolicy.PaymentPlan.PremiumFinance.MinimumValueToFinance);
            parameters[7] = new NameValue("@PROMISSORY_NOTE_STATUS_CD", (int)companyPolicy.PaymentPlan.PremiumFinance.StatePay);
            parameters[8] = new NameValue("@CURRENT_FROM", companyPolicy.PaymentPlan.PremiumFinance.CurrentFrom);
            parameters[9] = new NameValue("@CURRENT_TO", companyPolicy.PaymentPlan.PremiumFinance.CurrentTo);
            parameters[10] = new NameValue("@QUANTITY_OF_QUOTAS", companyPolicy.PaymentPlan.PremiumFinance.NumberQuotas);
            parameters[11] = new NameValue("@CREATION_DATE", DateTime.Now);
            parameters[12] = new NameValue("@BRANCH_CD", companyPolicy.Branch.Id);
            parameters[13] = new NameValue("@POLICYHOLDER_ID", companyPolicy.PaymentPlan.PremiumFinance.Insured.IndividualId);

            DataTable dataTable;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("ISS.CREATE_PAYER_FINANCIAL_PREMIUM", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                companyPolicy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode = Convert.ToInt32(dataTable.Rows[0][2]);
            }
            return companyPolicy;
        }

        public CompanyPremiumFinance GetCompanyNumberFinalcialPremium(int policyId)
        {
            PayerFinancialPremium payerFinancialPremium = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PayerFinancialPremium.Properties.PolicyId, typeof(PayerFinancialPremium).Name);
            filter.Equal();
            filter.Constant(policyId);

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                payerFinancialPremium = daf.List(typeof(PayerFinancialPremium), filter.GetPredicate()).Cast<PayerFinancialPremium>().FirstOrDefault();
            }
            CompanyPremiumFinance companyPremiumFinance = ModelAssembler.MappCompanyFinancialPremium(payerFinancialPremium);
            return companyPremiumFinance;
        }

        /// <summary>
        /// Metodo que realiza la emision la poliza desde politicas
        /// </summary>
        /// <param name="temporalId">Id del temporal</param>
        /// <returns></returns>
        public string CreatePolicyAuthorization(string temporalId)
        {
            string result = string.Empty;
            string notification = string.Empty;
            CompanyPolicy policy = this.GetCompanyPolicyByTemporalId(Convert.ToInt32(temporalId), false);
            try
            {
                if (policy != null)
                {

                    CompanyPolicy docNumber = this.CreateCompanyPolicy(policy);

                    if (policy.TemporalType == TemporalType.Policy)
                    {
                        result += "Se genero la poliza " + docNumber.DocumentNumber + "</br>";
                        notification = "Se genero la poliza " + docNumber.DocumentNumber;
                    }
                    else if (policy.TemporalType == TemporalType.Endorsement)
                    {
                        result += "El endoso se ha generado con exito." +
                                  "</br>Número de poliza " + Convert.ToInt32(policy.DocumentNumber) + "." +
                                  "</br>Número de endoso " + docNumber.DocumentNumber + "</br>";

                        notification = "Se genero el endoso " + docNumber.DocumentNumber + " para la poliza " + Convert.ToInt32(policy.DocumentNumber);
                    }

                    DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId, null, docNumber.DocumentNumber.ToString());

                    NotificationUser notificationUser = new NotificationUser
                    {
                        UserId = policy.UserId,
                        CreateDate = DateTime.Now,
                        NotificationType = new NotificationType { Type = NotificationTypes.Emission },
                        Message = notification,
                        Parameters = new Dictionary<string, object> { { "DocumentNumber", docNumber.DocumentNumber }, { "Branch", policy.Branch.Id }, { "Prefix", policy.Prefix.Id } }
                    };
                    DelegateService.uniqueUserService.CreateNotification(notificationUser);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Core.Framework.BAF.BusinessException(ex.Message, ex);
            }
            throw new Exception("El temporalId" + temporalId + " no es valido");
        }
        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyPolicy policy)
        {
            Rules.Facade facade = new Rules.Facade();

            if (policy.User == null)
            {
                policy.User = new CompanyPolicyUser();
                policy.User.UserId = policy.UserId;
            }
            policy.User.UserProfileId = GetProfileUser(policy.UserId);
            policy.Branch.GroupBranchId = GetGroupBranchId(policy.Branch.Id);

            facade.SetConcept(RuleConceptPolicies.UserId, policy.UserId);
            EntityAssembler.CreateFacadeGeneral(policy, facade);
            return DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, policy.Prefix.Id + "," + (int)policy.Product.CoveredRisk.CoveredRiskType, facade, FacadeType.RULE_FACADE_GENERAL);
        }

        /// <summary>
        /// Gets the company covered risk by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyCoveredRisk GetCompanyCoveredRiskByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
                bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "false";
                PendingOperation pendingOperation = new PendingOperation();

                if (isMasive && boolUseReplicatedDatabase)
                {
                }
                else
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(temporalId);
                }

                if (pendingOperation != null)
                {
                    CompanyPolicy companyPolicy = COMUT.JsonHelper.DeserializeJson<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = pendingOperation.Id;
                    return companyPolicy.Product.CoveredRisk;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorGetPolicyByPolicyIdEndorsementId);
            }
        }

        /// <summary>
        /// Metodo para guardar en ENDORSEMENT_OPERATION
        /// </summary>
        /// <param name="endorsementId">Id del endoso</param>
        /// <param name="pendingOperationId">Id de pending operation</param>
        public void RecordEndorsementOperation(int endorsementId, int pendingOperationId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@OPERATION_ID", pendingOperationId);
            parameters[1] = new NameValue("@ENDORSEMENT_ID", endorsementId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteSPScalar("ISS.RECORD_ENDORSEMENT_OPERATION", parameters);
            }
        }

        /// <summary>
        /// DeleteEndorsmentByPolicyIdEndorsementIdEndorsementType
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="endorsementType"></param>
        /// <returns></returns>
        public bool DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(int policyId, int endorsementId, EndorsementType endorsementType)
        {
            NameValue[] parameters = new NameValue[3];
            parameters[0] = new NameValue("@POLICY_ID", policyId);
            parameters[1] = new NameValue("@ENDORSEMENT_ID", endorsementId);
            parameters[2] = new NameValue("@ENDO_TYPE_CD", (int)endorsementType);
            object result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("ISS.POLICY_ROLLBACK", parameters);
            }
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public int GetCurrentRiskNumByPolicyId(int policyId)
        {
            SelectQuery selectQuery = new SelectQuery();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Function maxFunction = new Function(FunctionType.Max);
            filter.PropertyEquals(ISSEN.EndorsementRisk.Properties.PolicyId, "e", policyId);
            maxFunction.AddParameter(new Column(ISSEN.EndorsementRisk.Properties.RiskNum));
            selectQuery.AddSelectValue(new SelectValue(maxFunction));
            selectQuery.Table = new ClassNameTable(typeof(ISSEN.EndorsementRisk), "e");
            selectQuery.Where = filter.GetPredicate();
            int riskCount;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                reader.Read();
                riskCount = Convert.ToInt32(reader[0]);
            }
            return riskCount;
        }
        /// <summary>
        /// Obtener Endosos de una Póliza
        /// </summary>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <param name="branchId">Id Sucursal</param>
        /// <param name="policyNumber">Número de Póliza</param>
        /// <returns>Endosos</returns>
        public List<CompanyEndorsement> GetCiaEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, int riskId, bool isCurrent,bool? isExchange)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(branchId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(policyNumber);
            if (riskId > 0)
            {
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(riskId);
            }
            if (isCurrent)
            {
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(true);
            }

            ISSView.EndorsementView view = new ISSView.EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Policies != null && view.Policies.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                ConcurrentBag<CompanyEndorsement> endorsements = new ConcurrentBag<CompanyEndorsement>();

                TP.Parallel.ForEach(view.Endorsements.Cast<ISSEN.Endorsement>().ToList(), item =>
                {
                    CompanyPolicyControl Control = GetTemporalPolicyControl(item.PolicyId, item.EndorsementId);
                    endorsements.Add(new CompanyEndorsement
                    {
                        Id = item.EndorsementId,
                        CurrentFrom = item.CurrentFrom,
                        CurrentTo = item.CurrentTo.Value,
                        Description = item.DocumentNum.ToString(),
                        Number = item.DocumentNum,
                        PolicyId = item.PolicyId,
                        EndorsementType = (EndorsementType)item.EndoTypeCode,
                        ExchangeRate = isExchange==true ? item.ExchangeRate:0
                    });
                });

                ISSEN.EndorsementRisk endorsementRisk = endorsementRisks?.OrderByDescending(x => x.EndorsementId)?.FirstOrDefault(x => x.IsCurrent);
                if (endorsementRisk != null)
                {
                    endorsements.First(x => x.Id == endorsementRisk.EndorsementId).IsCurrent = true;
                    endorsements.First(x => x.Id == endorsementRisk.EndorsementId).RiskId = endorsementRisk.RiskId;
                }
                else
                {
                    throw new Exception(Errors.ErrorRiskNotFound);
                }
                return endorsements.ToList();
            }
            else
            {
                return null;
            }
        }
        private CompanyProduct CreateCompanyProductByProductId(int ProductId, int PrefixId)
        {
            AutoMapper.IMapper mapper = ModelAssembler.CreateCompanyProduct();
            return mapper.Map<Sistran.Core.Application.ProductServices.Models.Product, CompanyProduct>(DelegateService.productService.GetProductByProductIdPrefixId(ProductId, PrefixId));
        }

        public List<Agency> CreateAgent(int EndorsementId, int PolicyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PolicyAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
            filter.Equal();
            filter.Constant(EndorsementId);
            filter.And();
            filter.Property(ISSEN.PolicyAgent.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
            filter.Equal();
            filter.Constant(PolicyId);

            List<ISSEN.PolicyAgent> policyAgent = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                policyAgent = daf.List(typeof(ISSEN.PolicyAgent), filter.GetPredicate()).Cast<ISSEN.PolicyAgent>().OrderByDescending(b => b.IsPrimary).ToList();
            }
            ConcurrentBag<Agency> Agencies = new ConcurrentBag<Agency>();
            for (int i = 0; i < policyAgent.Count; i++)
            {
                Agency agency = new Agency();
                agency.Agent = new Agent();
                agency.Agent.IndividualId = policyAgent[i].IndividualId;
                Task<Agent> agentTask = TP.Task.Run(() => { return DelegateService.uniquePersonServiceV1.GetAgentByIndividualId(agency.Agent.IndividualId); });
                agentTask.Wait();
                agency.Agent = agentTask.Result;
                Agencies.Add(agency);
            }
            return Agencies.ToList();
        }
        public CompanySummary CreateSummary(int EndorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(quotationEntitiesCore.Component.Properties.ComponentTypeCode, typeof(quotationEntitiesCore.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(ComponentType.Premium);
            filter.Constant(ComponentType.Expenses);
            filter.Constant(ComponentType.Taxes);
            filter.EndList();
            List<quotationEntitiesCore.Component> components = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                components = daf.List(typeof(quotationEntitiesCore.Component), filter.GetPredicate()).Cast<quotationEntitiesCore.Component>().ToList();
            }

            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
            filter.Equal();
            filter.Constant(EndorsementId);
            List<ISSEN.PayerComp> payerComponents = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                payerComponents = daf.List(typeof(ISSEN.PayerComp), filter.GetPredicate()).Cast<ISSEN.PayerComp>().ToList();
            }

            if (payerComponents != null && components != null && payerComponents.Count > 0)
            {
                CompanySummary Summary = new CompanySummary();
                int premium = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Premium).ComponentCode;
                int Expenses = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Expenses).ComponentCode;
                int taxes = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Taxes).ComponentCode;
                Summary.Premium = payerComponents.Where(x => x != null && x.ComponentCode == premium).Sum(y => y.ComponentAmount);
                Summary.Expenses = payerComponents.Where(x => x != null && x.ComponentCode == Expenses).Sum(y => y.ComponentAmount);
                Summary.Taxes = payerComponents.Where(x => x != null && x.ComponentCode == taxes).Sum(y => y.ComponentAmount);

                Summary.FullPremium = Summary.Premium + Summary.Expenses + Summary.Taxes;
                return Summary;
            }
            else
            {
                return null;
            }
        }

        public CiaCommonMoldel.CompanyPrefix CreatePrefixByPrefixId(int PrefixId)
        {
            AutoMapper.IMapper mapper = ModelAssembler.CreateCompanyPrefix();
            return mapper.Map<COMMMO.Prefix, CiaCommonMoldel.CompanyPrefix>(DelegateService.commonService.GetPrefixById(PrefixId));
        }

        public CiaCommonMoldel.CompanyBranch CreateBranchById(int branchId)
        {
            AutoMapper.IMapper mapper = ModelAssembler.CreateCompanyBranch();
            return mapper.Map<COMMMO.Branch, CiaCommonMoldel.CompanyBranch>(DelegateService.commonService.GetBranchById(branchId));
        }
        /// <summary>
        /// </summary>
        /// <param name="branchId">Id Sucursal</param>
        /// <param name="policyNumber">Número de Póliza</param>
        public List<CompanyPolicy> GetPolicyByFilter(ObjectCriteriaBuilder filter)
        {
            COISSView.EndorsementVehicleView view = new COISSView.EndorsementVehicleView();
            ViewBuilder builder = new ViewBuilder("EndorsementVehicleView");
            builder.Filter = filter.GetPredicate();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Policies.Count > 0)
            {
                ConcurrentBag<CompanyPolicy> policies = new ConcurrentBag<CompanyPolicy>();
                var endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>(), x => x.Endorsement.EndorsementId, y => y.EndorsementId, (dc, d) => new { EndorsementRisk = d }).Where(x => x.EndorsementRisk.IsCurrent == true).ToList();
                TP.Parallel.ForEach(view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(endorsements, z => z.Endorsement.PolicyId, w => w.EndorsementRisk.PolicyId, (u, n) => new { Endorsement = u, EndorSementRisk = n }).Select(b => new { Endorsement = b.Endorsement.Endorsement, Policy = b.Endorsement.Policy }).Distinct(), item =>
                {
                    CompanyPolicy currentPolicy = new CompanyPolicy
                    {
                        Id = item.Policy.PolicyId,
                        IssueDate = item.Policy.IssueDate,
                        CurrentFrom = item.Policy.CurrentFrom,
                        CurrentTo = item.Policy.CurrentTo ?? DateTime.Now,
                        DocumentNumber = item.Policy.DocumentNumber,
                        Endorsement = new CompanyEndorsement
                        {
                            Id = item.Endorsement.EndorsementId,
                            CurrentFrom = item.Endorsement.CurrentFrom,
                            CurrentTo = item.Endorsement.CurrentTo.Value,
                            Description = item.Endorsement.DocumentNum.ToString(),
                            PolicyId = item.Endorsement.PolicyId,
                            EndorsementType = (EndorsementType)item.Endorsement.EndoTypeCode,
                            IsCurrent = view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault() == null ? default(bool) : ((ISSEN.EndorsementRisk)view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault()).IsCurrent
                        }

                    };

                    Task<CompanyProduct> ProductModel = TP.Task.Run(() =>
                    {
                        CompanyProduct result = CreateCompanyProductByProductId(item.Policy.ProductId.GetValueOrDefault(), item.Policy.PrefixCode);
                        DataFacadeManager.Dispose();
                        return result;
                    });

                    Task<List<Holder>> holderTask = TP.Task.Run(() =>
                    {
                        HolderDAO holderDAO = new HolderDAO();
                        List<Holder> result = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(item.Policy.PolicyholderId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                        DataFacadeManager.Dispose();
                        return result;
                    });

                    Task<List<Agency>> AgenciesTask = TP.Task.Run(() =>
                    {
                        List<Agency> result = CreateAgent(item.Endorsement.EndorsementId, currentPolicy.Id);
                        DataFacadeManager.Dispose();
                        return result;
                    });

                    Task<CompanySummary> summaryTask = TP.Task.Run(() =>
                    {
                        CompanySummary result = CreateSummary(item.Endorsement.EndorsementId);
                        DataFacadeManager.Dispose();
                        return result;
                    });

                    Task<CiaCommonMoldel.CompanyPrefix> prefixTask = TP.Task.Run(() =>
                    {
                        CiaCommonMoldel.CompanyPrefix result = CreatePrefixByPrefixId(item.Policy.PrefixCode);
                        DataFacadeManager.Dispose();
                        return result;
                    });
                    Task<CiaCommonMoldel.CompanyBranch> branchTask = TP.Task.Run(() =>
                    {
                        CiaCommonMoldel.CompanyBranch result = CreateBranchById(item.Policy.BranchCode);
                        DataFacadeManager.Dispose();
                        return result;
                    });
                    ProductModel.Wait();
                    AgenciesTask.Wait();
                    holderTask.Wait();
                    summaryTask.Wait();
                    prefixTask.Wait();
                    branchTask.Wait();
                    currentPolicy.Product = ProductModel.Result;
                    foreach (Agency agency in AgenciesTask.Result)
                    {
                        currentPolicy.Agencies.Add(new IssuanceAgency
                        {
                            Id = agency.Id,
                            FullName = agency.FullName,
                            Agent = new IssuanceAgent
                            {
                                IndividualId = agency.Agent.IndividualId,
                                FullName = agency.Agent.FullName
                            }
                        });
                    }
                    currentPolicy.Summary = summaryTask.Result;
                    if (currentPolicy.Summary != null && view.EndorsementRisks != null && view.EndorsementRisks.Any())
                    {
                        currentPolicy.Summary.RiskCount = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().Where(x => x.PolicyId == item.Policy.PolicyId).Distinct().Count();
                    }
                    currentPolicy.Prefix = prefixTask.Result;
                    currentPolicy.Branch = branchTask.Result;
                    if (holderTask.Result.Count > 0)
                    {
                        currentPolicy.Holder = holderTask.Result[0];
                    }
                    policies.Add(currentPolicy);
                });
                return policies.ToList();

            }
            else
            {
                return null;
            }
        }

        #region  Previsora

        public CompanyPolicy SaveTemporalPolicy(CompanyPolicy companyPolicy)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[16];

            DataTable dataTable;

            DataTable dtTableParam = CreateDataTableSubscription(companyPolicy);
            parameters[0] = new NameValue(dtTableParam.TableName, dtTableParam);

            DataTable dtTableParanCo = CreateDataTableCoSubscription(companyPolicy);
            parameters[1] = new NameValue(dtTableParanCo.TableName, dtTableParanCo);

            DataTable dtTableParam2g = CreateDataTableSubscription2G(companyPolicy);
            parameters[2] = new NameValue(dtTableParam2g.TableName, dtTableParam2g);


            DataTable dataTableParamAgent = CreateDataTableSubscriptionAgent(companyPolicy);
            parameters[3] = new NameValue(dataTableParamAgent.TableName, dataTableParamAgent);

            DataTable dataTableParamComissAgent = CreateDataTableCommissAgent(companyPolicy);
            parameters[4] = new NameValue(dataTableParamComissAgent.TableName, dataTableParamComissAgent);

            DataTable dataTableParamSubscriptionPayer = CreateDataTableSubscriptionPayer(companyPolicy);
            parameters[5] = new NameValue(dataTableParamSubscriptionPayer.TableName, dataTableParamSubscriptionPayer);

            DataTable dataTableClause = CreateDataTableClause(companyPolicy);
            parameters[6] = new NameValue(dataTableClause.TableName, dataTableClause);

            DataTable dataTableCoinsurance = CreateDataTableCoinsurance(companyPolicy);
            parameters[7] = new NameValue(dataTableCoinsurance.TableName, dataTableCoinsurance);

            DataTable dataTableParamDinamicProp = CreateDataTableDynamicProperties(companyPolicy);
            parameters[8] = new NameValue(dataTableParamDinamicProp.TableName, dataTableParamDinamicProp);

            DataTable dtQuotas = CreateDataTablePayerPayment(companyPolicy);
            parameters[9] = new NameValue(dtQuotas.TableName, dtQuotas);

            DataTable dtParamFirstPayer = CreateDataTableParamFirstPayer(companyPolicy);
            parameters[10] = new NameValue(dtParamFirstPayer.TableName, dtParamFirstPayer);

            DataTable dtPayerComp = CreateDataTablePayerComp(companyPolicy);
            parameters[11] = new NameValue(dtPayerComp.TableName, dtPayerComp);

            DataTable dtDynamicPayerComp = CreateDataTableDynamicPayerComp(companyPolicy);
            parameters[12] = new NameValue(dtDynamicPayerComp.TableName, dtDynamicPayerComp);

            DataTable dtPaymentsComponents = CreateDataTablePaymentsComponents(companyPolicy);
            parameters[13] = new NameValue(dtPaymentsComponents.TableName, dtPaymentsComponents);

            #region FillingWork_Flow 
            parameters[14] = new NameValue("FILING_NUMBER", companyPolicy.Endorsement.TicketNumber.ToString());

            DateTime date_now;

            if (companyPolicy.Endorsement.TicketDate != null)
            {
                if (companyPolicy.Endorsement.TicketDate == DateTime.Today)
                {
                    date_now = DateTime.Now;
                }
                else
                {
                    date_now = Convert.ToDateTime(companyPolicy.Endorsement.TicketDate.ToString());
                }
            }
            else
            {
                date_now = DateTime.Now;
            }

            parameters[15] = new NameValue("FILING_DATE", date_now);
            #endregion

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.SAVE_TEMPORAL_POLICY_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                companyPolicy.Id = Convert.ToInt32(dataTable.Rows[0][0]);
                companyPolicy.Endorsement.TemporalId = Convert.ToInt32(dataTable.Rows[0][1]);
                return companyPolicy;
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordTemporal);
            }
        }

        public CompanyPolicy CreateTemporalCompanyPolicy(CompanyPolicy companyPolicy)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[3];

            DataTable dataTable;

            DataTable dtTableParam = CreateDataTableSubscription(companyPolicy);
            parameters[0] = new NameValue(dtTableParam.TableName, dtTableParam);

            DataTable dtTableParanCo = CreateDataTableCoSubscription(companyPolicy);
            parameters[1] = new NameValue(dtTableParanCo.TableName, dtTableParanCo);

            DataTable dtTableParam2g = CreateDataTableSubscription2G(companyPolicy);
            parameters[2] = new NameValue(dtTableParam2g.TableName, dtTableParam2g);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_POLICY", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                companyPolicy.Id = Convert.ToInt32(dataTable.Rows[0][0]);
                companyPolicy.Endorsement.TemporalId = Convert.ToInt32(dataTable.Rows[0][1]);
                return companyPolicy;
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordTemporal);
            }
        }

        public static DataTable CreateDataTablePaymentsComponents(CompanyPolicy companyPolicy)
        {
            DataTable dtPayerPaymentsComponents = new DataTable("INSERT_CO_TEMP_PAYER_PAYMENT_COMP");

            dtPayerPaymentsComponents.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("PAYER_ID", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("PAYMENT_NUM", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("COMPONENT_CD", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("PAYMENT_COMP_PCT", typeof(decimal));
            dtPayerPaymentsComponents.Columns.Add("COMPONENT_COMP_AMT", typeof(decimal));
            dtPayerPaymentsComponents.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtPayerPaymentsComponents.Columns.Add("DATE_PAYMENT", typeof(DateTime));

            if (companyPolicy.PayerPayments != null)
            {
                foreach (PayerPayment item in companyPolicy.PayerPayments)
                {
                    DataRow dataRow = dtPayerPaymentsComponents.NewRow();
                    dataRow["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    dataRow["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    dataRow["PAYMENT_NUM"] = item.PaymentNumber;
                    dataRow["COMPONENT_CD"] = item.ComponentType;
                    dataRow["PAYMENT_COMP_PCT"] = item.Porcentage;
                    dataRow["COMPONENT_COMP_AMT"] = item.Amount;
                    if (companyPolicy.Endorsement.Id > 0)
                    {
                        dataRow["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
                    }
                    dataRow["DATE_PAYMENT"] = item.PaymentDate;
                    dtPayerPaymentsComponents.Rows.Add(dataRow);
                }
            }

            return dtPayerPaymentsComponents;
        }

        public static DataTable CreateDataTableParamFirstPayer(CompanyPolicy companyPolicy)
        {
            DataTable dtParamFirstPayer = new DataTable("INSERT_TEMP_FIRST_PAY_COMP");

            dtParamFirstPayer.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtParamFirstPayer.Columns.Add("PAYER_ID", typeof(int));
            dtParamFirstPayer.Columns.Add("COMPONENT_CD", typeof(int));
            dtParamFirstPayer.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtParamFirstPayer.Columns.Add("PREFIX_CD", typeof(int));

            if (companyPolicy.ListFirstPayComponent != null && companyPolicy.ListFirstPayComponent.Count > 0)
            {
                foreach (CompanyPayerComponent firstPayComponent in companyPolicy.ListFirstPayComponent)
                {
                    DataRow dataRow = dtParamFirstPayer.NewRow();

                    dataRow["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    dataRow["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    dataRow["COMPONENT_CD"] = firstPayComponent.Component.Id;
                    if (companyPolicy.Endorsement != null && companyPolicy.Endorsement.Id > 0)
                    {
                        dataRow["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
                    }
                    dataRow["PREFIX_CD"] = companyPolicy.Prefix.Id;

                    dtParamFirstPayer.Rows.Add(dataRow);
                }

            }
            return dtParamFirstPayer;
        }

        public static DataTable CreateDataTablePayerComp(CompanyPolicy companyPolicy)
        {
            DataTable dtPayerComponents = new DataTable("INSERT_TEMP_PAYER_COMP");
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            dtPayerComponents.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtPayerComponents.Columns.Add("PAYER_ID", typeof(int));
            dtPayerComponents.Columns.Add("COMPONENT_CD", typeof(int));
            dtPayerComponents.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtPayerComponents.Columns.Add("RATE", typeof(decimal));
            dtPayerComponents.Columns.Add("CALC_BASE_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("COMPONENT_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("ADDIT_RATE_TYPE_CD", typeof(int));
            dtPayerComponents.Columns.Add("ADDIT_RATE", typeof(decimal));
            dtPayerComponents.Columns.Add("ADDIT_CALC_BASE_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("ADDIT_COMPONENT_AMT", typeof(decimal));
            dtPayerComponents.Columns.Add("LINE_BUSINESS_CD", typeof(int));
            dtPayerComponents.Columns.Add("STATE_CD", typeof(int));
            dtPayerComponents.Columns.Add("COUNTRY_CD", typeof(int));
            dtPayerComponents.Columns.Add("ECONOMIC_ACTIVITY_CD", typeof(int));
            dtPayerComponents.Columns.Add("COVERAGE_ID", typeof(int));
            dtPayerComponents.Columns.Add("TAX_CD", typeof(int));
            dtPayerComponents.Columns.Add("TAX_CATEGORY_CD", typeof(int));
            dtPayerComponents.Columns.Add("TAX_CONDITION_CD", typeof(int));
            dtPayerComponents.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtPayerComponents.Columns.Add("PREFIX_CD", typeof(int));
            dtPayerComponents.Columns.Add("EXEMPTION_PCT", typeof(decimal));
            dtPayerComponents.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dtPayerComponents.Columns.Add("COMPONENT_AMOUNT_LOCAL", typeof(decimal));

            if (companyPolicy.PayerComponents != null)
            {
                foreach (CompanyPayerComponent payerComponent in companyPolicy.PayerComponents)
                {
                    DataRow dataRow = dtPayerComponents.NewRow();
                    dataRow["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    dataRow["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    dataRow["COMPONENT_CD"] = payerComponent.Component.Id;
                    object rateTypeCd = payerComponent.Component.Id == 1 ? 1 : (object)payerComponent.RateType;
                    dataRow["RATE_TYPE_CD"] = rateTypeCd;

                    dataRow["RATE"] = Math.Round(payerComponent.Rate, 2);
                    dataRow["CALC_BASE_AMT"] = Math.Round(payerComponent.BaseAmount, 2);
                    dataRow["COMPONENT_AMT"] = Math.Round(payerComponent.Amount, 2);
                    dataRow["LINE_BUSINESS_CD"] = payerComponent.LineBusinessId;
                    if (companyPolicy.Holder.CompanyName.Address != null && companyPolicy.Holder.CompanyName.Address.City != null && companyPolicy.Holder.CompanyName.Address.City.State != null && companyPolicy.Holder.CompanyName.Address.City.State.Id > 0)
                    {
                        dataRow["STATE_CD"] = companyPolicy.Holder.CompanyName.Address.City.State.Id;
                        dataRow["COUNTRY_CD"] = companyPolicy.Holder.CompanyName.Address.City.State.Country.Id;
                    }

                    if (payerComponent.CoverageId > 0)
                    {
                        dataRow["COVERAGE_ID"] = payerComponent.CoverageId;
                    }

                    if (payerComponent.TaxId > 0)
                    {
                        dataRow["TAX_CD"] = payerComponent.TaxId;
                        dataRow["TAX_CONDITION_CD"] = payerComponent.TaxConditionId;
                    }

                    dataRow["PREFIX_CD"] = companyPolicy.Prefix.Id;

                    if (payerComponent.DynamicProperties != null && payerComponent.DynamicProperties.Count > 0)
                    {
                        DynamicPropertiesCollection dynamicCollectionPayer = new DynamicPropertiesCollection();
                        for (int i = 0; i < payerComponent.DynamicProperties.Count(); i++)
                        {
                            DynamicProperty dinamycProperty = new DynamicProperty();
                            dinamycProperty.Id = payerComponent.DynamicProperties[i].Id;
                            dinamycProperty.Value = payerComponent.DynamicProperties[i].Value;
                            dynamicCollectionPayer[i] = dinamycProperty;
                        }

                        byte[] serializedValuesPayer = dynamicPropertiesSerializer.Serialize(dynamicCollectionPayer);
                        dataRow["DYNAMIC_PROPERTIES"] = serializedValuesPayer;
                    }
                    dataRow["COMPONENT_AMOUNT_LOCAL"] = Math.Round(payerComponent.AmountLocal, 2); ;
                    dtPayerComponents.Rows.Add(dataRow);
                }
            }

            return dtPayerComponents;
        }

        public static DataTable CreateDataTableDynamicPayerComp(CompanyPolicy companyPolicy)
        {
            try
            {
                DataTable dtDynamicPayerComponents = new DataTable("INSERT_TEMP_DYNAMIC_PROPERTIES_COMPONENT");

                dtDynamicPayerComponents.Columns.Add("TEMP_ID", typeof(int));
                dtDynamicPayerComponents.Columns.Add("COVERAGE_ID", typeof(int));
                dtDynamicPayerComponents.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
                dtDynamicPayerComponents.Columns.Add("COMPONENT_CD", typeof(int));
                dtDynamicPayerComponents.Columns.Add("PAYER_ID", typeof(int));
                dtDynamicPayerComponents.Columns.Add("CONCEPT_ID", typeof(int));
                dtDynamicPayerComponents.Columns.Add("ENTITY_ID", typeof(int));
                dtDynamicPayerComponents.Columns.Add("CONCEPT_VALUE", typeof(string));
                dtDynamicPayerComponents.Columns.Add("QUESTION_ID", typeof(int));

                if (companyPolicy.PayerComponents != null)
                {
                    foreach (CompanyPayerComponent payerComponent in companyPolicy.PayerComponents)
                    {
                        if (payerComponent.DynamicProperties != null && payerComponent.DynamicProperties.Count > 0)
                        {
                            for (int i = 0; i < payerComponent.DynamicProperties.Count(); i++)
                            {
                                DataRow dataRow = dtDynamicPayerComponents.NewRow();

                                dataRow["TEMP_ID"] = companyPolicy.Endorsement.TemporalId;
                                dataRow["COVERAGE_ID"] = payerComponent.CoverageId;
                                dataRow["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                                dataRow["COMPONENT_CD"] = payerComponent.Component.Id;
                                dataRow["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                                dataRow["CONCEPT_ID"] = payerComponent.DynamicProperties[i].Id;
                                dataRow["ENTITY_ID"] = payerComponent.DynamicProperties[i].EntityId;
                                dataRow["CONCEPT_VALUE"] = payerComponent.DynamicProperties[i].Value;
                                if (payerComponent.DynamicProperties[i].QuestionId.HasValue)
                                {
                                    dataRow["QUESTION_ID"] = payerComponent.DynamicProperties[i].QuestionId;
                                }
                                else
                                {
                                    dataRow["QUESTION_ID"] = DBNull.Value;
                                }
                                dtDynamicPayerComponents.Rows.Add(dataRow);
                            }
                        }
                    }
                }

                DataTable dtcompanyPolicy = RemoveDuplicateRows(dtDynamicPayerComponents, "CONCEPT_ID");
                return dtcompanyPolicy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Removes from datatable duplicate rows
        /// </summary>
        /// <param name="dTable">Datatable</param>
        /// <param name="colName">Column name to remove it</param>
        /// <returns>Datatable</returns>
        private static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                {
                    duplicateList.Add(drow);
                }
                else
                {
                    hTable.Add(drow[colName], string.Empty);
                }
            }

            foreach (DataRow dRow in duplicateList)
            {
                dTable.Rows.Remove(dRow);
            }

            return dTable;
        }

        public static DataTable CreateDataTableClause(CompanyPolicy companyPolicy)
        {
            DataTable dtTempClause = new DataTable("INSERT_TEMP_CLAUSE");

            dtTempClause.Columns.Add("CLAUSE_ID", typeof(int));
            dtTempClause.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtTempClause.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtTempClause.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyPolicy.Clauses != null && companyPolicy.Clauses.Count() > 0)
            {
                foreach (var companyClause in companyPolicy.Clauses.ToList().GroupBy(x => x.Id).ToList())
                {
                    DataRow dataRowTempClause = dtTempClause.NewRow();

                    dataRowTempClause["CLAUSE_ID"] = companyClause.Key;
                    dataRowTempClause["ENDORSEMENT_ID"] = DBNull.Value;
                    dataRowTempClause["CLAUSE_STATUS_CD"] = DBNull.Value;
                    dataRowTempClause["CLAUSE_ORIG_STATUS_CD"] = DBNull.Value;

                    dtTempClause.Rows.Add(dataRowTempClause);
                }
            }
            return dtTempClause;
        }

        public static DataTable CreateDataTableCoinsurance(CompanyPolicy companyPolicy)
        {
            DataTable dtTempCoinsurance = new DataTable("INSERT_TEMP_COINSURANCE");

            dtTempCoinsurance.Columns.Add("INSURANCE_COMPANY_ID", typeof(decimal));
            dtTempCoinsurance.Columns.Add("PART_CIA_PCT", typeof(decimal));
            dtTempCoinsurance.Columns.Add("EXPENSES_PCT", typeof(decimal));
            dtTempCoinsurance.Columns.Add("PART_MAIN_PCT", typeof(decimal));
            dtTempCoinsurance.Columns.Add("ANNEX_NUM_MAIN", typeof(string));
            dtTempCoinsurance.Columns.Add("POLICY_NUM_MAIN", typeof(string));

            foreach (CompanyIssuanceCoInsuranceCompany coInsuranceCompany in companyPolicy.CoInsuranceCompanies)
            {
                DataRow dataRowCoinsurance = dtTempCoinsurance.NewRow();

                if (companyPolicy.BusinessType.GetValueOrDefault() == BusinessType.Accepted)
                {
                    dataRowCoinsurance["INSURANCE_COMPANY_ID"] = coInsuranceCompany.Id;
                    dataRowCoinsurance["PART_CIA_PCT"] = coInsuranceCompany.ParticipationPercentageOwn;
                    dataRowCoinsurance["EXPENSES_PCT"] = coInsuranceCompany.ExpensesPercentage;
                    dataRowCoinsurance["PART_MAIN_PCT"] = coInsuranceCompany.ParticipationPercentage;
                    dataRowCoinsurance["ANNEX_NUM_MAIN"] = coInsuranceCompany.EndorsementNumber ?? string.Empty;
                    dataRowCoinsurance["POLICY_NUM_MAIN"] = coInsuranceCompany.PolicyNumber ?? string.Empty;
                    dtTempCoinsurance.Rows.Add(dataRowCoinsurance);
                }
                else if (companyPolicy.BusinessType.GetValueOrDefault() == BusinessType.Assigned)
                {
                    dataRowCoinsurance["INSURANCE_COMPANY_ID"] = coInsuranceCompany.Id;
                    dataRowCoinsurance["PART_CIA_PCT"] = coInsuranceCompany.ParticipationPercentage;
                    dataRowCoinsurance["EXPENSES_PCT"] = coInsuranceCompany.ExpensesPercentage;
                    dataRowCoinsurance["ANNEX_NUM_MAIN"] = coInsuranceCompany.EndorsementNumber ?? string.Empty;
                    dataRowCoinsurance["POLICY_NUM_MAIN"] = coInsuranceCompany.PolicyNumber ?? string.Empty;
                    dtTempCoinsurance.Rows.Add(dataRowCoinsurance);
                }
            }
            return dtTempCoinsurance;
        }

        public static DataTable CreateDataTableDynamicProperties(CompanyPolicy companyPolicy)
        {
            DataTable dtTempDynamicProperties = new DataTable("INSERT_TEMP_DYNAMIC_PROPERTIES_GENERAL");

            dtTempDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtTempDynamicProperties.Columns.Add("ENTITY_ID", typeof(int));
            dtTempDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));
            dtTempDynamicProperties.Columns.Add("QUESTION_ID", typeof(int));

            if (companyPolicy.DynamicProperties != null && companyPolicy.DynamicProperties.Any())
            {
                foreach (DynamicConcept dynamicConcept in companyPolicy.DynamicProperties)
                {
                    DataRow dataRowCoinsurance = dtTempDynamicProperties.NewRow();

                    if (dynamicConcept.Value != null)
                    {
                        dataRowCoinsurance["DYNAMIC_ID"] = dynamicConcept.Id;
                        dataRowCoinsurance["ENTITY_ID"] = dynamicConcept.EntityId;
                        dataRowCoinsurance["CONCEPT_VALUE"] = dynamicConcept.Value;

                        if (dynamicConcept.QuestionId.HasValue)
                        {
                            dataRowCoinsurance["QUESTION_ID"] = dynamicConcept.QuestionId;
                        }
                        else
                        {
                            dataRowCoinsurance["QUESTION_ID"] = DBNull.Value;
                        }
                        dtTempDynamicProperties.Rows.Add(dataRowCoinsurance);
                    }
                }
            }
            return dtTempDynamicProperties;
        }

        public static DataTable CreateDataTablePayerPayment(CompanyPolicy companyPolicy)
        {
            DataTable dtQuotas = new DataTable("INSERT_TEMP_PAYER_PAYMENT");

            dtQuotas.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtQuotas.Columns.Add("PAYER_ID", typeof(int));
            dtQuotas.Columns.Add("PAYMENT_NUM", typeof(int));
            dtQuotas.Columns.Add("PAY_EXP_DATE", typeof(DateTime));
            dtQuotas.Columns.Add("PAYMENT_PCT", typeof(decimal));
            dtQuotas.Columns.Add("AMOUNT", typeof(decimal));
            dtQuotas.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtQuotas.Columns.Add("PREFIX_CD", typeof(int));
            dtQuotas.Columns.Add("AGT_PAY_EXP_DATE", typeof(DateTime));

            if (companyPolicy.PaymentPlan.Quotas != null)
            {
                foreach (Quota quota in companyPolicy.PaymentPlan.Quotas)
                {
                    DataRow dataRowQuota = dtQuotas.NewRow();

                    dataRowQuota["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    dataRowQuota["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    dataRowQuota["PAYMENT_NUM"] = quota.Number;
                    dataRowQuota["PAY_EXP_DATE"] = quota.ExpirationDate;
                    dataRowQuota["PAYMENT_PCT"] = quota.Percentage;
                    dataRowQuota["AMOUNT"] = quota.Amount;
                    if (companyPolicy.Endorsement.Id > 0)
                    {
                        dataRowQuota["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
                    }
                    dataRowQuota["PREFIX_CD"] = companyPolicy.Prefix.Id;
                    dataRowQuota["AGT_PAY_EXP_DATE"] = DBNull.Value;

                    dtQuotas.Rows.Add(dataRowQuota);
                }
            }
            return dtQuotas;
        }

        public static DataTable CreateDataTableSubscriptionPayer(CompanyPolicy companyPolicy)
        {
            DataTable dtTempSubscriptionPayer = new DataTable("INSERT_TEMP_SUBSCRIPTION_PAYER");

            dtTempSubscriptionPayer.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtTempSubscriptionPayer.Columns.Add("PAYER_ID", typeof(int));
            dtTempSubscriptionPayer.Columns.Add("SURCHARGE_PCT", typeof(decimal));
            dtTempSubscriptionPayer.Columns.Add("PAYMENT_SCHEDULE_ID", typeof(int));
            dtTempSubscriptionPayer.Columns.Add("PAYMENT_METHOD_CD", typeof(int));
            dtTempSubscriptionPayer.Columns.Add("PAYMENT_ID", typeof(int));
            dtTempSubscriptionPayer.Columns.Add("MAIL_ADDRESS_ID", typeof(int));

            DataRow dataRowSubscriptionPayer = dtTempSubscriptionPayer.NewRow();

            dataRowSubscriptionPayer["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
            dataRowSubscriptionPayer["PAYER_ID"] = companyPolicy.Holder.IndividualId;
            dataRowSubscriptionPayer["SURCHARGE_PCT"] = 0;
            dataRowSubscriptionPayer["PAYMENT_SCHEDULE_ID"] = companyPolicy.PaymentPlan.Id;
            if (companyPolicy.Holder.PaymentMethod == null && companyPolicy.Holder.CustomerType == CustomerType.Prospect)
            {
                dataRowSubscriptionPayer["PAYMENT_METHOD_CD"] = 1;
                dataRowSubscriptionPayer["PAYMENT_ID"] = 1;
            }
            else
            {
                dataRowSubscriptionPayer["PAYMENT_METHOD_CD"] = companyPolicy.Holder.PaymentMethod.Id;
                dataRowSubscriptionPayer["PAYMENT_ID"] = companyPolicy.Holder.PaymentMethod.PaymentId;
            }
            if (companyPolicy.Holder.CompanyName.Address != null && companyPolicy.Holder.CompanyName.Address.Id > 0)
            {
                dataRowSubscriptionPayer["MAIL_ADDRESS_ID"] = companyPolicy.Holder.CompanyName.Address.Id;
            }
            dtTempSubscriptionPayer.Rows.Add(dataRowSubscriptionPayer);
            return dtTempSubscriptionPayer;
        }

        public static DataTable CreateDataTableCommissAgent(CompanyPolicy companyPolicy)
        {
            DataTable dtTempCommissAgent = new DataTable("INSERT_TEMP_COMMISS_AGENT");

            dtTempCommissAgent.Columns.Add("COMMISS_AGENT_ID", typeof(int));
            dtTempCommissAgent.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dtTempCommissAgent.Columns.Add("AGENT_AGENCY_ID", typeof(int));
            dtTempCommissAgent.Columns.Add("COMMISS_NUM", typeof(int));
            dtTempCommissAgent.Columns.Add("AGENT_PART_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("ST_COMMISS_PCT", typeof(decimal));            
            dtTempCommissAgent.Columns.Add("ADDIT_COMMISS_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("ST_DISC_COMMISS_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("ADDIT_DISC_COMMISS_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("LINE_BUSINESS_CD", typeof(int));
            dtTempCommissAgent.Columns.Add("SUB_LINE_BUSINESS_CD", typeof(int));
            dtTempCommissAgent.Columns.Add("PREFIX_CD", typeof(int));
            dtTempCommissAgent.Columns.Add("SCH_COMMISS_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("INC_COMMISS_AD_FAC_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("DIS_COMMISS_AD_FAC_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("ORI_ST_COMMISS_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("ORI_SCH_COMMISS_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("ORI_ST_AGENT_COMMISS_PCT", typeof(decimal));
            dtTempCommissAgent.Columns.Add("AGENT_ST_COMMISS_PCT", typeof(decimal));

            int comissionNumber = 0;
            foreach (IssuanceAgency agency in companyPolicy.Agencies)
            {
                foreach (IssuanceCommission comission in agency.Commissions)
                {
                    DataRow dataRowCommissAgency = dtTempCommissAgent.NewRow();
                    dataRowCommissAgency["COMMISS_AGENT_ID"] = comissionNumber++;
                    dataRowCommissAgency["INDIVIDUAL_ID"] = agency.Agent.IndividualId;
                    dataRowCommissAgency["AGENT_AGENCY_ID"] = agency.Id;
                    dataRowCommissAgency["COMMISS_NUM"] = comissionNumber;
                    if (companyPolicy.Endorsement.EndorsementType != EndorsementType.ChangeAgentEndorsement && comission.AgentPercentage > 0)
                    {
                        comission.Percentage = (decimal)comission.AgentPercentage;
                        comission.AgentPercentage = 0;
                    }
                    dataRowCommissAgency["AGENT_PART_PCT"] = agency.Participation;
                    dataRowCommissAgency["ST_COMMISS_PCT"] = comission.Percentage;
                    dataRowCommissAgency["ADDIT_COMMISS_PCT"] = comission.PercentageAdditional;
                    if (comission.SubLineBusiness?.LineBusiness?.Id != null)
                    {
                        dataRowCommissAgency["LINE_BUSINESS_CD"] = comission.SubLineBusiness.LineBusiness.Id;
                    }
                    if (comission.SubLineBusiness != null && comission.SubLineBusiness.Id > 0)
                    {
                        dataRowCommissAgency["SUB_LINE_BUSINESS_CD"] = comission.SubLineBusiness.Id;
                    }
                    
                    dataRowCommissAgency["ST_DISC_COMMISS_PCT"] = DBNull.Value;
                    dataRowCommissAgency["ADDIT_DISC_COMMISS_PCT"] = DBNull.Value;
                    dataRowCommissAgency["ST_COMMISS_PCT"] = comission.Percentage;
                    dataRowCommissAgency["PREFIX_CD"] = companyPolicy.Prefix.Id;
                    dataRowCommissAgency["SCH_COMMISS_PCT"] = 0;
                    dataRowCommissAgency["INC_COMMISS_AD_FAC_PCT"] = DBNull.Value;
                    dataRowCommissAgency["DIS_COMMISS_AD_FAC_PCT"] = DBNull.Value;
                    dataRowCommissAgency["ORI_ST_COMMISS_PCT"] = DBNull.Value;
                    dataRowCommissAgency["ORI_SCH_COMMISS_PCT"] = DBNull.Value;
                    dataRowCommissAgency["ORI_ST_AGENT_COMMISS_PCT"] = agency.Participation;
                    dataRowCommissAgency["AGENT_ST_COMMISS_PCT"] = comission.AgentPercentage==null?0: comission.AgentPercentage;
                    
                    dtTempCommissAgent.Rows.Add(dataRowCommissAgency);
                }
            }
            return dtTempCommissAgent;
        }
        private static DataTable CreateDataTableSubscriptionAgent(CompanyPolicy companyPolicy)
        {
            DataTable dtTempSubscriptionAgent = new DataTable("INSERT_TEMP_SUBSCRIPTION_AGENT");

            dtTempSubscriptionAgent.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dtTempSubscriptionAgent.Columns.Add("AGENT_AGENCY_ID", typeof(int));
            dtTempSubscriptionAgent.Columns.Add("IS_PRIMARY", typeof(bool));
            dtTempSubscriptionAgent.Columns.Add("SALE_POINT_CD", typeof(int));

            foreach (IssuanceAgency agency in companyPolicy?.Agencies)
            {
                DataRow dataRowAgency = dtTempSubscriptionAgent.NewRow();
                dataRowAgency["INDIVIDUAL_ID"] = agency.Agent.IndividualId;
                dataRowAgency["AGENT_AGENCY_ID"] = agency.Id;
                dataRowAgency["IS_PRIMARY"] = agency.IsPrincipal;

                if (agency?.Branch?.SalePoints != null && agency.Branch.SalePoints.Count > 0 && agency.Branch.SalePoints[0].Id > 0)
                {
                    dataRowAgency["SALE_POINT_CD"] = agency.Branch.SalePoints[0].Id;
                }

                dtTempSubscriptionAgent.Rows.Add(dataRowAgency);
            }
            return dtTempSubscriptionAgent;
        }

        #region CompanyPolicyToTable

        public static DataTable CreateDataTableSubscription(CompanyPolicy companyPolicy)
        {
            DataTable dtTempSubscription = new DataTable("INSERT_TEMP_SUBSCRIPTION");
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            #region Colum
            dtTempSubscription.Columns.Add("OPERATION_ID", typeof(int));
            dtTempSubscription.Columns.Add("TEMP_ID", typeof(int));
            dtTempSubscription.Columns.Add("QUOTATION_ID", typeof(int));
            dtTempSubscription.Columns.Add("DOCUMENT_NUM", typeof(decimal));
            dtTempSubscription.Columns.Add("QUOT_VERSION", typeof(int));
            dtTempSubscription.Columns.Add("POLICYHOLDER_ID", typeof(int));
            dtTempSubscription.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtTempSubscription.Columns.Add("PREFIX_CD", typeof(int));
            dtTempSubscription.Columns.Add("BRANCH_CD", typeof(int));
            dtTempSubscription.Columns.Add("ENDO_TYPE_CD", typeof(int));
            dtTempSubscription.Columns.Add("CURRENCY_CD", typeof(int));
            dtTempSubscription.Columns.Add("USER_ID", typeof(int));
            dtTempSubscription.Columns.Add("EXCHANGE_RATE", typeof(decimal));
            dtTempSubscription.Columns.Add("IS_POLICYHOLDER_BILL", typeof(bool));
            dtTempSubscription.Columns.Add("ISSUE_DATE", typeof(DateTime));
            dtTempSubscription.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dtTempSubscription.Columns.Add("CURRENT_TO", typeof(DateTime));
            dtTempSubscription.Columns.Add("BEGIN_DATE", typeof(DateTime));
            dtTempSubscription.Columns.Add("COMMIT_DATE", typeof(DateTime));
            dtTempSubscription.Columns.Add("BILLING_DATE", typeof(DateTime));
            dtTempSubscription.Columns.Add("MAIL_ADDRESS_ID", typeof(int));
            dtTempSubscription.Columns.Add("PRINTED_DATE", typeof(DateTime));
            dtTempSubscription.Columns.Add("SALE_POINT_CD", typeof(int));
            dtTempSubscription.Columns.Add("PRODUCT_ID", typeof(int));
            dtTempSubscription.Columns.Add("BILLING_PERIOD_CD", typeof(int));
            dtTempSubscription.Columns.Add("AGG_ANNUAL_LMT_AMT", typeof(decimal));
            dtTempSubscription.Columns.Add("POLICY_ID", typeof(int));
            dtTempSubscription.Columns.Add("PREV_POLICY_ID", typeof(int));
            dtTempSubscription.Columns.Add("NEXT_POLICY_ID", typeof(int));
            dtTempSubscription.Columns.Add("BILLING_GROUP_CD", typeof(int));
            dtTempSubscription.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtTempSubscription.Columns.Add("TEMP_TYPE_CD", typeof(int));
            dtTempSubscription.Columns.Add("POLICY_NUM", typeof(decimal));
            dtTempSubscription.Columns.Add("CONDITION_TEXT", typeof(string));
            dtTempSubscription.Columns.Add("ENDO_REASON_CD", typeof(int));
            dtTempSubscription.Columns.Add("POL_FOLDER_ID", typeof(int));
            dtTempSubscription.Columns.Add("SUBSCRIPTION_REQ_ID", typeof(int));
            dtTempSubscription.Columns.Add("MAIL_AGENT_ADDRESS_ID", typeof(int));
            dtTempSubscription.Columns.Add("MAIL_AGENT_IND_ID", typeof(int));
            dtTempSubscription.Columns.Add("POLICY_NUM_1G", typeof(int));
            dtTempSubscription.Columns.Add("DFT_PAYMENT_SCHEDULE_ID", typeof(int));
            dtTempSubscription.Columns.Add("DFT_PAYMENT_METHOD_CD", typeof(int));
            dtTempSubscription.Columns.Add("ENDO_GROUP_ID", typeof(int));
            dtTempSubscription.Columns.Add("REF_ENDORSEMENT_ID", typeof(int));
            dtTempSubscription.Columns.Add("TEXT_REASON", typeof(string));
            dtTempSubscription.Columns.Add("CALCULATE_MIN_PREMIUM", typeof(int));
            dtTempSubscription.Columns.Add("ANNOTATIONS", typeof(string));
            dtTempSubscription.Columns.Add("BUSINESS_TYPE_CD", typeof(int));
            dtTempSubscription.Columns.Add("COISSUE_PCT", typeof(decimal));
            dtTempSubscription.Columns.Add("CAPACITY_OF_CD", typeof(int));
            dtTempSubscription.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dtTempSubscription.Columns.Add("NAME_NUM", typeof(int));
            dtTempSubscription.Columns.Add("IS_MASSIVE", typeof(bool));
            #endregion

            DataRow dataRowTempSubscription = dtTempSubscription.NewRow();

            #region row
            dataRowTempSubscription["OPERATION_ID"] = companyPolicy.Id;
            if (companyPolicy.Endorsement.TemporalId > 0)
            {
                dataRowTempSubscription["TEMP_ID"] = companyPolicy.Endorsement.TemporalId;
            }
            if (companyPolicy.Endorsement.QuotationId > 0)
            {
                dataRowTempSubscription["QUOTATION_ID"] = companyPolicy.Endorsement.QuotationId;
            }
            if (companyPolicy.DocumentNumber > 0)
            {
                dataRowTempSubscription["DOCUMENT_NUM"] = companyPolicy.DocumentNumber;
            }
            dataRowTempSubscription["QUOT_VERSION"] = companyPolicy.Endorsement.QuotationVersion;
            dataRowTempSubscription["POLICYHOLDER_ID"] = companyPolicy.Holder.IndividualId;
            dataRowTempSubscription["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
            dataRowTempSubscription["PREFIX_CD"] = companyPolicy.Prefix.Id;
            dataRowTempSubscription["BRANCH_CD"] = companyPolicy.Branch.Id;
            dataRowTempSubscription["ENDO_TYPE_CD"] = companyPolicy.Endorsement.EndorsementType.Value;
            dataRowTempSubscription["CURRENCY_CD"] = companyPolicy.ExchangeRate.Currency.Id;
            dataRowTempSubscription["USER_ID"] = companyPolicy.UserId;
            dataRowTempSubscription["EXCHANGE_RATE"] = companyPolicy.ExchangeRate.SellAmount;
            dataRowTempSubscription["IS_POLICYHOLDER_BILL"] = true;
            dataRowTempSubscription["ISSUE_DATE"] = companyPolicy.IssueDate;

            if (companyPolicy.Endorsement.EndorsementType == EndorsementType.ChangeAgentEndorsement ||
               companyPolicy.Endorsement.EndorsementType == EndorsementType.ChangeCoinsuranceEndorsement ||
               companyPolicy.Endorsement.EndorsementType == EndorsementType.ChangeConsolidationEndorsement ||
               companyPolicy.Endorsement.EndorsementType == EndorsementType.ChangePolicyHolderEndorsement)
            {
                dataRowTempSubscription["CURRENT_FROM"] = companyPolicy.Endorsement.CurrentFrom;
            }
            else {
                dataRowTempSubscription["CURRENT_FROM"] = companyPolicy.CurrentFrom;
            }
                
            dataRowTempSubscription["CURRENT_TO"] = companyPolicy.CurrentTo;
            dataRowTempSubscription["BEGIN_DATE"] = companyPolicy.BeginDate;
            dataRowTempSubscription["COMMIT_DATE"] = DateTime.Now;
            dataRowTempSubscription["BILLING_DATE"] = companyPolicy.CurrentTo;
            if (companyPolicy.Holder.CompanyName.Address != null && companyPolicy.Holder.CompanyName.Address.Id > 0)
            {
                dataRowTempSubscription["MAIL_ADDRESS_ID"] = companyPolicy.Holder.CompanyName.Address.Id;
            }
            dataRowTempSubscription["PRINTED_DATE"] = DateTime.Now;
            if (companyPolicy.Branch.SalePoints != null && companyPolicy.Branch.SalePoints.Count > 0)
            {
                if (companyPolicy.Branch.SalePoints[0].Id > 0)
                {
                    dataRowTempSubscription["SALE_POINT_CD"] = companyPolicy.Branch.SalePoints[0].Id;
                }

            }

            dataRowTempSubscription["PRODUCT_ID"] = companyPolicy.Product.Id;
            dataRowTempSubscription["BILLING_PERIOD_CD"] = DBNull.Value;
            dataRowTempSubscription["AGG_ANNUAL_LMT_AMT"] = 0;
            if (companyPolicy.Endorsement.PolicyId > 0)
            {
                dataRowTempSubscription["POLICY_ID"] = companyPolicy.Endorsement.PolicyId;
            }
            if (companyPolicy.Endorsement.PrevPolicyId.GetValueOrDefault() > 0)
            {
                dataRowTempSubscription["PREV_POLICY_ID"] = companyPolicy.Endorsement.PrevPolicyId.GetValueOrDefault();
            }

            dataRowTempSubscription["NEXT_POLICY_ID"] = DBNull.Value;
            if (companyPolicy.Request != null && companyPolicy.Request.BillingGroupId > 0)
            {
                dataRowTempSubscription["BILLING_GROUP_CD"] = companyPolicy.Request.BillingGroupId;
            }
            if (companyPolicy.Endorsement.Id > 0)
            {
                dataRowTempSubscription["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
            }
            dataRowTempSubscription["TEMP_TYPE_CD"] = companyPolicy.TemporalType;
            dataRowTempSubscription["POLICY_NUM"] = DBNull.Value;
            if (companyPolicy.Endorsement.Text != null && !string.IsNullOrEmpty(companyPolicy.Endorsement.Text.TextBody))
            {
                dataRowTempSubscription["CONDITION_TEXT"] = companyPolicy.Endorsement.Text.TextBody;
            }
            else
                if (companyPolicy.Text != null && !string.IsNullOrEmpty(companyPolicy.Text.TextBody))
            {
                dataRowTempSubscription["CONDITION_TEXT"] = companyPolicy.Text.TextBody;
            }
            else
            {
                dataRowTempSubscription["CONDITION_TEXT"] = string.Empty;
            }
            if (companyPolicy.Endorsement.EndorsementReasonId > 0)
            {
                dataRowTempSubscription["ENDO_REASON_CD"] = companyPolicy.Endorsement.EndorsementReasonId;
            }
            dataRowTempSubscription["POL_FOLDER_ID"] = DBNull.Value;
            dataRowTempSubscription["SUBSCRIPTION_REQ_ID"] = DBNull.Value;
            dataRowTempSubscription["MAIL_AGENT_ADDRESS_ID"] = DBNull.Value;
            dataRowTempSubscription["MAIL_AGENT_IND_ID"] = DBNull.Value;
            dataRowTempSubscription["POLICY_NUM_1G"] = DBNull.Value;
            dataRowTempSubscription["DFT_PAYMENT_SCHEDULE_ID"] = DBNull.Value;
            dataRowTempSubscription["DFT_PAYMENT_METHOD_CD"] = DBNull.Value;
            dataRowTempSubscription["ENDO_GROUP_ID"] = (int)companyPolicy.Endorsement.EndorsementType.GetValueOrDefault() == 6 ? 1 : dataRowTempSubscription["ENDO_GROUP_ID"];
            if (companyPolicy.Endorsement.ReferenceEndorsementId > 0)
            {
                dataRowTempSubscription["REF_ENDORSEMENT_ID"] = companyPolicy.Endorsement.ReferenceEndorsementId;
            }
            dataRowTempSubscription["TEXT_REASON"] = DBNull.Value;
            if (companyPolicy.CalculateMinPremium == null)
            {
                dataRowTempSubscription["CALCULATE_MIN_PREMIUM"] = 0;
            }
            else
            {
                dataRowTempSubscription["CALCULATE_MIN_PREMIUM"] = companyPolicy.CalculateMinPremium;
            }

            if(companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission ||(companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal && !companyPolicy.Endorsement.IsUnderIdenticalConditions))
            {
                if (companyPolicy.Text != null && !string.IsNullOrEmpty(companyPolicy.Text.Observations))
                {
                    dataRowTempSubscription["ANNOTATIONS"] = companyPolicy.Text.Observations;
                }
            }
            else
            { 
                if (companyPolicy.Endorsement.Text != null && !string.IsNullOrEmpty(companyPolicy.Endorsement.Text.Observations))
                {
                    dataRowTempSubscription["ANNOTATIONS"] = companyPolicy.Endorsement.Text.Observations;
                }
            }
            dataRowTempSubscription["BUSINESS_TYPE_CD"] = companyPolicy.BusinessType;
            if (companyPolicy.CoInsuranceCompanies != null && companyPolicy.CoInsuranceCompanies.Count > 0)
            {
                dataRowTempSubscription["COISSUE_PCT"] = companyPolicy.CoInsuranceCompanies[0].ParticipationPercentageOwn;
            }
            else
            {
                dataRowTempSubscription["COISSUE_PCT"] = 0;
            }
            dataRowTempSubscription["CAPACITY_OF_CD"] = DBNull.Value;
            #endregion

            if (companyPolicy.DynamicProperties != null && companyPolicy.DynamicProperties.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionPolicy = new DynamicPropertiesCollection();

                for (int i = 0; i < companyPolicy.DynamicProperties.Count; i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = companyPolicy.DynamicProperties[i].Id;
                    dinamycProperty.Value = companyPolicy.DynamicProperties[i].Value;
                    dynamicCollectionPolicy[i] = dinamycProperty;
                }

                dataRowTempSubscription["DYNAMIC_PROPERTIES"] = dynamicPropertiesSerializer.Serialize(dynamicCollectionPolicy);
            }
            if (companyPolicy.Holder.CompanyName.NameNum > 0)
            {
                dataRowTempSubscription["NAME_NUM"] = companyPolicy.Holder.CompanyName.NameNum;
            }
            dataRowTempSubscription["IS_MASSIVE"] = false;

            dtTempSubscription.Rows.Add(dataRowTempSubscription);
            return dtTempSubscription;
        }

        private static DataTable CreateDataTableCoSubscription(CompanyPolicy companyPolicy)
        {
            DataTable dtcoTempSubscription = new DataTable("INSERT_CO_TEMP_SUBSCRIPTION");

            dtcoTempSubscription.Columns.Add("POLICY_TYPE_CD", typeof(int));
            dtcoTempSubscription.Columns.Add("CORRELATIVE_POLICY", typeof(decimal));
            dtcoTempSubscription.Columns.Add("REQUEST_ID", typeof(int));
            dtcoTempSubscription.Columns.Add("EFFECT_PERIOD", typeof(int));
            dtcoTempSubscription.Columns.Add("REQUEST_ENDORSEMENT_ID", typeof(int));
            dtcoTempSubscription.Columns.Add("IS_REQUEST", typeof(bool));
            dtcoTempSubscription.Columns.Add("BUSINESS_ID", typeof(int));
            dtcoTempSubscription.Columns.Add("GROUP_QUOTATION_ID", typeof(int));
            dtcoTempSubscription.Columns.Add("ACTIVE_QUOTATION", typeof(bool));
            dtcoTempSubscription.Columns.Add("ENDO_TYPE_CD", typeof(int));
            DataRow dataRowcoTempSubscription = dtcoTempSubscription.NewRow();

            dataRowcoTempSubscription["POLICY_TYPE_CD"] = companyPolicy.PolicyType.Id;
            if (companyPolicy.CorrelativePolicyNumber.GetValueOrDefault() > 0)
            {
                dataRowcoTempSubscription["CORRELATIVE_POLICY"] = companyPolicy.CorrelativePolicyNumber;
            }
            if (companyPolicy.Request != null && companyPolicy.Request.Id > 0)
            {
                dataRowcoTempSubscription["REQUEST_ID"] = companyPolicy.Request.Id;
                dataRowcoTempSubscription["IS_REQUEST"] = true;
                dataRowcoTempSubscription["REQUEST_ENDORSEMENT_ID"] = companyPolicy.Request.Id;
            }
            else
            {
                dataRowcoTempSubscription["REQUEST_ID"] = DBNull.Value;
                dataRowcoTempSubscription["IS_REQUEST"] = false;
                dataRowcoTempSubscription["REQUEST_ENDORSEMENT_ID"] = DBNull.Value;
            }
            dataRowcoTempSubscription["EFFECT_PERIOD"] = companyPolicy.EffectPeriod;
            if (companyPolicy.BusinessId > 0)
            {
                dataRowcoTempSubscription["BUSINESS_ID"] = companyPolicy.BusinessId;
            }
            if (companyPolicy.GroupQuoteId > 0)
            {
                dataRowcoTempSubscription["GROUP_QUOTATION_ID"] = companyPolicy.GroupQuoteId;
            }
            dataRowcoTempSubscription["ACTIVE_QUOTATION"] = false;
            if (companyPolicy.Endorsement.ModificationTypeId > 0)
            {
                dataRowcoTempSubscription["ENDO_TYPE_CD"] = companyPolicy.Endorsement.ModificationTypeId;
            }
            else
            {
                dataRowcoTempSubscription["ENDO_TYPE_CD"] = DBNull.Value;
            }
            dtcoTempSubscription.Rows.Add(dataRowcoTempSubscription);
            return dtcoTempSubscription;
        }

        private static DataTable CreateDataTableSubscription2G(CompanyPolicy companyPolicy)
        {
            DataTable dtcoTempSubscription2G = new DataTable("INSERT_CO_TEMP_SUBSCRIPTION_2G");

            dtcoTempSubscription2G.Columns.Add("SINISTER_QTY", typeof(int));
            dtcoTempSubscription2G.Columns.Add("RENEWAL_QTY", typeof(int));
            dtcoTempSubscription2G.Columns.Add("HAS_TOTAL_LOSS", typeof(int));
            dtcoTempSubscription2G.Columns.Add("PORTFOLIO_BALANCE", typeof(decimal));
            dtcoTempSubscription2G.Columns.Add("STRO_LAST_THREE_YEARS", typeof(int));

            DataRow dataRowcoTempSubscription2G = dtcoTempSubscription2G.NewRow();

            dataRowcoTempSubscription2G["SINISTER_QTY"] = companyPolicy.SinisterQuantity;
            dataRowcoTempSubscription2G["RENEWAL_QTY"] = companyPolicy.RenewalsQuantity;
            dataRowcoTempSubscription2G["HAS_TOTAL_LOSS"] = companyPolicy.HasTotalLoss;
            dataRowcoTempSubscription2G["PORTFOLIO_BALANCE"] = companyPolicy.PortfolioBalance;
            dataRowcoTempSubscription2G["STRO_LAST_THREE_YEARS"] = companyPolicy.SinisterQuantityLastYears;

            dtcoTempSubscription2G.Rows.Add(dataRowcoTempSubscription2G);
            return dtcoTempSubscription2G;
        }


        #endregion
        public List<CompanyPolicyAgent> GetAgenciesByDesciption(string agentId = "", string description = "", string productId = "", string userId = "")
        {
            Int32 userCode = 0;
            Int32.TryParse(description, out userCode);

            List<UserAgency> agenciesUser = DelegateService.uniqueUserService.GetAgenciesByUserId(userCode);

            List<CompanyPolicyAgent> Agents = new List<CompanyPolicyAgent>();

            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("@AGENTID", agentId);
            parameters[1] = new NameValue("@DESCRIPTION", description);
            parameters[2] = new NameValue("@PRODUCTID", productId);
            parameters[3] = new NameValue("@USERID", userId);

            Int32 agentCode = 0;
            Int32.TryParse(description, out agentCode);

            if (agentCode > 0)
            {
                parameters[0] = new NameValue("@AGENTID", Convert.ToString(agentCode));
                parameters[1] = new NameValue("@DESCRIPTION", "");
                parameters[3] = new NameValue("@USERID", "");
            }
            else
            {
                parameters[0] = new NameValue("@AGENTID", "");
                parameters[1] = new NameValue("@DESCRIPTION", description);
                parameters[3] = new NameValue("@USERID", "");
            }

            if (agenciesUser.Count > 0)
            {
                parameters[3] = new NameValue("@USERID", Convert.ToString(userCode));
            }

            parameters[2] = new NameValue("@PRODUCTID", productId);

            DataTable dataTable = new DataTable();
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("UP.SP_ACCEPTED_AGENTS", parameters);
                if (agentCode > 0 && dataTable.Rows.Count == 0)
                {
                    parameters[0] = new NameValue("@AGENTID", "");
                    parameters[1] = new NameValue("@DESCRIPTION", description);
                    dataTable = pdb.ExecuteSPDataTable("UP.SP_ACCEPTED_AGENTS", parameters);
                }
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    CompanyPolicyAgent agent = new CompanyPolicyAgent();

                    agent.IndividualId = Convert.ToInt32(row[0]);
                    agent.Id = Convert.ToInt32(row[17]);
                    agent.FullName = Convert.ToString(row[18]);
                    agent.AgentId = Convert.ToInt32(row[21]);
                    Agents.Add(agent);
                }
            }
            else
            {
                throw new System.ArgumentException("No se encuentran intermediarios.");
            }

            return Agents;
        }

        /// <summary>
        /// persistencia de todos las emisiones y Endosos en la tabla CONDITION_TEXT_WITH_CLAUSE
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <param name="EndorsementId"></param>
        public void SaveTextLarge(int PolicyId, int EndorsementId)
        {
            StringBuilder TextLarge = new StringBuilder();
            StringBuilder TextRiskLarge = new StringBuilder();
            string TextFormat = String.Empty;
            string TextTD = "NIT O CC No.:";
            NameValue[] param;

            string PolicyHolderName = String.Empty;
            string PolicyHolderIdCardNo = String.Empty;
            string insuredname = String.Empty;
            string insuredIdCardno = String.Empty;

            #region Endorsement
            ObjectCriteriaBuilder filterII = new ObjectCriteriaBuilder();

            filterII.PropertyEquals(ISSEN.Endorsement.Properties.EndorsementId, EndorsementId);

            BusinessCollection listEndorsementActionRequest;
            ISSEN.Endorsement entityEndorsement = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                listEndorsementActionRequest = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.Endorsement), filterII.GetPredicate()));

            }
            if (listEndorsementActionRequest != null && listEndorsementActionRequest.Count > 0)
            {
                entityEndorsement = listEndorsementActionRequest.Cast<ISSEN.Endorsement>().First();
            }
            #endregion

            #region PolicyClause
            ObjectCriteriaBuilder filterIII = new ObjectCriteriaBuilder();
            filterIII.PropertyEquals(ISSEN.PolicyClause.Properties.PolicyId, PolicyId);
            filterIII.And();
            filterIII.PropertyEquals(ISSEN.PolicyClause.Properties.EndorsementId, EndorsementId);

            BusinessCollection listPolicyClauseActionRequest;
            ISSEN.PolicyClause entityClause = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                listPolicyClauseActionRequest = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.PolicyClause), filterIII.GetPredicate()));

            }
            if (listPolicyClauseActionRequest != null && listPolicyClauseActionRequest.Count > 0)
            {
                entityClause = listPolicyClauseActionRequest.Cast<ISSEN.PolicyClause>().First();
            }

            #endregion


            string SQLstatement = String.Empty;


            if ((entityEndorsement.ConditionText != null && entityEndorsement.ConditionText.ToString().Trim().Length > 0) ||
                 (entityClause != null && listPolicyClauseActionRequest != null && listPolicyClauseActionRequest.Count > 0)
                )
            {
                TextLarge.Append("----------------------------------------------------------------------------------------------------");
                TextLarge.Append("\n\r");
                TextLarge.Append(entityEndorsement?.ConditionText?.ToString());
                TextLarge.Append("\n\r");



                if (listPolicyClauseActionRequest.Count > 0)
                {

                    #region Clause
                    ObjectCriteriaBuilder filterIV = new ObjectCriteriaBuilder();
                    filterIV.Property(quotationEntitiesCore.Clause.Properties.ClauseId, typeof(quotationEntitiesCore.Clause).Name);
                    filterIV.In();
                    filterIV.ListValue();

                    foreach (ISSEN.PolicyClause item in listPolicyClauseActionRequest)
                    {
                        filterIV.Constant(item.ClauseId);
                    }

                    filterIV.EndList();


                    BusinessCollection listClauseActionRequest;
                    quotationEntitiesCore.Clause entityClauseEnd = null;
                    using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        listClauseActionRequest = new BusinessCollection(daf.SelectObjects(typeof(quotationEntitiesCore.Clause), filterIV.GetPredicate()));

                    }
                    if (listClauseActionRequest != null && listClauseActionRequest.Count > 0)
                    {
                        entityClauseEnd = listClauseActionRequest.Cast<quotationEntitiesCore.Clause>().First();
                    }

                    #endregion

                    TextLarge.Append("----------------------------------------------------------------------------------------------------");
                    TextLarge.Append("\n\r");
                    TextLarge.Append("----------------------------------------------------------------------------------------------------");
                    TextLarge.Append("\n\r");

                    if (listClauseActionRequest != null && listClauseActionRequest.Count > 0)
                    {
                        foreach (quotationEntitiesCore.Clause clause in listClauseActionRequest)
                        {
                            TextLarge.Append(clause.ClauseName + " " + clause.ClauseText);
                            TextLarge.Append("\n\r");
                        }
                    }
                }


                param = new NameValue[5];
                param[0] = new NameValue("@policy_id", PolicyId);
                param[1] = new NameValue("@endorsement_id", EndorsementId);
                param[2] = new NameValue("@risk_id", 0);
                param[3] = new NameValue("@risk_num", 0);
                param[4] = new NameValue("@condition_text", TextLarge.ToString().Replace("'", "''"));
                this.SaveTextLargeDAO(param);
                param = null;
            }


            //Parte II ---------------------------------------------------------

            int RiskId = 0;
            int RiskNumber = 0;
            string FormNumber = string.Empty;
            int PrefixCd = 0;

            #region SP_ENDORSEMENT_RISK_CLAUSE
            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("POLICY_ID", PolicyId);
            parameters[1] = new NameValue("ENDORSEMENT_ID", EndorsementId);

            DataTable dataTable;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("ISS.LIST_ENDORSEMENT_RISK_CLAUSE", parameters);
            }
            #endregion

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow itemRow in dataTable.Rows)
                {

                    RiskId = Convert.ToInt32(itemRow[2]);
                    RiskNumber = Convert.ToInt32(itemRow[3]);
                    PolicyHolderIdCardNo = Convert.ToString(itemRow[5]);
                    PolicyHolderName = Convert.ToString(itemRow[6]);
                    insuredIdCardno = Convert.ToString(itemRow[7]);
                    insuredname = Convert.ToString(itemRow[8]);
                    FormNumber = Convert.ToString(itemRow[9]);
                    PrefixCd = Convert.ToInt32(itemRow[10]);


                    TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");
                    TextRiskLarge.Append("\n\r");
                    TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");
                    TextRiskLarge.Append("\n\r");
                    TextRiskLarge.Append(FormNumber);
                    TextRiskLarge.Append("\n\r");

                    #region Risk
                    ObjectCriteriaBuilder filterV = new ObjectCriteriaBuilder();
                    filterV.Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Endorsement).Name);
                    filterV.Equal();
                    filterV.Constant(RiskId);

                    BusinessCollection findIssueRiskActionResponse;
                    ISSEN.Risk entityRisk = null;
                    using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        findIssueRiskActionResponse = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.Risk), filterV.GetPredicate()));

                    }
                    if (findIssueRiskActionResponse != null && findIssueRiskActionResponse.Count > 0)
                    {
                        entityRisk = findIssueRiskActionResponse.Cast<ISSEN.Risk>().First();
                    }
                    #endregion

                    if (entityRisk != null)
                    {
                        TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");
                        TextRiskLarge.Append("\n\r");
                        TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");
                        TextRiskLarge.Append("\n\r");
                        TextRiskLarge.Append(entityRisk.ConditionText != null ? entityRisk.ConditionText.ToString() : String.Empty);
                        TextRiskLarge.Append("\n\r");

                    }

                    if (PrefixCd == (int)Enums.PrefixType.Automoviles)
                    {
                        #region SP_WARRANTY_RISK_CLAUSE
                        NameValue[] parameter = new NameValue[1];

                        parameter[0] = new NameValue("RISK_ID", RiskId);

                        DataTable reqArraylistWarranty;
                        using (DynamicDataAccess pdb = new DynamicDataAccess())
                        {
                            reqArraylistWarranty = pdb.ExecuteSPDataTable("ISS.LIST_WARRANTY_RISK_CLAUSE", parameter);
                        }
                        #endregion

                        if (reqArraylistWarranty != null && reqArraylistWarranty.Rows.Count > 0)
                        {
                            string WarrantyDescription = string.Empty;
                            TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");
                            TextRiskLarge.Append("\n\r");
                            TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");

                            foreach (DataRow itemRowWarranty in reqArraylistWarranty.Rows)
                            {
                                WarrantyDescription = Convert.ToString(itemRowWarranty[0]);
                                TextRiskLarge.Append("\n\r");
                                TextRiskLarge.Append(WarrantyDescription);
                            }
                        }

                    }

                    #region SP_RISK_CLAUSE
                    NameValue[] paramete = new NameValue[1];

                    paramete[0] = new NameValue("RISK_ID", RiskId);

                    DataTable resArraylistClause;
                    using (DynamicDataAccess pdb = new DynamicDataAccess())
                    {
                        resArraylistClause = pdb.ExecuteSPDataTable("ISS.LIST_RISK_CLAUSE", paramete);
                    }
                    #endregion


                    if (resArraylistClause != null && resArraylistClause.Rows.Count > 0)
                    {
                        string ClauseName = string.Empty;
                        string ClauseText = string.Empty;
                        TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");
                        TextRiskLarge.Append("\n\r");
                        TextRiskLarge.Append("----------------------------------------------------------------------------------------------------");

                        foreach (DataRow itemRowClause in resArraylistClause.Rows)
                        {
                            ClauseName = Convert.ToString(itemRowClause[1]);
                            ClauseText = Convert.ToString(itemRowClause[0]);
                            TextRiskLarge.Append("\n\r");
                            TextFormat = ClauseName + " " + ClauseText;
                            TextFormat = TextFormat.Replace("_", " ");
                            int IndO = TextFormat.IndexOf(TextTD);
                            if (IndO > 0)
                            {
                                TextFormat.Insert(IndO, TextTD);
                            }

                            TextFormat = TextFormat.Replace("TOMADOR:", "TOMADOR: " + PolicyHolderName);
                            TextFormat = TextFormat.Replace("DOC_TOMADOR-NIT O CC No.:", "DOC_TOMADOR-:  " + PolicyHolderIdCardNo);
                            TextFormat = TextFormat.Replace("ASEGURADO::", "ASEGURADO: " + insuredname);
                            TextFormat = TextFormat.Replace("NIT O CC No.:", "NIT O CC No.: " + insuredIdCardno);
                            TextFormat = TextFormat.Replace("DOC_TOMADOR-", "NIT O CC No.");
                            TextFormat = TextFormat.Replace("A FAVOR DE:", "A FAVOR DE: " + PolicyHolderName);
                            TextFormat = TextFormat.Replace("CON NIT.", "CON NIT. " + PolicyHolderIdCardNo);
                            TextRiskLarge.Append(TextFormat);
                        }
                    }

                    param = new NameValue[5];
                    param[0] = new NameValue("@policy_id", PolicyId);
                    param[1] = new NameValue("@endorsement_id", EndorsementId);
                    param[2] = new NameValue("@risk_id", RiskId);
                    param[3] = new NameValue("@risk_num", RiskNumber);
                    param[4] = new NameValue("@condition_text", TextRiskLarge.ToString().Replace("'", "''"));
                    this.SaveTextLargeDAO(param);
                    param = null;


                    TextRiskLarge = new StringBuilder();
                }
            }
        }

        /// <summary>
        /// Persistir texto y clausulas
        /// </summary>
        /// <param name="parameters"></param>
        private void SaveTextLargeDAO(NameValue[] parameters)
        {
            DataTable dataTable;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("ISS.SAVE_CONDITION_TEXT_WITH_CLAUSE", parameters);
            }
        }
        #endregion

        public int GetProfileUser(int UserId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UNIQUEUSER.ProfileUniqueUser.Properties.UserId, typeof(UNIQUEUSER.ProfileUniqueUser).Name);
            filter.Equal();
            filter.Constant(UserId);
            var result = DataFacadeManager.Instance.GetDataFacade().List<UNIQUEUSER.ProfileUniqueUser>(filter.GetPredicate());
            DataFacadeManager.Dispose();
            if (result.Count > 0)
            {
                return result[0].ProfileId;
            }
            return 0;
        }

        public int GetGroupBranchId(int branchId)
        {
            int groupBrancCode = 0;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CORECOMM.CoBranch.Properties.BranchCode, typeof(CORECOMM.CoBranch).Name);
            filter.Equal();
            filter.Constant(branchId);
            var result = DataFacadeManager.Instance.GetDataFacade().List<CORECOMM.CoBranch>(filter.GetPredicate());
            DataFacadeManager.Dispose();
            if (result.Count > 0)
            {
                if (result[0].GroupBranchCode != null)
                {
                    groupBrancCode = (int)result[0].GroupBranchCode;
                }
            }
            return groupBrancCode;
        }

        public CompanyPolicy MoodificationEndorsementPremium(CompanyPolicy oCompanyPolicy)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<ISSEN.PayerComp> entityPayerComponents = null;

            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(oCompanyPolicy.Endorsement.Id);
            filter.EndList();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                entityPayerComponents = daf.List(typeof(ISSEN.PayerComp), filter.GetPredicate()).Cast<ISSEN.PayerComp>().ToList();
            }

            if (entityPayerComponents != null && entityPayerComponents.Count > 0)
            {
                oCompanyPolicy.Summary.Premium = entityPayerComponents.
                    Where(x => x.EndorsementId == oCompanyPolicy.Endorsement.Id && (ComponentType)x.ComponentCode == ComponentType.Premium).Sum(y => y.ComponentAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCalculatePayerComponents);
            }
            return oCompanyPolicy;
        }

        /// <summary>
        /// Gets the current status policy by endorsement identifier is current.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public CompanyPolicy GetCurrentStatusPolicyByEndorsementIdIsCurrentCompany(int endorsementId, bool isCurrent, bool fromPrinting = false)
        {
            ConcurrentBag<string> errors = new ConcurrentBag<string>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            ISSEN.Endorsement endorsementEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementEntity = daf.List(typeof(ISSEN.Endorsement), filter.GetPredicate()).Cast<ISSEN.Endorsement>().FirstOrDefault();
            }
            if (endorsementEntity != null)
            {
                var taskEntity = new List<Task>();
                ISSEN.Policy entityPolicy = null;
                PrimaryKey primaryKey = ISSEN.Policy.CreatePrimaryKey(endorsementEntity.PolicyId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityPolicy = (ISSEN.Policy)daf.GetObjectByPrimaryKey(primaryKey);
                }

                primaryKey = CORECOMM.Currency.CreatePrimaryKey(entityPolicy.CurrencyCode);
                CORECOMM.Currency entityCurrency = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityCurrency = (CORECOMM.Currency)daf.GetObjectByPrimaryKey(primaryKey);
                }
                if (entityPolicy != null)
                {
                    Policy policy = new Policy
                    {
                        Id = 0,
                        CurrentFrom = entityPolicy.CurrentFrom,
                        CurrentTo = entityPolicy.CurrentTo ?? DateTime.MinValue,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Prefix = new COMMMO.Prefix()
                        {
                            Id = entityPolicy.PrefixCode,
                            Description = DelegateService.commonService.GetPrefixById(entityPolicy.PrefixCode).Description
                        },
                        Branch = new COMMMO.Branch()
                        {
                            Id = entityPolicy.BranchCode,
                            Description = DelegateService.commonService.GetBranchById(entityPolicy.BranchCode).Description
                        },
                        IssueDate = endorsementEntity.IssueDate,
                        BusinessType = (BusinessType)entityPolicy.BusinessTypeCode,
                        ExchangeRate = new COMMMO.ExchangeRate
                        {
                            Currency = new COMMMO.Currency
                            {
                                Id = entityPolicy.CurrencyCode,
                                Description = entityCurrency.Description
                            }
                        },
                        Endorsement = new Endorsement
                        {
                            Id = endorsementEntity.EndorsementId,
                            PolicyId = endorsementEntity.PolicyId,
                            CurrentFrom = endorsementEntity.CurrentFrom,
                            CurrentTo = endorsementEntity.CurrentTo.Value,
                            EndorsementType = (EndorsementType)endorsementEntity.EndoTypeCode,
                            Number = endorsementEntity.DocumentNum,
                            Text = new Text
                            {
                                TextBody = endorsementEntity.ConditionText,
                                Observations = endorsementEntity.Annotations
                            }
                        },
                        Summary = new Summary(),
                        UserId = endorsementEntity.UserId

                    };


                    ISSEN.CoPolicy entityCoPolicy = null;

                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            PrimaryKey pkCoPolicy = ISSEN.CoPolicy.CreatePrimaryKey(entityPolicy.PolicyId);
                            entityCoPolicy = (ISSEN.CoPolicy)daf.GetObjectByPrimaryKey(pkCoPolicy);
                        }
                        DataFacadeManager.Dispose();
                    }).ContinueWith((result) =>
                    {
                        CORECOMM.CoPolicyType entityCoPolicyType = null;
                        taskEntity.Add(TP.Task.Run(() =>
                        {
                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                PrimaryKey pkEntity = CORECOMM.CoPolicyType.CreatePrimaryKey(entityPolicy.PrefixCode, entityCoPolicy.PolicyTypeCode);
                                entityCoPolicyType = (CORECOMM.CoPolicyType)daf.GetObjectByPrimaryKey(pkEntity);
                                policy.PolicyType = new COMMMO.PolicyType() { Description = entityCoPolicyType.Description, IsFloating = entityCoPolicyType.Floating };
                            }
                            DataFacadeManager.Dispose();
                        }));
                    }));

                    HolderDAO holderDAO = new HolderDAO();

                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        policy.Holder = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(entityPolicy.PolicyholderId.Value.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                        DataFacadeManager.Dispose();
                    }));
                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        policy.Product = DelegateService.productService.GetProductById(entityPolicy.ProductId.Value);
                        DataFacadeManager.Dispose();
                    }));
                    try
                    {
                        Task.WaitAll(taskEntity.ToArray());
                    }
                    catch (AggregateException ae)
                    {

                        errors.Add(ae.GetBaseException().Message);
                    }
                    EndorsementDAO endorsementDAO = new EndorsementDAO();
                    List<Endorsement> endorsements = new List<Endorsement>();
                    if (!isCurrent)
                    {
                        endorsements = endorsementDAO.GetEffectiveEndorsementsByPolicyId(entityPolicy.PolicyId)?? new List<Endorsement>();

                        if ((fromPrinting == false && endorsements != null && endorsements.Any()) || fromPrinting == true)
                        {
                            if (endorsements.Exists(x => x.EndorsementType == EndorsementType.Renewal))
                            {
                                policy.CurrentFrom = endorsements.Last(x => x.EndorsementType == EndorsementType.Renewal).CurrentFrom;
                            }
                            else if (endorsements.Exists(x => x.EndorsementType == EndorsementType.ChangeTermEndorsement))
                            {
                                policy.CurrentFrom = endorsements.Last(x => x.EndorsementType == EndorsementType.ChangeTermEndorsement).CurrentFrom;
                                endorsements = endorsements.Where(x => x.Id >= endorsements.Last(y => y.EndorsementType == EndorsementType.ChangeTermEndorsement).Id).ToList();
                            }
                            else if (endorsements.Exists(x => x.EndorsementType == EndorsementType.ChangeAgentEndorsement))
                            {
                                policy.CurrentFrom = endorsements.Last(x => x.EndorsementType == EndorsementType.ChangeAgentEndorsement).CurrentFrom;
                                endorsements = endorsements.Where(x => x.Id >= endorsements.Last(y => y.EndorsementType == EndorsementType.ChangeAgentEndorsement).Id).ToList();
                            }
                            else
                            {
                                if (endorsements.FirstOrDefault(x => x.EndorsementType == EndorsementType.Emission) != null)
                                {
                                    policy.CurrentFrom = endorsements.First(x => x.EndorsementType == EndorsementType.Emission).CurrentFrom;

                                }
                                else
                                {
                                    policy.CurrentFrom = endorsements.FirstOrDefault()?.CurrentFrom ?? policy.CurrentFrom;
                                }
                            }

                            if (endorsements.Exists(x => x.IsCurrent))
                            {
                                policy.CurrentTo = endorsements.First(x => x.IsCurrent).CurrentTo;
                            }


                        }
                        else
                        {
                            throw new Exception(Errors.ErrorEndorsementNotFound);
                        }
                    }
                    else
                    {
                        endorsements.Add(new Endorsement
                        {
                            Id = endorsementEntity.EndorsementId,
                            EndorsementType = (EndorsementType)endorsementEntity.EndoTypeCode,
                            CurrentFrom = endorsementEntity.CurrentFrom,
                            CurrentTo = endorsementEntity.CurrentTo.Value,
                            IsCurrent = true,
                            PolicyId = endorsementEntity.PolicyId
                        });

                    }

                    if (endorsements.Exists(x => x.IsCurrent))
                    {
                        if (!isCurrent && endorsements.First(x => x.IsCurrent).EndorsementType != EndorsementType.Cancellation &&
                            endorsements.First(x => x.IsCurrent).EndorsementType != EndorsementType.Nominative_cancellation)
                        {
                            filter = new ObjectCriteriaBuilder();
                            filter.Property(quotationEntitiesCore.Component.Properties.ComponentTypeCode, typeof(quotationEntitiesCore.Component).Name);
                            filter.In();
                            filter.ListValue();
                            filter.Constant(ComponentType.Premium);
                            filter.Constant(ComponentType.Expenses);
                            filter.Constant(ComponentType.Taxes);
                            filter.EndList();
                            List<quotationEntitiesCore.Component> components = null;
                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                components = daf.List(typeof(quotationEntitiesCore.Component), filter.GetPredicate()).Cast<quotationEntitiesCore.Component>().ToList();
                            }
                            if (components != null && components.Any())
                            {
                                var premium = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Premium).ComponentCode;
                                var Expenses = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Expenses).ComponentCode;
                                var taxes = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Taxes).ComponentCode;
                                filter = new ObjectCriteriaBuilder();
                                filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
                                filter.In();
                                filter.ListValue();
                                foreach (int id in endorsements.Select(x => x.Id))
                                {
                                    filter.Constant(id);
                                }
                                filter.EndList();
                                List<ISSEN.PayerComp> entityPayerComponents = null;
                                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                                {
                                    entityPayerComponents = daf.List(typeof(ISSEN.PayerComp), filter.GetPredicate()).Cast<ISSEN.PayerComp>().ToList();
                                }
                                if (entityPayerComponents != null && entityPayerComponents.Count > 0)
                                {
                                    policy.Summary.Premium = entityPayerComponents.Where(x => x != null && x.ComponentCode == premium).Sum(y => y.ComponentAmount);
                                    policy.Summary.Expenses = entityPayerComponents.Where(x => x != null && x.ComponentCode == Expenses).Sum(y => y.ComponentAmount);
                                    policy.Summary.Taxes = entityPayerComponents.Where(x => x != null && x.ComponentCode == taxes).Sum(y => y.ComponentAmount);
                                }
                                else
                                {
                                    throw new Exception(Errors.ErrorPayerComponents);
                                }
                            }
                            else
                            {
                                throw new Exception(Errors.ErrorPayerComponents);
                            }
                        }
                    }
                    else
                    {
                        filter = new ObjectCriteriaBuilder();
                        filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
                        filter.Equal();
                        filter.Constant(endorsementId);
                        List<ISSEN.PayerComp> entityPayerComponents = null;
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            entityPayerComponents = daf.List(typeof(ISSEN.PayerComp), filter.GetPredicate()).Cast<ISSEN.PayerComp>().ToList();
                        }
                        if (entityPayerComponents != null && entityPayerComponents.Count > 0)
                        {
                            filter = new ObjectCriteriaBuilder();
                            filter.Property(quotationEntitiesCore.Component.Properties.ComponentTypeCode, typeof(quotationEntitiesCore.Component).Name);
                            filter.In();
                            filter.ListValue();
                            filter.Constant(ComponentType.Premium);
                            filter.Constant(ComponentType.Expenses);
                            filter.Constant(ComponentType.Taxes);
                            filter.EndList();

                            List<quotationEntitiesCore.Component> components = null;
                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                components = daf.List(typeof(quotationEntitiesCore.Component), filter.GetPredicate()).Cast<quotationEntitiesCore.Component>().ToList();
                            }
                            if (components != null && components.Any())
                            {
                                var premium = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Premium).ComponentCode;
                                var Expenses = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Expenses).ComponentCode;
                                var taxes = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Taxes).ComponentCode;
                                policy.Summary.Premium += entityPayerComponents.Where(x => x != null && x.ComponentCode == premium).Sum(y => y.ComponentAmount);
                                policy.Summary.Expenses += entityPayerComponents.Where(x => x != null && x.ComponentCode == Expenses).Sum(y => y.ComponentAmount);
                                policy.Summary.Taxes += entityPayerComponents.Where(x => x != null && x.ComponentCode == taxes).Sum(y => y.ComponentAmount);
                            }
                        }
                    }

                    policy.Summary.FullPremium = policy.Summary.Premium + policy.Summary.Expenses + policy.Summary.Taxes;

                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.PolicyAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);
                    filter.And();
                    filter.Property(ISSEN.PolicyAgent.Properties.IsPrimary, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(true);
                    ISSEN.PolicyAgent entityPolicyAgent = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        entityPolicyAgent = daf.List(typeof(ISSEN.PolicyAgent), filter.GetPredicate()).Cast<ISSEN.PolicyAgent>().FirstOrDefault();
                    }
                    if (entityPolicyAgent != null)
                    {
                        AgencyDAO agencyDAO = new AgencyDAO();
                        IssuanceAgency agency = agencyDAO.GetAgencyByAgentIdAgentAgencyId(entityPolicyAgent.IndividualId, entityPolicyAgent.AgentAgencyId);

                        policy.Agencies = new List<IssuanceAgency>();
                        policy.Agencies.Add(agency);
                    }

                    CoverageDAO coverageDAO = new CoverageDAO();
                    CompanySummary companySummary = coverageDAO.getTotalSumary(endorsementId);
                    policy.Summary.RiskCount = companySummary.RiskCount;
                    policy.Summary.AmountInsured = companySummary.AmountInsured;
                    policy.Summary.PolicyId = endorsementEntity.PolicyId;
                    var imapper = ModelAssembler.CreateMapCompanyPolicy();
                    return imapper.Map<Policy, CompanyPolicy>(policy);
                }
                else
                {
                    throw new Exception(Errors.ErrorEndorsementNotFound);
                }
            }
            else
            {
                throw new Exception(Errors.ErrorEndorsementNotFound);
            }
        }


        /// <summary>
        /// Obtener Poliza por 
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="policyId">Id endoso</param>
        /// <returns>Objeto policy</returns>
        public CompanyPolicyControl GetTemporalPolicyControl(int policyId, int endorsementId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.TempPolicyControl.Properties.PolicyId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.TempPolicyControl.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            ISSEN.TempPolicyControl tmpPolicyControl = null;
            CompanyPolicyControl objPolicyControl = new CompanyPolicyControl();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                tmpPolicyControl = daf.List(typeof(ISSEN.TempPolicyControl), filter.GetPredicate()).Cast<ISSEN.TempPolicyControl>().FirstOrDefault();
            }
            if (tmpPolicyControl != null)
            {

                objPolicyControl = ModelAssembler.CreatePolicyControl(tmpPolicyControl);
            }

            return objPolicyControl;
        }


        public CompanyPolicy UpdateCompanyPolicyDocumentNumber(CompanyPolicy companyPolicy)
        {
            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("@ENDO_TYPE_CD", (int)companyPolicy.Endorsement.EndorsementType);
            parameters[1] = new NameValue("@BRANCH_CD", companyPolicy.Branch.Id);
            parameters[2] = new NameValue("@PREFIX_CD", companyPolicy.Prefix.Id);
            parameters[3] = new NameValue("@POLICY_ID", companyPolicy.Endorsement.PolicyId);
            object result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("ISS.UPDATE_MSV_POLICY_DOCUMENT_NUMBER", parameters);
            }
            if (result != null)
            {
                companyPolicy.DocumentNumber = Convert.ToDecimal(result.ToString());
            }

            return companyPolicy;
        }

        public int? GetOperationIdTemSubscription(int temporalId)
        {
            int? result = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = TMPEN.TempSubscription.CreatePrimaryKey(temporalId);
            TMPEN.TempSubscription tempSubscription = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                tempSubscription = (TMPEN.TempSubscription)daf.GetObjectByPrimaryKey(key);
            }
            if (tempSubscription != null)
            {
                if (tempSubscription.OperationId != 0)
                {
                    result = tempSubscription.OperationId;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public int GetEndorsementRiskCount(int policyId, EndorsementType endorsementType)
        {
            int countEndorsementRisk = 0;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);

            List<ISSEN.EndorsementRisk> endorsementRisk = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementRisk = daf.List(typeof(ISSEN.EndorsementRisk), filter.GetPredicate()).Cast<ISSEN.EndorsementRisk>().ToList();
            }

            switch (endorsementType)
            {
                case EndorsementType.Emission:
                    countEndorsementRisk = 0;
                    break;
                case EndorsementType.Modification:
                    var riskLastNumber = endorsementRisk.ToList().Where(x => x.RiskStatusCode != 0);

                    if (riskLastNumber != null && riskLastNumber.ToList().Count > 0)
                    {
                        countEndorsementRisk = riskLastNumber.ToList().OrderByDescending(x => x.RiskNum).FirstOrDefault().RiskNum;
                    }
                    else
                    {
                        countEndorsementRisk = 1;
                    }
                    break;
            }

            return countEndorsementRisk;
        }

        public int GetPolicyRiskCount(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(1); // activo
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Distinct();
            filter.Constant(3); // <> cancelada

            List<ISSEN.EndorsementRisk> endorsementRisk = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementRisk = daf.List(typeof(ISSEN.EndorsementRisk), filter.GetPredicate()).Cast<ISSEN.EndorsementRisk>().ToList();
            }

            var riskLastNumber = endorsementRisk.ToList().Where(x => x.RiskStatusCode != 3 && x.RiskStatusCode != 7);
            return riskLastNumber.ToList().Count;
        }

        public TemporalDTO GetTemporalByDocumentNumberPrefixIdBrachId(decimal documentNumber, int prefixId, int branchId)
        {
            TemporalDTO temporalDTO = new TemporalDTO { Id = 1 };
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TMPEN.TempSubscription.Properties.DocumentNumber, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            filter.And();
            filter.Property(TMPEN.TempSubscription.Properties.PrefixCode, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(TMPEN.TempSubscription.Properties.BranchCode, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(branchId);
            SelectQuery SelectQuery = new SelectQuery();
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(TMPEN.TempSubscription.Properties.OperationId, typeof(TMPEN.TempSubscription).Name), TMPEN.TempSubscription.Properties.OperationId));
            SelectQuery.AddSelectValue(new SelectValue(new Column(TMPEN.TempSubscription.Properties.EndorsementTypeCode, typeof(TMPEN.TempSubscription).Name), TMPEN.TempSubscription.Properties.EndorsementTypeCode));
            SelectQuery.AddSelectValue(new SelectValue(new Column(TMPEN.TempSubscription.Properties.EndorsementId, typeof(TMPEN.TempSubscription).Name), TMPEN.TempSubscription.Properties.EndorsementId));
            #endregion Select

            SelectQuery.Table = new ClassNameTable(typeof(TMPEN.TempSubscription), typeof(TMPEN.TempSubscription).Name);
            SelectQuery.Where = filter.GetPredicate();
            SelectQuery.GetFirstSelect();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    if (reader[TMPEN.TempSubscription.Properties.OperationId] != null)
                    {
                        temporalDTO.Id = (int)reader[TMPEN.TempSubscription.Properties.OperationId];
                        temporalDTO.EndorsementType = (int)reader[TMPEN.TempSubscription.Properties.EndorsementTypeCode];
                        temporalDTO.EndorsementId = (int)reader[TMPEN.TempSubscription.Properties.EndorsementId];
                    }
                    else
                    {
                        temporalDTO.Id = 0;
                        temporalDTO.EndorsementType = 0;
                    }
                    break;
                }
            }
            return temporalDTO;

        }
        /// <summary>
        /// Verifar Si el endoso tiene temporales
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public TemporalDTO GetTemporalByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            TemporalDTO temporalDTO = new TemporalDTO { Id = 1 };
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TMPEN.TempSubscription.Properties.PolicyId, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(TMPEN.TempSubscription.Properties.EndorsementId, typeof(TMPEN.TempSubscription).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            SelectQuery SelectQuery = new SelectQuery();
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(TMPEN.TempSubscription.Properties.OperationId, typeof(TMPEN.TempSubscription).Name), TMPEN.TempSubscription.Properties.OperationId));
            SelectQuery.AddSelectValue(new SelectValue(new Column(TMPEN.TempSubscription.Properties.EndorsementTypeCode, typeof(TMPEN.TempSubscription).Name), TMPEN.TempSubscription.Properties.EndorsementTypeCode));
            #endregion Select

            SelectQuery.Table = new ClassNameTable(typeof(TMPEN.TempSubscription), typeof(TMPEN.TempSubscription).Name);
            SelectQuery.Where = filter.GetPredicate();
            SelectQuery.GetFirstSelect();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    if (reader[TMPEN.TempSubscription.Properties.OperationId] != null)
                    {
                        temporalDTO.Id = (int)reader[TMPEN.TempSubscription.Properties.OperationId];
                        temporalDTO.EndorsementType = (int)reader[TMPEN.TempSubscription.Properties.EndorsementTypeCode];
                    }
                    else
                    {
                        temporalDTO.Id = 0;
                        temporalDTO.EndorsementType = 0;
                    }
                    break;
                }
            }
            return temporalDTO;
        }

        public List<EndorsementCompanyDTO> GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(int branchId, int prefixId, decimal policyNumber)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(branchId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(policyNumber);

            EndorsementCompanyView view = new EndorsementCompanyView();
            ViewBuilder builder = new ViewBuilder("EndorsementCompanyView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Policies.Count > 0)
            {
                var policies = view.Policies.Cast<ISSEN.Policy>().Select(m => new { m.PolicyId, m.ProductId }).ToList();
                var products = view.Products.Cast<Product>().Where(x => policies.Select(a => a.ProductId).Contains(x.ProductId)).Select(u => new { u.ProductId, u.Description }).ToList();
                var coEndorsement = view.CoEndorsements.Cast<ISSEN.CoEndorsement>().ToList();
                List<EndorsementCompanyDTO> endorsements = ModelAssembler.CreateMapCompanyEndorsements(view.Endorsements.Cast<ISSEN.Endorsement>().ToList());
                if (endorsements?.Count > 0)
                {
                    List<SummaryDTO> assuredSum = GetAssuredSumByPolicyId(policies.Select(m => m.PolicyId).ToList(), prefixId);
                    List<SummaryDTO> premiumSum = GetPremiumByPolicyId(policies.Select(m => m.PolicyId).ToList());

                    TP.Parallel.ForEach(endorsements, item =>
                    {
                        var culture = new System.Globalization.CultureInfo("es-CO");
                        CultureInfo.DefaultThreadCurrentCulture = culture;
                        CultureInfo.DefaultThreadCurrentUICulture = culture;

                        item.DescriptionEndorsementType = Errors.ResourceManager.GetString(EnumHelper.GetItemName<EndorsementType>(item.EndorsementType), culture);
                        item.DescriptionProduct = products.FirstOrDefault(m => m.ProductId == policies.First(a => a.PolicyId == item.PolicyId).ProductId).Description;
                        item.ModificationTypeId = coEndorsement.FirstOrDefault(x => x.EndorsementId == item.Id).EndoTypeCd.GetValueOrDefault();
                        item.AssuredSum = assuredSum.FirstOrDefault(m => m.EndorsementId == assuredSum.FirstOrDefault(a => a.EndorsementId == item.Id)?.EndorsementId)?.AssuredSum ?? 0;
                        item.TotalPremium = premiumSum.FirstOrDefault(m => m.EndorsementId == premiumSum.FirstOrDefault(a => a.EndorsementId == item.Id)?.EndorsementId)?.TotalPremium ?? 0;

                    });
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany");

                    return endorsements.ToList();
                }
                else
                {
                    return new List<EndorsementCompanyDTO>();
                }
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany");
                return null;
            }
        }
        #region optimizacion
        public List<EndorsementDTO> GetCompanyEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber, bool isCurrent = true)
        {
            List<EndorsementDTO> endorsementsDTO = null;
            int? policyId = GetCompanyCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            if (policyId.HasValue)
            {
                return GetCompanyCurrentEndorsementsByPolicyId(policyId.Value, isCurrent);
            }
            return endorsementsDTO;
        }

        public int? GetCompanyCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            var parameters = new NameValue[3];
            parameters[0] = new NameValue("DOCUMENT_NUMBER", policyNumber);
            parameters[1] = new NameValue("PREFIX_ID", prefixId);
            parameters[2] = new NameValue("BRANCH_ID", branchId);
            DataTable resultTable;
            using (var dataAccess = new DynamicDataAccess())
            {
                resultTable = dataAccess.ExecuteSPDataTable("ISS.GET_CURRENT_POLICY", parameters);
            }
            if (resultTable == null || resultTable.Rows.Count == 0)
            {
                return null;
            }
            return (int)resultTable.Rows[0]["POLICY_ID"];

        }
        public List<EndorsementDTO> GetCompanyCurrentEndorsementsByPolicyId(int policyId, bool isCurrent)
        {
            ConcurrentBag<EndorsementDTO> endorsements = new ConcurrentBag<EndorsementDTO>();
            var parameters = new NameValue[2];
            parameters[0] = new NameValue("POLICY_ID", policyId);
            parameters[1] = new NameValue("IS_CURRENT", isCurrent);

            DataTable resultTable;
            using (var dataAccess = new DynamicDataAccess())
            {
                resultTable = dataAccess.ExecuteSPDataTable("ISS.GET_CURRENTS_ENDORSEMENTS", parameters);
            }
            if (resultTable == null || resultTable.Rows.Count == 0)
            {
                return null;
            }
            ConcurrentBag<string> errors = new ConcurrentBag<string>();
            TP.Parallel.ForEach(resultTable.Rows.Cast<DataRow>(), drow =>
            {
                if (isCurrent)
                {
                    if (drow[0] == DBNull.Value)
                    {
                        errors.Add(Errors.ErrorEndorsementNotFound);
                    }
                    else
                    {
                        endorsements.Add(new EndorsementDTO { Id = Convert.ToInt32(drow[0]) });
                    }
                }
                else
                {
                    endorsements.Add(new EndorsementDTO { Id = Convert.ToInt32(drow[0]), State = Convert.ToInt32(drow[1]) });
                }
            });
            if (errors?.Count > 0)
            {
                throw new Exception(string.Join("", errors));
            }
            return endorsements.ToList();

        }

        /// <summary>
        /// Gets the current policy by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public CompanyPolicy GetCurrentPolicyByEndorsementId(int endorsementId, bool isCurrent = true)
        {
            ConcurrentBag<string> errors = new ConcurrentBag<string>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            ISSEN.Endorsement endorsementEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementEntity = daf.List(typeof(ISSEN.Endorsement), filter.GetPredicate()).Cast<ISSEN.Endorsement>().FirstOrDefault();
            }
            if (endorsementEntity != null)
            {
                List<Task> taskEntity = new List<Task>();

                ISSEN.Policy entityPolicy = null;
                PrimaryKey primaryKey = ISSEN.Policy.CreatePrimaryKey(endorsementEntity.PolicyId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityPolicy = (ISSEN.Policy)daf.GetObjectByPrimaryKey(primaryKey);
                }

                primaryKey = CORECOMM.Currency.CreatePrimaryKey(entityPolicy.CurrencyCode);
                CORECOMM.Currency entityCurrency = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityCurrency = (CORECOMM.Currency)daf.GetObjectByPrimaryKey(primaryKey);
                }
                if (entityPolicy != null)
                {
                    string descriptionPrefix = string.Empty;
                    string descriptionBranch = string.Empty;
                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        descriptionPrefix = DelegateService.commonService.GetPrefixById(entityPolicy.PrefixCode).Description;
                    }));
                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        descriptionBranch = DelegateService.commonService.GetBranchById(entityPolicy.BranchCode).Description;
                    }));
                    try
                    {
                        Task.WaitAll(taskEntity.ToArray());
                    }
                    catch (AggregateException ae)
                    {

                        errors.Add(ae.GetBaseException().Message);
                    }
                    taskEntity = new List<Task>();
                    Policy policy = new Policy
                    {
                        Id = 0,
                        CurrentFrom = entityPolicy.CurrentFrom,
                        CurrentTo = entityPolicy.CurrentTo ?? DateTime.MinValue,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Prefix = new COMMMO.Prefix()
                        {
                            Id = entityPolicy.PrefixCode,
                            Description = descriptionPrefix
                        },
                        Branch = new COMMMO.Branch()
                        {
                            Id = entityPolicy.BranchCode,
                            Description = descriptionBranch
                        },
                        IssueDate = endorsementEntity.IssueDate,
                        BusinessType = (BusinessType)entityPolicy.BusinessTypeCode,
                        ExchangeRate = new COMMMO.ExchangeRate
                        {
                            Currency = new COMMMO.Currency
                            {
                                Id = entityPolicy.CurrencyCode,
                                Description = entityCurrency.Description
                            },
                            SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today), entityPolicy.CurrencyCode).SellAmount
                        },
                        Endorsement = new Endorsement
                        {
                            Id = endorsementEntity.EndorsementId,
                            EndorsementType = (EndorsementType)endorsementEntity.EndoTypeCode,
                            CurrentFrom = endorsementEntity.CurrentFrom,
                            CurrentTo = endorsementEntity.CurrentTo.Value,
                            IsCurrent = true,
                            PolicyId = endorsementEntity.PolicyId,
                            Number = endorsementEntity.DocumentNum,
                            Text = new Text
                            {
                                TextBody = endorsementEntity.ConditionText
                            },
                        },
                        Summary = new Summary(),
                        UserId = endorsementEntity.UserId

                    };

                    ISSEN.CoPolicy entityCoPolicy = null;

                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            PrimaryKey pkCoPolicy = ISSEN.CoPolicy.CreatePrimaryKey(entityPolicy.PolicyId);
                            entityCoPolicy = (ISSEN.CoPolicy)daf.GetObjectByPrimaryKey(pkCoPolicy);
                        }
                        DataFacadeManager.Dispose();
                    }).ContinueWith((result) =>
                    {
                        CORECOMM.CoPolicyType entityCoPolicyType = null;
                        taskEntity.Add(TP.Task.Run(() =>
                        {
                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                PrimaryKey pkEntity = CORECOMM.CoPolicyType.CreatePrimaryKey(entityPolicy.PrefixCode, entityCoPolicy.PolicyTypeCode);
                                entityCoPolicyType = (CORECOMM.CoPolicyType)daf.GetObjectByPrimaryKey(pkEntity);
                                policy.PolicyType = new COMMMO.PolicyType() { Description = entityCoPolicyType.Description, IsFloating = entityCoPolicyType.Floating };
                            }
                            DataFacadeManager.Dispose();
                        }));
                    }));

                    HolderDAO holderDAO = new HolderDAO();

                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        policy.Holder = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(entityPolicy.PolicyholderId.Value.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                        DataFacadeManager.Dispose();
                    }));
                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        policy.Product = DelegateService.productService.GetProductById(entityPolicy.ProductId.Value);
                        DataFacadeManager.Dispose();
                    }));
                    try
                    {
                        Task.WaitAll(taskEntity.ToArray());
                    }
                    catch (AggregateException ae)
                    {

                        errors.Add(ae.GetBaseException().Message);
                    }
                    EndorsementDAO endorsementDAO = new EndorsementDAO();
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(quotationEntitiesCore.Component.Properties.ComponentTypeCode, typeof(quotationEntitiesCore.Component).Name);
                    filter.In();
                    filter.ListValue();
                    filter.Constant(ComponentType.Premium);
                    filter.Constant(ComponentType.Expenses);
                    filter.Constant(ComponentType.Taxes);
                    filter.EndList();
                    List<quotationEntitiesCore.Component> components = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        components = daf.List(typeof(quotationEntitiesCore.Component), filter.GetPredicate()).Cast<quotationEntitiesCore.Component>().ToList();
                    }
                    if (components != null && components.Any())
                    {
                        var premium = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Premium).ComponentCode;
                        var Expenses = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Expenses).ComponentCode;
                        var taxes = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Taxes).ComponentCode;
                        filter = new ObjectCriteriaBuilder();
                        if (endorsementEntity.EndoTypeCode == (int)EndorsementType.Nominative_cancellation)

                        {

                            filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
                            filter.Equal().Constant(endorsementEntity.EndorsementId);

                        }

                        else

                        {

                            filter.Property(ISSEN.PayerComp.Properties.PolicyId, typeof(ISSEN.PayerComp).Name);
                            filter.Equal().Constant(endorsementEntity.PolicyId);

                        }

                        List<ISSEN.PayerComp> entityPayerComponents = null;
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            entityPayerComponents = daf.List(typeof(ISSEN.PayerComp), filter.GetPredicate()).Cast<ISSEN.PayerComp>().ToList();
                        }
                        if (entityPayerComponents != null && entityPayerComponents.Count > 0)
                        {
                            policy.Summary.Premium = entityPayerComponents.Where(x => x != null && x.ComponentCode == premium).Sum(y => y.ComponentAmount);
                            policy.Summary.Expenses = entityPayerComponents.Where(x => x != null && x.ComponentCode == Expenses).Sum(y => y.ComponentAmount);
                            policy.Summary.Taxes = entityPayerComponents.Where(x => x != null && x.ComponentCode == taxes).Sum(y => y.ComponentAmount);
                        }
                        else
                        {
                            throw new Exception(Errors.ErrorPayerComponents);
                        }
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorPayerComponents);
                    }

                    policy.Summary.FullPremium = policy.Summary.Premium + policy.Summary.Expenses + policy.Summary.Taxes;

                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.PolicyAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);
                    filter.And();
                    filter.Property(ISSEN.PolicyAgent.Properties.IsPrimary, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(true);
                    ISSEN.PolicyAgent entityPolicyAgent = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        entityPolicyAgent = daf.List(typeof(ISSEN.PolicyAgent), filter.GetPredicate()).Cast<ISSEN.PolicyAgent>().FirstOrDefault();
                    }
                    if (entityPolicyAgent != null)
                    {
                        AgencyDAO agencyDAO = new AgencyDAO();
                        IssuanceAgency agency = agencyDAO.GetAgencyByAgentIdAgentAgencyId(entityPolicyAgent.IndividualId, entityPolicyAgent.AgentAgencyId);
                        agency.IsPrincipal = entityPolicyAgent.IsPrimary;

                        if (entityPolicyAgent.SalesPointCode.HasValue)
                        {
                            ObjectCriteriaBuilder filterSalePoint = new ObjectCriteriaBuilder();
                            filterSalePoint.Property(CORECOMM.SalePoint.Properties.SalePointCode, typeof(CORECOMM.SalePoint).Name);
                            filterSalePoint.Equal();
                            filterSalePoint.Constant(entityPolicyAgent.SalesPointCode);
                            CORECOMM.SalePoint SalePointEntity = null;

                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                SalePointEntity = daf.List(typeof(CORECOMM.SalePoint), filterSalePoint.GetPredicate()).Cast<CORECOMM.SalePoint>().FirstOrDefault();
                            }

                            DataFacadeManager.Dispose();

                            ObjectCriteriaBuilder filterBranch = new ObjectCriteriaBuilder();
                            filterBranch.Property(CORECOMM.Branch.Properties.BranchCode, typeof(CORECOMM.Branch).Name);
                            filterBranch.Equal();
                            filterBranch.Constant(SalePointEntity.BranchCode);
                            CORECOMM.Branch BranchEntity = null;

                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                BranchEntity = daf.List(typeof(CORECOMM.Branch), filterBranch.GetPredicate()).Cast<CORECOMM.Branch>().FirstOrDefault();
                            }

                            DataFacadeManager.Dispose();

                            agency.Branch = new COMMMO.Branch();
                            agency.Branch.Id = (int)SalePointEntity.BranchCode;
                            agency.Branch.Description = BranchEntity.Description;
                            agency.Branch.SmallDescription = BranchEntity.SmallDescription;
                            agency.Branch.SalePoints = new List<COMMMO.SalePoint>();
                            agency.Branch.SalePoints.Add(new COMMMO.SalePoint() { Id = (int)entityPolicyAgent.SalesPointCode, Description = SalePointEntity.Description, SmallDescription = SalePointEntity.SmallDescription });
                        }
                        else
                        {
                            ObjectCriteriaBuilder filterBranch = new ObjectCriteriaBuilder();
                            filterBranch.Property(CORECOMM.Branch.Properties.BranchCode, typeof(CORECOMM.Branch).Name);
                            filterBranch.Equal();
                            filterBranch.Constant(entityPolicy.BranchCode);
                            CORECOMM.Branch BranchEntity = null;

                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                BranchEntity = daf.List(typeof(CORECOMM.Branch), filterBranch.GetPredicate()).Cast<CORECOMM.Branch>().FirstOrDefault();
                            }

                            DataFacadeManager.Dispose();

                            ObjectCriteriaBuilder filterSalePoint = new ObjectCriteriaBuilder();
                            filterSalePoint.Property(CORECOMM.SalePoint.Properties.SalePointCode, typeof(CORECOMM.SalePoint).Name);
                            filterSalePoint.Equal();
                            filterSalePoint.Constant(entityPolicy.SalePointCode);
                            CORECOMM.SalePoint SalePointEntity = null;

                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                SalePointEntity = daf.List(typeof(CORECOMM.SalePoint), filterSalePoint.GetPredicate()).Cast<CORECOMM.SalePoint>().FirstOrDefault();
                            }

                            DataFacadeManager.Dispose();

                            agency.Branch = new COMMMO.Branch();
                            agency.Branch.Id = (int)entityPolicy.BranchCode;
                            agency.Branch.Description = BranchEntity.Description;
                            agency.Branch.SmallDescription = BranchEntity.SmallDescription;
                            agency.Branch.SalePoints = new List<COMMMO.SalePoint>();
                            agency.Branch.SalePoints.Add(new COMMMO.SalePoint() { Id = (int)entityPolicy.SalePointCode, Description = SalePointEntity.Description, SmallDescription = SalePointEntity.SmallDescription });
                        }

                        policy.Agencies = new List<IssuanceAgency>();
                        policy.Agencies.Add(agency);
                    }

                    CoverageDAO coverageDAO = new CoverageDAO();
                    CompanySummary companySummary = coverageDAO.getTotalSumary(endorsementId);
                    policy.Summary.RiskCount = companySummary.RiskCount;
                    policy.Summary.AmountInsured = companySummary.AmountInsured;
                    policy.Summary.PolicyId = endorsementEntity.PolicyId;

                    CompanyPolicyControl Control = null;
                    try { Control = GetTemporalPolicyControl(endorsementEntity.PolicyId, endorsementId); } catch (Exception) { }
                    //InfoCoasegurado Aceptado
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(ISSEN.CoinsuranceAccepted.Properties.EndorsementId, typeof(ISSEN.CoinsuranceAccepted).Name, endorsementId);
                    ISSEN.CoinsuranceAccepted entitycoinsuranceAccepted = null;
                    entitycoinsuranceAccepted = DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.CoinsuranceAccepted), filter.GetPredicate()).Cast<ISSEN.CoinsuranceAccepted>().FirstOrDefault();

                    //InfoCoasegurado Assignado
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(ISSEN.CoinsuranceAssigned.Properties.EndorsementId, typeof(ISSEN.CoinsuranceAssigned).Name, endorsementId);
                    BusinessCollection businessObjectsAssigned = DataFacadeManager.GetObjects(typeof(ISSEN.CoinsuranceAssigned), filter.GetPredicate());
                    policy.CoInsuranceCompanies = new List<IssuanceCoInsuranceCompany>();

                    if (entitycoinsuranceAccepted != null)
                    {
                        policy.CoInsuranceCompanies.Add(ModelAssembler.CreateCoInsuranceAccepted(entitycoinsuranceAccepted));
                    }

                    if (businessObjectsAssigned.Count > 0)
                    {
                        policy.CoInsuranceCompanies = ModelAssembler.CreateCoInsurancesAssigneds(businessObjectsAssigned);
                        foreach (IssuanceCoInsuranceCompany coInsurance in policy.CoInsuranceCompanies)
                        {
                            coInsurance.ParticipationPercentageOwn = entityPolicy.CoissuePercentage.GetValueOrDefault();
                        }
                    }

                    foreach (IssuanceCoInsuranceCompany item in policy.CoInsuranceCompanies)
                    {
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(CORECOMM.CoInsuranceCompany.Properties.InsuranceCompanyId, typeof(CORECOMM.CoInsuranceCompany).Name, item.Id);
                        CORECOMM.CoInsuranceCompany entityCoInsuranceCompany = null;
                        entityCoInsuranceCompany = DataFacadeManager.Instance.GetDataFacade().List(typeof(CORECOMM.CoInsuranceCompany), filter.GetPredicate()).Cast<CORECOMM.CoInsuranceCompany>().FirstOrDefault();

                        if (item.Id == entityCoInsuranceCompany.InsuranceCompanyId)
                        {
                            item.Description = entityCoInsuranceCompany.Description;
                        }
                    }
                    var imapper = ModelAssembler.CreateMapCompanyPolicy();
                    CompanyPolicy companyPolicy = imapper.Map<Policy, CompanyPolicy>(policy);

                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(CiaCoinsuranceAccepted.Properties.EndorsementId, typeof(CiaCoinsuranceAccepted).Name, endorsementId);
                    BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(CiaCoinsuranceAccepted), filter.GetPredicate());
                    if (businessObjects.Count > 0)
                    {
                        companyPolicy.CoInsuranceCompanies[0].acceptCoInsuranceAgent = ModelAssembler.CreateCiaCoInsuranceAccepteds(businessObjects);
                        filter = new ObjectCriteriaBuilder();
                        filter.Property(UPENT.Agent.Properties.IndividualId, typeof(UPENT.Agent).Name);
                        filter.In();
                        filter.ListValue();
                        foreach (CompanyAcceptCoInsuranceAgent companyAcceptCoInsuranceAgent in companyPolicy.CoInsuranceCompanies[0].acceptCoInsuranceAgent)
                        {
                            filter.Constant(companyAcceptCoInsuranceAgent.Agent.IndividualId);
                        }
                        filter.EndList();

                        List<UPENT.Agent> entityAgents = null;
                        entityAgents = DataFacadeManager.Instance.GetDataFacade().List(typeof(UPENT.Agent), filter.GetPredicate()).Cast<UPENT.Agent>().ToList();

                        foreach (CompanyAcceptCoInsuranceAgent companyAcceptCoInsuranceAgent in companyPolicy.CoInsuranceCompanies[0].acceptCoInsuranceAgent)
                        {
                            companyAcceptCoInsuranceAgent.Agent.FullName = entityAgents.Where(x => x.IndividualId == companyAcceptCoInsuranceAgent.Agent.IndividualId).FirstOrDefault().CheckPayableTo;
                        }

                    }
                    companyPolicy.PolicyOrigin = Control?.PolicyOrigin ?? PolicyOrigin.Individual;
                    return companyPolicy;
                }
                else
                {
                    throw new Exception(Errors.ErrorEndorsementNotFound);
                }
            }
            else
            {
                throw new Exception(Errors.ErrorEndorsementNotFound);
            }
        }
        private List<SummaryDTO> GetAssuredSumByPolicyId(List<int> policyIds, int? prefix)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<SummaryDTO> listSummaryDTOs = new List<SummaryDTO>();
            SelectQuery selectQuery = new SelectQuery();

            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.In();
            filter.ListValue();
            foreach (int policyId in policyIds)
            {
                filter.Constant(policyId);
            }
            filter.EndList();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskCoverage.Properties.EndorsementLimitAmount, typeof(ISSEN.RiskCoverage).Name)));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name)));
            selectQuery.AddSelectValue(new SelectValue(new Column(QUO.Coverage.Properties.IsPrimary, typeof(QUO.Coverage).Name)));
            Join join = new Join(new ClassNameTable(typeof(ISSEN.EndorsementRiskCoverage), typeof(ISSEN.EndorsementRiskCoverage).Name), new ClassNameTable(typeof(ISSEN.RiskCoverage), typeof(ISSEN.RiskCoverage).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.RiskCoverage.Properties.RiskCoverId, typeof(ISSEN.RiskCoverage).Name)
                .Equal()
                .Property(ISSEN.EndorsementRiskCoverage.Properties.RiskCoverId, typeof(ISSEN.EndorsementRiskCoverage).Name).GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(QUO.Coverage), typeof(QUO.Coverage).Name), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(ISSEN.RiskCoverage.Properties.CoverageId, typeof(ISSEN.RiskCoverage).Name).Equal()
                    .Property(QUO.Coverage.Properties.CoverageId, typeof(QUO.Coverage).Name).GetPredicate()
            };

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    if ((COUN.PrefixType)prefix == COUN.PrefixType.Responsabilidad)
                    {
                        var principal = Convert.ToBoolean(reader[QUO.Coverage.Properties.IsPrimary]);
                        if (principal == true)
                        {
                            listSummaryDTOs.Add(new SummaryDTO()
                            {
                                AssuredSum = Convert.ToDecimal(reader[ISSEN.RiskCoverage.Properties.EndorsementLimitAmount]),
                                EndorsementId = Convert.ToInt32(reader[ISSEN.EndorsementRiskCoverage.Properties.EndorsementId]),
                                Primary = Convert.ToBoolean(reader[QUO.Coverage.Properties.IsPrimary])
                            });
                        }
                    }
                    else
                    {
                        listSummaryDTOs.Add(new SummaryDTO()
                        {
                            AssuredSum = Convert.ToDecimal(reader[ISSEN.RiskCoverage.Properties.EndorsementLimitAmount]),
                            EndorsementId = Convert.ToInt32(reader[ISSEN.EndorsementRiskCoverage.Properties.EndorsementId])
                        });
                    }

                }
                return listSummaryDTOs.GroupBy(x => x.EndorsementId = x.EndorsementId).Select(y => new SummaryDTO { EndorsementId = y.Key, AssuredSum = y.Select(a => a.AssuredSum).Sum() }).ToList();
            }
        }
        private List<SummaryDTO> GetPremiumByPolicyId(List<int> policyIds)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<SummaryDTO> listSummaryDTOs = new List<SummaryDTO>();

            filter.Property(ISSEN.PayerComp.Properties.PolicyId, typeof(ISSEN.PayerComp).Name);
            filter.In();
            filter.ListValue();
            foreach (int policyId in policyIds)
            {
                filter.Constant(policyId);
            }
            filter.EndList();
            List<ISSEN.PayerComp> entityPayerComponents = new List<ISSEN.PayerComp>();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                entityPayerComponents = daf.List(typeof(ISSEN.PayerComp), filter.GetPredicate()).Cast<ISSEN.PayerComp>().ToList();
            }
            if (entityPayerComponents != null && entityPayerComponents.Count > 0)
            {
                filter = new ObjectCriteriaBuilder();
                filter.Property(quotationEntitiesCore.Component.Properties.ComponentTypeCode, typeof(quotationEntitiesCore.Component).Name);
                filter.In();
                filter.ListValue();
                filter.Constant(ComponentType.Premium);
                filter.Constant(ComponentType.Expenses);
                filter.Constant(ComponentType.Taxes);
                filter.EndList();

                List<quotationEntitiesCore.Component> components = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    components = daf.List(typeof(quotationEntitiesCore.Component), filter.GetPredicate()).Cast<quotationEntitiesCore.Component>().ToList();
                }
                if (components != null && components.Any())
                {
                    var endorsements = entityPayerComponents.GroupBy(x => x.EndorsementId).ToList();
                    int premiumComponent = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Premium).ComponentCode;
                    int expensesComponent = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Expenses).ComponentCode;
                    int taxesComponent = components.FirstOrDefault(x => (ComponentType)x.ComponentTypeCode == ComponentType.Taxes).ComponentCode;
                    foreach (var item in endorsements.Select(a => a.Key))
                    {
                        decimal premium = decimal.Zero;
                        decimal expenses = decimal.Zero;
                        decimal taxes = decimal.Zero;
                        var payerComponetsAll = endorsements.Where(x => x.Key == item).SelectMany(a => a).ToList();
                        premium += payerComponetsAll.Where(x => x != null && x.ComponentCode == premiumComponent).Sum(y => y.ComponentAmount);
                        expenses += payerComponetsAll.Where(x => x != null && x.ComponentCode == expensesComponent).Sum(y => y.ComponentAmount);
                        taxes += payerComponetsAll.Where(x => x != null && x.ComponentCode == taxesComponent).Sum(y => y.ComponentAmount);

                        listSummaryDTOs.Add(new SummaryDTO
                        {
                            TotalPremium = Convert.ToDecimal(premium + expenses + taxes),
                            EndorsementId = item
                        });
                    }
                }

                return listSummaryDTOs.ToList();
            }
            else
            {
                return null;
            }
        }
        #endregion optimizacion

        #region ContractObject
        public Endorsement SaveContractObjectPolicyId(int endorsementId, int riskId, string textRisk, string textPolicy)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name, endorsementId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(ISSEN.Endorsement), filter.GetPredicate());
            List<Endorsement> endorsements = ModelAssembler.CreateMapCompanyEndorsements(businessObjects);
            Endorsement endorsement = endorsements.Last();
            endorsement.Text.TextBody = textPolicy;
            ISSEN.Endorsement entityendorsement = EntityAssembler.CreateCompanyEndorsement(endorsement);
            DataFacadeManager.Update(entityendorsement);

            ObjectCriteriaBuilder filterRisk = new ObjectCriteriaBuilder();
            filterRisk.PropertyEquals(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name, riskId);
            BusinessCollection businessObjects1 = DataFacadeManager.GetObjects(typeof(ISSEN.Risk), filterRisk.GetPredicate());
            List<RiskChangeText> risks = ModelAssembler.CreateMapCompanyRisk(businessObjects1);
            RiskChangeText risk = risks.Last();
            risk.ConditionText = textRisk;
            ISSEN.Risk entityrisk = EntityAssembler.CreateCompanyRisks(risk);
            DataFacadeManager.Update(entityrisk);
            endorsement = ModelAssembler.CreateCompanyEndorsement(entityendorsement, entityrisk);



            return endorsement;
        }

        public void CreateCoEndoChangeTextControl(int endorsementId, int riskId)
        {
            try
            {
                ModelAssembler.CreateCoEndoChangeTextControl((INTEN.IssCoEndoChangeTextControl)DataFacadeManager.Insert(EntityAssembler.CreateCoEndoChangeTextControl(endorsementId, riskId)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("No integra el Objecto de Contracto", "Sistran.Company.Application.UnderwritingServices.EEProvider.PolicyDAO.CreateCoEndoChangeTextControl");
            }
        }



        public EndoChangeText SaveLog(EndoChangeText endoChangeText)
        {
            ISSEN.EndoChangeText entityEndoChangeText = EntityAssembler.CreateEndoChangeText(endoChangeText);
            DataFacadeManager.Insert(entityEndoChangeText);
            EndoChangeText EndoChangeText = ModelAssembler.CreateEndoChangeText(entityEndoChangeText);

            CreateCoEndoChangeTextControl(entityEndoChangeText.EndoChangeTextCode, 0);
            return EndoChangeText;
        }
        #endregion


        public decimal GetCumulusQSise(int individualId) {
            Param[] parameters = new Param[5];
            parameters[0] = new Param("@CUTOFF_DATE", DateTime.Now);
            parameters[1] = new Param("@PREFIX", 30);
            parameters[2] = new Param("@INDIVIDUAL_ID", individualId);
            parameters[3] = new Param("@FUTURAS", 1);
            parameters[4] = new Param("@DEBUG", 0);



            var result = DataFacadeManager.Instance.GetDataFacade().ExecuteSPReader("REPORT.QS_REINS_CUMULOS", parameters);

            if (result.ToArray().Any())
            {
                var item = (result[0] as object[]);

                decimal.TryParse(item[2].ToString(), out decimal cumuloIndividual);
                decimal.TryParse(item[6].ToString(), out decimal cumuloGrupoEconomico);

                return cumuloIndividual + cumuloGrupoEconomico;
            }

            return 0;
        }
    }
}
