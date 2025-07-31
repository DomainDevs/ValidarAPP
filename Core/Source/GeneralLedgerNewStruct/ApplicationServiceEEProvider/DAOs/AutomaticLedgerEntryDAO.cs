//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Helper;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AutomaticLedgerEntryDAO
    {
        #region Class

        public class Summary
        {
            public int BranchId { get; set; }
            public int AccountingModuleId { get; set; }
            public int AccountingAccountId { get; set; }
            public int CurrencyId { get; set; }
            public int AccountingNature { get; set; }
            public int EntryNumber { get; set; }
            public decimal Amount { get; set; }
            public decimal LocalValue { get; set; }
            public decimal ExchangeRate { get; set; }
        }

        #endregion

        #region Constants


        #endregion Constants

        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private readonly LedgerEntryDAO _ledgerEntryDAO = new LedgerEntryDAO();
        
        private readonly EntryNumberDAO _entryNumberDAO = new EntryNumberDAO();
        private readonly AnalysisDAO _analysisDAO = new AnalysisDAO();
        private readonly PostDatedDAO _postDatedDAO = new PostDatedDAO();
        private readonly CostCenterEntryDAO _costCenterEntryDAO = new CostCenterEntryDAO();

        #endregion

        #region Public Methods

        /// <summary>
        /// SaveAutomaticLedgerEntry
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SaveAutomaticLedgerEntry(int moduleDateId, DateTime date, int userId)
        {
            int saved = 0;
            int ledgerEntryNumber = 0;
            int rows;

            try
            {
                // Primero borro registros mayorizados anteriormente y que sean asientos automáticos.
                DeleteAutomaticLedgerEntry(date.Year, date.Month, 2);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.SummaryJournalEntry.Properties.AccountingModuleId, moduleDateId).And();
                criteriaBuilder.Property(GENERALLEDGEREN.SummaryJournalEntry.Properties.Date);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(new DateTime(date.Year, date.Month, 1));
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.SummaryJournalEntry.Properties.Date);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(new DateTime(date.Year, date.Month, date.Day, 23, 59, 59));

                UIView viewSummaries = _dataFacadeManager.GetDataFacade().GetView("SummaryJournalEntryView", criteriaBuilder.GetPredicate(), null, 0, 2147483647, null, true, out rows);

                List<Summary> summaries = new List<Summary>();

                if (viewSummaries.Count > 0)
                {
                    foreach (DataRow dataRow in viewSummaries)
                    {
                        summaries.Add(new Summary()
                        {
                            AccountingAccountId = Convert.ToInt32(dataRow["AccountingAccountId"]),
                            AccountingModuleId = Convert.ToInt32(dataRow["AccountingModuleId"]),
                            AccountingNature = Convert.ToInt32(dataRow["AccountingNature"]),
                            Amount = Convert.ToDecimal(dataRow["TotalAmountValue"]),
                            BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                            CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                            EntryNumber = Convert.ToInt32(dataRow["EntryNumber"]),
                            ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                            LocalValue = Convert.ToDecimal(dataRow["TotalAmountLocalValue"])
                        });
                    }
                }

                //Se agrupa por sucursal, módulo y moneda
                var groupSummaries = from p in summaries
                                     group p by new
                                     {
                                         p.BranchId,
                                         p.AccountingModuleId,
                                         p.CurrencyId
                                     } into groupSummary
                                     select groupSummary;

                using (Context.Current)
                {
                    Transaction transaction = new Transaction();

                    try
                    {
                        foreach (var groupSummary in groupSummaries)
                        {
                            LedgerEntry ledgerEntry = new LedgerEntry();
                            ledgerEntry.AccountingCompany = new AccountingCompany()
                            {
                                AccountingCompanyId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_ACCOUNTING_COMPANY_BY_DEFAULT))
                            };
                            ledgerEntry.AccountingDate = date.Date;
                            ledgerEntry.AccountingMovementType = new AccountingMovementType()
                            {
                                AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_AUTOMATIC_ENTRIES))
                            };
                            ledgerEntry.Branch = new Branch() { Id = Convert.ToInt32(groupSummary.Key.BranchId) };
                            ledgerEntry.Description = "MAYORIZACIÓN DEL MES " + Convert.ToString(date.Month) + "/" + Convert.ToString(date.Year) +
                                                      " - " + moduleDateId;
                            ledgerEntry.EntryDestination = new EntryDestination()
                            {
                                DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_ENTRY_DESTINATION_BOTH))
                            };
                            ledgerEntry.EntryNumber = 0;
                            ledgerEntry.Id = 0;

                            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItem>();

                            foreach (var summary in groupSummary)
                            {
                                LedgerEntryItem ledgerEntryItem = new LedgerEntryItem();
                                ledgerEntryItem.Id = 0;
                                ledgerEntryItem.AccountingAccount = new AccountingAccount();
                                ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(summary.AccountingAccountId);
                                ledgerEntryItem.AccountingNature = Convert.ToInt32(summary.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? AccountingNatures.Credit : AccountingNatures.Debit;

                                ledgerEntryItem.Amount = new Amount() { Value = Convert.ToDecimal(summary.Amount) };
                                ledgerEntryItem.Amount.Currency = new Currency() { Id = Convert.ToInt32(summary.CurrencyId) };
                                ledgerEntryItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(summary.ExchangeRate) };
                                ledgerEntryItem.LocalAmount = new Amount() { Value = Convert.ToDecimal(summary.LocalValue) };
                                ledgerEntryItem.Analysis = new List<Analysis>();
                                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType();
                                ledgerEntryItem.ReconciliationMovementType.Id = 0;
                                ledgerEntryItem.CostCenters = new List<CostCenter>();
                                ledgerEntryItem.Currency = new Currency() { Id = Convert.ToInt32(summary.CurrencyId) };
                                ledgerEntryItem.Description = "MAYORIZACIÓN DEL MES " + Convert.ToString(date.Month) + "/" + Convert.ToString(date.Year) +
                                                              " - " + moduleDateId;
                                ledgerEntryItem.Individual = new Individual() { IndividualId = 0 };
                                ledgerEntryItem.PostDated = new List<PostDated>();
                                ledgerEntryItem.Receipt = new Receipt();
                                ledgerEntryItem.Receipt.Date = null;
                                ledgerEntryItem.Receipt.Number = 0;

                                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                            }

                            ledgerEntry.ModuleDateId = Convert.ToInt32(groupSummary.Key.AccountingModuleId);
                            ledgerEntry.RegisterDate = DateTime.Now;
                            ledgerEntry.SalePoint = new SalePoint() { Id = 0 };
                            ledgerEntry.UserId = userId;

                            saved = SaveLedgerEntryTransaction(ledgerEntry, true, ledgerEntryNumber);
                            ledgerEntryNumber = saved;
                        }

                        transaction.Complete();
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Dispose();

                        if (ex.Message.Contains("BusinessException"))
                        {
                            throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_BUSINESS_EXCEPTION_MSJ).ToString());
                        }

                        throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_UNHANDLED_EXCEPTION_MSJ).ToString());
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return saved;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// DeleteAutomaticLedgerEntry
        /// Elimina asientos de mayor marcados como CONTABILIDAD DIARIA
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="movementType"></param>
        private void DeleteAutomaticLedgerEntry(int year, int month, int movementType)
        {
            try
            {
                int numberOfDays = DateTime.DaysInMonth(year, month);
                string date = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
                date = date + " 23:59:59";

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(new DateTime(year, month, 1));
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(Convert.ToDateTime(date));

                if (movementType == 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_DAILY_ACCOUNTING)));
                }
                if (movementType == 1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId, movementType);
                }
                if (movementType == 2)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_AUTOMATIC_ENTRIES)));
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntry), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.LedgerEntry ledgerEntryEntity in businessCollection.OfType<GENERALLEDGEREN.LedgerEntry>())
                    {
                        // Se obtiene el detalle
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.LedgerEntryId, ledgerEntryEntity.LedgerEntryId);
                        if (movementType == 2)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.IsJournalEntry, 1); //indica que fue mayorizacíon de asientos.
                        }

                        BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntryItem), criteriaBuilder.GetPredicate()));

                        if (businessCollectionItems.Count > 0)
                        {
                            foreach (GENERALLEDGEREN.LedgerEntryItem ledgerEntryItemEntity in businessCollectionItems.OfType<GENERALLEDGEREN.LedgerEntryItem>())
                            {
                                _ledgerEntryDAO.DeleteLedgerEntryItem(ledgerEntryItemEntity.LedgerEntryItemId);
                            }

                            _ledgerEntryDAO.DeleteLedgerEntry(ledgerEntryEntity.LedgerEntryId);
                        }                        
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveLedgerEntryTransaction
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        private int SaveLedgerEntryTransaction(LedgerEntry ledgerEntry, bool isJournalEntry, int moduleLedgerEntryNumber)
        {
            // Se graba la cabecera
            int ledgerEntryId = 0;
            int ledgerEntryNumber = 0;
            int entryNumberId = 0;

            // Se obtiene el número de asiento
            if (moduleLedgerEntryNumber == 0)
            {
                EntryNumber entryNumber = new EntryNumber();
                entryNumber.AccountingMovementType = new AccountingMovementType()
                {
                    AccountingMovementTypeId = ledgerEntry.AccountingMovementType.AccountingMovementTypeId
                };
                entryNumber.Date = ledgerEntry.AccountingDate;
                entryNumber.EntryDestination = new EntryDestination()
                {
                    DestinationId = ledgerEntry.EntryDestination.DestinationId
                };
                entryNumber.Id = 0;
                entryNumber.IsJournalEntry = isJournalEntry;
                entryNumber.Number = 0;
                entryNumber.Year = ledgerEntry.AccountingDate.Year;

                entryNumber = _entryNumberDAO.GetLastEntryNumber(entryNumber);
                ledgerEntryNumber = Convert.ToInt32(entryNumber.Number);
                entryNumberId = Convert.ToInt32(entryNumber.Id);
            }
            else
            {
                ledgerEntryNumber = moduleLedgerEntryNumber;
            }

            ledgerEntry.EntryNumber = ledgerEntryNumber;

            ledgerEntryId = _ledgerEntryDAO.SaveLedgerEntry(ledgerEntry);

            foreach (LedgerEntryItem ledgerEntryItem in ledgerEntry.LedgerEntryItems)
            {
                // Se graba el asiento
                LedgerEntryItem newLedgerEntryItem = ledgerEntryItem;

                newLedgerEntryItem.Id = 0; //seteo en 0 para el nuevo movimiento
                newLedgerEntryItem = _ledgerEntryDAO.SaveLedgerEntryItem(newLedgerEntryItem, ledgerEntryId, isJournalEntry);

                SaveLedgerEntryItemGroup(ledgerEntryItem, newLedgerEntryItem.Id);
            }

            // Se actualiza el número de asiento
            if (moduleLedgerEntryNumber == 0)
            {
                EntryNumber newEntryNumber = _entryNumberDAO.GetEntryNumber(Convert.ToInt32(entryNumberId));
                newEntryNumber.Number = newEntryNumber.Number + 1;
                _entryNumberDAO.UpdateEntryNumber(newEntryNumber);
            }

            return ledgerEntryNumber;
        }

        /// <summary>
        /// SaveLedgerEntryItemGroup
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <param name="ledgerEntryItemId"></param>
        private void SaveLedgerEntryItemGroup(LedgerEntryItem ledgerEntryItem, int ledgerEntryItemId)
        {
            // Se graba centro de costos asociados al movimiento.
            if (ledgerEntryItem.CostCenters != null)
            {
                foreach (CostCenter costCenter in ledgerEntryItem.CostCenters)
                {
                    _costCenterEntryDAO.SaveCostCenterEntry(costCenter, ledgerEntryItemId, false);
                }
            }

            // Grabación de análisis de movimientos
            if (ledgerEntryItem.Analysis != null)
            {
                foreach (Analysis analysisItem in ledgerEntryItem.Analysis)
                {
                    int correlativeNumber = 0;

                    correlativeNumber = GetCorrelativeNumber(analysisItem.AnalysisConcept.AnalysisCode.AnalysisCodeId, analysisItem.AnalysisConcept.AnalysisConceptId, analysisItem.Key) + 1;
                    analysisItem.AnalysisId = 0; // Id autonumérico
                    _analysisDAO.SaveAnalysis(analysisItem, ledgerEntryItemId, correlativeNumber, false);
                }
            }

            // Grabación de Postfechados
            if (ledgerEntryItem.PostDated != null)
            {
                foreach (PostDated postDatedItem in ledgerEntryItem.PostDated)
                {
                    _postDatedDAO.SavePostDated(postDatedItem, ledgerEntryItemId, false);
                }
            }
        }

        /// <summary>
        /// GetCorrelativeNumber
        /// Obtiene el número correlativo para la tabla Entry Análisis 
        /// </summary>
        /// <returns>int</returns>
        private int GetCorrelativeNumber(int analysisCodeId, int analysisConceptId, string key)
        {
            int correlativeNumber = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisId, analysisCodeId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisConceptId, analysisConceptId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.ConceptKey, key);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                var maxNumber = (from GENERALLEDGEREN.AnalysisEntryItem entryAnalisis in businessCollection select entryAnalisis.CorrelativeNumber).Max();
                correlativeNumber = Convert.ToInt32(maxNumber);
            }
            else
            {
                correlativeNumber = 1;
            }

            return correlativeNumber;
        }

        #endregion

    }
}
