using System;
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Amortizations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CreditNotes;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{

    public class LogSpecialProcessDAO
    {
        #region Constants

        // Tipo de proceso utilizado para notas de crédito
        private const int ProcessTypeId = 1;

        #endregion

        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveLogSpecialProcess
        /// Graba la cabecera del proceso masivo de cruce automático de notas de crédito
        /// </summary>
        /// <param name="creditNoteProcess"></param>
        /// <param name="processTypeId"></param>
        /// <param name="tempImpuationId"></param>
        /// <returns>int</returns>
        public int SaveLogSpecialProcess(CreditNote creditNoteProcess, int processTypeId, int tempImpuationId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.LogSpecialProcess creditNoteEntity = EntityAssembler.
                    CreateLogSpecialProcess(creditNoteProcess, processTypeId, tempImpuationId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(creditNoteEntity);

                return creditNoteEntity.LogSpecialProcessId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveLogSpecialProcess
        /// </summary>
        /// <param name="amortization"></param>
        /// <param name="processTypeId"></param>
        /// <param name="tempImpuationId"></param>
        /// <returns></returns>
        public int SaveLogSpecialProcess(Amortization amortization, int processTypeId, int tempImpuationId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.LogSpecialProcess logSpecialProcessEntity = EntityAssembler.
                    CreateLogSpecialProcess(amortization, processTypeId, tempImpuationId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(logSpecialProcessEntity);

                return logSpecialProcessEntity.LogSpecialProcessId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateLogSpecialProcess
        /// Actualiza la cabecera del proceso masivo de cruce automático de notas de crédito
        /// </summary>
        /// <param name="creditNoteProcess"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="recordsProcessed"></param>
        /// <param name="endDate"></param>
        /// <param name="imputationId"></param>
        /// <param name="receiptNumber"></param>
        public void UpdateLogSpecialProcess(CreditNote creditNoteProcess, decimal exchangeRate,
                                             int recordsProcessed, DateTime? endDate, int imputationId, int receiptNumber)
        {
            try
            {
                _dataFacadeManager.GetDataFacade().ClearObjectCache();

                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.LogSpecialProcess.CreatePrimaryKey(creditNoteProcess.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.LogSpecialProcess logSpecialProcessEntity = (ACCOUNTINGEN.LogSpecialProcess)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (creditNoteProcess.CreditNoteStatus == CreditNoteStatus.Rejected)
                {
                    logSpecialProcessEntity.Status = (int)creditNoteProcess.CreditNoteStatus;
                    logSpecialProcessEntity.UserId = creditNoteProcess.UserId;
                    logSpecialProcessEntity.LowDate = endDate;
                }
                else if (creditNoteProcess.CreditNoteStatus == CreditNoteStatus.Applied)
                {
                    logSpecialProcessEntity.Status = (int)creditNoteProcess.CreditNoteStatus;
                    logSpecialProcessEntity.ImputationId = imputationId;
                    logSpecialProcessEntity.ImputationReceiptNumber = receiptNumber;
                }
                else if (creditNoteProcess.CreditNoteStatus == CreditNoteStatus.Actived && creditNoteProcess.NegativeAppliedTotal.Value != 0)
                {
                    logSpecialProcessEntity.IncomeAmount = logSpecialProcessEntity.IncomeAmount - creditNoteProcess.NegativeAppliedTotal.Value;
                    logSpecialProcessEntity.Amount = logSpecialProcessEntity.IncomeAmount * logSpecialProcessEntity.ExchangeRate;
                }
                else
                {
                    logSpecialProcessEntity.IncomeAmount = creditNoteProcess.PositiveAppliedTotal.Value;
                    logSpecialProcessEntity.Amount = creditNoteProcess.PositiveAppliedTotal.Value * exchangeRate;
                    logSpecialProcessEntity.ExchangeRate = exchangeRate;
                    logSpecialProcessEntity.EndDate = endDate;
                    logSpecialProcessEntity.Status = (int)creditNoteProcess.CreditNoteStatus;
                    logSpecialProcessEntity.RecordsProcessed = recordsProcessed;
                }

                // Falta la parte cuando ya se aplique

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(logSpecialProcessEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateLogSpecialProcess
        /// </summary>
        /// <param name="amortization"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="recordsProcessed"></param>
        /// <param name="endDate"></param>
        /// <param name="imputationId"></param>
        /// <param name="receiptNumber"></param>
        public void UpdateLogSpecialProcess(Amortization amortization, decimal exchangeRate,
                                             int recordsProcessed, DateTime? endDate, int imputationId, int receiptNumber)
        {
            try
            {
                _dataFacadeManager.GetDataFacade().ClearObjectCache();

                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.LogSpecialProcess.CreatePrimaryKey(amortization.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.LogSpecialProcess logSpecialProcessEntity = (ACCOUNTINGEN.LogSpecialProcess)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (amortization.AmortizationStatus == AmortizationStatus.Rejected)
                {
                    logSpecialProcessEntity.Status = (int)amortization.AmortizationStatus;
                    logSpecialProcessEntity.UserId = amortization.UserId;
                    logSpecialProcessEntity.LowDate = endDate;
                    logSpecialProcessEntity.EndDate = endDate;
                    logSpecialProcessEntity.IncomeAmount = 0;
                    logSpecialProcessEntity.Amount = logSpecialProcessEntity.IncomeAmount * logSpecialProcessEntity.ExchangeRate;
                }
                else if (amortization.AmortizationStatus == AmortizationStatus.Applied)
                {
                    logSpecialProcessEntity.Status = (int)amortization.AmortizationStatus;
                    logSpecialProcessEntity.ImputationId = imputationId;
                    logSpecialProcessEntity.ImputationReceiptNumber = receiptNumber;
                }
                else if (amortization.AmortizationStatus == AmortizationStatus.Actived && amortization.NegativeAppliedTotal.Value != 0)
                {
                    logSpecialProcessEntity.IncomeAmount = amortization.NegativeAppliedTotal.Value;
                    logSpecialProcessEntity.Amount = logSpecialProcessEntity.IncomeAmount * logSpecialProcessEntity.ExchangeRate;
                    logSpecialProcessEntity.TempImputationId = imputationId;
                }
                else
                {
                    logSpecialProcessEntity.Amount = amortization.PositiveAppliedTotal.Value * exchangeRate;
                    logSpecialProcessEntity.ExchangeRate = exchangeRate;
                    logSpecialProcessEntity.EndDate = endDate;
                    logSpecialProcessEntity.IncomeAmount = amortization.PositiveAppliedTotal.Value;
                    logSpecialProcessEntity.RecordsProcessed = recordsProcessed;
                    logSpecialProcessEntity.Status = (int)amortization.AmortizationStatus;
                    logSpecialProcessEntity.TempImputationId = imputationId;
                }

                // Falta la parte cuando ya se aplique

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(logSpecialProcessEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetLogSpecialProcess
        /// Obtiene la cabecera del proceso automático de cruce de notas de crédito
        /// </summary>
        /// <returns>List<CreditNote/></returns>
        public List<CreditNote> GetLogSpecialProcess()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader.Properties.LogSpecialProcessId);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader.Properties.ProcessTypeId, ProcessTypeId);

                BusinessCollection creditNoteCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(
                ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader), criteriaBuilder.GetPredicate()));

                // Return del model
                return ModelAssembler.CreateLogsSpecialProcess(creditNoteCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
