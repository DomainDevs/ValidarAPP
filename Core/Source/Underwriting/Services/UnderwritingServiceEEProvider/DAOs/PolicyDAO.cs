using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.underwritingService.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using ComponentType = Sistran.Core.Application.UnderwritingServices.Enums.ComponentType;
using CORUT = Sistran.Core.Application.Utilities.Helper;
using EnumsUnCo = Sistran.Core.Application.UnderwritingServices.Enums;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using PRODModel = Sistran.Core.Application.ProductServices.Models;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PolicyDAO
    {


        /// <summary>
        /// Obtener Endosos de una Póliza
        /// </summary>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <param name="branchId">Id Sucursal</param>
        /// <param name="policyNumber">Número de Póliza</param>
        /// <returns>Endosos</returns>
        public List<Models.Endorsement> GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, int riskId, bool isCurrent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (prefixId == 0 && branchId == 0)
            {
                filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policyNumber);
            }
            else
            {
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
            }

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

            EndorsementView view = new EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Policies.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                ConcurrentBag<Models.Endorsement> endorsements = new ConcurrentBag<Models.Endorsement>();
                TP.Parallel.ForEach(view.Policies.Cast<ISSEN.Policy>().ToList(), item =>
                {
                    branchId = item.BranchCode;
                    prefixId = item.PrefixCode;
                });
                TP.Parallel.ForEach(view.Endorsements.Cast<ISSEN.Endorsement>().ToList(), item =>
                {
                    endorsements.Add(new Models.Endorsement
                    {
                        Id = item.EndorsementId,
                        CurrentFrom = item.CurrentFrom,
                        CurrentTo = item.CurrentTo.Value,
                        Description = item.DocumentNum.ToString(),
                        Number = item.DocumentNum,
                        PolicyId = item.PolicyId,
                        EndorsementType = (Enums.EndorsementType)item.EndoTypeCode,
                        Branch = new Branch { Id = branchId },
                        Prefix = new Prefix { Id = prefixId }
                    });
                });


                ISSEN.EndorsementRisk endorsementRisk = endorsementRisks?.OrderByDescending(x => x.EndorsementId)?.FirstOrDefault(x => x.IsCurrent);
                if (endorsementRisk != null)
                {
                    endorsements.First(x => x.Id == endorsementRisk.EndorsementId).IsCurrent = true;
                }
                else
                {
                    throw new Exception(Errors.ErrorRiskNotFound);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetEndorsementsByPrefixIdBranchIdPolicyNumber");

                return endorsements.ToList();
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetEndorsementsByPrefixIdBranchIdPolicyNumber");

                return null;
            }
        }

        /// <summary>
        /// Obtener resumen de un endoso
        /// </summary>
        /// <param name="endorsementId">Id endoso</param>
        /// <param name="isCurrent">endoso Actual</param>
        /// <returns>Modelo summary</returns>
        public Models.Policy GetSummaryByEndorsementId(int endorsementId, bool isCurrent = true)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Models.Policy policy = new Models.Policy();
            //Obtener datos del endoso
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.Endorsement), filter.GetPredicate());
            ISSEN.Endorsement endorsementEntity = businessCollection.Cast<ISSEN.Endorsement>().FirstOrDefault();

            if (endorsementEntity != null)
            {
                policy.Id = endorsementEntity.PolicyId;
                policy.IssueDate = endorsementEntity.IssueDate;
                policy.Endorsement = new Models.Endorsement
                {
                    CurrentFrom = endorsementEntity.CurrentFrom,
                    CurrentTo = endorsementEntity.CurrentTo.Value,
                    EndorsementType = (Enums.EndorsementType)endorsementEntity.EndoTypeCode,
                    Number = endorsementEntity.DocumentNum
                };
                policy.Text = new Models.Text
                {
                    TextBody = endorsementEntity.ConditionText,
                    Observations = endorsementEntity.Annotations
                };
                policy.CurrentFrom = endorsementEntity.CurrentFrom;
                policy.CurrentTo = endorsementEntity.CurrentTo ?? DateTime.Now;
            }


            //Resumen de la prima
            filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(Enums.ComponentType.Premium);
            filter.Constant(Enums.ComponentType.Expenses);
            filter.Constant(Enums.ComponentType.Taxes);
            filter.EndList();
            IList components = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.Component), filter.GetPredicate());

            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            IList iList = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PayerComp), filter.GetPredicate());
            List<ISSEN.PayerComp> payerComponents = iList.Cast<ISSEN.PayerComp>().ToList();

            if (payerComponents != null)
            {
                policy.Summary = new Models.Summary();

                foreach (QUOEN.Component item in components)
                {
                    switch ((Enums.ComponentType)item.ComponentTypeCode)
                    {
                        case Enums.ComponentType.Premium:
                            policy.Summary.Premium += payerComponents.Where(x => x.ComponentCode == item.ComponentCode).Sum(y => y.ComponentAmount);
                            break;
                        case Enums.ComponentType.Expenses:
                            policy.Summary.Expenses += payerComponents.Where(x => x.ComponentCode == item.ComponentCode).Sum(y => y.ComponentAmount);
                            break;
                        case Enums.ComponentType.Taxes:
                            policy.Summary.Taxes += payerComponents.Where(x => x.ComponentCode == item.ComponentCode).Sum(y => y.ComponentAmount);
                            break;
                        default:
                            break;
                    }
                }

                policy.Summary.FullPremium = policy.Summary.Premium + policy.Summary.Expenses + policy.Summary.Taxes;
            }


            //Obtener numero de riesgos y las coberturas para la suma asegurada
            EndorsementDAO endorsementDAO = new EndorsementDAO();
            List<Endorsement> endorsements = endorsementDAO.GetEffectiveEndorsementsByPolicyId(policy.Id);
            if (endorsements != null && endorsements.Count > 0)
            {
                filter = new ObjectCriteriaBuilder();
                int lastEndorsementId = 0;
                filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                filter.In();
                filter.ListValue();
                foreach (Endorsement endorsement in endorsements)
                {
                    filter.Constant(endorsement.Id);
                    lastEndorsementId = endorsement.Id;
                }
                filter.EndList();

                //En endoso actual no se muestran los excluidos/cancelados
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filter.In();
                filter.ListValue();
                filter.Constant(Enums.RiskStatusType.Included);
                filter.Constant(Enums.RiskStatusType.Modified);
                filter.Constant(Enums.RiskStatusType.NotModified);
                filter.Constant(Enums.RiskStatusType.Original);
                filter.Constant(Enums.RiskStatusType.Rehabilitated);

                if (policy.Endorsement.EndorsementType == Enums.EndorsementType.Cancellation || policy.Endorsement.EndorsementType == Enums.EndorsementType.Nominative_cancellation || policy.Endorsement.EndorsementType == Enums.EndorsementType.LastEndorsementCancellation)
                {
                    filter.Constant(Enums.RiskStatusType.Excluded);
                    filter.Constant(Enums.RiskStatusType.Cancelled);
                }
                filter.EndList();
                if (isCurrent)
                {
                    filter.And();
                    filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(true);
                }

                CoverageDAO coverageDAO = new CoverageDAO();
                policy.Summary = coverageDAO.GetCoveragesEndorsementSummary(filter, policy.Summary, policy.Endorsement.EndorsementType);

                if (policy.Summary != null && policy.Summary.RiskCount > 0)
                {
                    policy.Id = policy.Summary.PolicyId;
                    //Obtener datos de la poliza
                    PrimaryKey keyPolicy = ISSEN.Policy.CreatePrimaryKey(policy.Id);
                    ISSEN.Policy policyEntity = (ISSEN.Policy)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyPolicy);

                    if (policyEntity != null)
                    {
                        policy.BusinessType = (Enums.BusinessType)policyEntity.BusinessTypeCode;
                        policy.ExchangeRate = new CommonService.Models.ExchangeRate();
                        policy.ExchangeRate.Currency = new CommonService.Models.Currency
                        {
                            Id = policyEntity.CurrencyCode
                        };
                        policy.Holder = new Models.Holder();
                        policy.Holder.IndividualId = policyEntity.PolicyholderId.Value;
                        policy.Product = new PRODModel.Product
                        {
                            Id = policyEntity.ProductId.Value
                        };
                    }

                    if (endorsements != null)
                    {
                        if (endorsements.Exists(x => x.Id == endorsementId))
                        {
                            if (endorsements.Where(x => x.Id == endorsementId).First().IsCurrent)
                            {
                                if (endorsements.Exists(x => x.EndorsementType == Enums.EndorsementType.Renewal))
                                {
                                    policy.CurrentFrom = endorsements.Last(x => x.EndorsementType == Enums.EndorsementType.Renewal).CurrentFrom;
                                }
                                else if (endorsements.Exists(x => x.EndorsementType == Enums.EndorsementType.ChangeTermEndorsement))
                                {
                                    policy.CurrentFrom = endorsements.First(x => x.EndorsementType == Enums.EndorsementType.ChangeTermEndorsement).CurrentFrom;
                                }
                                else if (endorsements.Exists(x => x.EndorsementType == Enums.EndorsementType.ChangeAgentEndorsement))
                                {
                                    policy.CurrentFrom = endorsements.First(x => x.EndorsementType == Enums.EndorsementType.ChangeAgentEndorsement).CurrentFrom;
                                }
                                else
                                {
                                    policy.CurrentFrom = endorsements.First(x => x.EndorsementType == Enums.EndorsementType.Emission).CurrentFrom;
                                }
                                policy.CurrentTo = endorsements.Where(x => x.IsCurrent == true).First().CurrentTo;
                            }
                            else
                            {
                                policy.CurrentFrom = endorsements.Where(x => x.Id == endorsementId).First().CurrentFrom;
                                policy.CurrentTo = endorsements.Where(x => x.Id == endorsementId).First().CurrentTo;
                            }
                        }
                    }

                    //***************************************---------------------**********************************************

                    //Obtener descripcion moneda
                    PrimaryKey keyCurrency = COMMEN.Currency.CreatePrimaryKey(policy.ExchangeRate.Currency.Id);
                    COMMEN.Currency currencyEntity = (COMMEN.Currency)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCurrency);

                    if (currencyEntity != null)
                    {
                        policy.ExchangeRate.Currency.Description = currencyEntity.Description;
                    }

                    //Obtener tomador
                    HolderDAO holderDAO = new HolderDAO();
                    List<Models.Holder> holders = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    if (holders.Count > 0)
                    {
                        policy.Holder = holders[0];
                    }

                    //Obtener intermediario
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.PolicyAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);
                    filter.And();
                    filter.Property(ISSEN.PolicyAgent.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(policy.Id);
                    IList iListAgent = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PolicyAgent), filter.GetPredicate());
                    List<ISSEN.PolicyAgent> policyAgent = iListAgent.Cast<ISSEN.PolicyAgent>().OrderByDescending(b => b.IsPrimary).ToList();
                    policy.Agencies = new List<IssuanceAgency>();

                    //Obtiene Comisiones
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.CommissAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);
                    filter.And();
                    filter.Property(ISSEN.CommissAgent.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(policy.Id);
                    IList iListComission = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.CommissAgent), filter.GetPredicate());
                    List<ISSEN.CommissAgent> commissAgent = iListComission.Cast<ISSEN.CommissAgent>().ToList();
                    for (int i = 0; i < policyAgent.Count; i++)
                    {
                        AgencyDAO agencyDAO = new AgencyDAO();
                        IssuanceAgency issuanceAgency = agencyDAO.GetAgencyByAgentIdAgentAgencyId(policyAgent[i].IndividualId, policyAgent[i].AgentAgencyId);

                        issuanceAgency.Participation = commissAgent.Where(b => b.AgentAgencyId == policyAgent[i].AgentAgencyId).FirstOrDefault().StComissionPercentage;
                        issuanceAgency.Commissions = new List<IssuanceCommission>();
                        issuanceAgency.Commissions.Add(new IssuanceCommission { Percentage = commissAgent.Where(b => b.AgentAgencyId == policyAgent[i].AgentAgencyId).FirstOrDefault().AgentPartPercentage });
                        issuanceAgency.FullName = issuanceAgency.Agent.FullName;
                        policy.Agencies.Add(issuanceAgency);

                    }


                    //Obtener producto
                    PrimaryKey keyProduct = PRODEN.Product.CreatePrimaryKey(policy.Product.Id);
                    PRODEN.Product productEntity = (PRODEN.Product)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyProduct);

                    if (productEntity != null)
                    {
                        policy.Product.Description = productEntity.Description;
                        policy.Product.IsCollective = productEntity.IsCollective == null ? false : true;
                    }

                    //Obtener Plan de pago
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.EndorsementPayer.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(policy.Id);
                    BusinessCollection businessCollectionPayer = DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.EndorsementPayer), filter.GetPredicate());
                    policy.PaymentPlan = new Models.PaymentPlan();
                    policy.PaymentPlan.Id = businessCollectionPayer.Cast<ISSEN.EndorsementPayer>().FirstOrDefault().PaymentScheduleId;

                    //Obtener Cuotas 
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.PayerPayment.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(policy.Id);
                    BusinessCollection businessCollectionPayment = DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PayerPayment), filter.GetPredicate());
                    policy.PaymentPlan.Quotas = ModelAssembler.CreateTemporalQuotas(businessCollectionPayment);

                    //Obtener Clausulas
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.PolicyClause.Properties.PolicyId, typeof(ISSEN.PolicyClause).Name);
                    filter.Equal();
                    filter.Constant(policy.Id);
                    filter.And();
                    filter.Property(ISSEN.PolicyClause.Properties.EndorsementId, typeof(ISSEN.PolicyClause).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);

                    IList iListClause = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PolicyClause), filter.GetPredicate());
                    List<ISSEN.PolicyClause> policyClause = iListClause.Cast<ISSEN.PolicyClause>().ToList();
                    policy.Clauses = new List<Models.Clause>();
                    for (int i = 0; i < policyClause.Count; i++)
                    {
                        policy.Clauses.Add(new Models.Clause { Id = policyClause[i].ClauseId });
                    }
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetSummaryByEndorsementId");

            return policy;
        }

        /// <summary>
        /// Listado de Temporales Por Filtro
        /// </summary>
        /// <param name="policy">Filtro</param>
        /// <returns>Temporales</returns>
        public List<Models.Policy> GetTemporalPoliciesByPolicy(Models.Policy policy)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<int> operationIds = new List<int>();
            List<Models.Policy> policies = new List<Models.Policy>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(TMPEN.TempSubscription.Properties.OperationId, "t");
            filter.IsNotNull();
            filter.And();
            filter.Property(TMPEN.TempSubscription.Properties.TemporalTypeCode, "t");
            filter.Equal();
            filter.Constant(policy.TemporalType);

            if (policy.Id > 0)
            {
                filter.And();
                filter.Property(TMPEN.TempSubscription.Properties.OperationId, "t");
                filter.Equal();
                filter.Constant(policy.Id);
            }

            if (policy.Branch != null && policy.Branch.Id > 0)
            {
                filter.And();
                filter.Property(TMPEN.TempSubscription.Properties.BranchCode, "t");
                filter.Equal();
                filter.Constant(policy.Branch.Id);
            }

            if (policy.Prefix != null && policy.Prefix.Id > 0)
            {
                filter.And();
                filter.Property(TMPEN.TempSubscription.Properties.PrefixCode, "t");
                filter.Equal();
                filter.Constant(policy.Prefix.Id);
            }

            if (policy.Holder != null && policy.Holder.IndividualId > 0)
            {
                filter.And();
                filter.Property(TMPEN.TempSubscription.Properties.PolicyHolderId, "t");
                filter.Equal();
                filter.Constant(policy.Holder.IndividualId);
            }

            if (policy.UserId > 0)
            {
                filter.And();
                filter.Property(TMPEN.TempSubscription.Properties.UserId, "t");
                filter.Equal();
                filter.Constant(policy.UserId);
            }

            if (policy.CurrentFrom > DateTime.MinValue)
            {
                filter.And();
                filter.Property(TMPEN.TempSubscription.Properties.IssueDate, "t");
                filter.GreaterEqual();
                filter.Constant(policy.CurrentFrom);
            }

            if (policy.CurrentTo > DateTime.MinValue)
            {
                filter.And();
                filter.Property(TMPEN.TempSubscription.Properties.IssueDate, "t");
                filter.LessEqual();
                filter.Constant(policy.CurrentTo);
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = 20;

            selectQuery.AddSelectValue(new SelectValue(new Column(TMPEN.TempSubscription.Properties.OperationId, "t")));

            selectQuery.Table = new ClassNameTable(typeof(TMPEN.TempSubscription), "t");
            selectQuery.AddSortValue(new SortValue(new Column(TMPEN.TempSubscription.Properties.IssueDate, "t"), SortOrderType.Descending));
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    operationIds.Add(Convert.ToInt32(reader["OperationId"]));
                }
            }

            foreach (int operationId in operationIds)
            {
                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                if (pendingOperation != null)
                {
                    policy = new Models.Policy();
                    policy = CORUT.JsonHelper.DeserializeJson<Models.Policy>(pendingOperation.Operation);
                    policy.Id = operationId;
                    policies.Add(policy);
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetTemporalPoliciesByPolicy");

            return policies;
        }

        /// <summary>
        /// Gets the current status policy by endorsement identifier is current.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public Policy GetCurrentStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            //BusinessCollection businessCollection = null;
            ISSEN.Endorsement endorsementEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementEntity = daf.List(typeof(ISSEN.Endorsement), filter.GetPredicate()).Cast<ISSEN.Endorsement>().FirstOrDefault();
            }
            if (endorsementEntity != null)
            {
                //List<Task> taskEntity = new List<Task>();
                var taskEntity = new List<Task>();
                ISSEN.Policy entityPolicy = null;
                PrimaryKey primaryKey = ISSEN.Policy.CreatePrimaryKey(endorsementEntity.PolicyId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityPolicy = (ISSEN.Policy)daf.GetObjectByPrimaryKey(primaryKey);
                }

                primaryKey = COMMEN.Currency.CreatePrimaryKey(entityPolicy.CurrencyCode);
                COMMEN.Currency entityCurrency = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityCurrency = (COMMEN.Currency)daf.GetObjectByPrimaryKey(primaryKey);
                }

                Policy policy = new Policy
                {
                    Id = 0,
                    DocumentNumber = entityPolicy.DocumentNumber,
                    Prefix = new Prefix()
                    {
                        Id = entityPolicy.PrefixCode,
                        Description = DelegateService.commonServiceCore.GetPrefixById(entityPolicy.PrefixCode).Description
                    },
                    Branch = new Branch()
                    {
                        Id = entityPolicy.BranchCode,
                        Description = DelegateService.commonServiceCore.GetBranchById(entityPolicy.BranchCode).Description
                    },
                    IssueDate = endorsementEntity.IssueDate,
                    BusinessType = (BusinessType)entityPolicy.BusinessTypeCode,
                    ExchangeRate = new ExchangeRate
                    {
                        Currency = new Currency
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

                //Tipo de poliza

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
                    COMMEN.CoPolicyType entityCoPolicyType = null;
                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            PrimaryKey pkEntity = COMMEN.CoPolicyType.CreatePrimaryKey(entityPolicy.PrefixCode, entityCoPolicy.PolicyTypeCode);
                            entityCoPolicyType = (COMMEN.CoPolicyType)daf.GetObjectByPrimaryKey(pkEntity);
                            policy.PolicyType = new PolicyType()
                            {
                                Description = entityCoPolicyType.Description,
                                IsFloating = entityCoPolicyType.Floating
                            };
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
                        policy.Product = DelegateService.productServiceCore.GetProductById(entityPolicy.ProductId.Value);
                        DataFacadeManager.Dispose();
                    }));
                Task.WaitAll(taskEntity.ToArray());
                EndorsementDAO endorsementDAO = new EndorsementDAO();
                List<Endorsement> endorsements = new List<Endorsement>();
                if (!isCurrent)
                {
                    endorsements = endorsementDAO.GetEffectiveEndorsementsByPolicyId(entityPolicy.PolicyId);
                    if (endorsements != null && endorsements.Any())
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
                                policy.CurrentFrom = endorsements.FirstOrDefault().CurrentFrom;
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
                        CurrentTo = endorsementEntity.CurrentTo.Value
                    });

                    policy.CurrentTo = endorsements.First(x => x.Id == endorsementId).CurrentTo;
                }

                if (endorsements.Exists(x => x.IsCurrent))
                {
                    if (!isCurrent && endorsements.First(x => x.IsCurrent).EndorsementType != EndorsementType.Cancellation
                        && endorsements.First(x => x.IsCurrent).EndorsementType != EndorsementType.Nominative_cancellation)
                    {
                        filter = new ObjectCriteriaBuilder();
                        filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
                        filter.In();
                        filter.ListValue();
                        filter.Constant(EnumsUnCo.ComponentType.Premium);
                        filter.Constant(EnumsUnCo.ComponentType.Expenses);
                        filter.Constant(EnumsUnCo.ComponentType.Taxes);
                        filter.EndList();
                        List<QUOEN.Component> components = null;
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            components = daf.List(typeof(QUOEN.Component), filter.GetPredicate()).Cast<QUOEN.Component>().ToList();
                        }
                        if (components != null && components.Any())
                        {
                            var premium = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Premium).ComponentCode;
                            var Expenses = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Expenses).ComponentCode;
                            var taxes = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Taxes).ComponentCode;
                            //Pagadores
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
                        filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
                        filter.In();
                        filter.ListValue();
                        filter.Constant(EnumsUnCo.ComponentType.Premium);
                        filter.Constant(EnumsUnCo.ComponentType.Expenses);
                        filter.Constant(EnumsUnCo.ComponentType.Taxes);
                        filter.EndList();

                        List<QUOEN.Component> components = null;
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            components = daf.List(typeof(QUOEN.Component), filter.GetPredicate()).Cast<QUOEN.Component>().ToList();
                        }
                        if (components != null && components.Any())
                        {
                            var premium = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Premium).ComponentCode;
                            var Expenses = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Expenses).ComponentCode;
                            var taxes = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Taxes).ComponentCode;
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
                    agency.IsPrincipal = entityPolicyAgent.IsPrimary;
                    policy.Agencies = new List<IssuanceAgency>();
                    policy.Agencies.Add(agency);
                }
                filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(endorsementId);

                CoverageDAO coverageDAO = new CoverageDAO();
                policy.Summary = coverageDAO.GetCoveragesEndorsementSummary(filter, policy.Summary, null);
                return policy;
            }
            else
            {
                throw new Exception(Errors.ErrorEndorsementNotFound);
            }
        }

        public Models.Summary CalculateSummary(Models.Policy policy, List<Sistran.Core.Application.UnderwritingServices.Models.Risk> risks)
        {
            policy.Summary = new Models.Summary();
            if ((PrefixRc)policy.Prefix.Id == PrefixRc.Liability && policy.Prefix != null)
            {
                if (policy.Endorsement.EndorsementType == EndorsementType.Cancellation || policy.Endorsement.EndorsementType == EndorsementType.Nominative_cancellation)
                {
                    policy.Summary.RiskCount = risks.Where(x => x.Status == RiskStatusType.Excluded).Count();
                    policy.Summary.AmountInsured = risks.Where(a => a != null).SelectMany(x => x.Coverages).Where(x => x.IsPrimary).Sum(x => x.EndorsementLimitAmount);

                }
                else
                {
                    policy.Summary.RiskCount = risks.Where(x => x.Status != RiskStatusType.Excluded).Count();
                    if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal || policy.Endorsement.EndorsementType == EndorsementType.ChangeTermEndorsement || policy.Endorsement.EndorsementType == EndorsementType.ChangeAgentEndorsement)
                    {
                        policy.Summary.AmountInsured = risks.Where(a => a != null && a.Status != RiskStatusType.Excluded).SelectMany(x => x.Coverages).Where(x => x.IsPrimary).Sum(x => x.LimitAmount);
                    }
                    else
                    {
                        policy.Summary.AmountInsured = risks.Where(a => a != null).SelectMany(x => x.Coverages).Where(x => x.IsPrimary).Sum(x => x.EndorsementLimitAmount);
                    }

                }

            }
            else
            {
                if (policy.Endorsement.EndorsementType == EndorsementType.Cancellation || policy.Endorsement.EndorsementType == EndorsementType.Nominative_cancellation)
                {
                    policy.Summary.RiskCount = risks.Where(x => x.Status == RiskStatusType.Excluded).Count();
                    policy.Summary.AmountInsured = risks.Where(a => a != null).SelectMany(x => x.Coverages).Sum(x => x.EndorsementLimitAmount);

                }
                else
                {
                    policy.Summary.RiskCount = risks.Where(x => x.Status != RiskStatusType.Excluded).Count();
                    if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal || policy.Endorsement.EndorsementType == EndorsementType.ChangeTermEndorsement || policy.Endorsement.EndorsementType == EndorsementType.ChangeAgentEndorsement)
                    {
                        policy.Summary.AmountInsured = risks.Where(a => a != null && a.Status != RiskStatusType.Excluded).SelectMany(x => x.Coverages).Sum(x => x.LimitAmount);
                    }
                    else
                    {
                        policy.Summary.AmountInsured = risks.Where(a => a != null).SelectMany(x => x.Coverages).Sum(x => x.EndorsementLimitAmount);
                    }

                }
            }

            policy.Summary.CoveredRiskType = policy.Product.CoveredRisk.CoveredRiskType;

            foreach (Models.PayerComponent payer in policy.PayerComponents)
            {
                switch (payer.Component.ComponentType)
                {
                    case EnumsUnCo.ComponentType.Premium:
                        policy.Summary.Premium += payer.Amount;
                        break;
                    case EnumsUnCo.ComponentType.Expenses:
                        if (payer.AmountExpense > 0)
                        {
                            policy.Summary.Expenses += Math.Round(payer.AmountExpense, 2);
                        }
                        else
                        {
                            policy.Summary.Expenses += payer.Amount;
                        }
                        policy.Summary.ExpensesLocal += payer.AmountLocal;
                        break;
                    case EnumsUnCo.ComponentType.Taxes:
                        policy.Summary.Taxes += payer.Amount;
                        break;
                    default:
                        break;
                }
            }

            policy.Summary.FullPremium = policy.Summary.Premium + policy.Summary.Expenses + policy.Summary.Taxes;
            return policy.Summary;
        }

        public async Task<List<IssuanceAgency>> GetAgentsByPolicyIdEndorsementId(int? policyId, int? endorsementId)
        {
            List<Task> taskAgent = new List<Task>();
            ConcurrentBag<IssuanceAgency> Agencies = new ConcurrentBag<IssuanceAgency>();
            //Obtener intermediario          
            List<ISSEN.PolicyAgent> policyAgents = null;
            taskAgent.Add(TP.Task.Run(() =>
            {
                ObjectCriteriaBuilder filterAgent = new ObjectCriteriaBuilder();
                filterAgent.Property(ISSEN.PolicyAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
                filterAgent.Equal();
                filterAgent.Constant(endorsementId);
                filterAgent.And();
                filterAgent.Property(ISSEN.PolicyAgent.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
                filterAgent.Equal();
                filterAgent.Constant(policyId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    policyAgents = daf.List(typeof(ISSEN.PolicyAgent), filterAgent.GetPredicate()).Cast<ISSEN.PolicyAgent>().OrderByDescending(b => b.IsPrimary).ToList();
                }
            }));

            //Obtiene Comisiones
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.CommissAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.CommissAgent.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
            filter.Equal();
            filter.Constant(policyId);
            var commissAgent = await GetCommisAgentsByFiler(filter);

            var policy = TP.Task.Run(() =>
             {
                 ObjectCriteriaBuilder filterPolicy = new ObjectCriteriaBuilder();
                 filterPolicy.Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name);
                 filterPolicy.Equal();
                 filterPolicy.Constant(policyId);
                 ISSEN.Policy policyEntity = null;
                 using (var daf = DataFacadeManager.Instance.GetDataFacade())
                 {
                     policyEntity = daf.List(typeof(ISSEN.Policy), filterPolicy.GetPredicate()).Cast<ISSEN.Policy>().FirstOrDefault();
                 }
                 DataFacadeManager.Dispose();
                 return Convert.ToInt16(policyEntity.PrefixCode);

             });
            Task.WaitAll(taskAgent.ToArray());

            ConcurrentBag<String> errros = new ConcurrentBag<String>();
            TP.Parallel.ForEach(policyAgents, policyAgent =>
            {
                if (policyAgent != null)
                {
                    decimal? AdditionalPercentage = commissAgent.Where(b => b.IndividualId == policyAgent.IndividualId).FirstOrDefault().AdditCommissPercentage;
                    IssuanceAgency agency = new IssuanceAgency();
                    agency.Agent = new IssuanceAgent();

                    int subLineId = (int)commissAgent.Where(b => b.AgentAgencyId == policyAgent.AgentAgencyId && b.IndividualId == policyAgent.IndividualId).FirstOrDefault().SubLineBusinessCode;
                    int lineId = (int)commissAgent.Where(b => b.AgentAgencyId == policyAgent.AgentAgencyId && b.IndividualId == policyAgent.IndividualId).FirstOrDefault().LineBusinessCode;
                    var subLine = DelegateService.commonServiceCore.GetSubLineBusinessById(subLineId, lineId);
                    subLine.LineBusiness = DelegateService.commonServiceCore.GetLineBusinessById("", lineId);

                    AgencyDAO agencyDAO = new AgencyDAO();
                    IssuanceAgency issuanceAgency = agencyDAO.GetAgencyByAgentIdAgentAgencyId(policyAgent.IndividualId, policyAgent.AgentAgencyId);

                    issuanceAgency.Agent.IndividualId = policyAgent.IndividualId;
                    issuanceAgency.Participation = commissAgent.Where(b => b.AgentAgencyId == policyAgent.AgentAgencyId && b.IndividualId == policyAgent.IndividualId).FirstOrDefault().AgentPartPercentage;
                    issuanceAgency.Commissions = new List<IssuanceCommission>();
                    issuanceAgency.Commissions.Add(new IssuanceCommission
                    {
                        AgentPercentage = commissAgent.Where(b => b.AgentAgencyId == policyAgent.AgentAgencyId && b.IndividualId == policyAgent.IndividualId).FirstOrDefault().StAgentComissionPercentage,
                        Percentage = commissAgent.Where(b => b.AgentAgencyId == policyAgent.AgentAgencyId && b.IndividualId == policyAgent.IndividualId).FirstOrDefault().StComissionPercentage,
                        SubLineBusiness = subLine
                    });
                    issuanceAgency.AgentType = new IssuanceAgentType()
                    {
                        Id = issuanceAgency.Agent.AgentType.Id
                    };
                    issuanceAgency.Id = policyAgent.AgentAgencyId;

                    if (policyAgent.SalesPointCode.HasValue)
                    {
                        ObjectCriteriaBuilder filterSalePoint = new ObjectCriteriaBuilder();
                        filterSalePoint.Property(COMMEN.SalePoint.Properties.SalePointCode, typeof(COMMEN.SalePoint).Name);
                        filterSalePoint.Equal();
                        filterSalePoint.Constant(policyAgent.SalesPointCode);
                        COMMEN.SalePoint SalePointEntity = null;

                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            SalePointEntity = daf.List(typeof(COMMEN.SalePoint), filterSalePoint.GetPredicate()).Cast<COMMEN.SalePoint>().FirstOrDefault();
                        }

                        DataFacadeManager.Dispose();

                        ObjectCriteriaBuilder filterBranch = new ObjectCriteriaBuilder();
                        filterBranch.Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name);
                        filterBranch.Equal();
                        filterBranch.Constant(SalePointEntity.BranchCode);
                        COMMEN.Branch BranchEntity = null;

                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            BranchEntity = daf.List(typeof(COMMEN.Branch), filterBranch.GetPredicate()).Cast<COMMEN.Branch>().FirstOrDefault();
                        }

                        DataFacadeManager.Dispose();

                        issuanceAgency.Branch = new Branch();
                        issuanceAgency.Branch.Id = (int)SalePointEntity.BranchCode;
                        issuanceAgency.Branch.Description = BranchEntity.Description;
                        issuanceAgency.Branch.SmallDescription = BranchEntity.SmallDescription;
                        issuanceAgency.Branch.SalePoints = new List<SalePoint>();
                        issuanceAgency.Branch.SalePoints.Add(new SalePoint() { Id = (int)policyAgent.SalesPointCode, Description = SalePointEntity.Description, SmallDescription = SalePointEntity.SmallDescription });

                    }
                    else
                    {
                        //Obtener datos de la poliza
                        PrimaryKey keyPolicy = ISSEN.Policy.CreatePrimaryKey(policy.Id);
                        ISSEN.Policy policyEntity = (ISSEN.Policy)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyPolicy);

                        if (policyEntity != null)
                        {
                            ObjectCriteriaBuilder filterBranch = new ObjectCriteriaBuilder();
                            filterBranch.Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name);
                            filterBranch.Equal();
                            filterBranch.Constant(policyEntity.BranchCode);
                            COMMEN.Branch BranchEntity = null;

                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                BranchEntity = daf.List(typeof(COMMEN.Branch), filterBranch.GetPredicate()).Cast<COMMEN.Branch>().FirstOrDefault();
                            }

                            DataFacadeManager.Dispose();

                            ObjectCriteriaBuilder filterSalePoint = new ObjectCriteriaBuilder();
                            filterSalePoint.Property(COMMEN.SalePoint.Properties.SalePointCode, typeof(COMMEN.SalePoint).Name);
                            filterSalePoint.Equal();
                            filterSalePoint.Constant(policyEntity.SalePointCode);
                            COMMEN.SalePoint SalePointEntity = null;

                            using (var daf = DataFacadeManager.Instance.GetDataFacade())
                            {
                                SalePointEntity = daf.List(typeof(COMMEN.SalePoint), filterSalePoint.GetPredicate()).Cast<COMMEN.SalePoint>().FirstOrDefault();
                            }

                            DataFacadeManager.Dispose();

                            issuanceAgency.Branch = new Branch();
                            issuanceAgency.Branch.Id = (int)policyEntity.BranchCode;
                            issuanceAgency.Branch.Description = BranchEntity.Description;
                            issuanceAgency.Branch.SmallDescription = BranchEntity.SmallDescription;
                            issuanceAgency.Branch.SalePoints = new List<SalePoint>();
                            issuanceAgency.Branch.SalePoints.Add(new SalePoint() { Id = (int)policyEntity.SalePointCode, Description = SalePointEntity.Description, SmallDescription = SalePointEntity.SmallDescription });

                        }
                    }

                    Agencies.Add(issuanceAgency);
                }
                else
                {
                    errros.Add("Error Obteniendo Agente de la poliza");
                }

            });
            Agencies.Where(x => policyAgents.Where(a => a.IsPrimary == true).Select(z => new
            {
                z.IndividualId,
                z.AgentAgencyId
            }).Contains(new
            {
                x.Agent.IndividualId,
                AgentAgencyId = x.Id
            })).AsParallel().ForAll(
                m =>
                {
                    m.IsPrincipal = true;
                }
                );

            return Agencies.ToList();

        }
        private async Task<List<ISSEN.CommissAgent>> GetCommisAgentsByFiler(ObjectCriteriaBuilder filter)
        {
            var result = await TP.Task.Run(() =>
             {
                 List<ISSEN.CommissAgent> commissAgent = null;
                 using (var daf = DataFacadeManager.Instance.GetDataFacade())
                 {
                     commissAgent = daf.List(typeof(ISSEN.CommissAgent), filter.GetPredicate()).Cast<ISSEN.CommissAgent>().ToList();
                 }
                 return commissAgent;
             });
            return result;
        }

        public List<Models.Policy> GetPoliciesByPolicy(Models.Policy policy)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<int> operationIds = new List<int>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            bool emptyFilter = true;
            if (policy.Branch != null && policy.Branch.Id > 0)
            {
                filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policy.Branch.Id);
                emptyFilter = false;
            }
            if (policy.Prefix != null && policy.Prefix.Id > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policy.Prefix.Id);
                emptyFilter = false;
            }
            if (policy.DocumentNumber > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policy.DocumentNumber);
                emptyFilter = false;
            }
            if (policy.Holder != null && policy.Holder.IndividualId > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.PolicyholderId, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policy.Holder.IndividualId);
                emptyFilter = false;
            }

            if (policy.UserId > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Endorsement.Properties.UserId, typeof(ISSEN.Endorsement).Name);
                filter.Equal();
                filter.Constant(policy.UserId);
                emptyFilter = false;
            }

            if (policy.CurrentFrom > DateTime.MinValue)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.CurrentFrom, typeof(ISSEN.Policy).Name);
                filter.GreaterEqual();
                filter.Constant(policy.CurrentFrom);
                emptyFilter = false;
            }

            if (policy.CurrentTo > DateTime.MinValue)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(ISSEN.Policy.Properties.CurrentFrom, typeof(ISSEN.Policy).Name);
                filter.LessEqual();
                filter.Constant(policy.CurrentTo);
            }
            EndorsementView view = new EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view.Policies.Count > 0)
            {
                List<Models.Policy> policies = new List<Models.Policy>();
                var endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>(), x => x.Endorsement.EndorsementId, y => y.EndorsementId, (dc, d) => new { EndorsementRisk = d }).Where(x => x.EndorsementRisk.IsCurrent == true).ToList();
                foreach (var item in view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(endorsements, z => z.Endorsement.PolicyId, w => w.EndorsementRisk.PolicyId, (u, n) => new { Endorsement = u, EndorSementRisk = n }).Select(b => new { Endorsement = b.Endorsement.Endorsement, Policy = b.Endorsement.Policy }).Distinct())
                {
                    Models.Policy currentPolicy = new Models.Policy
                    {
                        Id = item.Policy.PolicyId,
                        IssueDate = item.Policy.IssueDate,
                        CurrentFrom = item.Policy.CurrentFrom,
                        CurrentTo = item.Policy.CurrentTo ?? DateTime.Now,
                        DocumentNumber = item.Policy.DocumentNumber,
                        Endorsement = new Models.Endorsement
                        {
                            Id = item.Endorsement.EndorsementId,
                            CurrentFrom = item.Endorsement.CurrentFrom,
                            CurrentTo = item.Endorsement.CurrentTo.Value,
                            Description = item.Endorsement.DocumentNum.ToString(),
                            PolicyId = item.Endorsement.PolicyId,
                            EndorsementType = (Enums.EndorsementType)item.Endorsement.EndoTypeCode,
                            IsCurrent = view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault() == null ? default(bool) : ((ISSEN.EndorsementRisk)view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault()).IsCurrent
                        }

                    };
                    currentPolicy.Product = DelegateService.productServiceCore.GetProductByProductIdPrefixId((int)item.Policy.ProductId, item.Policy.PrefixCode);
                    HolderDAO holderDAO = new HolderDAO();
                    List<Models.Holder> holders = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(item.Policy.PolicyholderId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    if (holders.Count > 0)
                    {
                        currentPolicy.Holder = holders[0];
                    }

                    //Obtener intermediario
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.PolicyAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(item.Endorsement.EndorsementId);
                    filter.And();
                    filter.Property(ISSEN.PolicyAgent.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name);
                    filter.Equal();
                    filter.Constant(currentPolicy.Id);
                    IList iListAgent = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PolicyAgent), filter.GetPredicate());
                    List<ISSEN.PolicyAgent> policyAgent = iListAgent.Cast<ISSEN.PolicyAgent>().OrderByDescending(b => b.IsPrimary).ToList();
                    currentPolicy.Agencies = new List<IssuanceAgency>();

                    for (int i = 0; i < policyAgent.Count; i++)
                    {
                        AgencyDAO agencyDAO = new AgencyDAO();
                        IssuanceAgency issuanceAgency = agencyDAO.GetAgencyByAgentIdAgentAgencyId(policyAgent[i].IndividualId, policyAgent[i].AgentAgencyId);
                        currentPolicy.Agencies.Add(issuanceAgency);
                    }

                    //Resumen de la prima
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
                    filter.In();
                    filter.ListValue();
                    filter.Constant(Enums.ComponentType.Premium);
                    filter.Constant(Enums.ComponentType.Expenses);
                    filter.Constant(Enums.ComponentType.Taxes);
                    filter.EndList();
                    IList components = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.Component), filter.GetPredicate());

                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
                    filter.Equal();
                    filter.Constant(item.Endorsement.EndorsementId);
                    IList iList = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PayerComp), filter.GetPredicate());
                    List<ISSEN.PayerComp> payerComponents = iList.Cast<ISSEN.PayerComp>().ToList();

                    if (payerComponents != null)
                    {
                        currentPolicy.Summary = new Models.Summary();

                        foreach (QUOEN.Component itemCom in components)
                        {
                            switch ((Enums.ComponentType)itemCom.ComponentTypeCode)
                            {
                                case Enums.ComponentType.Premium:
                                    currentPolicy.Summary.AmountInsured += payerComponents.Where(x => x.ComponentCode == itemCom.ComponentCode).Sum(y => y.CalcBaseAmount);
                                    currentPolicy.Summary.Premium += payerComponents.Where(x => x.ComponentCode == itemCom.ComponentCode).Sum(y => y.ComponentAmount);
                                    break;
                                case Enums.ComponentType.Expenses:
                                    currentPolicy.Summary.Expenses += payerComponents.Where(x => x.ComponentCode == itemCom.ComponentCode).Sum(y => y.ComponentAmount);
                                    break;
                                case Enums.ComponentType.Taxes:
                                    currentPolicy.Summary.Taxes += payerComponents.Where(x => x.ComponentCode == itemCom.ComponentCode).Sum(y => y.ComponentAmount);
                                    break;
                                default:
                                    break;
                            }
                        }

                        currentPolicy.Summary.FullPremium = currentPolicy.Summary.Premium + currentPolicy.Summary.Expenses + currentPolicy.Summary.Taxes;
                    }

                    currentPolicy.Branch = DelegateService.commonServiceCore.GetBranchById(item.Policy.BranchCode);
                    currentPolicy.Prefix = DelegateService.commonServiceCore.GetPrefixById(item.Policy.PrefixCode);

                    policies.Add(currentPolicy);


                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetPoliciesByPolicy");
                return policies;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetPoliciesByPolicy");
                return null;
            }
        }
        /// <summary>
        /// Obtener Polizas de N Polizas
        /// </summary>
        /// <param name="policies">The policies.</param>
        /// <returns>
        /// Lista de Policy
        /// </returns>
        public List<Models.Policy> GetPoliciesByPolicies(List<Policy> policies)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            object lockobject = new object();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            for (int i = 0; i < policies.Count; i++)
            {
                filter.OpenParenthesis();
                filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policies[i].Prefix.Id);
                filter.And();
                filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policies[i].Branch.Id);
                filter.And();
                filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(policies[i].DocumentNumber);
                filter.CloseParenthesis();
                if (i < policies.Count - 1)
                {
                    filter.Or();
                }
            }
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            EndorsementView view = new EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Policies.Count > 0)
            {
                List<Models.Policy> policiesModel = new List<Models.Policy>();
                TP.Parallel.ForEach(view.Endorsements, (endorsement) =>
                {
                    var item = (ISSEN.Endorsement)endorsement;
                    var policy = new Policy
                    {
                        Id = 0,
                        Endorsement = new Models.Endorsement
                        {
                            Id = item.EndorsementId,
                            CurrentFrom = item.CurrentFrom,
                            CurrentTo = item.CurrentTo.Value,
                            EndorsementType = (Enums.EndorsementType)item.EndoTypeCode,
                            IsCurrent = true
                        }
                    };
                    lock (lockobject)
                    {
                        policiesModel.Add(policy);
                    }
                });
                return policiesModel;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetEndorsementsByPrefixIdBranchIdPolicyNumber");
                return null;
            }
        }

        #region Underwriting en el Administrativo Contable
        /// <summary>
        /// Obtener todas las polizas por asegurado
        /// </summary>
        /// <param name="insuredId">Identificador del asegurado</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error in GetPoliciesByInsuredId</exception>
        public List<Models.Policy> GetPoliciesByInsuredId(int insuredId)
        {

            EndorsementRiskView view = new EndorsementRiskView();
            ViewBuilder builder = new ViewBuilder("EndorsementRiskView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view.Policies.Count > 0)
            {
                List<Models.Policy> Policies = new List<Models.Policy>();
                var endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>(), x => x.Endorsement.EndorsementId, y => y.EndorsementId, (dc, d) => new { EndorsementRisk = d }).Where(x => x.EndorsementRisk.IsCurrent == true).ToList();
                foreach (var item in view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(endorsements, z => z.Endorsement.PolicyId, w => w.EndorsementRisk.PolicyId, (u, n) => new { Endorsement = u, EndorSementRisk = n }).Select(b => new { Endorsement = b.Endorsement.Endorsement, Policy = b.Endorsement.Policy, EndorsementRisk = b.EndorSementRisk.EndorsementRisk }).Distinct())
                {

                    Policies.Add(
                        new Models.Policy
                        {
                            Id = item.Policy.PolicyId,
                            IssueDate = item.Policy.IssueDate,
                            CurrentFrom = item.Policy.CurrentFrom,
                            CurrentTo = item.Policy.CurrentTo ?? DateTime.Now,
                            DocumentNumber = item.Policy.DocumentNumber,
                            Endorsement = new Models.Endorsement
                            {
                                Id = item.Endorsement.EndorsementId,
                                CurrentFrom = item.Endorsement.CurrentFrom,
                                CurrentTo = item.Endorsement.CurrentTo.Value,
                                Description = item.Endorsement.DocumentNum.ToString(),
                                PolicyId = item.Endorsement.PolicyId,
                                EndorsementType = (Enums.EndorsementType)item.Endorsement.EndoTypeCode,
                                IsCurrent = item.EndorsementRisk.IsCurrent
                            }

                        });
                }
                return Policies;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Obtener polizas
        /// </summary>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <param name="branchId">Id Sucursal</param>
        /// <param name="policyNumber">Número de Póliza</param>
        /// <param name="CurrentDate">Fecha busqueda</param>
        /// <returns>Polizas</returns>
        public List<Models.Policy> GetPoliciesByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, DateTime CurrentDate)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
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
            filter.And();
            filter.Property(ISSEN.Policy.Properties.CurrentTo, typeof(ISSEN.Policy).Name);
            filter.GreaterEqual();
            filter.Constant(CurrentDate);
            EndorsementView view = new EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view.Policies.Count > 0)
            {
                List<Models.Policy> policies = new List<Models.Policy>();
                var endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>(), x => x.Endorsement.EndorsementId, y => y.EndorsementId, (dc, d) => new { EndorsementRisk = d }).Where(x => x.EndorsementRisk.IsCurrent == true).ToList();
                foreach (var item in view.Endorsements.Cast<ISSEN.Endorsement>().Join(view.Policies.Cast<ISSEN.Policy>(), x => x.PolicyId, y => y.PolicyId, (dc, d) => new { Endorsement = dc, Policy = d }).Join(endorsements, z => z.Endorsement.PolicyId, w => w.EndorsementRisk.PolicyId, (u, n) => new { Endorsement = u, EndorSementRisk = n }).Select(b => new { Endorsement = b.Endorsement.Endorsement, Policy = b.Endorsement.Policy }).Distinct())
                {

                    policies.Add(
                       new Models.Policy
                       {
                           Id = item.Policy.PolicyId,
                           IssueDate = item.Policy.IssueDate,
                           CurrentFrom = item.Policy.CurrentFrom,
                           CurrentTo = item.Policy.CurrentTo ?? DateTime.Now,
                           DocumentNumber = item.Policy.DocumentNumber,
                           Endorsement = new Models.Endorsement
                           {
                               Id = item.Endorsement.EndorsementId,
                               CurrentFrom = item.Endorsement.CurrentFrom,
                               CurrentTo = item.Endorsement.CurrentTo.Value,
                               Description = item.Endorsement.DocumentNum.ToString(),
                               PolicyId = item.Endorsement.PolicyId,
                               EndorsementType = (Enums.EndorsementType)item.Endorsement.EndoTypeCode,
                               IsCurrent = view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault() == null ? default(bool) : ((ISSEN.EndorsementRisk)view.EndorsementRisks.Where(y => ((ISSEN.EndorsementRisk)y).PolicyId == item.Endorsement.PolicyId && ((ISSEN.EndorsementRisk)y).IsCurrent == true).FirstOrDefault()).IsCurrent
                           }

                       });
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetPoliciesByPrefixIdBranchIdPolicyNumber");
                return policies;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetPoliciesByPrefixIdBranchIdPolicyNumber");
                return null;
            }

        }

        /// <summary>
        ///Obtiene el Identificador de la poliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="branchId">Identificador Sucursal</param>
        /// <param name="policyNumber">Numero Documento Poliza.</param>
        /// <returns></returns>
        public int GetPolicyIdByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
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
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            EndorsementView view = new EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view.Policies.Count > 0)
            {
                return view.Policies.Max(x => ((ISSEN.Policy)x).PolicyId);
            }
            return 0;
        }

        public Policy GetPolicyByPolicyId(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(policyId);
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(ISSEN.Policy), filter.GetPredicate());
            ISSEN.Policy policyEnt = businessCollection.Cast<ISSEN.Policy>().FirstOrDefault();
            Policy policyModel = new Policy
            {
                Id = policyEnt.PolicyId,
                Branch = new Branch
                {
                    Id = policyEnt.BranchCode
                },
                DocumentNumber = policyEnt.DocumentNumber,
                Prefix = new Prefix
                {
                    Id = policyEnt.PrefixCode
                }
            };
            return policyModel;
        }

        #endregion

        public Policy GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
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
            //Nunca debería llegar nulo pero se deja por si se llega a modificar el sp
            if (resultTable == null || resultTable.Rows.Count == 0)
            {
                return null;
            }
            var result = resultTable.Rows[0];
            var currentTo = result["CURRENT_TO"];
            var policy = new Policy
            {
                Id = 0,
                DocumentNumber = policyNumber,
                CurrentFrom = (DateTime)result["CURRENT_FROM"],
                Prefix = new Prefix
                {
                    Id = prefixId > 0 ? prefixId : Convert.ToInt32(result["PREFIX_CD"])
                },
                Branch = new Branch
                {
                    Id = branchId > 0 ? branchId : Convert.ToInt32(result["BRANCH_CD"])
                },
                Product = new PRODModel.Product
                {
                    Id = (int)result["PRODUCT_ID"]
                },
                Endorsement = new Endorsement
                {
                    Id = (int)result["ENDORSEMENT_ID"],
                    EndorsementType = (EndorsementType)(int)result["ENDORSEMENT_TYPE"],
                    PolicyId = (int)result["POLICY_ID"]
                },
                ExchangeRate = new ExchangeRate
                {
                    Currency = new Currency
                    {
                        Id = (int)result["CURRENCY_ID"],
                    }
                }
            };
            if (currentTo != DBNull.Value)
            {
                policy.CurrentTo = (DateTime)currentTo;
            }
            return policy;
        }

        public SubCoveredRiskType? GetSubcoverRiskTypeByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            var parameters = new NameValue[3];
            parameters[0] = new NameValue("DOCUMENT_NUMBER", policyNumber);
            parameters[1] = new NameValue("PREFIX_ID", prefixId);
            parameters[2] = new NameValue("BRANCH_ID", branchId);
            object result;
            using (var dataAccess = new DynamicDataAccess())
            {
                result = dataAccess.ExecuteSPScalar("ISS.GET_SUB_COVERED_RISK_TYPE", parameters);
            }
            //Nunca debería llegar nulo pero se deja por si se llega a modificar el sp
            if (result == DBNull.Value)
            {
                return null;
            }
            else
            {
                return (SubCoveredRiskType)Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Elimina la informacion del temporal
        /// </summary>
        /// <param name="tempId"> id del Temporal</param>
        /// <returns></returns>
        public static bool DeleteTemporalByTemporalId(int temporalId)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("TEMP_ID", temporalId);

            object result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("TMP.DELETE_TEMP", parameters);
            }

            return true;
        }

        /// <summary>
        /// Elimina toda la información de los temporales (masivos).
        /// </summary>
        /// <param name="operationId">The operation identifier.</param>
        public static void DeleteTemporalsByOperationId(int operationId)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("OPERATION_ID", operationId);

            object result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("TMP.DELETE_TEMP_BY_OPERATION_ID", parameters);
            }
        }

        /// <summary>
        /// Elimina toda la información del temporal
        /// </summary>
        /// <param name="operationId">id operation</param>
        /// <returns>bool</returns>
        public static string DeleteTemporalByOperationId(int operationId, long documentNum, int prefixId, int branchId)
        {
            NameValue[] parameters = new NameValue[4];
            parameters[0] = new NameValue("OPERATION_ID", operationId);
            parameters[1] = new NameValue("DOCUMENT_NUM", documentNum);
            parameters[2] = new NameValue("PREFIX_CD", prefixId);
            parameters[3] = new NameValue("BRANCH_CD", branchId);

            object result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("TMP.DELETE_TEMP_BY_OPERATIONID", parameters);
            }

            return result.ToString();
        }

        /// <summary>
        /// Resumen de la prima
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        public Models.Summary GetSummaryByEndorsementId(int endorsementId)
        {
            Models.Summary summary = new Models.Summary();
            //Resumen de la prima
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(Enums.ComponentType.Premium);
            filter.Constant(Enums.ComponentType.Expenses);
            filter.Constant(Enums.ComponentType.Taxes);
            filter.EndList();
            IList components = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.Component), filter.GetPredicate());

            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            IList iList = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PayerComp), filter.GetPredicate());
            List<ISSEN.PayerComp> payerComponents = iList.Cast<ISSEN.PayerComp>().ToList();

            if (payerComponents != null)
            {

                foreach (QUOEN.Component item in components)
                {
                    switch ((Enums.ComponentType)item.ComponentTypeCode)
                    {
                        case Enums.ComponentType.Premium:
                            summary.Premium += payerComponents.Where(x => x.ComponentCode == item.ComponentCode).Sum(y => y.ComponentAmount);
                            break;
                        case Enums.ComponentType.Expenses:
                            summary.Expenses += payerComponents.Where(x => x.ComponentCode == item.ComponentCode).Sum(y => y.ComponentAmount);
                            break;
                        case Enums.ComponentType.Taxes:
                            summary.Taxes += payerComponents.Where(x => x.ComponentCode == item.ComponentCode).Sum(y => y.ComponentAmount);
                            break;
                        default:
                            break;
                    }
                }

                summary.FullPremium = summary.Premium + summary.Expenses + summary.Taxes;
            }
            return summary;

        }

        /// <summary>
        /// Gets the current status policy by endorsement identifier is current.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public Policy GetStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            //BusinessCollection businessCollection = null;
            ISSEN.Endorsement endorsementEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementEntity = daf.List(typeof(ISSEN.Endorsement), filter.GetPredicate()).Cast<ISSEN.Endorsement>().FirstOrDefault();
            }
            if (endorsementEntity != null)
            {
                //List<Task> taskEntity = new List<Task>();
                var taskEntity = new List<Task>();
                ISSEN.Policy entityPolicy = null;
                PrimaryKey primaryKey = ISSEN.Policy.CreatePrimaryKey(endorsementEntity.PolicyId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityPolicy = (ISSEN.Policy)daf.GetObjectByPrimaryKey(primaryKey);
                }

                primaryKey = COMMEN.Currency.CreatePrimaryKey(entityPolicy.CurrencyCode);
                COMMEN.Currency entityCurrency = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityCurrency = (COMMEN.Currency)daf.GetObjectByPrimaryKey(primaryKey);
                }

                Policy policy = new Policy
                {
                    Id = 0,
                    DocumentNumber = entityPolicy.DocumentNumber,
                    Prefix = new Prefix()
                    {
                        Id = entityPolicy.PrefixCode,
                        Description = DelegateService.commonServiceCore.GetPrefixById(entityPolicy.PrefixCode).Description
                    },
                    Branch = new Branch()
                    {
                        Id = entityPolicy.BranchCode,
                        Description = DelegateService.commonServiceCore.GetBranchById(entityPolicy.BranchCode).Description
                    },
                    IssueDate = endorsementEntity.IssueDate,
                    BusinessType = (BusinessType)entityPolicy.BusinessTypeCode,
                    ExchangeRate = new ExchangeRate
                    {
                        Currency = new Currency
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

                //Tipo de poliza

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
                    COMMEN.CoPolicyType entityCoPolicyType = null;
                    taskEntity.Add(TP.Task.Run(() =>
                    {
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            PrimaryKey pkEntity = COMMEN.CoPolicyType.CreatePrimaryKey(entityPolicy.PrefixCode, entityCoPolicy.PolicyTypeCode);
                            entityCoPolicyType = (COMMEN.CoPolicyType)daf.GetObjectByPrimaryKey(pkEntity);
                            policy.PolicyType = new PolicyType() { Description = entityCoPolicyType.Description };
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
                    policy.Product = DelegateService.productServiceCore.GetProductById(entityPolicy.ProductId.Value);
                    DataFacadeManager.Dispose();
                }));
                Task.WaitAll(taskEntity.ToArray());
                EndorsementDAO endorsementDAO = new EndorsementDAO();
                List<Endorsement> endorsements = new List<Endorsement>();
                if (!isCurrent)
                {
                    endorsements = endorsementDAO.GetEndorsementsByPolicyId(entityPolicy.PolicyId);
                    if (endorsements != null && endorsements.Any())
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
                                policy.CurrentFrom = endorsements.FirstOrDefault().CurrentFrom;
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
                        CurrentTo = endorsementEntity.CurrentTo.Value
                    });

                    policy.CurrentTo = endorsements.First(x => x.Id == endorsementId).CurrentTo;
                }

                if (endorsements.Exists(x => x.IsCurrent))
                {
                    if (!isCurrent && endorsements.First(x => x.IsCurrent).EndorsementType != EndorsementType.Cancellation
                        && endorsements.First(x => x.IsCurrent).EndorsementType != EndorsementType.Nominative_cancellation)
                    {
                        filter = new ObjectCriteriaBuilder();
                        filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
                        filter.In();
                        filter.ListValue();
                        filter.Constant(EnumsUnCo.ComponentType.Premium);
                        filter.Constant(EnumsUnCo.ComponentType.Expenses);
                        filter.Constant(EnumsUnCo.ComponentType.Taxes);
                        filter.EndList();
                        List<QUOEN.Component> components = null;
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            components = daf.List(typeof(QUOEN.Component), filter.GetPredicate()).Cast<QUOEN.Component>().ToList();
                        }
                        if (components != null && components.Any())
                        {
                            var premium = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Premium).ComponentCode;
                            var Expenses = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Expenses).ComponentCode;
                            var taxes = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Taxes).ComponentCode;
                            //Pagadores
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
                        filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
                        filter.In();
                        filter.ListValue();
                        filter.Constant(EnumsUnCo.ComponentType.Premium);
                        filter.Constant(EnumsUnCo.ComponentType.Expenses);
                        filter.Constant(EnumsUnCo.ComponentType.Taxes);
                        filter.EndList();

                        List<QUOEN.Component> components = null;
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            components = daf.List(typeof(QUOEN.Component), filter.GetPredicate()).Cast<QUOEN.Component>().ToList();
                        }
                        if (components != null && components.Any())
                        {
                            var premium = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Premium).ComponentCode;
                            var Expenses = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Expenses).ComponentCode;
                            var taxes = components.FirstOrDefault(x => (EnumsUnCo.ComponentType)x.ComponentTypeCode == EnumsUnCo.ComponentType.Taxes).ComponentCode;
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
                filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(endorsementId);

                CoverageDAO coverageDAO = new CoverageDAO();
                policy.Summary = coverageDAO.GetCoveragesEndorsementSummary(filter, policy.Summary, null);
                return policy;
            }
            else
            {
                throw new Exception(Errors.ErrorEndorsementNotFound);
            }
        }

        internal Summary CalculateSummary(Policy policy, List<Risk> risks, int riskId, int insuredObjectId)
        {
            policy.Summary = new Models.Summary();

            List<Coverage> coverages = new List<Coverage>();
            coverages.AddRange(risks.Where(x => x.RiskId == riskId).FirstOrDefault().Coverages.Where(z => z.InsuredObject.Id == insuredObjectId));
            policy.Summary.RiskCount = risks.Where(x => x.Status != RiskStatusType.Excluded).Count();

            policy.Summary.AmountInsured = coverages.Sum(x => x.EndorsementLimitAmount);

            policy.Summary.CoveredRiskType = policy.Product.CoveredRisk.CoveredRiskType;

            policy.PayerComponents = (from x in policy.PayerComponents
                                      where (from z in coverages select z.Id).Contains(x.CoverageId)
                                      || x.Component.ComponentType == EnumsUnCo.ComponentType.Taxes
                                      select x).ToList();

            foreach (Models.PayerComponent payer in policy.PayerComponents)
            {
                switch (payer.Component.ComponentType)
                {
                    case EnumsUnCo.ComponentType.Premium:
                        policy.Summary.Premium += payer.Amount;
                        break;
                    case EnumsUnCo.ComponentType.Expenses:
                        policy.Summary.Expenses += payer.Amount;
                        policy.Summary.ExpensesLocal += payer.AmountLocal;
                        break;
                    case EnumsUnCo.ComponentType.Taxes:
                        policy.Summary.Taxes += payer.Amount;
                        break;
                    default:
                        break;
                }
            }

            policy.Summary.FullPremium = policy.Summary.Premium + policy.Summary.Expenses + policy.Summary.Taxes;

            return policy.Summary;
        }


        #region Producto Underwriting

        /// <summary>
        /// Validar si el objeto del seguro existe en un producto y si este ya fue usado en un temporal
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>
        public Boolean ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal().Constant(productId);
            filter.And();
            filter.Property(PRODEN.ProductGroupCover.Properties.CoverGroupId, typeof(PRODEN.ProductGroupCover).Name)
           .Equal().Constant(groupCoverageId);
            filter.And();
            filter.Property(PRODEN.GroupInsuredObject.Properties.InsuredObject, typeof(PRODEN.GroupInsuredObject).Name)
            .Equal().Constant(insuredObjectId);

            TemporalInsuredObjectProductView view = new TemporalInsuredObjectProductView();
            ViewBuilder builder = new ViewBuilder("TemporalInsuredObjectProductView");

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.TempSubscriptionList.Count > 0)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId");

                return true;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId");

                return ExistInsuredObjectProductPolicyByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId);
            }
        }

        /// <summary>
        /// Validar si el objeto existe en un producto y si este ya fue usado en una poliza
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>
        private Boolean ExistInsuredObjectProductPolicyByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal().Constant(productId);
            filter.And();
            filter.Property(PRODEN.ProductGroupCover.Properties.CoverGroupId, typeof(PRODEN.ProductGroupCover).Name)
           .Equal().Constant(groupCoverageId);
            filter.And();
            filter.Property(PRODEN.GroupInsuredObject.Properties.InsuredObject, typeof(PRODEN.GroupInsuredObject).Name)
            .Equal().Constant(insuredObjectId);

            PolicyInsuredObjectProductView view = new PolicyInsuredObjectProductView();
            ViewBuilder builder = new ViewBuilder("PolicyInsuredObjectProductView");

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.PolicyList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Validar si la cobertura existe en un producto y si este ya fue usado en un temporal
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>
        public Boolean ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId, int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal().Constant(productId);
            filter.And();
            filter.Property(PRODEN.ProductGroupCover.Properties.CoverGroupId, typeof(PRODEN.ProductGroupCover).Name)
           .Equal().Constant(groupCoverageId);
            filter.And();
            filter.Property(PRODEN.GroupInsuredObject.Properties.InsuredObject, typeof(PRODEN.GroupInsuredObject).Name)
            .Equal().Constant(insuredObjectId);
            filter.And();
            //filter.Property(PRODEN.GroupCoverage.Properties.CoverageId, typeof(PRODEN.GroupCoverage).Name)
            //.Equal().Constant(coverageId);
            //filter.And();
            filter.Property(TMPEN.TempRiskCoverage.Properties.CoverageId, typeof(TMPEN.TempRiskCoverage).Name)
            .Equal().Constant(coverageId);

            TemporalCoverageProductView view = new TemporalCoverageProductView();
            ViewBuilder builder = new ViewBuilder("TemporalCoverageProductView");

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.TempSubscriptionList.Count > 0)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId");

                return true;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId");

                return ExistCoverageProductPolicyByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId, coverageId);
            }
        }
        /// <summary>
        /// Validar si la cobertura existe en un producto y si este ya fue usado en una poliza
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>
        private Boolean ExistCoverageProductPolicyByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId, int coverageId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal().Constant(productId);
            filter.And();
            filter.Property(PRODEN.ProductGroupCover.Properties.CoverGroupId, typeof(PRODEN.ProductGroupCover).Name)
           .Equal().Constant(groupCoverageId);
            filter.And();
            filter.Property(PRODEN.GroupInsuredObject.Properties.InsuredObject, typeof(PRODEN.GroupInsuredObject).Name)
            .Equal().Constant(insuredObjectId);
            filter.And();
            filter.Property(ISSEN.RiskCoverage.Properties.CoverageId, typeof(ISSEN.RiskCoverage).Name)
            .Equal().Constant(coverageId);


            PolicyCoverageProductView view = new PolicyCoverageProductView();
            ViewBuilder builder = new ViewBuilder("PolicyCoverageProductView");

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.PolicyList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Obtiene Riesgo de cobertura de acuerdo al Id del Producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public PRODModel.Product GetCoveredProductById(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PRODModel.Product product = new PRODModel.Product();
            List<PRODModel.CoveredRisk> productCoveredRisks = null;
            List<GroupCoverage> groupCoverages = null;
            List<Models.Coverage> Coverages = null;

            ProductRelatedCoverageView view = new ProductRelatedCoverageView();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductGroupCover.Properties.ProductId, typeof(PRODEN.ProductGroupCover).Name);
            filter.Equal();
            filter.Constant(productId);

            ViewBuilder builder = new ViewBuilder("ProductRelatedCoverageView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.ProductCoveredRiskTypeList != null && view.ProductCoveredRiskTypeList.Count > 0)
            {
                productCoveredRisks = ModelAssembler.CreateCoveredRisks(view.ProductCoveredRiskTypeList);
                if (view.CoveredRiskTypeList != null)
                {
                    foreach (PRODModel.CoveredRisk item in productCoveredRisks)
                    {
                        foreach (PARAMEN.CoveredRiskType coveredRiskType in view.CoveredRiskTypeList)
                        {
                            if (coveredRiskType.CoveredRiskTypeCode == (int)item.CoveredRiskType)
                            {
                                item.Description = coveredRiskType.SmallDescription;
                                break;
                            }
                        }
                    }
                }

            }

            if (view.ProductGroupCoverageList != null && view.ProductGroupCoverageList.Count > 0)
            {
                groupCoverages = ModelAssembler.CreateGroupCoveragesByProducts(view.ProductGroupCoverageList);
                foreach (PRODEN.ProductGroupCover productGroupCover in view.ProductGroupCoverageList)
                {
                    Coverages = new List<Models.Coverage>();
                    BusinessCollection groupCoverage = view.GetGroupCoverageListByProdGroupCoverage(productGroupCover);
                    //Objetos del seguro Producto
                    foreach (PRODEN.GroupCoverage groupCoverageEntities in groupCoverage)
                    {
                        var GroupInsuredObjectList = view.GroupInsuredObjectList.Cast<PRODEN.GroupInsuredObject>().ToList().Where(x => x.CoverageGroupCode == groupCoverageEntities.CoverGroupId).Select(x => new InsuredObject { Id = x.InsuredObject, IsSelected = x.IsSelected, IsMandatory = x.IsMandatory, Description = "" }).ToList();
                        var ProdGroupInsuredObjectList =
                        (from groupInsuredObject in GroupInsuredObjectList
                         join insuredObject in view.InsuredObjectList.Cast<QUOEN.InsuredObject>().ToList()
                         on groupInsuredObject.Id equals insuredObject.InsuredObjectId
                         select new InsuredObject { Id = groupInsuredObject.Id, IsSelected = groupInsuredObject.IsSelected, IsMandatory = groupInsuredObject.IsMandatory, Description = insuredObject.Description }).ToList();
                        if (ProdGroupInsuredObjectList != null)
                        {
                            groupCoverages.Where(x => x.Id == groupCoverageEntities.CoverGroupId).FirstOrDefault().InsuredObjects = (List<InsuredObject>)ProdGroupInsuredObjectList;
                        }
                        QUOEN.Coverage coverage = view.GetCoverageByGroupCoverage(groupCoverageEntities);
                        if (coverage != null)
                        {
                            Models.Coverage cover = ModelAssembler.CreateCoverage(coverage);

                            //Linea negocio                        
                            cover.PosRuleSetId = groupCoverageEntities.PosRuleSetId;
                            cover.RuleSetId = groupCoverageEntities.RuleSetId;
                            cover.ScriptId = groupCoverageEntities.ScriptId;
                            cover.IsMandatory = groupCoverageEntities.IsMandatory;
                            cover.IsSelected = groupCoverageEntities.IsSelected;
                            cover.IsSublimit = groupCoverageEntities.IsSublimit;
                            cover.MainCoverageId = groupCoverageEntities.MainCoverageId;
                            cover.MainCoveragePercentage = groupCoverageEntities.MainCoveragePercentage;
                            cover.CoverNum = groupCoverageEntities.CoverNum;

                            if (cover.SubLineBusiness != null)
                            {
                                cover.SubLineBusiness.Description = view.SubLineBusinessList.Cast<COMMEN.SubLineBusiness>().ToList().Where(x => x.LineBusinessCode == cover.SubLineBusiness.LineBusiness.Id && x.SubLineBusinessCode == cover.SubLineBusiness.Id).Select(x => x.Description).FirstOrDefault();
                                if (cover.SubLineBusiness.LineBusiness != null)
                                {
                                    cover.SubLineBusiness.LineBusiness.Description = view.LineBusinessList.Cast<COMMEN.LineBusiness>().ToList().Where(x => x.LineBusinessCode == cover.SubLineBusiness.LineBusiness.Id).Select(x => x.Description).FirstOrDefault();
                                }
                            }
                            //Objeto del seguro
                            cover.InsuredObject.Description = view.InsuredObjectList.Cast<QUOEN.InsuredObject>().ToList().Where(x => x.InsuredObjectId == cover.InsuredObject.Id).Select(x => x.Description).FirstOrDefault();
                            //Coberturas aliadas
                            CoverageDAO coverageDAO = new CoverageDAO();
                            cover.CoverageAllied = coverageDAO.GetCoverageAlliedByCoverageId(cover.Id);
                            Coverages.Add(cover);
                        }
                    }
                    groupCoverages.Where(x => x.Id == productGroupCover.CoverGroupId).FirstOrDefault().Coverages = Coverages;
                    groupCoverages.Where(x => x.Id == productGroupCover.CoverGroupId).FirstOrDefault().Product = null;
                }
            }
            //product.GroupCoverages = groupCoverages;
            //product.ProductCoveredRisks = productCoveredRisks;
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetCoveredRiskByProductId");

            return product;
        }

        public List<Models.LimitRCRelation> GetLimitRCRelationByProductId(int productId)
        {
            List<LimitRCRelation> limitRCRelation = new List<LimitRCRelation>();
            ProductRelatedEntitiesView view = new ProductRelatedEntitiesView();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            // Limites  de RC producto
            filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.CoLimitsRcRel.Properties.ProductId, typeof(COMMEN.CoLimitsRcRel).Name).Equal().Constant(productId);
            view.CoLimitsRcRelList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoLimitsRcRel), filter.GetPredicate()));


            //limite Rc (datos adicionales)
            if (view.CoLimitsRcRelList != null)
            {
                limitRCRelation = new List<LimitRCRelation>();
                foreach (COMMEN.CoLimitsRcRel item in view.CoLimitsRcRelList)
                {
                    limitRCRelation.Add(new LimitRCRelation
                    {
                        Id = item.LimitRcCode,
                        IsDefault = item.IsDefault,
                        PolicyType = new CommonModel.PolicyType { Id = item.PolicyTypeCode }
                    });
                }
            }
            return limitRCRelation;
        }

        /// <summary>
        /// Validacion si el producto esta en Uso en tablas reales o temporales
        /// </summary>
        /// <param name="ProductId">Id del Producto</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="Exception"></exception>
        public bool ValidatePolicyByProductId(int ProductId)
        {
            bool validatePolicyId = false;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.ProductId);
            filter.Equal();
            filter.Constant(ProductId);
            try
            {
                validatePolicyId = CheckPolicy(filter.GetPredicate());
                if (!validatePolicyId)
                {
                    validatePolicyId = CheckTempSubscription(filter.GetPredicate(), false);
                }
                return validatePolicyId;

            }
            catch (Exception ex)
            {
                if (ex is BusinessException)
                {
                    throw new BusinessException("Error in ValidatePolicyByProductId", ex);
                }
                throw new Exception(ex.Message, ex);
            }

        }
        private bool CheckPolicy(Predicate filter)
        {
            int policyCount = 0;
            #region Select
            Function f = new Function(FunctionType.Count);
            f.AddParameter(new Constant(1, System.Data.DbType.Int32));
            SelectQuery policySelectQuery = new SelectQuery();
            policySelectQuery.AddSelectValue(new SelectValue(f));
            policySelectQuery.Distinct = true;
            #endregion
            policySelectQuery.Table = new ClassNameTable(typeof(ISSEN.Policy), "Policy");
            policySelectQuery.Where = filter;
            policySelectQuery.GetFirstSelect();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(policySelectQuery))
            {
                while (reader.Read())
                {
                    if ((int)reader[0] != 0)
                    {
                        policyCount++;
                        break;
                    }
                }
            }
            if (policyCount > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Verificar si el producto está siendo usado en una cotización temporal.
        /// </summary>
        /// <param name="filter">
        /// Filtro de ProductId.
        /// </param>
        /// <param name="throwException">
        /// True : Si el producto está siendo usado, dispara una excepción.
        /// </param>
        /// <returns>   
        /// Referencia de la transacción si es que el producto está siendo usado, sino 
        /// devuelve la cadena vacía.
        /// </returns>
        private bool CheckTempSubscription(Predicate filter, bool throwException)
        {
            TMPEN.TempSubscription tempSubscription = (TMPEN.TempSubscription)DataFacadeManager.Instance.GetDataFacade().List(typeof(TMPEN.TempSubscription), filter).FirstOrDefault();
            if (tempSubscription != null)
            {

                if (throwException)
                {
                    throw new BusinessException("Error in CheckTempSubscription tempId: " + tempSubscription.TempId.ToString());
                }
                return true;
            }
            return false;
        }

        #endregion

        private PARAMEN.HardRiskType GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(int prefixId, int RiskTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(RiskTypeId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.HardRiskType), filter.GetPredicate());
            PARAMEN.HardRiskType entityHardRiskType = businessCollection.Cast<PARAMEN.HardRiskType>().FirstOrDefault();

            return entityHardRiskType;
        }
        /// <summary>
        /// Obtener Endosos de una Póliza 
        /// </summary>
        /// <param name="policyNumber">Número de Póliza</param>
        /// <returns>Endosos</returns>
        public List<Endorsement> GetEndorsementsAvaibleByPolicyId(int policyId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name);
            filter.Equal();
            filter.Constant(policyId);
            EndorsementView view = new EndorsementView();
            ViewBuilder builder = new ViewBuilder("EndorsementView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Policies.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                ConcurrentBag<Models.Endorsement> endorsements = new ConcurrentBag<Models.Endorsement>();

                TP.Parallel.ForEach(view.Endorsements.Cast<ISSEN.Endorsement>().ToList(), item =>
                {
                    endorsements.Add(new Models.Endorsement
                    {
                        Id = item.EndorsementId,
                        CurrentFrom = item.CurrentFrom,
                        CurrentTo = item.CurrentTo.Value,
                        Description = item.DocumentNum.ToString(),
                        Number = item.DocumentNum,
                        PolicyId = item.PolicyId,
                        EndorsementType = (Enums.EndorsementType)item.EndoTypeCode
                    });
                });

                ISSEN.EndorsementRisk endorsementRisk = endorsementRisks?.OrderByDescending(x => x.EndorsementId)?.FirstOrDefault(x => x.IsCurrent);
                if (endorsementRisk != null)
                {
                    endorsements.First(x => x.Id == endorsementRisk.EndorsementId).IsCurrent = true;
                }
                else
                {
                    throw new Exception(Errors.ErrorRiskNotFound);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetEndorsementsByPrefixIdBranchIdPolicyNumber");

                return endorsements.ToList();
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetEndorsementsByPrefixIdBranchIdPolicyNumber");

                return null;
            }
        }

        public List<Policy> GetClaimPoliciesByPolicyPersontypeId(Policy policy, int? personTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            PrefixDAO prefixDAO = new PrefixDAO();
            InsuredDAO insuredDAO = new InsuredDAO();
            HolderDAO holderDAO = new HolderDAO();
            List<Policy> policies = new List<Policy>();
            SelectQuery selectQuery = new SelectQuery();
            List<Dictionary<string, dynamic>> dictionaryPolicies = new List<Dictionary<string, dynamic>>();

            int coveredRiskType = 0;
            bool emptyFilter = true;
            bool specificFilter = false;

            #region Filters

            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
            {
                coveredRiskType = prefixDAO.GetPrefixCoveredRiskTypeByPrefixCode(policy.Prefix.Id);
                if (policy.Risk.RiskId > 0)
                {
                    switch ((CoveredRiskType)coveredRiskType)
                    {
                        case CoveredRiskType.Vehicle:
                            filter.Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name);
                            filter.Equal();
                            filter.Constant(policy.Risk.RiskId);
                            emptyFilter = false;
                            specificFilter = true;
                            break;
                        case CoveredRiskType.Location:
                            filter.Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name);
                            filter.Equal();
                            filter.Constant(policy.Risk.RiskId);
                            emptyFilter = false;
                            specificFilter = true;
                            break;
                        case CoveredRiskType.Surety:
                            filter.Property(ISSEN.RiskSurety.Properties.RiskId, typeof(ISSEN.RiskSurety).Name);
                            filter.Equal();
                            filter.Constant(policy.Risk.RiskId);
                            emptyFilter = false;
                            specificFilter = true;
                            break;
                        case CoveredRiskType.Transport:
                            break;
                        case CoveredRiskType.Aircraft:
                            break;
                        case CoveredRiskType.Aeronavigation:
                            break;
                    }

                }
                else
                {
                    filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
                    filter.Equal();
                    filter.Constant(policy.Prefix.Id);
                    emptyFilter = false;
                }

                if (policy?.Risk?.RiskId == 0 && policy.Risk?.MainInsured?.IndividualId > 0 && personTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsUnderwritingKeys>(ClaimsUnderwritingKeys.CLM_INSURED_PERSON_TYPE)))
                {
                    if (!emptyFilter)
                    {
                        filter.And();
                    }
                    filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
                    filter.Equal();
                    filter.Constant(policy.Risk.MainInsured.IndividualId);
                    emptyFilter = false;
                }

                if (policy?.Risk?.RiskId == 0 && policy.Risk?.MainInsured?.IndividualId > 0 && (CoveredRiskType)coveredRiskType == CoveredRiskType.Surety && personTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsUnderwritingKeys>(ClaimsUnderwritingKeys.CLM_SURETY_PERSON_TYPE)))
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name);
                    filter.Equal();
                    filter.Constant(policy.Risk.MainInsured.IndividualId);
                    emptyFilter = false;
                }
            }

            if (!specificFilter)
            {
                if (policy.Branch != null && policy.Branch.Id > 0)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
                    filter.Equal();
                    filter.Constant(policy.Branch.Id);
                    emptyFilter = false;
                }

                if (policy.DocumentNumber > 0)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
                    filter.Equal();
                    filter.Constant(policy.DocumentNumber);
                    emptyFilter = false;
                }

                if (policy.Holder != null && policy.Holder.IndividualId > 0)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Policy.Properties.PolicyholderId, typeof(ISSEN.Policy).Name);
                    filter.Equal();
                    filter.Constant(policy.Holder.IndividualId);
                    emptyFilter = false;
                }

                if (policy.CurrentFrom > DateTime.MinValue)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Policy.Properties.CurrentFrom, typeof(ISSEN.Policy).Name);
                    filter.GreaterEqual();
                    filter.Constant(policy.CurrentFrom);
                    emptyFilter = false;
                }

                if (policy.CurrentTo > DateTime.MinValue)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Policy.Properties.CurrentFrom, typeof(ISSEN.Policy).Name);
                    filter.LessEqual();
                    filter.Constant(policy.CurrentTo);
                }
            }

            filter.And();
            filter.PropertyEquals(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name, true);

            #endregion

            #region Selects

            //Policy
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name), "PolicyId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name), "DocumentNumber"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BusinessTypeCode, typeof(ISSEN.Policy).Name), "BusinessTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.CurrentFrom, typeof(ISSEN.Policy).Name), "CurrentFrom"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.CurrentTo, typeof(ISSEN.Policy).Name), "CurrentTo"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.IssueDate, typeof(ISSEN.Policy).Name), "IssueDate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PolicyholderId, typeof(ISSEN.Policy).Name), "PolicyholderId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name), "BranchCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name), "PrefixCode"));

            //Endorsement Risk
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name), "EndorsementId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name), "RiskId"));

            //Endorsement
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.CurrentTo, typeof(ISSEN.Endorsement).Name), "EndorsementCurrentTo"));

            //Risk
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name), "InsuredId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Risk.Properties.CoveredRiskTypeCode, typeof(ISSEN.Risk).Name), "CoveredRiskTypeCode"));

            //Hard Risk Type
            selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.HardRiskType.Properties.SubCoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name), "SubCoveredRiskTypeCode"));

            //Product
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name), "ProductId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, typeof(PRODEN.Product).Name), "Description"));

            #endregion

            #region Joins

            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), new ClassNameTable(typeof(ISSEN.EndorsementRisk), typeof(ISSEN.EndorsementRisk).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name)
                .Equal()
                .Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name)
                .Equal()
                .Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(ISSEN.Risk), typeof(ISSEN.Risk).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                .Equal()
                .Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.HardRiskType), typeof(PARAMEN.HardRiskType).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name)
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name)
                .And()
                .Property(ISSEN.Risk.Properties.CoveredRiskTypeCode, typeof(ISSEN.Risk).Name)
                .Equal()
                .Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PRODEN.Product), typeof(PRODEN.Product).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
                .Equal()
                .Property(ISSEN.Policy.Properties.ProductId, typeof(ISSEN.Policy).Name))
                .GetPredicate();

            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
            {
                switch ((SubCoveredRiskType)GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(policy.Prefix.Id, coveredRiskType).SubCoveredRiskTypeCode)
                {
                    case SubCoveredRiskType.Vehicle:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.ThirdPartyLiability:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Property:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name), "Street"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskLocation), typeof(ISSEN.RiskLocation).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Liability:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name), "Street"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskLocation), typeof(ISSEN.RiskLocation).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Surety:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskSurety), typeof(ISSEN.RiskSurety).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskSurety.Properties.RiskId, typeof(ISSEN.RiskSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskJudicialSurety.Properties.InsuredId, typeof(ISSEN.RiskJudicialSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskJudicialSurety), typeof(ISSEN.RiskJudicialSurety).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskJudicialSurety.Properties.RiskId, typeof(ISSEN.RiskJudicialSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Transport:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskTransport.Properties.TransportCargoTypeCode, typeof(ISSEN.RiskTransport).Name), "TransportCargoTypeCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskTransport), typeof(ISSEN.RiskTransport).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskTransport.Properties.RiskId, typeof(ISSEN.RiskTransport).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Aircraft:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.RegisterNo, typeof(ISSEN.RiskAircraft).Name), "RegisterNo"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Marine:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftDescription, typeof(ISSEN.RiskAircraft).Name), "AircraftDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));


                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Fidelity:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.Description, typeof(ISSEN.RiskFidelity).Name), "FidelityDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.RiskCommercialClassCode, typeof(ISSEN.RiskFidelity).Name), "RiskCommercialClassCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskFidelity), typeof(ISSEN.RiskFidelity).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskFidelity.Properties.RiskId, typeof(ISSEN.RiskFidelity).Name))
                            .GetPredicate();
                        break;

                    case SubCoveredRiskType.Lease:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskSurety), typeof(ISSEN.RiskSurety).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name)
                            .Equal()
                            .Property(ISSEN.RiskSurety.Properties.RiskId, typeof(ISSEN.RiskSurety).Name))
                            .GetPredicate();
                        break;
                };
            }

            #endregion

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    Dictionary<string, dynamic> dictionaryPolicy = new Dictionary<string, dynamic>();
                    dictionaryPolicy.Add("PolicyId", Convert.ToInt32(reader["PolicyId"]));
                    dictionaryPolicy.Add("DocumentNumber", Convert.ToInt32(reader["DocumentNumber"]));
                    dictionaryPolicy.Add("RiskId", Convert.ToInt32(reader["RiskId"]));
                    dictionaryPolicy.Add("InsuredId", Convert.ToInt32(reader["InsuredId"]));
                    dictionaryPolicy.Add("CoveredRiskTypeCode", Convert.ToInt32(reader["CoveredRiskTypeCode"]));
                    dictionaryPolicy.Add("BranchCode", Convert.ToInt32(reader["BranchCode"]));
                    dictionaryPolicy.Add("BusinessTypeCode", Convert.ToInt32(reader["BusinessTypeCode"]));
                    dictionaryPolicy.Add("CurrentFrom", Convert.ToDateTime(reader["CurrentFrom"]));
                    dictionaryPolicy.Add("CurrentTo", Convert.ToDateTime(reader["CurrentTo"]));
                    dictionaryPolicy.Add("EndorsementCurrentTo", Convert.ToDateTime(reader["EndorsementCurrentTo"]));
                    dictionaryPolicy.Add("EndorsementId", Convert.ToInt32(reader["EndorsementId"]));
                    dictionaryPolicy.Add("IssueDate", Convert.ToDateTime(reader["IssueDate"]));
                    dictionaryPolicy.Add("PrefixCode", Convert.ToInt32(reader["PrefixCode"]));
                    dictionaryPolicy.Add("SubCoveredRiskTypeCode", Convert.ToInt32(reader["SubCoveredRiskTypeCode"]));
                    dictionaryPolicy.Add("ProductId", Convert.ToInt32(reader["ProductId"]));
                    dictionaryPolicy.Add("ProductDescription", Convert.ToString(reader["Description"]));
                    dictionaryPolicy.Add("PolicyholderId", Convert.ToInt32(reader["PolicyholderId"]));


                    switch ((SubCoveredRiskType)Convert.ToInt32(reader["SubCoveredRiskTypeCode"]))
                    {
                        case SubCoveredRiskType.Vehicle:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("LicensePlate", Convert.ToString(reader["LicensePlate"]));
                            }
                            break;
                        case SubCoveredRiskType.ThirdPartyLiability:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("LicensePlate", Convert.ToString(reader["LicensePlate"]));
                            }
                            break;
                        case SubCoveredRiskType.Property:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("Street", Convert.ToString(reader["Street"]));
                            }
                            break;
                        case SubCoveredRiskType.Liability:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("Street", Convert.ToString(reader["Street"]));
                            }
                            break;
                        case SubCoveredRiskType.Surety:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                            }
                            break;
                        case SubCoveredRiskType.JudicialSurety:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                            }
                            break;
                        case SubCoveredRiskType.Transport:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("TransportCargoTypeCode", Convert.ToInt32(reader["TransportCargoTypeCode"]));
                            }
                            break;
                        case SubCoveredRiskType.Lease:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                            }
                            break;
                        case SubCoveredRiskType.Marine:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("AircraftDescription", Convert.ToString(reader["AircraftDescription"]));
                                dictionaryPolicy.Add("AircraftYear", Convert.ToString(reader["AircraftYear"]));
                            }
                            break;
                        case SubCoveredRiskType.Fidelity:
                            if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                            {
                                dictionaryPolicy.Add("RiskCommercialClassCode", Convert.ToInt32(reader["RiskCommercialClassCode"]));
                                dictionaryPolicy.Add("FidelityDescription", Convert.ToString(reader["FidelityDescription"]));
                            }
                            break;
                        case SubCoveredRiskType.FidelityNewVersion:
                            break;
                        case SubCoveredRiskType.Aircraft:
                            if (Convert.ToInt32(policy.Prefix?.Id) > 0)
                            {
                                dictionaryPolicy.Add("RegisterNo", Convert.ToString(reader["RegisterNo"]));
                                dictionaryPolicy.Add("AircraftYear", Convert.ToString(reader["AircraftYear"]));
                            }
                            break;
                    }

                    dictionaryPolicies.Add(dictionaryPolicy);
                }
            }

            foreach (Dictionary<string, dynamic> dictionaryPolicy in dictionaryPolicies)
            {
                Policy claimPolicy = new Policy
                {
                    Id = dictionaryPolicy["PolicyId"],
                    DocumentNumber = dictionaryPolicy["DocumentNumber"],
                    Risk = new Risk
                    {
                        Id = dictionaryPolicy["RiskId"],
                        MainInsured = new IssuanceInsured
                        {
                            IndividualId = dictionaryPolicy["InsuredId"]
                        },
                        CoveredRiskType = (CoveredRiskType)dictionaryPolicy["CoveredRiskTypeCode"]
                    },
                    Branch = new Branch
                    {
                        Id = dictionaryPolicy["BranchCode"]
                    },
                    BusinessType = (BusinessType)dictionaryPolicy["BusinessTypeCode"],
                    CurrentFrom = dictionaryPolicy["CurrentFrom"],
                    CurrentTo = dictionaryPolicy["EndorsementCurrentTo"],
                    Endorsement = new Endorsement
                    {
                        Id = dictionaryPolicy["EndorsementId"]
                    },
                    Holder = new Holder
                    {
                        IndividualId = dictionaryPolicy["PolicyholderId"]
                    },
                    IssueDate = dictionaryPolicy["IssueDate"],
                    Prefix = new Prefix
                    {
                        Id = dictionaryPolicy["PrefixCode"]
                    },
                    Product = new PRODModel.Product
                    {
                        Description = dictionaryPolicy["ProductDescription"],
                        Id = dictionaryPolicy["ProductId"]
                    }
                };

                switch ((SubCoveredRiskType)dictionaryPolicy["SubCoveredRiskTypeCode"])
                {
                    case SubCoveredRiskType.Vehicle:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskVehicle.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(key);
                            if (entityRiskVehicle != null)
                            {
                                claimPolicy.Risk.Description = entityRiskVehicle.LicensePlate;
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            claimPolicy.Risk.Description = dictionaryPolicy["LicensePlate"];
                        }
                        break;
                    case SubCoveredRiskType.ThirdPartyLiability:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskVehicle.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(key);
                            claimPolicy.Risk.Description = entityRiskVehicle.LicensePlate;
                        }
                        else
                        {
                            claimPolicy.Risk.Description = dictionaryPolicy["LicensePlate"];
                        }
                        break;
                    case SubCoveredRiskType.Property:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskLocation.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(key);
                            if (entityRiskLocation != null)
                            {
                                claimPolicy.Risk.Description = entityRiskLocation.Street;
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            claimPolicy.Risk.Description = dictionaryPolicy["Street"];
                        }
                        break;
                    case SubCoveredRiskType.Liability:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskLocation.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(key);
                            claimPolicy.Risk.Description = entityRiskLocation.Street;
                        }
                        else
                        {
                            claimPolicy.Risk.Description = dictionaryPolicy["Street"];
                        }
                        break;
                    case SubCoveredRiskType.Surety:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskSurety.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskSurety entityRiskSurety = (ISSEN.RiskSurety)DataFacadeManager.GetObject(key);

                            if (entityRiskSurety != null)
                            {
                                IssuanceInsured suretyIssuanceInsured = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(entityRiskSurety.IndividualId), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                                claimPolicy.Risk.Description = suretyIssuanceInsured == null ? "" : (suretyIssuanceInsured.IndividualType == IndividualType.Person ? (
                                            suretyIssuanceInsured.Surname + " " + (string.IsNullOrEmpty(suretyIssuanceInsured.SecondSurname) ? "" : suretyIssuanceInsured.SecondSurname + " ") + suretyIssuanceInsured.Name
                                            ) : suretyIssuanceInsured.Name);
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            string insuranceId = dictionaryPolicy["InsuranceId"];
                            IssuanceInsured suretyIssuanceInsured = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(insuranceId, InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                            claimPolicy.Risk.Description = suretyIssuanceInsured == null ? "" : (suretyIssuanceInsured.IndividualType == IndividualType.Person ? (
                                            suretyIssuanceInsured.Surname + " " + (string.IsNullOrEmpty(suretyIssuanceInsured.SecondSurname) ? "" : suretyIssuanceInsured.SecondSurname + " ") + suretyIssuanceInsured.Name
                                            ) : suretyIssuanceInsured.Name);
                        }
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskJudicialSurety.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskJudicialSurety entityRiskJudicialSurety = (ISSEN.RiskJudicialSurety)DataFacadeManager.GetObject(key);

                            if (entityRiskJudicialSurety != null)
                            {
                                IssuanceInsured judicialIssuanceInsured = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(entityRiskJudicialSurety.InsuredId), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                                claimPolicy.Risk.Description = judicialIssuanceInsured == null ? "" : (judicialIssuanceInsured.IndividualType == IndividualType.Person ? (
                                            judicialIssuanceInsured.Surname + " " + (string.IsNullOrEmpty(judicialIssuanceInsured.SecondSurname) ? "" : judicialIssuanceInsured.SecondSurname + " ") + judicialIssuanceInsured.Name
                                            ) : judicialIssuanceInsured.Name);
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            string insuranceId = dictionaryPolicy["InsuranceId"];
                            IssuanceInsured judicialIssuanceInsured = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(insuranceId, InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                            claimPolicy.Risk.Description = judicialIssuanceInsured == null ? "" : (judicialIssuanceInsured.IndividualType == IndividualType.Person ? (
                                            judicialIssuanceInsured.Surname + " " + (string.IsNullOrEmpty(judicialIssuanceInsured.SecondSurname) ? "" : judicialIssuanceInsured.SecondSurname + " ") + judicialIssuanceInsured.Name
                                            ) : judicialIssuanceInsured.Name);
                        }
                        break;
                    case SubCoveredRiskType.Transport:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskTransport.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskTransport entityRiskTransport = (ISSEN.RiskTransport)DataFacadeManager.GetObject(key);

                            if (entityRiskTransport != null)
                            {
                                key = COMMEN.TransportCargoType.CreatePrimaryKey(entityRiskTransport.TransportCargoTypeCode);
                                COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(key);

                                claimPolicy.Risk.Description = entityTransportCargoType.Description;
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            PrimaryKey primaryKey = COMMEN.TransportCargoType.CreatePrimaryKey(dictionaryPolicy["TransportCargoTypeCode"]);
                            COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(primaryKey);

                            claimPolicy.Risk.Description = entityTransportCargoType.Description;
                        }
                        break;
                    case SubCoveredRiskType.Aircraft:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskAircraft.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(key);

                            if (entityRiskAircraft != null)
                            {
                                claimPolicy.Risk.Description = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            claimPolicy.Risk.Description = dictionaryPolicy["RegisterNo"] + " - " + dictionaryPolicy["AircraftYear"];
                        }
                        break;
                    case SubCoveredRiskType.Marine:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskAircraft.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(key);

                            if (entityRiskAircraft != null)
                            {
                                claimPolicy.Risk.Description = entityRiskAircraft.AircraftDescription + " - " + entityRiskAircraft.AircraftYear;
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            claimPolicy.Risk.Description = dictionaryPolicy["AircraftDescription"] + " - " + dictionaryPolicy["AircraftYear"];
                        }
                        break;
                    case SubCoveredRiskType.Fidelity:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskFidelity.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskFidelity entityRiskFidelity = (ISSEN.RiskFidelity)DataFacadeManager.GetObject(key);

                            if (entityRiskFidelity != null)
                            {
                                key = PARAMEN.RiskCommercialClass.CreatePrimaryKey(entityRiskFidelity.RiskCommercialClassCode);
                                PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(key);

                                claimPolicy.Risk.Description = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass?.Description;
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            PrimaryKey key = PARAMEN.RiskCommercialClass.CreatePrimaryKey(dictionaryPolicy["RiskCommercialClassCode"]);
                            PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(key);

                            claimPolicy.Risk.Description = (!string.IsNullOrEmpty(dictionaryPolicy["FidelityDescription"]) ? dictionaryPolicy["FidelityDescription"] + " - " : "") + entityRiskCommercialClass?.Description;
                        }
                        break;

                    case SubCoveredRiskType.Lease:
                        if (Convert.ToInt32(policy.Prefix?.Id) == 0)
                        {
                            PrimaryKey key = ISSEN.RiskSurety.CreatePrimaryKey(claimPolicy.Risk.Id);
                            ISSEN.RiskSurety entityRiskSurety = (ISSEN.RiskSurety)DataFacadeManager.GetObject(key);

                            if (entityRiskSurety != null)
                            {
                                IssuanceInsured suretyIssuanceInsured = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(entityRiskSurety.IndividualId), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                                claimPolicy.Risk.Description = suretyIssuanceInsured == null ? "" : (suretyIssuanceInsured.IndividualType == IndividualType.Person ? (
                                            suretyIssuanceInsured.Surname + " " + (string.IsNullOrEmpty(suretyIssuanceInsured.SecondSurname) ? "" : suretyIssuanceInsured.SecondSurname + " ") + suretyIssuanceInsured.Name
                                            ) : suretyIssuanceInsured.Name);
                            }
                            else
                            {
                                claimPolicy.Risk.Description = Resources.Errors.RiskNotFound;
                            }
                        }
                        else
                        {
                            string insuranceId = dictionaryPolicy["InsuranceId"];
                            IssuanceInsured suretyIssuanceInsured = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(insuranceId, InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                            claimPolicy.Risk.Description = suretyIssuanceInsured == null ? "" : (suretyIssuanceInsured.IndividualType == IndividualType.Person ? (
                                            suretyIssuanceInsured.Surname + " " + (string.IsNullOrEmpty(suretyIssuanceInsured.SecondSurname) ? "" : suretyIssuanceInsured.SecondSurname + " ") + suretyIssuanceInsured.Name
                                            ) : suretyIssuanceInsured.Name);
                        }
                        break;
                }

                claimPolicy.Holder = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(claimPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();

                policies.Add(claimPolicy);
            }

            return policies.GroupBy(x => x.Id).Select(x => x.First()).ToList();
        }
        public List<Endorsement> GetPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            var parameters = new NameValue[3];
            parameters[0] = new NameValue("DOCUMENT_NUMBER", policyNumber);
            parameters[1] = new NameValue("PREFIX_ID", prefixId);
            parameters[2] = new NameValue("BRANCH_ID", branchId);
            DataTable resultTable;
            using (var dataAccess = new DynamicDataAccess())
            {
                resultTable = dataAccess.ExecuteSPDataTable("ISS.GET_POLICY_ENDORSEMENTS", parameters);
            }
            //Nunca debería llegar nulo pero se deja por si se llega a modificar el sp
            if (resultTable == null || resultTable.Rows.Count == 0)
            {
                return null;
            }
            List<Endorsement> listEndorsement = new List<Endorsement>();

            foreach (DataRow resul in resultTable.Rows)
            {
                var currentTo = resul["CURRENT_TO"];
                var endorsement = new Endorsement
                {
                    Id = (int)resul["ENDORSEMENT_ID"],
                    CurrentFrom = (DateTime)resul["CURRENT_FROM"],
                    EndorsementType = (EndorsementType)Convert.ToInt32(resul["ENDO_TYPE_CD"]),
                    PolicyId = (int)resul["POLICY_ID"]
                };
                if (currentTo != DBNull.Value)
                {
                    endorsement.CurrentTo = (DateTime)currentTo;
                }
                endorsement.EndorsementDays = (int)(endorsement.CurrentTo - endorsement.CurrentFrom).TotalDays;
                listEndorsement.Add(endorsement);
            }

            return listEndorsement;
        }

        public List<Endorsement> GetPolicyEndorsementsWithPremiumByPolicyId(int policyId)
        {
            var parameters = new NameValue[1];
            parameters[0] = new NameValue("POLICY_ID", policyId);
            DataTable resultTable;
            using (var dataAccess = new DynamicDataAccess())
            {
                resultTable = dataAccess.ExecuteSPDataTable("ISS.GET_POLICY_ENDORSEMENTS_WITH_PREMIUM", parameters);
            }
            //Nunca debería llegar nulo pero se deja por si se llega a modificar el sp
            if (resultTable == null || resultTable.Rows.Count == 0)
            {
                return null;
            }
            List<Endorsement> listEndorsement = new List<Endorsement>();

            foreach (DataRow result in resultTable.Rows)
            {
                var currentTo = result["CURRENT_TO"];
                var endorsement = new Endorsement
                {
                    Id = (int)result["ENDORSEMENT_ID"],
                    CurrentFrom = (DateTime)result["CURRENT_FROM"],
                    EndorsementType = (EndorsementType)Convert.ToInt32(result["ENDO_TYPE_CD"]),
                    PolicyId = (int)result["POLICY_ID"],
                    Number = (int)result["DOCUMENT_NUM"],
                    EndorsementTypeDescription = (string)result["DESCRIPTION_ENDO_TYPE"]
                };
                if (currentTo != DBNull.Value)
                {
                    endorsement.CurrentTo = (DateTime)currentTo;
                }
                listEndorsement.Add(endorsement);
            }

            return listEndorsement;
        }
        #region grabado integracion
        /// <summary>
        /// Grabado Integracion
        /// </summary>
        /// <param name="endorsemenId"></param>
        /// <param name="operationId"></param>
        /// <param name="isMassive"></param>
        public void SaveControlPolicy(int policyId, int endorsemenId, int operationId, int policyOrigin)
        {
            ISSEN.TempPolicyControl tempPolicyControl = new ISSEN.TempPolicyControl(operationId);
            tempPolicyControl.EndorsementId = endorsemenId;
            tempPolicyControl.PolicyOrigin = policyOrigin;
            tempPolicyControl.AppVersionId = (int)AppSource.R2;
            tempPolicyControl.PolicyId = policyId;
            DataFacadeManager.Insert(tempPolicyControl);
            DataFacadeManager.Dispose();
        }
        #endregion

        #region Claim

        /// <summary>
        /// Obtener póliza de siniestro por identificador del endoso
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns> Póliza </returns>
        public Policy GetClaimPolicyByEndorsementid(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            ClaimPolicyView view = new ClaimPolicyView();
            ViewBuilder builder = new ViewBuilder("ClaimPolicyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Endorsements.Count > 0)
            {
                Policy policy = ModelAssembler.CreateClaimPolicy((ISSEN.Policy)view.Policies.First(), (ISSEN.CoPolicy)view.CoPolicies.First(), (ISSEN.Endorsement)view.Endorsements.First(), (PARAMEN.BusinessType)view.BusinessTypes.First(), (COMMEN.Currency)view.Currencies.First(), (COMMEN.CoPolicyType)view.CoPolicyTypes.First(), (PARAMEN.EndorsementType)view.EndorsementTypes.First());

                HolderDAO holderDAO = new HolderDAO();
                policy.Holder = holderDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                Agency agency = DelegateService.uniquePersonServiceCoreV1.GetAgencyByAgentIdAgentAgencyId(view.PolicyAgents.Cast<ISSEN.PolicyAgent>().First(x => x.IsPrimary).IndividualId, view.PolicyAgents.Cast<ISSEN.PolicyAgent>().First(x => x.IsPrimary).AgentAgencyId);

                policy.Agencies.Add(new IssuanceAgency
                {
                    Id = agency.Id,
                    FullName = agency.FullName,
                    AgentType = new IssuanceAgentType
                    {
                        Description = agency.AgentType.Description
                    },
                    Agent = new IssuanceAgent
                    {
                        IndividualId = agency.Agent.IndividualId,
                        FullName = agency.Agent.FullName
                    }
                });

                policy.Prefix.Description = view.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == policy.Prefix.Id).Description;
                policy.Branch.Description = view.Branches.Cast<COMMEN.Branch>().First(x => x.BranchCode == policy.Branch.Id).Description;

                #region Summary

                policy.Summary = GetTotalSummary(endorsementId);

                filter.Clear();
                filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
                filter.Equal();
                filter.Constant(ComponentType.Premium);

                QUOEN.Component entityComponent = DataFacadeManager.GetObjects(typeof(QUOEN.Component), filter.GetPredicate()).Cast<QUOEN.Component>().FirstOrDefault();
                if (entityComponent != null)
                {
                    filter.Clear();
                    if (policy.Endorsement.EndorsementType == EndorsementType.Nominative_cancellation)
                    {
                        filter.Property(ISSEN.PayerComp.Properties.EndorsementId, typeof(ISSEN.PayerComp).Name);
                        filter.Equal();
                        filter.Constant(policy.Endorsement.Id);
                    }
                    else
                    {
                        filter.Property(ISSEN.PayerComp.Properties.PolicyId, typeof(ISSEN.PayerComp).Name);
                        filter.Equal();
                        filter.Constant(policy.Id);
                    }

                    List<ISSEN.PayerComp> entityPayerComponents = DataFacadeManager.GetObjects(typeof(ISSEN.PayerComp), filter.GetPredicate()).Cast<ISSEN.PayerComp>().ToList();

                    policy.Summary.Premium = entityPayerComponents.Where(x => x != null && (ComponentType)x.ComponentCode == ComponentType.Premium).Sum(x => x.ComponentAmount);
                }

                #endregion

                decimal ownParticipation = policy.CoInsuranceCompanies.First().ParticipationPercentageOwn;

                if (policy.BusinessType == BusinessType.Assigned)
                {
                    policy.CoInsuranceCompanies = ModelAssembler.CreateCoInsurancesAssigned(view.CoinsurancesAssigned);

                    foreach (var item in policy.CoInsuranceCompanies)
                    {
                        PrimaryKey primaryKey = COMMEN.CoInsuranceCompany.CreatePrimaryKey(item.Id);
                        COMMEN.CoInsuranceCompany coInsuranceCompany = (COMMEN.CoInsuranceCompany)DataFacadeManager.GetObject(primaryKey);
                        item.Description = coInsuranceCompany.Description;
                        item.ParticipationPercentageOwn = ownParticipation;
                    }
                }
                else if (policy.BusinessType == BusinessType.Accepted)
                {
                    policy.CoInsuranceCompanies = ModelAssembler.CreateCoInsuranceAccepted(view.CoinsurancesAccepted);

                    foreach (var item in policy.CoInsuranceCompanies)
                    {
                        PrimaryKey primaryKey = COMMEN.CoInsuranceCompany.CreatePrimaryKey(item.Id);
                        COMMEN.CoInsuranceCompany coInsuranceCompany = (COMMEN.CoInsuranceCompany)DataFacadeManager.GetObject(primaryKey);
                        item.Description = coInsuranceCompany.Description;
                        item.ParticipationPercentageOwn = ownParticipation;
                    }
                }

                return policy;
            }
            else
            {
                return null;
            }
        }

        public Summary GetTotalSummary(int endorsementId)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", endorsementId);
            DataTable dt;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dt = dynamicDataAccess.ExecuteSPDataTable("QUO.GET_TOTAL_SUMMARY", parameters);
            }

            Summary summary = new Summary();
            if (dt != null && dt.Rows.Count > 0)
            {
                summary = new Summary()
                {
                    RiskCount = Convert.ToInt16(dt.Rows[0][0].ToString()),
                    AmountInsured = Convert.ToDecimal(dt.Rows[0][1].ToString())
                };
            }
            return summary;
        }

        #endregion


        public int GetPolicyIdByEndormestId(int endormestId)
        {

            SelectQuery selectQuery = new SelectQuery();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endormestId);

            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name)));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name)));
            selectQuery.Table = new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name);
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    return Convert.ToInt32(reader["PolicyId"].ToString());
                }
            }
            return 0;
        }

        public PortfolioPolicy GetPortfolioPolicyByPolicy(int branch, int prefix, string documentNumber)
        {
            var parameters = new NameValue[5];
            parameters[0] = new NameValue("BRANCH", branch);
            parameters[1] = new NameValue("PREFIX", prefix);
            parameters[2] = new NameValue("POLICY_NUMBER", documentNumber);
            parameters[3] = new NameValue("DOCUMENT_TYPE", DBNull.Value);
            parameters[4] = new NameValue("DOCUMENT_NUMBER", DBNull.Value);

            DataTable resultTable;

            using (var dataAccess = new DynamicDataAccess())
            {
                resultTable = dataAccess.ExecuteSPDataTable("ISS.GET_PORTFOLIO_POLICY", parameters);
            }

            return new PortfolioPolicy
            {
                PortfolioDays = (int)resultTable.Rows[0]["DIAS_DE_CARTERA"],
                IssueValue = double.Parse(resultTable.Rows[0]["VALOR_EMITIDO"].ToString()),
                CollectedValue = double.Parse(resultTable.Rows[0]["VALOR_RECAUDADO"].ToString()),
                PendingValue = double.Parse(resultTable.Rows[0]["SALDO_PENDIENTE"].ToString())
            };
        }

        public List<PortfolioPolicy> GetPortfolioPolicyByPerson(List<PortfolioPolicy> portfolioPolicy)
        {

            var parameters = new NameValue[5];
            parameters[0] = new NameValue("BRANCH", DBNull.Value);
            parameters[1] = new NameValue("PREFIX", DBNull.Value);
            parameters[2] = new NameValue("POLICY_NUMBER", DBNull.Value);

            foreach (PortfolioPolicy item in portfolioPolicy)
            {
                parameters[3] = new NameValue("DOCUMENT_TYPE", item.DocumentType.Id);
                parameters[4] = new NameValue("DOCUMENT_NUMBER", item.DocumentNumber);

                DataTable resultTable;

                using (var dataAccess = new DynamicDataAccess())
                {
                    resultTable = dataAccess.ExecuteSPDataTable("ISS.GET_PORTFOLIO_POLICY", parameters);
                }


                item.PortfolioDays = (int)resultTable.Rows[0]["DIAS_DE_CARTERA"];
                item.IssueValue = double.Parse(resultTable.Rows[0]["VALOR_EMITIDO"].ToString());
                item.CollectedValue = double.Parse(resultTable.Rows[0]["VALOR_RECAUDADO"].ToString());
                item.PendingValue = double.Parse(resultTable.Rows[0]["SALDO_PENDIENTE"].ToString());
            }

            return portfolioPolicy;
        }
    }
}