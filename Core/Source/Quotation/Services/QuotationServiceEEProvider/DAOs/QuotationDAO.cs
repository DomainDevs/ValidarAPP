using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.QuotationServices.EEProvider.Assemblers;
using Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using CMM = Sistran.Core.Application.Common.Entities;
using QUO = Sistran.Core.Application.Quotation.Entities;
using PAR = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.ModelServices.Models.Underwriting;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.QuotationServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Linq;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.QuotationServices.EEProvider.DAOs
{
    public class QuotationDAO
    {
        /// <summary>
        /// Obtener Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Cotización</returns>
        public List<Policy> GetPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Policy> policies = new List<Policy>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(TempSubscription.Properties.QuotationId, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant(quotationId);
            filter.And();
            filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant(TemporalType.Quotation);

            if (version > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.QuotationVersion, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(version);
                filter.And();
                filter.Property(TempSubscription.Properties.PrefixCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(prefixId);
                filter.And();
                filter.Property(TempSubscription.Properties.BranchCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(branchId);
            }

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(TempSubscription), filter.GetPredicate());

            if (businessCollection.Count == 1)
            {
                int operationId = ((TempSubscription)businessCollection[0]).OperationId.GetValueOrDefault();
                int temporalId = ((TempSubscription)businessCollection[0]).TempId;

                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                if (pendingOperation != null)
                {
                    Policy policy = JsonConvert.DeserializeObject<Policy>(pendingOperation.Operation);
                    policy.Id = operationId;
                    policy.Endorsement.QuotationId = quotationId;
                    policy.Endorsement.TemporalId = temporalId;

                    pendingOperation.Operation = JsonConvert.SerializeObject(policy);
                    pendingOperation = DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

                    policies.Add(policy);
                }
            }
            else if (businessCollection.Count > 1)
            {
                policies = businessCollectionPolicies(businessCollection);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.GetPoliciesByQuotationIdVersionPrefixId");

            return policies;
        }

        /// <summary>
        /// Crear Nueva Versión De Una Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <returns>Cotización</returns>
        public Policy CreateNewVersionQuotation(int operationId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

            if (pendingOperation != null)
            {
                Policy policy = JsonConvert.DeserializeObject<Policy>(pendingOperation.Operation);

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(TempSubscription.Properties.QuotationVersion, typeof(TempSubscription).Name);
                filter.IsNotNull();
                filter.And();
                filter.Property(TempSubscription.Properties.QuotationId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Endorsement.QuotationId);
                filter.And();
                filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(TemporalType.Quotation);
                filter.And();
                filter.Property(TempSubscription.Properties.PrefixCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Prefix.Id);
                filter.And();
                filter.Property(TempSubscription.Properties.BranchCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Branch.Id);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(TempSubscription), filter.GetPredicate());

                policy.Endorsement.TemporalId = 0;
                policy.Endorsement.QuotationVersion = businessCollection.Count + 1;

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

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.CreateNewVersionQuotation");

                return policy;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.CreateNewVersionQuotation");

                return null;
            }
        }

        /// <summary>
        /// Convertir Cotización en Temporal
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Temporal</returns>
        public Policy CreateTemporalFromQuotation(int operationId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

            if (pendingOperation != null)
            {
                Policy policy = JsonConvert.DeserializeObject<Policy>(pendingOperation.Operation);
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
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.CreateTemporalFromQuotation");

                return policy;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.CreateTemporalFromQuotation");

                return null;
            }
        }

        /// <summary>
        /// Obtener Cotización de busqueda avanzada
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="issueFrom"></param>
        /// <param name="issueTo"></param>
        /// <returns>Cotizaciones</returns>
        public List<Policy> GetPoliciesByPolicy(Policy policy)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Policy> policies = new List<Policy>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TempSubscription.Properties.QuotationVersion, typeof(TempSubscription).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant((int)TemporalType.Quotation);
            if (policy.Endorsement != null && policy.Endorsement.QuotationId > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.QuotationId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Endorsement.QuotationId);
            }

            if (policy.Holder != null && policy.Holder.IndividualId > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.PolicyHolderId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Holder.IndividualId);
            }

            if (policy.UserId > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.UserId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.UserId);
            }

            if (policy.CurrentFrom > DateTime.MinValue)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.CurrentFrom, typeof(TempSubscription).Name);
                filter.GreaterEqual();
                filter.Constant(policy.CurrentFrom);
            }

            if (policy.CurrentTo > DateTime.MinValue)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.CurrentTo, typeof(TempSubscription).Name);
                filter.LessEqual();
                filter.Constant(policy.CurrentTo);
            }
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TempSubscription), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                policies = businessCollectionPolicies(businessCollection);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.GetPoliciesByPolicy");

            return policies;
        }

        public List<Policy> businessCollectionPolicies(BusinessCollection businessCollection)
        {
            List<Policy> policies = new List<Policy>();
            int cont = 0;
            foreach (TempSubscription tempSubscription in businessCollection)
            {
                cont++;
                if (cont <= 20)
                {
                    int operationId = tempSubscription.OperationId.GetValueOrDefault();
                    int quotationId = tempSubscription.QuotationId.GetValueOrDefault();

                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                    if (pendingOperation != null && !string.IsNullOrEmpty(pendingOperation.Operation))
                    {
                        Policy policyBusiness = JsonConvert.DeserializeObject<Policy>(pendingOperation.Operation);
                        policyBusiness.Id = operationId;
                        policyBusiness.Endorsement.QuotationId = quotationId;
                        policyBusiness.Endorsement.TemporalId = tempSubscription.TempId;
                        Policy policy = GetPolicyByQuotationId(quotationId, policyBusiness.Prefix.Id, policyBusiness.Branch.Id);
                        if (policy != null)
                        {
                            policyBusiness.DocumentNumber = policy.DocumentNumber;
                        }
                        policies.Add(policyBusiness);
                    }
                }
            }
            return policies;
        }
        public Policy GetPolicyByQuotationId(int quotationId, int prefixCd, int branchCd)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.QuotationId, "e");
            filter.Equal();
            filter.Constant(quotationId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.PrefixCode, "p");
            filter.Equal();
            filter.Constant(prefixCd);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.BranchCode, "p");
            filter.Equal();
            filter.Constant(branchCd);
            SelectQuery SelectQuery = new SelectQuery();
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, "p"), "DocumentNumber"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PrefixCode, "p"), "PrefixCode"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BranchCode, "p"), "BranchCode"));
            #endregion Select
            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), "p"), new ClassNameTable(typeof(ISSEN.Endorsement), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PolicyId, "p")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.PolicyId, "e")
                .GetPredicate());
            SelectQuery.Table = join;
            SelectQuery.Where = filter.GetPredicate();
            SelectQuery.GetFirstSelect();
            Policy policy = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    policy = new Policy();
                    policy.DocumentNumber = (decimal)reader["DocumentNumber"];
                    policy.Branch = new Branch { Id = (int)reader["BranchCode"] };
                    policy.Prefix = new Prefix { Id = (int)reader["PrefixCode"] };
                    break;
                }
            }
            return policy;
        }

        #region ConditionText
        #region get 
        public List<Models.ConditionTextModel> GetConditionTexts(string nameConditionText)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();            

            //ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.ConditionLevelCode, "a"), "ConditionLevelCode"));
            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.ConditionTextId, "a"), "ConditionTextId"));
            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.TextBody, "a"), "TextBody"));
            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.TextTitle, "a"), "TextTitle"));

            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.ConditionLevelId, "ai"), "ConditionLevelId"));
            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.ConditionTextId, "ai"), "ConditionTextId"));
            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.CondTextLevelId, "ai"), "CondTextLevelId"));
            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.IsAutomatic, "ai"), "IsAutomatic"));


            select.AddSelectValue(new SelectValue(new Column(PAR.ConditionLevel.Properties.ConditionLevelCode, "cl"), "ConditionLevelCode"));
            select.AddSelectValue(new SelectValue(new Column(PAR.ConditionLevel.Properties.SmallDescription, "cl"), "SmallDescription"));

            select.AddSelectValue(new SelectValue(new Column(CMM.Prefix.Properties.PrefixCode, "p"), "PrefixCode"));
            select.AddSelectValue(new SelectValue(new Column(CMM.Prefix.Properties.Description, "p"), "Description"));

            select.AddSelectValue(new SelectValue(new Column(PAR.CoveredRiskType.Properties.CoveredRiskTypeCode, "cr"), "CoveredRiskTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(PAR.CoveredRiskType.Properties.SmallDescription, "cr"), "SmallDescription"));

            select.AddSelectValue(new SelectValue(new Column(QUO.Coverage.Properties.CoverageId, "c"), "CoverageId"));
            select.AddSelectValue(new SelectValue(new Column(QUO.Coverage.Properties.PrintDescription, "c"), "PrintDescription"));

            filter.Property(ConditionText.Properties.TextTitle, "a");
            filter.Like();
            filter.Constant("%" + nameConditionText+"%");

            Join join = new Join(new ClassNameTable(typeof(ConditionText), "a"), new ClassNameTable(typeof(CondTextLevel), "ai"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ConditionText.Properties.ConditionTextId, "a")
                .Equal()
                .Property(CondTextLevel.Properties.ConditionTextId, "ai")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PAR.ConditionLevel), "cl"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ConditionText.Properties.ConditionLevelCode, "a")
                .Equal()
                .Property(PAR.ConditionLevel.Properties.ConditionLevelCode, "cl")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(CMM.Prefix), "p"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CMM.Prefix.Properties.PrefixCode, "p")
                .Equal()
                .Property(CondTextLevel.Properties.ConditionLevelId, "ai")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PAR.CoveredRiskType), "cr"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CondTextLevel.Properties.ConditionLevelId, "ai")
                .Equal()
                .Property(PAR.CoveredRiskType.Properties.CoveredRiskTypeCode, "cr")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(QUO.Coverage), "c"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CondTextLevel.Properties.ConditionLevelId, "ai")
                .Equal()
                .Property(QUO.Coverage.Properties.CoverageId, "c")
                .GetPredicate());
            select.Table = join;
            select.Where = filter.GetPredicate();
            List<Models.ConditionTextModel> listConditionTextModel = new List<Models.ConditionTextModel>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    Models.ConditionTextModel model = new Models.ConditionTextModel();
                    model.ConditionLevelCode = (int)reader["ConditionLevelCode"];
                    model.ConditionTextId = (int)reader["ConditionTextId"];
                    model.TextTitle = reader["TextTitle"].ToString();
                    model.TextBody = reader["TextBody"].ToString();

                    model.CondTextLevelModel = new Models.CondTextLevelModel
                    {
                        CondTextLevelId = (int)reader["CondTextLevelId"],
                        ConditionLevelId = (int?)reader["ConditionLevelId"],
                        IsAutomatic = (bool)reader["IsAutomatic"]
                    };
                    if (reader["ConditionLevelCode"] != null)
                    {
                        model.ConditionLevel = new ConditionLevel
                        {
                            Id = (int)reader["ConditionLevelCode"],
                            Description = reader["SmallDescription"].ToString(),
                        };
                    }
                    if (reader["PrefixCode"] != null)
                    {
                        model.Prefix = new Prefix
                        {
                            Id = (int)reader["PrefixCode"],
                            Description = reader["Description"].ToString(),

                        };
                    }

                    if (reader["CoverageId"] != null)
                    {
                        model.coverage = new UnderwritingServices.Models.Coverage
                        {
                            Id = (int)reader["CoverageId"],
                            Description = reader["PrintDescription"].ToString(),

                        };
                    }

                    if (!(listConditionTextModel.Exists(x => x.ConditionTextId == model.ConditionTextId)))
                    {
                        listConditionTextModel.Add(model);
                    }
                }
            }
            return listConditionTextModel;
        }

        public List<Models.ConditionTextModel> GetConditionText()
        {
            //ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.ConditionLevelCode, "a"), "ConditionLevelCode"));
            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.ConditionTextId, "a"), "ConditionTextId"));
            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.TextBody, "a"), "TextBody"));
            select.AddSelectValue(new SelectValue(new Column(ConditionText.Properties.TextTitle, "a"), "TextTitle"));

            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.ConditionLevelId, "ai"), "ConditionLevelId"));
            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.ConditionTextId, "ai"), "ConditionTextId"));
            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.CondTextLevelId, "ai"), "CondTextLevelId"));
            select.AddSelectValue(new SelectValue(new Column(CondTextLevel.Properties.IsAutomatic, "ai"), "IsAutomatic"));


            select.AddSelectValue(new SelectValue(new Column(PAR.ConditionLevel.Properties.ConditionLevelCode, "cl"), "ConditionLevelCode"));
            select.AddSelectValue(new SelectValue(new Column(PAR.ConditionLevel.Properties.SmallDescription, "cl"), "SmallDescription"));

            select.AddSelectValue(new SelectValue(new Column(CMM.Prefix.Properties.PrefixCode, "p"), "PrefixCode"));
            select.AddSelectValue(new SelectValue(new Column(CMM.Prefix.Properties.Description, "p"), "Description"));

            select.AddSelectValue(new SelectValue(new Column(PAR.CoveredRiskType.Properties.CoveredRiskTypeCode, "cr"), "CoveredRiskTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(PAR.CoveredRiskType.Properties.SmallDescription, "cr"), "SmallDescription"));

            select.AddSelectValue(new SelectValue(new Column(QUO.Coverage.Properties.CoverageId, "c"), "CoverageId"));
            select.AddSelectValue(new SelectValue(new Column(QUO.Coverage.Properties.PrintDescription, "c"), "PrintDescription"));


            Join join = new Join(new ClassNameTable(typeof(ConditionText), "a"), new ClassNameTable(typeof(CondTextLevel), "ai"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ConditionText.Properties.ConditionTextId, "a")
                .Equal()
                .Property(CondTextLevel.Properties.ConditionTextId, "ai")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PAR.ConditionLevel), "cl"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ConditionText.Properties.ConditionLevelCode, "a")
                .Equal()
                .Property(PAR.ConditionLevel.Properties.ConditionLevelCode, "cl")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(CMM.Prefix), "p"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CMM.Prefix.Properties.PrefixCode, "p" )
                .Equal()
                .Property(CondTextLevel.Properties.ConditionLevelId, "ai")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PAR.CoveredRiskType), "cr"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CondTextLevel.Properties.ConditionLevelId, "ai")
                .Equal()
                .Property(PAR.CoveredRiskType.Properties.CoveredRiskTypeCode, "cr")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(QUO.Coverage), "c"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CondTextLevel.Properties.ConditionLevelId, "ai")
                .Equal()
                .Property(QUO.Coverage.Properties.CoverageId, "c")
                .GetPredicate());



            select.Table = join;

            //select.Where = filter.GetPredicate();
            List<Models.ConditionTextModel> listConditionTextModel = new List<Models.ConditionTextModel>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    Models.ConditionTextModel model = new Models.ConditionTextModel();
                    model.ConditionLevelCode = (int)reader["ConditionLevelCode"];
                    model.ConditionTextId = (int)reader["ConditionTextId"];
                    model.TextTitle = reader["TextTitle"].ToString();
                    model.TextBody = reader["TextBody"].ToString();

                    model.CondTextLevelModel = new Models.CondTextLevelModel{
                        CondTextLevelId = (int)reader["CondTextLevelId"],
                        ConditionLevelId = (int?)reader["ConditionLevelId"],
                        IsAutomatic = (bool)reader["IsAutomatic"]
                    };
                    if(reader["ConditionLevelCode"]!=null)
                    {
                        model.ConditionLevel = new ConditionLevel
                        {
                            Id = (int)reader["ConditionLevelCode"],
                            Description = reader["SmallDescription"].ToString(),
                        };
                    }
                    if(reader["PrefixCode"]!=null)
                    {
                        model.Prefix = new Prefix
                        {
                            Id = (int)reader["PrefixCode"],
                            Description = reader["Description"].ToString(),

                        };
                    }
                   
                    if(reader["CoverageId"]!=null)
                    {
                        model.coverage = new UnderwritingServices.Models.Coverage
                        {
                            Id = (int)reader["CoverageId"],
                            Description = reader["PrintDescription"].ToString(),

                        };
                    }

                    if (!(listConditionTextModel.Exists(x => x.ConditionTextId == model.ConditionTextId)))
                    {
                        listConditionTextModel.Add(model);
                    }
                }
            }
            return listConditionTextModel;
        }
        #endregion
        #region insert 
        public ConditionText CreateConditionText(ConditionText conditionTextModel)
        {
            DataFacadeManager.Instance.GetDataFacade().InsertObject(conditionTextModel);
            return conditionTextModel;
        }
        public List<ConditionText> LsitCreateCondition(List<ConditionText> conditionTextModel)
        {
            var ConditionText = new List<ConditionText>();
            foreach (var item in conditionTextModel)
            {
                ConditionText.Add(CreateConditionText(item));
            }
            return conditionTextModel;
        }
        public CondTextLevel CreateConditionTextLevel(CondTextLevel conditionTextModel)
        {
            DataFacadeManager.Instance.GetDataFacade().InsertObject(conditionTextModel);
            return conditionTextModel;
        }

        public List<CondTextLevel> CreateConditionTextLevels(List<CondTextLevel> conditionTextModels)
        {
            var CodnitionText = new List<CondTextLevel>();
            foreach (var item in conditionTextModels)
            {
                CodnitionText.Add(CreateConditionTextLevel(item));
            }

            return CodnitionText;
        }
        #endregion
        #region update 
        public ConditionText UpdateConditionText(ConditionText conditionText)
        {
            PrimaryKey primaryKey = Quotation.Entities.ConditionText.CreatePrimaryKey(conditionText.ConditionTextId);
            Quotation.Entities.ConditionText ConditionTextEntidad = (Quotation.Entities.ConditionText)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            ConditionTextEntidad.ConditionLevelCode = conditionText.ConditionLevelCode;
            ConditionTextEntidad.TextTitle = conditionText.TextTitle;
            ConditionTextEntidad.TextBody = conditionText.TextBody;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(ConditionTextEntidad);
            return conditionText;
        }
        public CondTextLevel UpdateConditionTextLevel(CondTextLevel condTextLevel)
        {
            PrimaryKey primaryKey = Quotation.Entities.CondTextLevel.CreatePrimaryKey(condTextLevel.CondTextLevelId);
            Quotation.Entities.CondTextLevel ConditionTextLevelEntidad = (Quotation.Entities.CondTextLevel)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(ConditionTextLevelEntidad);
            return condTextLevel;
        }

        public List<ConditionText> UpdateConditionTexts(List<ConditionText> conditionTextModels)
        {
            var CodnitionText = new List<ConditionText>();
            foreach (var item in conditionTextModels)
            {
                CodnitionText.Add(UpdateConditionText(item));
            }

            return CodnitionText;
        }

        public List<CondTextLevel> UpdateConditionTextLevels(List<CondTextLevel> CondTextLevel)
        {
            var CodnitionText = new List<CondTextLevel>();
            foreach (var item in CondTextLevel)
            {
                CodnitionText.Add(UpdateConditionTextLevel(item));
            }
            return CodnitionText;
        }
        #endregion
        #region delete
        public Models.ConditionTextModel DeleteConditionText(Models.ConditionTextModel conditionTextModel)
        {
            ConditionText ConditionText = null;
            PrimaryKey primaryKey = Quotation.Entities.ConditionText.CreatePrimaryKey(conditionTextModel.ConditionLevelCode);
            Quotation.Entities.ConditionText ConditionTextEntidad = (Quotation.Entities.ConditionText)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (ConditionTextEntidad != null)
            {
                ConditionText = EntityAssembler.CreateConditionText(conditionTextModel);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ConditionText);
            }

            return ModelAssembler.CreateConditionText(ConditionText);
        }
        public Models.CondTextLevelModel DeleteConditionTextLevel(Models.CondTextLevelModel conditionTextModel)
        {           
            CondTextLevel ConditionTextLevel = null;
            PrimaryKey primaryKey = Quotation.Entities.CondTextLevel.CreatePrimaryKey(conditionTextModel.condition.ConditionLevelCode);
            Quotation.Entities.CondTextLevel ConditionTextLevelEntidad = (Quotation.Entities.CondTextLevel)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (ConditionTextLevelEntidad != null)
            {
                ConditionTextLevel = EntityAssembler.CreateConditionTextLevel(conditionTextModel);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ConditionTextLevel);
            }

            return ModelAssembler.CreateConditionTextLevel(ConditionTextLevel);
        }

        public CondTextLevel DeleteConditionTextLevel(CondTextLevel condTextLevel)
        {
            PrimaryKey primaryKey = Quotation.Entities.CondTextLevel.CreatePrimaryKey(condTextLevel.CondTextLevelId);
            Quotation.Entities.CondTextLevel ConditionTextLevelEntidad = (Quotation.Entities.CondTextLevel)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(ConditionTextLevelEntidad);
            return condTextLevel;
        }

        public ConditionText DeleteConditionText(ConditionText condText)
        {
            PrimaryKey primaryKey = Quotation.Entities.ConditionText.CreatePrimaryKey(condText.ConditionTextId);
            Quotation.Entities.ConditionText ConditionTextEntidad = (Quotation.Entities.ConditionText)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(ConditionTextEntidad);
            return condText;
        }

        public string GenerateFile(List<ConditionTextModel> list, string fileName)
        {
            try
            {

                FileProcessValue fileProcessValue = new FileProcessValue();
                fileProcessValue.Key1 = (int)FileProcessType.ParametrizationTextPrecatalogued;

                File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ConditionTextModel citylist in list)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();

                        fields[0].Value = citylist.ConditionTextId.ToString();
                        fields[1].Value = citylist.TextTitle.ToString();
                        fields[2].Value = citylist.TextBody.ToString();

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    return DelegateService.utilitiesServiceCore.GenerateFile(file);
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        #endregion
        #endregion




    }
}