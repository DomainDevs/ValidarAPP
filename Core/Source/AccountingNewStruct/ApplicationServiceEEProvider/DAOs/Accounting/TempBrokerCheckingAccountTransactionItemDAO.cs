using System;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempBrokerCheckingAccountTransactionItemDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        ///<summary>
        /// SaveTempBrokerCheckingAccountTransactionItem
        /// </summary>
        /// <param name="brokersCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="collectCode"></param>
        /// <param name="tempBrokerParentCode"></param>
        /// <param name="accountingDate"></param>
        /// <returns>BrokersCheckingAccountTransactionItem</returns>
        public BrokersCheckingAccountTransactionItem SaveTempBrokerCheckingAccountTransactionItem(BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem, int tempImputationId, int tempBrokerParentCode, int collectCode, DateTime accountingDate)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity = EntityAssembler.CreateTempBrokerCheckingAccount(brokersCheckingAccountTransactionItem, tempImputationId, tempBrokerParentCode, collectCode, accountingDate);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempBrokerCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempBrokersCheckingAccountTransactionItem(tempBrokerCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempBrokerCheckingAccount
        /// </summary>
        /// <param name="brokersCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="tempBrokerParentCode"></param>
        /// <param name="collectCode"></param>
        /// <param name="accountingDate"></param>
        /// <returns>BrokersCheckingAccountTransactionItem</returns>
        public BrokersCheckingAccountTransactionItem UpdateTempBrokerCheckingAccount(BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem, int tempImputationId, int tempBrokerParentCode, int collectCode, DateTime accountingDate)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempBrokerCheckingAccTrans.CreatePrimaryKey(brokersCheckingAccountTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity = (ACCOUNTINGEN.TempBrokerCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempBrokerCheckingAccountEntity.TempApplicationCode = Convert.ToInt32(tempImputationId);
                tempBrokerCheckingAccountEntity.TempBrokerParentCode = Convert.ToInt32(tempBrokerParentCode); //0
                tempBrokerCheckingAccountEntity.AgentTypeCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Holder.FullName);
                tempBrokerCheckingAccountEntity.AgentId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Holder.IndividualId);
                tempBrokerCheckingAccountEntity.AgentAgencyId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Agencies[0].Id);
                tempBrokerCheckingAccountEntity.BranchCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Branch.Id);
                tempBrokerCheckingAccountEntity.SalePointCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.SalePoint.Id);
                tempBrokerCheckingAccountEntity.AccountingCompanyCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Company.IndividualId);
                tempBrokerCheckingAccountEntity.AccountingDate = Convert.ToDateTime(accountingDate);
                tempBrokerCheckingAccountEntity.AccountingNature = brokersCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2;
                tempBrokerCheckingAccountEntity.CheckingAccountConceptCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.CheckingAccountConcept.Id);
                tempBrokerCheckingAccountEntity.CurrencyCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Amount.Currency.Id);
                tempBrokerCheckingAccountEntity.ExchangeRate = Convert.ToDecimal(brokersCheckingAccountTransactionItem.ExchangeRate.SellAmount);
                tempBrokerCheckingAccountEntity.IncomeAmount = Convert.ToDecimal(brokersCheckingAccountTransactionItem.Amount.Value);
                tempBrokerCheckingAccountEntity.Amount = Convert.ToDecimal(brokersCheckingAccountTransactionItem.Amount.Value * brokersCheckingAccountTransactionItem.ExchangeRate.SellAmount);
                tempBrokerCheckingAccountEntity.Description = brokersCheckingAccountTransactionItem.Comments;
                tempBrokerCheckingAccountEntity.TransactionNumber = Convert.ToInt32(collectCode);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempBrokerCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempBrokersCheckingAccountTransactionItem(tempBrokerCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempBrokersCheckingAccountTotal
        /// </summary>
        /// <param name="tempBrokerCheckingAccountCode"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>BrokersCheckingAccountTransaction</returns>
        public BrokersCheckingAccountTransaction UpdateTempBrokersCheckingAccountTotal(int tempBrokerCheckingAccountCode, decimal selectedTotal)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempBrokerCheckingAccTrans.CreatePrimaryKey(tempBrokerCheckingAccountCode);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity = (ACCOUNTINGEN.TempBrokerCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempBrokerCheckingAccountEntity.IncomeAmount = selectedTotal;
                tempBrokerCheckingAccountEntity.Amount = tempBrokerCheckingAccountEntity.ExchangeRate * selectedTotal;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempBrokerCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempBrokersCheckingAccountTransaction(tempBrokerCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempBrokerCheckingAccountTransactionItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempBrokerCheckingAccountTransactionItem(int tempTransactionItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempBrokerCheckingAccTrans.CreatePrimaryKey(tempTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity = (ACCOUNTINGEN.TempBrokerCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempBrokerCheckingAccountEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempBrokerCheckingAccountByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempBrokerCheckingAccountByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(ACCOUNTINGEN.TempBrokerCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempBrokerCheckingAccTrans>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempBrokerCheckingAccountEntity);
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #region TempBrokerCheckingAccountItem

        ///<summary>
        /// SaveTempBrokerCheckingAccountItem
        /// </summary>
        /// <param name="brokerCheckingAccountItem"></param>
        /// <returns>BrokerCheckingAccountItem</returns>
        public BrokerCheckingAccountItem SaveTempBrokerCheckingAccountItem(BrokerCheckingAccountItem brokerCheckingAccountItem)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItemEntity = EntityAssembler.CreateTempBrokerCheckingAccountItem(brokerCheckingAccountItem);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempBrokerCheckingAccountItemEntity);

                // Return del model
                return ModelAssembler.CreateTempBrokerCheckingAccountItem(tempBrokerCheckingAccountItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempBrokerCheckingAccountItem
        /// </summary>
        /// <param name="brokerCheckingAccountItem"></param>
        /// <returns>BrokerCheckingAccountItem</returns>
        public BrokerCheckingAccountItem UpdateTempBrokerCheckingAccountItem(BrokerCheckingAccountItem brokerCheckingAccountItem)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempBrokerCheckingAccountItem.CreatePrimaryKey(brokerCheckingAccountItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItemEntity = (ACCOUNTINGEN.TempBrokerCheckingAccountItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempBrokerCheckingAccountItemEntity.BrokerCheckingAccountCode = brokerCheckingAccountItem.BrokerCheckingAccountId;
                tempBrokerCheckingAccountItemEntity.TempBrokerCheckingAccTransCode = brokerCheckingAccountItem.TempBrokerCheckingAccountId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempBrokerCheckingAccountItemEntity);

                // Return del model
                return ModelAssembler.CreateTempBrokerCheckingAccountItem(tempBrokerCheckingAccountItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempBrokerCheckingAccountItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempBrokerCheckingAccountItem(int tempTransactionItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempBrokerCheckingAccountItem.CreatePrimaryKey(tempTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItemEntity = (ACCOUNTINGEN.TempBrokerCheckingAccountItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempBrokerCheckingAccountItemEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempBrokerCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempBrokerCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempBrokerCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempBrokerCheckingAccTrans>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccountItem.Properties.TempBrokerCheckingAccTransCode, tempBrokerCheckingAccountEntity.TempBrokerCheckingAccTransCode);

                    BusinessCollection businessCollectionItems = new
                        BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                            typeof(ACCOUNTINGEN.TempBrokerCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItemEntity in businessCollectionItems.OfType<ACCOUNTINGEN.TempBrokerCheckingAccountItem>())
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(tempBrokerCheckingAccountItemEntity);
                    }
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
