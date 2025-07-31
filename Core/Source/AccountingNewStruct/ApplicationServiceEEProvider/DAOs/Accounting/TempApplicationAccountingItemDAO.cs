//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;

using System;
using System.Linq;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    class TempApplicationAccountingItemDAO
    {
        /// <summary>
        /// Guardar un movimiento contable
        /// </summary>
        /// <param name="applicationAccounting">Movimiento contable</param>
        /// <returns>Identificador de la inserción</returns>
        public int SaveTempApplicationAccounting(ApplicationAccounting applicationAccounting)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationAccounting entityTempApplicationAccounting = EntityAssembler.CreateTempApplicationAccounting(applicationAccounting);
                DataFacadeManager.Insert(entityTempApplicationAccounting);
                return entityTempApplicationAccounting.TempAppAccountingCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Actualizar un movimiento contable
        /// </summary>
        /// <param name="applicationAccounting">Movimiento contable</param>
        /// <returns>Identificador del registro</returns>
        public int UpdateTempApplicationAccounting(ApplicationAccounting applicationAccounting)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccounting.CreatePrimaryKey(applicationAccounting.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplicationAccounting entityTempApplicationAccounting = (ACCOUNTINGEN.TempApplicationAccounting)
                    DataFacadeManager.GetObject(primaryKey);

                EntityAssembler.CreateTempApplicationAccounting(applicationAccounting);

                entityTempApplicationAccounting.TempAppCode = applicationAccounting.ApplicationAccountingId;
                entityTempApplicationAccounting.BranchCode = applicationAccounting.Branch.Id;
                entityTempApplicationAccounting.SalePointCode = applicationAccounting.SalePoint.Id;
                entityTempApplicationAccounting.AccountingConceptCode = Convert.ToInt32(applicationAccounting.AccountingConcept.Id);
                entityTempApplicationAccounting.IndividualCode = applicationAccounting.Beneficiary.IndividualId;
                entityTempApplicationAccounting.AccountingAccountCode = applicationAccounting.BookAccount.Id;
                entityTempApplicationAccounting.AccountingNature = Convert.ToInt32(applicationAccounting.AccountingNature);
                entityTempApplicationAccounting.CurrencyCode = applicationAccounting.Amount.Currency.Id;
                entityTempApplicationAccounting.LocalAmount = applicationAccounting.LocalAmount.Value;
                entityTempApplicationAccounting.ExchangeRate = applicationAccounting.ExchangeRate.SellAmount;
                entityTempApplicationAccounting.Amount = applicationAccounting.Amount.Value;
                entityTempApplicationAccounting.Description = applicationAccounting.Description;
                entityTempApplicationAccounting.BankReconciliationCode = applicationAccounting.BankReconciliationId;
                entityTempApplicationAccounting.ReceiptNumber = applicationAccounting.ReceiptNumber;
                entityTempApplicationAccounting.ReceiptDate = applicationAccounting.ReceiptDate;

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Update(entityTempApplicationAccounting);

                // Return del model
                return entityTempApplicationAccounting.TempAppAccountingCode;
            }
            catch (BusinessException)
            {
                return 0;
            }
        }

        /// <summary>
        /// DeleteTempDailyAccountingTransactionItem
        /// </summary>
        /// <param name="tempApplicationAccountingId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplicationAccounting(int tempApplicationAccountingId)
        {
            bool isDeleted = false;

            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccounting.CreatePrimaryKey(tempApplicationAccountingId);
                DataFacadeManager.Delete(primaryKey);
                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// DeleteTempDailyAccountingTransactionItemByTempImputationId
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplicationAccountingsByTempApplicationId(int tempApplicationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.TempAppCode, tempApplicationId);

                List<ACCOUNTINGEN.TempApplicationAccounting> entityTempApplicationAccounting = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationAccounting), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationAccounting>().ToList();

                if (entityTempApplicationAccounting != null && entityTempApplicationAccounting.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempApplicationAccounting tempApplicationAccounting in entityTempApplicationAccounting)
                    {
                        //se eliminan los códigos de análisis
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccountingAnalysis.Properties.TempAppAccountingCode, tempApplicationAccounting.TempAppAccountingCode);

                        List<ACCOUNTINGEN.TempApplicationAccountingAnalysis> entityAnalysisList = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationAccountingAnalysis), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationAccountingAnalysis>().ToList();

                        if (entityAnalysisList != null && entityAnalysisList.Count > 0)
                        {
                            foreach (ACCOUNTINGEN.TempApplicationAccountingAnalysis tempApplicationAccountingAnalysis in entityAnalysisList)
                            {
                                DataFacadeManager.Delete(tempApplicationAccountingAnalysis.PrimaryKey);
                            }
                        }

                        //se eliminan los centros de costos
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccountingCostCenter.Properties.TempAppAccountingCode, tempApplicationAccounting.TempAppAccountingCode);

                        List<ACCOUNTINGEN.TempApplicationAccountingCostCenter> entityCenterCostList = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationAccountingCostCenter), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationAccountingCostCenter>().ToList();

                        if (entityCenterCostList != null && entityCenterCostList.Count > 0)
                        {
                            foreach (ACCOUNTINGEN.TempApplicationAccountingCostCenter tempApplicationAccountingCostCenter in entityCenterCostList)
                            {
                                DataFacadeManager.Delete(tempApplicationAccountingCostCenter.PrimaryKey);
                            }
                        }
                        DataFacadeManager.Delete(tempApplicationAccounting.PrimaryKey);
                    }
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
        /// <param name="tempApplicationAccounting"></param>
        /// <returns></returns>
        public int UpdateTempApplicationAccountingAmount(ApplicationAccounting tempApplicationAccounting)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccounting.CreatePrimaryKey(tempApplicationAccounting.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplicationAccounting entityTempApplicationAccounting = (ACCOUNTINGEN.TempApplicationAccounting)
                    DataFacadeManager.GetObject(primaryKey);
                entityTempApplicationAccounting.Amount = tempApplicationAccounting.Amount.Value;
                entityTempApplicationAccounting.LocalAmount = tempApplicationAccounting.LocalAmount.Value;
                entityTempApplicationAccounting.ExchangeRate = tempApplicationAccounting.ExchangeRate.SellAmount;

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Update(entityTempApplicationAccounting);

                // Return del model
                return entityTempApplicationAccounting.TempAppAccountingCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
