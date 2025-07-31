//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempDailyAccountingTransactionItemDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        ///<summary>
        /// SaveTempDailyAccountingTransactionItem
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        /// 
        public int SaveTempDailyAccountingTransactionItem(DailyAccountingTransactionItem tempDailyAccountingTransactionItem, int tempImputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountingEntity = EntityAssembler.CreateTempDailyAccountingTrans(tempDailyAccountingTransactionItem, tempImputationId, paymentConceptCode, description, bankReconciliationId, receiptNumber, receiptDate, postdatedAmount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempDailyAccountingEntity);

                // Return del model
                return tempDailyAccountingEntity.TempDailyAccountingTransId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempDailyAccountingTransactionItem
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        public int UpdateTempDailyAccountingTransactionItem(DailyAccountingTransactionItem tempDailyAccountingTransactionItem, int tempImputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            try
            {
                _dataFacadeManager.GetDataFacade().ClearObjectCache();

                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingTrans.CreatePrimaryKey(tempDailyAccountingTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountingEntity = (ACCOUNTINGEN.TempDailyAccountingTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempDailyAccountingEntity.TempDailyAccountingTransId = tempDailyAccountingTransactionItem.Id;
                tempDailyAccountingEntity.TempImputationCode = tempImputationId;
                tempDailyAccountingEntity.BranchCode = tempDailyAccountingTransactionItem.Branch.Id;
                tempDailyAccountingEntity.SalePointCode = tempDailyAccountingTransactionItem.SalePoint.Id;
                tempDailyAccountingEntity.CompanyCode = tempDailyAccountingTransactionItem.Company.IndividualId;
                tempDailyAccountingEntity.PaymentConceptCode = paymentConceptCode;
                tempDailyAccountingEntity.BeneficiaryId = tempDailyAccountingTransactionItem.Beneficiary.IndividualId;
                tempDailyAccountingEntity.BookAccountCode = tempDailyAccountingTransactionItem.BookAccount.Id;
                tempDailyAccountingEntity.AccountingNature = Convert.ToInt32(tempDailyAccountingTransactionItem.AccountingNature);
                tempDailyAccountingEntity.CurrencyCode = tempDailyAccountingTransactionItem.Amount.Currency.Id;
                tempDailyAccountingEntity.IncomeAmount = tempDailyAccountingTransactionItem.Amount.Value;
                tempDailyAccountingEntity.ExchangeRate = tempDailyAccountingTransactionItem.ExchangeRate.SellAmount;
                tempDailyAccountingEntity.Amount = tempDailyAccountingTransactionItem.LocalAmount.Value;
                tempDailyAccountingEntity.Description = description;
                tempDailyAccountingEntity.BankReconciliationId = bankReconciliationId;
                tempDailyAccountingEntity.ReceiptNumber = receiptNumber;
                tempDailyAccountingEntity.ReceiptDate = receiptDate;
                tempDailyAccountingEntity.PostdatedAmount = postdatedAmount;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempDailyAccountingEntity);

                // Return del model
                return tempDailyAccountingEntity.TempDailyAccountingTransId;
            }
            catch (BusinessException)
            {
                return 0;
            }
        }

        /// <summary>
        /// DeleteTempDailyAccountingTransactionItem
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempDailyAccountingTransactionItem(int tempDailyAccountingTransactionItemId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingTrans.CreatePrimaryKey(tempDailyAccountingTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountingEntity = (ACCOUNTINGEN.TempDailyAccountingTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempDailyAccountingEntity);
                isDeleted = true;
            }
            catch (BusinessException )
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// DeleteTempDailyAccountingTransactionItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempDailyAccountingTransactionItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempDailyAccountingTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountingEntity in businessCollection.OfType<ACCOUNTINGEN.TempDailyAccountingTrans>())
                {
                    //se eliminan los códigos de análisis
                    ObjectCriteriaBuilder analysisFilter = new ObjectCriteriaBuilder();
                    analysisFilter.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingAnalysis.Properties.TempDailyAccountingTransCode, tempDailyAccountingEntity.TempDailyAccountingTransId);

                    BusinessCollection analysisCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempDailyAccountingAnalysis), analysisFilter.GetPredicate()));

                    if (analysisCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.TempDailyAccountingAnalysis tempDailyAccountingAnalysisEntity in analysisCollection.OfType<ACCOUNTINGEN.TempDailyAccountingAnalysis>())
                        {
                            _dataFacadeManager.GetDataFacade().DeleteObject(tempDailyAccountingAnalysisEntity);
                        }
                    }

                    //se eliminan los centros de costos
                    ObjectCriteriaBuilder costCenterFilter = new ObjectCriteriaBuilder();
                    costCenterFilter.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingCostCenter.Properties.TempDailyAccountingTransCode, tempDailyAccountingEntity.TempDailyAccountingTransId);

                    BusinessCollection costCenterCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempDailyAccountingCostCenter), costCenterFilter.GetPredicate()));

                    if (costCenterCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.TempDailyAccountingCostCenter tempDailyAccountingCostCenterEntity in costCenterCollection.OfType<ACCOUNTINGEN.TempDailyAccountingCostCenter>())
                        {
                            _dataFacadeManager.GetDataFacade().DeleteObject(tempDailyAccountingCostCenterEntity);
                        }
                    }

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempDailyAccountingEntity);
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// UpdateTempDailyAccountingTransAmount
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItem"></param>
        /// <returns></returns>
        public int UpdateTempDailyAccountingTransAmount(DailyAccountingTransactionItem tempDailyAccountingTransactionItem)
        {
            try
            {
                _dataFacadeManager.GetDataFacade().ClearObjectCache();
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingTrans.CreatePrimaryKey(tempDailyAccountingTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountingEntity = (ACCOUNTINGEN.TempDailyAccountingTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                tempDailyAccountingEntity.IncomeAmount = tempDailyAccountingTransactionItem.Amount.Value;
                tempDailyAccountingEntity.Amount = tempDailyAccountingTransactionItem.LocalAmount.Value;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempDailyAccountingEntity);

                // Return del model
                return tempDailyAccountingEntity.TempDailyAccountingTransId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }



    }
}
