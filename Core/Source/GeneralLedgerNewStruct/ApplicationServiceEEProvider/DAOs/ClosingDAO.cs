//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Helper;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class ClosingDAO
    {
        #region Class

        public class Summary
        {
            public int BranchId { get; set; }
            public int SalePointId { get; set; }
            public int AccountingMovementTypeId { get; set; }
            public int EntryDestinationId { get; set; }
            public int AccountingAccountId { get; set; }
            public string AccountingAccountNumber { get; set; }
            public DateTime AccountingDate { get; set; }
            public int CurrencyId { get; set; }
            public int AccountingNature { get; set; }
            public int IndividualId { get; set; }
            public decimal Amount { get; set; }
            public decimal LocalValue { get; set; }
            public decimal ExchangeRate { get; set; }
        }

        #endregion

        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        private readonly LedgerEntryDAO _ledgerEntryDAO = new LedgerEntryDAO();
        private readonly EntryNumberDAO _entryNumberDAO = new EntryNumberDAO();
        private readonly AnalysisDAO _analysisDAO = new AnalysisDAO();
        private readonly PostDatedDAO _postDatedDAO = new PostDatedDAO();
        private readonly CostCenterEntryDAO _costCenterEntryDAO = new CostCenterEntryDAO();
        private readonly DestinationDAO _destinationDAO = new DestinationDAO();
        private readonly EntryRevertionDAO _entryRevertionDAO = new EntryRevertionDAO();

        #endregion

        #region Public Methods

        /// <summary>
        /// AssetAndLiabilityOpening
        /// Asiento de Apertura de Activos y Pasivos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int AssetAndLiabilityOpening(int year, int userId)
        {
            var saved = 0;
            int accountingCompanyId = 0;

            try
            {
                // Se obtiene el código de compañía contable por default
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingCompany.Properties.Default, 1);

                BusinessCollection companyBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingCompany), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.AccountingCompany company in companyBusinessCollection.OfType<GENERALLEDGEREN.AccountingCompany>())
                {
                    accountingCompanyId = company.AccountingCompanyId;
                }

                // Se obtiene los asientos cuyo primer dígito es distinto de 4 y 5
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.Date);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("%" + year + "%"); // Son los movimientos que se generaron en el año que se realizará el cierre
                criteriaBuilder.And();
                criteriaBuilder.Not();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.AccountNumber);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("4%");
                criteriaBuilder.And();
                criteriaBuilder.Not();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.AccountNumber);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("5%");

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.GetEntryTotals), criteriaBuilder.GetPredicate()));

                int moduleDateId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_EXERCISE_CLOSING));

                List<Summary> summaries = new List<Summary>();

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.GetEntryTotals item in businessCollection.OfType<GENERALLEDGEREN.GetEntryTotals>())
                    {
                        summaries.Add(new Summary()
                        {
                            AccountingAccountId = item.AccountingAccountId,
                            AccountingAccountNumber = item.AccountNumber,
                            AccountingDate = Convert.ToDateTime(item.Date),
                            AccountingMovementTypeId = Convert.ToInt32(item.AccountingMovementTypeId),
                            AccountingNature = Convert.ToInt32(item.AccountingNature),
                            Amount = Convert.ToDecimal(item.TotalValue),
                            BranchId = Convert.ToInt32(item.BranchCode),
                            CurrencyId = Convert.ToInt32(item.CurrencyCode),
                            EntryDestinationId = Convert.ToInt32(item.EntryDestinationId),
                            ExchangeRate = Convert.ToDecimal(item.ExchangeRate),
                            IndividualId = Convert.ToInt32(item.IndividualId),
                            LocalValue = Convert.ToDecimal(item.TotalLocalValue),
                            SalePointId = Convert.ToInt32(item.SalePointCode),
                        });
                    }

                    saved = SaveAssetAndLiabilityOpeningLedgerEntries(summaries, year, userId, accountingCompanyId, moduleDateId);
                }

                return saved;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// IncomeOutcomeClosing
        /// Cierre de Ingresos y Egresos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int IncomeOutcomeClosing(int year, int userId)
        {
            int saved = 0;
            int accountingCompanyId = 0;

            try
            {
                // Se obtiene el código de compañía contable por default
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingCompany.Properties.Default, 1);

                BusinessCollection companyBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingCompany), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.AccountingCompany company in companyBusinessCollection.OfType<GENERALLEDGEREN.AccountingCompany>())
                {
                    accountingCompanyId = company.AccountingCompanyId;
                }

                // Se obtiene los asientos cuyo primer dígito es 4 y 5
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.Date);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("%" + year + "%"); //son los movimientos que se generaron en el año que se realizará el cierre
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.AccountNumber);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("4%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.AccountNumber);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("5%");

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.GetEntryTotals), criteriaBuilder.GetPredicate()));

                int moduleDateId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_EXERCISE_CLOSING));

                List<Summary> summaries = new List<Summary>();

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.GetEntryTotals item in businessCollection.OfType<GENERALLEDGEREN.GetEntryTotals>())
                    {
                        summaries.Add(new Summary()
                        {
                            AccountingAccountId = item.AccountingAccountId,
                            AccountingAccountNumber = item.AccountNumber,
                            AccountingDate = Convert.ToDateTime(item.Date),
                            AccountingMovementTypeId = Convert.ToInt32(item.AccountingMovementTypeId),
                            AccountingNature = Convert.ToInt32(item.AccountingNature),
                            Amount = Convert.ToDecimal(item.TotalValue),
                            BranchId = Convert.ToInt32(item.BranchCode),
                            CurrencyId = Convert.ToInt32(item.CurrencyCode),
                            EntryDestinationId = Convert.ToInt32(item.EntryDestinationId),
                            ExchangeRate = Convert.ToDecimal(item.ExchangeRate),
                            IndividualId = Convert.ToInt32(item.IndividualId),
                            LocalValue = Convert.ToDecimal(item.TotalLocalValue),
                            SalePointId = Convert.ToInt32(item.SalePointCode),
                        });
                    }
                }

                saved = SaveIncomeOutcomeClosingLedgerEntries(summaries, year, userId, accountingCompanyId, moduleDateId);

                return saved;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// MonthlyIncomeClosing
        /// Cierre de utilidad mensual - Cierre de Activo y Pasivo
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public int MonthlyIncomeClosing(int year, int userId, int month)
        {
            int saved = 0;
            int accountingCompanyId = 0;

            try
            {
                // Se obtiene el código de compañía contable por default
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingCompany.Properties.Default, 1);

                BusinessCollection companyBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingCompany), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.AccountingCompany company in companyBusinessCollection.OfType<GENERALLEDGEREN.AccountingCompany>())
                {
                    accountingCompanyId = company.AccountingCompanyId;
                }

                // Se arma la fecha para la búsqueda de registros
                int numberOfDays = DateTime.DaysInMonth(year, month);
                string date = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
                date = date + " 23:59:59";

                // Se obtiene los asientos cuyo primer dígito es 4 y 5
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.Date);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(date); // Son los movimientos que se generaron en el año que se realizará el cierre
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.AccountNumber);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("4%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(GENERALLEDGEREN.GetEntryTotals.Properties.AccountNumber);
                criteriaBuilder.Like();
                criteriaBuilder.Constant("5%");
                criteriaBuilder.And();
                criteriaBuilder.Not();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.GetEntryTotals.Properties.AccountingMovementTypeId, Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_INCOME_OUT_COME_CANCELLATION_ENTRY)));

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.GetEntryTotals), criteriaBuilder.GetPredicate()));
                int moduleDateId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_EXERCISE_CLOSING));

                List<Summary> summaries = new List<Summary>();

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.GetEntryTotals item in businessCollection.OfType<GENERALLEDGEREN.GetEntryTotals>())
                    {
                        summaries.Add(new Summary()
                        {
                            AccountingAccountId = item.AccountingAccountId,
                            AccountingAccountNumber = item.AccountNumber,
                            AccountingDate = Convert.ToDateTime(item.Date),
                            AccountingMovementTypeId = Convert.ToInt32(item.AccountingMovementTypeId),
                            AccountingNature = Convert.ToInt32(item.AccountingNature),
                            Amount = Convert.ToDecimal(item.TotalValue),
                            BranchId = Convert.ToInt32(item.BranchCode),
                            CurrencyId = Convert.ToInt32(item.CurrencyCode),
                            EntryDestinationId = Convert.ToInt32(item.EntryDestinationId),
                            ExchangeRate = Convert.ToDecimal(item.ExchangeRate),
                            IndividualId = Convert.ToInt32(item.IndividualId),
                            LocalValue = Convert.ToDecimal(item.TotalLocalValue),
                            SalePointId = Convert.ToInt32(item.SalePointCode),
                        });
                    }
                }

                saved = SaveMonthlyIncomeClosingLedgerEntries(summaries, year, month, numberOfDays, userId, accountingCompanyId, moduleDateId);

                return saved;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// RevertAnualEntryOpening
        /// Revesión de Asiento Anual de Apertura
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int RevertAnualEntryOpening(int year, int userId)
        {
            try
            {
                int saved = 0;

                //Se obtiene los asientos de mayor
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.LedgerEntryId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntry), criteriaBuilder.GetPredicate()));

                //Se obtiene los asientos de mayor reversados
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.EntryRevertion.Properties.EntryRevertionId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);
                BusinessCollection businessCollectionRevertion = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryRevertion), criteriaBuilder.GetPredicate()));

                //Se obtiene los registros cuyo código de sistema sea "asiento de cierre y apertura del ejercicio"
                var innerQuery = from GENERALLEDGEREN.LedgerEntry ledgerEntry in businessCollection where ledgerEntry.AccountingMovementTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_PROFIT_OPENING_CLOSING_ENTRY)) select ledgerEntry;

                // Hago un select de los sourceId de la tabla entry revertion
                var entryRevertionQuery = from GENERALLEDGEREN.EntryRevertion revertion in businessCollectionRevertion select revertion.EntrySourceId;

                // Se obtiene los registros de entry que no se encuentran dentro de la consulta anterior(not in entryRevertionQuery)
                var resultQuery = from GENERALLEDGEREN.LedgerEntry ledgerEntry in innerQuery where !entryRevertionQuery.Contains(ledgerEntry.LedgerEntryId) select ledgerEntry;

                saved = SaveRevertAnualEntryOpeningLedgerEntry(resultQuery, year, userId);

                return saved;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// RevertIncomeOutcomeClosing
        /// Reversar Cierre Anual de Ingresos y Egresos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int RevertIncomeOutcomeClosing(int year, int userId)
        {
            int saved = 0;

            try
            {
                // Se arma las fechas para consulta
                string dateFrom = "01" + "/" + "12" + "/" + Convert.ToString(year); // El mes de búsqueda es diciembre por ser el mes de cierre

                int numberOfDays = DateTime.DaysInMonth(year, 12);
                string dateTo = Convert.ToString(numberOfDays) + "/" + "12" + "/" + Convert.ToString(year);
                dateTo = dateTo + " 23:59:59";

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_INCOME_OUT_COME_CANCELLATION_ENTRY)));
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntry), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    saved = SaveRevertIncomeOutcomeClosingLedgerEntry(businessCollection, year, userId);
                }

                return saved;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// SaveLedgerEntryClosing
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        private int SaveLedgerEntryClosing(LedgerEntry ledgerEntry, bool isJournalEntry, int moduleLedgerEntryNumber)
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
                newLedgerEntryItem = _ledgerEntryDAO.SaveLedgerEntryItem(newLedgerEntryItem, ledgerEntryId, isJournalEntry);

                //Se graban los centros de costos, analisis y postfechados
                SaveLedgerEntryClosingItemAssociatedMovements(ledgerEntryItem, newLedgerEntryItem.Id, isJournalEntry);
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
        /// SaveLedgerEntryClosingItemAssociatedMovements
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <param name="ledgerEntryId"></param>
        /// <param name="isJournalEntry"></param>
        private void SaveLedgerEntryClosingItemAssociatedMovements(LedgerEntryItem ledgerEntryItem, int ledgerEntryId, bool isJournalEntry)
        {
            // Se graba centro de costos asociados al movimiento.
            if (ledgerEntryItem.CostCenters != null)
            {
                foreach (CostCenter costCenter in ledgerEntryItem.CostCenters)
                {
                    _costCenterEntryDAO.SaveCostCenterEntry(costCenter, ledgerEntryId, isJournalEntry);
                }
            }

            // Grabación de análisis de movimientos
            if (ledgerEntryItem.Analysis != null)
            {
                foreach (Analysis analysisItem in ledgerEntryItem.Analysis)
                {
                    int correlativeNumber = 0;

                    correlativeNumber = GetCorrelativeNumber(analysisItem.AnalysisConcept.AnalysisCode.AnalysisCodeId,
                                                             analysisItem.AnalysisConcept.AnalysisConceptId, analysisItem.Key) + 1;
                    analysisItem.AnalysisId = 0; // Id autonumérico
                    _analysisDAO.SaveAnalysis(analysisItem, ledgerEntryId, correlativeNumber, isJournalEntry);
                }
            }

            // Grabación de Postfechados
            if (ledgerEntryItem.PostDated != null)
            {
                foreach (PostDated postDatedItem in ledgerEntryItem.PostDated)
                {
                    _postDatedDAO.SavePostDated(postDatedItem, ledgerEntryId, isJournalEntry);
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

        /// <summary>
        /// SaveLedgerEntryRevertionClosing
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="ledgerEntrySourceId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int SaveLedgerEntryRevertionClosing(LedgerEntry ledgerEntry, int ledgerEntrySourceId, int userId)
        {
            // Se graba la cabecera
            int ledgerEntryId = 0;
            int ledgerEntryNumber = 0;
            bool isJournalEntry = false;

            // Se obtiene el número de asiento
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

            ledgerEntry.EntryNumber = Convert.ToInt32(entryNumber.Number);
            ledgerEntryNumber = Convert.ToInt32(entryNumber.Number);
            ledgerEntryId = _ledgerEntryDAO.SaveLedgerEntry(ledgerEntry);

            foreach (LedgerEntryItem ledgerEntryItem in ledgerEntry.LedgerEntryItems)
            {
                // Se graba el asiento
                LedgerEntryItem newLedgerEntryItem = ledgerEntryItem;
                
                //Se graba la reversión.
                newLedgerEntryItem.Id = 0; //seteo en 0 para el nuevo movimiento
                newLedgerEntryItem = _ledgerEntryDAO.SaveLedgerEntryItem(newLedgerEntryItem, ledgerEntryId, isJournalEntry);
                _entryRevertionDAO.SaveEntryRevertion(0, ledgerEntrySourceId, ledgerEntryId, userId, DateTime.Now, isJournalEntry);
                
                //Se graban los centros de costos, analisis y postfechados
                SaveLedgerEntryRevertionClosingItemAssociatedMovements(ledgerEntryItem, newLedgerEntryItem.Id, isJournalEntry);
            }

            // Se actualiza el número de asiento
            EntryNumber newEntryNumber = _entryNumberDAO.GetEntryNumber(Convert.ToInt32(entryNumber.Id));
            newEntryNumber.Number = newEntryNumber.Number + 1;
            _entryNumberDAO.UpdateEntryNumber(newEntryNumber);

            return ledgerEntryNumber;
        }

        /// <summary>
        /// SaveLedgerEntryRevertionClosingItemAssociatedMovements
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <param name="ledgerEntryId"></param>
        /// <param name="isJournalEntry"></param>
        private void SaveLedgerEntryRevertionClosingItemAssociatedMovements(LedgerEntryItem ledgerEntryItem, int ledgerEntryId, bool isJournalEntry)
        {
            try
            {
                // Se graba centro de costos asociados al movimiento.
                if (ledgerEntryItem.CostCenters != null)
                {
                    foreach (CostCenter costCenter in ledgerEntryItem.CostCenters)
                    {
                        _costCenterEntryDAO.SaveCostCenterEntry(costCenter, ledgerEntryId, isJournalEntry);
                    }
                }

                // Grabación de análisis de movimientos
                if (ledgerEntryItem.Analysis != null)
                {
                    foreach (Analysis analysisItem in ledgerEntryItem.Analysis)
                    {
                        int correlativeNumber = 0;

                        correlativeNumber = GetCorrelativeNumber(analysisItem.AnalysisConcept.AnalysisCode.AnalysisCodeId,
                                                                 analysisItem.AnalysisConcept.AnalysisConceptId, analysisItem.Key) + 1;
                        analysisItem.AnalysisId = 0; // Id autonumérico
                        _analysisDAO.SaveAnalysis(analysisItem, ledgerEntryId, correlativeNumber, isJournalEntry);
                    }
                }

                // Grabación de Postfechados
                if (ledgerEntryItem.PostDated != null)
                {
                    foreach (PostDated postDatedItem in ledgerEntryItem.PostDated)
                    {
                        _postDatedDAO.SavePostDated(postDatedItem, ledgerEntryId, isJournalEntry);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveAssetAndLiabilityOpeningLedgerEntries
        /// </summary>
        /// <param name="summaries"></param>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="moduleDateId"></param>
        /// <returns></returns>
        private int SaveAssetAndLiabilityOpeningLedgerEntries(List<Summary> summaries, int year, int userId, int accountingCompanyId, int moduleDateId)
        {
            int saved = 0;
            int ledgerEntryNumber = 0;

            //Se agrupa por sucursal, punto de venta y moneda
            var groupSummaries = from p in summaries
                                 group p by new
                                 {
                                     p.BranchId,
                                     p.SalePointId,
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
                            AccountingCompanyId = accountingCompanyId // Compañía por defecto
                        };
                        ledgerEntry.AccountingDate = new DateTime(year + 1, 1, 1); // Primer día del siguiente año
                        ledgerEntry.AccountingMovementType = new AccountingMovementType()
                        {
                            AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_PROFIT_OPENING_CLOSING_ENTRY)) //ASIENTOS DE CIERRE Y DE APERTURA DE EJERCICIO
                        };
                        ledgerEntry.Branch = new Branch() { Id = Convert.ToInt32(groupSummary.Key.BranchId) };
                        ledgerEntry.Description = "APERTURA DE ACTIVOS, PASIVOS Y CAPITAL DEL AÑO (LOCAL) " + year;
                        ledgerEntry.EntryDestination = new EntryDestination()
                        {
                            DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_ENTRY_DESTINATION_LOCAL))
                        };
                        ledgerEntry.EntryNumber = 0;
                        ledgerEntry.Id = 0;

                        ledgerEntry.LedgerEntryItems = new List<LedgerEntryItem>();

                        foreach (var summary in groupSummary)
                        {
                            LedgerEntryItem ledgerEntryItem = new LedgerEntryItem();
                            ledgerEntryItem.AccountingAccount = new AccountingAccount();
                            ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(summary.AccountingAccountId);
                            ledgerEntryItem.AccountingNature = Convert.ToInt32(summary.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? AccountingNatures.Credit : AccountingNatures.Debit;

                            ledgerEntryItem.Amount = new Amount();
                            ledgerEntryItem.Amount.Value = Convert.ToDecimal(summary.Amount);
                            ledgerEntryItem.Amount.Currency = new Currency();
                            ledgerEntryItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(summary.ExchangeRate) };
                            ledgerEntryItem.Amount.Currency.Id = Convert.ToInt32(summary.CurrencyId);
                            ledgerEntryItem.LocalAmount = new Amount() { Value = Convert.ToDecimal(summary.LocalValue) };
                            ledgerEntryItem.Analysis = new List<Analysis>();
                            ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType();
                            ledgerEntryItem.ReconciliationMovementType.Id = 0;
                            ledgerEntryItem.CostCenters = new List<CostCenter>();
                            ledgerEntryItem.Currency = new Currency() { Id = Convert.ToInt32(summary.CurrencyId) };
                            ledgerEntryItem.Description = "APERTURA DE ACTIVOS, PASIVOS Y CAPITAL DEL AÑO (LOCAL) " + year;
                            ledgerEntryItem.EntryType = new EntryType();
                            ledgerEntryItem.Id = 0;
                            ledgerEntryItem.Individual = new Individual() { IndividualId = 0 };
                            ledgerEntryItem.PostDated = new List<PostDated>();
                            ledgerEntryItem.Receipt = new Receipt();
                            ledgerEntryItem.Receipt.Date = null;
                            ledgerEntryItem.Receipt.Number = 0;

                            ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                        }

                        ledgerEntry.ModuleDateId = moduleDateId;
                        ledgerEntry.RegisterDate = DateTime.Now;
                        ledgerEntry.SalePoint = new SalePoint() { Id = groupSummary.Key.SalePointId };
                        ledgerEntry.UserId = userId;

                        saved = SaveLedgerEntryClosing(ledgerEntry, false, ledgerEntryNumber);
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

            return saved;
        }

        /// <summary>
        /// SaveIncomeOutcomeClosingLedgerEntries
        /// </summary>
        /// <param name="summaries"></param>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="moduleDateId"></param>
        /// <returns></returns>
        private int SaveIncomeOutcomeClosingLedgerEntries(List<Summary> summaries, int year, int userId, int accountingCompanyId, int moduleDateId)
        {
            int saved = 0;
            int ledgerEntryNumber = 0;

            //Se agrupa por sucursal, punto de venta y moneda
            var groupSummaries = from p in summaries
                                 group p by new
                                 {
                                     p.BranchId,
                                     p.SalePointId,
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
                            AccountingCompanyId = accountingCompanyId
                        };
                        ledgerEntry.AccountingDate = new DateTime(year, 12, 31); // Se cierra con el último día del año
                        ledgerEntry.AccountingMovementType = new AccountingMovementType()
                        {
                            AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_INCOME_OUT_COME_CANCELLATION_ENTRY)) //ASIENTO DE CANCELACION DE INGRESOS Y EGRESOS
                        };
                        ledgerEntry.Branch = new Branch() { Id = Convert.ToInt32(groupSummary.Key.BranchId) };
                        ledgerEntry.EntryDestination = new EntryDestination()
                        {
                            DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_ENTRY_DESTINATION_LOCAL))
                        };
                        ledgerEntry.Description = "CIERRE DE INGRESOS Y EGRESOS " + _destinationDAO.GetEntryDestination(ledgerEntry.EntryDestination).Description.ToUpper() + " DEL AÑO " + year;
                        ledgerEntry.EntryNumber = 0;
                        ledgerEntry.Id = 0;

                        ledgerEntry.LedgerEntryItems = new List<LedgerEntryItem>();

                        foreach (var summary in groupSummary)
                        {
                            LedgerEntryItem ledgerEntryItem = new LedgerEntryItem();
                            ledgerEntryItem.AccountingAccount = new AccountingAccount();
                            ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(summary.AccountingAccountId);
                            ledgerEntryItem.AccountingNature = Convert.ToInt32(summary.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? AccountingNatures.Credit : AccountingNatures.Debit;
                            ledgerEntryItem.Amount = new Amount();
                            ledgerEntryItem.Amount.Value = Convert.ToDecimal(summary.Amount);
                            ledgerEntryItem.Amount.Currency = new Currency();
                            ledgerEntryItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(summary.ExchangeRate) };
                            ledgerEntryItem.Amount.Currency.Id = Convert.ToInt32(summary.CurrencyId);
                            ledgerEntryItem.LocalAmount = new Amount() { Value = Convert.ToDecimal(summary.LocalValue) };
                            ledgerEntryItem.Analysis = new List<Analysis>();
                            ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType();
                            ledgerEntryItem.ReconciliationMovementType.Id = 0;
                            ledgerEntryItem.CostCenters = new List<CostCenter>();
                            ledgerEntryItem.Currency = new Currency() { Id = Convert.ToInt32(summary.CurrencyId) };
                            ledgerEntryItem.Description = "CIERRE DE INGRESOS Y EGRESOS " + _destinationDAO.GetEntryDestination(ledgerEntry.EntryDestination).Description.ToUpper() + " DEL AÑO " + year;
                            ledgerEntryItem.EntryType = new EntryType();
                            ledgerEntryItem.Id = 0;
                            ledgerEntryItem.Individual = new Individual() { IndividualId = Convert.ToInt32(summary.IndividualId) };
                            ledgerEntryItem.PostDated = new List<PostDated>();
                            ledgerEntryItem.Receipt = new Receipt();
                            ledgerEntryItem.Receipt.Date = null;
                            ledgerEntryItem.Receipt.Number = 0;

                            ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                            // Se arma la cuenta de contrapartida
                            LedgerEntryItem counterpartLedgerEntryItem = new LedgerEntryItem();
                            counterpartLedgerEntryItem.AccountingAccount = new AccountingAccount();
                            counterpartLedgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_YEARS_PROFIT_ACCOUNT_ID));
                            counterpartLedgerEntryItem.AccountingNature = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? AccountingNatures.Debit : AccountingNatures.Credit;
                            counterpartLedgerEntryItem.Amount = new Amount();
                            counterpartLedgerEntryItem.Amount.Value = Math.Abs(Convert.ToDecimal(summary.Amount));
                            counterpartLedgerEntryItem.Amount.Currency = new Currency();
                            counterpartLedgerEntryItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(summary.ExchangeRate) };
                            counterpartLedgerEntryItem.Amount.Currency.Id = Convert.ToInt32(summary.CurrencyId);
                            counterpartLedgerEntryItem.LocalAmount = new Amount() { Value = Math.Abs(Convert.ToDecimal(summary.LocalValue)) };
                            counterpartLedgerEntryItem.Analysis = new List<Analysis>();
                            counterpartLedgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType() { Id = 0 };
                            counterpartLedgerEntryItem.CostCenters = new List<CostCenter>();
                            counterpartLedgerEntryItem.Currency = new Currency() { Id = Convert.ToInt32(summary.CurrencyId) };
                            counterpartLedgerEntryItem.Description = "CIERRE DE INGRESOS Y EGRESOS " + _destinationDAO.GetEntryDestination(ledgerEntry.EntryDestination).Description.ToUpper() + " DEL AÑO " + year;
                            counterpartLedgerEntryItem.EntryType = new EntryType();
                            counterpartLedgerEntryItem.Id = 0; // Seteo el id en cero por ser un nuevo registro
                            counterpartLedgerEntryItem.Individual = new Individual() { IndividualId = Convert.ToInt32(summary.IndividualId) };
                            counterpartLedgerEntryItem.PostDated = new List<PostDated>();
                            counterpartLedgerEntryItem.Receipt = new Receipt();
                            counterpartLedgerEntryItem.Receipt.Date = null;
                            counterpartLedgerEntryItem.Receipt.Number = 0;

                            ledgerEntry.LedgerEntryItems.Add(counterpartLedgerEntryItem);
                        }

                        ledgerEntry.ModuleDateId = moduleDateId;
                        ledgerEntry.RegisterDate = DateTime.Now;
                        ledgerEntry.SalePoint = new SalePoint() { Id = groupSummary.Key.SalePointId };
                        ledgerEntry.UserId = userId;

                        saved = SaveLedgerEntryClosing(ledgerEntry, false, ledgerEntryNumber);
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

            return saved;
        }

        /// <summary>
        /// SaveMonthlyIncomeClosingLedgerEntries
        /// </summary>
        /// <param name="summaries"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="userId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="moduleDateId"></param>
        /// <returns></returns>
        private int SaveMonthlyIncomeClosingLedgerEntries(List<Summary> summaries, int year, int month, int numberOfDays, int userId, int accountingCompanyId, int moduleDateId)
        {
            int saved = 0;
            int ledgerEntryNumber = 0;

            //Se agrupa por sucursal, punto de venta y moneda
            var groupSummaries = from p in summaries
                                 group p by new
                                 {
                                     p.BranchId,
                                     p.SalePointId,
                                     p.CurrencyId
                                 } into grupSummary
                                 select grupSummary;

            using (Context.Current)
            {
                Transaction transaction = new Transaction();

                try
                {
                    foreach (var grupSummary in groupSummaries)
                    {
                        LedgerEntry ledgerEntry = new LedgerEntry();
                        ledgerEntry.AccountingCompany = new AccountingCompany()
                        {
                            AccountingCompanyId = accountingCompanyId
                        };
                        ledgerEntry.AccountingDate = new DateTime(year, 12, numberOfDays); // Se cierra con el último día del año
                        ledgerEntry.AccountingMovementType = new AccountingMovementType()
                        {
                            AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_INCOME_OUT_COME_CANCELLATION_ENTRY)) //ASIENTO DE CANCELACION DE INGRESOS Y EGRESOS
                        };
                        ledgerEntry.Branch = new Branch() { Id = Convert.ToInt32(grupSummary.Key.BranchId) };
                        ledgerEntry.EntryDestination = new EntryDestination()
                        {
                            DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_ENTRY_DESTINATION_LOCAL))
                        };
                        ledgerEntry.Description = "UTILIDAD DEL MES " + Convert.ToString(month) + "/" + Convert.ToString(year);
                        ledgerEntry.EntryNumber = 0;
                        ledgerEntry.Id = 0;

                        ledgerEntry.LedgerEntryItems = new List<LedgerEntryItem>();

                        foreach (var summary in grupSummary)
                        {
                            LedgerEntryItem ledgerEntryItem = new LedgerEntryItem();
                            ledgerEntryItem.AccountingAccount = new AccountingAccount();
                            ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_YEARS_LOSS_ACCOUNT_ID));
                            ledgerEntryItem.AccountingNature = Convert.ToInt32(summary.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? AccountingNatures.Debit : AccountingNatures.Credit;
                            ledgerEntryItem.Amount = new Amount();
                            ledgerEntryItem.Amount.Value = Convert.ToDecimal(summary.Amount);
                            ledgerEntryItem.Amount.Currency = new Currency();
                            ledgerEntryItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(summary.ExchangeRate) };
                            ledgerEntryItem.Amount.Currency.Id = Convert.ToInt32(summary.CurrencyId);
                            ledgerEntryItem.LocalAmount = new Amount() { Value = Convert.ToDecimal(summary.LocalValue) };
                            ledgerEntryItem.Analysis = new List<Analysis>();
                            ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType();
                            ledgerEntryItem.ReconciliationMovementType.Id = 0;
                            ledgerEntryItem.CostCenters = new List<CostCenter>();
                            ledgerEntryItem.Currency = new Currency() { Id = Convert.ToInt32(summary.CurrencyId) };
                            ledgerEntryItem.Description = "UTILIDAD DEL MES " + Convert.ToString(month) + "/" + Convert.ToString(year);
                            ledgerEntryItem.EntryType = new EntryType();
                            ledgerEntryItem.Id = 0;
                            ledgerEntryItem.Individual = new Individual() { IndividualId = Convert.ToInt32(summary.IndividualId) };
                            ledgerEntryItem.PostDated = new List<PostDated>();
                            ledgerEntryItem.Receipt = new Receipt();
                            ledgerEntryItem.Receipt.Date = null;
                            ledgerEntryItem.Receipt.Number = 0;

                            ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                            // Se arma la cuenta de contrapartida
                            LedgerEntryItem counterpartLedgerEntryItem = new LedgerEntryItem();
                            counterpartLedgerEntryItem.AccountingAccount = new AccountingAccount();
                            counterpartLedgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_YEARS_PROFIT_ACCOUNT_ID));
                            counterpartLedgerEntryItem.AccountingNature = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? AccountingNatures.Debit : AccountingNatures.Credit;
                            counterpartLedgerEntryItem.Amount = new Amount();
                            counterpartLedgerEntryItem.Amount.Value = Math.Abs(Convert.ToDecimal(summary.Amount));
                            counterpartLedgerEntryItem.Amount.Currency = new Currency();
                            counterpartLedgerEntryItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(summary.ExchangeRate) };
                            counterpartLedgerEntryItem.Amount.Currency.Id = Convert.ToInt32(summary.CurrencyId);
                            counterpartLedgerEntryItem.LocalAmount = new Amount() { Value = Math.Abs(Convert.ToDecimal(summary.LocalValue)) };
                            counterpartLedgerEntryItem.Analysis = new List<Analysis>();
                            counterpartLedgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType() { Id = 0 };
                            counterpartLedgerEntryItem.CostCenters = new List<CostCenter>();
                            counterpartLedgerEntryItem.Currency = new Currency() { Id = Convert.ToInt32(summary.CurrencyId) };
                            counterpartLedgerEntryItem.Description = "UTILIDAD DEL MES " + Convert.ToString(month) + "/" + Convert.ToString(year);
                            counterpartLedgerEntryItem.EntryType = new EntryType();
                            counterpartLedgerEntryItem.Id = 0; // Seteo el id en cero por ser un nuevo registro
                            counterpartLedgerEntryItem.Individual = new Individual() { IndividualId = Convert.ToInt32(summary.IndividualId) };
                            counterpartLedgerEntryItem.PostDated = new List<PostDated>();
                            counterpartLedgerEntryItem.Receipt = new Receipt();
                            counterpartLedgerEntryItem.Receipt.Date = null;
                            counterpartLedgerEntryItem.Receipt.Number = 0;

                            ledgerEntry.LedgerEntryItems.Add(counterpartLedgerEntryItem);
                        }

                        ledgerEntry.ModuleDateId = moduleDateId;
                        ledgerEntry.RegisterDate = DateTime.Now;
                        ledgerEntry.SalePoint = new SalePoint() { Id = grupSummary.Key.SalePointId };
                        ledgerEntry.UserId = userId;

                        saved = SaveLedgerEntryClosing(ledgerEntry, false, ledgerEntryNumber);
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

            return saved;
        }

        /// <summary>
        /// SaveRevertAnualEntryOpeningLedgerEntry
        /// </summary>
        /// <param name="ledgerEntries"></param>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int SaveRevertAnualEntryOpeningLedgerEntry(IEnumerable<GENERALLEDGEREN.LedgerEntry> ledgerEntries, int year, int userId)
        {
            //Se arma el modelo ledgerentry para la reversión
            LedgerEntry ledgerEntryRevertion = new LedgerEntry();
            ledgerEntryRevertion.LedgerEntryItems = new List<LedgerEntryItem>();

            int saved = 0;

            if (ledgerEntries.Any())
            {
                using (Context.Current)
                {
                    Transaction transaction = new Transaction();

                    try
                    {
                        foreach (GENERALLEDGEREN.LedgerEntry ledgerEntryEntity in ledgerEntries)
                        {
                            ledgerEntryRevertion = _ledgerEntryDAO.GetLedgerEntryById(ledgerEntryEntity.LedgerEntryId);
                            ledgerEntryRevertion.Description = "REVERSION ASIENTO APERTURA DEL AÑO " + (year + 1);
                            ledgerEntryRevertion.Id = 0;
                            ledgerEntryRevertion.EntryNumber = 0;

                            foreach (LedgerEntryItem ledgerEntryItem in ledgerEntryRevertion.LedgerEntryItems)
                            {
                                ledgerEntryItem.AccountingNature = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? AccountingNatures.Debit : AccountingNatures.Credit;
                                ledgerEntryItem.Description = "REVERSION ASIENTO APERTURA DEL AÑO " + (year + 1);
                                ledgerEntryItem.Id = 0;
                            }

                            saved = SaveLedgerEntryRevertionClosing(ledgerEntryRevertion, ledgerEntryEntity.LedgerEntryId, userId);
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

            return saved;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ledgerEntryEntities"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int SaveRevertIncomeOutcomeClosingLedgerEntry(BusinessCollection ledgerEntryEntities, int year, int userId)
        {
            int saved = 0;
            LedgerEntry ledgerEntryRevertion = new LedgerEntry();
            ledgerEntryRevertion.LedgerEntryItems = new List<LedgerEntryItem>();

            using (Context.Current)
            {
                Transaction transaction = new Transaction();

                try
                {
                    foreach (GENERALLEDGEREN.LedgerEntry ledgerEntryEntity in ledgerEntryEntities.OfType<GENERALLEDGEREN.LedgerEntry>())
                    {
                        ledgerEntryRevertion = _ledgerEntryDAO.GetLedgerEntryById(ledgerEntryEntity.LedgerEntryId);
                        if (ledgerEntryRevertion.EntryDestination.DestinationId == 0)
                        {
                            ledgerEntryRevertion.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys. GL_ENTRY_DESTINATION_LOCAL));
                        }
                        ledgerEntryRevertion.Description = "REVERSION CIERRE DE INGRESOS Y EGRESOS " + _destinationDAO.GetEntryDestination(ledgerEntryRevertion.EntryDestination).Description.ToUpper() + " DEL AÑO " + year;
                        ledgerEntryRevertion.Id = 0;
                        ledgerEntryRevertion.EntryNumber = 0;

                        foreach (LedgerEntryItem ledgerEntryItem in ledgerEntryRevertion.LedgerEntryItems)
                        {
                            ledgerEntryItem.AccountingNature = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? AccountingNatures.Debit : AccountingNatures.Credit;
                            ledgerEntryItem.Description = "REVERSION CIERRE DE INGRESOS Y EGRESOS " + _destinationDAO.GetEntryDestination(ledgerEntryRevertion.EntryDestination).Description.ToUpper() + " DEL AÑO " + year;
                            ledgerEntryItem.Id = 0;
                        }

                        saved = SaveLedgerEntryRevertionClosing(ledgerEntryRevertion, ledgerEntryEntity.LedgerEntryId, userId);
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

            return saved;
        }

        #endregion PrivateMethods

    }
}
