//System
using System;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempCoinsuranceCheckingAccountTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveTempCoinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="tempCoinsuranceParentCode"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionItem</returns>
        public CoInsuranceCheckingAccountTransactionItem SaveTempCoinsuranceCheckingAccountTransactionItem(CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem, int imputationId, int tempCoinsuranceParentCode)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity = EntityAssembler.CreateTempCoinsuranceCheckingAccount(coinsuranceCheckingAccountTransactionItem, imputationId, tempCoinsuranceParentCode);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempCoinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempCoinsuranceCheckingAccountTransactionItem(tempCoinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="tempCoinsuranceParentCode"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionItem</returns>
        public CoInsuranceCheckingAccountTransactionItem UpdateTempCoinsuranceCheckingAccount(CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem, int imputationId, int tempCoinsuranceParentCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempCoinsCheckingAccTrans.CreatePrimaryKey(coinsuranceCheckingAccountTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempCoinsCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempCoinsuranceCheckingAccountEntity.TempApplicationCode = imputationId;
                tempCoinsuranceCheckingAccountEntity.TempCoinsuranceParentCode = tempCoinsuranceParentCode; //0
                tempCoinsuranceCheckingAccountEntity.BranchCode = coinsuranceCheckingAccountTransactionItem.Branch.Id;
                tempCoinsuranceCheckingAccountEntity.AccountingCompanyCode = coinsuranceCheckingAccountTransactionItem.Company.IndividualId;
                tempCoinsuranceCheckingAccountEntity.SalePointCode = coinsuranceCheckingAccountTransactionItem.SalePoint.Id;
                tempCoinsuranceCheckingAccountEntity.CoinsuranceType =
                    coinsuranceCheckingAccountTransactionItem.CoInsuranceType == CoInsuranceTypes.Accepted ? 1 : 2;
                tempCoinsuranceCheckingAccountEntity.CoinsuredCompanyId = coinsuranceCheckingAccountTransactionItem.Holder.IndividualId;
                tempCoinsuranceCheckingAccountEntity.CheckingAccountConceptCode = coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept.Id;
                tempCoinsuranceCheckingAccountEntity.CurrencyCode = coinsuranceCheckingAccountTransactionItem.Amount.Currency.Id;
                tempCoinsuranceCheckingAccountEntity.ExchangeRate = coinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount;
                tempCoinsuranceCheckingAccountEntity.IncomeAmount = coinsuranceCheckingAccountTransactionItem.Amount.Value;
                tempCoinsuranceCheckingAccountEntity.Amount = coinsuranceCheckingAccountTransactionItem.Amount.Value * coinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount;
                tempCoinsuranceCheckingAccountEntity.Description = coinsuranceCheckingAccountTransactionItem.Comments;
                tempCoinsuranceCheckingAccountEntity.AccountingNatureCode = coinsuranceCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2;
                tempCoinsuranceCheckingAccountEntity.IsPersisted = true;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempCoinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempCoinsuranceCheckingAccountTransactionItem(tempCoinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempCoinsuranceCheckingAccountTotal
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccountCode"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>CoInsuranceCheckingAccountTransaction</returns>
        public CoInsuranceCheckingAccountTransaction UpdateTempCoinsuranceCheckingAccountTotal(int tempCoinsuranceCheckingAccountCode, decimal selectedTotal)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempCoinsCheckingAccTrans.CreatePrimaryKey(tempCoinsuranceCheckingAccountCode);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempCoinsCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempCoinsuranceCheckingAccountEntity.IncomeAmount = selectedTotal;
                tempCoinsuranceCheckingAccountEntity.Amount = tempCoinsuranceCheckingAccountEntity.ExchangeRate * selectedTotal;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempCoinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempCoinsuranceCheckingAccountTransaction(tempCoinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempCoinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempCoinsuranceCheckingAccountTransactionItem(int tempTransactionItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempCoinsCheckingAccTrans.CreatePrimaryKey(tempTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempCoinsCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempCoinsuranceCheckingAccountEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempCoinsuranceCheckingAccountByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempCoinsuranceCheckingAccountByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempCoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempCoinsCheckingAccTrans>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempCoinsuranceCheckingAccountEntity);
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #region TempCoinsuranceCheckingAccountItem

        ///<summary>
        /// SaveTempCoinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="coinsuranceCheckingAccountItem"></param>
        /// <returns>CoInsuranceCheckingAccountItem</returns>
        public CoInsuranceCheckingAccountItem SaveTempCoinsuranceCheckingAccountItem(CoInsuranceCheckingAccountItem coinsuranceCheckingAccountItem)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItemEntity = EntityAssembler.CreateTempCoinsuranceCheckingAccountItem(coinsuranceCheckingAccountItem);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempCoinsuranceCheckingAccountItemEntity);

                // Return del model
                return ModelAssembler.CreateTempCoinsuranceCheckingAccountItem(tempCoinsuranceCheckingAccountItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempCoinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="coinsuranceCheckingAccountItem"></param>
        /// <returns>CoInsuranceCheckingAccountItem</returns>
        public CoInsuranceCheckingAccountItem UpdateTempCoinsuranceCheckingAccountItem(CoInsuranceCheckingAccountItem coinsuranceCheckingAccountItem)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem.CreatePrimaryKey(coinsuranceCheckingAccountItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItemEntity = (ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempCoinsuranceCheckingAccountItemEntity.CoinsuranceCheckingAccountCode = coinsuranceCheckingAccountItem.CoinsuranceCheckingAccountId;
                tempCoinsuranceCheckingAccountItemEntity.TempCoinsCheckingAccTransCode = coinsuranceCheckingAccountItem.TempCoinsuranceCheckingAccountId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempCoinsuranceCheckingAccountItemEntity);

                // Return del model
                return ModelAssembler.CreateTempCoinsuranceCheckingAccountItem(tempCoinsuranceCheckingAccountItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempCoinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempCoinsuranceCheckingAccountItem(int tempTransactionItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem.CreatePrimaryKey(tempTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItemEntity = (ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempCoinsuranceCheckingAccountItemEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempCoinsuranceCheckingAccountItemByTempImputationId
        /// bcardenas
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempCoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempCoinsCheckingAccTrans>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem.Properties.TempCoinsCheckingAccTransCode, tempCoinsuranceCheckingAccountEntity.TempCoinsCheckingAccTransCode);

                    BusinessCollection businessCollectionItems = new
                        BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItemEntity in businessCollectionItems.OfType<ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem>())
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(tempCoinsuranceCheckingAccountItemEntity);
                    }
                }

                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        #endregion

    }
}
