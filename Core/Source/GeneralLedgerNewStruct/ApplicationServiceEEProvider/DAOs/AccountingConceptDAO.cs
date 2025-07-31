#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GLEN = Sistran.Core.Application.GeneralLedger.Entities;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.Helper;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class AccountingConceptDAO
    {
        #region Constants


        #endregion Constants

        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns></returns>
        public AccountingConcept SaveAccountingConcept(AccountingConcept accountingConcept)
        {
            try
            {
                // recuperar todos los registros
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GLEN.AccountingConcept)));

                var accountingConceptId = 0;
                if (businessCollection.Count > 0)
                {
                    foreach (GLEN.AccountingConcept accountingConceptEntityFind in businessCollection.OfType<GLEN.AccountingConcept>())
                    {
                        accountingConceptId = Convert.ToInt32(accountingConceptEntityFind.AccountingConceptCode);
                    }
                    accountingConceptId++;
                }
                else
                {
                    accountingConceptId = 1;
                }
                accountingConcept.Id = accountingConceptId;

                // Convertir de model a entity
                GLEN.AccountingConcept accountingConceptEntity = EntityAssembler.CreateAccountingConcept(accountingConcept);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingConceptEntity);

                // Return del model
                return ModelAssembler.CreateAccountingConcept(accountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns></returns>
        public AccountingConcept UpdateAccountingConcept(AccountingConcept accountingConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GLEN.AccountingConcept.CreatePrimaryKey(accountingConcept.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GLEN.AccountingConcept accountingConceptEntity = (GLEN.AccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                int? accountingAccountId = null;
                if (accountingConcept.AccountingAccount.AccountingAccountId == 0)
                {
                    accountingAccountId = null;
                }
                else
                {
                    accountingAccountId = accountingConcept.AccountingAccount.AccountingAccountId;
                }

                accountingConceptEntity.AccountingAccountId = accountingAccountId;
                accountingConceptEntity.AgentEnabled = accountingConcept.AgentEnabled;
                accountingConceptEntity.CoinsuranceEnabled = accountingConcept.CoInsurancedEnabled;
                accountingConceptEntity.Description = accountingConcept.Description;
                accountingConceptEntity.InsuredEnabled = accountingConcept.InsuredEnabled;
                accountingConceptEntity.ItemEnabled = accountingConcept.ItemEnabled;
                accountingConceptEntity.ReinsuranceEnabled = accountingConcept.ReInsuranceEnabled;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingConceptEntity);

                // Return del model
                return ModelAssembler.CreateAccountingConcept(accountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns></returns>
        public bool DeleteAccountingConcept(AccountingConcept accountingConcept)
        {
            bool isDeleted = false;

            try
            {   // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GLEN.AccountingConcept.CreatePrimaryKey(accountingConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GLEN.AccountingConcept accountingConceptEntity = (GLEN.AccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingConceptEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns></returns>        
        public AccountingConcept GetAccountingConcept(AccountingConcept accountingConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GLEN.AccountingConcept.CreatePrimaryKey(accountingConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GLEN.AccountingConcept accountingConceptEntity = (GLEN.AccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingConcept(accountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<AccountingConcept> GetAccountingConceptsByBranchId(int branchId)
        {
            try
            {
                List<AccountingConcept> accountingConcepts = new List<AccountingConcept>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(GLEN.BranchAccountingConcept.Properties.BranchCode, typeof(GLEN.BranchAccountingConcept).Name);
                filter.Equal();
                filter.Constant(branchId);

                BranchAccountingConceptView branchAccountingConceptView = new BranchAccountingConceptView();
                ViewBuilder viewBuilder = new ViewBuilder("BranchAccountingConceptView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, branchAccountingConceptView);

                if (branchAccountingConceptView.AccountingConcepts.Count > 0)
                {
                    accountingConcepts = ModelAssembler.CreateAccountingConcepts(branchAccountingConceptView.AccountingConcepts);
                }

                return accountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingConcepts
        /// </summary>
        /// <returns></returns>
        public List<AccountingConcept> GetAccountingConcepts()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GLEN.AccountingConcept)));

                // Return como Lista
                return ModelAssembler.CreateAccountingConcepts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingConceptsByCriteria
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<AccountingConcept> GetAccountingConceptsByCriteria(int userId, int branchId, int individualId)
        {
            try
            {
                List<AccountingConcept> accountingConcepts = GetAccountingConceptsByCriteriaByUserIdBranchIdIndividualId(userId, branchId, individualId);
                List<AccountingConcept> resultAccountingConcepts = new List<AccountingConcept>();
                accountingConcepts.ForEach(x =>
                {
                    resultAccountingConcepts.Add(GetAccountingConcept(x));
                });
                return resultAccountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingConceptsByCriteria
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<AccountingConcept> GetAccountingConceptsByCriteria(AccountingAccountFilter accountingAccountFilter)
        {
            try
            {
                List<AccountingConcept> accountingConcepts = new List<AccountingConcept>();
                List<AccountingAccount> accountingAccounts = new List<AccountingAccount>();

                if (accountingAccountFilter.IndividualId > 0)
                {
                    int rows;
                    // se obtiene los codigos de conceptos contables por proveeedor
                    ObjectCriteriaBuilder criteriaBuilderInd = new ObjectCriteriaBuilder();
                    criteriaBuilderInd.PropertyEquals(GLEN.ProviderAccountingConcept.Properties.IndividualId, accountingAccountFilter.IndividualId);

                    UIView viewAccountingConcept = _dataFacadeManager.GetDataFacade().GetView("ProviderAccountingConceptView",
                                                       criteriaBuilderInd.GetPredicate(), null, 0, 2147483647, null, true, out rows);

                    if (viewAccountingConcept.Count > 0)
                    {
                        foreach (DataRow dataRow in viewAccountingConcept)
                        {
                            accountingConcepts.Add(GetAccountingConcept(new AccountingConcept() { Id = Convert.ToInt32(dataRow["AccountingConceptCode"]) }));
                        }
                    }
                }
                else
                {
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.Property(GLEN.UserBranchAccountingConcept.Properties.UserCode, "UBAC");
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(accountingAccountFilter.UserId);
                    
                    SelectQuery selectQuery = new SelectQuery();
                    if (accountingAccountFilter.BranchId > 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(GLEN.BranchAccountingConcept.Properties.BranchCode, "BAC");
                        criteriaBuilder.Equal();
                        criteriaBuilder.Constant(accountingAccountFilter.BranchId);
                    }

                    if (accountingAccountFilter.ConceptId > 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(GLEN.BranchAccountingConcept.Properties.AccountingConceptCode, "BAC");
                        criteriaBuilder.Like();
                        criteriaBuilder.Constant(Convert.ToString(accountingAccountFilter.ConceptId) + "%");

                        selectQuery.AddSortValue(new SortValue(new Column(GLEN.BranchAccountingConcept.Properties.AccountingConceptCode, "BAC"), SortOrderType.Ascending));
                    }

                    if ((accountingAccountFilter.ConceptDescription != null
                            && accountingAccountFilter.ConceptDescription != ""))
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(GLEN.AccountingConcept.Properties.Description, "AC");
                        criteriaBuilder.Like();
                        criteriaBuilder.Constant(Convert.ToString(accountingAccountFilter.ConceptDescription) + "%");

                        selectQuery.AddSortValue(new SortValue(new Column(GLEN.AccountingConcept.Properties.Description, "AC"), SortOrderType.Ascending));
                    }

                    if ((accountingAccountFilter.AccountingDescription != null
                        && accountingAccountFilter.AccountingDescription != ""))
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(GLEN.AccountingAccount.Properties.AccountName, "AA");
                        criteriaBuilder.Like();
                        criteriaBuilder.Constant(Convert.ToString(accountingAccountFilter.AccountingDescription) + "%");

                        selectQuery.AddSortValue(new SortValue(new Column(GLEN.AccountingAccount.Properties.AccountName, "AA"), SortOrderType.Ascending));
                    }

                    if ((accountingAccountFilter.AccountingNumber != null
                        && accountingAccountFilter.AccountingNumber != ""))
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(GLEN.AccountingAccount.Properties.AccountNumber, "AA");
                        criteriaBuilder.Like();
                        criteriaBuilder.Constant(Convert.ToString(accountingAccountFilter.AccountingNumber) + "%");

                        selectQuery.AddSortValue(new SortValue(new Column(GLEN.AccountingAccount.Properties.AccountNumber, "AA"), SortOrderType.Ascending));
                    }

                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.AccountingConceptCode, "AC"), "AccountingConceptCode"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.Description, "AC"), "Description"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingAccountId, "AA"), "AccountingAccountId"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountNumber, "AA"), "AccountNumber"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountName, "AA"), "AccountName"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingNature, "AA"), "AccountingNature"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.DefaultCurrencyCode, "AA"), "DefaultCurrencyCode"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsMulticurrency, "AA"), "IsMulticurrency"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RequireAnalysis, "AA"), "RequireAnalysis"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RequireCostCenter, "AA"), "RequireCostCenter"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AnalysisId, "AA"), "AnalysisId"));

                    Join join = new Join(new ClassNameTable(typeof(GLEN.UserBranchAccountingConcept), "UBAC"), new ClassNameTable(typeof(GLEN.BranchAccountingConcept), "BAC"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(GLEN.UserBranchAccountingConcept.Properties.BranchAccountingConceptId, "UBAC")
                        .Equal()
                        .Property(GLEN.BranchAccountingConcept.Properties.BranchAccountingConceptId, "BAC")
                        .GetPredicate());

                    join = new Join(join, new ClassNameTable(typeof(GLEN.AccountingConcept), "AC"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(GLEN.BranchAccountingConcept.Properties.AccountingConceptCode, "BAC")
                        .Equal()
                        .Property(GLEN.AccountingConcept.Properties.AccountingConceptCode, "AC")
                        .GetPredicate());

                    join = new Join(join, new ClassNameTable(typeof(GLEN.AccountingAccount), "AA"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(GLEN.AccountingConcept.Properties.AccountingAccountId, "AC")
                        .Equal()
                        .Property(GLEN.AccountingAccount.Properties.AccountingAccountId, "AA")
                        .GetPredicate());

                    selectQuery.Table = join;
                    selectQuery.Where = criteriaBuilder.GetPredicate();
                    selectQuery.MaxRows = 10;

                    using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                    {
                        while (reader.Read())
                        {
                            accountingConcepts.Add(new AccountingConcept()
                            {
                                AccountingAccount = new AccountingAccount()
                                {
                                    AccountingAccountId = Convert.ToInt32(reader["AccountingAccountId"].ToString()),
                                    Number = Convert.ToString(reader["AccountNumber"].ToString()),
                                    AccountingNature = (AccountingNatures)Convert.ToInt32(reader["AccountingNature"].ToString()),
                                    CurrencyId = (reader["DefaultCurrencyCode"] == null)
                                        ? -1 : Convert.ToInt32(reader["DefaultCurrencyCode"].ToString()),
                                    Description = Convert.ToString(reader["AccountName"].ToString()),
                                    RequiresAnalysis = Convert.ToBoolean(reader["RequireAnalysis"].ToString()),
                                    RequiresCostCenter = Convert.ToBoolean(reader["RequireCostCenter"].ToString()),
                                    MultiCurrency = Convert.ToBoolean(reader["IsMulticurrency"].ToString()),
                                    Analysis = new Analysis() { 
                                        AnalysisId = (reader["AnalysisId"] == null)
                                        ? 0 : Convert.ToInt32(reader["AnalysisId"].ToString()),
                                    }
                                },
                                Id = Convert.ToInt32(reader["AccountingConceptCode"].ToString()),
                                Description = Convert.ToString(reader["Description"].ToString())
                            });
                        }
                    }
                }

                return accountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<AccountingConcept> GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int branchId, int movementTypeId, int personTypeId, int individualId)
        {
            try
            {
                List<AccountingConcept> accountingConcepts = new List<AccountingConcept>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(GLEN.BranchAccountingConcept.Properties.BranchCode, typeof(GLEN.BranchAccountingConcept).Name);
                filter.Equal();
                filter.Constant(branchId);
                filter.And();
                filter.Property(GLEN.BranchAccountingConcept.Properties.MovementTypeCode, typeof(GLEN.BranchAccountingConcept).Name);
                filter.Equal();
                filter.Constant(movementTypeId);

                if (personTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.CLM_PERSON_TYPE_PROVIDER)))
                {
                    filter.And();
                    filter.Property(UPEN.Supplier.Properties.IndividualId, typeof(UPEN.Supplier).Name);
                    filter.Equal();
                    filter.Constant(individualId);

                    SupplierBranchAccountingConceptView supplierBranchAccountingConceptView = new SupplierBranchAccountingConceptView();
                    ViewBuilder viewBuilder = new ViewBuilder("SupplierBranchAccountingConceptView");
                    viewBuilder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, supplierBranchAccountingConceptView);

                    if (supplierBranchAccountingConceptView.AccountingConcepts.Any())
                    {
                        accountingConcepts = ModelAssembler.CreateAccountingConcepts(supplierBranchAccountingConceptView.AccountingConcepts);
                    }
                }
                else
                {
                    BranchAccountingConceptView branchAccountingConceptView = new BranchAccountingConceptView();
                    ViewBuilder viewBuilder = new ViewBuilder("BranchAccountingConceptView");
                    viewBuilder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, branchAccountingConceptView);

                    if (branchAccountingConceptView.AccountingConcepts.Any())
                    {
                        accountingConcepts = ModelAssembler.CreateAccountingConcepts(branchAccountingConceptView.AccountingConcepts);
                    }
                }                

                return accountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
                throw;
            }
        }

        #endregion

        public List<AccountingConcept> GetAccountingConceptsByUserIdBranchIdIndividualId(int userId, int branchId, int individualId)
        {
            try
            {
                List<AccountingConcept> accountingConcepts = GetAccountingConceptsByCriteriaByUserIdBranchIdIndividualId(userId, branchId, individualId);
                return GetFullAccountingConcepts(accountingConcepts);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<AccountingConcept> GetFullAccountingConcepts(List<AccountingConcept> accountingConcepts)
        {
            try
            {
                if (accountingConcepts == null || accountingConcepts.Count == 0)
                    return accountingConcepts;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(GLEN.AccountingConcept.Properties.AccountingAccountId, typeof(GLEN.AccountingConcept).Name);
                filter.ListValue();
                accountingConcepts.ForEach(x =>
                {
                    filter.Constant(x.Id);
                });
                filter.EndList();
                
                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.AccountingConceptCode, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.AccountingConceptCode));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.AgentEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.AgentEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.CoinsuranceEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.CoinsuranceEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.Description, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.Description));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.InsuredEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.InsuredEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.ItemEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.ItemEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.ReinsuranceEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.ReinsuranceEnabled));

                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingAccountId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountingAccountId));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingNature, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountingNature));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountName, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountName));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountNumber, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountNumber));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AnalysisId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AnalysisId));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.DefaultBranchCode, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.DefaultBranchCode));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.DefaultCurrencyCode, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.DefaultCurrencyCode));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsMulticurrency, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsMulticurrency));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RequireAnalysis, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RequireAnalysis));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RequireCostCenter, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RequireCostCenter));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RevAcountingNeg, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RevAcountingNeg));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RevAcountingPos, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RevAcountingPos));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountApplication, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountApplication));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingAccountParentId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountingAccountParentId));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountTypeId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountTypeId));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.Comments, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.Comments));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsOfficialNomenclature, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsOfficialNomenclature));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsReclassify, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsReclassify));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsRevalue, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsRevalue));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RecAccounting, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RecAccounting));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsMultibranch, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsMultibranch));

                Join join = new Join(new ClassNameTable(typeof(GLEN.AccountingConcept), typeof(GLEN.AccountingConcept).Name), new ClassNameTable(typeof(GLEN.AccountingAccount), typeof(GLEN.AccountingAccount).Name), JoinType.Left);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(GLEN.AccountingConcept.Properties.AccountingAccountId, typeof(GLEN.AccountingConcept).Name)
                    .Equal()
                    .Property(GLEN.AccountingAccount.Properties.AccountingAccountId, typeof(GLEN.AccountingAccount).Name)
                    .GetPredicate());

                selectQuery.Table = join;
                selectQuery.Where = filter.GetPredicate();

                int conceptId;
                List<AccountingConcept> resultAccountingConcepts = new List<AccountingConcept>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        conceptId = Convert.ToInt32(reader[GLEN.AccountingConcept.Properties.AccountingConceptCode]);
                        if (resultAccountingConcepts.Count(x => x.Id == conceptId) == 0)
                        {
                            AccountingConcept accountingConcept = new AccountingConcept()
                            {
                                Id = Convert.ToInt32(reader[GLEN.AccountingConcept.Properties.AccountingConceptCode]),
                                AgentEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.AgentEnabled]),
                                CoInsurancedEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.CoinsuranceEnabled]),
                                Description = Convert.ToString(reader[GLEN.AccountingConcept.Properties.Description]),
                                InsuredEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.InsuredEnabled]),
                                ItemEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.ItemEnabled]),
                                ReInsuranceEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.ReinsuranceEnabled]),
                                AccountingAccount = new AccountingAccount() { }
                            };
                            if (reader[GLEN.AccountingAccount.Properties.AccountingAccountId] != null)
                            {
                                accountingConcept.AccountingAccount = new AccountingAccount()
                                {
                                    AccountingAccountId = Convert.ToInt32(reader[GLEN.AccountingConcept.Properties.AccountingAccountId]),
                                    Number = Convert.ToString(reader[GLEN.AccountingAccount.Properties.AccountNumber]),
                                    AccountingNature = (AccountingNatures)Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.AccountingNature]),
                                    MultiCurrency = Convert.ToBoolean(reader[GLEN.AccountingAccount.Properties.IsMulticurrency]),
                                    RequiresAnalysis = Convert.ToBoolean(reader[GLEN.AccountingAccount.Properties.RequireAnalysis]),
                                    RequiresCostCenter = Convert.ToBoolean(reader[GLEN.AccountingAccount.Properties.RequireCostCenter]),
                                    Description = Convert.ToString(reader[GLEN.AccountingAccount.Properties.AccountName])
                                };
                                if (reader[GLEN.AccountingAccount.Properties.AnalysisId] != null)
                                {
                                    accountingConcept.AccountingAccount.Analysis = new Analysis
                                    {
                                        AnalysisId = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.AnalysisId])
                                    };
                                }
                                if (reader[GLEN.AccountingAccount.Properties.DefaultBranchCode] != null)
                                {
                                    accountingConcept.AccountingAccount.Branch = new CommonService.Models.Branch()
                                    {
                                        Id = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.DefaultBranchCode])
                                    };
                                }
                                if (reader[GLEN.AccountingAccount.Properties.DefaultCurrencyCode] != null)
                                {
                                    accountingConcept.AccountingAccount.CurrencyId = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.DefaultCurrencyCode]);
                                    accountingConcept.AccountingAccount.Currency = new CommonService.Models.Currency
                                    {
                                        Id = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.DefaultCurrencyCode])
                                    };
                                }
                            }
                            resultAccountingConcepts.Add(accountingConcept);
                        }
                    }
                }
                return resultAccountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        private List<AccountingConcept> GetAccountingConceptsByCriteriaByUserIdBranchIdIndividualId(int userId, int branchId, int individualId)
        {
            try
            {
                int rows;

                List<AccountingConcept> accountingConcepts = new List<AccountingConcept>();

                if (individualId > 0)
                {
                    // se obtiene los codigos de conceptos contables por proveeedor
                    ObjectCriteriaBuilder criteriaBuilderInd = new ObjectCriteriaBuilder();
                    criteriaBuilderInd.PropertyEquals(GLEN.ProviderAccountingConcept.Properties.IndividualId, individualId);

                    UIView viewAccountingConcept = _dataFacadeManager.GetDataFacade().GetView("ProviderAccountingConceptView",
                                                       criteriaBuilderInd.GetPredicate(), null, 0, 2147483647, null, true, out rows);

                    if (viewAccountingConcept.Count > 0)
                        foreach (DataRow dataRow in viewAccountingConcept)
                        {
                            accountingConcepts.Add(new AccountingConcept() { Id = Convert.ToInt32(dataRow["AccountingConceptCode"]) });
                        }
                }
                else
                {
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GLEN.UserBranchAccountingConcept.Properties.UserCode, userId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GLEN.BranchAccountingConcept.Properties.BranchCode, branchId);

                    SelectQuery selectQuery = new SelectQuery();
                    selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.BranchAccountingConcept.Properties.AccountingConceptCode, "BAC"), GLEN.BranchAccountingConcept.Properties.AccountingConceptCode));

                    Join join = new Join(new ClassNameTable(typeof(GLEN.UserBranchAccountingConcept), "UBAC"), new ClassNameTable(typeof(GLEN.BranchAccountingConcept), "BAC"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(GLEN.UserBranchAccountingConcept.Properties.BranchAccountingConceptId, "UBAC")
                        .Equal()
                        .Property(GLEN.BranchAccountingConcept.Properties.BranchAccountingConceptId, "BAC")
                        .GetPredicate());

                    selectQuery.Table = join;
                    selectQuery.Where = criteriaBuilder.GetPredicate();

                    using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                    {
                        while (reader.Read())
                            accountingConcepts.Add(new AccountingConcept()
                            {
                                Id = Convert.ToInt32(reader[GLEN.BranchAccountingConcept.Properties.AccountingConceptCode])
                            });
                    }
                }
                return accountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        public List<AccountingConcept> GetLiteAccountingConcepts()
        {
            try
            {
                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.AccountingConceptCode, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.AccountingConceptCode));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.AgentEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.AgentEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.CoinsuranceEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.CoinsuranceEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.Description, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.Description));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.InsuredEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.InsuredEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.ItemEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.ItemEnabled));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingConcept.Properties.ReinsuranceEnabled, typeof(GLEN.AccountingConcept).Name), GLEN.AccountingConcept.Properties.ReinsuranceEnabled));

                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingAccountId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountingAccountId));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingNature, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountingNature));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountName, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountName));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountNumber, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountNumber));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AnalysisId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AnalysisId));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.DefaultBranchCode, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.DefaultBranchCode));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.DefaultCurrencyCode, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.DefaultCurrencyCode));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsMulticurrency, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsMulticurrency));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RequireAnalysis, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RequireAnalysis));
                selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RequireCostCenter, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RequireCostCenter));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RevAcountingNeg, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RevAcountingNeg));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RevAcountingPos, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RevAcountingPos));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountApplication, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountApplication));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountingAccountParentId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountingAccountParentId));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.AccountTypeId, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.AccountTypeId));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.Comments, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.Comments));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsOfficialNomenclature, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsOfficialNomenclature));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsReclassify, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsReclassify));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsRevalue, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsRevalue));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.RecAccounting, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.RecAccounting));
                //selectQuery.AddSelectValue(new SelectValue(new Column(GLEN.AccountingAccount.Properties.IsMultibranch, typeof(GLEN.AccountingAccount).Name), GLEN.AccountingAccount.Properties.IsMultibranch));

                Join join = new Join(new ClassNameTable(typeof(GLEN.AccountingConcept), typeof(GLEN.AccountingConcept).Name), new ClassNameTable(typeof(GLEN.AccountingAccount), typeof(GLEN.AccountingAccount).Name), JoinType.Left);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(GLEN.AccountingConcept.Properties.AccountingAccountId, typeof(GLEN.AccountingConcept).Name)
                    .Equal()
                    .Property(GLEN.AccountingAccount.Properties.AccountingAccountId, typeof(GLEN.AccountingAccount).Name)
                    .GetPredicate());

                selectQuery.Table = join;

                int conceptId;
                List<AccountingConcept> resultAccountingConcepts = new List<AccountingConcept>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        conceptId = Convert.ToInt32(reader[GLEN.AccountingConcept.Properties.AccountingConceptCode]);
                        if (resultAccountingConcepts.Count(x => x.Id == conceptId) == 0)
                        {
                            AccountingConcept accountingConcept = new AccountingConcept()
                            {
                                Id = Convert.ToInt32(reader[GLEN.AccountingConcept.Properties.AccountingConceptCode]),
                                AgentEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.AgentEnabled]),
                                CoInsurancedEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.CoinsuranceEnabled]),
                                Description = Convert.ToString(reader[GLEN.AccountingConcept.Properties.Description]),
                                InsuredEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.InsuredEnabled]),
                                ItemEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.ItemEnabled]),
                                ReInsuranceEnabled = Convert.ToBoolean(reader[GLEN.AccountingConcept.Properties.ReinsuranceEnabled]),
                                AccountingAccount = new AccountingAccount() { }
                            };
                            if (reader[GLEN.AccountingAccount.Properties.AccountingAccountId] != null)
                            {
                                accountingConcept.AccountingAccount = new AccountingAccount()
                                {
                                    AccountingAccountId = Convert.ToInt32(reader[GLEN.AccountingConcept.Properties.AccountingAccountId]),
                                    Number = Convert.ToString(reader[GLEN.AccountingAccount.Properties.AccountNumber]),
                                    AccountingNature = (AccountingNatures)Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.AccountingNature]),
                                    MultiCurrency = Convert.ToBoolean(reader[GLEN.AccountingAccount.Properties.IsMulticurrency]),
                                    RequiresAnalysis = Convert.ToBoolean(reader[GLEN.AccountingAccount.Properties.RequireAnalysis]),
                                    RequiresCostCenter = Convert.ToBoolean(reader[GLEN.AccountingAccount.Properties.RequireCostCenter]),
                                    Description = Convert.ToString(reader[GLEN.AccountingAccount.Properties.AccountName])
                                };
                                if (reader[GLEN.AccountingAccount.Properties.AnalysisId] != null)
                                {
                                    accountingConcept.AccountingAccount.Analysis = new Analysis
                                    {
                                        AnalysisId = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.AnalysisId])
                                    };
                                }
                                if (reader[GLEN.AccountingAccount.Properties.DefaultBranchCode] != null)
                                {
                                    accountingConcept.AccountingAccount.Branch = new CommonService.Models.Branch()
                                    {
                                        Id = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.DefaultBranchCode])
                                    };
                                }
                                if (reader[GLEN.AccountingAccount.Properties.DefaultCurrencyCode] != null)
                                {
                                    accountingConcept.AccountingAccount.CurrencyId = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.DefaultCurrencyCode]);
                                    accountingConcept.AccountingAccount.Currency = new CommonService.Models.Currency
                                    {
                                        Id = Convert.ToInt32(reader[GLEN.AccountingAccount.Properties.DefaultCurrencyCode])
                                    };
                                }
                            }
                            resultAccountingConcepts.Add(accountingConcept);
                        }
                    }
                }
                return resultAccountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
