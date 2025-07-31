//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using ReclassificationModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountReclassification;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class EntryReclassificationDAO
    {
        #region Instance Variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountReclassificationResult
        /// </summary>
        /// <param name="accountReclassificationResult"></param>
        /// <returns></returns>
        public ReclassificationModels.AccountReclassificationResult SaveAccountReclassificationResult(ReclassificationModels.AccountReclassificationResult accountReclassificationResult)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingReclassification accountingReclassificationEntity = EntityAssembler.CreateAccountingReclassification(accountReclassificationResult);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingReclassificationEntity);

                // Return del model
                return ModelAssembler.CreateAccountingReclassification(accountingReclassificationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLog</returns>
        public ProcessLog SaveProcessLog(ProcessLog processLog)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.ReclassificationLog reclassificationLogEntity = EntityAssembler.CreateReclassificationLog(processLog);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(reclassificationLogEntity);

                // Return del model
                return ModelAssembler.CreateReclassificationLog(reclassificationLogEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion

        #region Update

        /// <summary>
        /// UpdateProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns></returns>
        public ProcessLog UpdateProcessLog(ProcessLog processLog)
        {
            try
            {
                if (processLog.Id == -1)
                {
                    // Se obtiene el id del proceso dado el mes y año
                    processLog.Id = GetProcessLogIdByMonthYear(processLog.Month, processLog.Year);
                    processLog.ProcessLogStatus = ProcessLogStatus.Canceled;
                }

                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.ReclassificationLog.CreatePrimaryKey(processLog.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.ReclassificationLog reclassificationLogEntity = (GENERALLEDGEREN.ReclassificationLog)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                reclassificationLogEntity.EndDate = DateTime.Now;
                reclassificationLogEntity.ProcessStatus = Convert.ToInt32(processLog.ProcessLogStatus);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(reclassificationLogEntity);

                // Return del model
                return ModelAssembler.CreateReclassificationLog(reclassificationLogEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Get

        /// <summary>
        /// GetAccountReclassificationByMonthAndYear
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns></returns>
        public List<ReclassificationModels.AccountReclassificationResult> GetAccountingReclassificationByMonthAndYear(int month, int year)
        {
            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingReclassification.Properties.ProcessYear, year).And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingReclassification.Properties.ProcessMonth, month); 

                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingReclassification), criteriaBuilder.GetPredicate()));

                var reclassifications = ModelAssembler.CreateAccountingReclassifications(businessCollection);

                List<ReclassificationModels.AccountReclassificationResult> accountingReclassifications = new List<ReclassificationModels.AccountReclassificationResult>();

                foreach (ReclassificationModels.AccountReclassificationResult accountingReclassification in reclassifications)
                {
                    accountingReclassification.Analysis.AnalysisConcept.Description = GetAnalysisConceptDescriptionById(accountingReclassification.Analysis.AnalysisConcept.AnalysisConceptId);
                    accountingReclassification.Analysis.Description = GetAnalysisDescriptionById(accountingReclassification.Analysis.AnalysisId);
                    accountingReclassification.CostCenter.Description = GetCostCenterDescriptionById(accountingReclassification.CostCenter.CostCenterId);
                    accountingReclassification.DestinationAccountingAccount.Description = GetAccountingAccountById(accountingReclassification.DestinationAccountingAccount.AccountingAccountId).Description;
                    accountingReclassification.DestinationAccountingAccount.Number = GetAccountingAccountById(accountingReclassification.DestinationAccountingAccount.AccountingAccountId).Number;
                    accountingReclassification.SourceAccountingAccount.Description = GetAccountingAccountById(accountingReclassification.SourceAccountingAccount.AccountingAccountId).Description;
                    accountingReclassification.SourceAccountingAccount.Number = GetAccountingAccountById(accountingReclassification.SourceAccountingAccount.AccountingAccountId).Number;

                    accountingReclassifications.Add(accountingReclassification);
                }

                return accountingReclassifications;
            }
            catch
            {
                return new List<ReclassificationModels.AccountReclassificationResult>();
            }
        }

        /// <summary>
        /// GetAccountingReclassifications
        /// </summary>
        /// <returns></returns>
        public List<ReclassificationModels.AccountReclassificationResult> GetAccountingReclassifications()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingReclassification)));
                // Return como Lista
                return ModelAssembler.CreateAccountingReclassifications(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLog</returns>
        public ProcessLog GetProcessLog(ProcessLog processLog)
        {
            try
            {
                ProcessLog newProcessLog = new ProcessLog();

                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.ReclassificationLog.Properties.ProcessYear, processLog.Year).And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.ReclassificationLog.Properties.ProcessMonth, processLog.Month);

                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.ReclassificationLog), criteriaBuilder.GetPredicate()));
                
                if (businessCollection.Count > 0)
                {
                    // Return  como Lista
                    var processLogs = ModelAssembler.CreateReclassificationLogs(businessCollection);

                    foreach (ProcessLog processLogEntity in processLogs)
                    {
                        newProcessLog = processLogEntity;
                    }
                    return newProcessLog;
                }
                else
                {
                    return new ProcessLog();
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Borra la tabla temporal de generación de reclasificación de cuentas contables
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public void DeleteEntryReclassification(int month, int year)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingReclassification.Properties.ProcessMonth, month).And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingReclassification.Properties.ProcessYear, year);

                BusinessCollection collections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingReclassification), criteriaBuilder.GetPredicate()));

                if (collections.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingReclassification reclassificationEntity in collections.OfType<GENERALLEDGEREN.AccountingReclassification>())
                    {
                        PrimaryKey primaryKey = GENERALLEDGEREN.AccountingReclassification.CreatePrimaryKey(reclassificationEntity.AccountingReclassificationId);

                        // Realizar las operaciones con los entities utilizando DAF
                        GENERALLEDGEREN.AccountingReclassification accountingReclassificationEntity = (GENERALLEDGEREN.AccountingReclassification)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(accountingReclassificationEntity);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// GetAccountingAccountsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private AccountingAccount GetAccountingAccountById(int id)
        {
            AccountingAccount filteredAccountingAccount;

            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, id);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                List<AccountingAccount> accountingAccounts = new List<AccountingAccount>();

                foreach (GENERALLEDGEREN.AccountingAccount accountingAccountEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                {
                    Branch branch = new Branch();
                    branch.Id = Convert.ToInt32(accountingAccountEntity.DefaultBranchCode);

                    Currency currency = new Currency();
                    currency.Id = Convert.ToInt32(accountingAccountEntity.DefaultCurrencyCode);

                    Analysis analysis = new Analysis();
                    analysis.AnalysisId = Convert.ToInt32(accountingAccountEntity.AnalysisId);

                    accountingAccounts.Add(new AccountingAccount
                    {
                        AccountingAccountId = accountingAccountEntity.AccountingAccountId,
                        Number = accountingAccountEntity.AccountNumber,
                        Description = accountingAccountEntity.AccountName,
                        Branch = branch,
                        Currency = currency,
                        AccountingNature = (AccountingNatures)accountingAccountEntity.AccountingNature
                    });
                }

                filteredAccountingAccount = accountingAccounts[0];
            }
            catch
            {
                filteredAccountingAccount = new AccountingAccount();
            }

            return filteredAccountingAccount;
        }

        /// <summary>
        /// GetAnalysisDescriptionById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetAnalysisDescriptionById(int id)
        {
            string analysisDescription = "";

            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Analysis.Properties.AnalysisId, id);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.Analysis), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.Analysis analysisEntity in businessCollection.OfType<GENERALLEDGEREN.Analysis>())
                {
                    analysisDescription = analysisEntity.Description;
                }
            }
            catch
            {
                analysisDescription = "";
            }

            return analysisDescription;
        }

        /// <summary>
        /// GetAnalysisConceptDescriptionById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetAnalysisConceptDescriptionById(int id)
        {
            string analysisConceptDescription = "";

            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisConcept.Properties.AnalysisConceptId, id);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AnalysisConcept), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.AnalysisConcept analysisConceptEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisConcept>())
                {
                    analysisConceptDescription = analysisConceptEntity.Description;
                }
            }
            catch
            {
                analysisConceptDescription = "";
            }

            return analysisConceptDescription;
        }

        /// <summary>
        /// GetCostCenterDescriptionById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetCostCenterDescriptionById(int id)
        {
            string costCenterDescription = "";

            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenter.Properties.CostCenterId, id);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.CostCenter), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.CostCenter costCenterEntity in businessCollection.OfType<GENERALLEDGEREN.CostCenter>())
                {
                    costCenterDescription = costCenterEntity.Description;
                }
            }
            catch
            {
                costCenterDescription = "";
            }

            return costCenterDescription;
        }

        /// <summary>
        /// GetProcessLogIdByMonthYear
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private int GetProcessLogIdByMonthYear(int month, int year)
        {
            int processLogId = 0;

            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.ReclassificationLog.Properties.ProcessYear, year).And(); //año
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.ReclassificationLog.Properties.ProcessMonth, month);     //mes

                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.ReclassificationLog), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.ReclassificationLog reclassificationLogEntity in businessCollection.OfType<GENERALLEDGEREN.ReclassificationLog>())
                    {
                        processLogId = reclassificationLogEntity.ReclassificationLogId;
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return processLogId;
        }

        #endregion

    }
}
