//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempReinsuranceCheckingAccountTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        ///<summary>
        /// SaveTempReinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="tempReinsuranceParentCode"></param>
        /// <returns>ReInsuranceCheckingAccountTransactionItem</returns>
        public ReInsuranceCheckingAccountTransactionItem SaveTempReinsuranceCheckingAccountTransactionItem(ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItem, int imputationId, int tempReinsuranceParentCode)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity = EntityAssembler.CreateTempReinsuranceCheckingAccount(reinsuranceCheckingAccountTransactionItem, imputationId, tempReinsuranceParentCode);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempReinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempReinsuranceCheckingAccountTransactionItem(tempReinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempReinsuranceCheckingAccount
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="tempReinsuranceParentCode"></param>
        /// <returns>ReInsuranceCheckingAccountTransactionItem</returns>
        public ReInsuranceCheckingAccountTransactionItem UpdateTempReinsuranceCheckingAccount(ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItem, int imputationId, int tempReinsuranceParentCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempReinsCheckingAccTrans.CreatePrimaryKey(reinsuranceCheckingAccountTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempReinsCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempReinsuranceCheckingAccountEntity.AccountingCompanyCode = reinsuranceCheckingAccountTransactionItem.Company.IndividualId;
                tempReinsuranceCheckingAccountEntity.AccountingNature = reinsuranceCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2;
                tempReinsuranceCheckingAccountEntity.AgentId = reinsuranceCheckingAccountTransactionItem.Broker.IndividualId;
                tempReinsuranceCheckingAccountEntity.Amount = reinsuranceCheckingAccountTransactionItem.Amount.Value * reinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount;
                tempReinsuranceCheckingAccountEntity.ApplicationMonth = reinsuranceCheckingAccountTransactionItem.Month;
                tempReinsuranceCheckingAccountEntity.ApplicationYear = reinsuranceCheckingAccountTransactionItem.Year;
                tempReinsuranceCheckingAccountEntity.BranchCode = reinsuranceCheckingAccountTransactionItem.Branch.Id;
                tempReinsuranceCheckingAccountEntity.CheckingAccountConceptCode = reinsuranceCheckingAccountTransactionItem.CheckingAccountConcept.Id;
                tempReinsuranceCheckingAccountEntity.ContractCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.ContractNumber); // No es ContractNumber, es ContractId
                tempReinsuranceCheckingAccountEntity.CurrencyCode = reinsuranceCheckingAccountTransactionItem.Amount.Currency.Id;
                tempReinsuranceCheckingAccountEntity.Description = reinsuranceCheckingAccountTransactionItem.Comments;
                tempReinsuranceCheckingAccountEntity.EndorsementId = reinsuranceCheckingAccountTransactionItem.EndorsementId;
                tempReinsuranceCheckingAccountEntity.ExchangeRate = reinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount;
                tempReinsuranceCheckingAccountEntity.IncomeAmount = reinsuranceCheckingAccountTransactionItem.Amount.Value;
                tempReinsuranceCheckingAccountEntity.IsFacultative = reinsuranceCheckingAccountTransactionItem.IsFacultative;
                tempReinsuranceCheckingAccountEntity.IsPersisted = true;
                tempReinsuranceCheckingAccountEntity.LineBusinessCode = reinsuranceCheckingAccountTransactionItem.Prefix.Id;
                tempReinsuranceCheckingAccountEntity.Period = reinsuranceCheckingAccountTransactionItem.Period;
                tempReinsuranceCheckingAccountEntity.PolicyId = reinsuranceCheckingAccountTransactionItem.PolicyId;
                tempReinsuranceCheckingAccountEntity.Region = reinsuranceCheckingAccountTransactionItem.Region;
                tempReinsuranceCheckingAccountEntity.ReinsuranceCompanyId = reinsuranceCheckingAccountTransactionItem.Holder.IndividualId;
                tempReinsuranceCheckingAccountEntity.SalePointCode = reinsuranceCheckingAccountTransactionItem.SalePoint.Id;
                tempReinsuranceCheckingAccountEntity.Section = reinsuranceCheckingAccountTransactionItem.Section;
                tempReinsuranceCheckingAccountEntity.SlipNumber = reinsuranceCheckingAccountTransactionItem.SlipNumber;
                tempReinsuranceCheckingAccountEntity.SubLineBusinessCode = reinsuranceCheckingAccountTransactionItem.Prefix.LineBusinessId;
                tempReinsuranceCheckingAccountEntity.TempApplicationCode = imputationId;
                tempReinsuranceCheckingAccountEntity.TempReinsuranceParentCode = tempReinsuranceParentCode;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempReinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempReinsuranceCheckingAccountTransactionItem(tempReinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempReinsuranceCheckingAccountTotal
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccountCode"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>ReInsuranceCheckingAccountTransaction</returns>
        public ReInsuranceCheckingAccountTransaction UpdateTempReinsuranceCheckingAccountTotal(int tempReinsuranceCheckingAccountCode, decimal selectedTotal)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempReinsCheckingAccTrans.CreatePrimaryKey(tempReinsuranceCheckingAccountCode);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempReinsCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempReinsuranceCheckingAccountEntity.IncomeAmount = selectedTotal;
                tempReinsuranceCheckingAccountEntity.Amount = tempReinsuranceCheckingAccountEntity.ExchangeRate * selectedTotal;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempReinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateTempReinsuranceCheckingAccountTransaction(tempReinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempReinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempReinsuranceCheckingAccountTransactionItem(int tempTransactionItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempReinsCheckingAccTrans.CreatePrimaryKey(tempTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempReinsCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempReinsuranceCheckingAccountEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempReinsuranceCheckingAccountByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempReinsuranceCheckingAccountByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilde = new ObjectCriteriaBuilder();
                criteriaBuilde.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempReinsCheckingAccTrans), criteriaBuilde.GetPredicate()));

                foreach (ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempReinsCheckingAccTrans>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempReinsuranceCheckingAccountEntity);
                }

                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        #region TempReinsuranceCheckingAccountItem

        ///<summary>
        /// SaveTempReinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="reinsuranceCheckingAccountItem"></param>
        /// <returns>ReinsuranceCheckingAccountItem</returns>
        public ReinsuranceCheckingAccountItem SaveTempReinsuranceCheckingAccountItem(ReinsuranceCheckingAccountItem reinsuranceCheckingAccountItem)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItemEntity = EntityAssembler.CreateTempReinsuranceCheckingAccountItem(reinsuranceCheckingAccountItem);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempReinsuranceCheckingAccountItemEntity);

                // Return del model
                return ModelAssembler.CreateTempReinsuranceCheckingAccountItem(tempReinsuranceCheckingAccountItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempReinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="reinsuranceCheckingAccountItem"></param>
        /// <returns>ReinsuranceCheckingAccountItem</returns>
        public ReinsuranceCheckingAccountItem UpdateTempReinsuranceCheckingAccountItem(ReinsuranceCheckingAccountItem reinsuranceCheckingAccountItem)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempReinsuranceCheckingAccountItem.CreatePrimaryKey(reinsuranceCheckingAccountItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItemEntity = (ACCOUNTINGEN.TempReinsuranceCheckingAccountItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempReinsuranceCheckingAccountItemEntity.ReinsCheckingAccTransCode = reinsuranceCheckingAccountItem.ReinsuranceCheckingAccountId;
                tempReinsuranceCheckingAccountItemEntity.TempReinsCheckingAccTransCode = reinsuranceCheckingAccountItem.TempReinsuranceCheckingAccountId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempReinsuranceCheckingAccountItemEntity);

                // Return del model
                return ModelAssembler.CreateTempReinsuranceCheckingAccountItem(tempReinsuranceCheckingAccountItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempReinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempReinsuranceCheckingAccountItem(int tempTransactionItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempReinsuranceCheckingAccountItem.CreatePrimaryKey(tempTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItemEntity = (ACCOUNTINGEN.TempReinsuranceCheckingAccountItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempReinsuranceCheckingAccountItemEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempReinsuranceCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempReinsuranceCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempReinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempReinsCheckingAccTrans>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsuranceCheckingAccountItem.Properties.TempReinsCheckingAccTransCode, tempReinsuranceCheckingAccountEntity.TempReinsCheckingAccTransCode);

                    BusinessCollection businessCollectionItems = new
                        BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempReinsuranceCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItemEntity in businessCollectionItems.OfType<ACCOUNTINGEN.TempReinsuranceCheckingAccountItem>())
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(tempReinsuranceCheckingAccountItemEntity);
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

        #endregion
    }
}
