// Sistran
using Sistran.Core.Application.TempCommonServices.Models;

// Sistran FWK
using Sistran.Core.Framework.BAF;

// System
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Co.Application.Data;
using System.Data;
using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.AccountingClosingServices.DTOs;
using Sistran.Core.Application.AccountingClosingServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using System.Threading.Tasks;
using System.Globalization;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingClosingServices.EEProvider.Enums;
using AccountingRulesModels = Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingRules;
using Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingRules;
using Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingConcepts;

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider
{
    public class AccountingClosingApplicationServiceProvider : IAccountingClosingApplicationService
    {
        #region Public Methods

        #region Actions
        /// <summary>
        /// GetStatus
        /// </summary>
        /// <param name="module"></param>
        /// <returns>JsonResult</returns>
        public List<object> GetStatus(int module)
        {
            AccountingClosingDTO AccountingClosingProcess = GetAccountingClosing(module);
            var accountingClosingResponse = new List<object>();

            double hourElapsed = 0;
            double minElapsed = 0;
            double dayElapsed = 0;

            DateTime startDate;
            DateTime endDate;

            if (AccountingClosingProcess.ModuleId == 0)
            {
                var accounting = new
                {
                    Module = "",
                    StartDate = "",
                    EndDate = "",
                    Delay = "",
                    InProgress = ""
                };

                accountingClosingResponse.Add(accounting);
            }
            else
            {
                startDate = Convert.ToDateTime(AccountingClosingProcess.StartDate);
                endDate = Convert.ToDateTime(AccountingClosingProcess.EnDate);

                TimeSpan timeSpan;

                if (AccountingClosingProcess.isProgress)
                {
                    timeSpan = DateTime.Now - startDate;

                    if (timeSpan.TotalDays >= 1)
                    {
                        dayElapsed = timeSpan.Days;
                        hourElapsed = timeSpan.Hours;
                        minElapsed = timeSpan.Minutes;
                    }
                    else
                    {
                        dayElapsed = System.Math.Round((DateTime.Now.TimeOfDay.TotalDays - startDate.TimeOfDay.TotalDays), 2);
                        hourElapsed = timeSpan.TotalHours;
                        minElapsed = timeSpan.Minutes;
                    }
                }
                else
                {
                    timeSpan = endDate - startDate;

                    if (timeSpan.TotalDays >= 1)
                    {
                        dayElapsed = timeSpan.TotalDays;
                        hourElapsed = System.Math.Round((endDate.TimeOfDay.TotalHours - startDate.TimeOfDay.TotalHours), 2);
                        minElapsed = hourElapsed - System.Math.Truncate(hourElapsed);
                        minElapsed = System.Math.Ceiling(minElapsed * 60);
                    }
                    else
                    {
                        dayElapsed = System.Math.Round((endDate.TimeOfDay.TotalHours - startDate.TimeOfDay.TotalHours), 2);
                        hourElapsed = System.Math.Round((endDate.TimeOfDay.TotalHours - startDate.TimeOfDay.TotalHours), 2);
                        minElapsed = hourElapsed - System.Math.Truncate(hourElapsed);
                        minElapsed = System.Math.Ceiling(minElapsed * 60);
                    }
                }

                var accounting = new
                {
                    Module = DelegateService.tempCommonService.GetModuleDates().FirstOrDefault(i => i.Id == module).Description,
                    StartDate = AccountingClosingProcess.StartDate.HasValue ? AccountingClosingProcess.StartDate.Value.ToString() : "",
                    EndDate = AccountingClosingProcess.EnDate.HasValue ? AccountingClosingProcess.EnDate.Value.ToString() : "",
                    Delay = System.Math.Truncate(dayElapsed) + " d " + System.Math.Truncate(hourElapsed) + " h " + System.Math.Truncate(minElapsed) + " m",
                    InProgress = AccountingClosingProcess.isProgress ? Resources.Resources.Started : Resources.Resources.Finalized
                };

                accountingClosingResponse.Add(accounting);
            }

            return accountingClosingResponse;
        }

        /// <summary>
        /// MonthlyClosure
        /// Realiza el proceso de precierre
        /// </summary>
        /// <param name="module"></param>
        /// <returns>Task<JsonResult/></returns>
        public int MonthlyClosureAsync(int module)
        {//TODO: En espera nueva definicion

            var result = 0;
            var accountingDate = GetClosingDate(module);

            int day = 0;

            /*if (!ConfigurationManager.AppSettings.AllKeys.Any(key => ConfigurationManager.AppSettings[key].Equals(module.ToString())))
            {
                return -1;
            }*/
            //BATCH EMISION / EMISION
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_ISSUANCE_MODULE)))
            {
                result = IssuanceClosureGeneration(accountingDate.Year, accountingDate.Month, day, module);
            }
            //SINIESTROS
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CLAIMS_MODULE)))
            {
                result = ClaimClosureGeneration(accountingDate.Year, accountingDate.Month, module);
            }
            //REASEGURO
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_REINSURANCE_MODULE)))
            {
                result = ReinsuranceClosureGeneration(accountingDate.Year, accountingDate.Month, day, module);
            }
            //RES. RIESGOS CURSO/RESERVA TECNICA
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_TECHNICAL_RESERVE_MODULE)))
            {
                result = ClaimReserveClosureGeneration(accountingDate.Year, accountingDate.Month, module);
            }
            //RESERVAS IBNR 
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_IBNR_MODULE)))
            {
                result = IbnrClosureGeneration(accountingDate.Year, module);
            }
            //RESERVAS PREVISIÓN 
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_RISK_PREVENTION_MODULE)))
            {
                result = RiskPreventionReserveClosureGeneration(accountingDate.Year, accountingDate.Month, module);
            }
            //RIESGOS CATASTRÓFICOS
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CATASTROPHIC_RISK_RESERVE_MODULE)))
            {
                result = CatastrophicRiskReserveClosureGeneration(accountingDate.Year, accountingDate.Month, module);
            }
            //PRIMAS VENCIDAS
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_EXPIRED_PREMIUMS_MODULE)))
            {
                result = ExpiredPremiumsGeneration(accountingDate.Year, accountingDate.Month, module);
            }
            return result;
        }

        /// <summary>
        /// AccountClosure
        /// Realiza el proceso de Contabilización
        /// </summary>
        /// <param name="module"></param>
        /// <returns>JsonResult</returns>
        public List<string> AccountClosure(int module, int userId, int day)
        {

            List<string> result = new List<string>();
            DateTime accountingDate = GetClosingDate(module);

            #region Issuance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_ISSUANCE_MODULE)))
            {
                result = AccountClosureIssuance(accountingDate, module, userId, day);
            }

            #endregion Issuance

            #region Claims

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CLAIMS_MODULE)))
            {

            }

            #endregion Claims

            #region Reinsurance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_REINSURANCE_MODULE)))
            {
                result = AccountClosureReinsurance(accountingDate, module, userId);
            }

            #endregion Reinsurance

            #region RiskReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_TECHNICAL_RESERVE_MODULE)))
            {
                result = AccountClosureRiskReserve(accountingDate, module, userId);
            }

            #endregion RiskReserve

            #region IBNR Reserves

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_IBNR_MODULE)))
            {
                result = AccountClosureIBNRReserves(accountingDate, module, userId);
            }

            #endregion

            #region PrevisionReserves

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_RISK_PREVENTION_MODULE)))
            {
                result = AccountClosurePrevisionReserves(accountingDate, module, userId);
            }

            #endregion

            #region CatastrophicRisk

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CATASTROPHIC_RISK_RESERVE_MODULE)))
            {
                result = AccountClosureCatastrophicRiskReserve(accountingDate, module, userId);
            }

            #endregion

            #region ExpiredPremiums

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_EXPIRED_PREMIUMS_MODULE)))
            {
                result = AccountClosureExpiredPremiums(accountingDate, module, userId, day);
            }

            #endregion

            #region IncomeAndExpenses

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_INCOME_AND_EXPENSES_MODULE)))
            {
                result = AccountClosureIncomeAndExpenses(accountingDate, userId, day);
            }

            #endregion


            return result;

        }


        #endregion

        #region Claim

        public List<string> AccountingClosureClaim(DateTime accountingDate, int module, int userId)
        {
            List<string> entryNumbers = new List<string>();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            var entryNumber = 0;
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            // Se obtiene los parámetros para generar el asiento
            closures = GetClaimClosureReport();

            if (closures.Count > 0)
            {
                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> accountingClosingReportDTOs;
                accountingClosingReportDTOs = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                foreach (var accountingClosingReport in accountingClosingReportDTOs)
                {
                    accountingClosingReport.ModuleId = module;
                    //filtro por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReports;

                    accountingClosingReports = (from AccountingClosingReportDTO item in closures where item.PrefixCd == accountingClosingReport.PrefixCd && item.BrachCd == accountingClosingReport.BrachCd select item).ToList();

                    LedgerEntryDTO entry;
                    entry = GenerateClaimReserveEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(entry);
                }

                entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                    entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                    entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                    entryHeader.Branch = ledgerEntry.Branch;
                    entryHeader.Description = ledgerEntry.Description;
                    entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                    entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                    entryHeader.Id = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        entryHeader.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                    entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                    entryHeader.SalePoint = ledgerEntry.SalePoint;
                    entryHeader.Status = ledgerEntry.Status;
                    entryHeader.UserId = ledgerEntry.UserId;
                }

                // Se valida débitos y créditos
                decimal debits = 0;
                decimal credits = 0;

                foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
                {
                    if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                    {
                        debits = debits + accountingEntry.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + accountingEntry.LocalAmount.Value;
                    }
                }

                if (debits == credits)
                {
                    if (entryHeader.LedgerEntryItems.Count > 10)
                    {
                        // Se borra los datos de la tabla temporal de trabajo
                        DelegateService.generalLedgerService.ClearTempAccountEntry();

                        DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                        entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                    }
                    else
                    {
                        entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                    }

                    if (entryNumber > 0)
                    {
                        entryNumbers.Add(" " + entryNumber);
                        ClaimClosureEnding(accountingDate.Year, accountingDate.Month);
                    }
                    else
                    {
                        entryNumbers.Add(Resources.Resources.EntryRecordingError);
                    }
                }
            }
            return entryNumbers;
        }
        /// <summary>
        /// ClaimClosureGeneration
        /// Ejecuta el cierre de siniestros
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        public int ClaimClosureGeneration(int year, int month, int module)
        {
            int success = 0;

            try
            {
                int processId = SaveLogProcess(module);
                int first = 0;
                int count = 50;
                int last = count * 1;
                int total = 0;

                var parameters = new NameValue[4];
                parameters[0] = new NameValue("YEAR", year);
                parameters[1] = new NameValue("MONTH", month);
                parameters[2] = new NameValue("FIRST", first);
                parameters[3] = new NameValue("LAST", last);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_CLAIM_ESTIMATION_DATA", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        total = Convert.ToInt32(arrayItem[0]);
                    }
                }

                if (total >= last)
                {
                    for (int i = 2; i <= total / count + 1; i++)
                    {
                        first = last;
                        last = count * i;
                        parameters[0] = new NameValue("YEAR", year);
                        parameters[1] = new NameValue("MONTH", month);
                        parameters[2] = new NameValue("FIRST", first);
                        parameters[3] = new NameValue("LAST", last);
                        using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                        {
                            dynamicDataAccess.ExecuteSPDataTable("ACL.GET_CLAIM_ESTIMATION_DATA", parameters);
                        }
                    }
                }

                count = 100;
                last = count * 1;
                total = 0;

                parameters[0] = new NameValue("YEAR", year);
                parameters[1] = new NameValue("MONTH", month);
                parameters[2] = new NameValue("FIRST", 0);
                parameters[3] = new NameValue("LAST", last);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_CLAIM_PAYMENT_DATA", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        total = Convert.ToInt32(arrayItem[0]);
                    }
                }

                if (total >= last)
                {
                    for (int i = 2; i <= total / count + 1; i++)
                    {
                        first = last;
                        last = count * i;
                        parameters[0] = new NameValue("YEAR", year);
                        parameters[1] = new NameValue("MONTH", month);
                        parameters[2] = new NameValue("FIRST", first);
                        parameters[3] = new NameValue("LAST", last);
                        using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                        {
                            result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_CLAIM_PAYMENT_DATA", parameters);
                        }
                    }
                }

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_ACCOUNT_CLAIM_RESERVE", null);
                }
                UpdateLogProcess(processId);

                success = (result != null && result.Rows.Count > 0) ? 1 : 0;
            }
            catch (BusinessException)
            {
                success = -1;
            }

            return success;
        }

        /// <summary>
        /// GetClaimClosureReport
        /// </summary>
        /// <returns>List<AccountingClosingReportDto></returns>
        public List<AccountingClosingReportDTO> GetClaimClosureReport()
        {
            List<AccountingClosingReportDTO> reports = new List<AccountingClosingReportDTO>();

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_ACCOUNT_CLAIM_RESERVE_RECORDS", null);
                }
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        AccountingClosingReportDTO accountingClosingReportDto = new AccountingClosingReportDTO();
                        accountingClosingReportDto.BrachCd = Convert.ToInt32(arrayItem[1]);
                        accountingClosingReportDto.PrefixCd = Convert.ToInt32(arrayItem[12]);
                        accountingClosingReportDto.CurrencyCd = Convert.ToInt32(arrayItem[2]);
                        accountingClosingReportDto.ExchangeRate = Convert.ToDecimal(arrayItem[3]);
                        accountingClosingReportDto.TotalAmount = Convert.ToDecimal(arrayItem[9]);
                        accountingClosingReportDto.LocalAmountValue = Convert.ToDecimal(arrayItem[10]);
                        accountingClosingReportDto.BranchDescription = Convert.ToString(arrayItem[11]);
                        accountingClosingReportDto.PrefixDescription = Convert.ToString(arrayItem[13]);
                        accountingClosingReportDto.ConceptId = Convert.ToInt32(arrayItem[6]);
                        accountingClosingReportDto.Description = Convert.ToString(arrayItem[7]);
                        accountingClosingReportDto.AccountNatureCd = Convert.ToString(arrayItem[8]);

                        reports.Add(accountingClosingReportDto);
                    }
                }
            }
            catch (BusinessException)
            {
                reports = new List<AccountingClosingReportDTO>();
            }

            return reports;
        }

        /// <summary>
        /// ClaimClosureEnding 
        /// Realiza el proceso de Contabilización de Siniestros        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        public int ClaimClosureEnding(int year, int month)
        {
            var parameters = new NameValue[2];
            parameters[0] = new NameValue("YEAR", year);
            parameters[1] = new NameValue("MONTH", month);

            try
            {
                // Ejecuta el sp de contabilización   
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.CLAIM_MONTHLY_ENTRY", parameters);
                }

                // Devuelve el número de asiento
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());

                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Claim

        #region ClaimReserve

        /// <summary>
        /// ClaimReserveClosureGeneration
        /// Ejecuta el cierre mensual de reserva en curso de riesgos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        public int ClaimReserveClosureGeneration(int year, int month, int module)
        {
            int success = 0;
            int processId = 0;

            DateTime startDate = Convert.ToDateTime("01/" + month + "/" + Convert.ToString(year) + " " + "00:00:00");
            DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(year, month)) + "/" + month + "/" + Convert.ToString(year) + " " + "23:59:59");
            DateTime firstDayOfYear = Convert.ToDateTime("01/01/" + Convert.ToString(year) + " 00:00:00");
            DateTime annuityFirstDayOfYear = firstDayOfYear.AddYears(-1);

            try
            {
                processId = SaveLogProcess(module);

                var parameters = new NameValue[4];

                parameters[0] = new NameValue("START_DATE", startDate);
                parameters[1] = new NameValue("END_DATE", endDate);
                parameters[2] = new NameValue("FIRST_DAY_OF_YEAR", firstDayOfYear);
                parameters[3] = new NameValue("ANNUITY_FIRST_DAY_OF_YEAR", annuityFirstDayOfYear);

                // Selección de pólizas a procesar
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.RISK_RESERVE_SELECTION", parameters);
                }

                // Se obtiene las coberturas de las pólizas seleccionadas.
                if (result != null && result.Rows.Count > 0)
                {
                    parameters = new NameValue[1];
                    parameters[0] = new NameValue("END_DATE", endDate);

                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        result = dynamicDataAccess.ExecuteSPDataTable("ACL.RISK_RESERVE_SELECTION_COVERAGES", parameters);
                    }


                    if (result != null && result.Rows.Count > 0)
                    {
                        // Se realiza los calculos en la tabla de precierre.
                        foreach (DataRow arrayItem in result.Rows)
                        {
                            decimal calculationPercentage = 0;
                            int daysToEarn = 0;
                            int validityDays = 0;
                            decimal coeficient = 0;
                            decimal reinsuranceGivenPrime = 0;
                            decimal grossReserve = 0;
                            decimal givenReserve = 0;
                            decimal netReserve = 0;
                            calculationPercentage = Convert.ToInt32(arrayItem[13]) == 5 ? 0.5M : 0.8M; //ramo de transportes

                            daysToEarn = Convert.ToInt32((Convert.ToDateTime(arrayItem[26]) - endDate).TotalDays);
                            validityDays = Convert.ToInt32((Convert.ToDateTime(arrayItem[26]) - (Convert.ToDateTime(arrayItem[25]))).TotalDays);
                            coeficient = Decimal.Divide(Convert.ToDecimal(daysToEarn), Convert.ToDecimal(validityDays));

                            // Cálculo de la prima cedida de reaseguros
                            var reinsuranceParameters = new NameValue[3];
                            reinsuranceParameters[0] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(arrayItem[1]));
                            reinsuranceParameters[1] = new NameValue("RISK_NUM", Convert.ToInt32(arrayItem[22]));
                            reinsuranceParameters[2] = new NameValue("COVERAGE_NUM", Convert.ToInt32(arrayItem[27]));

                            DataTable premiums;
                            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                            {
                                premiums = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_REINSURANCE_GIVEN_PRIME", reinsuranceParameters);
                            }

                            if (premiums != null && premiums.Rows.Count > 0)
                            {
                                foreach (DataRow premium in premiums.Rows)
                                {
                                    reinsuranceGivenPrime = Convert.ToDecimal(premium[0]);
                                }
                            }

                            grossReserve = Convert.ToDecimal((Convert.ToDecimal(arrayItem[21])) * calculationPercentage * coeficient);
                            givenReserve = Convert.ToDecimal(reinsuranceGivenPrime * calculationPercentage * coeficient);
                            netReserve = Convert.ToDecimal(grossReserve - givenReserve);

                            //GRABO EL REGISTRO EN LA TABLA TEMPORAL
                            var riskParameters = new NameValue[22];
                            riskParameters[0] = new NameValue("POLICY_ID", Convert.ToInt32(arrayItem[2]));
                            riskParameters[1] = new NameValue("POLICY_DOCUMENT_NUMBER", Convert.ToInt32(arrayItem[11]));
                            riskParameters[2] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(arrayItem[1]));
                            riskParameters[3] = new NameValue("ENDORSEMENT_DOCUMENT_NUMBER", Convert.ToInt32(arrayItem[3]));
                            riskParameters[4] = new NameValue("ENDO_TYPE_CD", Convert.ToInt32(arrayItem[6]));
                            riskParameters[5] = new NameValue("PREFIX_CD", Convert.ToInt32(arrayItem[13]));
                            riskParameters[6] = new NameValue("BRANCH_CD", Convert.ToInt32(arrayItem[12]));
                            riskParameters[7] = new NameValue("CURRENCY_CD", Convert.ToInt32(arrayItem[14]));
                            riskParameters[8] = new NameValue("EXCHANGE_RATE", Convert.ToDecimal(arrayItem[5]));
                            riskParameters[9] = new NameValue("POLICY_ISSUE_DATE", Convert.ToDateTime(arrayItem[15]));
                            riskParameters[10] = new NameValue("POLICYHOLDER_ID", Convert.ToInt32(arrayItem[16]));
                            riskParameters[11] = new NameValue("BUSINESS_TYPE_CD", Convert.ToInt32(arrayItem[19]));
                            riskParameters[12] = new NameValue("COVERAGE_CURRENT_FROM", Convert.ToDateTime(arrayItem[25]));
                            riskParameters[13] = new NameValue("COVERAGE_CURRENT_TO", Convert.ToDateTime(arrayItem[26]));
                            riskParameters[14] = new NameValue("VALIDITY_DAYS", Convert.ToInt32(validityDays));
                            riskParameters[15] = new NameValue("DAYS_TO_EARN", Convert.ToInt32(daysToEarn));
                            riskParameters[16] = new NameValue("COEFFICIENT", Convert.ToDecimal(coeficient));
                            riskParameters[17] = new NameValue("PREMIUM_AMT", Convert.ToDecimal(arrayItem[21]));
                            riskParameters[18] = new NameValue("REINSURANCE_GIVEN_PRIME", Convert.ToDecimal(reinsuranceGivenPrime));
                            riskParameters[19] = new NameValue("GROSS_RESERVE", Convert.ToDecimal(grossReserve));
                            riskParameters[20] = new NameValue("GIVEN_RESERVE", Convert.ToDecimal(givenReserve));
                            riskParameters[21] = new NameValue("NET_RESERVE", Convert.ToDecimal(netReserve));

                            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                            {
                                dynamicDataAccess.ExecuteSPDataTable("ACL.SAVE_TEMP_RISK_RESERVE_POLICY_ENTRY_DATA", riskParameters);
                            }
                        }
                    }



                }

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_TEMP_RISK_RESERVE_POLICY_ENTRY_DATA", null);
                }
                UpdateLogProcess(processId);
                success = (result != null && result.Rows.Count > 0) ? 1 : 0;
            }
            catch (BusinessException)
            {
                // Significa que hubo un error a nivel de bdd
                success = -1;
                UpdateLogProcess(processId);
            }

            return success;
        }

        /// <summary>
        /// GetClaimReserveClosureReport
        /// </summary>
        /// <returns>List<AccountingClosingReportDto></returns>
        public List<AccountingClosingReportDTO> GetClaimReserveClosureReport()
        {
            List<AccountingClosingReportDTO> reports = new List<AccountingClosingReportDTO>();

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_ACCOUNT_RISK_RESERVE_RECORDS", null);
                }
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        AccountingClosingReportDTO accountingClosingReportDto = new AccountingClosingReportDTO();
                        accountingClosingReportDto.PolicyId = Convert.ToInt32(arrayItem[0]);
                        accountingClosingReportDto.PolicyDocumentNumber = Convert.ToString(arrayItem[1]);
                        accountingClosingReportDto.EndorsementId = Convert.ToInt32(arrayItem[2]);
                        accountingClosingReportDto.EndorsementDocumentNumber = Convert.ToString(arrayItem[3]);
                        accountingClosingReportDto.EndorsementTypeId = Convert.ToInt32(arrayItem[4]);
                        accountingClosingReportDto.PrefixCd = Convert.ToInt32(arrayItem[5]);
                        accountingClosingReportDto.PrefixDescription = Convert.ToString(arrayItem[6]);
                        accountingClosingReportDto.BrachCd = Convert.ToInt32(arrayItem[7]);
                        accountingClosingReportDto.BranchDescription = Convert.ToString(arrayItem[8]);
                        accountingClosingReportDto.CurrencyCd = Convert.ToInt32(arrayItem[9]);
                        accountingClosingReportDto.CurrencyDescription = Convert.ToString(arrayItem[10]);
                        accountingClosingReportDto.ExchangeRate = Convert.ToDecimal(arrayItem[11]);
                        accountingClosingReportDto.PayerId = Convert.ToInt32(arrayItem[12]);
                        accountingClosingReportDto.BusinessTypeId = Convert.ToInt32(arrayItem[13]);
                        accountingClosingReportDto.TotalAmount = Convert.ToDecimal(arrayItem[14]);
                        accountingClosingReportDto.LocalAmountValue = Convert.ToDecimal(Convert.ToDecimal(arrayItem[11]) * Convert.ToDecimal(arrayItem[14]));

                        reports.Add(accountingClosingReportDto);
                    }
                }
            }
            catch (BusinessException)
            {
                reports = new List<AccountingClosingReportDTO>();
            }

            return reports;
        }

        /// <summary>
        /// ClaimReserveClosureEnding        
        /// Realiza el proceso de Contabilización de Reserva de siniestros
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        public int ClaimReserveClosureEnding(int year, int month)
        {
            var parameters = new NameValue[2];
            parameters[0] = new NameValue("YEAR", year);
            parameters[1] = new NameValue("MONTH", month);

            try
            {
                // Ejecuta el sp de contabilización   
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.TECHNICAL_RESERVATION_MONTHLY_ENTRY", parameters);
                }

                // Devuelve el número de asiento
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());

                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GenerateClaimReserveEntry
        /// Genera los asientos para el cierre de reservas de riesgos.
        /// </summary>
        /// <param name="AccountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateClaimReserveEntry(List<AccountingClosingReportDTO> AccountingClosings, int moduleId, int userId)
        {
            #region Parameters

            decimal debits = 0;
            decimal credits = 0;
            decimal localDebits = 0;
            decimal localCredits = 0;

            #endregion Parameters

            #region LedgerEntryHeader

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();

            ledgerEntry.Id = 0;
            ledgerEntry.AccountingCompany = new AccountingCompanyDTO() { AccountingCompanyId = accountingCompanyId };
            ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO() { AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES)) };
            ledgerEntry.ModuleDateId = moduleId;
            ledgerEntry.Branch = new BranchDTO() { Id = AccountingClosings[0].BrachCd };
            ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
            ledgerEntry.EntryDestination = new EntryDestinationDTO();
            ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
            ledgerEntry.Description = Resources.Resources.ClaimReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(AccountingClosings[0].PolicyDocumentNumber) + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(AccountingClosings[0].EndorsementDocumentNumber);
            ledgerEntry.EntryNumber = 0;
            ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
            ledgerEntry.RegisterDate = DateTime.Now;
            ledgerEntry.Status = 1; //activo
            ledgerEntry.UserId = userId;

            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            #endregion LedgerEntryHeader

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in AccountingClosings)
            {
                //Detalle
                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                AccountingAccountDTO accountingAccount = new AccountingAccountDTO();
                if (Convert.ToInt32(accountingClosing.AccountingAccountCd) > 0)
                {
                    accountingAccount.AccountingAccountId = Convert.ToInt32(accountingClosing.AccountingAccountCd);
                }
                else
                {
                    AccountingConcept accountingConcept = new AccountingConcept();
                    accountingConcept.Id = accountingClosing.ConceptId;
                    accountingAccount.AccountingAccountId = DelegateService.generalLedgerService.GetAccountingConcept(accountingConcept.ToDTO()).AccountingAccount.AccountingAccountId;
                }

                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();

                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                ledgerEntryItem.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccount(Convert.ToInt32(accountingAccount.AccountingAccountId)).ToDTO();
                ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(accountingClosing.AccountNatureCd);
                ledgerEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                    Value = accountingClosing.TotalAmount
                };
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = accountingClosing.LocalAmountValue };
                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                ledgerEntryItem.Description = Resources.Resources.ClaimReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntryItem.EntryType = new EntryTypeDTO();
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();

                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
            }

            // Calculo el valor para la contraparte.
            decimal counterpartValue = 0;
            decimal counterpartLocalValue = 0;

            foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
            {
                if (ledgerEntryItem.AccountingNature == (int)AccountingNatures.Credit)
                {
                    credits = credits + ledgerEntryItem.Amount.Value;
                    localCredits = localCredits + ledgerEntryItem.LocalAmount.Value;
                }
                else
                {
                    debits = debits + ledgerEntryItem.Amount.Value;
                    localDebits = localDebits + ledgerEntryItem.LocalAmount.Value;
                }
            }

            if (System.Math.Abs(debits) > System.Math.Abs(credits))
            {
                counterpartValue = (System.Math.Abs(debits) - System.Math.Abs(credits)) * -1;
                counterpartLocalValue = (System.Math.Abs(localDebits) - System.Math.Abs(localCredits)) * -1;
            }

            if (System.Math.Abs(debits) < System.Math.Abs(credits))
            {
                counterpartValue = (System.Math.Abs(credits) - System.Math.Abs(debits));
                counterpartLocalValue = (System.Math.Abs(localCredits) - System.Math.Abs(localDebits));
            }

            if (counterpartValue != 0)
            {
                // Se arma la estructura de parámetros para su evaluación.
                List<AccountingRulesModels.Parameter> parameters = new List<AccountingRulesModels.Parameter>();

                parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(counterpartValue, CultureInfo.InvariantCulture) });

                List<Result> results;
                results = DelegateService.entryParameterService.ExecuteAccountingRulePackage(moduleId, parameters.ToDTOs().ToList()).ToModels().ToList();

                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        if (result.Id > 0)
                        {
                            //Detalle
                            LedgerEntryItemDTO counterpartEntry = new LedgerEntryItemDTO();
                            counterpartEntry.AccountingAccount = new AccountingAccountDTO();
                            counterpartEntry.AccountingAccount.Number = result.AccountingAccount;
                            counterpartEntry.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(counterpartEntry.AccountingAccount.ToDTO()).ToList().Count == 0 ? new AccountingAccountDTO() : DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(counterpartEntry.AccountingAccount.ToDTO()).First().ToDTO();
                            counterpartEntry.AccountingNature = (int)result.AccountingNature;
                            counterpartEntry.Amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = ledgerEntry.LedgerEntryItems[0].Amount.Currency.Id },
                                Value = System.Math.Abs(counterpartValue)
                            };
                            counterpartEntry.ExchangeRate = new ExchangeRateDTO() { SellAmount = ledgerEntry.LedgerEntryItems[0].ExchangeRate.SellAmount };
                            counterpartEntry.LocalAmount = new AmountDTO() { Value = System.Math.Abs(counterpartLocalValue) };
                            counterpartEntry.Analysis = new List<AnalysisDTO>();
                            counterpartEntry.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                            counterpartEntry.CostCenters = new List<CostCenterDTO>();
                            counterpartEntry.Currency = new CurrencyDTO() { Id = ledgerEntry.LedgerEntryItems[0].Amount.Currency.Id };
                            counterpartEntry.Description = ledgerEntry.LedgerEntryItems[0].Description;
                            counterpartEntry.EntryType = new EntryTypeDTO();
                            counterpartEntry.Id = 0;
                            counterpartEntry.Individual = new IndividualDTO() { IndividualId = ledgerEntry.LedgerEntryItems[0].Individual.IndividualId };
                            counterpartEntry.PostDated = new List<PostDatedDTO>();
                            counterpartEntry.Receipt = new ReceiptDTO();

                            ledgerEntry.LedgerEntryItems.Add(counterpartEntry);
                        }
                    }
                }
            }

            return ledgerEntry;
        }

        #endregion ClaimReserve

        #region IssuanceDailyEntry

        /// <summary>
        /// GenerateIssuanceEntry
        /// Genera los asientos para el cierre de emisión.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public LedgerEntryDTO GenerateIssuanceEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            #region Parameters

            decimal primeAmount = 0;
            decimal issuanceExpenses = 0;
            decimal surcharges = 0;
            decimal discounts = 0;
            decimal fireDepartmentFee = 0;
            decimal taxes = 0;
            decimal bonuses = 0;
            decimal acceptedCoinsurancePrimeAmount = 0;
            decimal givenCoinsurancePrimeAmount = 0;
            int endorsementTypeId = 0;
            int prefixId = 0;
            int businessTypeId = 0;
            int branchId = 0;
            int currencyId = 0;
            decimal exchangeRate = 0;

            prefixId = accountingClosings[0].PrefixCd;
            endorsementTypeId = accountingClosings[0].EndorsementTypeId;
            businessTypeId = accountingClosings[0].BusinessTypeId;
            branchId = accountingClosings[0].BrachCd;
            currencyId = accountingClosings[0].CurrencyCd;
            exchangeRate = accountingClosings[0].ExchangeRate;

            foreach (var accountingClosing in accountingClosings)
            {
                if (accountingClosing.ComponentId == Convert.ToInt32(Convert.ToString(EnumHelper.GetEnumParameterValue<AccountingClosingComponentType>(AccountingClosingComponentType.ACL_PRIME)))) //prima
                {
                    if (businessTypeId == 1) //100% compañía
                    {
                        primeAmount = accountingClosing.TotalAmount;
                    }
                    if (businessTypeId == 2) //aceptado
                    {
                        acceptedCoinsurancePrimeAmount = accountingClosing.TotalAmount;
                    }
                    if (businessTypeId == 3) //cedido
                    {
                        givenCoinsurancePrimeAmount = accountingClosing.TotalAmount;
                    }
                }
                if (accountingClosing.ComponentId == Convert.ToInt32(Convert.ToString(EnumHelper.GetEnumParameterValue<AccountingClosingComponentType>(AccountingClosingComponentType.ACL_ADMINISTRATIVE_SURCHARGES)))) //recargos administrativos
                {
                    surcharges = surcharges + accountingClosing.TotalAmount;
                }
                if (accountingClosing.ComponentId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingComponentType>(AccountingClosingComponentType.ACL_FINANCIAL_SURCHARGES))) //recargos financieros
                {
                    surcharges = surcharges + accountingClosing.TotalAmount;
                }
                if (accountingClosing.ComponentId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingComponentType>(AccountingClosingComponentType.ACL_ISSUANCE_RIGHTS))) //emisión
                {
                    issuanceExpenses = accountingClosing.TotalAmount;
                }
                if (accountingClosing.ComponentId == Convert.ToInt32(Convert.ToString(EnumHelper.GetEnumParameterValue<AccountingClosingComponentType>(AccountingClosingComponentType.ACL_TAXES)))) //IVA
                {
                    taxes = accountingClosing.TotalAmount;
                }
                if (accountingClosing.ComponentId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingComponentType>(AccountingClosingComponentType.ACL_BONUSES))) //bonificación
                {
                    bonuses = bonuses + +accountingClosing.TotalAmount;
                }
            }

            // Se arma la estructura de parámetros para su evaluación.
            List<AccountingRulesModels.Parameter> parameters = new List<AccountingRulesModels.Parameter>();

            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(primeAmount, CultureInfo.InvariantCulture) }); //prima para seguro 100% compañía
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(endorsementTypeId, CultureInfo.InvariantCulture) }); //tipo de endoso
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(prefixId, CultureInfo.InvariantCulture) }); //tipo de ramo
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(issuanceExpenses, CultureInfo.InvariantCulture) }); //importe gastos emisión
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(surcharges, CultureInfo.InvariantCulture) }); //importe recargos
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(discounts, CultureInfo.InvariantCulture) }); //importe descuento
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(fireDepartmentFee, CultureInfo.InvariantCulture) }); //importe contribución bomberos
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(taxes, CultureInfo.InvariantCulture) }); //importe IVA
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(acceptedCoinsurancePrimeAmount, CultureInfo.InvariantCulture) }); //importe prima coaseguro aceptado
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(givenCoinsurancePrimeAmount, CultureInfo.InvariantCulture) }); //importe prima coaseguro cedido
            parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(currencyId, CultureInfo.InvariantCulture) }); //moneda

            #endregion Parameters

            #region LedgerEntryHeader

            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            ledgerEntry.AccountingCompany = new AccountingCompanyDTO() { AccountingCompanyId = accountingCompanyId };
            ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
            ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO() { AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES)) };

            ledgerEntry.Branch = new BranchDTO()
            {
                Id = DelegateService.uniqueUserService.GetBranchesByUserId(userId).Where(br => br.IsDefault).ToList()[0].Id
            };

            ledgerEntry.Description = Resources.Resources.IssuanceAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosings[0].PolicyDocumentNumber)
                                            + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosings[0].EndorsementDocumentNumber)
                                            + ", " + Resources.Resources.Branch + ": " + Convert.ToString(accountingClosings[0].BranchDescription)
                                            + ", " + Resources.Resources.Prefix + ": " + Convert.ToString(accountingClosings[0].PrefixDescription);
            ledgerEntry.EntryDestination = new EntryDestinationDTO();
            ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
            ledgerEntry.ModuleDateId = moduleId;
            ledgerEntry.RegisterDate = DateTime.Now;
            ledgerEntry.EntryNumber = 0;
            ledgerEntry.Id = 0;
            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            #endregion LedgerEntryHeader

            #region LedgerEntryItem

            List<Result> results;

            results = DelegateService.entryParameterService.ExecuteAccountingRulePackage(moduleId, parameters.ToDTOs().ToList()).ToModels().ToList();

            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    if (result.Id > 0)
                    {
                        //Detalle
                        LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                        ledgerEntryItem.AccountingAccount = new AccountingAccountDTO() { Number = result.AccountingAccount };
                        ledgerEntryItem.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(ledgerEntryItem.AccountingAccount.ToDTO()).ToList().Count == 0 ? new AccountingAccountDTO() : DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(ledgerEntryItem.AccountingAccount.ToDTO()).First().ToDTO();
                        ledgerEntryItem.AccountingNature = (int)result.AccountingNature;
                        ledgerEntryItem.Amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = currencyId },
                            Value = Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture)
                        };
                        ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = exchangeRate };
                        ledgerEntryItem.LocalAmount = new AmountDTO() { Value = Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture) };
                        ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                        ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                        ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                        ledgerEntryItem.Currency = new CurrencyDTO { Id = currencyId };
                        ledgerEntryItem.Description = Resources.Resources.IssuanceAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosings[0].PolicyDocumentNumber)
                                                        + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosings[0].EndorsementDocumentNumber)
                                                        + ", " + Resources.Resources.Branch + ": " + Convert.ToString(accountingClosings[0].BranchDescription)
                                                        + ", " + Resources.Resources.Prefix + ": " + Convert.ToString(accountingClosings[0].PrefixDescription);
                        ledgerEntryItem.EntryType = new EntryTypeDTO() { EntryTypeId = 0 };
                        ledgerEntryItem.Id = 0;
                        ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosings[0].PayerId };
                        ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                        ledgerEntryItem.Receipt = new ReceiptDTO();
                        ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                        ledgerEntry.ModuleDateId = moduleId;
                        ledgerEntry.RegisterDate = DateTime.Now;
                        ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                        ledgerEntry.Status = 1; //activo
                        ledgerEntry.UserId = userId;
                    }
                }
            }

            #endregion LedgerEntryItem

            return ledgerEntry;
        }

        #endregion IssuanceDailyEntry

        #region Issuance

        public List<string> AccountClosureIssuance(DateTime accountingDate, int module, int userId, int day)
        {

            List<string> entryNumbers = new List<string>();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            var entryNumber = 0;
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            string accoutingMonth = Convert.ToString(accountingDate.Month).Length == 2 ? Convert.ToString(accountingDate.Month) : "0" + Convert.ToString(accountingDate.Month);

            DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "00:00:00");
            DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month)) + "/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "23:59:59");

            // Se obtiene los parámetros para generar el asiento
            closures = GetIssuanceClosureReportParameters(startDate, endDate, module);

            if (closures.Count > 0)
            {
                // Se obtiene el listado de pólizas y endosos.
                List<AccountingClosingReportDTO> accountingClosingReportDTOs;
                accountingClosingReportDTOs = closures.GroupBy(p => new { p.PolicyId, p.EndorsementId }).Select(g => g.First()).ToList();

                foreach (var accountingClosingReport in accountingClosingReportDTOs)
                {
                    // Se filtra por póliza y endoso
                    List<AccountingClosingReportDTO> accountingClosingReports;

                    accountingClosingReports = (from AccountingClosingReportDTO item in closures where item.PolicyId == accountingClosingReport.PolicyId && item.EndorsementId == accountingClosingReport.EndorsementId select item).ToList();

                    // Se obtiene el asiento
                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateIssuanceEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                    entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                    entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                    entryHeader.Branch = ledgerEntry.Branch;
                    entryHeader.Description = ledgerEntry.Description;
                    entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                    entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                    entryHeader.Id = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        entryHeader.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                    entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                    entryHeader.SalePoint = ledgerEntry.SalePoint;
                    entryHeader.Status = ledgerEntry.Status;
                    entryHeader.UserId = ledgerEntry.UserId;
                }

                // Se valida débitos y créditos
                decimal debits = 0;
                decimal credits = 0;

                foreach (LedgerEntryItemDTO ledgerEntryItem in entryHeader.LedgerEntryItems)
                {
                    if (ledgerEntryItem.AccountingNature == (int)AccountingNatures.Debit)
                    {
                        debits = debits + ledgerEntryItem.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + ledgerEntryItem.LocalAmount.Value;
                    }
                }

                if (debits == credits)
                {
                    // Se graba el asiento
                    if (entryHeader.LedgerEntryItems.Count > 10)
                    {
                        // Se borra los datos de la tabla temporal de trabajo
                        DelegateService.generalLedgerService.ClearTempAccountEntry();

                        DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                        entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                    }
                    else
                    {
                        entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                    }

                    if (entryNumber > 0)
                    {
                        entryNumbers.Add(" " + entryNumber);
                        IssuanceClosureEnding(accountingDate.Year, accountingDate.Month, day);
                    }
                    else
                    {
                        entryNumbers.Add(Resources.Resources.EntryRecordingError);
                    }
                }
            }
            return entryNumbers;

        }

        /// <summary>
        /// IssuanceClosureGeneration
        /// Ejecuta el cierre mensual de Emisión
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        public int IssuanceClosureGeneration(int year, int month, int day, int module)
        {
            try
            {
                string accoutingMonth = Convert.ToString(month).Length == 2 ? Convert.ToString(month) : "0" + Convert.ToString(month);

                DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(year) + " " + "00:00:00");
                DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(year, month)) + "/" + accoutingMonth + "/" + Convert.ToString(year) + " " + "23:59:59");

                int processId = SaveLogProcess(module);

                var parameters = new NameValue[2];

                parameters[0] = new NameValue("START_DATE", startDate);
                parameters[1] = new NameValue("END_DATE", endDate);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_ISSUANCE_ACCOUNTING_PARAMETERS", parameters);
                }

                UpdateLogProcess(processId);

                if (result != null && result.Rows.Count > 0)
                {
                    return 1;
                }

                return 0;
            }
            catch (BusinessException)
            {
                return -1;
            }
        }

        /// <summary>
        /// GetIssuanceClosureReportParameters
        /// Método Creado solo para EE
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="moduleId"></param>
        /// <returns>List<AccountingClosingReportDto/></returns>
        public List<AccountingClosingReportDTO> GetIssuanceClosureReportParameters(DateTime startDate, DateTime endDate, int moduleId)
        {
            List<AccountingClosingReportDTO> reports = new List<AccountingClosingReportDTO>();

            try
            {
                var parameters = new NameValue[2];

                parameters[0] = new NameValue("START_DATE", startDate);
                parameters[1] = new NameValue("END_DATE", endDate);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_ISSUANCE_ACCOUNTING_PARAMETERS", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        AccountingClosingReportDTO accountingClosingReportDto = new AccountingClosingReportDTO();
                        accountingClosingReportDto.PolicyId = Convert.ToInt32(arrayItem[0]);
                        accountingClosingReportDto.EndorsementId = Convert.ToInt32(arrayItem[1]);
                        accountingClosingReportDto.BrachCd = Convert.ToInt32(arrayItem[2]);
                        accountingClosingReportDto.PrefixCd = Convert.ToInt32(arrayItem[3]);
                        accountingClosingReportDto.CurrencyCd = Convert.ToInt32(arrayItem[4]);
                        accountingClosingReportDto.ExchangeRate = Convert.ToDecimal(arrayItem[5]);
                        accountingClosingReportDto.EndorsementTypeId = Convert.ToInt32(arrayItem[6]);
                        accountingClosingReportDto.BusinessTypeId = Convert.ToInt32(arrayItem[7]);
                        accountingClosingReportDto.TotalAmount = Convert.ToDecimal(arrayItem[8]);
                        accountingClosingReportDto.ComponentId = Convert.ToInt32(arrayItem[9]);
                        accountingClosingReportDto.ComponentSmallDescription = Convert.ToString(arrayItem[10]);
                        accountingClosingReportDto.PayerId = Convert.ToInt32(arrayItem[11]);
                        accountingClosingReportDto.PolicyDocumentNumber = Convert.ToString(arrayItem[12]);
                        accountingClosingReportDto.EndorsementDocumentNumber = Convert.ToString(arrayItem[13]);
                        accountingClosingReportDto.BranchDescription = Convert.ToString(arrayItem[14]);
                        accountingClosingReportDto.PrefixDescription = Convert.ToString(arrayItem[15]);

                        reports.Add(accountingClosingReportDto);
                    }
                }
            }
            catch (BusinessException)
            {
                reports = new List<AccountingClosingReportDTO>();
            }

            return reports;
        }

        /// <summary>
        /// IssuanceClosureEnding
        /// Realiza el proceso de Contabilización de Emisión
        /// Devuelve el Nro. de asiento generado        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>int</returns>
        public int IssuanceClosureEnding(int year, int month, int day)
        {
            var parameters = new NameValue[3];
            parameters[0] = new NameValue("YEAR", year);
            parameters[1] = new NameValue("MONTH", month);
            parameters[2] = new NameValue("DAY", day);

            try
            {
                // Ejecuta el sp de contabilización   
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.ISSUANCE_DAMAGES_MONTHLY_ENTRY", parameters);
                }

                // Devuelve el número de asiento
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());

                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public void GenerateIssuanceModule(DateTime accountingDate, int module)
        {
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            string accoutingMonth = Convert.ToString(accountingDate.Month).Length == 2 ? Convert.ToString(accountingDate.Month) : "0" + Convert.ToString(accountingDate.Month);

            DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "00:00:00");
            DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month)) + "/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "23:59:59");
            // Se obtiene los parámetros para generar el asiento
            closures = GetIssuanceClosureReportParameters(startDate, endDate, module);


        }

        #endregion Issuance

        #region Reinsurance

        /// <summary>
        /// ReinsuranceClosureEnding
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// </summary>
        /// <returns>int</returns>
        public int ReinsuranceClosureEnding(int year, int month)
        {
            int success;

            try
            {
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("YEAR", year);
                parameters[1] = new NameValue("MONTH", month);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("REINS.GENERATE_CLOSURE_RECORDING_FINAL_TABLES", parameters);
                }

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.REINSURANCE_MONTHLY_ENTRY", parameters);
                }

                success = 1;
            }
            catch (BusinessException)
            {
                success = 0;
            }

            return success;
        }

        /// <summary>
        /// ReinsuranceClosureGeneration
        /// Ejecuta el pre cierre de Reservas de Reaseguros
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        public int ReinsuranceClosureGeneration(int year, int month, int day, int module)
        {
            int processId = 0;

            try
            {
                processId = SaveLogProcess(module);

                NameValue[] parameters = new NameValue[4];
                parameters[0] = new NameValue("YEAR", year);
                parameters[1] = new NameValue("MONTH", month);
                parameters[2] = new NameValue("TRANSACTION_ID", DBNull.Value); //nulo para que ejecute proceso batch
                parameters[3] = new NameValue("MODULE_ID", DBNull.Value);      //nulo para que ejecute proceso batch

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    // Se ejecuta el pre-cierre
                    dynamicDataAccess.ExecuteSPDataTable("REINS.GENERATE_CLOSURE", parameters);
                }
                return processId;
            }
            catch
            {
                // Si hay un error en el SP se cambia el estado del proceso a 0 para que no siga ejecutando en la parte web
                UpdateLogProcess(processId);

                // Significa que hubo un error a nivel de bdd
                return -1;
            }
        }

        /// <summary>
        /// GetReinsuranceClosureReport
        /// </summary>
        /// <returns>List<AccountingClosingReportDto/></returns>
        public List<AccountingClosingReportDTO> GetReinsuranceClosureReport()
        {
            List<AccountingClosingReportDTO> reports = new List<AccountingClosingReportDTO>();

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("REINS.GET_CLOSURE_RECORDING_FINAL_TABLES", null);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        AccountingClosingReportDTO accountingClosingReportDto = new AccountingClosingReportDTO();
                        accountingClosingReportDto.PolicyId = Convert.ToInt32(arrayItem[25]);
                        accountingClosingReportDto.EndorsementId = Convert.ToInt32(arrayItem[26]);
                        accountingClosingReportDto.BrachCd = Convert.ToInt32(arrayItem[2]);
                        accountingClosingReportDto.PrefixCd = Convert.ToInt32(arrayItem[28]);
                        accountingClosingReportDto.CurrencyCd = Convert.ToInt32(arrayItem[18]);
                        accountingClosingReportDto.ExchangeRate = Convert.ToDecimal(arrayItem[19]);
                        accountingClosingReportDto.TotalAmount = Convert.ToDecimal(arrayItem[21]);
                        accountingClosingReportDto.PayerId = Convert.ToInt32(arrayItem[8]);
                        accountingClosingReportDto.PolicyDocumentNumber = Convert.ToString(arrayItem[30]);
                        accountingClosingReportDto.EndorsementDocumentNumber = Convert.ToString(arrayItem[31]);
                        accountingClosingReportDto.BranchDescription = Convert.ToString(arrayItem[32]);
                        accountingClosingReportDto.PrefixDescription = Convert.ToString(arrayItem[33]);
                        accountingClosingReportDto.ContractTypeId = Convert.ToInt32(arrayItem[11]);
                        accountingClosingReportDto.CompanyTypeId = Convert.ToInt32(arrayItem[29]);
                        accountingClosingReportDto.ConceptId = Convert.ToInt32(arrayItem[16]);

                        reports.Add(accountingClosingReportDto);
                    }
                }
            }
            catch (BusinessException)
            {
                reports = new List<AccountingClosingReportDTO>();
            }

            return reports;
        }

        /// <summary>
        /// GetReinsuranceClosureReportRecords
        /// Obtiene el número de registros procesados del cierre mensual de Reaseguros
        /// </summary>
        /// <returns></returns>
        public int GetReinsuranceClosureReportRecords()
        {
            int rows = 0;

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("REINS.GET_CLOSURE_RECORDING_FINAL_TABLES", null);
                }

                rows = (result != null && result.Rows.Count > 0) ? result.Rows.Count : 0;
            }
            catch (BusinessException)
            {
                rows = 0;
            }

            return rows;
        }

        /// <summary>
        /// GetReinsurancePaginatedClosureReport
        /// Obtiene los registros paginados del cierre mensual de Reaseguros debido a los limitantes de enviar muchos registros de servicio a controlador
        /// </summary>
        /// <returns>List<AccountingClosingReportDto/></returns>
        public List<AccountingClosingReportDTO> GetReinsurancePaginatedClosureReport(int pageSize, int pageNumber, int records)
        {
            List<AccountingClosingReportDTO> reports = new List<AccountingClosingReportDTO>();

            var parameters = new NameValue[4];
            parameters[0] = new NameValue("PageSize", pageSize);
            parameters[1] = new NameValue("PageNumber", pageNumber);
            parameters[2] = new NameValue("PageCount", 0);
            parameters[3] = new NameValue("RecordCount", records);

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_REINSURANCE_CLOSURE_PAGINATED_RECORDS", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        AccountingClosingReportDTO accountingClosingReportDto = new AccountingClosingReportDTO();
                        accountingClosingReportDto.PolicyId = Convert.ToInt32(arrayItem[25]);
                        accountingClosingReportDto.EndorsementId = Convert.ToInt32(arrayItem[26]);
                        accountingClosingReportDto.BrachCd = Convert.ToInt32(arrayItem[2]);
                        accountingClosingReportDto.PrefixCd = Convert.ToInt32(arrayItem[28]);
                        accountingClosingReportDto.CurrencyCd = Convert.ToInt32(arrayItem[18]);
                        accountingClosingReportDto.ExchangeRate = Convert.ToDecimal(arrayItem[19]);
                        accountingClosingReportDto.TotalAmount = Convert.ToDecimal(arrayItem[21]);
                        accountingClosingReportDto.PayerId = Convert.ToInt32(arrayItem[8]);
                        accountingClosingReportDto.PolicyDocumentNumber = Convert.ToString(arrayItem[30]);
                        accountingClosingReportDto.EndorsementDocumentNumber = Convert.ToString(arrayItem[31]);
                        accountingClosingReportDto.BranchDescription = Convert.ToString(arrayItem[32]);
                        accountingClosingReportDto.PrefixDescription = Convert.ToString(arrayItem[33]);
                        accountingClosingReportDto.ContractTypeId = Convert.ToInt32(arrayItem[11]);
                        accountingClosingReportDto.CompanyTypeId = Convert.ToInt32(arrayItem[29]);
                        accountingClosingReportDto.ConceptId = Convert.ToInt32(arrayItem[16]);

                        reports.Add(accountingClosingReportDto);
                    }
                }
            }
            catch (BusinessException)
            {
                reports = new List<AccountingClosingReportDTO>();
            }

            return reports;
        }

        public List<string> AccountClosureReinsurance(DateTime accountingDate, int module, int userId)
        {

            List<string> entryNumbers = new List<string>();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            var entryNumber = 0;
            // Se graba los registros en la tabla de cuenta corriente de reaseguros
            ReinsuranceClosureEnding(accountingDate.Year, accountingDate.Month);

            // Se obtiene el id de proceso
            int processId = GetAccountingClosing(module).Id;

            // Tamaño de la página
            int pageSize = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_REINSURANCE_ACCOUNTING_CLOSING_PAGE_SIZE_PARAMETER));

            // Se obtiene el número de registros.
            int records = GetGeneratedRecordsCount(module, processId);

            // Se obtiene el número de páginas.
            decimal pages = System.Math.Ceiling(Convert.ToDecimal(records) / Convert.ToDecimal(pageSize));

            // Se carga los registros en una lista.
            for (int i = 1; i <= pages; i++)
            {
                var accountingClosingReports = GetGeneratedClosureReportRecords(module, processId, pageSize, i, records);

                if (accountingClosingReports.Count > 0)
                {
                    foreach (var item in accountingClosingReports)
                    {
                        closures.Add(item);
                    }
                }
            }

            // Se filtra los registros por número de asiento en los registros de pre-cierre.
            var closuresResult = closures.GroupBy(x => x.EntryNumber).Select(y => y.First()).ToList();

            // Se armo los asientos
            foreach (var accountingClosing in closuresResult)
            {
                accountingClosing.ModuleId = module;
                var filteredEntry = (from tempEntryRecord in closures where tempEntryRecord.EntryNumber == accountingClosing.EntryNumber select tempEntryRecord).ToList();

                LedgerEntryDTO entry = new LedgerEntryDTO();
                entry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                if (filteredEntry.Count > 0)
                {
                    foreach (var entryItem in filteredEntry)
                    {
                        //Cabecera
                        entry.AccountingCompany = new AccountingCompanyDTO { AccountingCompanyId = entryItem.AccountingCompanyId };
                        entry.AccountingDate = entryItem.Date;
                        entry.AccountingMovementType = new AccountingMovementTypeDTO()
                        {
                            AccountingMovementTypeId = entryItem.AccountingMovementTypeId
                        };
                        entry.Branch = new BranchDTO() { Id = entryItem.BrachCd };
                        entry.Description = entryItem.Description;
                        entry.EntryDestination = new EntryDestinationDTO() { DestinationId = entryItem.EntryDestinationId };
                        entry.EntryNumber = 0;
                        entry.Id = 0;

                        //Detalle
                        LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                        ledgerEntryItem.AccountingAccount = new AccountingAccountDTO()
                        {
                            AccountingAccountId = Convert.ToInt32(entryItem.AccountingAccountCd)
                        };
                        ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(entryItem.AccountNatureCd);
                        ledgerEntryItem.Amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = entryItem.CurrencyCd },
                            Value = entryItem.TotalAmount
                        };
                        ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = entryItem.ExchangeRate };
                        ledgerEntryItem.LocalAmount = new AmountDTO() { Value = entryItem.LocalAmountValue };
                        ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                        ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = entryItem.BankReconciliationId };
                        ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                        ledgerEntryItem.Currency = new CurrencyDTO() { Id = entryItem.CurrencyCd };
                        ledgerEntryItem.Description = entryItem.Description;
                        ledgerEntryItem.EntryType = new EntryTypeDTO() { EntryTypeId = 0 };
                        ledgerEntryItem.Id = 0;
                        ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = entryItem.PayerId };
                        ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                        ledgerEntryItem.Receipt = new ReceiptDTO();
                        ledgerEntryItem.Receipt.Number = entryItem.ReceiptNumber;
                        ledgerEntryItem.Receipt.Date = entryItem.ReceiptDate;
                        entry.LedgerEntryItems.Add(ledgerEntryItem);
                        entry.ModuleDateId = entryItem.ModuleId;
                        entry.RegisterDate = DateTime.Now;
                        entry.Status = 1; //activo
                        entry.UserId = userId;
                    }
                }
                ledgerEntries.Add(entry);
            }

            entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
            {
                entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                entryHeader.Branch = ledgerEntry.Branch;
                entryHeader.Description = ledgerEntry.Description;
                entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                entryHeader.Id = 0;

                foreach (LedgerEntryItemDTO accountingEntryItem in ledgerEntry.LedgerEntryItems)
                {
                    entryHeader.LedgerEntryItems.Add(accountingEntryItem);
                }

                entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                entryHeader.SalePoint = ledgerEntry.SalePoint;
                entryHeader.Status = ledgerEntry.Status;
                entryHeader.UserId = ledgerEntry.UserId;
            }

            // Se valida débitos y créditos
            decimal debits = 0;
            decimal credits = 0;

            foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
            {
                if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                {
                    debits = debits + accountingEntry.LocalAmount.Value;
                }
                else
                {
                    credits = credits + accountingEntry.LocalAmount.Value;
                }
            }

            if (debits == credits)
            {
                // Se graba el asiento
                if (entryHeader.LedgerEntryItems.Count > 10)
                {
                    // Se borra los datos de la tabla temporal de trabajo
                    DelegateService.generalLedgerService.ClearTempAccountEntry();

                    DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                    entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                }
                else
                {
                    entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                }

                if (entryNumber > 0)
                {
                    entryNumbers.Add(" " + entryNumber);
                }
                else
                {
                    entryNumbers.Add(Resources.Resources.EntryRecordingError);
                }
            }
            return entryNumbers;

        }

        /// <summary>
        /// SaveTemporalEntryRecord
        /// Método para grabar el movimiento en la tabla temporal
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="temporalEntryNumber"></param>
        /// <param name="processId"></param>
        /// <param name="module"></param>
        public void SaveTemporalEntryRecord(LedgerEntryDTO ledgerEntry, int temporalEntryNumber, int processId, int module, int userId)
        {
            ClosureTempEntryGenerationDTO tempEntry = new ClosureTempEntryGenerationDTO();
            tempEntry.ProcessId = processId;
            tempEntry.BranchId = ledgerEntry.Branch.Id;
            tempEntry.SalePointId = ledgerEntry.SalePoint.Id;
            tempEntry.AccountingCompanyId = ledgerEntry.AccountingCompany.AccountingCompanyId;
            tempEntry.Amount = ledgerEntry.LedgerEntryItems[0].Amount.Value;
            tempEntry.CurrencyId = ledgerEntry.LedgerEntryItems[0].Currency.Id;
            tempEntry.ExchangeRate = ledgerEntry.LedgerEntryItems[0].ExchangeRate.SellAmount;
            tempEntry.LocalAmount = ledgerEntry.LedgerEntryItems[0].LocalAmount.Value;
            tempEntry.IndividualId = ledgerEntry.LedgerEntryItems[0].Individual.IndividualId;
            tempEntry.AccountingAccountId = ledgerEntry.LedgerEntryItems[0].AccountingAccount.AccountingAccountId;
            tempEntry.AccountingNature = Convert.ToInt32(ledgerEntry.LedgerEntryItems[0].AccountingNature);
            tempEntry.Description = ledgerEntry.LedgerEntryItems[0].Description;
            tempEntry.Date = ledgerEntry.AccountingDate; // String.Format("{0:dd/MM/yyyy HH:mm:ss}"
            tempEntry.AccountingMovementTypeId = ledgerEntry.AccountingMovementType.AccountingMovementTypeId;
            tempEntry.AccountingModuleId = module;
            tempEntry.BankReconciliationId = ledgerEntry.LedgerEntryItems[0].ReconciliationMovementType.Id;
            tempEntry.ReceiptNumber = ledgerEntry.LedgerEntryItems[0].Receipt.Number ?? 0;
            tempEntry.ReceiptDate = Convert.ToDateTime(ledgerEntry.LedgerEntryItems[0].Receipt.Date); //String.Format("{0:dd/MM/yyyy HH:mm:ss}"
            tempEntry.PaymentMovementTypeId = ledgerEntry.AccountingMovementType.AccountingMovementTypeId;
            tempEntry.EntryDestinationId = ledgerEntry.EntryDestination.DestinationId;
            tempEntry.IsDailyEntry = true;
            tempEntry.EntryNumber = temporalEntryNumber;
            tempEntry.UserId = userId;
            tempEntry.ReconciliationId = 0;
            tempEntry.ReconciliationDate = Convert.ToDateTime("01/01/1900");
            tempEntry.DueDate = Convert.ToDateTime("01/01/1900");

            SaveClosureTempEntryGeneration(tempEntry);
        }


        public void GenerateReinsuranceEntry(List<AccountingClosingReportDTO> accountingClosings, int temporalEntryNumber, int processId, int moduleId, int userId)
        { // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in accountingClosings)
            {
                if (accountingClosing.TotalAmount != 0)
                {
                    #region ledgerEntryHeader

                    int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

                    LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
                    ledgerEntry.Id = 0;
                    ledgerEntry.AccountingCompany = new AccountingCompanyDTO() { AccountingCompanyId = accountingCompanyId };
                    ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO() { AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES)) };
                    ledgerEntry.ModuleDateId = moduleId;
                    ledgerEntry.Branch = new BranchDTO() { Id = accountingClosing.BrachCd };
                    ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                    ledgerEntry.EntryDestination = new EntryDestinationDTO() { DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL)) };
                    ledgerEntry.Description = Resources.Resources.ReinsuranceAccounting + " -" + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber) + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber) + Resources.Resources.Branch + ": " + Convert.ToString(accountingClosing.BranchDescription);
                    ledgerEntry.EntryNumber = 0;
                    ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING_CLOSING)), DateTime.Now);
                    ledgerEntry.RegisterDate = DateTime.Now;
                    ledgerEntry.Status = 1; //activo
                    ledgerEntry.UserId = userId;

                    ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                    #endregion ledgerEntryHeader

                    #region ledgerEntryHeaderItem

                    // Se arma la estructura de parámetros para su evaluación.
                    List<AccountingRulesModels.Parameter> parameters = new List<AccountingRulesModels.Parameter>();

                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.TotalAmount, CultureInfo.InvariantCulture) }); //amount
                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.CompanyTypeId, CultureInfo.InvariantCulture) }); //company_type_cd
                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.ContractTypeId, CultureInfo.InvariantCulture) }); //contract_type_cd
                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.ConceptId, CultureInfo.InvariantCulture) }); //concept_cd

                    List<Result> results;
                    results = DelegateService.entryParameterService.ExecuteAccountingRulePackage(moduleId, parameters.ToDTOs().ToList()).ToModels().ToList();

                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            if (result.Id > 0)
                            {
                                //Detalle
                                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                                ledgerEntryItem.Id = 0;
                                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                                ledgerEntryItem.AccountingAccount.Number = result.AccountingAccount;
                                ledgerEntryItem.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(ledgerEntryItem.AccountingAccount.ToDTO()).ToList().Count == 0 ? new AccountingAccountDTO() : DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(ledgerEntryItem.AccountingAccount.ToDTO()).First().ToDTO();
                                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                                ledgerEntryItem.Receipt = new ReceiptDTO();
                                ledgerEntryItem.AccountingNature = (int)result.AccountingNature;
                                ledgerEntryItem.Description = Resources.Resources.ReinsuranceAccounting + " -" + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber) + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber) + Resources.Resources.Branch + ": " + Convert.ToString(accountingClosing.BranchDescription);
                                ledgerEntryItem.Amount = new AmountDTO()
                                {
                                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                                    Value = Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture)
                                };
                                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture) };
                                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                                ledgerEntryItem.EntryType = new EntryTypeDTO { EntryTypeId = -1 };
                                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                                ledgerEntryItem.PostDated = new List<PostDatedDTO>();

                                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                            }
                        }
                    }

                    #endregion ledgerEntryHeaderItem

                    // Se graba en la tabla temporal
                    SaveTemporalEntryRecord(ledgerEntry, temporalEntryNumber, processId, moduleId, userId);
                }
            }
        }

        /// <summary>
        /// ReinsuranceClosureGeneration
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public int ReinsuranceGenerationClosure(int year, int month, int day, int module,int userId)
        {
            int processId = 0;
            int records = 0;

            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();

            try
            {
                // Se genera los datos y se obtiene el id de proceso
                processId = ReinsuranceClosureGeneration(year, month, day, module);

                // Se genera los asientos y se graba los asientos con el id de proceso
                if (processId > 0)
                {
                    // Tamaño de la página
                    int pageSize = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_REINSURANCE_ACCOUNTING_CLOSING_PAGE_SIZE_PARAMETER));

                    // Se obtiene el número de registros.
                    records = GetReinsuranceClosureReportRecords();

                    if (records > 0)
                    {
                        // Se obtiene el número de páginas.
                        decimal pages = System.Math.Ceiling(Convert.ToDecimal(records) / Convert.ToDecimal(pageSize));

                        for (int i = 1; i <= pages; i++)
                        {
                            var accountingClosings = GetReinsurancePaginatedClosureReport(pageSize, i, records);

                            if (accountingClosings.Count > 0)
                            {
                                foreach (var accountingClosing in accountingClosings)
                                {
                                    accountingClosing.ModuleId = module;
                                    closures.Add(accountingClosing);
                                }
                            }
                        }

                        // Se obtiene el listado de pólizas y endosos.
                        List<AccountingClosingReportDTO> reinsurances = closures.GroupBy(p => new { p.PolicyId, p.EndorsementId }).Select(g => g.First()).ToList();

                        int tempEntryNumber = 0;

                        foreach (var reinsuranceItem in reinsurances)
                        {
                            // Se filtra por póliza y endoso
                            List<AccountingClosingReportDTO> accountingClosingByPolicyByEndorsement;

                            accountingClosingByPolicyByEndorsement = (from AccountingClosingReportDTO item in closures where item.PolicyId == reinsuranceItem.PolicyId && item.EndorsementId == reinsuranceItem.EndorsementId select item).ToList();

                            tempEntryNumber = tempEntryNumber + 1;

                            // Se graba en la temporal y se obtiene el asiento.                    
                            GenerateReinsuranceEntry(accountingClosingByPolicyByEndorsement, tempEntryNumber, processId, module,userId);
                        }
                    }
                    // Se actualiza el estado del proceso.
                    UpdateLogProcess(processId);
                }
            }
            catch (Exception)
            {
                records = 0;
            }

            return records;
        }

        #endregion Reinsurance

        #region RiskReserveDailyEntry

        /// <summary>
        /// GenerateRiskReserveEntry
        /// Genera los asientos para el cierre de reservas de riesgos.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateRiskReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            #region Parameters

            #endregion Parameters

            #region LedgerHeader

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();

            ledgerEntry.Id = 0;
            ledgerEntry.AccountingCompany = new AccountingCompanyDTO() { AccountingCompanyId = accountingCompanyId };
            ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO() { AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES)) };
            ledgerEntry.ModuleDateId = moduleId;
            ledgerEntry.Branch = new BranchDTO() { Id = accountingClosings[0].BrachCd };
            ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
            ledgerEntry.EntryDestination = new EntryDestinationDTO();
            ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
            ledgerEntry.Description = Resources.Resources.RiskReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosings[0].PolicyDocumentNumber)
                                            + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosings[0].EndorsementDocumentNumber);
            ledgerEntry.EntryNumber = 0;
            ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
            ledgerEntry.RegisterDate = DateTime.Now;
            ledgerEntry.Status = 1; //activo
            ledgerEntry.UserId = userId;

            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            #endregion LedgerHeader

            #region LedgerItems

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in accountingClosings)
            {
                if (accountingClosing.TotalAmount != 0)
                {
                    // Se arma la estructura de parámetros para su evaluación.
                    List<AccountingRulesModels.Parameter> parameters = new List<AccountingRulesModels.Parameter>();

                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.TotalAmount, CultureInfo.InvariantCulture) }); //valor
                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.CurrencyCd, CultureInfo.InvariantCulture) }); //moneda
                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.EndorsementTypeId, CultureInfo.InvariantCulture) }); //tipo de endoso
                    parameters.Add(new AccountingRulesModels.Parameter() { Value = Convert.ToString(accountingClosing.BusinessTypeId, CultureInfo.InvariantCulture) }); //tipo de negocio

                    List<Result> results;
                    results = DelegateService.entryParameterService.ExecuteAccountingRulePackage(moduleId, parameters.ToDTOs().ToList()).ToModels().ToList();

                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            if (result.Id > 0)
                            {
                                //Detalle
                                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                                ledgerEntryItem.AccountingAccount.Number = result.AccountingAccount;
                                ledgerEntryItem.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(ledgerEntryItem.AccountingAccount.ToDTO()).ToList().Count == 0 ? new AccountingAccountDTO() : DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(ledgerEntryItem.AccountingAccount.ToDTO()).First().ToDTO();
                                ledgerEntryItem.AccountingNature = (int)result.AccountingNature;
                                ledgerEntryItem.Amount = new AmountDTO()
                                {
                                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                                    Value = System.Math.Abs(Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture))
                                };
                                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = System.Math.Abs(Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture)) };
                                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                                ledgerEntryItem.Description = Resources.Resources.RiskReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                                ledgerEntryItem.EntryType = new EntryTypeDTO();
                                ledgerEntryItem.Id = 0;
                                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                                ledgerEntryItem.Receipt = new ReceiptDTO();
                                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                                // Calculo el valor para la contraparte.
                                AccountingNatures counterpartAccountingNature;
                                counterpartAccountingNature = result.AccountingNature == AccountingNatures.Credit ? AccountingNatures.Debit : AccountingNatures.Credit;

                                AccountingAccountDTO counterpartAccountingAccount = new AccountingAccountDTO();
                                counterpartAccountingAccount.AccountingAccountId = Convert.ToInt32(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_CLOSING_RISK_RESERVE_ACCOUNTING_ACCOUNT_ID)));
                                counterpartAccountingAccount.AccountingNature = (int)counterpartAccountingNature;

                                LedgerEntryItemDTO counterpartEntry = new LedgerEntryItemDTO();
                                counterpartEntry.AccountingAccount = new AccountingAccountDTO();
                                counterpartEntry.AccountingAccount = counterpartAccountingAccount;
                                counterpartEntry.AccountingNature = counterpartAccountingAccount.AccountingNature;
                                counterpartEntry.Amount = new AmountDTO()
                                {
                                    Currency = new CurrencyDTO() { Id = ledgerEntry.LedgerEntryItems[0].Amount.Currency.Id },
                                    Value = System.Math.Abs(accountingClosing.TotalAmount)
                                };
                                counterpartEntry.ExchangeRate = new ExchangeRateDTO() { SellAmount = ledgerEntry.LedgerEntryItems[0].ExchangeRate.SellAmount };
                                counterpartEntry.LocalAmount = new AmountDTO() { Value = System.Math.Abs(accountingClosing.LocalAmountValue) };
                                counterpartEntry.Analysis = new List<AnalysisDTO>();
                                counterpartEntry.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                                counterpartEntry.CostCenters = new List<CostCenterDTO>();
                                counterpartEntry.Currency = new CurrencyDTO() { Id = ledgerEntry.LedgerEntryItems[0].Currency.Id };
                                counterpartEntry.Description = Resources.Resources.RiskReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                                + ", " + Resources.Resources.Endorsement + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                                counterpartEntry.EntryType = new EntryTypeDTO();
                                counterpartEntry.Id = 0;
                                counterpartEntry.Individual = new IndividualDTO() { IndividualId = ledgerEntry.LedgerEntryItems[0].Individual.IndividualId };
                                counterpartEntry.PostDated = new List<PostDatedDTO>();
                                counterpartEntry.Receipt = new ReceiptDTO();

                                ledgerEntry.LedgerEntryItems.Add(counterpartEntry);
                            }
                        }
                    }
                }
            }

            #endregion LedgerItems

            return ledgerEntry;
        }

        #endregion RiskReserveDailyEntry

        #region RiskReserve
        public List<string> AccountClosureRiskReserve(DateTime accountingDate, int module, int userId)
        {
            List<string> entryNumbers = new List<string>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            var entryNumber = 0;

            closures = GetClaimReserveClosureReport();

            if (closures.Count > 0)
            {
                // Se agrupa por sucursal, ramo y moneda
                List<AccountingClosingReportDTO> accountingClosingReportDTOs;
                accountingClosingReportDTOs = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd, p.CurrencyCd }).Select(g => g.First()).ToList();

                foreach (var filteredListItem in accountingClosingReportDTOs)
                {
                    filteredListItem.ModuleId = module;
                    // Se filtra por ramo, sucursal y moneda
                    List<AccountingClosingReportDTO> accountingClosingReports;

                    accountingClosingReports = (from AccountingClosingReportDTO item in closures where item.PrefixCd == filteredListItem.PrefixCd && item.BrachCd == filteredListItem.BrachCd && item.CurrencyCd == filteredListItem.CurrencyCd select item).ToList();

                    // Se obtiene el asiento
                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateRiskReserveEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                    entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                    entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                    entryHeader.Branch = ledgerEntry.Branch;
                    entryHeader.Description = ledgerEntry.Description;
                    entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                    entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                    entryHeader.Id = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        entryHeader.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                    entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                    entryHeader.SalePoint = ledgerEntry.SalePoint;
                    entryHeader.Status = ledgerEntry.Status;
                    entryHeader.UserId = ledgerEntry.UserId;
                }

                // Se valida débitos y créditos
                decimal debits = 0;
                decimal credits = 0;

                foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
                {
                    if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                    {
                        debits = debits + accountingEntry.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + accountingEntry.LocalAmount.Value;
                    }
                }

                if (debits == credits)
                {
                    // Se graba el asiento
                    if (entryHeader.LedgerEntryItems.Count > 10)
                    {
                        // Se borra los datos de la tabla temporal de trabajo
                        DelegateService.generalLedgerService.ClearTempAccountEntry();

                        DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                        entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                    }
                    else
                    {
                        entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                    }

                    if (entryNumber > 0)
                    {
                        entryNumbers.Add(" " + entryNumber);
                        ClaimReserveClosureEnding(accountingDate.Year, accountingDate.Month);
                    }
                    else
                    {
                        entryNumbers.Add(Resources.Resources.EntryRecordingError);
                    }
                }
            }
            return entryNumbers;
        }
        #endregion


        #region Closure

        /// <summary>
        /// Alejandro Villagran
        /// GetGeneratedRecordsCount
        /// Obtiene el número de registros de pre-cierre de contabilidad para cierres mensuales
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public int GetGeneratedRecordsCount(int moduleId, int processId)
        {
            int records = 0;

            try
            {
                var parameters = new NameValue[2];
                parameters[0] = new NameValue("MODULE_ID", moduleId);
                parameters[1] = new NameValue("PROCESS_ID", processId);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_PRE_LOADED_RECORDS_COUNT", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        records = Convert.ToInt32(arrayItem[0]);
                    }
                }
            }
            catch (BusinessException)
            {
                records = 0;
            }

            return records;
        }

        /// <summary>
        /// Obtiene los datos de Cierre de Reaseguros para el reporte
        /// </summary>
        /// <param name="moduleId">Identificador de módulo</param>
        /// <param name="processId">Identificador de proceso</param>
        /// <returns></returns>
        public List<AccountingClosingReportDTO> GetReinsuranceClosureReportByModuleIdAndProcessId(int moduleId, int processId)
        {
            var parameters = new NameValue[2];
            parameters[0] = new NameValue("MODULE_ID", moduleId);
            parameters[1] = new NameValue("PROCESS_ID", processId);

            var reports = new List<AccountingClosingReportDTO>();

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_PRE_LOADED_REPORT", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        var accountingClosingReportDto = new AccountingClosingReportDTO()
                        {
                            EntryNumber = Convert.ToInt32(arrayItem[0]),
                            AccountNatureCd = Convert.ToString(arrayItem[1]),
                            CurrencyDescription = Convert.ToString(arrayItem[2]),
                            AccountingAccountCd = Convert.ToString(arrayItem[3]),
                            AccountingAccountDescription = Convert.ToString(arrayItem[4]),
                            TotalAmount = Convert.ToDecimal(arrayItem[5])
                        };
                        reports.Add(accountingClosingReportDto);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return reports;
        }

        /// <summary>
        /// Alejandro Villagran
        /// GetGeneratedClosureReportRecords
        /// Obtiene los registros paginados de pre-cierre de contabilidad para cierres mensuales
        /// </summary>
        /// <returns>List<AccountingClosingReportDto/></returns>
        public List<AccountingClosingReportDTO> GetGeneratedClosureReportRecords(int moduleId, int processId, int pageSize, int pageNumber, int records)
        {
            List<AccountingClosingReportDTO> reports = new List<AccountingClosingReportDTO>();

            var parameters = new NameValue[6];
            parameters[0] = new NameValue("MODULE_ID", moduleId);
            parameters[1] = new NameValue("PROCESS_ID", processId);
            parameters[2] = new NameValue("PageSize", pageSize);
            parameters[3] = new NameValue("PageNumber", pageNumber);
            parameters[4] = new NameValue("PageCount", 0);
            parameters[5] = new NameValue("RecordCount", records);

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_PRE_LOADED_RECORDS", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        AccountingClosingReportDTO accountingClosingReportDto = new AccountingClosingReportDTO();
                        accountingClosingReportDto.BrachCd = Convert.ToInt32(arrayItem[2]);
                        accountingClosingReportDto.SalePointId = Convert.ToInt32(arrayItem[3]);
                        accountingClosingReportDto.AccountingCompanyId = Convert.ToInt32(arrayItem[4]);
                        accountingClosingReportDto.TotalAmount = Convert.ToDecimal(arrayItem[5]);
                        accountingClosingReportDto.CurrencyCd = Convert.ToInt32(arrayItem[6]);
                        accountingClosingReportDto.ExchangeRate = Convert.ToDecimal(arrayItem[7]);
                        accountingClosingReportDto.LocalAmountValue = Convert.ToDecimal(arrayItem[8]);
                        accountingClosingReportDto.PayerId = Convert.ToInt32(arrayItem[9]);
                        accountingClosingReportDto.AccountingAccountCd = Convert.ToInt32(arrayItem[10]).ToString();
                        accountingClosingReportDto.AccountNatureCd = Convert.ToString(arrayItem[11]);
                        accountingClosingReportDto.Description = Convert.ToString(arrayItem[12]);
                        accountingClosingReportDto.Date = Convert.ToDateTime(arrayItem[13]);
                        accountingClosingReportDto.AccountingMovementTypeId = Convert.ToInt32(arrayItem[14]);
                        accountingClosingReportDto.ModuleId = Convert.ToInt32(arrayItem[15]);
                        accountingClosingReportDto.BankReconciliationId = Convert.ToInt32(arrayItem[16]);
                        accountingClosingReportDto.ReceiptNumber = Convert.ToInt32(arrayItem[17]);
                        accountingClosingReportDto.ReceiptDate = arrayItem[18] == DBNull.Value ? Convert.ToDateTime(null) : Convert.ToDateTime(arrayItem[18]);
                        accountingClosingReportDto.PaymentMovementTypeId = Convert.ToInt32(arrayItem[19]);
                        accountingClosingReportDto.EntryDestinationId = Convert.ToInt32(arrayItem[20]);
                        accountingClosingReportDto.IsDailyEntry = Convert.ToInt32(arrayItem[21]);
                        accountingClosingReportDto.EntryNumber = Convert.ToInt32(arrayItem[22]);
                        accountingClosingReportDto.UserId = Convert.ToInt32(arrayItem[23]);
                        accountingClosingReportDto.ConciliationId = Convert.ToInt32(arrayItem[24]);
                        accountingClosingReportDto.ConciliationDate = arrayItem[25] == DBNull.Value ? Convert.ToDateTime(null) : Convert.ToDateTime(arrayItem[25]);
                        accountingClosingReportDto.DueDate = arrayItem[26] == DBNull.Value ? Convert.ToDateTime(null) : Convert.ToDateTime(arrayItem[26]);

                        reports.Add(accountingClosingReportDto);
                    }
                }
            }
            catch (BusinessException)
            {
                reports = new List<AccountingClosingReportDTO>();
            }

            return reports;
        }


        public AccountingClosingDTO GetAccountingClosing(int module)
        {
            var parameters = new NameValue[1];
            parameters[0] = new NameValue("module", module);

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_LOG_ACCOUNTING_CLOSING", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        var resulttoReturn = (from DataRow item in result.Rows
                                              select new AccountingClosingDTO()
                                              {
                                                  Id = Convert.ToInt32(arrayItem[0].ToString()),
                                                  ModuleId = Convert.ToInt32(arrayItem[1].ToString()),
                                                  StartDate = arrayItem[3] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(arrayItem[3].ToString()),
                                                  EnDate = arrayItem[5] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(arrayItem[5].ToString()),
                                                  isProgress = (Convert.ToInt32(arrayItem[6].ToString()) == 1)
                                              }).FirstOrDefault();

                        return resulttoReturn;
                    }
                }
                return new AccountingClosingDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// ExecuteClosing
        /// Realiza cierre de Procesos
        /// </summary>
        /// <param name="closingTypeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>JsonResult</returns>
        public int ExecuteClosing(int closingTypeId, int year, int month, int userId)
        {

            int result = 0;


            switch (closingTypeId)
            {
                case 1: //Cierre de Ingresos y Egresos
                    result = DelegateService.generalLedgerService.IncomeOutcomeClosing(year, userId);
                    break;
                case 2: //Cierre de Utilidad Mensual
                    result = DelegateService.generalLedgerService.MonthlyIncomeClosing(year, userId, month);
                    break;
                case 3: //Asiento de Apertura de Activos y Pasivos
                    result = DelegateService.generalLedgerService.AssetAndLiabilityOpening(year, userId);
                    break;
                case 4: //Reversar Asiento Anual de Apertura
                    result = DelegateService.generalLedgerService.RevertAnualEntryOpening(year, userId);
                    break;
                case 5: //Reversar Cierre Anual de Ingresos y Egresos
                    result = DelegateService.generalLedgerService.RevertIncomeOutcomeClosing(year, userId);
                    break;
                default:
                    result = DelegateService.generalLedgerService.IncomeOutcomeClosing(year, userId);
                    break;
            }
            return result;
        }

        /// <summary>
        /// CheckClosedModules
        /// Metodo para saber si todos los módulos han sido cerrados para el mes de diciembre.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>UifJsonResult</returns>
        public bool CheckClosedModules(int year)
        {
            bool result;
            int count = 0;
           
                List<ModuleDate> moduleDates = DelegateService.tempCommonService.GetModuleDates();

                foreach (ModuleDate moduleDate in moduleDates)
                {
                    if (moduleDate.LastClosingYyyy != year && moduleDate.LastClosingMm != 12)
                    {
                        count = count + 1;
                    }
                }

                if (count > 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }

                return  result;
            
        }
        #endregion Closure

        #region Process

        /// <summary>
        /// GetLogProcessId
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public int GetLogProcessId(int module)
        {
            var parameters = new NameValue[1];
            parameters[0] = new NameValue("module", module);
            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_ACCOUNTING_CLOSING_LOG_RECORD_BY_MODULE", parameters);
            }

            var processId = Convert.ToInt32((from DataRow item in result.Rows select item[0].ToString()).FirstOrDefault());

            return processId;
        }

        /// <summary>
        /// UpdateLogProcess
        /// Actualiza el proceso masivo que se está ejecutando
        /// </summary>
        /// <param name="processId"></param>
        public void UpdateLogProcess(int processId)
        {
            var parameters = new NameValue[1];
            parameters[0] = new NameValue("Id", processId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteSPDataTable("ACL.UPDATE_LOG_ACCOUNTING_CLOSING", parameters);
            }
        }

        #endregion Process

        #region ClosingDate

        /// <summary>
        /// GetClosingDate: Obtiene Fecha de Cierre 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>DateTime</returns>
        public DateTime GetClosingDate(int moduleId)
        {
            ModuleDate moduleDate = new ModuleDate() { Id = moduleId };
            moduleDate = DelegateService.tempCommonService.GetModuleDate(moduleDate);

            return GetFirstAndLastDayOfMonth(DateTime.Now, moduleDate.LastClosingMm, moduleDate.LastClosingYyyy);
        }

        #endregion ClosingDate

        #region ClosureTempEntry

        /// <summary>
        /// Graba registro en la tabla temporal para la generación de asientos de cierre.
        /// </summary>
        /// <param name="closureTempEntryGenerationDTO"></param>
        public void SaveClosureTempEntryGeneration(ClosureTempEntryGenerationDTO closureTempEntryGenerationDTO)
        {
            try
            {
                var parameters = new NameValue[26];
                parameters[0] = new NameValue("PROCESS_ID", closureTempEntryGenerationDTO.ProcessId);
                parameters[1] = new NameValue("BRANCH_CD", closureTempEntryGenerationDTO.BranchId);
                parameters[2] = new NameValue("SALE_POINT_CD", closureTempEntryGenerationDTO.SalePointId);
                parameters[3] = new NameValue("ACCOUNTING_COMPANY_CD", closureTempEntryGenerationDTO.AccountingCompanyId);
                parameters[4] = new NameValue("AMOUNT", closureTempEntryGenerationDTO.Amount);
                parameters[5] = new NameValue("CURRENCY_CD", closureTempEntryGenerationDTO.CurrencyId);
                parameters[6] = new NameValue("EXCHANGE_RATE", closureTempEntryGenerationDTO.ExchangeRate);
                parameters[7] = new NameValue("LOCAL_AMOUNT", closureTempEntryGenerationDTO.LocalAmount);
                parameters[8] = new NameValue("INDIVIDUAL_ID", closureTempEntryGenerationDTO.IndividualId);
                parameters[9] = new NameValue("ACCOUNTING_ACCOUNT_ID", closureTempEntryGenerationDTO.AccountingAccountId);
                parameters[10] = new NameValue("ACCOUNTING_NATURE", closureTempEntryGenerationDTO.AccountingNature);
                parameters[11] = new NameValue("DESCRIPTION", closureTempEntryGenerationDTO.Description);
                parameters[12] = new NameValue("DATE", closureTempEntryGenerationDTO.Date);
                parameters[13] = new NameValue("ACCOUNTING_MOVEMENT_TYPE_ID", closureTempEntryGenerationDTO.AccountingMovementTypeId);
                parameters[14] = new NameValue("ACCOUNTING_MODULE_ID", closureTempEntryGenerationDTO.AccountingModuleId);
                parameters[15] = new NameValue("BANK_RECONCILIATION_ID", closureTempEntryGenerationDTO.BankReconciliationId);
                parameters[16] = new NameValue("RECEIPT_NUMBER", closureTempEntryGenerationDTO.ReceiptNumber);
                parameters[17] = new NameValue("RECEIPT_DATE", closureTempEntryGenerationDTO.ReceiptDate == Convert.ToDateTime("01/01/1900") ? null : String.Format("{0:dd/MM/yyyy HH:mm:ss}", closureTempEntryGenerationDTO.ReceiptDate));
                parameters[18] = new NameValue("PAYMENT_MOVEMENT_TYPE_CD", closureTempEntryGenerationDTO.PaymentMovementTypeId);
                parameters[19] = new NameValue("ENTRY_DESTINATION_ID", closureTempEntryGenerationDTO.EntryDestinationId);
                parameters[20] = new NameValue("IS_DAILY_ENTRY", closureTempEntryGenerationDTO.IsDailyEntry ? 1 : 0);
                parameters[21] = new NameValue("ENTRY_NUMBER", closureTempEntryGenerationDTO.EntryNumber);
                parameters[22] = new NameValue("USER_CD", closureTempEntryGenerationDTO.UserId);
                parameters[23] = new NameValue("CONCILATION_CD", closureTempEntryGenerationDTO.ReconciliationId);
                parameters[24] = new NameValue("CONCILATION_DATE", closureTempEntryGenerationDTO.ReconciliationDate == Convert.ToDateTime("01/01/1900") ? null : String.Format("{0:dd/MM/yyyy HH:mm:ss}", closureTempEntryGenerationDTO.ReconciliationDate));
                parameters[25] = new NameValue("DUE_DATE", closureTempEntryGenerationDTO.DueDate == Convert.ToDateTime("01/01/1900") ? null : String.Format("{0:dd/MM/yyyy HH:mm:ss}", closureTempEntryGenerationDTO.DueDate));

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.SAVE_CLOSURE_TEMP_ENTRY_GENERATION", parameters);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion ClosureTempEntry

        #region ExpensesClosure

        /// <summary>
        /// Genera el reporte de cierre de gastos
        /// </summary>
        /// <returns></returns>
        public List<AccountingClosingReportDTO> ExpensesClousureReport()
        {
            var closures = new List<AccountingClosingReportDTO>();

            try
            {
                var parameters = new NameValue[1];
                parameters[0] = new NameValue("@ENTRY_NUMBER", DBNull.Value, DbType.Int16);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_INCOME_EXPENSES_ENTRY", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        if (!(Convert.ToDecimal(arrayItem[8]) == 0 && Convert.ToDecimal(arrayItem[9]) == 0))
                        {
                            closures.Add(new AccountingClosingReportDTO()
                            {
                                BrachCd = Convert.ToInt32(arrayItem[0]),
                                BranchDescription = DBNull.ReferenceEquals(arrayItem[1], DBNull.Value) ? String.Empty : arrayItem[1].ToString(),
                                CurrencyCd = Convert.ToInt32(arrayItem[2]),
                                CurrencyDescription = arrayItem[3].ToString(),
                                AccountNatureCd = Convert.ToInt32(arrayItem[7]) == 2 ? "D" : "C",
                                AccountingAccountCd = arrayItem[5].ToString(),
                                AccountingAccountDescription = arrayItem[6].ToString(),
                                LocalAmountValue = Convert.ToDecimal(arrayItem[8]) != 0 ?
                                Convert.ToDecimal(arrayItem[8]) : Convert.ToDecimal(arrayItem[9]),
                                ComponentId = Convert.ToInt32(arrayItem[4]),
                                EntryNumber = Convert.ToInt32(arrayItem[11])
                            });
                        }
                    }
                }
                return closures;
            }
            catch (BusinessException)
            {
                return closures;
            }
        }

        /// <summary>
        /// Contabiliza el cierre de gastos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public int ExpensesClousureEnding(int year, int month, int day)
        {
            var parameters = new NameValue[3];

            parameters[0] = new NameValue("YEAR", year);
            parameters[1] = new NameValue("MONTH", month);
            parameters[2] = new NameValue("DAY", day);

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.INCOME_EXPENSES_ENTRY", parameters);
                }

                // Devuelve el número de asiento de Contabilidad Central, en caso de error devuelve cero.
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());

                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion ExpensesClosure

        #region IBNR Reserves

        /// <summary>
        /// Genera el cierre de reservas IBNR
        /// </summary>
        /// <param name="year"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public int IbnrClosureGeneration(int year, int module)
        {
            try
            {
                var parameters = new NameValue[1];
                int processId = SaveLogProcess(module);

                parameters[0] = new NameValue("YEAR", year);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.IBNR_MONTHLY_CLOSING", parameters);
                }

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_IBNR_MONTHLY_CLOSING", parameters);
                }

                UpdateLogProcess(processId);

                return (result != null && result.Rows.Count > 0) ? 1 : 0;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Genera el reporte de cierre de reservas IBNR
        /// </summary>
        /// <returns></returns>
        public List<AccountingClosingReportDTO> IbnrClosureReport()
        {
            var closures = new List<AccountingClosingReportDTO>();

            try
            {
                var parameters = new NameValue[1];
                parameters[0] = new NameValue("@YEAR", DBNull.Value, DbType.Int16);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_IBNR_MONTHLY_CLOSING", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        if (!(Convert.ToDecimal(arrayItem[6]) == 0 && Convert.ToDecimal(arrayItem[7]) == 0))
                        {
                            closures.Add(new AccountingClosingReportDTO()
                            {
                                BrachCd = Convert.ToInt32(arrayItem[0]),
                                BranchDescription = DBNull.ReferenceEquals(arrayItem[1], DBNull.Value) ? String.Empty : arrayItem[1].ToString(),
                                CurrencyCd = Convert.ToInt32(arrayItem[2]),
                                CurrencyDescription = arrayItem[3].ToString(),
                                AccountNatureCd = Convert.ToDecimal(arrayItem[6]) != 0 ? Convert.ToString(Convert.ToInt32(AccountingNatures.Debit)) : Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)),
                                AccountingAccountCd = arrayItem[4].ToString(),
                                AccountingAccountDescription = arrayItem[5].ToString(),
                                LocalAmountValue = Convert.ToDecimal(arrayItem[6]) != 0 ?
                                Convert.ToDecimal(arrayItem[6]) : Convert.ToDecimal(arrayItem[7]),
                                ComponentId = Convert.ToInt32(arrayItem[8])
                            });
                        }
                    }
                }

                return closures;
            }
            catch (BusinessException)
            {
                return closures;
            }
        }

        /// <summary>
        /// Contabiliza el cierre de reservas IBNR
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int IbnrClosureEnding(int year)
        {
            var parameters = new NameValue[1];
            parameters[0] = new NameValue("YEAR", year);

            try
            {
                // Ejecuta el sp de contabilización   
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.IBNR_MONTHLY_ENTRY", parameters);
                }

                // Devuelve el número de asiento
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());

                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<string> AccountClosureIBNRReserves(DateTime accountingDate, int module, int userId)
        {
            List<string> entryNumbers = new List<string>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            var entryNumber = 0;


            // Se obtiene los parámetros para generar el asiento
            closures = IbnrClosureReport();

            if (closures.Count > 0)
            {
                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> accountingClosingReportDTOs;

                accountingClosingReportDTOs = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                foreach (var accountingClosing in accountingClosingReportDTOs)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReports = (from AccountingClosingReportDTO item in closures where item.PrefixCd == accountingClosing.PrefixCd && item.BrachCd == accountingClosing.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateIBNRReserveEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                    entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                    entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                    entryHeader.Branch = ledgerEntry.Branch;
                    entryHeader.Description = ledgerEntry.Description;
                    entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                    entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                    entryHeader.Id = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        entryHeader.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                    entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                    entryHeader.SalePoint = ledgerEntry.SalePoint;
                    entryHeader.Status = ledgerEntry.Status;
                    entryHeader.UserId = ledgerEntry.UserId;
                }

                // Se valida débitos y créditos
                decimal debits = 0;
                decimal credits = 0;

                foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
                {
                    if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                    {
                        debits = debits + accountingEntry.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + accountingEntry.LocalAmount.Value;
                    }
                }

                if (debits == credits)
                {
                    if (entryHeader.LedgerEntryItems.Count > 10)
                    {
                        // Se borra los datos de la tabla temporal de trabajo
                        DelegateService.generalLedgerService.ClearTempAccountEntry();

                        DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                        entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                    }
                    else
                    {
                        entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                    }

                    if (entryNumber > 0)
                    {
                        entryNumbers.Add(" " + entryNumber);
                        IbnrClosureEnding(accountingDate.Year);
                    }
                    else
                    {
                        entryNumbers.Add(Resources.Resources.EntryRecordingError);
                    }
                }
            }


            return entryNumbers;
        }

        #endregion IBNR Reserves

        #region IBNRReserveDailyEntry

        /// <summary>
        /// GenerateIBNRReserveEntry
        /// Genera los asientos para el cierre de reservas de IBNR.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateIBNRReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosingReport in accountingClosings)
            {
                //Cabecera
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                {
                    AccountingCompanyId = accountingCompanyId
                };
                ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                {
                    AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES))
                };
                ledgerEntry.Branch = new BranchDTO() { Id = accountingClosingReport.BrachCd };
                ledgerEntry.Description = Resources.Resources.IBNReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosingReport.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosingReport.EndorsementDocumentNumber);
                ledgerEntry.EntryDestination = new EntryDestinationDTO();
                ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
                ledgerEntry.EntryNumber = 0;
                ledgerEntry.Id = 0;

                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                ledgerEntryItem.AccountingAccount = DTOAssembler.ToDTO(DelegateService.generalLedgerService.GetAccountingAccount(Convert.ToInt32(accountingClosingReport.ComponentId)));
                ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(accountingClosingReport.AccountNatureCd);
                ledgerEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = accountingClosingReport.CurrencyCd },
                    Value = accountingClosingReport.TotalAmount
                };
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosingReport.ExchangeRate };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = accountingClosingReport.LocalAmountValue };
                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosingReport.CurrencyCd };
                ledgerEntryItem.Description = Resources.Resources.IBNReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosingReport.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosingReport.EndorsementDocumentNumber);
                ledgerEntryItem.EntryType = new EntryTypeDTO();
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosingReport.PayerId };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();
                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                ledgerEntry.ModuleDateId = moduleId;
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                ledgerEntry.Status = 1; //activo
                ledgerEntry.UserId = userId;
            }

            return ledgerEntry;
        }

        #endregion IBNRReserveDailyEntry

        #region Prevision Reserves
        public List<string> AccountClosurePrevisionReserves(DateTime accountingDate, int module, int userId)
        {
            List<string> entryNumbers = new List<string>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            var entryNumber = 0;

            // Se obtiene los parámetros para generar el asiento
            closures = GetRiskPreventionReserveClosureReport();

            if (closures.Count > 0)
            {
                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> accountingClosingReportDTOs;

                accountingClosingReportDTOs = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                foreach (var accountingClosing in accountingClosingReportDTOs)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReports = (from AccountingClosingReportDTO accountingItem in closures where accountingItem.PrefixCd == accountingClosing.PrefixCd && accountingItem.BrachCd == accountingClosing.BrachCd select accountingItem).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GeneratePrevisionReserveEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                    entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                    entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                    entryHeader.Branch = ledgerEntry.Branch;
                    entryHeader.Description = ledgerEntry.Description;
                    entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                    entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                    entryHeader.Id = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        entryHeader.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                    entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                    entryHeader.SalePoint = ledgerEntry.SalePoint;
                    entryHeader.Status = ledgerEntry.Status;
                    entryHeader.UserId = ledgerEntry.UserId;
                }

                // Se valida débitos y créditos
                decimal debits = 0;
                decimal credits = 0;

                foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
                {
                    if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                    {
                        debits = debits + accountingEntry.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + accountingEntry.LocalAmount.Value;
                    }
                }

                if (debits == credits)
                {
                    if (entryHeader.LedgerEntryItems.Count > 10)
                    {
                        // Se borra los datos de la tabla temporal de trabajo
                        DelegateService.generalLedgerService.ClearTempAccountEntry();

                        DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                        entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                    }
                    else
                    {
                        entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                    }

                    if (entryNumber > 0)
                    {
                        entryNumbers.Add(" " + entryNumber);
                        RiskPreventionReserveClosureEnding(accountingDate.Year, accountingDate.Month);
                    }
                    else
                    {
                        entryNumbers.Add(Resources.Resources.EntryRecordingError);
                    }
                }
            }


            return entryNumbers;
        }
        #endregion

        #region PrevisionReserveDailyEntry

        /// <summary>
        /// GeneratePrevisionReserveEntry
        /// Genera los asientos para el cierre de reservas de previsión.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GeneratePrevisionReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in accountingClosings)
            {
                //Cabecera
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                {
                    AccountingCompanyId = accountingCompanyId
                };
                ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                {
                    AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES))
                };
                ledgerEntry.Branch = new BranchDTO() { Id = accountingClosing.BrachCd };
                ledgerEntry.Description = Resources.Resources.PrevisionReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                          + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntry.EntryDestination = new EntryDestinationDTO();
                ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
                ledgerEntry.EntryNumber = 0;
                ledgerEntry.Id = 0;

                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                
                ledgerEntryItem.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccount(Convert.ToInt32(accountingClosing.AccountingAccountCd)).ToDTO();
                ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(accountingClosing.AccountNatureCd);
                ledgerEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                    Value = accountingClosing.TotalAmount
                };
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = accountingClosing.LocalAmountValue };
                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                ledgerEntryItem.Description = Resources.Resources.PrevisionReserveAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntryItem.EntryType = new EntryTypeDTO();
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();
                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                ledgerEntry.ModuleDateId = moduleId;
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                ledgerEntry.Status = 1; //activo
                ledgerEntry.UserId = userId;
            }

            return ledgerEntry;
        }

        #endregion PrevisionReserveDailyEntry

        #region RiskPreventionReserveClosure

        /// <summary>
        /// Genera el cierre de reserva de prevención de riesgos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public int RiskPreventionReserveClosureGeneration(int year, int month, int module)
        {
            try
            {
                var parameters = new NameValue[2];
                int processId = SaveLogProcess(module);

                parameters[0] = new NameValue("YEAR", year);
                parameters[1] = new NameValue("MONTH", month);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.PREVISION_MONTHLY_CLOSING", parameters);
                }

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_PREVISION_MONTHLY_CLOSING", parameters);
                }

                UpdateLogProcess(processId);

                return (result != null && result.Rows.Count > 0) ? 1 : 0;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Genera el reporte de cierre de reserva de prevención de riesgos
        /// </summary>
        /// <returns></returns>
        public List<AccountingClosingReportDTO> GetRiskPreventionReserveClosureReport()
        {
            var closures = new List<AccountingClosingReportDTO>();

            try
            {
                var parameters = new NameValue[2];
                parameters[0] = new NameValue("@YEAR", DBNull.Value, DbType.Int16);
                parameters[1] = new NameValue("@MONTH", DBNull.Value, DbType.Int16);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_PREVISION_MONTHLY_CLOSING", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        if (!(Convert.ToDecimal(arrayItem[6]) == 0 && Convert.ToDecimal(arrayItem[7]) == 0))
                        {
                            closures.Add(new AccountingClosingReportDTO()
                            {
                                BrachCd = Convert.ToInt32(arrayItem[0]),
                                BranchDescription = DBNull.ReferenceEquals(arrayItem[1], DBNull.Value) ? String.Empty : arrayItem[1].ToString(),
                                CurrencyCd = Convert.ToInt32(arrayItem[2]),
                                CurrencyDescription = arrayItem[3].ToString(),
                                AccountNatureCd = Convert.ToDecimal(arrayItem[6]) != 0 ? Convert.ToString(Convert.ToInt32(AccountingNatures.Debit)) : Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)),
                                AccountingAccountCd = arrayItem[8].ToString(),
                                AccountingAccountDescription = arrayItem[5].ToString(),
                                LocalAmountValue = Convert.ToDecimal(arrayItem[6]) != 0 ?
                                Convert.ToDecimal(arrayItem[6]) : Convert.ToDecimal(arrayItem[7]),
                                ComponentId = Convert.ToInt32(arrayItem[8])
                            });
                        }
                    }
                }
                return closures;
            }
            catch (BusinessException)
            {
                return closures;
            }
        }

        /// <summary>
        /// Contabiliza el cierre de reserva de prevención de riesgos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public int RiskPreventionReserveClosureEnding(int year, int month)
        {
            var parameters = new NameValue[2];
            parameters[0] = new NameValue("YEAR", year);
            parameters[1] = new NameValue("MONTH", month);

            try
            {
                // Ejecuta el sp de contabilización   
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.PREVISION_MONTHLY_ENTRY", parameters);
                }

                // Devuelve el número de asiento
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());

                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion RiskPreventionReserveClosure

        #region ExpiredPremiums

        /// <summary>
        /// Genera el cierre de primas vencidas
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public int ExpiredPremiumsGeneration(int year, int month, int module)
        {
            try
            {
                var parameters = new NameValue[2];
                int processId = SaveLogProcess(module);

                parameters[0] = new NameValue("YEAR", year);
                parameters[1] = new NameValue("MONTH", month);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.EXPIRED_PREMIUMS_MONTHLY_CLOSING", parameters);
                }

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_EXPIRED_PREMIUMS_MONTHLY_CLOSING", parameters);
                }

                UpdateLogProcess(processId);

                return (result != null && result.Rows.Count > 0) ? 1 : 0;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Genera el reporte de cierre de primas vencidas
        /// </summary>
        /// <returns></returns>
        public List<AccountingClosingReportDTO> ExpiredPremiumsReport()
        {
            var closures = new List<AccountingClosingReportDTO>();

            try
            {
                var parameters = new NameValue[2];
                parameters[0] = new NameValue("@YEAR", DBNull.Value, DbType.Int16);
                parameters[1] = new NameValue("@MONTH", DBNull.Value, DbType.Int16);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_EXPIRED_PREMIUMS_MONTHLY_CLOSING", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        if (!(Convert.ToDecimal(arrayItem[6]) == 0 && Convert.ToDecimal(arrayItem[7]) == 0))
                        {
                            closures.Add(new AccountingClosingReportDTO()
                            {
                                BrachCd = Convert.ToInt32(arrayItem[0]),
                                BranchDescription = DBNull.ReferenceEquals(arrayItem[1], DBNull.Value) ? String.Empty : arrayItem[1].ToString(),
                                CurrencyCd = Convert.ToInt32(arrayItem[2]),
                                CurrencyDescription = arrayItem[3].ToString(),
                                AccountNatureCd = Convert.ToDecimal(arrayItem[6]) != 0 ? Convert.ToString(Convert.ToInt32(AccountingNatures.Debit)) : Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)),
                                AccountingAccountCd = arrayItem[8].ToString(),
                                AccountingAccountDescription = arrayItem[5].ToString(),
                                LocalAmountValue = Convert.ToDecimal(arrayItem[6]) != 0 ?
                                Convert.ToDecimal(arrayItem[6]) : Convert.ToDecimal(arrayItem[7]),
                                ComponentId = Convert.ToInt32(arrayItem[8])
                            });
                        }
                    }
                }
                return closures;
            }
            catch (BusinessException)
            {
                return closures;
            }
        }

        /// <summary>
        /// Genera el reporte de cierre reservas provisión de primas vencidas
        /// </summary>
        /// <returns></returns>
        public List<AccountingClosingReportDTO> ProvisionExpiredPremiumsReport()
        {
            var closures = new List<AccountingClosingReportDTO>();

            try
            {
                var parameters = new NameValue[2];
                parameters[0] = new NameValue("@YEAR", null);
                parameters[1] = new NameValue("@MONTH", null);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_PREVISION_CLOSING", parameters);
                }
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        if (!(Convert.ToDecimal(arrayItem[6]) == 0 && Convert.ToDecimal(arrayItem[7]) == 0))
                        {
                            closures.Add(new AccountingClosingReportDTO()
                            {
                                BrachCd = Convert.ToInt32(arrayItem[0]),
                                BranchDescription = DBNull.ReferenceEquals(arrayItem[1], DBNull.Value) ? String.Empty : arrayItem[1].ToString(),
                                CurrencyCd = Convert.ToInt32(arrayItem[2]),
                                CurrencyDescription = arrayItem[3].ToString(),
                                AccountNatureCd = Convert.ToDecimal(arrayItem[6]) != 0 ? Convert.ToString(Convert.ToInt32(AccountingNatures.Debit)) : Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)),
                                AccountingAccountCd = arrayItem[8].ToString(),
                                AccountingAccountDescription = arrayItem[5].ToString(),
                                LocalAmountValue = Convert.ToDecimal(arrayItem[6]) != 0 ?
                                Convert.ToDecimal(arrayItem[6]) : Convert.ToDecimal(arrayItem[7]),
                                ComponentId = Convert.ToInt32(arrayItem[8])
                            });
                        }
                    }
                }
                return closures;
            }
            catch (BusinessException)
            {
                return closures;
            }
        }

        /// <summary>
        /// Contabiliza el cierre de reservas provisión de primas vencidas
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public int ExpiredPremiumsEnding(int year, int month, int day)
        {
            var parameters = new NameValue[3];
            parameters[0] = new NameValue("YEAR", year);
            parameters[1] = new NameValue("MONTH", month);
            parameters[2] = new NameValue("DAY", day);

            try
            {
                // Se ejecuta el sp de contabilización   
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.EXPIRED_PREMIUMS_MONTHLY_ENTRY", parameters);
                }
                // Devuelve el número de asiento y número de asiento de provisión
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());
                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<string> AccountClosureExpiredPremiums(DateTime accountingDate, int module, int userId, int day)
        {
            List<string> entryNumbers = new List<string>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            var entryNumber = 0;

            // Se obtiene los parámetros para generar el asiento
            closures = ExpiredPremiumsReport();

            if (closures.Count > 0)
            {
                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> accountingClosingReportDTOs;

                accountingClosingReportDTOs = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                foreach (var accountingClosingReport in accountingClosingReportDTOs)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReports = (from AccountingClosingReportDTO item in closures where item.PrefixCd == accountingClosingReport.PrefixCd && item.BrachCd == accountingClosingReport.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateExpiredPremiumsEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                    entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                    entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                    entryHeader.Branch = ledgerEntry.Branch;
                    entryHeader.Description = ledgerEntry.Description;
                    entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                    entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                    entryHeader.Id = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        entryHeader.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                    entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                    entryHeader.SalePoint = ledgerEntry.SalePoint;
                    entryHeader.Status = ledgerEntry.Status;
                    entryHeader.UserId = ledgerEntry.UserId;
                }

                // Se valida débitos y créditos
                decimal debits = 0;
                decimal credits = 0;

                foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
                {
                    if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                    {
                        debits = debits + accountingEntry.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + accountingEntry.LocalAmount.Value;
                    }
                }

                if (debits == credits)
                {
                    if (entryHeader.LedgerEntryItems.Count > 10)
                    {
                        // Se borra los datos de la tabla temporal de trabajo
                        DelegateService.generalLedgerService.ClearTempAccountEntry();

                        DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                        entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                    }
                    else
                    {
                        entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                    }

                    if (entryNumber > 0)
                    {
                        entryNumbers.Add(" " + entryNumber);

                        // Se obtiene los parámetros para generar el asiento de previsiones
                        closures = ProvisionExpiredPremiumsReport();

                        // Se agrupa por sucursal y ramo                                
                        accountingClosingReportDTOs = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                        foreach (var accountingClosing in accountingClosingReportDTOs)
                        {
                            // Se filtra por ramo y sucursal
                            List<AccountingClosingReportDTO> accountingClosingReportFiltered;
                            accountingClosingReportFiltered = (from AccountingClosingReportDTO item in closures where item.PrefixCd == accountingClosing.PrefixCd && item.BrachCd == accountingClosing.BrachCd select item).ToList();

                            LedgerEntryDTO ledgerEntry;
                            ledgerEntry = GeneratePrevisionExpiredPremiumsEntry(accountingClosingReportFiltered, module, userId);

                            ledgerEntries.Add(ledgerEntry);
                        }

                        entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                        foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                        {
                            entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                            entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                            entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                            entryHeader.Branch = ledgerEntry.Branch;
                            entryHeader.Description = ledgerEntry.Description;
                            entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                            entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                            entryHeader.Id = 0;

                            foreach (LedgerEntryItemDTO accountingEntryItem in ledgerEntry.LedgerEntryItems)
                            {
                                entryHeader.LedgerEntryItems.Add(accountingEntryItem);
                            }

                            entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                            entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                            entryHeader.SalePoint = ledgerEntry.SalePoint;
                            entryHeader.Status = ledgerEntry.Status;
                            entryHeader.UserId = ledgerEntry.UserId;
                        }

                        // Se valida débitos y créditos
                        debits = 0;
                        credits = 0;

                        foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
                        {
                            if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                            {
                                debits = debits + accountingEntry.LocalAmount.Value;
                            }
                            else
                            {
                                credits = credits + accountingEntry.LocalAmount.Value;
                            }
                        }

                        if (debits == credits)
                        {
                            if (entryHeader.LedgerEntryItems.Count > 10)
                            {
                                // Se borra los datos de la tabla temporal de trabajo
                                DelegateService.generalLedgerService.ClearTempAccountEntry();

                                DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                                entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                            }
                            else
                            {
                                entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                            }

                            if (entryNumber > 0)
                            {
                                entryNumbers.Add(" " + entryNumber);
                                ExpiredPremiumsEnding(accountingDate.Year, accountingDate.Month, day);
                            }
                            else
                            {
                                entryNumbers.Add(Resources.Resources.EntryRecordingError);
                            }
                        }
                    }
                    else
                    {
                        entryNumbers.Add(Resources.Resources.EntryRecordingError);
                    }
                }
            }

            return entryNumbers;
        }

        #endregion ExpiredPremiums

        #region ExpiredPremiumsDailyEntry

        /// <summary>
        /// GenerateExpiredPremiumsEntry
        /// Genera los asientos para el cierre de primas vencidas.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateExpiredPremiumsEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in accountingClosings)
            {
                //Cabecera
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                {
                    AccountingCompanyId = accountingCompanyId
                };
                ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                {
                    AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES))
                };
                ledgerEntry.Branch = new BranchDTO() { Id = accountingClosing.BrachCd };
                ledgerEntry.Description = Resources.Resources.ExpiredPremiumsAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                          + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntry.EntryDestination = new EntryDestinationDTO();
                ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
                ledgerEntry.EntryNumber = 0;
                ledgerEntry.Id = 0;

                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                ledgerEntryItem.AccountingAccount = DTOAssembler.ToDTO(DelegateService.generalLedgerService.GetAccountingAccount(Convert.ToInt32(accountingClosing.AccountingAccountCd)));
                ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(accountingClosing.AccountNatureCd);
                ledgerEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                    Value = accountingClosing.TotalAmount
                };
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = accountingClosing.LocalAmountValue };
                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                ledgerEntryItem.Description = Resources.Resources.ExpiredPremiumsAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntryItem.EntryType = new EntryTypeDTO();
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();
                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                ledgerEntry.ModuleDateId = moduleId;
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                ledgerEntry.Status = 1; //activo
                ledgerEntry.UserId = userId;
            }

            return ledgerEntry;
        }

        /// <summary>
        /// GeneratePrevisionExpiredPremiumsEntry
        /// Genera los asientos de previsión para el cierre de primas vencidas.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GeneratePrevisionExpiredPremiumsEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in accountingClosings)
            {
                //Cabecera
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                {
                    AccountingCompanyId = accountingCompanyId
                };
                ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                {
                    AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES))
                };
                ledgerEntry.Branch = new BranchDTO() { Id = accountingClosing.BrachCd };
                ledgerEntry.Description = Resources.Resources.ExpiredPremiumsAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                          + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntry.EntryDestination = new EntryDestinationDTO();
                ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
                ledgerEntry.EntryNumber = 0;
                ledgerEntry.Id = 0;

                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                ledgerEntryItem.AccountingAccount = DTOAssembler.ToDTO(DelegateService.generalLedgerService.GetAccountingAccount(Convert.ToInt32(accountingClosing.AccountingAccountCd)));
                ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(accountingClosing.AccountNatureCd);
                ledgerEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                    Value = accountingClosing.TotalAmount
                };
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = accountingClosing.LocalAmountValue };
                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                ledgerEntryItem.Description = Resources.Resources.ExpiredPremiumsAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntryItem.EntryType = new EntryTypeDTO();
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();

                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                ledgerEntry.ModuleDateId = moduleId;
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                ledgerEntry.Status = 1; //activo
                ledgerEntry.UserId = userId;
            }

            return ledgerEntry;
        }

        #endregion ExpiredPremiumsDailyEntry

        #region IncomeAndExpensesDailyEntry

        /// <summary>
        /// GenerateIncomeAndExpensesEntry
        /// Genera los asientos para el cierre de ingresos y egresos.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateIncomeAndExpensesEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in accountingClosings)
            {
                //Cabecera
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                {
                    AccountingCompanyId = accountingCompanyId
                };
                ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                {
                    AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES))
                };
                ledgerEntry.Branch = new BranchDTO() { Id = accountingClosing.BrachCd };
                ledgerEntry.Description = Resources.Resources.ClosingIncomeExpenses + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntry.EntryDestination = new EntryDestinationDTO();
                ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL));
                ledgerEntry.EntryNumber = 0;
                ledgerEntry.Id = 0;

                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                ledgerEntryItem.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccount(Convert.ToInt32(accountingClosing.AccountingAccountCd)).ToDTO();
                ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(accountingClosing.AccountNatureCd);
                ledgerEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                    Value = accountingClosing.TotalAmount
                };
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = accountingClosing.LocalAmountValue };
                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                ledgerEntryItem.Description = Resources.Resources.ClosingIncomeExpenses + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntryItem.EntryType = new EntryTypeDTO();
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();
                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                ledgerEntry.ModuleDateId = moduleId;
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                ledgerEntry.Status = 1; //activo
                ledgerEntry.UserId = userId;
            }

            return ledgerEntry;
        }

        #endregion IncomeAndExpensesDailyEntry

        #region CatastrophicRiskReserveClosure

        /// <summary>
        /// Genera el cierre de reserva de riesgo catastrófico
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public int CatastrophicRiskReserveClosureGeneration(int year, int month, int module)
        {
            try
            {
                var parameters = new NameValue[2];
                int processId = SaveLogProcess(module);

                parameters[0] = new NameValue("YEAR", year);
                parameters[1] = new NameValue("MONTH", month);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.CATASTROPHIC_MONTHLY_CLOSING", parameters);
                }
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_CATASTROPHIC_MONTHLY_CLOSING", parameters);
                }

                UpdateLogProcess(processId);

                return (result != null && result.Rows.Count > 0) ? 1 : 0;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Genera el reporte cierre de reserva de riesgo catastrófico
        /// </summary>
        /// <returns></returns>
        public List<AccountingClosingReportDTO> GetCatastrophicRiskReserveClosureReport()
        {
            var closures = new List<AccountingClosingReportDTO>();

            try
            {
                var parameters = new NameValue[2];
                parameters[0] = new NameValue("@YEAR", DBNull.Value, DbType.Int16);
                parameters[1] = new NameValue("@MONTH", DBNull.Value, DbType.Int16);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_CATASTROPHIC_MONTHLY_CLOSING", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        if (!(Convert.ToDecimal(arrayItem[6]) == 0 && Convert.ToDecimal(arrayItem[7]) == 0))
                        {
                            closures.Add(new AccountingClosingReportDTO()
                            {
                                BrachCd = Convert.ToInt32(arrayItem[0]),
                                BranchDescription = DBNull.ReferenceEquals(arrayItem[1], DBNull.Value) ? String.Empty : arrayItem[1].ToString(),
                                CurrencyCd = Convert.ToInt32(arrayItem[2]),
                                CurrencyDescription = arrayItem[3].ToString(),
                                AccountNatureCd = Convert.ToDecimal(arrayItem[6]) != 0 ? Convert.ToString(Convert.ToInt32(AccountingNatures.Debit)) : Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)),
                                AccountingAccountCd = arrayItem[8].ToString(),
                                AccountingAccountDescription = arrayItem[5].ToString(),
                                LocalAmountValue = Convert.ToDecimal(arrayItem[6]) != 0 ?
                                Convert.ToDecimal(arrayItem[6]) : Convert.ToDecimal(arrayItem[7]),
                                ComponentId = Convert.ToInt32(arrayItem[8])
                            });
                        }
                    }
                }
                return closures;
            }
            catch (BusinessException)
            {
                return closures;
            }
        }

        /// <summary>
        /// Contabiliza el cierre de reserva de riesgo catastrófico
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public int CatastrophicRiskReserveClosureEnding(int year, int month)
        {
            var parameters = new NameValue[2];
            parameters[0] = new NameValue("YEAR", year);
            parameters[1] = new NameValue("MONTH", month);

            try
            {
                // Ejecuta el sp de contabilización   
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.CATASTROPHIC_MONTHLY_ENTRY", parameters);
                }

                // Devuelve el número de asiento
                int entryNumber = Convert.ToInt32((from DataRow item in result.Rows select item[0]).FirstOrDefault());
                return entryNumber;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        public List<string> AccountClosureCatastrophicRiskReserve(DateTime accountingDate, int module, int userId)
        {
            List<string> entryNumbers = new List<string>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            LedgerEntryDTO entryHeader = new LedgerEntryDTO();
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            var entryNumber = 0;
            // Se obtiene los parámetros para generar el asiento
            closures = GetCatastrophicRiskReserveClosureReport();

            if (closures.Count > 0)
            {
                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> accountingClosingReportDTOs;
                accountingClosingReportDTOs = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                foreach (var accountingClosing in accountingClosingReportDTOs)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReportsFiltered;

                    accountingClosingReportsFiltered = (from AccountingClosingReportDTO item in closures where item.PrefixCd == accountingClosing.PrefixCd && item.BrachCd == accountingClosing.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateCatastrophicRiskEntry(accountingClosingReportsFiltered, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                entryHeader.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    entryHeader.AccountingCompany = ledgerEntry.AccountingCompany;
                    entryHeader.AccountingDate = ledgerEntry.AccountingDate;
                    entryHeader.AccountingMovementType = ledgerEntry.AccountingMovementType;
                    entryHeader.Branch = ledgerEntry.Branch;
                    entryHeader.Description = ledgerEntry.Description;
                    entryHeader.EntryDestination = ledgerEntry.EntryDestination;
                    entryHeader.EntryNumber = ledgerEntry.EntryNumber;
                    entryHeader.Id = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        entryHeader.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    entryHeader.ModuleDateId = ledgerEntry.ModuleDateId;
                    entryHeader.RegisterDate = ledgerEntry.RegisterDate;
                    entryHeader.SalePoint = ledgerEntry.SalePoint;
                    entryHeader.Status = ledgerEntry.Status;
                    entryHeader.UserId = ledgerEntry.UserId;
                }

                // Se valida débitos y créditos
                decimal debits = 0;
                decimal credits = 0;

                foreach (LedgerEntryItemDTO accountingEntry in entryHeader.LedgerEntryItems)
                {
                    if (accountingEntry.AccountingNature == (int)AccountingNatures.Debit)
                    {
                        debits = debits + accountingEntry.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + accountingEntry.LocalAmount.Value;
                    }
                }

                if (debits == credits)
                {
                    if (entryHeader.LedgerEntryItems.Count > 10)
                    {
                        // Se borra los datos de la tabla temporal de trabajo
                        DelegateService.generalLedgerService.ClearTempAccountEntry();

                        DelegateService.generalLedgerService.SaveTempEntryItem(entryHeader.ToDTO(), module, false, userId);

                        entryNumber = DelegateService.generalLedgerService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                    }
                    else
                    {
                        entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(entryHeader.ToDTO());
                    }

                    if (entryNumber > 0)
                    {
                        entryNumbers.Add(" " + entryNumber);
                        CatastrophicRiskReserveClosureEnding(accountingDate.Year, accountingDate.Month);
                    }
                    else
                    {
                        entryNumbers.Add(Resources.Resources.EntryRecordingError);
                    }
                }
            }
            return entryNumbers;
        }


        #endregion CatastrophicRiskReserveClosure

        #region CatastrophicRiskDailyEntry

        /// <summary>
        /// GenerateCatastrophicRiskEntry
        /// Genera los asientos para el cierre de reservas riesgos catastróficos.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateCatastrophicRiskEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            // Se arma los movimientos para los tipos de componentes
            foreach (AccountingClosingReportDTO accountingClosing in accountingClosings)
            {
                //Cabecera
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                {
                    AccountingCompanyId = accountingCompanyId
                };
                ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING)), DateTime.Now);
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                {
                    AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES))
                };
                ledgerEntry.Branch = new BranchDTO() { Id = accountingClosing.BrachCd };
                ledgerEntry.Description = Resources.Resources.CatastrophicRiskAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                          + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntry.EntryDestination = new EntryDestinationDTO();
                ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES));
                ledgerEntry.EntryNumber = 0;
                ledgerEntry.Id = 0;

                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                ledgerEntryItem.AccountingAccount = DelegateService.generalLedgerService.GetAccountingAccount(Convert.ToInt32(accountingClosing.AccountingAccountCd)).ToDTO();
                ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(accountingClosing.AccountNatureCd);
                ledgerEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd },
                    Value = accountingClosing.TotalAmount
                };
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = accountingClosing.ExchangeRate };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = accountingClosing.LocalAmountValue };
                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                ledgerEntryItem.Currency = new CurrencyDTO() { Id = accountingClosing.CurrencyCd };
                ledgerEntryItem.Description = Resources.Resources.CatastrophicRiskAccounting + " - " + Resources.Resources.Policy + ": " + Convert.ToString(accountingClosing.PolicyDocumentNumber)
                                                + ", " + Resources.Resources.Endorsement + ": " + Convert.ToString(accountingClosing.EndorsementDocumentNumber);
                ledgerEntryItem.EntryType = new EntryTypeDTO();
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = accountingClosing.PayerId };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();
                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);

                ledgerEntry.ModuleDateId = moduleId;
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                ledgerEntry.Status = 1; //activo
                ledgerEntry.UserId = userId;
            }

            return ledgerEntry;
        }

        #endregion CatastrophicRiskDailyEntry

        #region IncomeAndExpenses
        public List<string> AccountClosureIncomeAndExpenses(DateTime accountingDate, int userId, int day)
        {
            List<string> entryNumbers = new List<string>();
            var entryNumber = 0;

            entryNumber = ExpensesClousureEnding(accountingDate.Year, accountingDate.Month, day);
            entryNumbers.Add(" " + entryNumber);

            return entryNumbers;
        }

        #endregion

        #region ExchangeDifference 

        /// <summary>
        /// GetExchangeDifference
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="rateDate"></param>
        /// <param name="localCurrencyId"></param>
        /// <returns></returns>
        public int GetExchangeDifference(DateTime startDate, DateTime endDate, DateTime rateDate, int accountingYear, int localCurrencyId)
        {
            try
            {
                //Se eliminan los registros no contabilizados de la tabla de reporte.
                DeleteUnpostedRecords(accountingYear);

                List<ExchangeDifferenceReportDTO> reportList = new List<ExchangeDifferenceReportDTO>();

                //Se obtiene el listado de cuentas contables a procesar.
                var parameters = new NameValue[3];
                parameters[0] = new NameValue("START_DATE", Convert.ToDateTime(startDate));
                parameters[1] = new NameValue("END_DATE", Convert.ToDateTime(endDate));
                parameters[2] = new NameValue("LOCAL_CURRENCY_ID", Convert.ToInt32(localCurrencyId));

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_EXCHANGE_DIFFERENCE_ACCOUNTS", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        ExchangeDifferenceReportDTO item = new ExchangeDifferenceReportDTO();
                        item.AccountingAccountNumber = Convert.ToString(arrayItem[0]);
                        item.AccountingAccountId = Convert.ToInt32(arrayItem[1]);
                        item.AccountingAccountName = Convert.ToString(arrayItem[2]);
                        item.Amount = Convert.ToDecimal(arrayItem[3]);
                        item.LocalAmount = Convert.ToDecimal(arrayItem[4]);
                        item.AccountingNature = Convert.ToInt32(arrayItem[5]);
                        item.CurrencyId = Convert.ToInt32(arrayItem[6]);
                        item.ExchangeRate = Convert.ToDecimal(arrayItem[7]);
                        item.BranchId = Convert.ToInt32(arrayItem[8]);
                        item.SalePointId = arrayItem[9] == DBNull.Value ? 0 : Convert.ToInt32(arrayItem[9]);
                        item.EntryDestinationId = Convert.ToInt32(arrayItem[10]);
                        item.AccountingMovementTypeId = Convert.ToInt32(arrayItem[11]);
                        item.Date = Convert.ToDateTime(arrayItem[12]);
                        item.IndividualId = arrayItem[13] == DBNull.Value ? 0 : Convert.ToInt32(arrayItem[13]);

                        reportList.Add(item);
                    }

                    //Se obtiene el listado de cuentas contables y monedas
                    var filterData = reportList.GroupBy(p => new { p.AccountingAccountId, p.CurrencyId }).Select(g => g.First()).ToList();

                    if (reportList.Count > 0)
                    {
                        foreach (var filterDataItem in filterData)
                        {
                            //Se valida que la cuenta y moneda no hayan sido contabilizadas
                            if (ValidatePostedAccount(filterDataItem.AccountingAccountId, filterDataItem.CurrencyId, accountingYear))
                            {
                                List<ExchangeDifferenceReportDTO> filteredTempData;

                                filteredTempData = (from ExchangeDifferenceReportDTO exchangeDifferenceReportDTOItem in reportList where exchangeDifferenceReportDTOItem.AccountingAccountId == filterDataItem.AccountingAccountId && exchangeDifferenceReportDTOItem.CurrencyId == filterDataItem.CurrencyId select exchangeDifferenceReportDTOItem).ToList();

                                decimal localAmountBalance = 0;
                                decimal foreingAmountBalance = 0;

                                decimal debitLocalAmount = 0;
                                decimal creditLocalAmount = 0;
                                decimal debitForeingAmount = 0;
                                decimal creditForeingAmount = 0;
                                decimal currentExchangeRate = 0;

                                decimal exchangeDifference = 0;

                                foreach (var item in filteredTempData)
                                {
                                    if (item.AccountingNature == (int)AccountingNatures.Credit)
                                    {
                                        creditLocalAmount = creditLocalAmount + item.LocalAmount;
                                        creditForeingAmount = creditForeingAmount + item.Amount;
                                    }
                                    if (item.AccountingNature == (int)AccountingNatures.Debit)
                                    {
                                        debitLocalAmount = debitLocalAmount + item.LocalAmount;
                                        debitForeingAmount = debitForeingAmount + item.Amount;
                                    }
                                }

                                localAmountBalance = debitLocalAmount - creditLocalAmount;
                                foreingAmountBalance = debitForeingAmount - creditForeingAmount;
                                currentExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(Convert.ToDateTime(rateDate), filterDataItem.CurrencyId).SellAmount;

                                exchangeDifference = Math.Truncate((foreingAmountBalance * currentExchangeRate) - localAmountBalance);

                                var reportParameters = new NameValue[9];
                                reportParameters[0] = new NameValue("ACCOUNTING_ACCOUNT_CD", Convert.ToInt32(filterDataItem.AccountingAccountId));
                                reportParameters[1] = new NameValue("ACCOUNTING_ACCOUNT_NUMBER", Convert.ToString(filterDataItem.AccountingAccountNumber));
                                reportParameters[2] = new NameValue("ACCOUNTING_ACCOUNT_NAME", Convert.ToString(filterDataItem.AccountingAccountName));
                                reportParameters[3] = new NameValue("CURRENCY_CD", Convert.ToInt32(filterDataItem.CurrencyId));
                                reportParameters[4] = new NameValue("LOCAL_AMOUNT_BALANCE", Convert.ToDecimal(localAmountBalance));
                                reportParameters[5] = new NameValue("FOREIGN_AMOUNT_BALANCE", Convert.ToDecimal(foreingAmountBalance));
                                reportParameters[6] = new NameValue("EXCHANGE_RATE", Convert.ToDecimal(currentExchangeRate));
                                reportParameters[7] = new NameValue("EXCHANGE_DIFFERENCE", Convert.ToDecimal(exchangeDifference));
                                reportParameters[8] = new NameValue("ACCOUNTING_YEAR", Convert.ToInt32(accountingYear));

                                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                                {
                                    dynamicDataAccess.ExecuteSPDataTable("ACL.INSERT_EXCHANGE_DIFFERENCE_REPORT", reportParameters);
                                }
                            }
                        }
                    }
                }

                return 1;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetExchangeDifferenceRecords
        /// </summary>
        /// <returns></returns>
        public List<ExchangeDifferenceReportDTO> GetExchangeDifferenceRecords()
        {
            List<ExchangeDifferenceReportDTO> exchangeDifferenceRecords = new List<ExchangeDifferenceReportDTO>();

            try
            {
                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACL.GET_EXCHANGE_DIFFERENCE_REPORT_RECORDS", null);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        ExchangeDifferenceReportDTO item = new ExchangeDifferenceReportDTO();
                        item.Id = Convert.ToInt32(arrayItem[0]);
                        item.AccountingAccountId = arrayItem[1] == DBNull.Value ? 0 : Convert.ToInt32(arrayItem[1]);
                        item.AccountingAccountNumber = Convert.ToString(arrayItem[2]);
                        item.AccountingAccountName = Convert.ToString(arrayItem[3]);
                        item.CurrencyId = arrayItem[4] == DBNull.Value ? 0 : Convert.ToInt32(arrayItem[4]);
                        item.LocalAmount = arrayItem[5] == DBNull.Value ? 0 : Convert.ToDecimal(arrayItem[5]);
                        item.Amount = arrayItem[6] == DBNull.Value ? 0 : Convert.ToDecimal(arrayItem[6]);
                        item.ExchangeRate = arrayItem[7] == DBNull.Value ? 0 : Convert.ToDecimal(arrayItem[7]);
                        item.ExchangeDifference = arrayItem[8] == DBNull.Value ? 0 : Convert.ToDecimal(arrayItem[8]);
                        item.Posted = arrayItem[10] == DBNull.Value ? Convert.ToBoolean(0) : Convert.ToBoolean(arrayItem[10]);
                        item.AccountingYear = arrayItem[9] == DBNull.Value ? 0 : Convert.ToInt32(arrayItem[9]);

                        exchangeDifferenceRecords.Add(item);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return exchangeDifferenceRecords;
        }

        /// <summary>
        /// DeleteExchangeDifferenceRecord
        /// </summary>
        /// <param name="exchangeDifferenceRecordId"></param>
        public void DeleteExchangeDifferenceRecord(int exchangeDifferenceRecordId)
        {
            try
            {
                var reportParameters = new NameValue[1];

                reportParameters[0] = new NameValue("EXCHANGE_DIFFERENCE_REPORT_ID", Convert.ToInt32(exchangeDifferenceRecordId));

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.DELETE_EXCHANGE_DIFFERENCE_REPORT_RECORD", reportParameters);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// PostExchangeDifferenceRecord
        /// </summary>
        /// <param name="exchangeDifferenceRecordId"></param>
        public void PostExchangeDifferenceRecord(int exchangeDifferenceRecordId)
        {
            try
            {
                var reportParameters = new NameValue[1];

                reportParameters[0] = new NameValue("EXCHANGE_DIFFERENCE_REPORT_ID", Convert.ToInt32(exchangeDifferenceRecordId));
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACL.POST_EXCHANGE_DIFFERENCE_RECORD", reportParameters);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// AccountExchangeDifferenceRecords
        /// </summary>
        public int AccountExchangeDifferenceRecords(int userId)
        {
            int entryNumber = 0;

            try
            {
                //Se obtienen los registros a procesar
                List<ExchangeDifferenceReportDTO> exchangeDifferenceReportDTOs = GetExchangeDifferenceReportRecords();
                int utilityAccountingAccountId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_UTILITY_ACCOUNTING_ACCOUNT));
                int lossAccountingAccountId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_LOSS_ACCOUNTING_ACCOUNT));

                int accountingCompanyId = (from item in DelegateService.generalLedgerService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;


                //Se arma el asiento de mayor
                LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
                ledgerEntry.Id = 0; //autonumérico
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO() { AccountingCompanyId = accountingCompanyId };
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO() { AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_AUTOMATIC_ENTRIES)) };
                ledgerEntry.ModuleDateId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING_CLOSING));
                ledgerEntry.Branch = new BranchDTO() { Id = GetBranchDefaultByUserId(userId) };
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                ledgerEntry.EntryDestination = new EntryDestinationDTO() { DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_DESTINATION_LOCAL)) };
                ledgerEntry.Description = Resources.Resources.AccountExchangeDifference + " " + exchangeDifferenceReportDTOs[0].AccountingYear;
                ledgerEntry.EntryNumber = 0; //Se genera a nivel de servicio.
                ledgerEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING_CLOSING)), DateTime.Now);
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.Status = 1; //activo
                ledgerEntry.UserId = userId;
                ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (ExchangeDifferenceReportDTO item in exchangeDifferenceReportDTOs)
                {
                    int localCurrencyId = DelegateService.tempCommonService.GetCurrencyLocal();

                    LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                    ledgerEntryItem.Currency = new CurrencyDTO() { Id = localCurrencyId };
                    ledgerEntryItem.ExchangeRate = DTOAssembler.ToDTO(DelegateService.commonService.GetExchangeRateByCurrencyId(localCurrencyId));
                    ledgerEntryItem.AccountingAccount = new AccountingAccountDTO() { AccountingAccountId = item.ExchangeDifference >= 0 ? utilityAccountingAccountId : lossAccountingAccountId };
                    ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                    ledgerEntryItem.Receipt = new ReceiptDTO();
                    ledgerEntryItem.AccountingNature = item.ExchangeDifference >= 0 ? (int)AccountingNatures.Credit : (int)AccountingNatures.Debit;
                    ledgerEntryItem.Description = (Resources.Resources.ExchangeDifference + " " + item.AccountingYear + ", " + Resources.Resources.AccountingAccount + ": " + item.AccountingAccountNumber + " - " + item.AccountingAccountName).ToUpper();
                    ledgerEntryItem.Amount = new AmountDTO() { Value = Convert.ToDecimal(item.ExchangeDifference) };
                    ledgerEntryItem.LocalAmount = new AmountDTO() { Value = Convert.ToDecimal(item.ExchangeDifference) };
                    ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = 0 };
                    ledgerEntryItem.EntryType = new EntryTypeDTO() { EntryTypeId = 0 };
                    ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                    ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                    ledgerEntryItem.PostDated = new List<PostDatedDTO>();

                    ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                }

                entryNumber = DelegateService.generalLedgerService.SaveLedgerEntry(ledgerEntry.ToDTO());

                if (entryNumber > 0)
                {
                    //Se actualizan los registros del reporte como contabilizados.
                    foreach (ExchangeDifferenceReportDTO item in exchangeDifferenceReportDTOs)
                    {
                        PostExchangeDifferenceRecord(item.Id);
                    }
                }
            }
            catch (BusinessException)
            {
                entryNumber = 0;
            }

            return entryNumber;
        }


        /// <summary>
        /// GetExchangeDifference
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetExchangeDifferenceDate(string startDate, string endDate)
        {
            startDate = startDate + " 00:00:00";
            endDate = endDate + " 23:59:59";

            string rateDate = Convert.ToString(DateTime.Now);
            ModuleDate module = new ModuleDate();
            module.Id = 9;
            module = DelegateService.tempCommonService.GetModuleDate(module);
            int accountingYear = module.LastClosingYyyy;

            int localCurrencyId = DelegateService.tempCommonService.GetCurrencyLocal();

            return GetExchangeDifference(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), Convert.ToDateTime(rateDate), accountingYear, localCurrencyId);
        }
        /// <summary>
        /// GenerateExchangeDifferenceReport
        /// </summary>
        /// <returns></returns>
        public List<ExchangeDifferenceReportDTO> GenerateExchangeDifferenceReport()
        {
            List<ExchangeDifferenceReportDTO> exchangeDifferenceReportDTO = new List<ExchangeDifferenceReportDTO>();

            List<ExchangeDifferenceReportDTO> exchangeDifferenceReportDTOs = GetExchangeDifferenceReportRecords();

            if (exchangeDifferenceReportDTOs.Count > 0)
            {
                foreach (var item in exchangeDifferenceReportDTOs)
                {
                    string currencyDescription = (from CurrencyDTO currencyItem in DelegateService.commonService.GetCurrencies() where currencyItem.Id == item.CurrencyId select currencyItem).ToList()[0].Description;

                    exchangeDifferenceReportDTO.Add(new ExchangeDifferenceReportDTO()
                    {
                        AccountingAccountId = item.AccountingAccountId,
                        AccountingAccountNumber = item.AccountingAccountNumber,
                        AccountingAccountName = item.AccountingAccountName,
                        AccountingAccountFullName = item.AccountingAccountNumber + " - " + item.AccountingAccountName,
                        CurrencyId = item.CurrencyId,
                        CurrencyDescription = currencyDescription,
                        LocalAmountBalance = item.LocalAmount,
                        ForeignAmountBalance = item.Amount,
                        ExchangeRate = item.ExchangeRate,
                        ExchangeDifference = item.ExchangeDifference
                    });
                }
            }
            return exchangeDifferenceReportDTO;
        }

        #endregion ExchangeDifference 

        #region Reporting

        public List<MassiveReportDTO> GetMassiveReportsByUserIdReportName(int userId, string reportName)
        {
            MassiveReport massiveReport = new MassiveReport
            {
                UserId = userId,
                Description = reportName,
                ModuleId = Convert.ToInt16(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING_CLOSING))
            };

            return DTOAssembler.ToDTOs(DelegateService.reportService.GetMassiveReports(massiveReport));
        }

        public string GenerateStructureReportMassive(int processId, string reportTypeDescription, int exportFormatType, decimal recordsNumber, int userId)
        {
            List<ReportingServices.Models.Parameter> procedureParameters = new List<ReportingServices.Models.Parameter>();
            List<ReportingServices.Models.Parameter> parameters = new List<ReportingServices.Models.Parameter>();
            Report report = new Report();
            int formatId = 0;
            string storedProcedureName = "";
            string exportedFileName = "";

            report.Filter = "";

            #region PRODUCCIÓN DE PRIMAS

            if (reportTypeDescription == (Resources.Resources.ProductionDetailList.ToUpper()))
            {
                formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingPrimeProduction>(AccountingClosingPrimeProduction.ACL_FORMAT_PRODUCTION_DETAIL));
                report.Name = EnumHelper.GetEnumParameterValue<AccountingClosingPrimeProduction>(AccountingClosingPrimeProduction.ACL_TEMPLATE_NAME_PRODUCTION_DETAIL).ToString();
                storedProcedureName = EnumHelper.GetEnumParameterValue<AccountingClosingPrimeProduction>(AccountingClosingPrimeProduction.ACL_PROCEDURE_GET_PRODUCTION_DETAIL).ToString();
            }

            #endregion

            #region CANCELLATION OF RECORDS

            if (reportTypeDescription == (Resources.Resources.CancellationRecord.ToUpper()))
            {
                formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingCancellationRecord>(AccountingClosingCancellationRecord.ACL_FORMAT_CANCELLATION_RECORD_ISSUANCE));
                report.Name = EnumHelper.GetEnumParameterValue<AccountingClosingCancellationRecord>(AccountingClosingCancellationRecord.ACL_TEMPLATE_NAME_CANCELLATION_RECORD_ISSUANCE).ToString();
                storedProcedureName = EnumHelper.GetEnumParameterValue<AccountingClosingCancellationRecord>(AccountingClosingCancellationRecord.ACL_PROCEDURE_GET_CANCELLATION_RECORD_ISSUANCE).ToString();
            }

            #endregion

            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 1,
                Description = "@MASSIVE_REPORT_ID",
                IsFormula = false,
                Value = processId
            });
            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 2,
                Description = "@RECORD_COUNT",
                IsFormula = false,
                Value = recordsNumber
            });
            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 3,
                Description = "@PAGE_SIZE",
                IsFormula = false,
                Value = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_PAGE_SIZE_REPORT))
            });
            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 4,
                Description = "@PAGE_NUMBER",
                IsFormula = false,
                Value = 1
            });

            report.StoredProcedure = new StoredProcedure()
            {
                ProcedureName = storedProcedureName,
                ProcedureParameters = procedureParameters
            };

            report.UserId = userId;

            report.ExportType = ExportTypes.Excel;
            report.IsAsync = true;
            report.Description = Resources.Resources.GenerateDocument;

            report.Format = new Format()
            {
                Id = formatId,
                FileType = FileTypes.Excel
            };

            exportedFileName = "20";

            Format format = new Format();
            format.Id = formatId;
            format.FileType = FileTypes.Text;

            List<FormatDetail> formatDetails = DelegateService.reportService.GetFormatDetailsByFormat(format);

            #region valida campo dinámico

            if (reportTypeDescription == (Resources.Resources.DuePorfolio.ToUpper()))
            {
                parameters.Add(new ReportingServices.Models.Parameter
                {
                    Description = "@WORD_RESERVE",
                    Value = EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_WORD_RESERVE).ToString()
                });

                report.Parameters = parameters;
            }
            #endregion

            if (formatDetails.Count > 0)
            {
                WorkerFactory.Instance.CreateWorkerStructure(report, true);
            }
            else
            {
                exportedFileName = "-1";
            }

            return exportedFileName;
        }

        public int GetProductionDetailReports(string year, string month, string day, int userId, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";

            MassiveReport massiveReport = new MassiveReport();
            Report report = new Report();
            List<ReportingServices.Models.Parameter> procedureParameters = new List<ReportingServices.Models.Parameter>();

            storedProcedureName = EnumHelper.GetEnumParameterValue<AccountingClosingPrimeProduction>(AccountingClosingPrimeProduction.ACL_PROCEDURE_PRODUCTION_DETAIL).ToString();
            massiveReport.Description = Resources.Resources.ProductionDetailList.ToUpper();
            massiveReport.UrlFile = EnumHelper.GetEnumParameterValue<AccountingClosingPrimeProduction>(AccountingClosingPrimeProduction.ACL_TEMPLATE_NAME_PRODUCTION_DETAIL).ToString();

            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 1,
                Description = "@YEAR",
                IsFormula = false,
                Value = year
            });

            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 2,
                Description = "@MONTH",
                IsFormula = false,
                Value = month == "" ? "0" : month
            });
            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 3,
                Description = "@DAY",
                IsFormula = false,
                Value = day == "" ? "0" : day
            });

            report.StoredProcedure = new StoredProcedure()
            {
                ProcedureName = storedProcedureName,
                ProcedureParameters = procedureParameters
            };

            report.Parameters = null;
            report.ExportType = ExportTypes.Excel;

            /*Total de registros*/
            if (process == -1)
            {
                report.IsAsync = false;
                int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                report.StoredProcedure.ProcedureParameters.Add(new ReportingServices.Models.Parameter
                {
                    Id = parameterNumber + 1,
                    Description = "@MASSIVE_REPORT_ID",
                    IsFormula = false,
                    Value = "0"
                });
                report.StoredProcedure.ProcedureParameters.Add(new ReportingServices.Models.Parameter
                {
                    Id = parameterNumber + 2,
                    Description = "@EXECUTE",
                    IsFormula = false,
                    Value = "0"
                });

                totalRecords = DelegateService.reportService.GetTotalRecordsMassiveReport(report);
            }
            else
            {
                report.IsAsync = true;
                massiveReport.Id = 0;
                massiveReport.UserId = userId;
                massiveReport.EndDate = new DateTime(1900, 1, 1);
                massiveReport.GenerationDate = DateTime.Now;
                massiveReport.StartDate = DateTime.Now;
                massiveReport.Success = false;
                massiveReport.ModuleId = Convert.ToInt16(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING_CLOSING));

                WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
            }

            return totalRecords;
        }

        public int GetCancellationRecordIssuanceReports(string year, string month, string day, int userId, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            MassiveReport massiveReport = new MassiveReport();
            Report report = new Report();
            List<ReportingServices.Models.Parameter> procedureParameters = new List<ReportingServices.Models.Parameter>();

            storedProcedureName = EnumHelper.GetEnumParameterValue<AccountingClosingCancellationRecord>(AccountingClosingCancellationRecord.ACL_PROCEDUTE_CANCELLATION_RECORD_ISSUANCE).ToString();
            massiveReport.Description = Resources.Resources.CancellationRecord.ToUpper();
            massiveReport.UrlFile = EnumHelper.GetEnumParameterValue<AccountingClosingCancellationRecord>(AccountingClosingCancellationRecord.ACL_TEMPLATE_NAME_CANCELLATION_RECORD_ISSUANCE).ToString();
            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 1,
                Description = "@YEAR",
                IsFormula = false,
                Value = year
            });

            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 2,
                Description = "@MONTH",
                IsFormula = false,
                Value = month == "" ? "0" : month
            });
            procedureParameters.Add(new ReportingServices.Models.Parameter
            {
                Id = 3,
                Description = "@DAY",
                IsFormula = false,
                Value = day == "" ? "0" : day
            });

            report.StoredProcedure = new StoredProcedure()
            {
                ProcedureName = storedProcedureName,
                ProcedureParameters = procedureParameters
            };

            report.Parameters = null;
            report.ExportType = ExportTypes.Excel;

            /*Total de registros*/
            if (process == -1)
            {
                report.IsAsync = false;
                int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                report.StoredProcedure.ProcedureParameters.Add(new ReportingServices.Models.Parameter
                {
                    Id = parameterNumber + 1,
                    Description = "@MASSIVE_REPORT_ID",
                    IsFormula = false,
                    Value = "0"
                });
                report.StoredProcedure.ProcedureParameters.Add(new ReportingServices.Models.Parameter
                {
                    Id = parameterNumber + 2,
                    Description = "@EXECUTE",
                    IsFormula = false,
                    Value = "0"
                });

                totalRecords = DelegateService.reportService.GetTotalRecordsMassiveReport(report);
            }
            else
            {
                report.IsAsync = true;

                massiveReport.UserId = userId;
                massiveReport.EndDate = new DateTime(1900, 1, 1);
                massiveReport.GenerationDate = DateTime.Now;
                massiveReport.Id = 0;
                massiveReport.StartDate = DateTime.Now;
                massiveReport.Success = false;
                massiveReport.ModuleId = Convert.ToInt16(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_MODULE_DATE_ACCOUNTING_CLOSING).ToString());

                WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
            }

            return totalRecords;
        }

        /// <summary>
        /// LoadMonthlyProcessReport
        /// Carga información para los reportes
        /// </summary>
        /// <param name="module"></param>
        /// <returns>int</returns>
        public List<MonthlyProcessModelDTO> LoadMonthlyProcessReport(int module, int userId, string userName)
        {
            DateTime dateMonthlyClosing = GetClosingDate(module);
            var moduleDates = GetModuleDateDescriptionByModuleId(module);

            var reports = new List<MonthlyProcessModelDTO>();

            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            decimal totalDebit = 0;
            decimal totalCredit = 0;

            #region Issuance

            // Emisión
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_ISSUANCE_MODULE)))
            {
                var accountingDate = GetClosingDate(module);
                string accoutingMonth = Convert.ToString(accountingDate.Month).Length == 2 ? Convert.ToString(accountingDate.Month) : "0" + Convert.ToString(accountingDate.Month);

                DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "00:00:00");
                DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month)) + "/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "23:59:59");

                // Se obtiene los parámetros para generar el asiento
                closures = GetIssuanceClosureReportParameters(startDate, endDate, module);

                // Se obtiene el listado de pólizas y endosos.
                List<AccountingClosingReportDTO> issues;
                issues = closures.GroupBy(p => new { p.PolicyId, p.EndorsementId }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var issueItem in issues)
                {
                    // Se filtra por póliza y endoso
                    List<AccountingClosingReportDTO> accountingClosingByPolicyByEndorsement = (from AccountingClosingReportDTO item in closures where item.PolicyId == issueItem.PolicyId && item.EndorsementId == issueItem.EndorsementId select item).ToList();

                    // Se obtiene el asiento.                    
                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateIssuanceEntry(accountingClosingByPolicyByEndorsement, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDates,
                            User = userName
                        });

                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion Issuance

            #region Reinsurance

            // Reaseguros
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_REINSURANCE_MODULE)))
            {
                // Se obtiene el id de proceso
                int processId = GetAccountingClosing(module).Id;

                // Se recupera los registro del reporte
                List<AccountingClosingReportDTO> reinsurances = GetReinsuranceClosureReportByModuleIdAndProcessId(module, processId);

                reports = reinsurances
                    .GroupBy(x => x.AccountingAccountCd)
                    .Select(
                        resultGroup => new MonthlyProcessModelDTO
                        {
                            BranchCode = 0,
                            BranchDescription = "",
                            CurrencyCode = 0,
                            CurrencyDescription = resultGroup.First().CurrencyDescription,
                            AccountNatureCode = "",
                            AccountingAccountDescription = Convert.ToString(resultGroup.First().AccountingAccountDescription),
                            AccountingAccountCode = resultGroup.First().AccountingAccountCd,
                            Credit = (resultGroup.First().AccountNatureCd.Equals("1")) ? resultGroup.First().TotalAmount : Convert.ToDecimal(0),
                            Debit = (resultGroup.Last().AccountNatureCd.Equals("2")) ? resultGroup.First().TotalAmount : Convert.ToDecimal(0),
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                                    DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                                + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDates,
                            User = userName
                        }
                    ).ToList();

                totalCredit = reports.Sum(x => x.Credit);
                totalDebit = reports.Sum(x => x.Debit);
            }

            #endregion Reinsurance

            #region Claims

            // Siniestros
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CLAIMS_MODULE)))
            {
                closures = GetClaimClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> claims = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var claimItem in claims)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReportFiltered = (from AccountingClosingReportDTO item in closures where item.PrefixCd == claimItem.PrefixCd && item.BrachCd == claimItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateClaimReserveEntry(accountingClosingReportFiltered, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDates,
                            User = userName
                        });

                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion Claims

            #region RiskReserve

            // Reserva de riesgos
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_TECHNICAL_RESERVE_MODULE)))
            {
                closures = GetClaimReserveClosureReport();

                // Se agrupa por sucursal, ramo y moneda
                List<AccountingClosingReportDTO> risks = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd, p.CurrencyCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var riskItem in risks)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReportFiltered = (from AccountingClosingReportDTO item in closures where item.PrefixCd == riskItem.PrefixCd && item.BrachCd == riskItem.BrachCd && item.CurrencyCd == riskItem.CurrencyCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateRiskReserveEntry(accountingClosingReportFiltered, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDates,
                            User = userName
                        });

                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion RiskReserve

            #region IBNR Reserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_IBNR_MODULE)))
            {
                closures = IbnrClosureReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    reports.Add(new MonthlyProcessModelDTO()
                    {
                        BranchCode = accountingClosing.BrachCd,
                        BranchDescription = accountingClosing.BranchDescription,
                        CurrencyCode = accountingClosing.CurrencyCd,
                        CurrencyDescription = accountingClosing.CurrencyDescription,
                        AccountNatureCode = (accountingClosing.AccountNatureCd.Equals("1")) ? @Resources.Resources.Credit.ToUpper()
                        : @Resources.Resources.Debit.ToUpper(),
                        AccountingAccountCode = accountingClosing.AccountingAccountCd,
                        AccountingAccountDescription = accountingClosing.AccountingAccountDescription,
                        Debit = (accountingClosing.AccountNatureCd.Equals("1")) ? 0 : accountingClosing.LocalAmountValue,
                        Credit = (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0,
                        Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                    DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                    + dateMonthlyClosing.Year.ToString(),
                        AccountDate = dateMonthlyClosing.ToShortDateString(),
                        Description = moduleDates,
                        User = userName
                    });

                    totalCredit += (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("2")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            #region PrevisionReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_RISK_PREVENTION_MODULE)))
            {
                closures = GetRiskPreventionReserveClosureReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    reports.Add(new MonthlyProcessModelDTO()
                    {
                        BranchCode = accountingClosing.BrachCd,
                        BranchDescription = accountingClosing.BranchDescription,
                        CurrencyCode = accountingClosing.CurrencyCd,
                        CurrencyDescription = accountingClosing.CurrencyDescription,
                        AccountNatureCode = (accountingClosing.AccountNatureCd.Equals("1")) ? @Resources.Resources.Credit.ToUpper()
                        : @Resources.Resources.Debit.ToUpper(),
                        AccountingAccountCode = accountingClosing.AccountingAccountCd,
                        AccountingAccountDescription = accountingClosing.AccountingAccountDescription,
                        Debit = (accountingClosing.AccountNatureCd.Equals("1")) ? 0 : accountingClosing.LocalAmountValue,
                        Credit = (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0,
                        Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                    DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                    + dateMonthlyClosing.Year.ToString(),
                        AccountDate = dateMonthlyClosing.ToShortDateString(),
                        Description = moduleDates,
                        User = userName
                    });

                    totalCredit += (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("2")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            #region CatastrophicRisk

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CATASTROPHIC_RISK_RESERVE_MODULE)))
            {
                closures = GetCatastrophicRiskReserveClosureReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    reports.Add(new MonthlyProcessModelDTO()
                    {
                        BranchCode = accountingClosing.BrachCd,
                        BranchDescription = accountingClosing.BranchDescription,
                        CurrencyCode = accountingClosing.CurrencyCd,
                        CurrencyDescription = accountingClosing.CurrencyDescription,
                        AccountNatureCode = (accountingClosing.AccountNatureCd.Equals("C")) ? @Resources.Resources.Credit.ToUpper()
                        : @Resources.Resources.Debit.ToUpper(),
                        AccountingAccountCode = accountingClosing.AccountingAccountCd,
                        AccountingAccountDescription = accountingClosing.AccountingAccountDescription,
                        Debit = (accountingClosing.AccountNatureCd.Equals("C")) ? 0 : accountingClosing.LocalAmountValue,
                        Credit = (accountingClosing.AccountNatureCd.Equals("C")) ? accountingClosing.LocalAmountValue : 0,
                        Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                    DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                    + dateMonthlyClosing.Year.ToString(),
                        AccountDate = dateMonthlyClosing.ToShortDateString(),
                        Description = moduleDates,
                        User = userName
                    });

                    totalCredit += (accountingClosing.AccountNatureCd.Equals("C")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("D")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            #region ExpiredPremiums

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_EXPIRED_PREMIUMS_MODULE)))
            {
                closures = ExpiredPremiumsReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    reports.Add(new MonthlyProcessModelDTO()
                    {
                        BranchCode = accountingClosing.BrachCd,
                        BranchDescription = accountingClosing.BranchDescription,
                        CurrencyCode = accountingClosing.CurrencyCd,
                        CurrencyDescription = accountingClosing.CurrencyDescription,
                        AccountNatureCode = (accountingClosing.AccountNatureCd.Equals("C")) ? @Resources.Resources.Credit.ToUpper()
                        : @Resources.Resources.Debit.ToUpper(),
                        AccountingAccountCode = accountingClosing.AccountingAccountCd,
                        AccountingAccountDescription = accountingClosing.AccountingAccountDescription,
                        Debit = (accountingClosing.AccountNatureCd.Equals("1")) ? 0 : accountingClosing.LocalAmountValue,
                        Credit = (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0,
                        Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                    DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                    + dateMonthlyClosing.Year.ToString(),
                        AccountDate = dateMonthlyClosing.ToShortDateString(),
                        Description = moduleDates,
                        User = userName
                    });

                    totalCredit += (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("2")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            return reports;
        }

        /// <summary>
        /// LoadMonthlyProcessReport
        /// Carga información para los reportes
        /// </summary>
        /// <param name="module"></param>
        /// <returns>int</returns>
        public List<MonthlyProcessSummaryModelDTO> LoadMonthlyProcessReportSummaries(int module, int userId)
        {
            DateTime dateMonthlyClosing = GetClosingDate(module);
            var moduleDates = GetModuleDateDescriptionByModuleId(module);
            var reportSummaries = new List<MonthlyProcessSummaryModelDTO>();

            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            decimal totalDebit = 0;
            decimal totalCredit = 0;

            #region Issuance

            // Emisión
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_ISSUANCE_MODULE)))
            {
                var accountingDate = GetClosingDate(module);
                string accoutingMonth = Convert.ToString(accountingDate.Month).Length == 2 ? Convert.ToString(accountingDate.Month) : "0" + Convert.ToString(accountingDate.Month);

                DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "00:00:00");
                DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month)) + "/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "23:59:59");

                // Se obtiene los parámetros para generar el asiento
                closures = GetIssuanceClosureReportParameters(startDate, endDate, module);

                // Se obtiene el listado de pólizas y endosos.
                List<AccountingClosingReportDTO> issues;
                issues = closures.GroupBy(p => new { p.PolicyId, p.EndorsementId }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var issueItem in issues)
                {
                    // Se filtra por póliza y endoso
                    List<AccountingClosingReportDTO> accountingClosingByPolicyByEndorsement = (from AccountingClosingReportDTO item in closures where item.PolicyId == issueItem.PolicyId && item.EndorsementId == issueItem.EndorsementId select item).ToList();

                    // Se obtiene el asiento.                    
                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateIssuanceEntry(accountingClosingByPolicyByEndorsement, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion Issuance

            #region Reinsurance

            // Reaseguros
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_REINSURANCE_MODULE)))
            {
                // Se obtiene el id de proceso
                int processId = GetAccountingClosing(module).Id;

                // Se recupera los registro del reporte
                List<AccountingClosingReportDTO> reinsurances = GetReinsuranceClosureReportByModuleIdAndProcessId(module, processId);

                totalCredit = reinsurances.GroupBy(x => x.AccountingAccountCd).Where(y => y.First().AccountNatureCd.Equals("1")).Sum(z => z.Sum(x => x.TotalAmount));

                totalDebit = reinsurances.GroupBy(x => x.AccountingAccountCd).Where(y => y.First().AccountNatureCd.Equals("2")).Sum(z => z.Sum(x => x.TotalAmount));
            }

            #endregion Reinsurance

            #region Claims

            // Siniestros
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CLAIMS_MODULE)))
            {
                closures = GetClaimClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> claims = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var claimItem in claims)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReportFiltered = (from AccountingClosingReportDTO item in closures where item.PrefixCd == claimItem.PrefixCd && item.BrachCd == claimItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateClaimReserveEntry(accountingClosingReportFiltered, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion Claims

            #region RiskReserve

            // Reserva de riesgos
            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_TECHNICAL_RESERVE_MODULE)))
            {
                closures = GetClaimReserveClosureReport();

                // Se agrupa por sucursal, ramo y moneda
                List<AccountingClosingReportDTO> risks = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd, p.CurrencyCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var riskItem in risks)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReportFiltered = (from AccountingClosingReportDTO item in closures where item.PrefixCd == riskItem.PrefixCd && item.BrachCd == riskItem.BrachCd && item.CurrencyCd == riskItem.CurrencyCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateRiskReserveEntry(accountingClosingReportFiltered, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion RiskReserve

            #region IBNR Reserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_IBNR_MODULE)))
            {
                closures = IbnrClosureReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    totalCredit += (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("2")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            #region PrevisionReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_RISK_PREVENTION_MODULE)))
            {
                closures = GetRiskPreventionReserveClosureReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    totalCredit += (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("2")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            #region CatastrophicRisk

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CATASTROPHIC_RISK_RESERVE_MODULE)))
            {
                closures = GetCatastrophicRiskReserveClosureReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    totalCredit += (accountingClosing.AccountNatureCd.Equals("C")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("D")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            #region ExpiredPremiums

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_EXPIRED_PREMIUMS_MODULE)))
            {
                closures = ExpiredPremiumsReport();

                foreach (AccountingClosingReportDTO accountingClosing in closures)
                {
                    totalCredit += (accountingClosing.AccountNatureCd.Equals("1")) ? accountingClosing.LocalAmountValue : 0;
                    totalDebit += (accountingClosing.AccountNatureCd.Equals("2")) ? accountingClosing.LocalAmountValue : 0;
                }
            }

            #endregion

            reportSummaries.Add(new MonthlyProcessSummaryModelDTO
            {
                TotalCredit = totalCredit,
                TotalDebit = totalDebit
            });


            return reportSummaries;
        }

        /// <summary>
        /// LoadPrintEntryReport
        /// Carga datos para el reporte
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public List<MonthlyProcessModelDTO> LoadPrintEntryReport(int entry, int module, int userId, string userName)
        {
            DateTime dateMonthlyClosing = GetClosingDate(module);
            var moduleDescription = GetModuleDateDescriptionByModuleId(module);
            var reports = new List<MonthlyProcessModelDTO>();
            List<AccountingClosingReportDTO> accountingClosingReportDTOs = new List<AccountingClosingReportDTO>();
            #region Issuance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_ISSUANCE_MODULE)))
            {
                var accountingDate = GetClosingDate(module);
                string accoutingMonth = Convert.ToString(accountingDate.Month).Length == 2 ? Convert.ToString(accountingDate.Month) : "0" + Convert.ToString(accountingDate.Month);

                DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "00:00:00");
                DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month)) + "/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "23:59:59");

                // Se obtiene los parámetros para generar el asiento
                accountingClosingReportDTOs = GetIssuanceClosureReportParameters(startDate, endDate, module);

                // Se obtiene el listado de pólizas y endosos.
                List<AccountingClosingReportDTO> issues = accountingClosingReportDTOs.GroupBy(p => new { p.PolicyId, p.EndorsementId }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var issueItem in issues)
                {
                    // Se filtra por póliza y endoso
                    List<AccountingClosingReportDTO> accountingClosingFiltered = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PolicyId == issueItem.PolicyId && item.EndorsementId == issueItem.EndorsementId select item).ToList();

                    // Se obtiene el asiento.                    
                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateIssuanceEntry(accountingClosingFiltered, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion Issuance

            #region Reinsurance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_REINSURANCE_MODULE)))
            {
                // Se obtiene el id de proceso
                int processId = GetAccountingClosing(module).Id;

                // Tamaño de la página
                int pageSize = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_REINSURANCE_ACCOUNTING_CLOSING_PAGE_SIZE_PARAMETER));

                // Se obtiene el número de registros.
                int records = GetGeneratedRecordsCount(module, processId);

                // Se obtiene el número de páginas.
                decimal pages = System.Math.Ceiling(Convert.ToDecimal(records) / Convert.ToDecimal(pageSize));

                // Se carga los registros en una lista.
                for (int i = 1; i <= pages; i++)
                {
                    var accountingClosingReports = GetGeneratedClosureReportRecords(module, processId, pageSize, i, records);

                    if (accountingClosingReports.Count > 0)
                    {
                        foreach (var accountingClosing in accountingClosingReports)
                        {
                            accountingClosingReportDTOs.Add(accountingClosing);
                        }
                    }
                }

                // Se filtra los registros por número de asiento en los registros de pre-cierre.
                var accountingClosingFiltered = accountingClosingReportDTOs.GroupBy(x => x.EntryNumber).Select(y => y.First()).ToList();

                // Se arma los asientos
                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var accountingClosing in accountingClosingFiltered)
                {
                    var filteredEntry = (from tempEntryRecord in accountingClosingReportDTOs where tempEntryRecord.EntryNumber == accountingClosing.EntryNumber select tempEntryRecord).ToList();

                    LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
                    ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                    if (filteredEntry.Count > 0)
                    {
                        foreach (var entryItem in filteredEntry)
                        {
                            //Cabecera
                            ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                            {
                                AccountingCompanyId = entryItem.AccountingCompanyId
                            };
                            ledgerEntry.AccountingDate = entryItem.Date;
                            ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                            {
                                AccountingMovementTypeId = entryItem.AccountingMovementTypeId
                            };
                            ledgerEntry.Branch = new BranchDTO() { Id = entryItem.BrachCd };
                            ledgerEntry.Description = entryItem.Description;
                            ledgerEntry.EntryDestination = new EntryDestinationDTO() { DestinationId = entryItem.EntryDestinationId };
                            ledgerEntry.EntryNumber = 0;
                            ledgerEntry.Id = 0;

                            //Detalle
                            LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                            ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                            ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(entryItem.AccountingAccountCd);
                            ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(entryItem.AccountNatureCd);
                            ledgerEntryItem.Amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = entryItem.CurrencyCd },
                                Value = entryItem.TotalAmount
                            };
                            ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = entryItem.ExchangeRate };
                            ledgerEntryItem.LocalAmount = new AmountDTO() { Value = entryItem.LocalAmountValue };
                            ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                            ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                            {
                                Id = entryItem.BankReconciliationId
                            };
                            ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                            ledgerEntryItem.Currency = new CurrencyDTO { Id = entryItem.CurrencyCd };
                            ledgerEntryItem.Description = entryItem.Description;
                            ledgerEntryItem.EntryType = new EntryTypeDTO() { EntryTypeId = 0 };
                            ledgerEntryItem.Id = 0;
                            ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = entryItem.PayerId };
                            ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                            ledgerEntryItem.Receipt = new ReceiptDTO();
                            ledgerEntryItem.Receipt.Number = entryItem.ReceiptNumber;
                            ledgerEntryItem.Receipt.Date = entryItem.ReceiptDate;

                            ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                            ledgerEntry.ModuleDateId = 1;
                            ledgerEntry.RegisterDate = DateTime.Now;
                            ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                            ledgerEntry.Status = 1; //activo
                            ledgerEntry.UserId = userId;
                        }
                    }
                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion Reinsurance

            #region ClaimReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CLAIMS_MODULE)))
            {
                accountingClosingReportDTOs = GetClaimClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> claims = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var claimItem in claims)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingByBranchByPrefix = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == claimItem.PrefixCd && item.BrachCd == claimItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateClaimReserveEntry(accountingClosingByBranchByPrefix, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion ClaimReserve

            #region RiskReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_TECHNICAL_RESERVE_MODULE)))
            {
                accountingClosingReportDTOs = GetClaimReserveClosureReport();

                // Se agrupa por sucursal, ramo y moneda
                List<AccountingClosingReportDTO> risks = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd, p.CurrencyCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var riskItem in risks)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingByPrefixByBranch = (
                        from AccountingClosingReportDTO accountingClosingItem in accountingClosingReportDTOs where accountingClosingItem.PrefixCd == riskItem.PrefixCd && accountingClosingItem.BrachCd == riskItem.BrachCd && accountingClosingItem.CurrencyCd == riskItem.CurrencyCd select accountingClosingItem).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateRiskReserveEntry(accountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion RiskReserve

            #region IBNR Reserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_IBNR_MODULE)))
            {
                accountingClosingReportDTOs = IbnrClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> reserves = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var reserveItem in reserves)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReports = (from AccountingClosingReportDTO accountingClosing in accountingClosingReportDTOs
                                                                                 where accountingClosing.PrefixCd == reserveItem.PrefixCd && accountingClosing.BrachCd == reserveItem.BrachCd
                                                                                 select accountingClosing).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateIBNRReserveEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion

            #region PrevisionReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_RISK_PREVENTION_MODULE)))
            {
                accountingClosingReportDTOs = GetRiskPreventionReserveClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> previsions = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var previsionItem in previsions)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingReports = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == previsionItem.PrefixCd && item.BrachCd == previsionItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GeneratePrevisionReserveEntry(AccountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion

            #region CatastrophicRisk

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CATASTROPHIC_RISK_RESERVE_MODULE)))
            {
                accountingClosingReportDTOs = GetCatastrophicRiskReserveClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> catastrophics = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var catastrophicItem in catastrophics)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == catastrophicItem.PrefixCd && item.BrachCd == catastrophicItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateCatastrophicRiskEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion

            #region ExpiredPremiums

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_EXPIRED_PREMIUMS_MODULE)))
            {
                accountingClosingReportDTOs = ExpiredPremiumsReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> premiums = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var premiumItem in premiums)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == premiumItem.PrefixCd && item.BrachCd == premiumItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateExpiredPremiumsEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion

            #region IncomeAndExpenses

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_INCOME_AND_EXPENSES_MODULE)))
            {
                accountingClosingReportDTOs = ExpensesClousureReport();
                var closures = from closure in accountingClosingReportDTOs
                               where closure.EntryNumber == entry
                               select closure;

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> expenses = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var expenseItem in expenses)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == expenseItem.PrefixCd && item.BrachCd == expenseItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateIncomeAndExpensesEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        reports.Add(new MonthlyProcessModelDTO()
                        {
                            BranchCode = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCode = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCode = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            Debit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? 0 : ledgerEntryItem.LocalAmount.Value,
                            Credit = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0,
                            Title = "PARTIDA DE PRODUCCION CORRESPONDIENTE\nAL MES DE " + CultureInfo.CurrentCulture.
                                        DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                        + dateMonthlyClosing.Year.ToString(),
                            AccountDate = dateMonthlyClosing.ToShortDateString(),
                            Description = moduleDescription,
                            User = userName
                        });
                    }
                }
            }

            #endregion

            return reports;
        }

        public List<MonthlyProcessSummaryModelDTO> LoadSummaryEntryReport(int entry, int module, int userId)
        {
            DateTime dateMonthlyClosing = GetClosingDate(module);
            var moduleDescription = GetModuleDateDescriptionByModuleId(module);
            var reportSummaries = new List<MonthlyProcessSummaryModelDTO>();
            List<AccountingClosingReportDTO> accountingClosingReportDTOs = new List<AccountingClosingReportDTO>();

            decimal totalDebit = 0;
            decimal totalCredit = 0;

            #region Issuance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_ISSUANCE_MODULE)))
            {
                var accountingDate = GetClosingDate(module);
                string accoutingMonth = Convert.ToString(accountingDate.Month).Length == 2 ? Convert.ToString(accountingDate.Month) : "0" + Convert.ToString(accountingDate.Month);

                DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "00:00:00");
                DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month)) + "/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "23:59:59");

                // Se obtiene los parámetros para generar el asiento
                accountingClosingReportDTOs = GetIssuanceClosureReportParameters(startDate, endDate, module);

                // Se obtiene el listado de pólizas y endosos.
                List<AccountingClosingReportDTO> issues = accountingClosingReportDTOs.GroupBy(p => new { p.PolicyId, p.EndorsementId }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var issueItem in issues)
                {
                    // Se filtra por póliza y endoso
                    List<AccountingClosingReportDTO> accountingClosingFiltered = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PolicyId == issueItem.PolicyId && item.EndorsementId == issueItem.EndorsementId select item).ToList();

                    // Se obtiene el asiento.                    
                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateIssuanceEntry(accountingClosingFiltered, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion Issuance

            #region Reinsurance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_REINSURANCE_MODULE)))
            {
                // Se obtiene el id de proceso
                int processId = GetAccountingClosing(module).Id;

                // Tamaño de la página
                int pageSize = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosing>(AccountingClosing.ACL_REINSURANCE_ACCOUNTING_CLOSING_PAGE_SIZE_PARAMETER));

                // Se obtiene el número de registros.
                int records = GetGeneratedRecordsCount(module, processId);

                // Se obtiene el número de páginas.
                decimal pages = System.Math.Ceiling(Convert.ToDecimal(records) / Convert.ToDecimal(pageSize));

                // Se carga los registros en una lista.
                for (int i = 1; i <= pages; i++)
                {
                    var accountingClosingReports = GetGeneratedClosureReportRecords(module, processId, pageSize, i, records);

                    if (accountingClosingReports.Count > 0)
                    {
                        foreach (var accountingClosing in accountingClosingReports)
                        {
                            accountingClosingReportDTOs.Add(accountingClosing);
                        }
                    }
                }

                // Se filtra los registros por número de asiento en los registros de pre-cierre.
                var accountingClosingFiltered = accountingClosingReportDTOs.GroupBy(x => x.EntryNumber).Select(y => y.First()).ToList();

                // Se arma los asientos
                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var accountingClosing in accountingClosingFiltered)
                {
                    var filteredEntry = (from tempEntryRecord in accountingClosingReportDTOs where tempEntryRecord.EntryNumber == accountingClosing.EntryNumber select tempEntryRecord).ToList();

                    LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
                    ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                    if (filteredEntry.Count > 0)
                    {
                        foreach (var entryItem in filteredEntry)
                        {
                            //Cabecera
                            ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                            {
                                AccountingCompanyId = entryItem.AccountingCompanyId
                            };
                            ledgerEntry.AccountingDate = entryItem.Date;
                            ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                            {
                                AccountingMovementTypeId = entryItem.AccountingMovementTypeId
                            };
                            ledgerEntry.Branch = new BranchDTO() { Id = entryItem.BrachCd };
                            ledgerEntry.Description = entryItem.Description;
                            ledgerEntry.EntryDestination = new EntryDestinationDTO() { DestinationId = entryItem.EntryDestinationId };
                            ledgerEntry.EntryNumber = 0;
                            ledgerEntry.Id = 0;

                            //Detalle
                            LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                            ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                            ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(entryItem.AccountingAccountCd);
                            ledgerEntryItem.AccountingNature = (int)(AccountingNatures)Convert.ToInt32(entryItem.AccountNatureCd);
                            ledgerEntryItem.Amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = entryItem.CurrencyCd },
                                Value = entryItem.TotalAmount
                            };
                            ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = entryItem.ExchangeRate };
                            ledgerEntryItem.LocalAmount = new AmountDTO() { Value = entryItem.LocalAmountValue };
                            ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                            ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                            {
                                Id = entryItem.BankReconciliationId
                            };
                            ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                            ledgerEntryItem.Currency = new CurrencyDTO { Id = entryItem.CurrencyCd };
                            ledgerEntryItem.Description = entryItem.Description;
                            ledgerEntryItem.EntryType = new EntryTypeDTO() { EntryTypeId = 0 };
                            ledgerEntryItem.Id = 0;
                            ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = entryItem.PayerId };
                            ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                            ledgerEntryItem.Receipt = new ReceiptDTO();
                            ledgerEntryItem.Receipt.Number = entryItem.ReceiptNumber;
                            ledgerEntryItem.Receipt.Date = entryItem.ReceiptDate;

                            ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                            ledgerEntry.ModuleDateId = 1;
                            ledgerEntry.RegisterDate = DateTime.Now;
                            ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 };
                            ledgerEntry.Status = 1; //activo
                            ledgerEntry.UserId = userId;
                        }
                    }
                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion Reinsurance

            #region ClaimReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CLAIMS_MODULE)))
            {
                accountingClosingReportDTOs = GetClaimClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> claims = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var claimItem in claims)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingByBranchByPrefix = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == claimItem.PrefixCd && item.BrachCd == claimItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateClaimReserveEntry(accountingClosingByBranchByPrefix, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion ClaimReserve

            #region RiskReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_TECHNICAL_RESERVE_MODULE)))
            {
                accountingClosingReportDTOs = GetClaimReserveClosureReport();

                // Se agrupa por sucursal, ramo y moneda
                List<AccountingClosingReportDTO> risks = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd, p.CurrencyCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var riskItem in risks)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingByPrefixByBranch = (
                        from AccountingClosingReportDTO accountingClosingItem in accountingClosingReportDTOs where accountingClosingItem.PrefixCd == riskItem.PrefixCd && accountingClosingItem.BrachCd == riskItem.BrachCd && accountingClosingItem.CurrencyCd == riskItem.CurrencyCd select accountingClosingItem).ToList();

                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateRiskReserveEntry(accountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion RiskReserve

            #region IBNR Reserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_IBNR_MODULE)))
            {
                accountingClosingReportDTOs = IbnrClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> reserves = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var reserveItem in reserves)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> accountingClosingReports = (from AccountingClosingReportDTO accountingClosing in accountingClosingReportDTOs
                                                                                 where accountingClosing.PrefixCd == reserveItem.PrefixCd && accountingClosing.BrachCd == reserveItem.BrachCd
                                                                                 select accountingClosing).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateIBNRReserveEntry(accountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion

            #region PrevisionReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_RISK_PREVENTION_MODULE)))
            {
                accountingClosingReportDTOs = GetRiskPreventionReserveClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> previsions = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var previsionItem in previsions)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingReports = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == previsionItem.PrefixCd && item.BrachCd == previsionItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GeneratePrevisionReserveEntry(AccountingClosingReports, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion

            #region CatastrophicRisk

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CATASTROPHIC_RISK_RESERVE_MODULE)))
            {
                accountingClosingReportDTOs = GetCatastrophicRiskReserveClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> catastrophics = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var catastrophicItem in catastrophics)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == catastrophicItem.PrefixCd && item.BrachCd == catastrophicItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateCatastrophicRiskEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion

            #region ExpiredPremiums

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_EXPIRED_PREMIUMS_MODULE)))
            {
                accountingClosingReportDTOs = ExpiredPremiumsReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> premiums = accountingClosingReportDTOs.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var premiumItem in premiums)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in accountingClosingReportDTOs where item.PrefixCd == premiumItem.PrefixCd && item.BrachCd == premiumItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateExpiredPremiumsEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion

            #region IncomeAndExpenses

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_INCOME_AND_EXPENSES_MODULE)))
            {
                accountingClosingReportDTOs = ExpensesClousureReport();
                var closures = from closure in accountingClosingReportDTOs
                               where closure.EntryNumber == entry
                               select closure;

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> expenses = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var expenseItem in expenses)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == expenseItem.PrefixCd && item.BrachCd == expenseItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateIncomeAndExpensesEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        totalCredit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                        totalDebit += (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Debit)) ? ledgerEntryItem.LocalAmount.Value : 0;
                    }
                }
            }

            #endregion

            var summary = new MonthlyProcessSummaryModelDTO();

            summary.TotalCredit = totalCredit;
            summary.TotalDebit = totalDebit;

            reportSummaries.Add(summary);

            return reportSummaries;
        }

        /// <summary>
        /// ReportExcel
        /// </summary>
        /// <param name="module"></param>
        /// <returns>ActionResult</returns>
        public List<AccountingClosingReportDTO> ReportExcel(int module, int userId)
        {
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            List<AccountingClosingReportDTO> excelReports = new List<AccountingClosingReportDTO>();

            #region Issuance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_ISSUANCE_MODULE)))
            {
                var accountingDate = GetClosingDate(module);
                string accoutingMonth = Convert.ToString(accountingDate.Month).Length == 2 ? Convert.ToString(accountingDate.Month) : "0" + Convert.ToString(accountingDate.Month);

                DateTime startDate = Convert.ToDateTime("01/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "00:00:00");
                DateTime endDate = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month)) + "/" + accoutingMonth + "/" + Convert.ToString(accountingDate.Year) + " " + "23:59:59");

                // Se obtiene los parámetros para generar el asiento
                closures = GetIssuanceClosureReportParameters(startDate, endDate, module);

                // Se obtiene el listado de pólizas y endosos.
                List<AccountingClosingReportDTO> issues = closures.GroupBy(p => new { p.PolicyId, p.EndorsementId }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var issueItem in issues)
                {
                    // Se filtra por póliza y endoso
                    List<AccountingClosingReportDTO> accountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PolicyId == issueItem.PolicyId && item.EndorsementId == issueItem.EndorsementId select item).ToList();

                    // Se obtiene el asiento
                    LedgerEntryDTO ledgerEntry;
                    ledgerEntry = GenerateIssuanceEntry(accountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        excelReports.Add(new AccountingClosingReportDTO()
                        {
                            BrachCd = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCd = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCd = Convert.ToString(Convert.ToInt32(ledgerEntryItem.AccountingNature)),
                            AccountNatureDescription = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            LocalAmountValue = ledgerEntryItem.LocalAmount.Value,
                            TotalAmount = ledgerEntryItem.Amount.Value
                        });
                    }
                }
            }
            #endregion Issuance

            #region Reinsurance

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_REINSURANCE_MODULE)))
            {
                // Se obtiene el id de proceso
                int processId = GetAccountingClosing(module).Id;

                // Se recupera los registros del reporte
                List<AccountingClosingReportDTO> reinsurances = GetReinsuranceClosureReportByModuleIdAndProcessId(module, processId);

                // Se agrupa por cuenta contable
                excelReports = reinsurances
                    .GroupBy(x => x.AccountingAccountCd)
                    .Select(
                        resultGroup => new AccountingClosingReportDTO
                        {
                            BrachCd = resultGroup.First().BrachCd,
                            BranchDescription = GetBranchDescriptionByBranchId(resultGroup.First().BrachCd),
                            CurrencyCd = resultGroup.First().CurrencyCd,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(resultGroup.First().CurrencyCd),
                            AccountNatureCd = resultGroup.First().AccountNatureCd,
                            AccountNatureDescription = (resultGroup.First().AccountNatureCd.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = resultGroup.First().AccountingAccountCd,
                            AccountingAccountDescription = resultGroup.First().AccountingAccountDescription,
                            LocalAmountValue = resultGroup.First().TotalAmount,
                            TotalAmount = resultGroup.First().TotalAmount
                        }
                    ).ToList<AccountingClosingReportDTO>();
            }

            #endregion Reinsurance

            #region Claims

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CLAIMS_MODULE)))
            {
                closures = GetClaimClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> claimsByPrefixByBranch = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var claimItem in claimsByPrefixByBranch)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == claimItem.PrefixCd && item.BrachCd == claimItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateClaimReserveEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        excelReports.Add(new AccountingClosingReportDTO()
                        {
                            BrachCd = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCd = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCd = Convert.ToString(Convert.ToInt32(ledgerEntryItem.AccountingNature)),
                            AccountNatureDescription = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            LocalAmountValue = ledgerEntryItem.LocalAmount.Value,
                            TotalAmount = ledgerEntryItem.Amount.Value
                        });
                    }
                }
            }

            #endregion Claims

            #region RiskReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_TECHNICAL_RESERVE_MODULE)))
            {
                closures = GetClaimReserveClosureReport();

                // Se agrupa por sucursal, ramo y moneda
                List<AccountingClosingReportDTO> risks = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd, p.CurrencyCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var riskItem in risks)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == riskItem.PrefixCd && item.BrachCd == riskItem.BrachCd && item.CurrencyCd == riskItem.CurrencyCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateRiskReserveEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        excelReports.Add(new AccountingClosingReportDTO()
                        {
                            BrachCd = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCd = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCd = Convert.ToString(Convert.ToInt32(ledgerEntryItem.AccountingNature)),
                            AccountNatureDescription = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            LocalAmountValue = ledgerEntryItem.LocalAmount.Value,
                            TotalAmount = ledgerEntryItem.Amount.Value
                        });
                    }
                }
            }

            #endregion RiskReserve

            #region IBNR Reserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_IBNR_MODULE)))
            {
                closures = IbnrClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> reservesByPrefixByBranch = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();
                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var reserveItem in reservesByPrefixByBranch)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == reserveItem.PrefixCd && item.BrachCd == reserveItem.BrachCd select item).ToList();
                    LedgerEntryDTO ledgerEntry = GenerateIBNRReserveEntry(AccountingClosingByPrefixByBranch, module, userId);
                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        excelReports.Add(new AccountingClosingReportDTO()
                        {
                            BrachCd = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCd = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCd = Convert.ToString(Convert.ToInt32(ledgerEntryItem.AccountingNature)),
                            AccountNatureDescription = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            LocalAmountValue = ledgerEntryItem.LocalAmount.Value,
                            TotalAmount = ledgerEntryItem.Amount.Value
                        });
                    }
                }
            }

            #endregion

            #region PrevisionReserve

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_RISK_PREVENTION_MODULE)))
            {
                closures = GetRiskPreventionReserveClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> previsions = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var previsionItem in previsions)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == previsionItem.PrefixCd && item.BrachCd == previsionItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GeneratePrevisionReserveEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        excelReports.Add(new AccountingClosingReportDTO()
                        {
                            BrachCd = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCd = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCd = Convert.ToString(Convert.ToInt32(ledgerEntryItem.AccountingNature)),
                            AccountNatureDescription = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            LocalAmountValue = ledgerEntryItem.LocalAmount.Value,
                            TotalAmount = ledgerEntryItem.Amount.Value
                        });
                    }
                }
            }

            #endregion

            #region CatastrophicRisk

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_CATASTROPHIC_RISK_RESERVE_MODULE)))
            {
                closures = GetCatastrophicRiskReserveClosureReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> catastrophics = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var catastrophicItem in catastrophics)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == catastrophicItem.PrefixCd && item.BrachCd == catastrophicItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateCatastrophicRiskEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        excelReports.Add(new AccountingClosingReportDTO()
                        {
                            BrachCd = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCd = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCd = Convert.ToString(Convert.ToInt32(ledgerEntryItem.AccountingNature)),
                            AccountNatureDescription = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            LocalAmountValue = ledgerEntryItem.LocalAmount.Value,
                            TotalAmount = ledgerEntryItem.Amount.Value
                        });
                    }
                }
            }

            #endregion

            #region ExpiredPremiums

            if (module == Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingClosingModule>(AccountingClosingModule.ACL_EXPIRED_PREMIUMS_MODULE)))
            {
                closures = ExpiredPremiumsReport();

                // Se agrupa por sucursal y ramo
                List<AccountingClosingReportDTO> premiums = closures.GroupBy(p => new { p.PrefixCd, p.BrachCd }).Select(g => g.First()).ToList();

                List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();

                foreach (var premiumItem in premiums)
                {
                    // Se filtra por ramo y sucursal
                    List<AccountingClosingReportDTO> AccountingClosingByPrefixByBranch = (from AccountingClosingReportDTO item in closures where item.PrefixCd == premiumItem.PrefixCd && item.BrachCd == premiumItem.BrachCd select item).ToList();

                    LedgerEntryDTO ledgerEntry = GenerateExpiredPremiumsEntry(AccountingClosingByPrefixByBranch, module, userId);

                    ledgerEntries.Add(ledgerEntry);
                }

                foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                    {
                        excelReports.Add(new AccountingClosingReportDTO()
                        {
                            BrachCd = ledgerEntry.Branch.Id,
                            BranchDescription = GetBranchDescriptionByBranchId(ledgerEntry.Branch.Id),
                            CurrencyCd = ledgerEntryItem.Amount.Currency.Id,
                            CurrencyDescription = GetCurrencyDescriptionByCurrencyId(ledgerEntryItem.Amount.Currency.Id),
                            AccountNatureCd = Convert.ToString(Convert.ToInt32(ledgerEntryItem.AccountingNature)),
                            AccountNatureDescription = (ledgerEntryItem.AccountingNature.Equals(AccountingNatures.Credit)) ? Resources.Resources.Credit.ToUpper() : Resources.Resources.Debit.ToUpper(),
                            AccountingAccountCd = Convert.ToString(DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Number),
                            AccountingAccountDescription = DelegateService.generalLedgerService.GetAccountingAccount(ledgerEntryItem.AccountingAccount.AccountingAccountId).Description,
                            LocalAmountValue = ledgerEntryItem.LocalAmount.Value,
                            TotalAmount = ledgerEntryItem.Amount.Value
                        });
                    }
                }
            }

            #endregion

            return excelReports;
        }
        #endregion

        /// <summary>
        /// GetExchangeDifferenceReportRecords
        /// </summary>
        /// <returns></returns>
        public List<ExchangeDifferenceReportDTO> GetExchangeDifferenceReportRecords()
        {
            ModuleDate module = new ModuleDate();
            module.Id = 9;
            module = DelegateService.tempCommonService.GetModuleDate(module);
            int accountingYear = module.LastClosingYyyy;

            List<ExchangeDifferenceReportDTO> exchangeDifferenceReportDTOs = GetExchangeDifferenceRecords();
            exchangeDifferenceReportDTOs = (from ExchangeDifferenceReportDTO item in exchangeDifferenceReportDTOs where item.AccountingYear == accountingYear && !item.Posted select item).ToList();

            return exchangeDifferenceReportDTOs;
        }


        #endregion Public Methods

        #region Private Methods

        #region Process

        /// <summary>
        /// SaveLogProcess
        /// Devuelve el id de proceso a ejecutarse de procesos masivos
        /// </summary>
        /// <param name="module"></param>
        /// <returns>int</returns>
        private int SaveLogProcess(int module)
        {
            var parameters = new NameValue[1];
            parameters[0] = new NameValue("module", module);
            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ACL.INSERT_LOG_ACCOUNTING_CLOSING", parameters);
            }

            var processId = Convert.ToInt32((from DataRow item in result.Rows select item[0].ToString()).FirstOrDefault());

            return processId;
        }

        #endregion Process

        #region Closing Date

        /// <summary>
        /// GetFirstAndLastDayOfMonth
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="lastClosingMonth"></param>
        /// <param name="lastClosingYear"></param>
        /// <returns>DateTime</returns>
        private DateTime GetFirstAndLastDayOfMonth(DateTime currentDate, int lastClosingMonth, int lastClosingYear)
        {
            DateTime operationDate = new DateTime();
            DateTime closeDate = new DateTime(lastClosingYear, lastClosingMonth, 15);
            closeDate = closeDate.AddMonths(1);
            DateTime closeFirstDay = closeDate.AddDays(-(closeDate.Day - 1));
            closeDate = closeDate.AddMonths(1);
            DateTime closeLastDay = closeDate.AddDays(-(closeDate.Day));

            // Si la fecha actual esta entre el mes del cierre, se retorna la fecha actual
            if (currentDate >= closeFirstDay && currentDate <= closeLastDay)
            {
                operationDate = currentDate;
            }
            else
            {
                // Si la fecha atual es menor a la fecha del primer día del mes del cierre
                if (currentDate < closeFirstDay)
                {
                    operationDate = closeFirstDay;
                }
                // Si la fecha atual es mayor a la fecha del último día del mes del cierre
                if (currentDate > closeLastDay)
                {
                    operationDate = closeLastDay;
                }
            }
            return operationDate;
        }

        #endregion Closing Date

        #region ExchangeDifference 


        /// <summary>
        /// DeleteUnpostedRecords
        /// </summary>
        private void DeleteUnpostedRecords(int accountingYear)
        {
            try
            {
                //Se obtienen todos los registros en el rango de fechas y que no estén contabilizados
                List<ExchangeDifferenceReportDTO> unpostedRecords = (from ExchangeDifferenceReportDTO item in GetExchangeDifferenceRecords() where !item.Posted && item.AccountingYear == accountingYear select item).ToList();

                if (unpostedRecords.Count > 0)
                {
                    foreach (ExchangeDifferenceReportDTO item in unpostedRecords)
                    {
                        DeleteExchangeDifferenceRecord(item.Id);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// ValidatePostedAccount
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        private bool ValidatePostedAccount(int accountingAccountId, int currencyId, int accountingYear)
        {
            bool valid = false;

            try
            {
                //Se obtienen todos los registros
                List<ExchangeDifferenceReportDTO> records = GetExchangeDifferenceRecords();

                //Se filtran los registros que han sido contabilizados.
                List<ExchangeDifferenceReportDTO> filteredRecords = (from ExchangeDifferenceReportDTO item in records where item.Posted && item.AccountingYear == accountingYear select item).ToList();

                //Se comprueba que la cuenta contable y moneda no se encuentren contabilizados en el rango de fechas indicado.
                var validate = (from ExchangeDifferenceReportDTO item in filteredRecords where item.AccountingAccountId == accountingAccountId && item.CurrencyId == currencyId select item).ToList();

                //Si no existen registros indica que la cuenta puede ser incluida para ser procesada
                valid = (validate.Count <= 0);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return valid;
        }

        #endregion ExchangeDifference 

        #region Base
        /// <summary>
        /// GetModuleDateDescriptionByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        private string GetModuleDateDescriptionByModuleId(int moduleId)
        {
            return DelegateService.tempCommonService.GetModuleDates().FirstOrDefault(i => i.Id == moduleId).Description;
        }
        /// <summary>
        /// GetBranchDescriptionByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>string</returns>
        private string GetBranchDescriptionByBranchId(int branchId)
        {
            return DelegateService.commonService.GetBranchById(branchId).Description;
        }

        /// <summary>
        /// GetBranchDefaultByUserId
        /// Obtiene la sucursal por defecto del usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        private int GetBranchDefaultByUserId(int userId)
        {
            return DelegateService.uniqueUserService.GetBranchesByUserId(userId).Where(br => br.IsDefault).ToList()[0].Id;
        }

        #endregion


        #region Currency

        /// <summary>
        /// GetCurrencyDescriptionbyCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public string GetCurrencyDescriptionByCurrencyId(int currencyId)
        {
            var currencies = DelegateService.commonService.GetCurrencies();
            var currency = currencies.Where(sl => sl.Id == currencyId).ToList();

            return currency[0].Description;
        }
        #endregion

        #endregion Private Methods

    }
}
