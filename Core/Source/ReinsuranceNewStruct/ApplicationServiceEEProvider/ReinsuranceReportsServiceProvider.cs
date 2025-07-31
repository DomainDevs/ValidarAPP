using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Application.ReinsuranceServices.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Business;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using GLDTO = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using REPMOD = Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Helper;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider
{
    public class ReinsuranceReportsServiceProvider : IReinsuranceReportsService
    {
        public string ClosureReport(int year, int month, int reportType, int userId)
        {
            try
            {
                var username = DelegateService.uniqueUserIntegrationService.GetUserByUserId(userId).AccountName;
                REPMOD.Report report = new REPMOD.Report();
                int formatId = 0;
                string storedProcedureName = "";
                List<REPMOD.Parameter> procedureParameters = new List<REPMOD.Parameter>();

                User user = new User()
                {
                    AccountName = username,
                    UserId = userId,
                };
                
                /*BORRADOR CUENTA TÉCNICA*/
                if (reportType == 10)
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_DRAFT_TECHNICAL_ACCOUNT));
                    report.Description = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.DESCRIPTION_DRAFT_TECHNICAL_ACCOUNT));
                    report.Name = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_DRAFT_TECHNICAL_ACCOUNT));
                    report.Filter = "";
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_DRAFT_TECHNICAL_ACCOUNT));
                }
                /*BORRADOR CUENTA CORRIENTE*/
                if (reportType == 11)
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_CURRENT_ACCOUNT));
                    report.Name = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_CURRENT_ACCOUNT));
                    report.Filter = "";
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_CURRENT_ACCOUNT));
                }
                /*CUENTA CORRIENTE*/
                if (reportType == 12)
                {
                    report.Description = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.DESCRIPTION_ACCOUNT));
                    report.Name = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_ACCOUNT));
                    report.Filter = "";
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_ACCOUNT));
                }
                
                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 1,
                    Description = "@MONTH",
                    IsFormula = false,
                    Value = month
                });

                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 2,
                    Description = "@YEAR",
                    IsFormula = false,
                    Value = year
                });

                report.StoredProcedure = new REPMOD.StoredProcedure()
                {
                    ProcedureName = storedProcedureName,
                    ProcedureParameters = procedureParameters
                };

                report.Parameters = null;
                report.UserId = user.UserId;
                report.ExportType = REPMOD.ExportTypes.PDF;
                report.IsAsync = false;

                report.Format = new Format()
                {
                    Id = formatId,
                    FileType = REPMOD.Formats.FileTypes.Excel
                };

                DelegateService.reportingService.GenerateFileByReport(report);
                
                return report.Name;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorClosureReport), ex);
            }
        }
        
        public decimal GetTotalRecordsMassiveReport(string dateFrom, string dateTo, int reportType)
        {
            try
            {
                decimal totalRecords = 0;
                REPMOD.Report report = new REPMOD.Report();
                string storedProcedureName = "";
                DateTime DateFrom = Convert.ToDateTime(dateFrom);
                List<REPMOD.Parameter> procedureParameters = new List<REPMOD.Parameter>();
                List<REPMOD.Parameter> parameters = new List<REPMOD.Parameter>();

                /*INCONSISTENCIA DE PRIMA*/
                if (reportType == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_INCONSISTENCY_PREMIUM)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_INCONSISTENCY_PREMIUM));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*BORDER AUX*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_BORDER_AUX)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_BORDER_AUX));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*CUENTA TÉCNICA*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_TECHNICAL_ACCOUNT)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_TECHNICAL_ACCOUNT));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = DateFrom.Year.ToString()
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = DateFrom.Month.ToString()
                    });
                }

                /*SINIESTROS*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_SINISTER)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_SINISTER));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*PAGOS*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_PAGER)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_PAGER));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*SLIPS FACULTATIVOS*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FACULTATIVE)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_FACULTATIVE));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*BORDER AUX SINIESTROS*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_BORDER)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_BORDER));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*BORDER AUX PAGOS*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_BORDER_PAGER)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_BORDER_PAGER));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*BORRADOR CUENTA TECNICA*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_ACCOUNT_TECNICAL)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_DRAFT_TECHNICAL_ACCOUNT));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = DateFrom.Year
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = DateFrom.Month
                    });
                }

                /*BORRADOR CUENTA CORRIENTE*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_CURRENT_ACCOUNT)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_CURRENT_ACCOUNT));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = DateFrom.Year.ToString()
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = DateFrom.Month.ToString()
                    });
                }

                /*CUENTA CORRIENTE*/
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_ACCOUNT)))
                {
                    storedProcedureName = Convert.ToString(EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_ACCOUNT));
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = DateFrom.Year.ToString()
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = DateFrom.Month.ToString()
                    });
                }

                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 3,
                    Description = "@MASSIVE_REPORT_ID",
                    IsFormula = false,
                    Value = 0
                });

                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 4,
                    Description = "@EXECUTE",
                    IsFormula = false,
                    Value = 0
                });

                report.StoredProcedure = new REPMOD.StoredProcedure()
                {
                    ProcedureName = storedProcedureName,
                    ProcedureParameters = procedureParameters
                };

                report.Parameters = null;
                report.IsAsync = false;
                report.ExportType = REPMOD.ExportTypes.Excel;
                totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                return totalRecords;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceReport), ex);
            }
        }

        public string ReinsuranceReports(string dateFrom, string dateTo, int reportType, int userId)
        {
            try
            {
                string fileName = "";
                DateTime DateFrom = Convert.ToDateTime(dateFrom);
                bool closureReport = false;
                bool reinsuredReport = false;
                string storedProcedureName = "";
                REPMOD.MassiveReport massiveReport = new REPMOD.MassiveReport();
                REPMOD.Report report = new REPMOD.Report();
                List<REPMOD.Parameter> procedureParameters = new List<REPMOD.Parameter>();

                #region INCONSISTENCIA DE PRIMA
                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_INCONSISTENCY_PREMIUM)))
                {
                    reinsuredReport = true;
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_INCONSISTENCY_PREMIUM).ToString();
                    massiveReport.Description = Resources.Resources.Inconsistency.ToUpper() + " " + Resources.Resources.Premium.ToUpper();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_INCONSISTENCY_PREMIUM).ToString();
                }

                #endregion

                #region BORDER AUX

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_BORDER_AUX)))
                {
                    reinsuredReport = true;
                    massiveReport.Description = Resources.Resources.BorderAux.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_BORDER_AUX).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_BORDER_AUX).ToString();
                }

                #endregion

                #region CUENTA TÉCNICA

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_TECHNICAL_ACCOUNT)))
                {
                    closureReport = true;
                    massiveReport.Description = Resources.Resources.AccountTechnical.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_TECHNICAL_ACCOUNT).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_TECHNICAL_ACCOUNT).ToString();
                }

                #endregion

                #region SINIESTROS

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_SINISTER)))
                {
                    reinsuredReport = true;
                    massiveReport.Description = Resources.Resources.Inconsistency.ToUpper() + " " + Resources.Resources.Claims.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_SINISTER).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_SINISTER).ToString();
                }

                #endregion

                #region PAGOS

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_PAGER)))
                {
                    reinsuredReport = true;
                    massiveReport.Description = Resources.Resources.Inconsistency.ToUpper() + " " + Resources.Resources.Payment.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_PAGER).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_PAGER).ToString();
                }

                #endregion

                #region SLIPS FACULTATIVO

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FACULTATIVE)))
                {
                    reinsuredReport = true;
                    massiveReport.Description = Resources.Resources.SlipsFacultative.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_FACULTATIVE).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_FACULTATIVE).ToString();
                }

                #endregion

                #region BORRADOR CUENTA TÉCNICA

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_ACCOUNT_TECNICAL)))
                {
                    closureReport = true;
                    massiveReport.Description = Resources.Resources.DraftTechnicalAccount.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_DRAFT_TECHNICAL_ACCOUNT).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_DRAFT_TECHNICAL_ACCOUNT).ToString();
                }

                #endregion

                #region BORDER AUX SINIESTRO

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue<ReportTypes>(ReportTypes.ID_BORDER)))
                {
                    reinsuredReport = true;
                    massiveReport.Description = Resources.Resources.BorderAuxClaims.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_BORDER).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_BORDER).ToString();
                }

                #endregion

                #region BORDER AUX PAGOS

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_BORDER_PAGER)))
                {
                    reinsuredReport = true;
                    massiveReport.Description = Resources.Resources.BorderAuxPager.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_PAGER).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_PAGER).ToString();
                }

                #endregion

                #region BORRADOR CUENTA CORRIENTE

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_CURRENT_ACCOUNT)))
                {
                    closureReport = true;
                    massiveReport.Description = Resources.Resources.DraftAccount.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_CURRENT_ACCOUNT).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_CURRENT_ACCOUNT).ToString();
                }

                #endregion

                #region CUENTA CORRIENTE

                if (reportType == Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.ID_ACCOUNT)))
                {
                    closureReport = true;
                    massiveReport.Description = Resources.Resources.CurrentAccount.ToUpper();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_NAME_ACCOUNT).ToString();
                    massiveReport.UrlFile = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_ACCOUNT).ToString();
                }

                #endregion


                if (reinsuredReport)
                {
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                }

                /*Reportes de Cierre*/
                if (closureReport)
                {
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 1,
                        Description = "YEAR",
                        IsFormula = false,
                        Value = DateFrom.Year
                    });
                    procedureParameters.Add(new REPMOD.Parameter
                    {
                        Id = 2,
                        Description = "MONTH",
                        IsFormula = false,
                        Value = DateFrom.Month
                    });
                }

                report.StoredProcedure = new REPMOD.StoredProcedure()
                {
                    ProcedureName = storedProcedureName,
                    ProcedureParameters = procedureParameters
                };

                report.Parameters = null;
                report.IsAsync = true;
                report.ExportType = REPMOD.ExportTypes.Excel;

                massiveReport.UserId = userId;
                massiveReport.EndDate = new DateTime(1900, 1, 1);
                massiveReport.GenerationDate = DateTime.Now;
                massiveReport.Id = 0;
                massiveReport.StartDate = DateTime.Now;
                massiveReport.Success = false;
                massiveReport.ModuleId = Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.MODULE_DATE_REINSURANCE));

                WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                fileName = massiveReport.UrlFile;
                return fileName;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceReport), ex);
            }
        }

        public string GenerateStructureReport(int processId, string reportTypeDescription, int exportFormatType, decimal recordsNumber, int userId)
        {
            try
            {
                REPMOD.Report report = new REPMOD.Report();
                string structureReports = "";
                int formatId = 0;
                string storedProcedureName = "";
                string exportedFileName = "";

                List<REPMOD.Parameter> procedureParameters = new List<REPMOD.Parameter>();
                report.Filter = "";
                
                /*INCONSISTENCIA DE PRIMAS*/
                if (reportTypeDescription == Resources.Resources.Inconsistency.ToUpper() + " " + Resources.Resources.Premium.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_INCONSISTENCY_PREMIUM));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_INCONSISTENCY_PREMIUM).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_INCONSISTENCY_PREMIUM).ToString();
                }
                /*BORDER AUX DE EMISION*/
                if (reportTypeDescription == Resources.Resources.BorderAux.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_BORDER_AUX));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_BORDER_AUX).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_BORDER_AUX).ToString();
                }
                /*CUENTA TÉCNICA*/
                if (reportTypeDescription == Resources.Resources.AccountTechnical.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_TECHNICAL_ACCOUNT));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_TECHNICAL_ACCOUNT).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_TECHNICAL_ACCOUNT).ToString();
                }

                /*SINIESTROS*/
                if (reportTypeDescription == (Resources.Resources.Inconsistency.ToUpper() + " " + Resources.Resources.Claims).ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_SINISTER));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_SINISTER).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_SINISTER).ToString();
                }

                /*PAGOS*/
                if (reportTypeDescription == (Resources.Resources.Inconsistency.ToUpper() + " " + Resources.Resources.Payment).ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_PAGER));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_PAGER).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_PAGER).ToString();
                }

                /*SLIPS FACULTATIVOS*/
                if (reportTypeDescription == Resources.Resources.SlipsFacultative.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_FACULTATIVE));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_FACULTATIVE).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_FACULTATIVE).ToString();
                }

                /*BORDER AUX SINIESTRO*/
                if (reportTypeDescription == Resources.Resources.BorderAuxClaims.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_BORDER));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_BORDER).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_BORDER).ToString();
                }

                /*BORDER AUX PAGOS*/
                if (reportTypeDescription == Resources.Resources.BorderAuxPager.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_BORDER_PAGER));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_BORDER_PAGER).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_BORDER_PAGER).ToString();
                }

                /*BORRADOR CUENTA TECNICA*/
                if (reportTypeDescription == Resources.Resources.DraftTechnicalAccount.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_DRAFT_TECHNICAL_ACCOUNT));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_DRAFT_TECHNICAL_ACCOUNT).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_DRAFT_TECHNICAL_ACCOUNT).ToString();
                }

                /*BORRADOR CUENTA CORRIENTE*/
                if (reportTypeDescription == Resources.Resources.DraftAccount.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_CURRENT_ACCOUNT));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_CURRENT_ACCOUNT).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_CURRENT_ACCOUNT).ToString();
                }

                /*CUENTA CORRIENTE*/
                if (reportTypeDescription == Resources.Resources.CurrentAccount.ToUpper())
                {
                    formatId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.ID_FORMAT_ACCOUNT));
                    report.Name = EnumHelper.GetEnumParameterValue(ReportTypes.TEMPLATE_NAME_ACCOUNT).ToString();
                    storedProcedureName = EnumHelper.GetEnumParameterValue(ReportTypes.PROCEDURE_GET_ACCOUNT).ToString();
                }

                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 1,
                    Description = "@MASSIVE_REPORT_ID",
                    IsFormula = false,
                    Value = processId
                });
                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 2,
                    Description = "@RECORD_COUNT",
                    IsFormula = false,
                    Value = recordsNumber
                });
                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 3,
                    Description = "@PAGE_SIZE",
                    IsFormula = false,
                    Value = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.PAGE_SIZE_REPORT))
                });
                procedureParameters.Add(new REPMOD.Parameter
                {
                    Id = 4,
                    Description = "@PAGE_NUMBER",
                    IsFormula = false,
                    Value = 1
                });

                report.StoredProcedure = new REPMOD.StoredProcedure()
                {
                    ProcedureName = storedProcedureName,
                    ProcedureParameters = procedureParameters
                };

                report.Parameters = null;

                report.UserId = userId;

                report.ExportType = REPMOD.ExportTypes.Excel;
                report.IsAsync = true;
                report.Description = Resources.Resources.GenerateDocument;

                report.Format = new Format()
                {
                    Id = formatId,
                    FileType = REPMOD.Formats.FileTypes.Excel
                };

                exportedFileName = report.Name;

                Format format = new Format();
                format.Id = formatId;
                format.FileType = REPMOD.Formats.FileTypes.Text;

                List<FormatDetail> formatDetails = new List<FormatDetail>();
                formatDetails = DelegateService.reportingService.GetFormatDetailsByFormat(format);
                if (formatDetails.Count > 0)
                {
                    WorkerFactory.Instance.CreateWorkerStructure(report, true);
                }
                else
                {
                    exportedFileName = "-1";
                }

                structureReports = exportedFileName;


                return structureReports;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceReport), ex);
            }
        }
        
        public List<ReportTypeDTO> GetReportTypes()
        {
            try
            {
                ReportTypeDAO reportTypeDAO = new ReportTypeDAO();
                return reportTypeDAO.GetReportTypes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReportTypes);
            }
        }

        public List<MassiveReportDTO> GetMassiveReportProcess(string reportName, int userId)
        {
            try
            {
                List<REPMOD.MassiveReport> massiveReports = new List<REPMOD.MassiveReport>();
                REPMOD.MassiveReport massiveReport = new REPMOD.MassiveReport();
                List<REPMOD.MassiveReport> massiveReportProcess = new List<REPMOD.MassiveReport>();
                massiveReport.UserId = userId;
                massiveReport.Description = reportName;
                massiveReport.ModuleId = Convert.ToInt16(EnumHelper.GetEnumParameterValue(ReportTypes.MODULE_DATE_REINSURANCE));

                massiveReports = DelegateService.reportingService.GetMassiveReports(massiveReport);
                List<REPMOD.MassiveReport> order = massiveReports.OrderByDescending(x => x.Id).ToList();

                if (massiveReports.Count > 0)
                {
                    double elapsed = 0;
                    double minElapsed = 0;
                    double progress = 0;

                    foreach (REPMOD.MassiveReport masiveReport in order)
                    {
                        if (Convert.ToDouble(masiveReport.RecordsProcessed) > 0)
                        {
                            progress = (Convert.ToDouble(masiveReport.RecordsProcessed)) / Convert.ToDouble(masiveReport.RecordsNumber);
                            if (progress < 1)
                            {
                                elapsed = System.Math.Round((DateTime.Now.TimeOfDay.TotalHours - masiveReport.StartDate.TimeOfDay.TotalHours), 2);
                                minElapsed = elapsed - System.Math.Truncate(elapsed);
                                minElapsed = minElapsed * 60;
                            }
                            else
                            {
                                elapsed = System.Math.Round((masiveReport.EndDate.TimeOfDay.TotalHours - masiveReport.StartDate.TimeOfDay.TotalHours), 2);
                                minElapsed = elapsed - System.Math.Truncate(elapsed);
                                minElapsed = minElapsed * 60;
                            }
                        }

                        massiveReportProcess.Add(new REPMOD.MassiveReport()
                        {
                            Order = "DESC",
                            Id = masiveReport.Id,
                            Description = masiveReport.Description,
                            RecordsNumber = masiveReport.RecordsNumber,
                            RecordsProcessed = masiveReport.RecordsProcessed,
                            Progress = progress.ToString("P", CultureInfo.InvariantCulture),
                            Elapsed = System.Math.Truncate(elapsed) + " h " + System.Math.Truncate(minElapsed) + " m",
                            UrlFile = masiveReport.UrlFile,
                            Success = masiveReport.Success
                        });
                    }
                }
                return massiveReportProcess.ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceReport), ex);
            }
        }

        public string RecordReinsuranceEntry(int processId, int userId)
        {
            try
            {
                ReinsuranceApplicationServiceProvider reinsuranceApplicationServiceProvider = new ReinsuranceApplicationServiceProvider();
                List<ReinsuranceAccountingParameterDTO> reinsuranceAccountingParameterDTOs = new List<ReinsuranceAccountingParameterDTO>();
                reinsuranceAccountingParameterDTOs = reinsuranceApplicationServiceProvider.GetReinsuranceAccountingParameters(processId);
                int entryNumber = 0;
                string message = "";

                if (EnumHelper.GetEnumParameterValue(ReportTypes.ENABLED_GENERAL_LEDGER).ToString() == "true")
                {
                    #region Parameters

                    int parameterId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.TRANSACTION_NUMBER));
                    int transactionNumber = Convert.ToInt32(DelegateService.commonIntegrationService.GetParameterByParameterId(parameterId).NumberParameter);

                    int moduleDateId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.REINSURANCE_MODULE));

                    // Método para obtener el listado de parámetros.

                    #endregion Parameters

                    if (reinsuranceAccountingParameterDTOs.Count > 0)
                    {
                        var headerDescription = Resources.Resources.ReinsuranceAccounting + " - " +
                                                Resources.Resources.Policy + ": " + Convert.ToString(reinsuranceAccountingParameterDTOs[0].PolicyDocumentNumber) + " - " +
                                                Resources.Resources.Endorsement + ": " + Convert.ToString(reinsuranceAccountingParameterDTOs[0].EndorsementDocumentNumber) + " - " +
                                                Resources.Resources.Branch + ": " + Convert.ToString(reinsuranceAccountingParameterDTOs[0].BranchDescription);

                        //Listado en donde se llevaran los grupos de parametros al servicio
                        List<List<ParameterDTO>> parametersCollection = new List<List<ParameterDTO>>();

                        #region JournalEntryHeader

                        GLDTO.JournalEntryDTO journalEntry = new GLDTO.JournalEntryDTO();

                        int accountingCompanyId = (from item in DelegateService.glAccountingApplicationService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

                        journalEntry.Id = 0;
                        journalEntry.AccountingCompany = new GLDTO.AccountingCompanyDTO { AccountingCompanyId = accountingCompanyId };
                        journalEntry.AccountingMovementType = new GLDTO.AccountingMovementTypeDTO() { AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.REINSURANCE_CLOSING)) };
                        journalEntry.ModuleDateId = moduleDateId;
                        journalEntry.Branch = new GLDTO.BranchDTO() { Id = reinsuranceAccountingParameterDTOs[0].BranchCd };
                        journalEntry.SalePoint = new GLDTO.SalePointDTO() { Id = 0 };
                        journalEntry.EntryNumber = 0;
                        journalEntry.TechnicalTransaction = transactionNumber;
                        journalEntry.Description = headerDescription;
                        journalEntry.AccountingDate = DelegateService.commonIntegrationService.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.REINSURANCE_MODULE)), DateTime.Now);
                        journalEntry.RegisterDate = DateTime.Now;
                        journalEntry.Status = 1; //activo
                        journalEntry.UserId = userId;

                        journalEntry.JournalEntryItems = new List<GLDTO.JournalEntryItemDTO>();

                        #endregion JournalEntryHeader

                        // Se arma los detalles
                        foreach (ReinsuranceAccountingParameterDTO item in reinsuranceAccountingParameterDTOs)
                        {
                            if (item.LocalAmount != 0)
                            {
                                // Se arma la estructura de parámetros para su evaluación.
                                List<ParameterDTO> parameters = new List<ParameterDTO>();

                                parameters.Add(new ParameterDTO() { Value = Convert.ToString(item.LocalAmount, CultureInfo.InvariantCulture) }); //amount
                                parameters.Add(new ParameterDTO() { Value = Convert.ToString(item.CompanyTypeId, CultureInfo.InvariantCulture) }); //company_type_cd
                                parameters.Add(new ParameterDTO() { Value = Convert.ToString(item.ContractTypeId, CultureInfo.InvariantCulture) }); //contract_type_cd
                                parameters.Add(new ParameterDTO() { Value = Convert.ToString(item.ConceptId, CultureInfo.InvariantCulture) }); //concept_cd

                                parametersCollection.Add(parameters);

                                //Detalle con parámetros fijos.
                                GLDTO.JournalEntryItemDTO journalEntryItem = new GLDTO.JournalEntryItemDTO();
                                journalEntryItem.AccountingAccount = new GLDTO.AccountingAccountDTO();
                                journalEntryItem.Amount = new GLDTO.AmountDTO();
                                journalEntryItem.Amount.Currency = new GLDTO.CurrencyDTO();
                                journalEntryItem.Amount.Currency.Id = item.CurrencyCd;
                                journalEntryItem.ExchangeRate = new GLDTO.ExchangeRateDTO() { SellAmount = item.ExchangeRate };
                                journalEntryItem.Analysis = new List<GLDTO.AnalysisDTO>();
                                journalEntryItem.ReconciliationMovementType = new GLDTO.ReconciliationMovementTypeDTO();
                                journalEntryItem.CostCenters = new List<GLDTO.CostCenterDTO>();
                                journalEntryItem.Currency = new GLDTO.CurrencyDTO() { Id = item.CurrencyCd };
                                journalEntryItem.Description = item.Description;
                                journalEntryItem.EntryType = new GLDTO.EntryTypeDTO();
                                journalEntryItem.Id = 0;
                                journalEntryItem.Individual = new GLDTO.IndividualDTO() { IndividualId = item.CompanyId };
                                journalEntryItem.PostDated = new List<GLDTO.PostDatedDTO>();
                                journalEntryItem.Receipt = new GLDTO.ReceiptDTO();
                                journalEntry.JournalEntryItems.Add(journalEntryItem);
                            }
                        }

                        entryNumber = DelegateService.glAccountingApplicationService.Accounting(moduleDateId, parametersCollection, journalEntry);
                    }
                    else
                    {
                        entryNumber = -3;
                    }

                    if (entryNumber > 0)
                    {
                        message = Resources.Resources.IntegrationSuccessMessage + " " + entryNumber;
                        // Se actualiza el parámetro de número de trasacción en comm.parameter
                        UpdateTransactionNumber(transactionNumber);
                    }
                    if (entryNumber == 0)
                    {
                        message = Resources.Resources.AccountingIntegrationUnbalanceEntry;
                    }
                    if (entryNumber == -2)
                    {
                        message = Resources.Resources.EntryRecordingError;
                    }
                    if (entryNumber == -3)
                    {
                        message = Resources.Resources.ParameterNotExist;
                    }
                }

                return message;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceReport), ex);
            }
        }

        private void UpdateTransactionNumber(int number)
        {
            Integration.CommonServices.DTOs.ParameterDTO parameter = new Integration.CommonServices.DTOs.ParameterDTO();
            parameter.Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReportTypes.JOURNAL_ENTRY_TRANSACTION_NUMBER)); //id de parametro de número de transacción de Asiento Diario
            parameter.NumberParameter = number;
            parameter.Description = Resources.Resources.TransactionNumber + " " + Resources.Resources.JournalEntry + ":";
            parameter.NumberParameter = parameter.NumberParameter + 1;
            DelegateService.commonIntegrationService.UpdateParameter(parameter);
        }
        
    }
}
